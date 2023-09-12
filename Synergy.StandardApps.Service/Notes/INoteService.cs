using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Notes
{
    public interface INoteService
    {
        Task<IResponse<NoteForm>> CreateNote(NoteCreationForm form);
        Task<IResponse<NoteForm>> UpdateNote(NoteCreationForm form, long id);
        Task<IResponse<IEnumerable<NoteForm>>> GetNotes();
        Task<IResponse<bool>> DeleteNote(long id);
    }
}
