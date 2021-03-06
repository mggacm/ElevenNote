﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Data
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }                       //Globly unqie identifier

        [Required]
        public string Title { get; set; }

       
        public string Content { get; set; }

        public string OtherContent { get; set; }

        public decimal Sub { get; set; }
        public decimal Tip { get; set; }
        public decimal TipDecimal { get; set; }
        public decimal Total { get; set; }

        [DefaultValue(false)]
        public bool IsStarred { get; set; }

        [Required]
        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset? ModifiedUtc { get; set; }        //Makes it null-able

    }
}
