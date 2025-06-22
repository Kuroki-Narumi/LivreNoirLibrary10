using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    internal class HistoryData
    {
        private readonly MemoryStream _data = new();
        internal readonly SelectionHistoryData _selection;
        internal readonly Dictionary<FlowAddress, FlowHistoryData> _flow;

        public HistoryData(BmsViewModel source)
        {
            _data.SetLength(0);
            using (DeflateStream stream = new(_data, CompressionMode.Compress, true))
            {
                source._root.WriteHistoryBuffer(stream);
            }
            _data.Position = 0;
            _selection = new(source._selection);
            _flow = source.CreateFlowHistoryData();
        }

        public void LoadRoot(BmsData root)
        {
            using DeflateStream stream = new(_data, CompressionMode.Decompress, true);
            root.LoadHistoryBuffer(stream);
        }
    }
}
