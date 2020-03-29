using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.pinkuburu.robot.Code.application.JIn10
{
    class MySqlHelperzha  //系统(DBMS)>数据库(DB)>数据表(Table)>字段(field)


    {
        public String connetStr;//= "server=127.0.0.1;port=3306;user=rootzha;password=rootzha; database=rootzha;";


        public MySqlHelperzha(string user, string password, string database, string port = "3307", string server = "192.168.50.162")
        {
            connetStr = "server=" + server + ";port=" + port + ";user=" + user + ";password=" + password + "; database=" + database + ";";
        }








        public string GetMySQlStringForCreatrTable(string stringToInsert)
        {
            var temp = stringToInsert.Split('|');
            string MySqlText = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (i == 0)
                {
                    MySqlText = MySqlText + "`" + temp[i] + "` " + "VarChar(255) ";//+ "primary key "
                }
                else
                {
                    MySqlText = MySqlText + ", `" + temp[i] + "` " + "VarChar(255) ";
                }
                //`keyWords` VarChar(255) not null primary key , `cnts` int

            }
            return MySqlText;
        }



        public string GetMySQlStringForInsertTable(string TableName, string stringToInsert)
        {
            var temp = stringToInsert.Split('|');
            string MySqlText = "insert into " + TableName + " (";
            //insert into resume (name, gender, education, others) values(@1,@2,@3,@4)
            for (int i = 0; i < temp.Length; i++)
            {
                if (i + 1 == temp.Length)
                {
                    MySqlText = MySqlText + temp[i] + ") ";
                }
                else
                {
                    MySqlText = MySqlText + temp[i] + ", ";
                }
                //`keyWords` VarChar(255) not null primary key , `cnts` int

            }
            MySqlText = MySqlText + "values(";
            for (int i = 0; i < temp.Length; i++)
            {
                if (i + 1 == temp.Length)
                {
                    MySqlText = MySqlText + "@" + (i + 1) + ")";
                }
                else
                {
                    MySqlText = MySqlText + "@" + (i + 1) + ",";
                }
                //`keyWords` VarChar(255) not null primary key , `cnts` int

            }

            return MySqlText;
        }

        public bool CreatedMysqlTable(string TableName, string stringToInsert)
        {
            #region 创建mysql数据库表

            var IsRegex = new Regex("^Is[A-Z]");
            using (var Conn = new MySqlConnection(connetStr))
            {
                Conn.Open();

                string createStatement = "CREATE TABLE " + TableName + " (" + GetMySQlStringForCreatrTable(stringToInsert) + ")ENGINE=MyISAM DEFAULT CHARSET=utf8";
                using (MySqlCommand cmd = new MySqlCommand(createStatement, Conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// 插入新一行
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="stringToInsert"></param>
        /// <param name="stringToInsertValue"></param>
        /// <returns></returns>
        public bool Insert(string TableName, string stringToInsert, string stringToInsertValue)
        {

            MySqlConnection connetor = new MySqlConnection(connetStr);

            try
            {
                connetor.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                //Console.WriteLine("已经建立连接");
                MySqlCommand cmd = connetor.CreateCommand();
                cmd.CommandText = GetMySQlStringForInsertTable(TableName, stringToInsert);
                var temp = stringToInsertValue.Split('|');
                for (int i = 0; i < temp.Length; i++)
                {
                    string num = "@" + (i + 1);
                    cmd.Parameters.AddWithValue(num, temp[i]);
                }


                if (cmd.ExecuteNonQuery() == 1)
                    return true;

                else
                    return false;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connetor.Close();
            }
            return false;
        }

        public bool JustToInsertText(string temp)
        {
            MySqlConnection Conn = new MySqlConnection(connetStr);
            Conn.Open();


            using (MySqlCommand cmd = new MySqlCommand(temp, Conn))
            {
                try
                {
                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        return true;

                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;

                }
                finally
                {
                    Conn.Close();
                }
            }

        }



        /// <summary>
        /// 数据更新利用最后找位置
        /// </summary>
        /// <param name="sqlTableName"></param>
        /// <param name="stringToInsert"></param>
        /// <param name="stringToInsertValue"></param>
        /// <returns></returns>
        public bool UpdateDate(string sqlTableName, string stringToInsert, string stringToInsertValue)
        {
            var sqlText = "UPDATE " + sqlTableName + " SET ";//`学号` = '学号',`name` = 'dddd',`age` = 'age' WHERE `学号` = '201816310' ";
            var temp = stringToInsert.Split('|');
            var temp2 = stringToInsertValue.Split('|');
            for (int i = 0; i < temp.Length; i++)
            {
                if (i + 1 != temp.Length)
                {
                    sqlText += temp[i] + " = '" + temp2[i] + "' , ";
                }
                else
                {
                    sqlText += temp[i] + " = '" + temp2[i] + "' ";
                }

            }
            sqlText += "WHERE " + temp[temp.Length - 1] + " = '" + temp2[temp.Length - 1] + "' ";  //根据最后一个参数找where
            return JustToInsertText(sqlText);            
        }

        public bool InsertOrUpData(string sqlTableName, string stringToInsert, string stringToInsertValue)
        {
            if (!UpdateDate(sqlTableName, stringToInsert, stringToInsertValue))
            {
                return Insert(sqlTableName, stringToInsert, stringToInsertValue);

            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="sqlTableName"></param>
        /// <returns></returns>
        public bool IsExistTable(string sqlTableName)
        {
            //string Text = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE  TABLE_NAME='" + sqlTableName + "' ";
            string Txt = "SELECT COUNT(*) FROM `" + sqlTableName + "` ";
            //long count=(long)cmd.ExecuteScalar();
            MySqlConnection Conn = new MySqlConnection(connetStr);
            Conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(Txt, Conn))
            {
                try
                {
                    long count = (long)cmd.ExecuteScalar();
                    if (count != -1)
                    {
                        return true;

                    }
                    return false;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    return false;

                }
                finally
                {
                    Conn.Close();
                }
            }


        }

        /// <summary>
        /// 在某个表中寻找关键词，用wherekey定位,空就返回== ""
        /// </summary>
        /// <param name="sqlTableName"></param>
        /// <param name="GetWhichKey"></param>
        /// <param name="WhereKey"></param>
        /// <param name="WhereValue"></param>
        /// <returns></returns>
        public string SearchKeyAndReturn(string sqlTableName, string GetWhichKey, String WhereKey, string WhereValue)
        {
            var sql = "SELECT " + GetWhichKey + " FROM " + sqlTableName + "  WHERE " + WhereKey + " = '" + WhereValue + "'";
            MySqlConnection Conn = new MySqlConnection(connetStr);
            Conn.Open();
            //打开数据库连接
            // Conn.Open();
            //创建数据库操作对象
            MySqlCommand cmd = new MySqlCommand(sql, Conn);
            //创建数据库填充对象
            MySqlDataAdapter myda = new MySqlDataAdapter(cmd);
            //创建数据集DataSet用来临时存储数据
            DataSet ds = new DataSet();
            //往ds里面添加数据
            myda.Fill(ds);
            Conn.Close();
            return ds.Tables[0].Rows[0][0].ToString();   //空就返回== ""

        }


        public bool IsKeyEmpty(string sqlTableName, string GetWhichKey, String WhereKey, string WhereValue)
        {
            var sql = "SELECT " + GetWhichKey + " FROM " + sqlTableName + "  WHERE " + WhereKey + " = '" + WhereValue + "'";
            MySqlConnection Conn = new MySqlConnection(connetStr);
            Conn.Open();
            //打开数据库连接
            // Conn.Open();
            //创建数据库操作对象
            MySqlCommand cmd = new MySqlCommand(sql, Conn);
            //创建数据库填充对象
            MySqlDataAdapter myda = new MySqlDataAdapter(cmd);
            //创建数据集DataSet用来临时存储数据
            DataSet ds = new DataSet();
            //往ds里面添加数据
            myda.Fill(ds);
            Conn.Close();
            return ds.Tables[0].Rows[0][0].ToString() == "";   //空就返回== ""

        }



        public string GetMySQlStringForCreatrTableWithId(string stringToInsert)
        {
            var temp = stringToInsert.Split('|');
            string MySqlText = "";
            for (int i = -1; i < temp.Length; i++)
            {
                if (i == -1)
                {
                    //`id` int(11) NOT NULL AUTO_INCREMENT
                    MySqlText = MySqlText + "`id` " + "int(11) NOT NULL AUTO_INCREMENT ";//+ "primary key "
                }
                else
                {
                    MySqlText = MySqlText + ", `" + temp[i] + "` " + "VarChar(255) ";
                }
                //`keyWords` VarChar(255) not null primary key , `cnts` int

            }
            MySqlText += ", PRIMARY KEY (`id`) ";
            return MySqlText;
        }

        /// <summary>
        /// 更具where查找某个字段的值，返回一个list
        /// </summary>
        /// <param name="sqlTableName"></param>
        /// <param name="GetWhichKey"></param>
        /// <param name="WhereKey"></param>
        /// <param name="WhereValue"></param>
        /// <returns></returns>
        public List<string> SearchKeyAndReturnList(string sqlTableName, string GetWhichKey, String WhereKey, string WhereValue)
        {
            List<string> list = new List<string>();
            //var sql = "SELECT " + GetWhichKey + " FROM " + sqlTableName + "  WHERE " + WhereKey + " like '" + WhereValue + "'";
            var sql = "SELECT * FROM " + sqlTableName + "  WHERE " + WhereKey + " like '%" + WhereValue + "%'";

            MySqlConnection Conn = new MySqlConnection(connetStr);
            Conn.Open();
            try
            {
                //打开数据库连接
                // Conn.Open();
                //创建数据库操作对象
                MySqlCommand cmd = new MySqlCommand(sql, Conn);
                //创建数据库填充对象
                MySqlDataAdapter myda = new MySqlDataAdapter(cmd);
                //创建数据集DataSet用来临时存储数据
                DataSet ds = new DataSet();
                //往ds里面添加数据
                myda.Fill(ds);
                var firstTable = ds.Tables[0];
                for (int i = 0; i < firstTable.Rows.Count; i++)
                {
                    list.Add(firstTable.Rows[i][0].ToString());
                }
                return list;
            }
            finally
            {
                Conn.Close();
            }




        }
        /// <summary>
        /// 创建一个带id的表
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="stringToInsert"></param>
        /// <returns></returns>
        public bool CreateTableWithId(string TableName, string stringToInsert)
        {
            var IsRegex = new Regex("^Is[A-Z]");
            using (var Conn = new MySqlConnection(connetStr))
            {
                Conn.Open();

                string createStatement = "CREATE TABLE " + TableName + " (" + GetMySQlStringForCreatrTableWithId(stringToInsert) + ")ENGINE=MyISAM DEFAULT CHARSET=utf8";
                using (MySqlCommand cmd = new MySqlCommand(createStatement, Conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }


            }

        }


        /// <summary>
        /// 创建一个带id的表并自动赋初值idstart
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="stringToInsert"></param>
        /// <param name="idStart"></param>
        /// <returns></returns>
        public bool CreateTableWithIdAndSetIdStart(string TableName, string stringToInsert, int idStart)
        {
            if (CreateTableWithId(TableName, stringToInsert))
            {
                string sql = "alter table " + TableName + " AUTO_INCREMENT=" + idStart;
                return JustToInsertText(sql);
                //JustToInsertText
            }
            return false;

        }




    }
}
