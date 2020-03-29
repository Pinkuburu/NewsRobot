using com.pinkuburu.robot.Code.application;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;

namespace com.pinkuburu.robot.Code
{
    public class Event_GroupMessage : IGroupMessage
    {
        /// <summary>
        /// 收到群消息
        /// </summary>
        /// <param name="sender">事件来源</param>
        /// <param name="e">事件参数</param>
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {           
            if (e.Message.Text.Length >= 4 && e.Message.Text.StartsWith(".龙约"))
            {
                CQCode cqat = e.FromQQ.CQCode_At();
                e.FromGroup.SendGroupMessage(cqat, " 找到结果如下，数据仅供参考\n" + DragaliaLost.chaincoab(e.Message.Text));
            }
        }







    }
}
