﻿using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity = new Note
            {
                OwnerId = _userId,
                Title = model.Title,
                Content = model.Content,
                OtherContent = model.OtherContent,
                Sub = model.Sub,
                Tip = model.Tip,
                TipDecimal = (model.Tip / 100) * model.Sub,
                Total = model.Sub + ((model.Tip / 100) * model.Sub),
                CreatedUtc = DateTimeOffset.UtcNow
            };

            using(var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using(var ctx = new ApplicationDbContext())
            {
                var querey = ctx
                    .Notes
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                            new NoteListItem
                            {
                                NoteId = e.NoteId,
                                Title = e.Title,
                                Tip = e.Tip,
                                TipDecimal = e.TipDecimal,
                                Total = e.Total,
                                IsStarred = e.IsStarred,
                                CreatedUtc = e.CreatedUtc
                            }
                            );

                return querey.ToArray();
            }
        }

        public NoteDetail GetNoteById(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == noteId && e.OwnerId == _userId);

                return new NoteDetail
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    OtherContent = entity.OtherContent,
                    Sub = entity.Sub,
                    Tip = entity.Tip,
                    TipDecimal = entity.TipDecimal,
                    Total = entity.Total,
                    
                    IsStarred = entity.IsStarred,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
            }
        }

        public bool UpdateNote(NoteEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = 
                    ctx
                        .Notes
                        .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.OtherContent = model.OtherContent;
                entity.Sub = model.Sub;
                entity.Tip = model.Tip;
                entity.TipDecimal = (model.Tip/100)*model.Sub;
                entity.Total = model.Sub+((model.Tip / 100) * model.Sub);
                entity.IsStarred = model.IsStarred;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == noteId && e.OwnerId == _userId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
