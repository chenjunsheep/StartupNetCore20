using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Shared.Api.Schedule;
using Shared.Api.Swagger;
using Startp.Server.Services;
using Startup.Server.Settings;
using Swashbuckle.AspNetCore.Swagger;

namespace Startup.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            BuildConfigSettings(services);
            BuildServices(services);
            BuildSwaggerSettings(services);
            BuildSechdules(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            applicationLifetime.ApplicationStarted.Register(() => OnStartup(env));

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" },
                RequestPath = new PathString(string.Empty)
            });

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            BuildSwaggerUI(app);

            app.UseMvc();
        }

        #region Private Method

        private void OnStartup(IHostingEnvironment env)
        {
            var path = env.ContentRootPath;
            var contentRootDirectoryInfo = new DirectoryInfo(path);
            try
            {
                var pathRoot = $@"{contentRootDirectoryInfo.Parent.Parent.FullName}";
                if (env.IsDevelopment())
                {
                    var pathCodegen = Path.Combine(pathRoot, @"codegen\setup.bat");

                    if (File.Exists(pathCodegen))
                    {
                        //ONLY when Swagger Codegen is enabled, the process could be executable
                        ExcuteProcess(pathCodegen);
                    }
                }
                else if (env.IsProduction())
                {
                    var pathSchedule = Path.Combine(pathRoot, @"schedule\process.bat");
                    if (File.Exists(pathSchedule))
                    {
                        ExcuteProcess(pathSchedule);
                    }
                    var pathCleanup = Path.Combine(pathRoot, @"mypublish\cleanup.bat");

                    if (File.Exists(pathCleanup))
                    {
                        ExcuteProcess(pathCleanup);
                    }
                }
            }
            catch (Exception) { }
        }

        private void ExcuteProcess(string path)
        {
            try
            {
                var proc = new Process();
                proc.StartInfo.FileName = path;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception)
            {
                //skip
            }
        }

        private void BuildConfigSettings(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            var setting = new AppSettings();
            Configuration.Bind(setting);
            AppSetting.Instance = setting.AppSetting;
        }

        private void BuildServices(IServiceCollection services)
        {
            //customized services
            //services.AddScoped<IRepository, Repository>();
        }

        private void BuildSwaggerSettings(IServiceCollection services)
        {
            //swagger settings
            var rootPath = PlatformServices.Default.Application.ApplicationBasePath;
            SwaggerManager.Instance.Register(Path.Combine(rootPath, "Shared.Util.dll")); //register customized models to swagger codegen
            services.UseSwaggerDefault(Path.Combine(rootPath, "Startup.Server.xml"));
            services.AddSwaggerGen(c => { c.SwaggerDoc(ApiVersion.V1, new Info { Title = $"{ApiVersion.V1} API version", Version = ApiVersion.V1 }); });
        }

        private void BuildSwaggerUI(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.UseSwaggerUIDefault();
                c.SwaggerEndpoint($"/swagger/{ApiVersion.V1}/swagger.json", $"{ApiVersion.V1} APIs");
            });
        }

        private void BuildSechdules(IServiceCollection services)
        {
            services.AddSingleton<IScheduledTask>(new TaskHeartbeats(AppSetting.Instance.Schedules.Heartbeats.TargetServer, AppSetting.Instance.Schedules.Heartbeats.Frequency));
            services.AddScheduler((sender, args) =>
            {
                try
                {
                    //handle exceptions for all schedules
                    LogProvider.Log($"schedule exception: [{args.Exception.Message}]");
                }
                catch (Exception ex)
                {
                    LogProvider.Log($"schedule inner exception: [{ex.Message}]");
                }
                finally
                {
                    args.SetObserved();
                }
            });
        }

        #endregion
    }
}
