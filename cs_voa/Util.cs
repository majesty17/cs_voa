using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace cs_voa
{
    class Util
    {
        public static string url_root = "http://www.51voa.com";
        public static WebClient wb = new WebClient();

        //获取指定网页内容
        public static string get_content(string url) {
            return wb.DownloadString(url);
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
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='list']");
            try
            {
                return nodes[0];
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}
