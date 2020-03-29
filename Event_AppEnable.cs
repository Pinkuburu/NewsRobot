using com.pinkuburu.robot.Code.application.JIn10;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pinkuburu.robot.Code
{
    public static class Common
    {
        public static CQApi CQApi { get; set; }
        public static CQLog CQLog { get; set; }
    }
    public class Event_AppEnable : IAppEnable
    {
        /// <summary>
        /// 酷Q应用启用 回调
        /// </summary>
        /// <param name="sender">事件来源对象</param>
        /// <param name="e">附加的事件参数</param>
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            Common.CQApi = e.CQApi;
            Common.CQLog = e.CQLog;
            TheTimer.Countdown1();
        }
    }
}
