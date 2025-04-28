using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectMobile.DataServices;
using ProjectMobile.Models;

namespace ProjectMobile.ViewModels
{
    [QueryProperty(nameof(BookModel), "ViewBookDetails")]
    public partial class BookDetailsPageViewmodel : AddBookBaseViewModel
    {
        [ObservableProperty]
        private Book bookModel;

        private readonly IBookService bookService;

        public BookDetailsPageViewmodel(IBookService bookService)
        {
            this.bookService = bookService;
            Title = "Book Details";
        }

        [RelayCommand]
        private async Task DeleteBook()
        {
            if (BookModel == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Book",
                $"Are you sure you want to delete \"{BookModel.Title}\"?",
                "Yes", "No");

            if (confirm)
            {
                var result = await bookService.DeleteBookAsync(BookModel);

                // Show the toast notification
                CancellationTokenSource cancellationTokenSource = new();
                var toast = Toast.Make(result.Message, ToastDuration.Long, 14);
                await toast.Show(cancellationTokenSource.Token);

                // Navigate back to BooklistHomePage and pass a flag to reload
                var parameters = new Dictionary<string, object>
        {
            { "ReloadBookList", true }
        };

                // Go back to the home page and pass the flag
                await Shell.Current.GoToAsync("..", parameters);
            }
        }

        [RelayCommand]
        private async Task ToggleFavorite()
        {
            if (BookModel == null) return;

            BookModel.IsFavorite = !BookModel.IsFavorite;

            var toast = Toast.Make(
                BookModel.IsFavorite ? "Marked as Favorite" : "Unmarked as Favorite",
                ToastDuration.Short,
                14);
            await toast.Show();
        }

        [RelayCommand]
        private async Task ShareBook()
        {
            if (BookModel == null) return;

            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Title = "Share Book",
                Text = $"Check out this book: {BookModel.Title}\n\n{BookModel.Description}"
            });
        }

        //[RelayCommand]
        //private async Task ReserveBook()
        //{
        //    if (BookModel == null) return;

        //    // Check if the book is already reserved
        //    if (BookModel.IsReserved)
        //    {
        //        var toast = Toast.Make("This book is already reserved!", ToastDuration.Short, 14);
        //        await toast.Show();
        //        return; // Exit if the book is already reserved
        //    }

        //    // Mark the book as reserved
        //    BookModel.IsReserved = true;
        //    BookModel.ReservedBy = "User123"; // Replace with the actual user's name or ID
        //    BookModel.ReservationDate = DateTime.Now;

        //    // Update the book status in the database (assuming you're using a service to save the book)
        //    await bookService.AddOrUpdateBookAsync(BookModel);

        //    // Show a toast notification that the book has been reserved
        //    var reserveToast = Toast.Make("The book has been reserved!", ToastDuration.Short, 14);
        //    await reserveToast.Show();
        //}


        //[RelayCommand]
        //private async Task MarkBookAsAvailable()
        //{
        //    if (BookModel == null || !BookModel.IsReserved) return;

        //    // Mark the book as available again
        //    BookModel.IsReserved = false;
        //    BookModel.ReservedBy = null;
        //    BookModel.ReservationDate = null;

        //    // Update the book in the database
        //    await bookService.AddOrUpdateBookAsync(BookModel);

        //    // Show a toast notification that the book is now available
        //    var availableToast = Toast.Make("The book is now available for checkout!", ToastDuration.Short, 14);
        //    await availableToast.Show();
        //}

    }
}


