﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
namespace Common
{
    public static class ReadXml
    {


        //把接收到的XML转为字典
        public static Dictionary<string, string> XmlModel(string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            Dictionary<string, string> mo = new Dictionary<string, string>();
            var data = doc.DocumentElement.ChildNodes;
            //.SelectNodes("xml");
            for (int i = 0; i < data.Count; i++)
            {
                mo.Add(data.Item(i).LocalName, data.Item(i).InnerText);
            }
            return mo;
        }



        ////从字典中读取指定的值
        public static string ReadModel(string key, Dictionary<string, string> model)
        {
            string str = "";
            model.TryGetValue(key, out str);
            if (str== null)
                str = "";
            
            return str;
        }
        //输出字符串并结束当前页面进程 MVC必须加return
        public static void ResponseToEnd(string str)
        {
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
            return;
        }

        //输出字符串并结束当前页面进程 MVC必须加return
        public static string  Menu()
        {
            string Content = "";
            Content += "欢迎使用XXXX/微笑\n\n";
            Content += "输入以下序号开始获取最新信息：\n";
           
            Content += "1,新闻30分\n";
            Content += "2,电影预告\n";
            Content += "3,今日说法\n";
            Content += "4,焦点访谈\n";
            Content += "5,新闻联播\n";

            Content += "输入?或帮助 可以显示此菜单";
            return Content;
        }


    }
}
