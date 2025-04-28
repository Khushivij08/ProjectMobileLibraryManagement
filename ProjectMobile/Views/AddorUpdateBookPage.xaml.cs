using ProjectMobile.ViewModels;

namespace ProjectMobile.Views
{
    public partial class AddOrUpdateBookPage : ContentPage
    {
        public AddOrUpdateBookPage(AddOrUpdateBookPageViewModel addOrUpdateBookPageViewmodel)
        {
            InitializeComponent();
            BindingContext = addOrUpdateBookPageViewmodel;
        }
    }
}
