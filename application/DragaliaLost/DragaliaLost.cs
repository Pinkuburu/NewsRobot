using com.pinkuburu.robot.Code.application.JIn10;
using System;
using System.Collections.Generic;
using Native.Sdk.Cqp;

namespace com.pinkuburu.robot.Code
{
    internal class DragaliaLost
    {
        public static string chaincoab(string text)
        {
            string charname = text.Replace(".龙约","");
            string msgReturn = "";
            MySqlHelperzha mySqlHelper = new MySqlHelperzha("root", "root", "dragalialost");
            List<string> listCharlList = new List<string>(mySqlHelper.SearchKeyAndReturnList("chaincoab", "charname", "charname", charname));
            foreach (var i in listCharlList)
            {
                string charName = i.ToString();
                string weapon = mySqlHelper.SearchKeyAndReturn("chaincoab", "weapon", "charname", i.ToString());
                string chainCoab = mySqlHelper.SearchKeyAndReturn("chaincoab", "chaincoab", "charname", i.ToString());
                string chainCoabText = mySqlHelper.SearchKeyAndReturn("chaincoab", "chaincoabtext", "charname", i.ToString());
                msgReturn += "角色:" + charName + "\n使用武器：" + weapon + "\n联效EX名：" + chainCoab + "\n联效EX效果：" +
                            chainCoabText+"\n\n";
            }

            msgReturn += "\n数据来源：https://bbs.nga.cn/read.php?tid=20983425";
            //Common.CQLog.Debug("龙约查询", "内容:" + msgReturn);
            return msgReturn;
        }
    }
}