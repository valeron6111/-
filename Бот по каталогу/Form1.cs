using System;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Бот_по_каталогу
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        WebClient webclient = new WebClient();

        IWebElement GetElement(By locator)
        {
            bool logic = false;
            do
            {
                try
                {
                    Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
                    List<IWebElement> elements = Browser.FindElements(locator).ToList();
                    if (elements.Count > 0) return elements[0];
                    else return null;
                }
                catch (WebDriverException)
                {
                    logic = false;
                }
            }
            while (logic == false);
            return null;
        }
        public void CreateCatalog(string nkatalog)
        {

            Browser.Navigate().GoToUrl("https://ok.ru/group/55240665399526/market");
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromHours(1);
            IWebElement createcatalogok = Browser.FindElement(By.ClassName("market-albums_new-lk")); createcatalogok.Click();
            IWebElement enternameok = Browser.FindElement(By.Name("st.layer.name"));enternameok.SendKeys(nkatalog);
            IWebElement saveok = Browser.FindElement(By.CssSelector("#hook_FormButton_button_save")); saveok.Click();
            Browser.Navigate().GoToUrl("https://vk.com/market-147979295");
            IWebElement createcatalogvk = Browser.FindElement(By.Id("market_add_album_btn")); createcatalogvk.Click();
            IWebElement enternamevk = Browser.FindElement(By.Id("market_album_edit_title")); enternamevk.SendKeys(nkatalog);
            IWebElement savevk = Browser.FindElement(By.XPath("/html/body[@class='layers_shown']/div[@id='box_layer_wrap']/div[@id='box_layer']/div[@class='popup_box_container']/div[@class='box_layout']/div[@class='box_controls_wrap']/div[@class='box_controls']/table[@class='fl_r']/tbody/tr/td[1]/button[@class='flat_button']")); savevk.Click();

        }
        

        public Form1()
        {
            InitializeComponent();
            int kolichestvotovarovvpost = 0;
            int countrazn = 0;
            string hashtag = null;
            string src = null;
            string[] artvar = null;
            string[] namevariant = null;
            string desk = null;
            const string campaign = "08";
            OpenQA.Selenium.Chrome.ChromeOptions co = new OpenQA.Selenium.Chrome.ChromeOptions();
            co.AddArgument(@"user-data-dir=C:\Users\homepc\AppData\Local\Google\Chrome\User Data");
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver(co); // 
            IWait<IWebDriver> wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            Browser.Manage().Window.Maximize();
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromHours(1);
            Browser.Navigate().GoToUrl("http://www.avon.ru/REPSuite/jsbrochure.page#page=0&campaign=" + campaign + "&year=18&index=01&zoom=0&type=core");
            IWebElement login = GetElement(By.Id("submitBtn")); if (login != null) login.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("jsbr_backnavbtn")));
            IWebElement back = Browser.FindElement(By.Id("jsbr_backnavbtn")); back.Click();
            System.Threading.Thread.Sleep(1500);
            string url = Browser.Url;
            int indexlast = url.IndexOf('&');
            int leng = indexlast - 49;
            string countpage = url.Substring(49, leng);
            int pagelast = int.Parse(countpage);
            for (int k = 0; k <= pagelast; k+=2)
            {
                Browser.Navigate().GoToUrl("http://www.avon.ru/REPSuite/jsbrochure.page#page="+k+"&campaign=" + campaign + "&year=18&index=01&zoom=0&type=core");
                System.Threading.Thread.Sleep(2000);
                Browser.Navigate().Refresh();
            Start:
                try
                {
                    System.Threading.Thread.Sleep(2000);
                    IWebElement zakazatprodsostr = Browser.FindElement(By.Id("jsbr_ver2_add")); zakazatprodsostr.Click();
                    IWebElement order = Browser.FindElement(By.CssSelector("#jsbr_ver2_to_order [target='_top']")); //order.Click();
                }
                catch (UnhandledAlertException)
                {
                    IAlert alert = Browser.SwitchTo().Alert();
                    alert.Accept();
                    IWebElement next = Browser.FindElement(By.Id("jsbr_fwdnav_container"));
                    next.Click();
                    goto Start;
                }
                IWebElement imagepost = Browser.FindElement(By.Id("jsbr_pageimage")); string srcpost = imagepost.GetAttribute("src"); 
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("jsBroDialogFrame"));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".js_bbo_lineno_val.cell .js_cell")));
                List<IWebElement> listart2 = Browser.FindElements(By.CssSelector(".js_bbo_lineno_val.cell .js_cell")).ToList();
                string[] articul = new string[listart2.Count];
                for (int j = 0; j < listart2.Count; j++)
                    {
                        articul[j] = listart2[j].Text;
                    }
                kolichestvotovarovvpost = listart2.Count;
                for (int i = 0; i < listart2.Count; i++)
                {
                    Browser.SwitchTo().DefaultContent();
                    Browser.Navigate().GoToUrl("https://my.avon.ru/poisk/rezultaty/?q=" + articul[i]);
                    IWebElement otsytarticul = Browser.FindElement(By.CssSelector("strong.ng-binding"));
                    int errornumber = int.Parse(otsytarticul.Text);
                    if (errornumber != 0)
                    {
                        IWebElement parsename = Browser.FindElement(By.CssSelector("a.ProductName.ng-binding"));
                        Name = parsename.Text;
                        parsename.Click();
                        IWebElement raznovinost = GetElement(By.CssSelector(".select2-chosen.ng-binding"));
                        if (raznovinost != null)
                        {
                            List<IWebElement> imageraznovid = Browser.FindElements(By.CssSelector(".VariantImageOverlay")).ToList();
                            countrazn = imageraznovid.Count;
                            for (int j = 0; j < imageraznovid.Count; j++)
                            {
                                src = imageraznovid[j].GetAttribute("scr");
                                webclient.DownloadFile(src, @"C:\avon\variants\" + articul[i] + ".jpg");
                                string pathToImage = @"C:\avon\variants\" + articul[i] + ".jpg";
                                Bitmap source = new Bitmap(pathToImage);
                                Bitmap result = new Bitmap(source, 400, 300);
                                namevariant[j] = imageraznovid[j].GetAttribute("alt");
                                List<IWebElement> articulvarianta = Browser.FindElements(By.ClassName("VariantLineNumber ng-binding ng-scope")).ToList();
                                artvar[j] = articulvarianta[j].Text;
                            }

                        }

                        IWebElement priceproduct = Browser.FindElement(By.CssSelector("#ProductDetails div div.Price.ng-binding"));
                        List<IWebElement> deskproduct = Browser.FindElements(By.CssSelector("#ProductDescription div div div div.ng-binding.ng-scope p")).ToList();
                        for (int j = 0; j < deskproduct.Count; j++)
                        {
                            desk += deskproduct[j].Text + "\r\n";
                        }
                        List<IWebElement> ht = Browser.FindElements(By.CssSelector("#Breadcrumbs .ng-binding")).ToList();
                        string kat = null;
                        int numcat = ht.Count - 1;
                        for (int j = 1; j < ht.Count; j++)
                        {
                            hashtag += "#" + ht[j].Text.Replace(" ", "") + " ";
                            if (j == numcat) kat = ht[j].Text;
                        }

                        string pr = priceproduct.Text.Replace(" руб.", "");
                        string price = pr.Replace(" ", "");
                        IWebElement yveliz = Browser.FindElement(By.CssSelector("span.EnlargeImageText")); yveliz.Click();
                        IWebElement image = Browser.FindElement(By.CssSelector("#ProductLightboxCarousel div div div div a div img"));
                        src = image.GetAttribute("src");
                        webclient.DownloadFile(src, @"c:\avon\" + articul + ".jpg");
                    //заполнение
                    CreateCatalog:
                        Browser.Navigate().GoToUrl("https://m.ok.ru/dk?st.cmd=createProduct&st.rtu=%2Fdk%3Fst.cmd%3DaltGroupMain%26st.groupId%3D55240665399526%26_prevCmd%3DaltGroupMain%26tkn%3D4595&st.groupId=55240665399526&_prevCmd=altGroupMain&tkn=2316");
                        System.Threading.Thread.Sleep(1000);
                        Browser.Navigate().Refresh();
                        IWebElement nameok = Browser.FindElement(By.CssSelector("#boxPage #content .input-text #field_name")); nameok.SendKeys(Name);
                        IWebElement priceok = Browser.FindElement(By.CssSelector("#idFormContentBeforePhoto div div input#field_price.input-text_element")); priceok.SendKeys(price.ToString());
                        IWebElement deskok = Browser.FindElement(By.CssSelector("#idFormContentBeforePhoto div div textarea#field_descr.textarea_element")); deskok.SendKeys(hashtag + "\r\n" + "\r\n" + desk);
                        IWebElement photo = Browser.FindElement(By.Id("upload-photo-files_field-id")); photo.SendKeys(@"C:\avon\" + articul + ".jpg");
                        System.Threading.Thread.Sleep(2000);
                        IWebElement katalog = Browser.FindElement(By.Id("field_selId")); SelectElement drop = new SelectElement(katalog);

                        try
                        {
                            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                            drop.SelectByText(kat);
                        }
                        catch (NoSuchElementException)
                        {
                            CreateCatalog(kat);
                            goto CreateCatalog;
                        }
                        Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromHours(1);

                        IWebElement dlit = Browser.FindElement(By.Id("field_lifeTm")); SelectElement drop2 = new SelectElement(dlit); drop2.SelectByValue("30");
                        IWebElement saveok = Browser.FindElement(By.CssSelector("input.base-button_target")); saveok.Click();
                        System.Threading.Thread.Sleep(2500);
                        Browser.Navigate().GoToUrl("https://vk.com/market-147979295");
                        IWebElement addvk = Browser.FindElement(By.Id("market_add_item_btn")); addvk.Click();
                        IWebElement namevk = Browser.FindElement(By.Id("item_name")); namevk.SendKeys(Name);
                        IWebElement imagevk = Browser.FindElement(By.Name("photo"));
                        IWebElement deskvk = Browser.FindElement(By.Id("item_description"));
                        if (raznovinost == null)
                        {
                            deskvk.SendKeys(hashtag + "\r\n\r\n" + desk);
                            imagevk.SendKeys(@"C:\avon\" + articul + ".jpg");
                            IWebElement acceptvkphoto = Browser.FindElement(By.Id("market_photo_crop_done"));
                            System.Threading.Thread.Sleep(2000);
                            acceptvkphoto.Click();
                        }
                        else
                        {
                            int n = 0;
                            do
                            {
                                deskvk.SendKeys(hashtag + "\r\n" + "Внимание! У данного товара есть разновидности:" + "\r\n" +
                                 "(" + (n + 2) + " фотография) - " + namevariant[i] + "(" + artvar[i] + ")\r\n" + desk);
                                imagevk.SendKeys(@"C:\avon\variants" + artvar[i] + ".jpg");
                                IWebElement acceptvkphoto = Browser.FindElement(By.Id("market_photo_crop_done"));
                                acceptvkphoto.Click();
                                n++;
                            } while (n < countrazn);
                        }
                        System.Threading.Thread.Sleep(2500);
                        IWebElement pricevk = Browser.FindElement(By.Id("item_price")); pricevk.SendKeys(price.ToString());
                        IWebElement katalogvk = Browser.FindElement(By.CssSelector("#container4 input.selector_input")); katalogvk.SendKeys(kat + OpenQA.Selenium.Keys.Enter);
                        IWebElement savevk = Browser.FindElement(By.XPath("/html/body[@class='layers_shown']/div[@id='box_layer_wrap']/div[@id='box_layer']/div[@class='popup_box_container']/div[@class='box_layout']/div[@class='box_controls_wrap']/div[@class='box_controls']/table[@class='fl_r']/tbody/tr/td[2]/button[@class='flat_button']")); savevk.Click();
                        System.Threading.Thread.Sleep(2500);

                    }
                }
                int m = 0;
                Browser.Navigate().GoToUrl("https://vk.com/rosuinka");
                IWebElement postvk = Browser.FindElement(By.Id("post_field"));postvk.SendKeys(hashtag);
                webclient.DownloadFile(srcpost,@"C:\avon\katalog\" + m + ".jpg");
                IWebElement addphotoiconvk = Browser.FindElement(By.CssSelector(".ms_item_photo")); addphotoiconvk.Click();
                IWebElement uploadkatalog = Browser.FindElement(By.Id("choose_photo_upload")); uploadkatalog.SendKeys(@"C:\avon\katalog\" + m + ".jpg");
                System.Threading.Thread.Sleep(3000);
                for (int i = 0; i < kolichestvotovarovvpost; i++)
                {
                    ((IJavaScriptExecutor)Browser).ExecuteScript("arguments[0].click();", Browser.FindElement(By.CssSelector("ms_item ms_item_market _type_market")));
                    IWebElement findtovar = Browser.FindElement(By.CssSelector("input#market_search_input")); findtovar.SendKeys(Name);
                    IWebElement accepttovar = Browser.FindElement(By.CssSelector("#market_subtab_pane_search .market_row_img")); accepttovar.Click();
                }
                Browser.Navigate().GoToUrl("https://ok.ru/group/55240665399526/market/search");
                string hrefok = null;
                IWebElement searchok = Browser.FindElement(By.CssSelector("label input"));
                for (int i = 0; i < kolichestvotovarovvpost; i++)
                {
                    searchok.SendKeys(Name);
                    IWebElement selecttovarok = Browser.FindElement(By.CssSelector(".market-list_lk"));
                    hrefok += selecttovarok.GetAttribute("href") + "\r\n";
                    searchok.Clear();
                }
                Browser.Navigate().GoToUrl("https://ok.ru/group/55240665399526");
                IWebElement lenta = Browser.FindElement(By.CssSelector(".mctc_navMenuSec[href='/group/55240665399526']"));lenta.Click();
                IWebElement postokclick = Browser.FindElement(By.ClassName("input_placeholder")); postokclick.Click();
                IWebElement postok = Browser.FindElement(By.CssSelector("[data-initial-text-to-modify='null']")); postok.SendKeys(hrefok + OpenQA.Selenium.Keys.Enter);System.Threading.Thread.Sleep(1000);postok.Clear();postok.SendKeys(hashtag);
                IWebElement addcatok = Browser.FindElement(By.CssSelector(".posting_ac_i.h-mod[title='Добавить фото']"));addcatok.Click();
                IWebElement upldkatok = Browser.FindElement(By.CssSelector(".dblock input[type='file'][name='photo']")); upldkatok.SendKeys(@"C:\avon\katalog\" + m + ".jpg");
                IWebElement poskokacc = Browser.FindElement(By.CssSelector(".posting_submit")); poskokacc.Click();
            }
            Browser.Quit();
        }

        
    }
}
