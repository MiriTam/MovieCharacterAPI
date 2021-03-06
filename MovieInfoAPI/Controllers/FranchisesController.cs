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
using MovieInfoAPI.Models.DTO;
using MovieInfoAPI.Models.DTO.Character;
using MovieInfoAPI.Models.DTO.Franchise;
using MovieInfoAPI.Models.DTO.Movie;
using System.Net.Mime;

namespace MovieInfoAPI.Controllers
{
    [Route("api/franchises")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FranchisesController : ControllerBase
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public FranchisesController(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method fetches all franchises in the database.
        /// </summary>
        /// <returns>List of franchises.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetFranchises()
        {
            return _mapper.Map<List<FranchiseReadDTO>>(await _context.Franchises.ToListAsync());
        }

        /// <summary>
        /// Method fetches the movie from the database with the given id.
        /// If no movie is found, the method returns the Not found error.
        /// </summary>
        /// <param name="id">Franchise id</param>
        /// <returns>Movie with given id.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);
            if (franchise == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FranchiseReadDTO>(franchise));
        }

        /// <summary>
        /// Method updates the movie with the given id using the values
        /// in the given movie object. Returns status bad request if the 
        /// given id does not match id in the new object.
        /// </summary>
        /// <param name="id">Franchise id.</param>
        /// <param name="movie">
        /// Movie object with new values.
        /// </param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFranchise(int id, FranchiseEditDTO franchiseDTO)
        {
            if (id != franchiseDTO.FranchiseId)
            {
                return BadRequest();
            }
            Franchise franchise = _mapper.Map<Franchise>(franchiseDTO);
            _context.Entry(franchise).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FranchiseExists(id))
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
        /// Method creates a new franchise record in the database.
        /// </summary>
        /// <param name="movie">New franchise object</param>
        /// <returns>Status created and new movie.</returns>
        [HttpPost]
        public async Task<ActionResult<FranchiseReadDTO>> PostFranchise(FranchiseCreateDTO franchiseDTO)
        {
            Franchise franchise = _mapper.Map<Franchise>(franchiseDTO);
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFranchise", new { id = franchise.FranchiseId }, _mapper.Map<FranchiseReadDTO>(franchise));
        }

        /// <summary>
        /// Method deletes the franchise record with the given id from
        /// the database. If id is not in database, method returns 
        /// status not found.
        /// </summary>
        /// <param name="id">Franchise id</param>
        /// <returns>Status no content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);
            if (franchise == null)
            {
                return NotFound();
            }
            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method takes a franchise id and a set of movie ids of an existing franchise and
        /// existing movies. Then it finds the franchise and movies in the database
        /// and adds the movies to the franchise. If the ids do not match any 
        /// database records, the method returns status code not found.
        /// </summary>
        /// <param name="id">Franchise id</param>
        /// <param name="movieIds">Ids of movies.</param>
        /// <returns>List of movies that were added to the franchise.</returns>
        [HttpPut("{id}/movies")]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> AddMovieToFranchise(int id, int[] movieIds)
        {
            Franchise franchise = await _context.Franchises
                .Include(f => f.Movies).FirstOrDefaultAsync(f => f.FranchiseId == id);
            if (franchise == null)
            {
                return NotFound();
            }
            foreach (int movieId in movieIds)
            {
                Movie movie = _context.Movies.FirstOrDefault(c => c.MovieId == movieId);
                if (movie == null)
                {
                    return NotFound();
                }
                else
                {
                    franchise.Movies.Add(movie);
                }
            }
            return Ok(_mapper.Map<List<MovieReadDTO>>(franchise.Movies));
        }

        /// <summary>
        /// Method takes the id of a franchise and returns a list of all the 
        /// movies that belong to that franchise.
        /// </summary>
        /// <param name="id">Franchise id</param>
        /// <returns>List of movies.</returns>
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetMoviesInFranchise(int id)
        {
            Franchise franchise = await _context.Franchises
                .Include(f => f.Movies).ThenInclude(m => m.Characters).FirstOrDefaultAsync(f => f.FranchiseId == id);
            if (franchise == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<List<MovieReadDTO>>(franchise.Movies));
            };
        }

        /// <summary>
        /// Method takes the id of a franchise and fetches all characters that are
        /// part of a movie, that are part of that franchise. If the id does not match 
        /// any recorded franchises, the method returns status not found.
        /// </summary>
        /// <param name="id">Franchise id</param>
        /// <returns>List of characters.</returns>
        [HttpGet("{id}/characters")]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharactersInFranchise(int id)
        {
            Franchise franchise = await _context.Franchises
                .Include(f => f.Movies).ThenInclude(m => m.Characters)
                .FirstOrDefaultAsync(f => f.FranchiseId == id);
            if (franchise == null)
            {
                return NotFound();
            } else
            {
                List<Movie> movies = franchise.Movies.ToList();
                List<Character> characters = new List<Character>();
                foreach (Movie movie in movies)
                {
                    foreach (Character character in movie.Characters)
                    {
                        if (!characters.Contains(character))
                        {
                            characters.Add(character);
                        }
                    }
                }
                return Ok(_mapper.Map<List<CharacterReadDTO>>(characters));
            }
        }

        /// <summary>
        /// Method takes in an id and checks if there are any franchises in the 
        /// database with that id.
        /// </summary>
        /// <param name="id">Franchise id</param>
        /// <returns>Boolean indicating if franchise is in database or not.</returns>
        private bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.FranchiseId == id);
        }
    }
}
