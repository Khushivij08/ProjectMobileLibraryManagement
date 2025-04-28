///KHUSHI VIJ
///4539042

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace ProjectMobile.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public string Genre { get; set; }


        public bool IsFavorite { get; set; }


        // New properties for reservation
        public bool IsReserved { get; set; } // Track if the book is reserved
        public string ReservedBy { get; set; } // Store the name or user id of the person who reserved it
        public DateTime? ReservationDate { get; set; } // Date of reservation
    }
}
