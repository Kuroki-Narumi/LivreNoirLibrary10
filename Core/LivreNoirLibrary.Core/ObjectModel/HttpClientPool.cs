using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public class HttpClientPool
    {
        public static HttpClient Instance { get; } = new();
    }
}
