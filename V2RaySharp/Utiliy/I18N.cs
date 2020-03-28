using System;
using System.Collections.Generic;
using System.Globalization;

namespace V2RaySharp.Utiliy
{
    class I18N
    {
        private static readonly Dictionary<string, string> Chinese = new Dictionary<string, string>()
        {
            {"FileNotFound", "缺少核心文件！"},
            {"PermissionDenied", "请以管理员身份运行！"},
            {"Setup", "安装 或 卸载 V2RaySharp\n是：安装\t否：卸载\t取消：取消"},
            {"Status", "状态"},
            {"ChangeNode", "切换节点"},
            {"Start", "启动"},
            {"Stop", "停止"},
            {"UpgradeNodeError", "节点更新失败！"},
            {"NoSubscription", "订阅链接未设置！"},
            {"NodeNotSelect", "节点未选择！"},
            {"Route", "路由"},
            {"Global", "全局"},
            {"Stoped", "停止"},
            {"RunningStatus", "运行状态"},
            {"Upgrade", "更新时间"},
            {"Waiting", "请稍后……"},
            {"HostOnly", "仅本机"},
            {"AllowAny", "允许所有"},
            {"UpgradeCompleted", "节点更新完成！"},
            {"PleaseWait", "请稍候……"},
        };

        private static readonly Dictionary<string, string> English = new Dictionary<string, string>()
        {
            {"FileNotFound", "Core file not found!"},
            {"PermissionDenied", "Please run as administrator!"},
            {"Setup", "Install or uninstall V2RaySharp\nYes: Install\tNo: Uninstall\tCancel: Cancel"},
            {"Status", "Status"},
            {"ChangeNode", "Change\nNode"},
            {"Start", "Start"},
            {"Stop", "Stop"},
            {"UpgradeNodeError", "Upgrade node failure!"},
            {"NoSubscription", "No subscription link!"},
            {"NodeNotSelect", "Node not selected!"},
            {"Route", "Route"},
            {"Global", "Global"},
            {"Stoped", "Stoped"},
            {"RunningStatus", "Running Status"},
            {"Upgrade", "Upgrade time"},
            {"Waiting", "Please wait..."},
            {"HostOnly", "Host Only"},
            {"AllowAny", "Allow Any"},
            {"UpgradeCompleted", "Upgrade node completed!"},
            {"PleaseWait", "Please wait..."},
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
