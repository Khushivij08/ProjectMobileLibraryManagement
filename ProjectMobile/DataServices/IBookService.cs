using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMobile.Models;

namespace ProjectMobile.DataServices
{
    public interface IBookService
    {
        Task<ServiceResponse> AddOrUpdateBookAsync(Book book);
        Task<ServiceResponse> DeleteBookAsync(Book book);
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(int id);
    }
}
