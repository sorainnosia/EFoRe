using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Efore
{
    public partial class frmMain : Form
    {
        FolderManager folderManager = new FolderManager();
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            txtFolderPath.Text = folderBrowser.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFolderPath.Text))
            {
                MessageBox.Show("Please input a folder to scan");
                return;
            }
            if (Directory.Exists(txtFolderPath.Text) == false)
            {
                MessageBox.Show("The directory you wish to scan is not exist");
                return;
            }

            ScanFolder(txtFolderPath.Text, false);
        }

        private void ScanFolder(string txtFolderPath, bool silenceMode)
        {
            List<string> emptyFolders = folderManager.ScanEmptyFolder(txtFolderPath);
            if ((emptyFolders == null || emptyFolders.Count == 0) && silenceMode == false)
            {
                MessageBox.Show("There is no empty folder in the path you select");
                return;
            }
            foreach (string str in emptyFolders)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = str;
                listView1.Items.Add(lvi);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> selected = new List<string>();
            foreach(ListViewItem lvi in listView1.Items)
            {
                if (lvi.Checked)
                {
                    selected.Insert(0, lvi.Text);
                }                
            }
            if (selected == null || selected.Count == 0)
            {
                MessageBox.Show("There is no item in your selection");
                return;
            }
            int count = folderManager.DeleteEmptyFolder(selected);
            MessageBox.Show(count.ToString() + " items deleted");
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (selected.Contains(listView1.Items[i].Text))
                {
                    listView1.Items.RemoveAt(i);
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.Checked = true;
            }
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.Checked = false;
            }
        }
    }
}
