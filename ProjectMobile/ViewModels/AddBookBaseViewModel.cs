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
using System.Diagnostics.CodeAnalysis;

namespace ProjectMobile.ViewModels
{
    public partial class AddBookBaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private Book bookModel;

    }
}
