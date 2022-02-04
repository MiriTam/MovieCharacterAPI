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

        // GET: api/Franchises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Franchise>>> GetFranchises()
        {
            return await _context.Franchises.ToListAsync();
        }

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

        // PUT: api/Franchises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Franchises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Franchise>> PostFranchise(Franchise franchise)
        {
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFranchise", new { id = franchise.FranchiseId }, franchise);
        }

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

        [HttpGet("{id}/characters")]
        public async Task<IActionResult> GetCharactersInFranchise(int id)
        {
            Franchise franchise = await _context.Franchises
                .Include(f => f.Movies).ThenInclude(m => m.Characters).FirstOrDefaultAsync(f => f.FranchiseId == id);
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


        private bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.FranchiseId == id);
        }
    }
}
