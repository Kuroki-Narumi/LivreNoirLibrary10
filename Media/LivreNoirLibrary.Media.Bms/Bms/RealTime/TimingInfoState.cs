using System;

namespace LivreNoirLibrary.Media.Bms
{
    public struct TimingInfoState(double initialTempo)
    {
        public double Tempo { get; set; }
        public double Stop { get; set; }
        public double Scroll { get; set; }
        public double Speed { get; set; }

        private double _tempo = initialTempo;
        private double _secondsPerBeat = 240 / initialTempo;
        private double _scroll = 1;
        private double _speed = 1;

        private double _previousTime;
        private double _previousBeat;
        private double _previousPosition;

        private double _beat;
        private double _time;
        private double _position;

        public readonly double CurrentTempo => _tempo;
        public readonly double CurrentBeat => _beat;
        public readonly double CurrentTime => _time;
        public readonly double CurrentPosition => _position;

        public double Setup(double beat)
        {
            _beat = beat;
            _time = _previousTime + (beat - _previousBeat) * _secondsPerBeat;
            _position = _previousPosition + (beat - _previousBeat) * _scroll;
            Tempo = double.NaN;
            Stop = 0;
            Scroll = double.NaN;
            Speed = double.NaN;
            return _time;
        }

        public bool Update(Note note)
        {
            switch (note.Channel)
            {
                case Channel.Bpm:
                    Tempo = note.Value;
                    return true;
                case Channel.Stop:
                    Stop += note.Value;
                    return true;
                case Channel.Scroll:
                    Scroll = note.Value;
                    return true;
                case Channel.Speed:
                    Speed = note.Value;
                    return true;
            }
            return false;
        }

        public bool Finalize(out TimingInfo info, out bool speedChanged, out double speed)
        {
            var tempo = Tempo;
            var tempoChanged = double.IsFinite(tempo) && tempo != _tempo;
            if (tempoChanged)
            {
                _tempo = tempo;
                _secondsPerBeat = 240 / tempo;
            }
            else
            {
                tempo = _tempo;
            }

            var stopTime = Stop * BmsConstants.StopUnit * _secondsPerBeat;

            var scroll = Scroll;
            var scrollChanged = double.IsFinite(scroll) && scroll != _scroll;
            if (scrollChanged)
            {
                _scroll = scroll;
            }
            else
            {
                scroll = _scroll;
            }

            speed = Speed;
            speedChanged = double.IsFinite(speed) && speed != _speed;
            if (speedChanged)
            {
                _speed = speed;
            }
            else
            {
                speed = _speed;
            }

            if (tempoChanged || stopTime is not 0 || scrollChanged || speedChanged)
            {
                var bps = tempo / 240;
                info = new(_beat, _time, _position, tempo, stopTime, scroll, 1 / bps, bps);
                _previousBeat = _beat;
                _previousTime = _time + stopTime;
                _previousPosition = _position;
                return true;
            }
            else
            {
                info = default;
                return false;
            }
        }
    }
}
