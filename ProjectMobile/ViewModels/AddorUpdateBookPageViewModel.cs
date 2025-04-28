///KHUSHI VIJ
///4539042

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectMobile.CustomControls;
using ProjectMobile.DataServices;
using ProjectMobile.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProjectMobile.ViewModels;

namespace ProjectMobile.ViewModels
{
    [QueryProperty(nameof(AddBookModel), "UpdateBookData")]
    public partial class AddOrUpdateBookPageViewModel : AddBookBaseViewModel
    {
        // Observable Collection for errors
        public ObservableCollection<Error> Errors { get; set; } = new();

        // Observable properties for Book and errors
        [ObservableProperty]
        private Book _addBookModel;

        [ObservableProperty] 
        private bool _showErrors;

        [ObservableProperty]
        private ImageSource _imageSourceFile;

        // Service for book operations
        private readonly IBookService bookService;

        // Constructor
        public AddOrUpdateBookPageViewModel(IBookService bookService)
        {
            this.bookService = bookService;
            Title = "Add Book Data";
            AddBookModel = new Book();  // Initialize the model
        }

        // Command for selecting an image
        [RelayCommand]
        private async Task SelectImage()
        {
            var image = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select Book Image",
                FileTypes = FilePickerFileType.Images
            });
            if (image == null) return;

            byte[] imageByte;
            var newFile = Path.Combine(FileSystem.CacheDirectory, image.FileName);
            var stream = await image.OpenReadAsync();
            using (MemoryStream memory = new())
            {
                stream.CopyTo(memory);
                imageByte = memory.ToArray();
            }

            // Convert image to base64 string
            var convertedImage = Convert.ToBase64String(imageByte);
            AddBookModel.Image = convertedImage;

            // Convert from base64 to image and set ImageSourceFile
            GetImage(convertedImage);
        }

        // Helper method to convert from base64 string to ImageSource
        private void GetImage(string base64)
        {
            var imgFromBase64 = Convert.FromBase64String(base64);
            MemoryStream memoryStream = new(imgFromBase64);
            ImageSourceFile = ImageSource.FromStream(() => memoryStream);
        }

        // Command for saving data
        [RelayCommand]
        private async Task SaveData()
        {
            Errors.Clear();
            if (!ValidateModel(AddBookModel)) return;

            if (Errors.Any() || Errors.Count > 0)
            {
                ShowErrors = true;
                return;
            }

            var result = await bookService.AddOrUpdateBookAsync(AddBookModel);
            if (result.Flag)
            {
                MakeToast(result.Message);
                AddBookModel = new Book();  // Reset the model
                ImageSourceFile = null;     // Clear image
                return;
            }

            // If there is an error, show it
            Errors.Clear();
            Errors.Add(new Error() { Property = "Alert: ", Value = result.Message });
            ShowErrors = true;
        }

        // Method to validate the book model
        private bool ValidateModel(Book validateBook)
        {
            if (string.IsNullOrEmpty(validateBook.Title))
                Errors.Add(new Error() { Property = "Title: ", Value = "Book Title cannot be empty" });

            if (string.IsNullOrEmpty(validateBook.Description))
            {
                Errors.Add(new Error() { Property = "Description: ", Value = "Book Description cannot be empty" });
            }
            else
            {
                if (validateBook.Description.Length < 20)
                    Errors.Add(new Error() { Property = "Description: ", Value = "Minimum length of description must be 20" });
            }

            if (string.IsNullOrEmpty(validateBook.Image))
                Errors.Add(new Error() { Property = "Image: ", Value = "Book Image cannot be empty" });

            return Errors.Count == 0; // Return true if no errors
        }

        // Helper method for showing toast messages
        private static async void MakeToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ToastDuration duration = ToastDuration.Long;
            double fontSize = 15;
            var toast = Toast.Make(message, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        // Command for navigating to the home page
        [RelayCommand]
        private async Task NavigateToHome() => await Shell.Current.GoToAsync("..", true);
    }
}
