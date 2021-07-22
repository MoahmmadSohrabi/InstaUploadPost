using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace InstaUploadPost.Models
{
    public class AddPostViewModel
    {
        [AllowNull]
        public string Caption { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
