using Synergy.StandardApps.Domain.Notes;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Notes.Converters
{
    public class NoteToNoteFormConverter : IConverter<Note, NoteForm>
    {
        public NoteForm Convert(Note entity)
        {
            return new NoteForm()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Created = DateOnly.FromDateTime(entity.Created),
                Updated = DateOnly.FromDateTime(entity.Updated)
            };
        }
    }
}
