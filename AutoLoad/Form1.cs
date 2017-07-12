using cadplugin.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoLoad
{
    public partial class FormAutoLoad : Form
    {
        public FormAutoLoad()
        {
            InitializeComponent();
        }


        private void buttonBrowse_Click(object sender, EventArgs e)
        {

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxFilePath.Text = this.openFileDialog1.FileName;
                this.textBoxApp.Text = System.IO.Path.GetFileNameWithoutExtension(this.textBoxFilePath.Text);
                this.textBoxAppDesc.Text = this.textBoxApp.Text;
            }
        }

        private void FormAutoLoad_Load(object sender, EventArgs e)
        {
            List<string> versionsList = RegeditTool.GetHasInstallCADVersion();
            foreach (string version in versionsList)
            {
                this.checkedListBoxAutoVersions.Items.Add(version);
            }

            List<RegeditModel> regeditModelList = RegeditTool.GetAutoLoadDllsInfo();
            if (regeditModelList.Count == 0)
            {
                //
            }
            else
            {
                foreach (RegeditModel regeditModel in regeditModelList)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = regeditModel.Version;
                    lvi.SubItems.Add(regeditModel.DllName);
                    lvi.SubItems.Add(regeditModel.DllPath);
                    this.listViewAssembly.Items.Add(lvi);
                }
            }
           
            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.openFileDialog1.FileName))
            {
                MessageBox.Show("你还未选择你要自动加载的DLL应用程序！！！");
            }
            else if (GetCheckedVersionNumber() == 0)
            {
                MessageBox.Show("你还未选择AutoCAD的版本！！！");
            }
            else
            {
                string appName = this.textBoxApp.Text;
                string appDesc = this.textBoxAppDesc.Text;
                string appPath = this.textBoxFilePath.Text;
                string versions = "";
                try
                {
                    for (int i = 0; i < this.checkedListBoxAutoVersions.Items.Count; i++)
                    {
                        if (this.checkedListBoxAutoVersions.GetItemChecked(i))
                        {
                            RegeditTool.AutoCADAutoLoadDLL(
                                RegeditTool.GetVersionSubKeyByCADVersion(this.checkedListBoxAutoVersions.GetItemText(this.checkedListBoxAutoVersions.Items[i]))
                                ,"CMCUCAD_"+appName, appDesc, appPath);

                            versions += this.checkedListBoxAutoVersions.GetItemText(this.checkedListBoxAutoVersions.Items[i]) + " ";
                        }

                    }
                    MessageBox.Show("已成功将 " + appName + " DLL应用程序添加到" + versions + "版本的AutoCAD的注册表中去！");
                    //刷新
                    this.listViewAssembly.Items.Clear();
                    List<RegeditModel> regeditModelList = RegeditTool.GetAutoLoadDllsInfo();
                    if (regeditModelList.Count == 0)
                    {
                        //
                    }
                    else
                    {
                        foreach (RegeditModel regeditModel in regeditModelList)
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = regeditModel.Version;
                            lvi.SubItems.Add(regeditModel.DllName);
                            lvi.SubItems.Add(regeditModel.DllPath);
                            this.listViewAssembly.Items.Add(lvi);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("添加" + appName + "DLL应用程序到" + versions + "AutoCAD注册表中失败！" + ex.ToString());
                }
            }
            

           
            
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

            string dllNames = "";
            foreach (ListViewItem item in this.listViewAssembly.Items)
            {
                if (item.Checked)
                {
                    dllNames += item.Text + " " + item.SubItems[1].Text + "\n";
                }
            }
            if (String.IsNullOrEmpty(dllNames))
            {
                MessageBox.Show("你还未选择要删除的DLL应用程序！！！");
            }
            else
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要删除以下DLL吗?\n" + dllNames, "删除注册表", messButton);
                if (dr == DialogResult.OK)
                {
                    bool flag = true;
                    string errorInfo = "";
                    foreach (ListViewItem item in this.listViewAssembly.Items)
                    {
                        if (item.Checked)
                        {
                            int count = RegeditTool.UninstallRegedit(RegeditTool.GetVersionSubKeyByCADVersion(item.Text), item.SubItems[1].Text);
                            if (count != 1)
                            {
                                flag = false;
                                errorInfo += "删除"+ item.Text + item.SubItems[1].Text +"失败！\n";
                            }
                        }
                    }
                    if (!flag)
                    {
                        MessageBox.Show(errorInfo, "警告");
                    }
                    else
                    {
                        MessageBox.Show("删除以下DLL应用程序成功：\n"+dllNames, "成功");
                        //刷新
                        this.listViewAssembly.Items.Clear();
                        List<RegeditModel> regeditModelList = RegeditTool.GetAutoLoadDllsInfo();
                        if (regeditModelList.Count == 0)
                        {
                            //
                        }
                        else
                        {
                            foreach (RegeditModel regeditModel in regeditModelList)
                            {
                                ListViewItem lvi = new ListViewItem();
                                lvi.Text = regeditModel.Version;
                                lvi.SubItems.Add(regeditModel.DllName);
                                lvi.SubItems.Add(regeditModel.DllPath);
                                this.listViewAssembly.Items.Add(lvi);
                            }
                        }
                    }

                }
                else
                {

                }
            }
          



        }

        private int GetCheckedVersionNumber()
        {
            int number = 0;
            for (int i = 0; i < this.checkedListBoxAutoVersions.Items.Count; i++)
            {
                if (this.checkedListBoxAutoVersions.GetItemChecked(i))
                    number += 1;
            }
            return number;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
