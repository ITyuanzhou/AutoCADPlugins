using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using CmcuCadPlugin;
using cadplugin.Tools;
using cadplugin.constant;

[assembly: ExtensionApplication(typeof(cadplugin.PluginApp))]
namespace cadplugin
{
    public class PluginApp : IExtensionApplication
    {

        private static LocalSetting localSetting;

        public void Initialize()
        {
            Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor aced = dwg.Editor;
            aced.WriteMessage("\n 开始初始化AutoCAD CMCU插件:");

            try
            {
                localSetting = LocalSetting.GetSetting();
                //初始化PopMenu
                PopMenuTool pm = new PopMenuTool(localSetting.LocalPluginVersion);
                if (pm.AddMenu())
                    pm.UpdateCUI();

                //初始化RibbonMenu
                Autodesk.AutoCAD.Ribbon.RibbonServices.RibbonPaletteSetCreated += new EventHandler(RibbonServices_RibbonPaletteSetCreated);
            }
            catch (System.Exception ex)
            {
                aced.WriteMessage("\n 初始化AutoCAD CMCU插件失败");
                aced.WriteMessage("\n{0}", ex.ToString());
            }
        }


        private void RibbonServices_RibbonPaletteSetCreated(object sender, EventArgs e)
        {
            Autodesk.AutoCAD.Ribbon.RibbonServices.RibbonPaletteSet.Load += new Autodesk.AutoCAD.Windows.PalettePersistEventHandler(RibbonPaletteSet_Loaded);
        }
        private void RibbonPaletteSet_Loaded(object sender, EventArgs e)
        {
            RibbonMenuTool.InitRibbonMenu();
        }


        public void Terminate()
        {
        }
    }
}
