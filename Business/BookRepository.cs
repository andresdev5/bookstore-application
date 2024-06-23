using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BookRepository : BaseRepository<Book>
    {
        public override bool Update(Book entity)
        {
            List<Genre> genres = [.. entity.Genres];
            var bookDB = Build(["Genres"]).Single(b => b.Id == entity.Id);

            bookDB.ISBN = entity.ISBN;
            bookDB.Publisher = entity.Publisher;
            bookDB.Year = entity.Year;
            bookDB.Price = entity.Price;
            bookDB.Title = entity.Title;
            bookDB.Image = entity.Image;
            bookDB.Author = Repository.Context.Authors.Find(entity.AuthorId);

            bookDB.Genres.Clear();

            foreach (Genre genre in genres)
            {
                if (bookDB.Genres.FirstOrDefault(g => g.Id == genre.Id) == null)
                {
                    var genreDB = Repository.Context.Genres.Find(genre.Id);

                    if (genreDB != null)
                    {
                        bookDB.Genres.Add(genreDB);
                    }
                }
                else
                {
                    bookDB.Genres.Remove(genre);
                }
            }

            Repository.Context.Attach(bookDB);
            Repository.Context.Entry(bookDB).State = EntityState.Modified;
            return Repository.Context.SaveChanges() > 0;
        }
    }
}
