using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class SimpleBmsViewModel(IBmsData? data = null) : IBmsViewModel
    {
        private bool _isTimeCounterReady;
        private readonly TimeCounter _timeCounter = new();

        public IBmsData Data
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    _isTimeCounterReady = false;
                    BarLengthCache.Clear();
                }
            }
        } = data ?? new BmsData();

        IBmsData IBmsViewModel.Root => Data;
        IBmsDataUnit IBmsViewModel.CurrentData => Data.Root;
        IEnumerable<IBmsDataUnit> IBmsViewModel.EnumerateParents() => [Data.Root];
        IEnumerable<IBmsDataUnit> IBmsViewModel.ReverseEnumerateParents() => [Data.Root];

        public DoubleBarLengthCache BarLengthCache { get; } = new();

        public void InvalidateTimeCounter()
        {
            _isTimeCounterReady = false;
        }

        protected TimeCounter GetTimeCounter()
        {
            if (!_isTimeCounterReady)
            {
                _timeCounter.Load(this);
                _isTimeCounterReady = true;
            }
            return _timeCounter;
        }

        public double MinTempo => GetTimeCounter().MinTempo;
        public double MaxTempo => GetTimeCounter().MaxTempo;
        public double MainTempo => GetTimeCounter().MainTempo;
        public double MainTimeTempo => GetTimeCounter().MainTimeTempo;
        public double Beat2Time(double absolutePosition) => GetTimeCounter().Beat2Time(absolutePosition);
        public double Time2Beat(double time) => GetTimeCounter().Time2Beat(time);
        public double GetHighSpeed(double time) => GetTimeCounter().GetHighSpeed(time);
    }
}
