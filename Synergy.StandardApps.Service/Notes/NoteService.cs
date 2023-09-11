using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Notes;
using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Notes
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IConverter<Note, NoteForm> _noteConverter;

        public NoteService(IRepository<Note> noteRepository, IConverter<Note, NoteForm> noteConverter)
        {
            _noteRepository = noteRepository;
            _noteConverter = noteConverter;
        }

        public async Task<IResponse<NoteForm>> CreateNote(NoteCreationForm form)
        {
            try
            {
                if (form.HasErrors)
                    throw new("Invalid form.");

                var note = await _noteRepository
                    .GetAll()
                    .Where(n => n.Name.Equals(form.Name))
                    .FirstOrDefaultAsync();

                if(note is not null)
                {
                    return ResponseFactory.BadResponse<NoteForm>(Domain.Enums.ErrorCode.NameAlreadyTaken);
                }

                note = new Note()
                {
                    Name = form.Name,
                    Description = form.Description
                };

                await _noteRepository
                    .Create(note);

                return ResponseFactory.OK(_noteConverter.Convert(note));
            }
            catch(Exception ex)
            {
                return ResponseFactory.BadResponse<NoteForm>(ex);
            }
        }

        public async Task<IResponse<bool>> DeleteNote(long id)
        {
            try
            {
                var note = await _noteRepository
                    .GetAll()
                    .FirstOrDefaultAsync(n => n.Id == id);

                if(note is null)
                {
                    return ResponseFactory.BadResponse<bool>(Domain.Enums.ErrorCode.NotFound);
                }

                await _noteRepository
                    .Delete(note);

                return ResponseFactory.OK(true);
            }
            catch(Exception ex)
            {
                return ResponseFactory.BadResponse<bool>(ex);
            }
        }

        public async Task<IResponse<IEnumerable<NoteForm>>> GetNotes()
        {
            try
            {
                var notes = await _noteRepository
                    .GetAll()
                    .Select(n => _noteConverter.Convert(n))
                    .ToListAsync();

                return ResponseFactory.OK<IEnumerable<NoteForm>>(notes);
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<IEnumerable<NoteForm>>(ex);
            }
        }
    }
}
