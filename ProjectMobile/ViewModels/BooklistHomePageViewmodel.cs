using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectMobile.DataServices;
using ProjectMobile.Models;
using ProjectMobile.ViewModels;
using ProjectMobile.Views;

using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectMobile.ViewModels
{
    public partial class BooklistHomePageViewmodel : AddBookBaseViewModel
    {
        [ObservableProperty]
        private bool gridVisibility;

        // New search term property
        [ObservableProperty]
        private string searchTerm;

        [ObservableProperty]
        private string selectedGenre;

        private readonly IBookService bookService;

        // Filtered books collection
        public ObservableCollection<Book> FilteredBooks { get; } = new();

        // Observable collection for genres
        public ObservableCollection<string> Genres { get; } = new ObservableCollection<string>
    {
        "Fiction",
        "Non-Fiction",
        "Science",
        "Fantasy",
        "Mystery",
        "Romance"
        // Add more genres as needed
    };

        public ObservableCollection<Book> Books { get; } = new();

        public BooklistHomePageViewmodel(IBookService bookService)
        {
            this.bookService = bookService;
            Title = "My Book List";

            // Subscribe to the message
            MessagingCenter.Subscribe<BookDetailsPageViewmodel>(this, "ReloadBookList", async (sender) =>
            {
                await LoadBookFromDatabase();
            });
        }

        [RelayCommand]
        private async Task LoadBookFromDatabase()
        {
            GridVisibility = false;
            await Task.Delay(1000);

            var results = await bookService.GetBooksAsync();
            if (results.Any())
            {
                Books.Clear();

                foreach (var book in results)
                {
                    string shortDesc = book.Description?.Length > 30
                        ? book.Description.Substring(0, 30) + "..."
                        : book.Description;

                    Books.Add(new Book
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = shortDesc,
                        Image = book.Image
                    });
                }
            }

            FilterBooks(); // Filter books after loading
            GridVisibility = true;
        }

        [RelayCommand]
        private async Task NavigateToAddBookPage() =>
            await Shell.Current.GoToAsync(nameof(AddOrUpdateBookPage), true);

        [RelayCommand]
        private async Task NavigateToDetails(Book bookModel)
        {
            if (bookModel is null) return;

            var fullBook = await bookService.GetBookAsync(bookModel.Id);
            

            var navParams = new Dictionary<string, object>
            {
                { "ViewBookDetails", fullBook }
            };

            await Shell.Current.GoToAsync(nameof(BookDetailsPage), navParams);
        }

        [RelayCommand]
        private async Task DeleteBookData(Book bookToDelete)
        {
            bool confirm = await Shell.Current.DisplayAlert("Confirm Delete?",
                $"Are you sure you want to delete \"{bookToDelete.Title}\"?", "Yes", "No");

            if (confirm)
            {
                var result = await bookService.DeleteBookAsync(bookToDelete);
                MakeToast(result.Message);
                await LoadBookFromDatabase();
            }
        }

        [RelayCommand]
        private async Task UpdateBookData(Book bookToUpdate)
        {
            bool confirm = await Shell.Current.DisplayAlert("Confirm Update?",
                $"Update \"{bookToUpdate.Title}\"?", "Yes", "No");

            if (confirm)
            {
                var fullBook = await bookService.GetBookAsync(bookToUpdate.Id);

                var navParams = new Dictionary<string, object>
                {
                    { "UpdateBookData", fullBook }
                };

                await Shell.Current.GoToAsync(nameof(AddOrUpdateBookPage), navParams);
            }
        }

        private static async void MakeToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new();
            var toast = Toast.Make(message, ToastDuration.Long, 15);
            await toast.Show(cancellationTokenSource.Token);
        }

        // New method to filter books based on the search term
        [RelayCommand]
        private void FilterBooks()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // If no search term, show all books
                FilteredBooks.Clear();
                foreach (var book in Books)
                {
                    FilteredBooks.Add(book);
                }
            }
            else
            {
                // Filter books based on the search term
                var filtered = Books.Where(b => b.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredBooks.Clear();
                foreach (var book in filtered)
                {
                    FilteredBooks.Add(book);
                }
            }
        }

        [RelayCommand]
        private void OnGenreChanged()
        {
            // Filter books based on the selected genre
            if (!string.IsNullOrEmpty(SelectedGenre))
            {
                FilterBooksByGenre();
            }
        }

        public void FilterBooksByGenre()
        {
            // Add your logic to filter the books by the selected genre
            var filteredBooks = Books.Where(b => b.Genre == SelectedGenre).ToList();
            FilteredBooks.Clear();
            foreach (var book in filteredBooks)
            {
                FilteredBooks.Add(book);
            }
        }

    }
}
