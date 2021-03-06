﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "انتخاب {0} برای دسته بندی الزامی است.")]
        public string Title { get; set; }

        public ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
