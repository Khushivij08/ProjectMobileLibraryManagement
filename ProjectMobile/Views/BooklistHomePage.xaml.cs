using ProjectMobile.ViewModels;

namespace ProjectMobile.Views
{
    public partial class BooklistHomePage : ContentPage
    {
        private readonly BooklistHomePageViewmodel _viewModel;

        public BooklistHomePage(BooklistHomePageViewmodel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is BooklistHomePageViewmodel vm)
            {
                vm.LoadBookFromDatabaseCommand.Execute(null);
            }
        }

        private void OnGenreChanged(object sender, EventArgs e)
        {
            // Ensure you're calling the method from the ViewModel
            (BindingContext as BooklistHomePageViewmodel)?.FilterBooksByGenre();
        }
    }
}
