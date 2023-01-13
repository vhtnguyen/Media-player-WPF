using Microsoft.Win32;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //video đang chơi
        public Video _currentPlaying = new Video();

        //video cuối cùng trước khi tắt
        public Video _recentPlaying = new Video();

        //true->đang chơi video; false->không có vieo nào đang chơi
        private bool _playing = false;

        //Playlist đang chơi
        public Playlist _CurPlaylist = new Playlist();
        //lưu playlist không shuffle
        public Playlist playlists_noshuffle = new Playlist();
        //Playlist cuối cùng trước khi tắt
        Playlist recentPlaylist = new Playlist();

        //index của Playlist đang chơi: -1 -> không có Playlist đang chơi
        public int _IndexCurPlaylist = -1;

        //timer
        DispatcherTimer _timer;
        //hook bàn phím
        private IKeyboardMouseEvents _hook;

        //Danh sách các playlist hiện tại
        ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();

        //đường dẫn lưu Playlist
        string PlaylistsPath = AppDomain.CurrentDomain.BaseDirectory + "Playlists";
        //đường dẫn lưu Data khi close window
        string DataPath = AppDomain.CurrentDomain.BaseDirectory + "Data";

        //filter openfile: các định dạng media
        const string videoExtension = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;" +
            "*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;" +
            "*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;" +
            "*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";

     

        bool _isShuffle = false;
        int _preIndex = -1;
        public MainWindow()
        {
            InitializeComponent();                       
        }

        //Window Loaded: Load playlist đã lưu + Recent playlist + Khởi tạo hook event + Tạo folder Playlist/Data + Load recent play files
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(PlaylistsPath);
            Directory.CreateDirectory(DataPath);

            _currentPlaying.VideoUrl = "";

            LoadPlaylist();
            LoadRecentPlays();

            home_click(this, new RoutedEventArgs());

            //Hooks            
            _hook = Hook.GlobalEvents();
            _hook.KeyUp += _hook_KeyUp;
        }

        //Window Closing: delete hook event + store recetn play files
        private void MainWindow_CLosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _hook.KeyUp -= _hook_KeyUp;
            _hook.Dispose();

            recentPlaylist = _CurPlaylist;
            SaveReccentPlays();

            Environment.Exit(0);
        }
                
        //delegate/event ChangCurPlaying: thay đổi CurrentPlaying
        private void change_curPlaying(Video newVideo)
        {
            _currentPlaying = (Video)newVideo.Clone();
         
        }

        //delegate/event ChangPlaylist: thay đổi Curplaylist
        private void change_Playlist(Playlist newPlaylist)
        {
            _CurPlaylist = (Playlist)newPlaylist.Clone();

        }

        //hiển thị page Playlist: xem danh sách playlist
        private void playlist_click(object sender, RoutedEventArgs e)
        {
            listPlaylist.ItemsSource = playlists;

            HomePage.Visibility = Visibility.Collapsed;
            PlaylistPage.Visibility = Visibility.Visible;
            PlaylistInforPage.Visibility = Visibility.Collapsed;
            PlayqueuePage.Visibility = Visibility.Collapsed;

            playlistButtonName.FontWeight = FontWeights.Bold;
            homeButtonName.FontWeight = FontWeights.Normal;
            playqueueButtonName.FontWeight= FontWeights.Normal;
        }

        //hiển thị page Home: video đang chạy + recent playlist
        private void home_click(object sender, RoutedEventArgs e)
        {
            HomePage.DataContext = _currentPlaying;
            if (recentPlaylist.VideoList.Count>0)
            {
                listrecentplay.ItemsSource = recentPlaylist.VideoList;
            }

            HomePage.Visibility = Visibility.Visible;
            PlaylistPage.Visibility = Visibility.Collapsed;
            PlaylistInforPage.Visibility = Visibility.Collapsed;
            PlayqueuePage.Visibility = Visibility.Collapsed;

            playlistButtonName.FontWeight = FontWeights.Normal;
            homeButtonName.FontWeight = FontWeights.Bold;
            playqueueButtonName.FontWeight = FontWeights.Normal;
        }

        //hiển thị page playqueue: danh sách các video trong playlist hiện tại
        private void playqueue_click(object sender, RoutedEventArgs e)
        {
            HomePage.Visibility = Visibility.Collapsed;
            PlaylistPage.Visibility = Visibility.Collapsed;
            PlaylistInforPage.Visibility = Visibility.Collapsed;
            PlayqueuePage.Visibility = Visibility.Visible;

            playlistButtonName.FontWeight = FontWeights.Normal;
            homeButtonName.FontWeight = FontWeights.Normal;
            playqueueButtonName.FontWeight = FontWeights.Bold;

            if (_playing == true && _IndexCurPlaylist > -1)
            {
                listPlayqueue.ItemsSource = _CurPlaylist.VideoList;
                PlayqueuePage.DataContext = _CurPlaylist;
            }
            else
            {
                listPlayqueue.ItemsSource = null;
                PlayqueuePage.DataContext = null;
            }
 
            playlists_noshuffle = _CurPlaylist;
        }

        //Tiếp tục phát khi nhấn Now Playing
        private void playing_click(object sender, RoutedEventArgs e)
        {
            if (_currentPlaying.VideoUrl != "")
            {
               
                _currentPlaying.hour = videoplayer.Position.Hours;
                _currentPlaying.minute = videoplayer.Position.Minutes;
                _currentPlaying.second = videoplayer.Position.Seconds+0.5;
               

                _playing = true;
                _currentPlaying.isplaying = true;
                _hook.KeyUp -= _hook_KeyUp;
                _hook.Dispose();
                _hook = Hook.GlobalEvents();
                _hook.KeyUp += _hook_KeyUp;
                player_MediaOpened(this, new RoutedEventArgs());
                //home_click(this, new RoutedEventArgs());
                //this.Show();
                playqueue_click(this, new RoutedEventArgs());
            }

        }

        //nhấn Add: thêm playlist
        private void addPlaylist_click(object sender, RoutedEventArgs e)
        {
            var screen = new AddPlaylist();
            bool check = true;
            if (screen.ShowDialog() == true)
            {
                var playlist = screen.NewPlaylist;
                if (playlist.PlaylistName!="")
                {
                    foreach (var p in playlists)
                    {
                        if (p.PlaylistName == playlist.PlaylistName)
                        {
                            check = false;                            
                        }
                    }
                    if (check)
                    {
                        playlist.isShufle = false;
                        playlists.Add(playlist);
                    }    
                }
            }
            else
            {
                
            }
        }

        //xoá playlist: nhấn chuột phải lên playlist -> delete
        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (playlists.Count != 0)
            {
                int i = listPlaylist.SelectedIndex;
                if (i==_IndexCurPlaylist)
                {
                    _IndexCurPlaylist = -1;
                    _currentPlaying.VideoUrl = "";
                }

                string filename = PlaylistsPath + "//" + playlists[i].PlaylistName + ".playlist";
               
                playlists.RemoveAt(i);

                try
                {
                    // Check if file exists with its full path    
                    if (File.Exists(filename))
                    {
                        // If file found, delete it    
                        File.Delete(filename);
                        Console.WriteLine("File deleted.");
                    }
                    else Console.WriteLine("File not found");
                }
                catch (IOException ioExp)
                {
                    Console.WriteLine(ioExp.Message);
                }
            }    

        }

        //nhấn view playlist: xem thông tin playlist
        private void viewPlaylist_click(object sender, RoutedEventArgs e)
        {
            var index = listPlaylist.SelectedIndex;

            if (index>=0 && index<playlists.Count)
            {
                HomePage.Visibility = Visibility.Collapsed;
                PlaylistPage.Visibility = Visibility.Collapsed;
                PlaylistInforPage.Visibility = Visibility.Visible;
                PlayqueuePage.Visibility = Visibility.Collapsed;

                playlistButtonName.FontWeight = FontWeights.Normal;
                homeButtonName.FontWeight = FontWeights.Bold;
                playqueueButtonName.FontWeight = FontWeights.Normal;

                _CurPlaylist = playlists[index];
                _IndexCurPlaylist = index;
                PlaylistInforPage.DataContext = _CurPlaylist;
                listVideo.ItemsSource = playlists[index].VideoList;
            }                
        }

        //nhấn Play: play playlist đang xem
        private void playPlaylist_click(object sender, RoutedEventArgs e)
        {
            int i = listPlaylist.SelectedIndex;
            _playing = false;
            if (playlists[i].VideoList.Count>0 && i>=0)
            {
                HomePage.Visibility = Visibility.Collapsed;
                PlaylistPage.Visibility = Visibility.Collapsed;
                PlaylistInforPage.Visibility = Visibility.Collapsed;
                PlayqueuePage.Visibility = Visibility.Visible;
                videoplayer.Visibility = Visibility.Visible;
                mp3bg.Visibility = Visibility.Collapsed;


                _CurPlaylist =new Playlist();
                _CurPlaylist = (Playlist)playlists[i].Clone();
                _IndexCurPlaylist = i;
                _currentPlaying = (Video)_CurPlaylist.VideoList[0].Clone();
                _playing = true;
                _currentPlaying.isplaying = true;
                _hook.KeyUp -= _hook_KeyUp;
                _hook.Dispose();
                _hook = Hook.GlobalEvents();
                _hook.KeyUp += _hook_KeyUp;
                player_MediaOpened(this, new RoutedEventArgs());
                //home_click(this, new RoutedEventArgs());
                //this.Show();
                playqueue_click(this, new RoutedEventArgs());
            }                
        }

        //nhấn Addto: thêm video vào playlist
        private void addSong_click(object sender, RoutedEventArgs e)//chưa sửa code
        {
            int i = listPlaylist.SelectedIndex;
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = videoExtension;
            screen.Multiselect = true;
            if (screen.ShowDialog() == true)
            {
                foreach (var filename in screen.FileNames)
                {
                    Video video = new Video();
                    video.VideoUrl = filename;
                    playlists[i].VideoList.Add(video);
                }
            }
        }

        //xoá video: nhấn chuột phải lên video -> remove
        private void deleteVideo_Click(object sender, RoutedEventArgs e)
        {
            int i = listPlaylist.SelectedIndex;
            int index = listVideo.SelectedIndex;
            playlists[i].VideoList.RemoveAt(index);
            if (i==_IndexCurPlaylist)
            {
                _CurPlaylist=playlists[i];
                _currentPlaying.playlist_index = -1;
            }    
        }

        //nhấn Delete: Xoá playlist đang xem
        private void deletePlaylist_click(object sender, RoutedEventArgs e)
        {
            int i = listPlaylist.SelectedIndex;
            if (i==_IndexCurPlaylist)
            {
                _IndexCurPlaylist = -1;
                _currentPlaying.VideoUrl = "";
            }
           

            string filename = PlaylistsPath + "//" + playlists[i].PlaylistName + ".playlist";

            playlists.RemoveAt(i);

            try
            {
                // Check if file exists with its full path    
                if (File.Exists(filename))
                {
                    // If file found, delete it    
                    File.Delete(filename);
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        
            playlist_click(this, new RoutedEventArgs());
        }

        //nhấn save playlist: lưu + hiển thị thông báo thành công
        private void savePlaylist_click(object sender, RoutedEventArgs e)
        {
            int i = listPlaylist.SelectedIndex;
            SavePlaylist(playlists[i]);
            System.Windows.MessageBox.Show("Saved");
        }

        //Lưu 1 playlist vào máy: định dạng file *.Playlist
        private void SavePlaylist(Playlist p)
        {            
            string filename = PlaylistsPath + "//" + p.PlaylistName + ".playlist";
            var writer = new StreamWriter(filename);
            foreach (var video in p.VideoList)
            {
                writer.WriteLine(video.VideoUrl);
            }
            writer.Close();            
        }

        //Load các playlist có trong máy
        private void LoadPlaylist()
        {
            string[] lists = Directory.GetFiles(PlaylistsPath, "*.playlist");
            foreach (var item in lists)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(item);
                Playlist p = new Playlist(name);

                string[] videos = File.ReadAllLines(item);
                foreach (var video in videos)
                {
                    Video v = new Video(video);
                    p.VideoList.Add(v);
                }
                playlists.Add(p);
            }
        }

        //lưu CurrentPlaylist -> Recent
        private void SaveReccentPlays()
        {
        
            string filename = DataPath + "//" + "data.dat";
                var writer = new StreamWriter(filename);
                writer.WriteLine(_IndexCurPlaylist);
                if (_currentPlaying.VideoUrl=="")
                {
                    writer.WriteLine(0);
                }
                else
                {
                    writer.WriteLine(1);
                    writer.WriteLine(videoplayer.Position.Hours.ToString());
                    writer.WriteLine(videoplayer.Position.Minutes.ToString());
                    writer.WriteLine(videoplayer.Position.Seconds.ToString());
                }
                foreach (var video in recentPlaylist.VideoList)
                {
                    writer.WriteLine(video.VideoUrl);
                }
                    writer.WriteLine(_currentPlaying.VideoUrl);
                    writer.WriteLine(_currentPlaying.playlist_index);
            writer.Close();

        }

        //load Recent Play files
        private void LoadRecentPlays()
        {
            var filename = DataPath + "//" + "data.dat";
            try
            {
                string[] lines = File.ReadAllLines(filename);
                if (lines.Length >= 4)
                {
                    recentPlaylist.PlaylistName = "RecentPlaylist";
                    recentPlaylist.VideoList.Clear();
                    _IndexCurPlaylist = Int32.Parse(lines[0]);
                    if (lines[1]=="0")
                    {
                        for (int i =2; i < lines.Length - 2; i++)
                        {
                            if (lines[i] != "" && lines[i] != null)
                            {
                                Video video = new Video(lines[i]);
                                recentPlaylist.VideoList.Add(video);
                            }
                        }
                        _recentPlaying = recentPlaylist.VideoList[0];
                    }
                    if (lines[1]=="1")
                    {
                        for (int i = 5; i < lines.Length - 2; i++)
                        {
                            if (lines[i] != "" && lines[i] != null)
                            {
                                Video video = new Video(lines[i]);
                                recentPlaylist.VideoList.Add(video);
                            }    
                        }
                        _recentPlaying = new Video(lines[lines.Length - 2], Int32.Parse(lines[lines.Length - 1]));
                        int hour = Int32.Parse(lines[2]);
                        int minute = Int32.Parse(lines[3]);
                        int second = Int32.Parse(lines[4]);
                        _recentPlaying.hour = hour;
                        _recentPlaying.minute = minute;
                        _recentPlaying.second = second;
                    }    
                }
            }
            catch
            {
                File.Create(filename);
            }
        }
        private bool isPlayingAudio()
        {
            string fileExt = (_currentPlaying.VideoName.Substring(_currentPlaying.VideoName.Length - 3)).ToUpper();
            return Regex.IsMatch(fileExt, @"((WAV)|(AIF)|(MP3)|(MID))$");
            //return true;
           
        }

        //khi mở file video mới: -> reset timer + slider
        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (_currentPlaying.VideoUrl!="")
            {

                videoplayer.Source = new Uri(_currentPlaying.VideoUrl, UriKind.Absolute);
                _playing = true;
                videoplayer.Volume = 100;

                _timer = new DispatcherTimer();
                _timer.Interval = new TimeSpan(0, 0, 0, 1, 0); 
                _timer.Tick += _timer_Tick;

                //Slider
                progressSlider.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(progressSlider_MouseLeftButtonUp), true);
                progressSlider.ValueChanged += progressSlider_ValueChanged;

                if (videoplayer.NaturalDuration.HasTimeSpan)
                {
                    int hours = videoplayer.NaturalDuration.TimeSpan.Hours;
                    int minutes = videoplayer.NaturalDuration.TimeSpan.Minutes;
                    int seconds = videoplayer.NaturalDuration.TimeSpan.Seconds;
                    totalPosition.Text = $"{hours}:{minutes}:{seconds}";
                    progressSlider.Maximum = videoplayer.NaturalDuration.TimeSpan.TotalSeconds;
                }
                currentPosition.Text = $"{_currentPlaying.hour}:{_currentPlaying.minute}:{_currentPlaying.second}";
                progressSlider.Value = _currentPlaying.hour * 60 * 60 + _currentPlaying.minute * 60 + _currentPlaying.second;

                videoplayer.Position = TimeSpan.FromSeconds(_currentPlaying.hour * 60 * 60 + _currentPlaying.minute * 60 + _currentPlaying.second);
                videoplayer.Play();
                _timer.Start();

                if (_currentPlaying.isplaying==false)//không nhận diện được vị trí hiện tại
                {
                    //Chổ này làm không tự play sau khi hết bài được nên cmt lại là hết bị

                    //videoplay_click(this, new RoutedEventArgs());
                }      
            }
            else
            {
                progressSlider.Maximum = 0;
                progressSlider.Value = 0;
                currentPosition.Text = "0:0:0";
                totalPosition.Text = "0:0:0";
            }
            videoname.Text =_currentPlaying.VideoName;
            if (isPlayingAudio())
            {
                videoplayer.Visibility = Visibility.Collapsed;
                mp3bg.Visibility = Visibility.Visible;

            }
            else
            {

                videoplayer.Visibility = Visibility.Visible;
                mp3bg.Visibility = Visibility.Collapsed;

            }
            panel_nowplaying.Visibility=Visibility.Visible;
          
            if (_preIndex >= 0)
            {
                _CurPlaylist.VideoList[_preIndex].displayMode = "Hidden";
               
            }
            _CurPlaylist.VideoList[_currentPlaying.playlist_index].displayMode = "Visible";

        }

        //xử lí timer
        private void _timer_Tick(object sender, EventArgs e)
        {
            if (videoplayer.Source != null)
            {
                int hours = videoplayer.Position.Hours;
                int minutes = videoplayer.Position.Minutes;
                int seconds = videoplayer.Position.Seconds;
                var currenpost = videoplayer.Position;

                currentPosition.Text = $"{hours}:{minutes}:{seconds}";
                progressSlider.Value = currenpost.TotalSeconds;

              
               
            }
        }

        //xử lý slider
        private void progressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (videoplayer.Source != null)
            {
                double value = progressSlider.Value;
                TimeSpan newPosition = TimeSpan.FromSeconds(value);
                videoplayer.Position = newPosition;
            }
        }

        //seek silder: nhấn chuột trái lên thanh slider
        private void progressSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (videoplayer.Source != null)
            {
                videoplayer.Position = TimeSpan.FromSeconds(progressSlider.Value);
            }
        }

        //khi kết thúc video -> chạy video tiếp theo
        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            _currentPlaying.isplaying = false;
            if (_currentPlaying.playlist_index >= _CurPlaylist.VideoList.Count - 1)
            {
                videoplayer.Stop();
            }
            else
            {
                videoplayer.Stop();
                _currentPlaying.VideoUrl = _CurPlaylist.VideoList[_currentPlaying.playlist_index + 1].VideoUrl;
                _preIndex = _currentPlaying.playlist_index;
                _currentPlaying.playlist_index += 1;
                videoplayer.Source = new Uri(_currentPlaying.VideoUrl, UriKind.Absolute);
                _playing = true;
                videoplayer.Volume = 100;
                videoname.Text = _currentPlaying.VideoName;
                
            }
        }

        //nhấn previous
        private void videoprev_click(object sender, RoutedEventArgs e)
        {
            if (_currentPlaying.VideoUrl!="")
            {
                if (_currentPlaying.playlist_index > 0)
                {
                    videoplayer.Stop();
                    _currentPlaying.VideoUrl = _CurPlaylist.VideoList[_currentPlaying.playlist_index - 1].VideoUrl;
                    _preIndex = _currentPlaying.playlist_index;
                    _currentPlaying.playlist_index -= 1;

                    videoplayer.Source = new Uri(_currentPlaying.VideoUrl, UriKind.Absolute);
                    _playing = true;
                    videoplayer.Volume = 100;
                }
            }                
        }

        //nhấn pause/play
        private void videoplay_click(object sender, RoutedEventArgs e)
        {
            if(_currentPlaying.VideoUrl!="")
            {
                if (_playing)
                {
                    _playing = false;
                    videoplayer.Pause();
                    _timer.Stop();

                    playvideoButtonText.Text = "Play";
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"/img/play.png", UriKind.Relative);
                    bitmap.EndInit();
                    playvideoButtonIcon.Source = bitmap;
                }
                else
                {
                    _playing = true;
                    videoplayer.Play();
                    _timer.Start();

                    playvideoButtonText.Text = "Pause";
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"/img/pause.png", UriKind.Relative);
                    bitmap.EndInit();
                    playvideoButtonIcon.Source = bitmap;
                }
            }                
        }

        //nhấn stop
        private void videostop_click(object sender, RoutedEventArgs e)
        {
            if(_currentPlaying.VideoUrl!="")
            {
                videoplayer.Stop();
                _playing = false;

                playvideoButtonText.Text = "Play";
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(@"/img/play.png", UriKind.Relative);
                bitmap.EndInit();
                playvideoButtonIcon.Source = bitmap;
            }                
        }

        //nhấn next
        private void videonext_click(object sender, RoutedEventArgs e)
        {
            if (_currentPlaying.VideoUrl != "")
            {
                if (_currentPlaying.playlist_index < _CurPlaylist.VideoList.Count - 1)
                {
                    videoplayer.Stop();
                    _currentPlaying.VideoUrl = _CurPlaylist.VideoList[_currentPlaying.playlist_index + 1].VideoUrl;
                    _preIndex = _currentPlaying.playlist_index;
                    _currentPlaying.playlist_index += 1;

                    videoplayer.Source = new Uri(_currentPlaying.VideoUrl, UriKind.Absolute);
                    _playing = true;
                    videoplayer.Volume = 100;
                }
            }                    
        }

        //xử lý hook bàn phím
        private void _hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.Alt && (e.KeyCode == Keys.N))
            {
                //Ctrl + Alt + N --> Next video
                videonext_click(this, new RoutedEventArgs());
            }

            if (e.Control && e.Alt && (e.KeyCode == Keys.P))
            {
                //Ctrl + Alt + P --> Previous video
                videoprev_click(this, new RoutedEventArgs());
            }

            if (e.Control && e.Alt && (e.KeyCode == Keys.B))
            {
                //Ctrl + Alt + B --> Pause/Play
                videoplay_click(this, new RoutedEventArgs());
            }
        }

        //nhấn recent play: chơi các video đã phát gần nhất
        private void recentplay_click(object sender, RoutedEventArgs e)
        {
            if (_playing == false && recentPlaylist.VideoList.Count > 0)
            {

                HomePage.Visibility = Visibility.Collapsed;
                PlaylistPage.Visibility = Visibility.Collapsed;
                PlaylistInforPage.Visibility = Visibility.Collapsed;
                PlayqueuePage.Visibility = Visibility.Visible;
                videoplayer.Visibility = Visibility.Visible;
                mp3bg.Visibility = Visibility.Collapsed;


               
                

                _CurPlaylist = new Playlist();
                _CurPlaylist = (Playlist)recentPlaylist.Clone();
                _currentPlaying = (Video)_recentPlaying.Clone();

              
                HomePage.Visibility = Visibility.Collapsed;
                PlaylistPage.Visibility = Visibility.Collapsed;
                PlaylistInforPage.Visibility = Visibility.Collapsed;
                PlayqueuePage.Visibility = Visibility.Visible;



                listPlayqueue.ItemsSource = _CurPlaylist.VideoList;
                PlayqueuePage.DataContext = _CurPlaylist;
                playlists_noshuffle = _CurPlaylist;

                _playing = true;
                _currentPlaying.isplaying = true;
                _hook.KeyUp -= _hook_KeyUp;
                _hook.Dispose();
               
                _hook = Hook.GlobalEvents();
                _hook.KeyUp += _hook_KeyUp;
                player_MediaOpened(this, new RoutedEventArgs());
               

                
            }
        }
        //nhấn shuffle: shuffle playlist
        private void videoshuffle_click(object sender, RoutedEventArgs e)
        {
            if (_isShuffle)
            {
                _isShuffle = false;
                _CurPlaylist = playlists_noshuffle;
                shufButtonText.Text = "Shuffle: Off";
                change_Playlist(playlists_noshuffle);
            }
            else
            {
                shufButtonText.Text = "Shuffle: On";
                Random rng = new Random();
                int n = _CurPlaylist.VideoList.Count;
             
                while (n > 0)
                {
                    n--;
                    int k = rng.Next(n + 1);
                 if(k== _currentPlaying.playlist_index)
                    {
                        _currentPlaying.playlist_index = n;
                    }
                    Video value = _CurPlaylist.VideoList[k];
                    _CurPlaylist.VideoList[k] = _CurPlaylist.VideoList[n];
                    _CurPlaylist.VideoList[n] = value;
                }
              
                _isShuffle = true;
                _CurPlaylist.isShufle = true;
                change_Playlist(_CurPlaylist);
            }
            
        }
    }
}
