using GhostCore.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.Components.ViewModels
{
    internal class _temp
    {
    }

    public class SessionViewModel : ViewModelBase
    {
        public ObservableCollection<SongViewModel> Songs { get; set; }

        public SessionViewModel()
        {
            Songs = new ObservableCollection<SongViewModel>
            {
                new SongViewModel("01_straight_up_and_lokka_vox_-_all_about_you_(denis_kenzo_remix)"),
                new SongViewModel("02-gregory_esayan-numb_capsule_(blugazer_remix)"),
                new SongViewModel("03_straight_up_and_lokka_vox_-_all_about_you_(original_mix)"),
                new SongViewModel("04-alan_morris_and_elles_de_graaf-calm_the_night_(ferrin_and_morris_dub)"),
            };
        }
    }

    public class SongViewModel : ViewModelBase
    {
        private string _filename;
        private string _previewImageUri;

        public string PreviewImageUri
        {
            get { return _previewImageUri; }
            set { _previewImageUri = value; OnPropertyChanged(nameof(PreviewImageUri)); }
        }

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; OnPropertyChanged(nameof(Filename)); }
        }

        public SongViewModel(string filename)
        {
            _filename = filename;
        }
    }

    [Flags]
    public enum StemType : byte
    {
        Drums,
        Bass,
        Other,
        Vocals, 
        Piano, 
        Guitar
    }
}
