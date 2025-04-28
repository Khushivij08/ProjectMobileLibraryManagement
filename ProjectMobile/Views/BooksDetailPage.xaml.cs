using ProjectMobile.ViewModels;

namespace ProjectMobile.Views;

public partial class BookDetailsPage : ContentPage
{
    public BookDetailsPage(BookDetailsPageViewmodel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
