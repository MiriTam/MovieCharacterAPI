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

namespace MovieInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        // GET: api/Franchises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Franchise>>> GetFranchises()
        {
            return await _context.Franchises.ToListAsync();
        }

        /// <summary>
        /// Method fetches the movie from the database with the given id.
        /// If no movie is found, the method returns the Not found error.
        /// </summary>
        /// <param name="id">Movie id.</param>
        /// <returns>Movie with given id.</returns>
        // GET: api/Franchises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Franchise>> GetFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);
            if (franchise == null)
            {
                return NotFound();
            }
            return Ok(franchise);
        }

        /// <summary>
        /// Method updates the movie with the given id using the values
        /// in the given movie object. Returns status bad request if the 
        /// given id does not match id in the new object.
        /// </summary>
        /// <param name="id">Id of movie.</param>
        /// <param name="movie">
        /// Movie object with new values.
        /// </param>
        /// <returns>No content</returns>
        // PUT: api/Franchises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFranchise(int id, Franchise franchise)
        {
            if (id != franchise.FranchiseId)
            {
                return BadRequest();
            }
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
        /// <param name="movie">New franchise object.</param>
        /// <returns>Status created and new movie.</returns>
        // POST: api/Franchises
        [HttpPost]
        public async Task<ActionResult<Franchise>> PostFranchise(Franchise franchise)
        {
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFranchise", new { id = franchise.FranchiseId }, franchise);
        }

        /// <summary>
        /// Method deletes the franchise record with the given id from
        /// the database. If id is not in database, method returns 
        /// status not found.
        /// </summary>
        /// <param name="id">Id of franchise to be deleted.</param>
        /// <returns>Status no content.</returns>
        // DELETE: api/Franchises/5
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
        /// <param name="id">Id of franchise.</param>
        /// <param name="movieIds">Ids of movies.</param>
        /// <returns>List of movies that were added to the franchise.</returns>
        [HttpPut("{id}/movies")]
        public async Task<IActionResult> AddMovieToFranchise(int id, int[] movieIds)
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
            return Ok(franchise.Movies);
        }

        /// <summary>
        /// Method takes the id of a franchise and returns a list of all the 
        /// movies that belong to that franchise.
        /// </summary>
        /// <param name="id">Id of franchise.</param>
        /// <returns>List of movies.</returns>
        [HttpGet("{id}/movies")]
        public async Task<IActionResult> GetMoviesInFranchise(int id)
        {
            Franchise franchise = await _context.Franchises
                .Include(f => f.Movies).FirstOrDefaultAsync(f => f.FranchiseId == id);
            if (franchise == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(franchise.Movies);
            };
        }

        /// <summary>
        /// Method takes the id of a franchise and fetches all characters that are
        /// part of a movie, that are part of that franchise. If the id does not match 
        /// any recorded franchises, the method returns status not found.
        /// </summary>
        /// <param name="id">Id of franchise.</param>
        /// <returns>List of characters.</returns>
        [HttpGet("{id}/characters")]
        public async Task<IActionResult> GetCharactersInFranchise(int id)
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
                    characters.AddRange(movie.Characters);
                }
                return Ok(characters);
            }
        }

        /// <summary>
        /// Method takes in an id and checks if there are any franchises in the 
        /// database with that id.
        /// </summary>
        /// <param name="id">Id of franchise.</param>
        /// <returns>Boolean indicating if franchise is in database or not.</returns>
        private bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.FranchiseId == id);
        }
    }
}
