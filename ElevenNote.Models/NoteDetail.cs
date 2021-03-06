﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteDetail
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string OtherContent { get; set; }
        [DataType(DataType.Currency)]
        public decimal Sub { get; set; }
        public decimal Tip { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        [DataType(DataType.Currency)]
        public decimal TipDecimal { get; set; }
        public decimal Y { get; set; }

        public bool IsStarred { get; set; }

        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset? ModifiedUtc { get; set; }

        public override string ToString() => $"[{NoteId}] {Title}";
        
    }
}
