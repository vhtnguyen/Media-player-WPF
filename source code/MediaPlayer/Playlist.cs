using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    public class Playlist : INotifyPropertyChanged, ICloneable
    {
        public string PlaylistName { get; set; }

        public bool isShufle { get; set; }

        public ObservableCollection<Video> VideoList { get; set; }

        public Playlist(string playlistName)
        {
            PlaylistName = playlistName;
            VideoList = new ObservableCollection<Video>();
        }

        public Playlist()
        {
            PlaylistName = "";
            VideoList=new ObservableCollection<Video>();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
