using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace cs_voa
{
    class Util
    {
        public static string url_root = "http://www.51voa.com";
        public static string url_mp3root = "http://media.51voa.com/play";
        public static WebClient wb = new WebClient();

        //获取指定网页内容
        public static string get_content(string url) {
            try
            {
                return wb.DownloadString(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        //获取首页内容
        public static string get_homepage() {
            return get_content(url_root);
        }

        //从网页内容里拿到左边栏
        public static HtmlNode get_column(string content)
        {
            if (content == null || content.Equals("")) { 
                return null;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='left_nav']");
            try {
                return nodes[0];
            }
            catch (Exception e) {
                return null;
            }
        }

        //从网页内容里拿到右边list
        public static HtmlNode get_list(string content)
        {
            if (content == null || content.Equals(""))
            {
                return null;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='List']");
            try
            {
                return nodes[0].FirstChild;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //拿到正文
        public static string getContent(string cont) {
            if (cont == null || cont.Equals(""))
            {
                return null;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(cont);
            HtmlNode  node = doc.DocumentNode.SelectSingleNode("//div[@class='Content']");
            try
            {
                return node.InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("no content found!");
                return "";
            }
        }

        public static string getMp3(string cont)
        {
            if (cont == null || cont.Equals(""))
            {
                return null;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(cont);
            //HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='playbar']");
            //可以直接拿到mp3 url了
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//a[@id='mp3']");

            try
            {
                /*string url = node.InnerText.Trim();
                url = url.Replace("Player(\"", "").Replace("\");","");
                if (url.StartsWith("/")) {
                    url = Util.url_mp3root + url;
                }*/
                return node.Attributes["href"].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }


        //废弃
        public static string getRealMp3(string url,string refe) {
            Console.WriteLine("ref:" + refe);
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.Referer = refe;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webRequest.Address.ToString();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        //修改ua
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        public static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);
    }
}
