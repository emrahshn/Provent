using Core;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Web.Framework.Mvc
{
    public class JsonSonuçDönüştürücü : JsonResult
    {
        private readonly JsonConverter[] _dönüştürücüler;
        public JsonSonuçDönüştürücü(params JsonConverter[] dönüştürücüler)
        {
            _dönüştürücüler = dönüştürücüler;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.HttpContext == null || context.HttpContext.Response == null)
                return;
            context.HttpContext.Response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : MimeTipleri.ApplicationJson;
            if (ContentEncoding != null)
                context.HttpContext.Response.ContentEncoding = ContentEncoding;
            if (Data != null)
                context.HttpContext.Response.Write(JsonConvert.SerializeObject(Data, _dönüştürücüler));
        }
    }
}
