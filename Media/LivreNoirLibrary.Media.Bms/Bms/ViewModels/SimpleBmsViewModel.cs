using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class SimpleBmsViewModel(IBmsData data) : IBmsViewModel
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
        } = data;

        IBmsData IBmsViewModel.Root => Data;
        IBmsDataUnit IBmsViewModel.CurrentData => Data.Root;
        IEnumerable<IBmsDataUnit> IBmsViewModel.EnumerateParents() => [Data.Root];
        IEnumerable<IBmsDataUnit> IBmsViewModel.ReverseEnumerateParents() => [Data.Root];

        public ITimeline ParentTimeline { get; } = new Timeline();
        public ITimeCounter TimeCounter => GetTimeCounter();
        public DoubleBarLengthCache BarLengthCache { get; } = new();

        public void InvalidateTimeCounter()
        {
            _isTimeCounterReady = false;
        }

        protected ITimeCounter GetTimeCounter()
        {
            if (!_isTimeCounterReady)
            {
                _timeCounter.Load(this);
                _isTimeCounterReady = true;
            }
            return _timeCounter;
        }
    }
}
