using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        // GET: api/<BooksController>
        [HttpGet]
        public async Task<IEnumerable<Book>> Get()
        {
            return await _bookService.GetAllBooks();
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookRequestModel value)
        {
            try
            {
                bool success = await _bookService.AddBook(value);

                if (!success)
                {
                    return StatusCode(500, "No se pudo crear el libro");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // PUT api/<BooksController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BookRequestModel value)
        {
            bool success = await _bookService.UpdateBook(value);

            if (!success)
            {
                return StatusCode(500, "No se pudo actualizar el libro");
            }

            return Ok();
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            bool success = await _bookService.DeleteBook(book);

            if (!success)
            {
                return StatusCode(500, "No se pudo eliminar el libro");
            }

            return Ok();
        }
    }
}
