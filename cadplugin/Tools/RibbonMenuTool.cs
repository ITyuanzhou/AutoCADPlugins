using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Ribbon;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cadplugin.Tools
{
    class RibbonMenuTool
    {

        private const string TAB_TITLE = "CMCU CAD插件";
        private const string TAB_ID = "FSJ_Custom_Tab";
        private const string PANEL_TITLE = "自动拆图";


        public static void InitRibbonMenu()
        {
            Editor ed = null;
            Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (dwg != null) ed = dwg.Editor;

            try
            {
                RibbonControl rc = RibbonServices.RibbonPaletteSet.RibbonControl;
                RibbonTab rt = GetExistingRibbonTab(rc);
                if (rt == null)
                {
                    rt = RibbonTab();
                    RibbonPanel rp = RibbonPanel();
                    RibbonButton rb = RibbonButton("连续取点", "ModelessDialog ");

                    rp.Source.Items.Add(rb);
                    rt.Panels.Add(rp);
                    rc.Tabs.Add(rt);
                }
                if (ed != null)
                {
                    ed.WriteMessage("\n初始化CMCU自动拆图插件成功");
                }
            }
            catch (System.Exception ex)
            {
                if (ed != null)
                {
                    ed.WriteMessage("\n初始化CMCU自动拆图插件失败{0}.", ex.ToString());
                }
            }
          
        }


        public static RibbonTab RibbonTab()
        {
            RibbonTab ribTab = new RibbonTab();
            ribTab.Title = TAB_TITLE;
            ribTab.Id = TAB_ID;
            return ribTab;
        }


        public static RibbonTab GetExistingRibbonTab(RibbonControl rc)
        {
            //Find existing ribbon tab
            foreach (var t in rc.Tabs)
            {
                if (t.Title.ToUpper() == TAB_TITLE.ToUpper() &&
                    t.Id.ToUpper() == TAB_ID.ToUpper())
                {
                    return t;
                }
            }

            return null;
        }

        public static RibbonPanel RibbonPanel()
        {
            RibbonPanelSource ribSoucePanel = new RibbonPanelSource();
            ribSoucePanel.Title = PANEL_TITLE;
            RibbonPanel ribPanel = new RibbonPanel();
            ribPanel.Source = ribSoucePanel;
            return ribPanel;
        }


        public static RibbonButton RibbonButton(string btName, string cmdName)
        {
            RibbonButton ribButton = new RibbonButton();
            ribButton.Text = btName;
            ribButton.CommandParameter = cmdName;
            ribButton.ShowText = true;
            ribButton.CommandHandler = new AdskCommandHandler();

            return ribButton;
        }

       
        public class AdskCommandHandler : System.Windows.Input.ICommand
        {
            public event EventHandler CanExecuteChanged;


            bool ICommand.CanExecute(object parameter)
            {
                return true;
            }

            void ICommand.Execute(object parameter)
            {
                Autodesk.Windows.RibbonButton ribBtn = parameter as Autodesk.Windows.RibbonButton;
                if (ribBtn != null)
                    Application.DocumentManager.MdiActiveDocument.SendStringToExecute((string)ribBtn.CommandParameter, true, false, true);

                RibbonTextBox ribTxt = parameter as RibbonTextBox;
                if (ribTxt != null)
                    System.Windows.Forms.MessageBox.Show(ribTxt.TextValue);
            }
        }

    }

}
