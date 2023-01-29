using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common.Interfaces;

namespace Fracture.Client.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        public ObservableCollection<FractureFile> Songs { get; set; } = new ObservableCollection<FractureFile>();

        [ObservableProperty]
        private string _outputPath = @"C:\temp";


        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
