using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Math_Parser_1._0
{
    public class AudioService
    {

        /// <summary>
        /// Manages audio
        /// </summary>

        private MediaPlayer _player = new();

        public AudioService()
        {
            _player.MediaEnded += OnMediaEnded;

            _player.MediaOpened += OnOpened;
        }
      

        private void OnOpened(object sender, EventArgs e)
        {
            if (_player.NaturalDuration.HasTimeSpan)
            {
                TotalSeconds = _player.NaturalDuration.TimeSpan.TotalSeconds;
            }
            else
            {
                TotalSeconds = 0;
            }

            
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            if (_isLooping)
            {
                _player.Position = TimeSpan.Zero;
                _player.Play();
            }
        }

        public void Load(string path)
        {
            _player.Stop();
            _player.Close();
            //_player.Open(new Uri(path, UriKind.Relative));
            _player.Open(new Uri(Path.GetFullPath(path), UriKind.Absolute));

            _player.Play();
        }

       

        public double TotalSeconds;
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isLooping = true; // можно потом переключать кнопкой

        
        public bool HasTimeSpan()
        {
            if (_player.NaturalDuration.HasTimeSpan)
                return true;
            else
                return false;
        }
      

 

        public void Play() => _player.Play();
        public void Pause() => _player.Pause();
        public void Stop() => _player.Stop(); //mediaPlayer.Dispose();?
        public void Rewind15()
        {

            TimeSpan currentpos = _player.Position;
            TimeSpan stepback = TimeSpan.FromSeconds(15);

            TimeSpan newpos = currentpos - stepback;

            if (newpos < TimeSpan.Zero)
            {
                _player.Position = TimeSpan.Zero;
            }
            else
            {
                _player.Position = newpos;
            }

            
        }
        public void Forward15()
        {
            if (!_player.NaturalDuration.HasTimeSpan)
                return;

            TimeSpan currentpos = _player.Position;
            TimeSpan stepforward = TimeSpan.FromSeconds(15);

            TimeSpan max = _player.NaturalDuration.TimeSpan;


            var newpos = currentpos + stepforward;

            if(newpos> max)
            {
                _player.Position = max;
            }
            else
            {
                _player.Position = newpos;
            }

           
        }

        public void SetPos( double sec)
        {
            _player.Position = TimeSpan.FromSeconds(sec);
        }
        public TimeSpan GetPos()
        {
            return _player.Position;
        }
        public void SetVolume(double volume)
        {
            _player.Volume = volume;
        }
    }
}
