using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Codeplex.Data;

namespace pushTestService
{
    public class PushTestService : System.Web.Services.WebService
    {
        [WebMethod]
        public string SendNotification()
        {
            var msg = new
            {
                type = "notice",
                id = "ID",
                title = "新闻标题",
                content = "新闻内容"
            };
            return JPushHelper.SendNotification(1, "119074021", DynamicJson.Serialize(msg)) ;
        }

        [WebMethod]
        public string BroadcastNotification(string id, string title, string content)
        {
            var msg = new
            {
                type = "notice",
                id = id,
                title = title,
                content = content
            };
            return JPushHelper.BroadcastNotification(1, DynamicJson.Serialize(msg));
        }
    }
}