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
                id = "2241",
                content = "2012-2013学年第二学期考试违规情况通报"
            };
            return JPushHelper.SendNotification(1, "119074021", DynamicJson.Serialize(msg)) ;
        }

        [WebMethod]
        public string BroadcastNotification(string id, string content)
        {
            var msg = new
            {
                id = id,
                content = content
            };
            return JPushHelper.BroadcastNotification(1, DynamicJson.Serialize(msg));
        }
    }
}