# NewsRobot
 代码的功能是定时从某个财经新闻的新闻源，抓取数据、储存并发送到QQ群里，本代码GPL V3协议开源，各自类库、组件遵循其各自协议
# 说明
代码无法直接被编译通过，你需要做些包含但不限于如下的事情：
  1. 引用LitJson的库
  2. 酷Q机器人
  3. [Native.SDK](https://native.run/#/)
  4. 修改群号、Json数据源信息等
# JinDataNews数据库设计
| 字段      |    类型 | 作用  |
| :-------- | --------:| :--: |
| newsID  | longtext |新闻源提供的新闻ID，由20位正整数组成|
| newsTime     |   longtext |  精确到日月年时分秒的时间，为新闻源数据附带的时间信息，需要经过转换和才能成为.net能识别的时间格式  |
| newsDataContent      |    longtext | 新闻内容  |
| infoStatus      |    text | 用0和1标记新闻是否被发送过  |
# chaincoab数据库设计
| 字段      |    类型 | 作用  |
| :-------- | --------:| :--: |
| charname| text |角色名称|
| weapon|   text |  角色职业、武器 |
| chaincoab|    text | 角色连携能力名称  |
| chaincoabtext|    text | 角色连携能力描述  |
# 代码说明
* Event_AppEnable.cs 插件启动事件，全局化CQApi和CQLog
* Event_GroupMessage.cs 发送群消息的方法
* TheTimer.cs 新闻抓取、储存、发送的方法
* MySqlHelperzha.cs 由炸炸编写的MySQL操作的方法，我将366行的WHERE 改为 Like，进行数据库的模糊查找
* JinDataParsing.cs 对新闻源Json解析的方法，由工具自动生成，看注释
* GetURLContent.cs C#使用GZIP解压缩完整读取网页内容 Get方法，来源网络
* DragaliaLost.cs 通过指定群消息去数据库里查找某手游角色的能力信息

# 感谢
  * MySqlHelperzha.cs 的代码由炸炸（1918681155#qq.com 请自行把#改为@）撰写，使用上对我指导颇多
  * 感谢酸辣鸡血(3472893403) 和雪（cirno#bakasnow.com 请自行把#改为@） 对我这些菜鸟代码的修改和帮助