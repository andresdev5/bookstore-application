using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Controllers;
using Service.Models;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test.Controllers
{
	public class BooksControllerTest
	{
		[Fact]
		public async Task ShouldGetAllBooks()
		{
			List<Book> books = [
				new Book()
				{
					Id = 1,
					Title = "Test",
				},
				new Book()
				{
					Id = 2,
					Title = "Test",
				}
			];
			var mockService = new Mock<IBookService>();
			mockService.Setup(svc => svc.GetAllBooks()).Returns(Task.Run(() => books));
			var controller = new BooksController(mockService.Object);

			var response = await controller.GetBooks();
			var result = Assert.IsAssignableFrom<IEnumerable<Book>>(response);
			Assert.Equal(2, response.Count());
		}

		[Fact]
		public async Task ShouldGetBookById()
		{
			int id = 1;
			Book book = new()
			{
				Id = id,
				Title = "Test Book"
			};
			var mockService = new Mock<IBookService>();
			mockService.Setup(svc => svc.GetBookById(id)).Returns(Task.FromResult<Book>(book));
			var controller = new BooksController(mockService.Object);

			var response = await controller.GetBook(id);
			var result = Assert.IsType<Book>(response);
			Assert.Equal(id, result.Id);
		}

		[Fact]
		public async Task ShouldAddBook()
		{
			var bookRequestModel = new BookRequestModel
			{
				Title = "New Book",
				Year = 2022,
				Publisher = "Publisher",
				ISBN = "ISBN",
				Price = 19.99m,
				Stock = 10
			};
			var mockService = new Mock<IBookService>();
			mockService.Setup(svc => svc.AddBook(bookRequestModel)).Returns(Task.FromResult(true));
			var controller = new BooksController(mockService.Object);

			var response = await controller.AddBook(bookRequestModel);
			var result = Assert.IsType<OkResult>(response);
		}
	}
}
