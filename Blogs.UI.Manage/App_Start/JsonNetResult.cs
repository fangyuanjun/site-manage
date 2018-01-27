using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage
{
    public class JsonNetResult : JsonResult
    {
        private string dateTimeFormat = null;
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
        }

        public JsonNetResult(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet, string contentType = null, Encoding contentEncoding = null,string dateTimeFormat=null)
        {
            this.Data = data;
            this.JsonRequestBehavior = behavior;
            this.ContentEncoding = contentEncoding;
            this.ContentType = contentType;
            this.dateTimeFormat = dateTimeFormat;
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;

            if (this.Data == null)
                return;

            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            if (dateTimeFormat==null)
            {
                timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            }
            else
            {
                timeConverter.DateTimeFormat = dateTimeFormat;
            }

            string serialStr = JsonConvert.SerializeObject(this.Data, timeConverter);
            response.Write(serialStr);


            //var scriptSerializer = JsonSerializer.Create(this.Settings);

            //using (var sw = new StringWriter())
            //{
            //    scriptSerializer.Serialize(sw, this.Data);
            //    response.Write(sw.ToString());
            //}
        }
    }
}