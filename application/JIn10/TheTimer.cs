using LitJson;
using System;
using System.Collections.Generic;
using System.Timers;

namespace com.pinkuburu.robot.Code.application.JIn10
{
    public class TheTimer
    {
        public static int inTimer = 0;
        public static string msgReal = string.Empty;
        public static void Countdown1()
        {
            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 60000; //执行间隔时间,单位为毫秒;   
            timer.Elapsed += new ElapsedEventHandler(SpiderSaver);
            timer.Start();
        }

        private static void SpiderSaver(object source, ElapsedEventArgs e)
        {
            if (inTimer == 0)
            {
                inTimer = 1;
                string a = GetURLContent.GetHtmlWithUtf("https://www.baidu.com/example.json" + rd.NextDouble());//这里的网址替换成你要的json数据的地址
                a = a.Replace("var newest = ", "").Replace("];", "]");
                List<JinDataParsing> list = JsonMapper.ToObject<List<JinDataParsing>>(a);//记得引用LitJson来将Json数据转换为List
                MySqlHelperzha mySqlHelper = new MySqlHelperzha("root", "root", "test");//链接数据库
                string newsTmp = string.Empty;
                List<string> listID0 = new List<string>(mySqlHelper.SearchKeyAndReturnList("JinDataNews", "newsID", "infoStatus", "0"));
                List<string> listID1 = new List<string>(mySqlHelper.SearchKeyAndReturnList("JinDataNews", "newsID", "infoStatus", "1"));
                foreach (var i in list)
                {
                    if (i.important == 1)
                    {
                        if (!listID1.Contains(i.id))
                        {
                            if (i.data.content.Contains("VIP专享") || i.data.content.Contains("<a href=") || i.data.content.Contains("大佬来了") || i.data.content.Contains("一周复盘")  || i.data.content.Contains("【交易热榜】") || i.data.content.Contains("|"))//发现新闻里含有广告信息就把广告直接清空
                            {
                                i.data.content = "";
                            }
                            DateTime newTimeFormat = Convert.ToDateTime(i.time);//这个其实没在用，主要新闻的数据用的是JS里的时间格式，这样子的：2020-03-29T13:13:26.000Z，这一步转换为c#的格式
                            i.data.content = i.data.content.Replace("</b>", "").Replace("<b>", "").Replace("<br/>", "\n").Replace("<br />", "\n");
                            newsTmp = i.id + "|" + newTimeFormat + "|" + i.data.content + "|0";
                            mySqlHelper.Insert("test.JinDataNews", "newsID|newsTime|newsDataContent|infoStatus", newsTmp);//新采集的新闻发送状态置为0
                            Common.CQLog.Debug("财经新闻", "ID:" + i + " 写入完毕");
                        }
                        else if (i.data.content == "")//空内容新闻就不写入数据库
                        {

                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                }

                List<string> listNews = new List<string>(mySqlHelper.SearchKeyAndReturnList("JinDataNews", "newsID", "infoStatus", "0"));//从数据库里找还没有发送的新闻呢
                foreach (var i in listNews)
                {
                    string msgText = mySqlHelper.SearchKeyAndReturn("JinDataNews", "newsDataContent", "infoStatus", "0");
                    msgReal = "【新闻内容】:\n" + msgText;
                    //UpdateDate(表名,"infoStatus|newsID","1（这是我要把0修改成的1）|newsID的值"）
                    string sqlText = "1|" + i;
                    mySqlHelper.UpdateDate("JinDataNews", "infoStatus|newsID", sqlText);//把以发送的新闻状态改为1
                    Common.CQApi.SendGroupMessage(1111111, msgReal);//别忘了改群号
                    Common.CQLog.Debug("财经新闻", "ID:" + i + " 发送完毕");
                }

                GC.Collect();//心里作用，触发下垃圾回收
                inTimer = 0;
            }
        }



    }
}
