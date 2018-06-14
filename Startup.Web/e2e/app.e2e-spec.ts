import { Startup.WebPage } from './app.po';

describe('startup.web App', () => {
  let page: Startup.WebPage;

  beforeEach(() => {
    page = new Startup.WebPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
