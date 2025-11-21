using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public class WaveDataCache : IClear
    {
        private readonly Dictionary<string, WaveBuffer?> _dic = [];

        public void Clear()
        {
            foreach (var (_, data) in _dic)
            {
                data?.Dispose();
            }
            _dic.Clear(); 
        }

        public bool Remove(string path)
        {
            if (_dic.Remove(path, out var data))
            {
                data?.Dispose();
                return true;
            }
            return false;
        }

        public WaveBuffer? Get(string path)
        {
            if (_dic.TryGetValue(path, out var data))
            {
                return data;
            }
            if (File.Exists(path))
            {
                try
                {
                    data = WaveBuffer.AutoOpen(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"failed to open \"{path}\"");
                    Console.WriteLine(ex);
                    data = null;
                }
            }
            _dic.Add(path, data);
            return data;
        }
    }
}
