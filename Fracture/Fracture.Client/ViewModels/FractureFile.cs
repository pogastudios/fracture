using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Fracture.Client.ViewModels
{
    public partial class FractureFile : ObservableObject
    {
        private FileStatus _status;

        [ObservableProperty]
        private double _progress;

        public string Name { get; set; }
        public string Path { get; internal set; }

        public BitmapSource PreviewImage => ShellFile.FromFilePath(Path).Thumbnail.BitmapSource;
        public Visibility ProgressVisibility => _status == FileStatus.Processing ? Visibility.Visible : Visibility.Collapsed;
        public Visibility CheckmarkVisibility => _status == FileStatus.Done ? Visibility.Visible : Visibility.Collapsed;

        public FileStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(ProgressVisibility));
                OnPropertyChanged(nameof(CheckmarkVisibility));
            }
        }
    }

    public enum FileStatus
    {
        None,
        Processing,
        Done
    }
}
