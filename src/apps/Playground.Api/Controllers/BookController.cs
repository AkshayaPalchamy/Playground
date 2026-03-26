using Microsoft.AspNetCore.Mvc;

namespace Playground.Api;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "book")]
public class BooksController : ControllerBase
{
	private static readonly List<Book> books = new()
	                                           {
		                                           new Book { Id = 1, Title = "API Basics", Author = "Akshaya" },
                                                    new Book { Id = 2, Title = "API Basics", Author = "Akshaya" }
                                               };

	//GET
	[HttpGet]
	public IActionResult GetBooks()
	{
		return Ok(books);
	}


	//POST
	[HttpPost]
	public IActionResult AddBook([FromBody] Book book)
	{
		books.Add(book);
		return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
	}

	//PUT
	[HttpPut("{id}")]
	public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
	{
		Book? book = books.FirstOrDefault(b => b.Id == id);
		if(book == null) return NotFound();
		book.Title = updatedBook.Title;
		book.Author = updatedBook.Author;
		return Ok(book);
	}

	//PATCH
	[HttpPatch("{id}")]
	public IActionResult PatchBook(int id, [FromBody] Book updateBook)
	{
		Book? book = books.FirstOrDefault(b => b.Id == id);
		if(book == null) return NotFound();

		if(!string.IsNullOrEmpty(updateBook.Title))
			book.Title = updateBook.Title;

		if(!string.IsNullOrEmpty(updateBook.Author))
			book.Author = updateBook.Author;
		return Ok(book);
	}

	//DELETE
	[HttpDelete("{id}")]
	public IActionResult DeleteBook(int id)
	{
		Book? book = books.FirstOrDefault(b => b.Id == id);
		if(book == null) return NotFound();
		books.Remove(book);
		return NoContent();
	}
}