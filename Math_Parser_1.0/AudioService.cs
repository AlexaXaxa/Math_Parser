using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Math_Parser_1._0
{
    public class AudioService
    {

        private MediaPlayer _player = new();

        public void Load(string path)
        {
            _player.Open(new Uri(path, UriKind.Relative));
            _player.Play();
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

    }
}
