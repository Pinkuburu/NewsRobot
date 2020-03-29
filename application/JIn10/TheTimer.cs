using LitJson;
using System;
using System.Collections.Generic;
using System.Timers;

namespace com.pinkuburu.robot.Code.application.JIn10
{
    public class TheTimer
    {
        public static bool outTimer = true;
        private static readonly MySqlHelperzha mySqlHelper = new MySqlHelperzha("root", "root", "test");//链接MySQL数据库

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
            if (outTimer)
            {
                outTimer = false;
                Random rd = new Random();
                string html = GetURLContent.GetHtmlWithUtf("https://www.baidu.com/example.js?rnd=" + rd.NextDouble());//把这里替换为你的数据json地址，由于我用的是非授权的接口，就不公布啦
                html = html.Replace("var newest = ", "").Replace("];", "]");
                if (string.IsNullOrEmpty(html))
                {
                    outTimer = true;
                    return;
                }
                List<Jin10DataParsing> list = JsonMapper.ToObject<List<Jin10DataParsing>>(html);//记得引用LitJson来将Json数据转换为List

                //List<string> listID0 = new List<string>(mySqlHelper.SearchKeyAndReturnList("Jin10DataNews", "newsID", "infoStatus", "0"));
                List<string> oldNewsList = new List<string>(mySqlHelper.SearchKeyAndReturnList("Jin10DataNews", "newsID", "infoStatus", "1"));
                Common.CQLog.Debug("财经新闻", "新闻采集开始");
                foreach (var i in list)
                {
                    //空内容新闻就不写入数据库
                    if (i.important != 1)
                    {
                        continue;
                    }
                    if (oldNewsList.Contains(i.id))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(i.data.content))
                    {
                        continue;
                    }
                    if (i.data.content.Contains("VIP专享") || i.data.content.Contains("<a href=") || i.data.content.Contains("大佬来了") || i.data.content.Contains("一周复盘") || i.data.content.Contains("|"))
                    {
                        i.data.content = string.Empty;
                    }
                    i.data.content = i.data.content.Replace("</b>", "").Replace("<b>", "").Replace("<br/>", "\n").Replace("<br />", "\n").Replace("|", "");
                    //DateTime newTimeFormat = Convert.ToDateTime(i.time);
                    //string toInsertMySQLValue = i.id + "|" + newTimeFormat + "|" + i.data.content + "|0";
                    string toInsertMySQLValue = $"{i.id}|{Convert.ToDateTime(i.time)}|{i.data.content}|0";//这个时间转换主要新闻的数据用的是JS里的时间格式，这样子的：2020-03-29T13:13:26.000Z，这一步转换为c#的格式，新采集的新闻发送状态置为0
                    mySqlHelper.Insert("test.Jin10DataNews", "newsID|newsTime|newsDataContent|infoStatus", toInsertMySQLValue);
                    Common.CQLog.Debug("财经新闻", "ID:" + i + " 写入完毕");

                }
                List<string> newsList = new List<string>(mySqlHelper.SearchKeyAndReturnList("Jin10DataNews", "newsID", "infoStatus", "0"));
                string msgText = string.Empty;
                foreach (var i in newsList)
                {
                    msgText = mySqlHelper.SearchKeyAndReturn("Jin10DataNews", "newsDataContent", "infoStatus", "0");
                    //UpdateDate(表名,"infoStatus|newsID","1（这是我要把0修改成的1）|newsID的值"）
                    mySqlHelper.UpdateDate("Jin10DataNews", "infoStatus|newsID", $"1|{i}");//把以发送的新闻状态改为1
                    Common.CQLog.Debug("财经新闻", "ID:" + i + " 发送完毕");
                    if (!string.IsNullOrEmpty(msgText))
                    {
                        msgText = "【新闻内容】:\n" + msgText;
                        Common.CQApi.SendGroupMessage(1027630080, msgText);
                    }
                }
                GC.Collect();//心里作用，触发下垃圾回收
                outTimer = true;
            }
        }
    }
}
