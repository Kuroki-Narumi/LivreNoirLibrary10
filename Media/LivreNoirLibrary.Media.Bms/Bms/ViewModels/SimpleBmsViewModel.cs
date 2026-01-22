using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class SimpleBmsViewModel(IBmsData? data = null, ITimeCounter? timeCounter = null) : IBmsViewModel
    {
        private BaseData? _random;

        public IBmsData Data
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    _random = null;
                    BarLengthCache.Clear();
                }
            }
        } = data ?? new BmsData();

        IBmsData IBmsViewModel.Root => Data;
        IBmsDataUnit IBmsViewModel.CurrentData => Data.Root;
        IEnumerable<IBmsDataUnit> IBmsViewModel.EnumerateParents() => [Data.Root];
        IEnumerable<IBmsDataUnit> IBmsViewModel.ReverseEnumerateParents() => [Data.Root];

        IListEnumerable<BarPosition, Note> IBmsViewModel.CurrentTimeline => EnsureRandom().Timeline;

        private BaseData EnsureRandom()
        {
            if (_random is null)
            {
                DetermineRandom();
            }
            return _random!;
        }

        public void DetermineRandom()
        {
            if (_random is { } data)
            {
                data.Clear();
            }
            else
            {
                data = new();
                _random = data;
            }
            Data.DetermineRandom(data, ProvideRandom);
        }

        private int ProvideRandom(int max, string? message = null)
        {
            return Random.Shared.Next(max) + 1;
        }

        public DoubleBarLengthCache BarLengthCache { get; } = new();
        public ITimeCounter TimeCounter { get; } = timeCounter ?? new TimeCounter();
    }
}
