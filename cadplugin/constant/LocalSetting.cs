using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cadplugin.constant
{
    public class LocalSetting
    {
        private static LocalSetting setting;


        #region 属性定义
        private string userName;
        private string passWord;
        private string paperSavePath;
        private string uploadPath;
        private string localPluginVersion;
        private string localBasePath;
        private bool savePassword;
        private bool autoLogin;

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string PassWord
        {
            get
            {
                return passWord;
            }

            set
            {
                passWord = value;
            }
        }

        public string PaperSavePath
        {
            get
            {
                return paperSavePath;
            }

            set
            {
                paperSavePath = value;
            }
        }

        public string UploadPath
        {
            get
            {
                return uploadPath;
            }

            set
            {
                uploadPath = value;
            }
        }

        public bool SavePassword
        {
            get
            {
                return savePassword;
            }

            set
            {
                savePassword = value;
            }
        }

        public bool AutoLogin
        {
            get
            {
                return autoLogin;
            }

            set
            {
                autoLogin = value;
            }
        }

        public string LocalPluginVersion
        {
            get
            {
                return localPluginVersion;
            }
            set
            {
                localPluginVersion = value;
            }
        }

        public string LocalBasePath
        {
            get
            {
                return localBasePath;
            }

            set
            {
                localBasePath = value;
            }
        }

        #endregion



        public static LocalSetting GetSetting()
        {
            if (setting == null)
            {
                string defualtUser = GetDefualtUser();
                if (string.IsNullOrEmpty(defualtUser))
                    setting = new LocalSetting();
                else
                {
                    setting = GetSetting(defualtUser);
                }
            }
            return setting;
        }

        public static LocalSetting GetSetting(string userName)
        {
            LocalSetting r = new LocalSetting();
            string settingDirectory = GetSettingPath();

            if (Directory.Exists(settingDirectory))
            {
                string[] files = Directory.GetFiles(settingDirectory);
                foreach (string filepath in files)
                {
                    string userFileName = Path.GetFileNameWithoutExtension(filepath);
                    if (userFileName.ToUpper().Equals(userName.ToUpper()))
                    {
                        string[] temp = File.ReadAllLines(filepath);
                        if (temp.Count() > 0)
                        {
                            foreach (string set in temp)
                            {
                                string ds = DecodeBase64(set);
                                string[] kp = ds.Split(new Char[] { '=' });
                                if (kp.Count() == 2)
                                {
                                    string key = kp[0];
                                    string value = kp[1];
                                    switch (key.ToUpper())
                                    {
                                        case "USERNAME": r.userName = value; break;
                                        case "PASSWORD": r.PassWord = value; break;
                                        case "PAPERSAVEPATH": r.PaperSavePath = value; break;
                                        case "UPLOADPATH": r.uploadPath = value; break;
                                        case "SAVEPASSWORD":
                                            {
                                                if (!string.IsNullOrEmpty(value) && "TRUE".Equals(value.ToUpper()))
                                                    r.SavePassword = true;
                                                else
                                                    r.SavePassword = false;
                                            }; break;
                                        case "AUTOLOGIN":
                                            {
                                                if (!string.IsNullOrEmpty(value) && "TRUE".Equals(value.ToUpper()))
                                                    r.AutoLogin = true;
                                                else
                                                    r.AutoLogin = false;
                                            }; break;
                                    }
                                }   
                            }
                        }
                    }
                }
            }
            if (!r.SavePassword)
                r.PassWord = "";
            return r;
        }

        public static string GetDefualtUser()
        {
            string defualtUser = "";
            string settingDirectory = GetSettingPath();
            if (Directory.Exists(settingDirectory))
            {
                string[] files = Directory.GetFiles(settingDirectory);
                DateTime defualtTime = DateTime.MinValue;
                foreach (string file in files)
                {
                    DateTime fileAccessTime = File.GetLastAccessTime(file);
                    if (fileAccessTime > defualtTime)
                    {
                        defualtTime = fileAccessTime;
                        defualtUser = Path.GetFileNameWithoutExtension(file);
                    }
                }
            }
            return defualtUser;
        }

        //获取配置文件存放路径
        public static string GetSettingPath()
        {
            string defualtPath = GetLocalRoot() + "\\Setting";
            if (!Directory.Exists(defualtPath))
            {
                Directory.CreateDirectory(defualtPath);
            }
            return defualtPath;
        }

        private static string GetLocalRoot()
        {
            string pathStr = @"C:\Program Files (x86)\CMCU中机中联CAD插件";
            string basePath = Assembly.GetExecutingAssembly().Location;
            string[] args = basePath.Split(new Char[] { '\\', '/' });
            if (args.Count() > 3)
            {
                List<string> parts = new List<string>();
                for (int i = 0; i < args.Length - 3; i++)
                {
                    string temp = args[i];
                    if (!string.IsNullOrEmpty(temp))
                        parts.Add(temp);
                }
                pathStr = string.Join("\\", parts.ToArray());
            }
            return pathStr;
        }


        private static string EncodeBase64(string source)//加密
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }
        private static string DecodeBase64(string source)//解密
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(source));
        }
    }
}
