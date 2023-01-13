using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    public class Video : INotifyPropertyChanged, ICloneable
    {
        public string VideoUrl { get; set; }
        public string VideoName
        {
            get
            {
                var tokens = VideoUrl.Split(new string[] { "\\" },
                StringSplitOptions.None);
                if (tokens.Length > 1)
                {
                    return tokens[tokens.Length - 1];
                }
                else
                {
                    tokens = VideoUrl.Split(new string[] { "/" }, StringSplitOptions.None);
                    return tokens[tokens.Length - 1];
                }
            }
        }

        public int playlist_index { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public double second { get; set; }

        public bool isplaying { get; set; }

        public string displayMode { get; set; }


        public Video(string value,int idx=0)
        {
            VideoUrl = value;
            isplaying = false;
            hour = 0;
            minute = 0;
            second = 0;
            displayMode = "Hidden";
            playlist_index = idx;


        }

        public Video()
        {
            VideoUrl = "";
            isplaying = false;
            hour = 0;
            minute = 0;
            second = 0;
            displayMode = "Hidden";
            playlist_index = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}

