using Fracture.Client.Data;
using Fracture.Client.Helpers;
using Fracture.Client.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.TaskBar;

namespace Fracture.Client.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        private static readonly List<string> _allowedExtensions = new List<string> { ".mp3", ".mp4", ".wav", ".flac" };
        private static bool _isCurDirSet = false;
        private Process _curDemucsProcess;
        private FractureFile _curSong;

        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }


        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            pnlDragOverlay.Visibility = Visibility.Visible;
        }

        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            pnlDragOverlay.Visibility = Visibility.Collapsed;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            var filePaths = e.Data.GetData(DataFormats.FileDrop) as string[];

            foreach (var filePath in filePaths)
            {
                var ext = Path.GetExtension(filePath).ToLower();

                if (!_allowedExtensions.Contains(ext))
                    continue;

                ViewModel.Songs.Add(new FractureFile { Name = Path.GetFileName(filePath), Path = filePath });
            }

            pnlDragOverlay.Visibility = Visibility.Collapsed;
        }

        private async void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Songs.Count == 0)
                return;

            btnConvert.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Visible;

            FirstRunHelper.EnsureCurDir();

            TaskBarProgress.SetState(Application.Current.MainWindow, TaskBarProgressState.Normal);

            foreach (var song in ViewModel.Songs)
            {
                if (song.Status == FileStatus.Done)
                    continue;

                _curSong = song;
                song.Status = FileStatus.Processing;

                var psInfo = new ProcessStartInfo
                {
                    FileName = "demucs",
                    Arguments = $"-d cpu \"{song.Path}\" -o \"{ViewModel.OutputPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                using (_curDemucsProcess = new Process())
                {
                    _curDemucsProcess.StartInfo = psInfo;

                    _curDemucsProcess.OutputDataReceived += DemucsProcess_OutputDataReceived;
                    _curDemucsProcess.ErrorDataReceived += DemucsProcess_ErrorDataReceived;

                    _curDemucsProcess.Start();
                    _curDemucsProcess.BeginOutputReadLine();
                    _curDemucsProcess.BeginErrorReadLine();

                    await _curDemucsProcess.WaitForExitAsync();

                    if (_curDemucsProcess.ExitCode != 0)
                        song.Status = FileStatus.None;
                    else
                        song.Status = FileStatus.Done;

                    _curDemucsProcess.OutputDataReceived -= DemucsProcess_OutputDataReceived;
                    _curDemucsProcess.ErrorDataReceived -= DemucsProcess_ErrorDataReceived;
                }

            }

            TaskBarProgress.SetState(Application.Current.MainWindow, TaskBarProgressState.None);
            btnConvert.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Collapsed;
        }

        private void DemucsProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        private void DemucsProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.Contains('%'))
            {
                var split = e.Data.Split('|');
                var percStr = split[0].Replace('%', '\0');
                var value = int.Parse(percStr);
                _curSong.Progress = value;

                Dispatcher.Invoke(() =>
                {
                    TaskBarProgress.SetValue(Application.Current.MainWindow, TaskBarProgressState.Normal, value);
                });

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_curDemucsProcess != null)
            {
                _curDemucsProcess.Kill();
            }
        }
    }
}