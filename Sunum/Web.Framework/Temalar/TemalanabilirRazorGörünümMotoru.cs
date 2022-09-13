﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Framework.Temalar
{
    public class TemalanabilirRazorGörünümMotoru : TemalanabilirSanalYolSağlayıcıGörünümMotoru
    {
        public TemalanabilirRazorGörünümMotoru()
        {
            AreaViewLocationFormats = new[]
                                          {
                                              //themes
                                              "~/Areas/{2}/Temalar/{3}/Views/{1}/{0}.cshtml",
                                              "~/Areas/{2}/Temalar/{3}/Views/Shared/{0}.cshtml",
                                              
                                              //default
                                              "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                              "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                          };

            AreaMasterLocationFormats = new[]
                                            {
                                                //themes
                                                "~/Areas/{2}/Temalar/{3}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Temalar/{3}/Views/Shared/{0}.cshtml",


                                                //default
                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                            };

            AreaPartialViewLocationFormats = new[]
                                                 {
                                                     //themes
                                                    "~/Areas/{2}/Temalar/{3}/Views/{1}/{0}.cshtml",
                                                    "~/Areas/{2}/Temalar/{3}/Views/Shared/{0}.cshtml",
                                                    
                                                    //default
                                                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                                                 };

            ViewLocationFormats = new[]
                                      {
                                            //themes
                                            "~/Temalar/{2}/Views/{1}/{0}.cshtml",
                                            "~/Temalar/{2}/Views/Shared/{0}.cshtml",

                                            //default
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml",

                                            //Admin
                                            "~/Administration/Views/{1}/{0}.cshtml",
                                            "~/Administration/Views/Shared/{0}.cshtml",
                                      };

            MasterLocationFormats = new[]
                                        {
                                            //themes
                                            "~/Temalar/{2}/Views/{1}/{0}.cshtml",
                                            "~/Temalar/{2}/Views/Shared/{0}.cshtml", 

                                            //default
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml"
                                        };

            PartialViewLocationFormats = new[]
                                             {
                                                 //themes
                                                "~/Temalar/{2}/Views/{1}/{0}.cshtml",
                                                "~/Temalar/{2}/Views/Shared/{0}.cshtml",

                                                //default
                                                "~/Views/{1}/{0}.cshtml",
                                                "~/Views/Shared/{0}.cshtml", 

                                                //Admin
                                                "~/Administration/Views/{1}/{0}.cshtml",
                                                "~/Administration/Views/Shared/{0}.cshtml",
                                             };

            FileExtensions = new[] { "cshtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, null, false, fileExtensions);
            //return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, masterPath, true, fileExtensions);
        }
    }
}
