﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least two characters.")]
        [MaxLength(128)]
        public string Title { get; set; }

       
        [MaxLength(8000)]
        public string Content { get; set; }
       
        [MaxLength(8000)]
        public string OtherContent { get; set; }

        public decimal Sub { get; set; }

        [Display(Name = "Tip %")]
        public decimal Tip { get; set; }

        public decimal TipDecimal { get; set; }

        public decimal Total { get; set; }

        public override string ToString() => Title;

       
    }
}
