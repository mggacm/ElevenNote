using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteEdit
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string OtherContent { get; set; }

        public decimal Sub { get; set; }
        public decimal Tip { get; set; }
        public decimal Total { get; set; }

        public decimal X { get; set; }
        public decimal Y { get; set; }

        public bool IsStarred { get; set; }

       

    }
}
