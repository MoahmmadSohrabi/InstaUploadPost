using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstaUploadPost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InstaUploadPost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserSessionData _user;
        private readonly IInstaApi _api;

        private const string username = "username";
        private const string password = "password.";
        [BindProperty]
        public AddPostViewModel AddPostModel { get; set; }
        public bool IsSuccess { get; set; } = false;

        public IndexModel(ILogger<IndexModel> logger, UserSessionData user, IInstaApi api)
        {
            this._logger = logger;
            this._user = user;
            this._api = api;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var file = Image.FromStream(AddPostModel.Image.OpenReadStream());

            var basePath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/Images/Post");
            var fileName = Guid.NewGuid() + ".jpg";
            var fullPath = Path.Join(basePath, fileName);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            file.Save(fullPath, ImageFormat.Jpeg);
            file.Dispose();

            AddPostModel.Caption ??= "";

            var instaUpload = new InstaImageUpload(fullPath);
            await _api.MediaProcessor.UploadPhotoAsync(instaUpload, AddPostModel.Caption);

            try
            {
                System.IO.File.Delete(fullPath);
                Directory.Delete(fullPath);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(@"Can't Delete File");
            }

            IsSuccess = true;
            return Page();
        }
    }
}
