using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieInfoAPI.Model;
using MovieInfoAPI.Models.Domain;
using MovieInfoAPI.Models.DTO.Character;

namespace MovieInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public CharactersController(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method fetches all characters in the database.
        /// </summary>
        /// <returns>List of characters.</returns>
        // GET: api/Characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharacters()
        {
            return _mapper.Map<List<CharacterReadDTO>>(await _context.Characters.ToListAsync());
        }

        /// <summary>
        /// Method fetches the character from the database with the given id.
        /// If no character is found, the method returns a not found error.
        /// </summary>
        /// <param name="id">Character id.</param>
        /// <returns>Character with given id.</returns>
        // GET: api/Characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterReadDTO>> GetCharacters(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            return _mapper.Map<CharacterReadDTO>(character);
        }

        /// <summary>
        /// Method updates the character with the given id using the values
        /// in the given character object. Returns status bad request if the 
        /// given id does not match id in the new object.
        /// </summary>
        /// <param name="id">Id of character.</param>
        /// <param name="character">
        /// Character object with new values.
        /// </param>
        /// <returns>No content</returns>
        // PUT: api/Characters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, CharacterEditDTO characterDTO)
        {
            if (id != characterDTO.CharacterId)
            {
                return BadRequest();
            }
            // Map DTO input to domain object
            Character character = _mapper.Map<Character>(characterDTO);
            _context.Entry(character).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Method creates a new character record in the database.
        /// </summary>
        /// <param name="character">New character object.</param>
        /// <returns>Status created and new character.</returns>
        // POST: api/Characters
        [HttpPost]
        public async Task<ActionResult<Character>> PostCharacter(CharacterCreateDTO characterDTO)
        {
            Character character = _mapper.Map<Character>(characterDTO);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCharacters), new { id = character.CharacterId }, _mapper.Map<CharacterReadDTO>(character));
        }

        /// <summary>
        /// Method deletes the character record with the given id from
        /// the database. If id is not in database, method returns 
        /// status not found.
        /// </summary>
        /// <param name="id">Id of character to be deleted.</param>
        /// <returns>Status no content.</returns>
        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Method takes in an id and checks if there are any characters in the 
        /// database with that id.
        /// </summary>
        /// <param name="id">Id of character.</param>
        /// <returns>Boolean indicating if character is in database or not.</returns>
        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.CharacterId == id);
        }
    }
}
