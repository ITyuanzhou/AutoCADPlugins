using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.Windows;
using cadplugin.constant;
using cadplugin.Tools;
using CmcuCadPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

[assembly: CommandClass(typeof(cadplugin.CommandMethod.MenuClass))]
namespace cadplugin.CommandMethod
{
    class MenuClass
    {
        private static LocalSetting localSetting;




        //[CommandMethod("AddMenu")]
        //public void AddMenu()
        //{

        //    PopMenuTool cm = new PopMenuTool(LocalPluginVersion);
        //    if (cm.AddMenu())   //如果CUI文件菜单中没有当前版本菜单信息，更新CUI文件
        //        cm.UpdateCUI();

        //}



        [CommandMethod("AddContextMenu")]
        public void AddContextMenu()
        {
            ContextMenuExtension ce = new ContextMenuExtension();
            ce.Title = "快捷菜单";
            Autodesk.AutoCAD.Windows.MenuItem mi1 = new Autodesk.AutoCAD.Windows.MenuItem("创建线");
            mi1.Click += new EventHandler(mi1_Click);

            Autodesk.AutoCAD.Windows.MenuItem mi2 = new Autodesk.AutoCAD.Windows.MenuItem("创建圆");
            mi2.Click += new EventHandler(mi2_Click);

            ce.MenuItems.Add(mi1);
            ce.MenuItems.Add(mi2);
            Autodesk.AutoCAD.ApplicationServices.Application.AddDefaultContextMenuExtension(ce);

        }

        void mi1_Click(object sender, EventArgs e)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("Line\n", true, false, true);
        }

        void mi2_Click(object sender, EventArgs e)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("Circle\n", true, false, true);
        }



        [CommandMethod("AddPalette")]
        public void AddPalette()
        {
            PaletteSet myPaletteSet = new PaletteSet();
            Autodesk.AutoCAD.Windows.PaletteSet ps = new Autodesk.AutoCAD.Windows.PaletteSet("PaletteSet");

           
            ps.Visible = true;
            ps.Style = PaletteSetStyles.ShowAutoHideButton;
            ps.Dock = DockSides.None;
            ps.MinimumSize = new System.Drawing.Size(200,100);
            ps.Add("PalettSet",myPaletteSet);
            ps.Visible = true;

        }


        //[CommandMethod("AddRibbon")]
        //public void AddRibbon()
        //{
        //    RibbonMenuTool.InitRibbonMenu();


        //}



        [CommandMethod("DeleteCMCUMenu")]
        public void DeleteCMCUMenu()
        {
            Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor aced = dwg.Editor;
            localSetting = LocalSetting.GetSetting();
            PopMenuTool pm = new PopMenuTool(localSetting.LocalPluginVersion);
            pm.RemoveHistoryMenu();
            pm.SaveCui();
            aced.WriteMessage("\n清除已添加的按钮");

        }

    }
}
