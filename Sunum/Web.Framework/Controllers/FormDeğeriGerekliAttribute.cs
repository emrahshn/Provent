using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Framework.Controllers
{
    public class FormDeğeriGerekliAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] _gönderButtonuAdları;
        private readonly FormDeğeriGerekliliği _gereklilik;
        private readonly bool _sadeceAdlarıDoğrula;
        public FormDeğeriGerekliAttribute(params string[] gönderButtonuAdları):
            this(FormDeğeriGerekliliği.Eşit, gönderButtonuAdları)
        {
        }
        public FormDeğeriGerekliAttribute(FormDeğeriGerekliliği gereklilik,params string[] gönderButtonuAdları):
            this(gereklilik,true,gönderButtonuAdları)
        {
        }
        public FormDeğeriGerekliAttribute(FormDeğeriGerekliliği gereklilik,bool sadeceAdlarıDoğrula,params string[] gönderButtonuAdları)
        {
            this._gönderButtonuAdları = gönderButtonuAdları;
            this._gereklilik = gereklilik;
            this._sadeceAdlarıDoğrula = sadeceAdlarıDoğrula;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            foreach (string buttonAdı in _gönderButtonuAdları)
            {
                try
                {
                    switch (this._gereklilik)
                    {
                        case FormDeğeriGerekliliği.Eşit:
                            {
                                if (_sadeceAdlarıDoğrula)
                                {
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(x => x.Equals(buttonAdı, StringComparison.InvariantCultureIgnoreCase)))
                                        return true;
                                }
                                else
                                {
                                    string value = controllerContext.HttpContext.Request.Form[buttonAdı];
                                    if (!String.IsNullOrEmpty(value))
                                        return true;
                                }
                            }
                            break;
                        case FormDeğeriGerekliliği.İleBaşla:
                            {
                                if (_sadeceAdlarıDoğrula)
                                {
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(x => x.StartsWith(buttonAdı, StringComparison.InvariantCultureIgnoreCase)))
                                        return true;
                                }
                                else
                                {
                                    foreach (var formValue in controllerContext.HttpContext.Request.Form.AllKeys)
                                        if (formValue.StartsWith(buttonAdı, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            var value = controllerContext.HttpContext.Request.Form[formValue];
                                            if (!String.IsNullOrEmpty(value))
                                                return true;
                                        }
                                }
                            }
                            break;
                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }
            }
            return false;
        }
    }
    public enum FormDeğeriGerekliliği
    {
        Eşit,
        İleBaşla
    }
}
