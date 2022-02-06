using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieInfoAPI.Model;
using MovieInfoAPI.Models.Domain;
using MovieInfoAPI.Models.DTO.Movie;
using MovieInfoAPI.Models.DTO.Character;
using System.Linq;

namespace MovieInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method fetches all movies in the database.
        /// </summary>
        /// <returns>List of all movies in the database.</returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetMovies()
        {
            return _mapper.Map<List<MovieReadDTO>>(await _context.Movies.Include(m => m.Characters).ToListAsync());
        }

        /// <summary>
        /// Method fetches the movie from the database with the given id.
        /// If no movie is found, the method returns the Not found error.
        /// </summary>
        /// <param name="id">Id of movie to be fetched.</param>
        /// <returns>Movie with given id.</returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieReadDTO>> GetMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Characters).FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            return _mapper.Map<MovieReadDTO>(movie);
        }

        /// <summary>
        /// Method updates the movie with the given id using the values
        /// in the given movie object. Returns status bad request if the 
        /// given id does not match id in the new object.
        /// </summary>
        /// <param name="id">Id of movie to be updated.</param>
        /// <param name="movie">
        /// Movie object with values used to update the movie in the database.
        /// </param>
        /// <returns>No content</returns>
        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieEditDTO movieDTO)
        {
            if (id != movieDTO.MovieId)
            {
                return BadRequest();
            }
            Movie movie = _mapper.Map<Movie>(movieDTO);
            _context.Entry(movie).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        /// Method creates a new movie record in the database.
        /// </summary>
        /// <param name="movie">Movie object to be stored.</param>
        /// <returns>Status created and the new movie.</returns>
        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieReadDTO>> PostMovie(MovieCreateDTO movieDTO)
        {
            Movie movie = _mapper.Map<Movie>(movieDTO);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMovie", new { id = movie.MovieId }, _mapper.Map<MovieReadDTO>(movie));
        }

        /// <summary>
        /// Method deletes the movie record with the given id from
        /// the database. If id is not in database, method returns 
        /// status not found.
        /// </summary>
        /// <param name="id">Id of movie to be deleted.</param>
        /// <returns>Status no content.</returns>
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method takes a movie id and a set of character ids of an existing movie and
        /// existing characters. Then it finds the movie and characters in the database
        /// and adds the characters to the movie. If the ids do not match any 
        /// database records, the method returns status code not found.
        /// </summary>
        /// <param name="id">Id of movie.</param>
        /// <param name="characterIds">Ids of characters.</param>
        /// <returns>List of characters that were added to the movie.</returns>
        [HttpPut("{id}/characters")]
        public async Task<IActionResult> AddCharactersToMovie(int id, int[] characterIds)
        {
            Movie movie = await _context.Movies
                .Include(m => m.Characters).FirstOrDefaultAsync(m => m.MovieId ==id);
            if (movie == null)
            {
                return NotFound();
            }
            foreach (int characterId in characterIds)
            {
                Character character = _context.Characters.FirstOrDefault(c => c.CharacterId == characterId);
                if (character == null)
                {
                    return NotFound();
                } else
                {
                    movie.Characters.Add(character);
                }
            }
            return Ok(_mapper.Map<List<CharacterReadDTO>>(movie.Characters));
        }

        /// <summary>
        /// Method takes the id of a movie and returns a list of all the 
        /// characters that belong to that movie.
        /// </summary>
        /// <param name="id">Id of movie.</param>
        /// <returns>List of characters.</returns>
        [HttpGet("{id}/characters")]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharactersInMovie(int id)
        {
            Movie movie = await _context.Movies
                .Include(m => m.Characters).FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<List<CharacterReadDTO>>(movie.Characters));
            }
        }

        /// <summary>
        /// Method takes a movie id and checks if a moie with that id
        /// exists in the database.
        /// </summary>
        /// <param name="id">Movie id.</param>
        /// <returns>Boolean indicating if movie is in database or not.</returns>
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
