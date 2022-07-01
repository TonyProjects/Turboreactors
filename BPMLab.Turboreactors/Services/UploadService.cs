using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BPMLab.Turboreactors.Services
{
    public class UploadService
    {
        public async Task UploadFileAsync(HttpPostedFileBase file, string path, string name)
        {
            if (file.ContentLength > 0)
            {
                using (FileStream destinationStream = System.IO.File.Create(Path.Combine(path, name)))
                {
                    await file.InputStream.CopyToAsync(destinationStream);
                }
            }
        }
    }
}