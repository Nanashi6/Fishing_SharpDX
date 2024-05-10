using Fishing_SharpDX.Enums;
using Fishing_SharpDX.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Fishing_SharpDX
{
    public class Fishing
    {
        private bool _isFishing = false;
        public bool IsFishing { get => _isFishing; }
        private Random _rd;

        private float _timePullOut;
        private float _timeGotOff;
        private float _timeWait;
        private float _time;

        private FishingStatus _status;
        public FishingStatus Status { get => _status; }
        private Fish _fish;
        public Fishing(Fish fish)
        {
            _time = 0;
            _rd = new Random();
            _fish = fish;
        }

        public void StartFishing()
        {
            _time = 0;
            _timeWait = _rd.Next(10, 20);
            _timePullOut = _rd.Next(2, 5);
            _timeGotOff = 3f;
            _status = FishingStatus.Expectation;
            _isFishing = true;
        }

        public void CatchingFish(float deltaT)
        {
            _time += deltaT;

            switch (_status)
            {
                case FishingStatus.Expectation:
                    if (_time >= _timeWait)
                    {
                        _status = FishingStatus.Pecks;
                        _time = 0;
                    }
                    break;
                case FishingStatus.Pecks:
                    if (_time >= _timePullOut)
                    {
                        _status = FishingStatus.GotOff;
                        _time = 0;
                    }
                    break;
                case FishingStatus.GotOff:
                    if (_time >= _timeGotOff)
                    {
                        EndFishing();
                        _time = 0;
                    }
                    break;
            }
        }

        public void EndFishing()
        {
            _isFishing = false;
        }

        public Fish CaughtFish()
        {
            _fish.RandomState();
            return _fish;
        }
    }
}
