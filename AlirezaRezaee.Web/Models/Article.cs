﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class Article
    {
        [Display(Name = "متن")]
        public string HtmlContent { get; set; }

        [Display(Name = "نشانی منبع")]
        public string SourcesUrl { get; set; }

        [Display(Name = "تصویر جلد")]
        public string CoverUrl { get; set; }

        #region Relations
        [Key]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
        #endregion
    }
}
