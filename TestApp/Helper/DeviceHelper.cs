using System;
using UIKit;

namespace Helper
{
    /// <summary>
    /// 单例模式的实现
    /// </summary>
    public class DeviceHelper
    {
        // 定义一个静态变量来保存类的实例
        private static DeviceHelper uniqueInstance;

        // 定义私有构造函数，使外界不能创建该类实例
        private DeviceHelper()
        {

        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static DeviceHelper Instance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new DeviceHelper();
            }
            return uniqueInstance;
        }

        /// <summary>
        /// 获取设备名称
        /// e.g. "My iPhone"
        /// </summary>
        /// <returns></returns>
        public string GetDeviceName()
        {
            UIDevice pDevice = UIDevice.CurrentDevice;
            return pDevice.Name;
        }

        /// <summary>
        /// 获取设备模型
        /// "iPhone", "iPod touch"
        /// </summary>
        /// <returns></returns>
        public string GetDeviceModel()
        {
            UIDevice pDevice = UIDevice.CurrentDevice;
            return pDevice.Model;
        }
        /// <summary>
        /// 获取设备本地化模型
        /// </summary>
        /// <returns></returns>
        public string GetLocalizedModel()
        {
            UIDevice pDevice = UIDevice.CurrentDevice;
            return pDevice.LocalizedModel;
        }

        /// <summary>
        /// 获取设备系统名称
        /// e.g. "iOS"
        /// </summary>
        /// <returns></returns>
        public string GetDeviceSystemName()
        {
            UIDevice pDevice = UIDevice.CurrentDevice;
            return pDevice.SystemName;
        }

        /// <summary>
        /// 获取设备系统版本
        /// e.g. "4.0"
        /// </summary>
        /// <returns></returns>
        public string GetDeviceSystemVersion()
        {
            UIDevice pDevice = UIDevice.CurrentDevice;
            return pDevice.SystemVersion;
        }

        /// <summary>
        /// 获取电量
        /// </summary>
        /// <returns></returns>
        public string GetBatteryMoniter()
        {
            UIDevice pDevice = UIDevice.CurrentDevice;
            pDevice.BatteryMonitoringEnabled = true;
            // 取正，防止负值出现
            float electricity = Math.Abs(pDevice.BatteryLevel);
            return (electricity * 100).ToString("0.00") +"%";
        }

    }
}
