
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace cadplugin.Tools
{
    public class RegeditTool
    {
        /// <summary>
        /// 得到电脑当前已经安装的CAD版本号
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHasInstallCADVersion()
        {
            List<string> versionList = new List<string>();
            RegistryKey locaIMachine = Registry.LocalMachine;
            RegistryKey applications = locaIMachine.OpenSubKey("SOFTWARE\\Autodesk\\AutoCAD\\", true);
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
                RegistryKey applicationVersion = locaIMachine.OpenSubKey("SOFTWARE\\Autodesk\\AutoCAD\\"+ subKey, true);
                string[] lanSubKeys = applicationVersion.GetSubKeyNames();
                foreach (string lanSubKey in lanSubKeys)
                {
                    if (lanSubKey.IndexOf(":") != -1)
                    {
                        string[] lanStrs = lanSubKey.Split(':');
                        if (lanStrs[1].Equals("804"))
                            version += " 简体中文版";
                        else if (lanStrs[1].Equals("409"))
                            version += " 英文版";
                        else
                            version += "";
                    }
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
        public static string GetVersionSubKeyByCADVersion(string apiVersion)
        {
            string[] apiVersionStr = apiVersion.Split(' ');
            string versionInfoR;   //代表版本信息 如：R18.0
            string versionInfoN;   //代表版本信息 如：B001
            string versionInfoL;   //代表版本语言信息 如：804

            switch (apiVersionStr[0])
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
            if (apiVersionStr[1].Equals("简体中文版"))
                versionInfoL = "804";
            else if (apiVersionStr[1].Equals("英文版"))
                versionInfoL = "409";
            else
                versionInfoL = "";

            string appPath = "SOFTWARE\\Autodesk\\AutoCAD\\" + versionInfoR + "\\ACAD-"+ versionInfoN + ":"+versionInfoL+ "\\Applications";     
            return appPath;
        }

        /// <summary>
        /// 得到当前VersionSubKey的AutoCADVersion
        /// </summary>
        /// <param name="apiVersion">2016</param>
        /// <returns></returns>
        public static string GetAutoCADVersionByVersionSubKey(string versionSubkey)
        {
            string versionInfoR;   //代表版本信息 如：ACAD-F001
            string versionInfoL;   //代表版本语言信息 如：804
            string cadVersion = "";     //要返回的CADVersion
        
            string[] versionInfo = versionSubkey.Split(':');
            versionInfoR = versionInfo[0];
            versionInfoL = versionInfo[1];

            switch (versionInfoR)
            {
                case "ACAD-6001":
                    cadVersion = "2008";
                    break;
                case "ACAD-7001":
                    cadVersion = "2009";
                    break;
                case "ACAD-8001":
                    cadVersion = "2010";
                    break;
                case "ACAD-9001":
                    cadVersion = "2011";
                    break;
                case "ACAD-A001":
                    cadVersion = "2012";
                    break;
                case "ACAD-B001":
                    cadVersion = "2013";
                    break;
                case "ACAD-D001":
                    cadVersion = "2014";
                    break;
                case "ACAD-E001":
                    cadVersion = "2015";
                    break;
                case "ACAD-F001":
                    cadVersion = "2016";
                    break;
            }
            switch (versionInfoL)
            {
                case "804":
                    cadVersion += " 简体中文版";
                    break;
                case "409":
                    cadVersion += " 英文版";
                    break;
            }
            return cadVersion;
        }

        /// <summary>
        /// 通过添加注册表的方式让AutoCAD自动加载DlL
        /// </summary>
        /// <param name="appPath">Applications注册表路径</param>
        /// <param name="subKeyName">注册表子菜单名</param>
        /// <param name="desc">文件描述</param>
        /// <param name="dllPath">DLL程序路径</param>
        public static void AutoCADAutoLoadDLL(string appPath,string subKeyName, string desc, string dllPath)
        {
            RegistryKey locaIMachine = Registry.LocalMachine;
            RegistryKey applications = locaIMachine.OpenSubKey(appPath,true);
            RegistryKey myPrograrm = applications.CreateSubKey(subKeyName);
            myPrograrm.SetValue("DESCRIPTION", desc, RegistryValueKind.String);
            myPrograrm.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            myPrograrm.SetValue("LOADER", dllPath, RegistryValueKind.String);
            myPrograrm.SetValue("MANAGED", 1, RegistryValueKind.DWord);
        }

        public static int UninstallRegedit(string appPath,string dllName)
        {
            int count = 0;
            try
            {
                RegistryKey LocalMachine = Registry.LocalMachine;
                RegistryKey Applications = LocalMachine.OpenSubKey(appPath, true);
                string[] subKeys = Applications.GetSubKeyNames();
                if (subKeys.Contains("CMCUCAD_"+dllName))
                {
                    Applications.DeleteSubKeyTree("CMCUCAD_" + dllName);
                    count = 1;
                }
            }
            catch(System.Exception ex)
            {
                
            }
            return count;
        }

        /// <summary>
        /// 得到自动加载的DLL应用程序的信息
        /// </summary>
        public static List<RegeditModel> GetAutoLoadDllsInfo()
        {
            string baseStr = "SOFTWARE\\Autodesk\\AutoCAD\\";
            List<RegeditModel> regeditModelList = new List<RegeditModel>();

            RegistryKey locaIMachine = Registry.LocalMachine;
            RegistryKey applications = locaIMachine.OpenSubKey(baseStr, true);
            if (applications != null)
            {
                string[] subKeys = applications.GetSubKeyNames();
                foreach (string subKey in subKeys)
                {
                    RegistryKey applicationVersion = locaIMachine.OpenSubKey(baseStr+subKey, true);
                    string[] versionSubKeys = applicationVersion.GetSubKeyNames();
                    foreach (string versionSubKey in  versionSubKeys)
                    {
                        if (versionSubKey.IndexOf(":") != -1)
                        {
                            RegistryKey applicationVersionLan = locaIMachine.OpenSubKey(baseStr + subKey + "\\" + versionSubKey + "\\Applications", true);
                            string[] dllSubKeys = applicationVersionLan.GetSubKeyNames();
                            foreach (string dllSubKey in dllSubKeys)
                            {
                                if (dllSubKey.StartsWith("CMCUCAD_"))
                                {
                                    RegistryKey applicationDll = locaIMachine.OpenSubKey(baseStr + subKey + "\\"+ versionSubKey + "\\Applications\\" + dllSubKey, true);
                                    RegeditModel regeditModel = new RegeditModel();
                                    regeditModel.DllName = applicationDll.GetValue("DESCRIPTION").ToString(); ;
                                    regeditModel.DllPath = applicationDll.GetValue("LOADER").ToString();
                                    regeditModel.Version = GetAutoCADVersionByVersionSubKey(versionSubKey);
                                    regeditModelList.Add(regeditModel);
                                }
                            }
                        }
                    }

                }
            }

            return regeditModelList;
            
        }

 
    }

    public class RegeditModel
    {
        private string version;
        private string dllName;
        private string dllPath;

        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }
        public string DllName
        {
            get
            {
                return dllName;
            }
            set
            {
                dllName = value;
            }
        }
        public string DllPath
        {
            get
            {
                return dllPath;
            }
            set
            {
                dllPath = value;
            }
        }
    }
}
