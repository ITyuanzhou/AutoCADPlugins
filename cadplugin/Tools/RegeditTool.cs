using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using cadplugin.constant;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cadplugin.Tools
{
    public class RegeditTool
    {
        /// <summary>
        /// 得到电脑当前已经安装的CAD版本号
        /// </summary>
        /// <returns></returns>
        private static List<string> GetHasInstallCADVersion()
        {
            List<string> versionList = new List<string>();
            Autodesk.AutoCAD.Runtime.RegistryKey locaIMachine = Autodesk.AutoCAD.Runtime.Registry.LocalMachine;
            Autodesk.AutoCAD.Runtime.RegistryKey applications = locaIMachine.OpenSubKey("SOFTWARE\\Autodesk\\AutoCAD\\", true);
            string[] subKeys = applications.GetSubKeyNames();
            foreach (string subKey in subKeys)
            {
                string version = "";
                switch (subKey)
                {
                    case "R17.1":
                        version = "2008";
                        break;

                    case "R17.2":
                        version = "2009";
                        break;

                    case "R18.0":
                        version = "2010";
                        break;

                    case "R18.1":
                        version = "2011";
                        break;

                    case "R18.2":
                        version = "2012";
                        break;

                    case "R19.0":
                        version = "2013";
                        break;

                    case "R19.1":
                        version = "2014";
                        break;

                    case "R20.0":
                        version = "2015";
                        break;

                    case "R20.1":
                        version = "2016";
                        break;

                    default:
                        version = "";
                        break;

                }
                versionList.Add(version);
            }
            return versionList;
        }

        /// <summary>
        /// 得到当前ApiVersion的ApplicationsPath
        /// </summary>
        /// <param name="apiVersion">2016</param>
        /// <returns></returns>
        private static string GetApplicationsPath(string apiVersion)
        {
            string versionInfoR;   //代表版本信息 如：R18.0
            string versionInfoN;   //代表版本信息 如：B001

            switch (apiVersion)
            {
                case "2008":
                    versionInfoR = "R17.1";
                    versionInfoN = "6001";
                    break;

                case "2009":
                    versionInfoR = "R17.2";
                    versionInfoN = "7001";
                    break;

                case "2010":
                    versionInfoR = "R18.0";
                    versionInfoN = "8001";
                    break;

                case "2011":
                    versionInfoR = "R18.1";
                    versionInfoN = "9001";
                    break;

                case "2012":
                    versionInfoR = "R18.2";
                    versionInfoN = "A001";
                    break;

                case "2013":
                    versionInfoR = "R19.0";
                    versionInfoN = "B001";
                    break;

                case "2014":
                    versionInfoR = "R19.1";
                    versionInfoN = "D001";
                    break;

                case "2015":
                    versionInfoR = "R20.0";
                    versionInfoN = "E001";
                    break;

                case "2016":
                    versionInfoR = "R20.1";
                    versionInfoN = "F001";
                    break;

                default:
                    versionInfoR = "";
                    versionInfoN = "";
                    break;
            }

            string appPath = "SOFTWARE\\Autodesk\\AutoCAD\\" + versionInfoR + "\\ACAD-"+ versionInfoN + ":";
            Autodesk.AutoCAD.Runtime.RegistryKey LocalMachine = Autodesk.AutoCAD.Runtime.Registry.LocalMachine;
            Autodesk.AutoCAD.Runtime.RegistryKey Applications = LocalMachine.OpenSubKey(appPath + "804", true);
            if (Applications != null)
                appPath += "804\\Applications";    //简体中文版
            else
                appPath += "409\\Applications";    //英文版
            return appPath;
        }

        /// <summary>
        /// 通过添加注册表的方式让AutoCAD自动加载DlL
        /// </summary>
        /// <param name="appPath">Applications注册表路径</param>
        /// <param name="subKeyName">注册表子菜单名</param>
        /// <param name="desc">文件描述</param>
        /// <param name="dllPath">DLL程序路径</param>
        private static void AutoCADAutoLoadDLL(string appPath,string subKeyName, string desc, string dllPath)
        {
            Autodesk.AutoCAD.Runtime.RegistryKey locaIMachine = Autodesk.AutoCAD.Runtime.Registry.LocalMachine;
            Autodesk.AutoCAD.Runtime.RegistryKey applications = locaIMachine.OpenSubKey(appPath,true);
            Autodesk.AutoCAD.Runtime.RegistryKey myPrograrm = applications.CreateSubKey(subKeyName);
            myPrograrm.SetValue("DESCRIPTION", desc, RegistryValueKind.String);
            myPrograrm.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            myPrograrm.SetValue("LOADER", dllPath, RegistryValueKind.String);
            myPrograrm.SetValue("MANAGED", 1, RegistryValueKind.DWord);
        }

        private static void UninstallRegedit(string appPath)
        {
            Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor aced = dwg.Editor;
            aced.WriteMessage("\n 开始删除AutoCAD CMCU插件注册表:");
            try
            {
                Autodesk.AutoCAD.Runtime.RegistryKey LocalMachine = Autodesk.AutoCAD.Runtime.Registry.LocalMachine;
                Autodesk.AutoCAD.Runtime.RegistryKey Applications = LocalMachine.OpenSubKey(appPath, true);
                string[] subKeys = Applications.GetSubKeyNames();
                if (subKeys.Contains(CommonVariable.regeditSubKeyName))
                {
                    Applications.DeleteSubKeyTree(CommonVariable.regeditSubKeyName);
                }
                aced.WriteMessage("\n 删除AutoCAD CMCU插件注册表成功");
            }
            catch(System.Exception ex)
            {
                aced.WriteMessage("\n 删除AutoCAD CMCU插件注册表失败！");
                aced.WriteMessage("\n{0}", ex.ToString());
            }
        }
    }
}
