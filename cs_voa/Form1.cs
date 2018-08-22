using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace cs_voa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string pBuffer = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.75 Mobile Safari/537.36";
            Util.UrlMkSetSessionOption(0x10000001, pBuffer, pBuffer.Length, 0);
            webBrowser1.ScriptErrorsSuppressed = true;
        }




        //刷新左侧树
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            string cont = Util.get_homepage();
            HtmlNode side_nav = Util.get_column(cont);
            //MessageBox.Show(side_nav.InnerHtml);
            HtmlNodeCollection sub_ul = side_nav.SelectNodes("ul");
            HtmlNodeCollection sub_root = side_nav.SelectNodes("div");
            for (int i = 0; i < sub_root.Count-1; i++) {
                TreeNode anode = new TreeNode(sub_root[i].InnerText);
                anode.Tag = sub_root[i].SelectSingleNode("a").GetAttributeValue("href", "/");
                //MessageBox.Show(sub_ul[i].InnerHtml);
                HtmlNodeCollection sub_ul_li = sub_ul[i].SelectNodes("li");


                for (int j = 0; sub_ul_li != null && j < sub_ul_li.Count; j++) {
                    TreeNode in_node = new TreeNode(sub_ul_li[j].InnerText);
                    in_node.Tag = sub_ul_li[j].SelectSingleNode("a").GetAttributeValue("href", "/");
                    anode.Nodes.Add(in_node);
                }


                //MessageBox.Show(sub_root[i].OuterHtml);
                //
                treeView1.Nodes.Add(anode);
            }
        }


        //选中节点
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode sel_node = treeView1.SelectedNode;
            if (sel_node == null) {
                return;
            }
            string url = (string)sel_node.Tag;
            Console.WriteLine("tag is;" + sel_node.Tag);
            if (url.EndsWith("/")) {
                return;
            }
            if (url.StartsWith("/"))
                url = Util.url_root + url;
            string cont=Util.get_content(url);
            if (cont == null || cont.Equals(""))
                return;
            HtmlNode node_list = Util.get_list(cont);
            if (node_list == null)
                return;
            //MessageBox.Show(node_list.InnerHtml);
            //Console.WriteLine(node_list.InnerHtml);

            HtmlNodeCollection sub_list = node_list.SelectNodes("li");   //.ChildNodes;


            Console.WriteLine(sub_list.Count + "'s child");
            listView1.Items.Clear();
            for (int i = 0; sub_list != null && i < sub_list.Count; i++) {
                string title = sub_list[i].InnerText;
                string art_url = sub_list[i].SelectSingleNode("a").GetAttributeValue("href", "");

                //MessageBox.Show(title);

                ListViewItem lvi = new ListViewItem(title);
                lvi.SubItems.Add(art_url);
                listView1.Items.Add(lvi);
            }

            //MessageBox.Show(sel_node.Text + "\n" + sel_node.Tag);

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem lvi = listView1.SelectedItems[0];
            string url = lvi.SubItems[1].Text;
            if (url.StartsWith("/")) {
                url = Util.url_root + url;
            }
            string cont = Util.get_content(url);
            if (cont == null || cont.Equals(""))
                return;
            richTextBox1.Text = Util.getContent(cont);

            string mp3 = Util.getMp3(cont);
            Console.WriteLine("mp3-1:" + mp3);
            //mp3 = "http://play.51voa.com/87abb69e2c9c96a50bcc19e14420ce87/5b7d039d/201808/terrorist-designation-qassim-abdullah-ali-ahmed.mp3";
            mp3 = Util.getRealMp3(mp3, url);
            Console.WriteLine("mp3-2:" + mp3);
            axWindowsMediaPlayer1.URL = mp3;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string word = richTextBox1.SelectedText.Trim();
            if (word == "") return;
            //MessageBox.Show(word);

            //webBrowser1.Navigate(Util.getTranslate(word));
            webBrowser1.Navigate("http://m.youdao.com/dict?le=eng&q=" + word + "#ec_contentWrp");
        }
    }
}
