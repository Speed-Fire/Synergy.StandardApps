using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Notes;
using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Service.Exceptions;
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
                form.Validate();
				if (form.HasErrors)
                    throw new InvalidFormException();

                var note = await _noteRepository
                    .GetAll()
                    .FirstOrDefaultAsync(n => n.Name == form.Name);

                if (note is not null)
                    throw new NameIsAlreadyTakenExceptionException();

                note = new Note()
                {
                    Name = form.Name,
                    Description = form.Description,
                    Color = form.ColorNum
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

        public async Task<IResponse<NoteForm>> UpdateNote(NoteCreationForm form, long id)
        {
            try
            {
				form.Validate();
				if (form.HasErrors)
                    throw new InvalidFormException();

				var note = await _noteRepository
                    .GetAll()
                    .FirstOrDefaultAsync(n => n.Id == id)
                    ?? throw new InvalidIdException();

				note.Name = form.Name;
                note.Description = form.Description;
                note.Color = form.ColorNum;

                note = await _noteRepository
                    .Update(note);

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
                    .FirstOrDefaultAsync(n => n.Id == id)
                    ?? throw new InvalidIdException();

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
