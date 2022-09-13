using System.Web.Mvc;
using System;
using Services.Medya;
using System.IO;
using System.Web;
using Core;
using Web.Framework.Güvenlik;

namespace Web.Controllers
{
    public partial class ResimController : TemelPublicController
    {
        private readonly IResimServisi _pictureService;

        public ResimController(IResimServisi pictureService)
        {
            this._pictureService = pictureService;
        }

        [HttpPost]
        [AntiForgery(true)]
        public virtual ActionResult AsyncUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTipleri.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTipleri.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTipleri.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTipleri.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTipleri.ImageTiff;
                        break;
                    default:
                        break;
                }
            }

            var picture = _pictureService.ResimEkle(fileBinary, contentType, null);
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.ResimUrlAl(picture, 100)
            },
                MimeTipleri.TextPlain);
        }
    }
}