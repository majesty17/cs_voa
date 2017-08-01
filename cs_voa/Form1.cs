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
        }




        //
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            if (url.StartsWith("/"))
                url = Util.url_root + url;
            string cont=Util.get_content(url);
            if (cont == null || cont.Equals(""))
                return;
            HtmlNode node_list = Util.get_list(cont);
            if (node_list == null)
                return;
            //MessageBox.Show(node_list.InnerHtml);

            HtmlNodeCollection sub_list = node_list.SelectNodes("li");
            listView1.Items.Clear();
            for (int i = 0; sub_list != null && i < sub_list.Count; i++) {
                string title = sub_list[i].InnerText;
                string art_url = sub_list[i].SelectSingleNode("a").GetAttributeValue("href", "");

                MessageBox.Show(title);

                ListViewItem lvi = new ListViewItem(title);
                lvi.SubItems.Add("haha");
                listView1.Items.Add(lvi);
            }

            //MessageBox.Show(sel_node.Text + "\n" + sel_node.Tag);

        }
    }
}
