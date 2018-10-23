using System;
using System.Collections.Generic;
using System.Globalization;

namespace V2RaySharpWPF.Controller
{
    class I18N
    {
        private static readonly Dictionary<string, string> Chinese = new Dictionary<string, string>()
        {
            {"FileNotFound", "缺少核心文件！"},
            {"AlreadyRunning", "V2Ray Sharp 已在运行……"},
            {"Setup", "安装 或 卸载 V2Ray Sharp\n是：安装\t否：卸载\t取消：取消"},
            {"Status", "状态"},
            {"ChangeNode", "切换节点"},
            {"EditConfig", "编辑配置"},
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
            {"None", "无"},
            {"Waiting", "请稍后……"},
        };

        private static readonly Dictionary<string, string> English = new Dictionary<string, string>()
        {
            {"FileNotFound", "Core file not found!"},
            {"AlreadyRunning", "V2Ray Sharp is already running..."},
            {"Setup", "Install or uninstall V2Ray Sharp\nYes: Install\tNo: Uninstall\tCancel: Cancel"},
            {"Status", "Status"},
            {"ChangeNode", "Change Node"},
            {"EditConfig", "Edit Config"},
            {"Start", "Start"},
            {"Stop", "Stop"},
            {"UpgradeNodeError", "Upgrade node failure!"},
            {"NoSubscription", "No subscription link!"},
            {"NodeNotSelect", "Node not selected!"},
            {"Route", "Route"},
            {"Global", "Global"},
            {"Stoped", "Stoped"},
            {"RunningStatus", "Running status"},
            {"Upgrade", "Upgrade time"},
            {"None", "None"},
            {"Waiting", "Please wait..."},
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
