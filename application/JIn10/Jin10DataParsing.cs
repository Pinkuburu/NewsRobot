using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pinkuburu.robot.Code.application.JIn10
{
    //不会写json数据格式解析的，可以直接用http://tool.sufeinet.com/Creater/JsonClassGenerator.aspx来生成
    //把不好用的数据条目全都干掉了
    public class Data
    {
        //public int flag { get; set; }
        //public string name { get; set; }
        //public int star { get; set; }
        //public int type { get; set; }
        //public string unit { get; set; }
        //public double actual { get; set; }
        //public int affect { get; set; }
        //public string country { get; set; }
        //public int data_id { get; set; }
        //public string measure { get; set; }
        //public object revised { get; set; }
        //public string previous { get; set; }
        //public string pub_time { get; set; }
        //public object consensus { get; set; }
        //public string time_period { get; set; }
        //public string pic { get; set; }
        public string content { get; set; }
        //public int indicator_id { get; set; }
    }

    //public class Remark
    //{
    //    public int id { get; set; }
    //    public string link { get; set; }
    //    public string type { get; set; }
    //    public string title { get; set; }
    //    public string content { get; set; }
    //}

    public class Jin10DataParsing
    {
        public string id { get; set; }
        public string time { get; set; }
        //public int type { get; set; }
        public Data data { get; set; }
        public int important { get; set; }
        //public IList<object> tags { get; set; }
        //public IList<int> channel { get; set; }
        //public IList<Remark> remark { get; set; }
    }
}
