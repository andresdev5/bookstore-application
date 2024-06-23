using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Services;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController(IAuthorService bookService) : ControllerBase
    {
        private readonly IAuthorService _authorService = bookService;

        [HttpGet]
        public async Task<IEnumerable<Author>> Get()
        {
            return await _authorService.GetAllAuthors();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _authorService.GetAuthorById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthorRequestModel author)
        {
            try
            {
                bool success = await _authorService.AddAuthor(author);

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
        public async Task<IActionResult> Put([FromBody] AuthorRequestModel author)
        {
            bool success = await _authorService.UpdateAuthor(author);

            if (!success)
            {
                return StatusCode(500, "No se pudo actualizar el autor");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound();
            }

            bool success = await _authorService.DeleteAuthor(author);

            if (!success)
            {
                return StatusCode(500, "No se pudo eliminar el autor");
            }

            return Ok();
        }
    }
}
