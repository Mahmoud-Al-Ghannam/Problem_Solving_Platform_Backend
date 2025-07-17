using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.API.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services;

public class FileService
{
    
    public static async Task<string?> SaveImageAndGetURL(IFormFile? imageFile)
    {
        if (imageFile == null)
            return null;
        
        string pathOfImage = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        string fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", pathOfImage);
        using (var stream = new FileStream(fullpath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }
        return pathOfImage;
    }

}
