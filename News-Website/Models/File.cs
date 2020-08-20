using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class BlobFile
    {
        public int Id { get; set; }
        [DataType(DataType.Url)]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "File")]
        //[MaxFileSize( 1 * 1024 * 1024)]
        //[PermittedExtensions(new string[] { ".jpg", ".png", ".gif", ".jpeg"})]
        [NotMapped]
        public virtual IFormFile File { get; set; }
        public string StorageName { get; set; }
        [Display(Name="Caption or copyright")]
        [StringLength(1000)]
        public string Description { get; set; }
        [Display(Name = "Alt Text")]
        [StringLength(1000)]
        public string AltText { get; set; }
    }
}
