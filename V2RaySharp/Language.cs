using System;
using System.Collections.Generic;
using System.Globalization;

namespace V2RaySharp
{
    class Language
    {
        static readonly Dictionary<string, string> Chinese = new Dictionary<string, string>()
        {
            {"Setup", "安装 或 卸载 V2Ray Sharp\n是：安装\t否：卸载\t取消：取消"},
            {"Start", "启动"},
            {"Restart", "重新启动"},
            {"Config", "编辑配置"},
            {"Exit", "退出"},
        };
        static readonly Dictionary<string, string> English = new Dictionary<string, string>()
        {
            {"Setup", "Install or uninstall V2Ray Sharp\nYes: Install\tNo: Uninstall\tCancel: Cancel"},
            {"Start", "Start"},
            {"Restart", "Restart"},
            {"Config", "Config"},
            {"Exit", "Exit"},
        };

        public static string GetString(string value)
        {
            try
            {
                switch (CultureInfo.InstalledUICulture.Name)
                {
                    case "zh-CN":
                        return Chinese[value];
                    default:
                        return English[value];
                }
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}
