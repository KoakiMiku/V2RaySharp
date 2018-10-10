using System;
using System.Collections.Generic;
using System.Globalization;

namespace V2RaySharp.Controller
{
    class Language
    {
        private static readonly Dictionary<string, string> Chinese = new Dictionary<string, string>()
        {
            {"Setup", "安装 或 卸载 V2Ray Sharp\n是：安装\t否：卸载\t取消：取消"},
            {"Switch", "状态"},
            {"ChangeNode", "切换节点"},
            {"EditConfig", "编辑配置"},
            {"Start", "启动"},
            {"Stop", "停止"},
            {"UpgradeNodeError", "节点更新失败！"},
            {"NoSubscription", "订阅链接未设置！"},
            {"NodeNotChange", "节点未更改！"},
            {"NodeNotSelect", "节点未选择！"},
        };

        private static readonly Dictionary<string, string> English = new Dictionary<string, string>()
        {
            {"Setup", "Install or uninstall V2Ray Sharp\nYes: Install\tNo: Uninstall\tCancel: Cancel"},
            {"Switch", "State"},
            {"ChangeNode", "Change Node"},
            {"EditConfig", "Edit Config"},
            {"Start", "Start"},
            {"Stop", "Stop"},
            {"UpgradeNodeError", "Upgrade node failure!"},
            {"NoSubscription", "No subscription link!"},
            {"NodeNotChange", "Node not changed!"},
            {"NodeNotSelect", "Node not selected!"},
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
