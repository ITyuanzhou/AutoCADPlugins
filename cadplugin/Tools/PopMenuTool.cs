using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.Geometry;
using System.IO;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
namespace CmcuCadPlugin
{
    
    public class PopMenuTool
    {
        
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        [DllImport("accore.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "acedCmd")]
        private static extern int acedCmd(System.IntPtr vlist);
        string localPluginVersion;
        CustomizationSection[] partialCuiFiles;  
        CustomizationSection mainCs;
        Workspace wsClassic;      
        public PopMenuTool(string arg){
            localPluginVersion = arg;
            string mainCuiFile = (string)Application.GetSystemVariable("MENUNAME");
            mainCuiFile += ".cuix";
            mainCs = new CustomizationSection(mainCuiFile);

            partialCuiFiles = new CustomizationSection[mainCs.PartialCuiFiles.Count];
            int index = 0;
            foreach (string fileName in mainCs.PartialCuiFiles)
            {
                if (File.Exists(fileName))
                {
                    partialCuiFiles[index++] = new CustomizationSection(fileName);

                }
            }
            foreach (Workspace w in mainCs.Workspaces)
            {
                if (w.ElementID.ToUpper().Equals("WS_ACADCLASSIC"))
                {
                    wsClassic = w;
                    break;
                }
            }
        }


        //判断目前是否含有当前版本的菜单信息
        public bool AddMenu()
        {
            bool rvalue = false;
            //判断是否有该插件的CUI信息 没有则创建       
            PopMenu exitTopMenu = GetCurrentTopPopMenu();
            if (exitTopMenu==null)
            {
                RemoveHistoryMenu();                    
                System.Collections.Specialized.StringCollection topAliases = new System.Collections.Specialized.StringCollection();
                topAliases.Add("CmcuPlugin"+ localPluginVersion);

                MacroGroup cmcuMacro = new MacroGroup("CmcuMacro"+localPluginVersion, mainCs.MenuGroup);
                MenuMacro mSplit = new MenuMacro(cmcuMacro, "连续取点", "MODElESSDIALOG", "");
                MenuMacro mLogin = new MenuMacro(cmcuMacro, "登录", "CMCULOGIN ", "");
                MenuMacro mExit = new MenuMacro(cmcuMacro, "退出", "CMCUEXIT ", "");
                MenuMacro mUpdate = new MenuMacro(cmcuMacro, "更新", "CMCUUPDATE ", "");

                PopMenu topMenu = new PopMenu("CMCU CAD"+localPluginVersion, topAliases, "CMCU CAD"+localPluginVersion, mainCs.MenuGroup);

                PopMenuItem menuSplit = new PopMenuItem(mSplit, "连续取点", topMenu, -1);
                PopMenuItem menuLogin = new PopMenuItem(mLogin, "登录", topMenu, -1);
                PopMenuItem menuExit = new PopMenuItem(mExit, "退出", topMenu, -1);
                PopMenuItem menuUpdate = new PopMenuItem(mUpdate, "更新", topMenu, -1);
                AddMenuToWorkspaces(topMenu);
                rvalue = true;
            }           
            return rvalue;
        }
        //取到当前PopMenu中Cmcu的菜单
        private PopMenu GetCurrentTopPopMenu() {
            PopMenu pm = null;
            foreach (PopMenu tpm in mainCs.MenuGroup.PopMenus)
            {
                AliasCollection acs = tpm.Aliases;
                foreach (var ac in acs)
                {
                    string acstr = ac as string;
                    if (!string.IsNullOrEmpty(acstr) && acstr.ToUpper().Equals("CMCUPLUGIN" + localPluginVersion))
                    {
                        pm = tpm; break;
                    }
                }
                if (pm != null)
                    break;
            }
            return pm;
        }
        //移除以前版本信息
        public void RemoveHistoryMenu()
        {
            try
            {
                PopMenu historyPopMenu = null;
                foreach (PopMenu tpm in mainCs.MenuGroup.PopMenus)
                {
                    AliasCollection acs = tpm.Aliases;
                    foreach (var ac in acs)
                    {
                        string acstr = ac as string;
                        if (!string.IsNullOrEmpty(acstr) && acstr.ToUpper().StartsWith("CMCUPLUGIN"))
                        {
                            historyPopMenu = tpm;
                        }
                        if (historyPopMenu != null)
                        {
                            foreach (Workspace wk in mainCs.Workspaces)
                            {
                                WorkspacePopMenu wkPm = wk.WorkspacePopMenus.FindWorkspacePopMenu(historyPopMenu.ElementID, historyPopMenu.Parent.Name);

                                if (wkPm != null)
                                    wk.WorkspacePopMenus.Remove(wkPm);
                            }

                            mainCs.MenuGroup.PopMenus.Remove(historyPopMenu);   // Deletes the Menu from ACAD Menu Group

                            MacroGroup deleteMacroGroup = null;
                            MacroGroupCollection mgc = mainCs.MenuGroup.MacroGroups;
                            List<int> indexList = new List<int>();
                            foreach (MacroGroup mg in mgc)
                            {
                                if (mg.Name.ToUpper().StartsWith("CMCUMACRO"))
                                    deleteMacroGroup = mg;
                                if (deleteMacroGroup != null)
                                {
                                    indexList.Add(mgc.IndexOf(deleteMacroGroup));
                                }
                            }
                            indexList.Reverse();
                            foreach (int index in indexList)
                            {
                                mainCs.MenuGroup.MacroGroups.Remove(index);
                            }
                            
                        }
                    }
                  
                }               
                
                
            }
            catch (System.Exception e)
            {
                ed.WriteMessage("\n异常:" + e.Message);

            }
        }
        //加入工作区
        private void AddMenuToWorkspaces(PopMenu popMenu)
        {
            try
            {
                foreach (Workspace wk in mainCs.Workspaces)
                {
                    WorkspacePopMenu wkpm = new WorkspacePopMenu(wk, popMenu);
                    wkpm.Display = 1;
                }
            }
            catch 
            {
               
            }
        }
        //更新CUI文件
        public void UpdateCUI()
        {
            if (mainCs.IsModified)
                mainCs.Save();

            for (int i = 0; i < partialCuiFiles.Count(); i++)
            {
                CustomizationSection tempcs = partialCuiFiles[i];
                if (tempcs!=null&& tempcs.IsModified)
                    tempcs.Save();
            }
            string mainCuiFileName = mainCs.CUIFileName;
            Application.LoadMainMenu(mainCuiFileName);


        }

        public void SaveCui()
        {
            if (mainCs.IsModified)
                mainCs.Save();

            for (int i = 0; i < partialCuiFiles.Count(); i++)
            {
                CustomizationSection tempcs = partialCuiFiles[i];
                if (tempcs != null && tempcs.IsModified)
                    tempcs.Save();
            }
        }

    }
}
