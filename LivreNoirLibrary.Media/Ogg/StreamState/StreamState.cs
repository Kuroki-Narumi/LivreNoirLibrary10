using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Ogg
{
    public abstract class StreamState(int serialNumber) : DisposableBase
    {
        protected int _serial_number = serialNumber;
        protected readonly Queue<OggPage> _pages = [];

        public int SerialNumber { get => _serial_number; set => _serial_number = value; }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            _pages.Clear();
        }

        public virtual void PageIn(in OggPage page) => _pages.Enqueue(page);
        public virtual bool PageOut([MaybeNullWhen(false)] out OggPage page) => _pages.TryDequeue(out page);
    }
}
