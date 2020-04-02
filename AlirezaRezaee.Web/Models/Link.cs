﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [Display(Name = "رتبه")]
        public short Rank { get; set; }

        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [MaxLength(100, ErrorMessage = "طول {0} نباید بیش از {1} کاراکتر باشد.")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [MaxLength(200, ErrorMessage = "طول {0} نباید بیش از {1} کاراکتر باشد.")]
        [Display(Name = "نشانی")]
        public string Url { get; set; }

        [MaxLength(200, ErrorMessage = "")]
        public string ImagePath { get; set; }
    }
}