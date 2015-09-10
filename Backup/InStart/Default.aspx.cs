using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Text;

namespace InStart
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Index();
        }
        public void Index()
        {
            IBLL.IDoWei BLLWei = new BLL.DoWei();

            Dictionary<string, string> Model = new Dictionary<string, string>();
            string xmlData = string.Empty;
            if (Request.HttpMethod.ToUpper() == "POST")
            {

                using (Stream stream = Request.InputStream)
                {
                    Byte[] byteData = new Byte[stream.Length];
                    stream.Read(byteData, 0, (Int32)stream.Length);
                    xmlData = Encoding.UTF8.GetString(byteData);
                }
                if ((xmlData + "").Length > 0)//!string.IsNullOrEmpty(xmlData)
                {
                    try
                    {
                        Model = ReadXml.XmlModel(xmlData);
                    }
                    catch
                    {
                        //未能正确处理 给微信服务器回复默认值
                        DefaultResult();
                    }
                }
                if (Model.Count > 0)
                {
                    string msgType = ReadXml.ReadModel("MsgType", Model);
                    #region 判断消息类型
                    switch (msgType)
                    {
                        case "text":
                            BLLWei.DoText(Model);//文本消息
                            break;
                        case "image":
                            BLLWei.DoImg(Model);//图片
                            break;
                        case "voice": //声音
                            BLLWei.DoVoice(Model);
                            break;

                        case "video"://视频
                            BLLWei.DoVideo(Model);
                            break;

                        case "location"://地理位置
                            BLLWei.DoLocation(Model);
                            break;
                        case "link"://链接
                            BLLWei.DoLink(Model);
                            break;
                        #region 事件
                        case "event":
                            switch (ReadXml.ReadModel("Event", Model))
                            {
                                case "subscribe":
                                    if (ReadXml.ReadModel("EventKey", Model).IndexOf("qrscene_") >= 0)
                                    {
                                        BLLWei.DoOnCode(Model);//带参数的二维码扫描关注
                                    }
                                    else
                                    {
                                        BLLWei.DoOn(Model);//普通关注
                                    }
                                    break;
                                case "unsubscribe":
                                    BLLWei.DoUnOn(Model);//取消关注
                                    break;

                                case "SCAN":
                                    BLLWei.DoSubCode(Model);//已经关注的用户扫描带参数的二维码
                                    break;
                                case "LOCATION"://用户上报地理位置
                                    BLLWei.DoSubLocation(Model);
                                    break;
                                case "CLICK"://自定义菜单点击
                                    BLLWei.DoSubClick(Model);
                                    break;
                                case "VIEW"://自定义菜单跳转事件
                                    BLLWei.DoSubView(Model);
                                    break;
                            };
                            break;
                        #endregion
                    }
                    #endregion
                }
            }
            else
            {
                //get
                string echostr = Request.QueryString["echostr"];
                //这里直接返回echostr参数接入成功;
                ReadXml.ResponseToEnd(echostr);

            }
        }
        //返回默认值
        public void DefaultResult()
        {
            ReadXml.ResponseToEnd("");

        }
    }
}
