using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteListItem
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        public decimal Total { get; set; }
        public decimal Tip { get; set; }
        public decimal TipDecimal { get; set; }
        [UIHint("Starred")]
        public bool IsStarred { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }

        public override string ToString() => $"[{NoteId}] {Title} {Total} {Tip}";


    }
}
