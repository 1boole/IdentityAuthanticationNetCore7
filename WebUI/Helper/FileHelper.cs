using Azure;
using Microsoft.AspNetCore.Mvc;
using System;
using WebUI.Entities;

namespace WebUI.Helper
{
    public class FileHelper
    {
        public static string PhotoSave(IFormFile photo, CancellationToken cancellationToken=default)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                     photo.CopyTo(stream);

                var returnPath = photo.FileName;

                return returnPath;


            }
            return null;

        }
    }
}
