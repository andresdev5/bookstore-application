using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Services;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController(IGenreService genreService) : ControllerBase
    {
        private readonly IGenreService _genreService = genreService;

        [HttpGet]
        public async Task<IEnumerable<Genre>> Get()
        {
            return await _genreService.GetAllGenres();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _genreService.GetGenreById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreRequestModel genre)
        {
            try
            {
                bool success = await _genreService.AddGenre(genre);

                if (!success)
                {
                    return StatusCode(500, "No se pudo crear el autor");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GenreRequestModel genre)
        {
            bool success = await _genreService.UpdateGenre(genre);

            if (!success)
            {
                return StatusCode(500, "No se pudo actualizar el autor");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreService.GetGenreById(id);

            if (genre == null)
            {
                return NotFound();
            }

            bool success = await _genreService.DeleteGenre(genre);

            if (!success)
            {
                return StatusCode(500, "No se pudo eliminar el autor");
            }

            return Ok();
        }
    }
}
