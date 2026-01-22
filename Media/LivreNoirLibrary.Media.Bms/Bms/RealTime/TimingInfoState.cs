using System;

namespace LivreNoirLibrary.Media.Bms
{
    public struct TimingInfoState(double initialTempo)
    {
        public double CurrentTempo { get; private set; } = initialTempo;
        public double CurrentScroll { get; private set; } = 1;
        public double CurrentSpeed { get; private set; } = 1;

        public double CurrentBeat { get; private set; }
        public double CurrentTime { get; private set; }
        public double CurrentPosition { get; private set; }

        public double NewTempo { get; private set; }
        public double NewStop { get; private set; }
        public double NewScroll { get; private set; }
        public double NewSpeed { get; private set; }

        public double FirstTime { get; private set; } = double.NaN;
        public double LastTime { get; private set; } = 0;

        private double _secondsPerBeat = 240 / initialTempo;
        private double _previousTime;
        private double _previousBeat;
        private double _previousPosition;

        public double Setup(double beat)
        {
            CurrentBeat = beat;
            CurrentTime = _previousTime + (beat - _previousBeat) * _secondsPerBeat;
            CurrentPosition = _previousPosition + (beat - _previousBeat) * CurrentScroll;
            NewTempo = double.NaN;
            NewStop = 0;
            NewScroll = double.NaN;
            NewSpeed = double.NaN;
            return CurrentTime;
        }

        public void UpdateFirstTime()
        {
            var time = CurrentTime;
            var first = FirstTime;
            if (double.IsNaN(first) || time < first)
            {
                FirstTime = time;
            }
        }

        public void UpdateLastTime()
        {
            LastTime = Math.Max(CurrentTime, LastTime);
        }

        public bool Update(Note note)
        {
            switch (note.Channel)
            {
                case Channel.Bpm:
                    NewTempo = note.Value;
                    return true;
                case Channel.Stop:
                    NewStop += note.Value;
                    return true;
                case Channel.Scroll:
                    NewScroll = note.Value;
                    return true;
                case Channel.Speed:
                    NewSpeed = note.Value;
                    return true;
            }
            return false;
        }

        public (bool IsTempoChanged, TimingInfo Info, bool IsSpeedChanged, double Speed) Finalize()
        {
            var tempo = NewTempo;
            var tempoChanged = double.IsFinite(tempo) && tempo != CurrentTempo;
            if (tempoChanged)
            {
                CurrentTempo = tempo;
                _secondsPerBeat = 240 / tempo;
            }
            else
            {
                tempo = CurrentTempo;
            }

            var stopTime = NewStop * BmsConstants.StopUnit * _secondsPerBeat;
            tempoChanged |= stopTime is not 0;

            var scroll = NewScroll;
            var scrollChanged = double.IsFinite(scroll) && scroll != CurrentScroll;
            if (scrollChanged)
            {
                CurrentScroll = scroll;
            }
            else
            {
                scroll = CurrentScroll;
            }

            var speed = NewSpeed;
            var speedChanged = double.IsFinite(speed) && speed != CurrentSpeed;
            if (speedChanged)
            {
                CurrentSpeed = speed;
            }
            else
            {
                speed = CurrentSpeed;
            }

            if (tempoChanged || scrollChanged || speedChanged)
            {
                var bps = tempo / 240;
                var beat = CurrentBeat;
                var time = CurrentTime;
                var position = CurrentPosition;
                TimingInfo info = new(beat, time, position, tempo, stopTime, scroll, 1 / bps, bps);
                _previousBeat = beat;
                _previousTime = time + stopTime;
                _previousPosition = position;
                return (tempoChanged, info, speedChanged, speed);
            }
            else
            {
                return default;
            }
        }
    }
}