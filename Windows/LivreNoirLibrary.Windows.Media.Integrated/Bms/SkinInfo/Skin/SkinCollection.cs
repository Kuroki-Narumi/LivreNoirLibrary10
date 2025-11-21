using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class SkinCollection : ObservableObjectBase, IClear
    {
        private static readonly ParserContext _ctx;

        static SkinCollection()
        {
            ParserContext ctx = new();
            var dic = ctx.XmlnsDictionary;
            dic.Add("", "clr-namespace:LivreNoirLibrary.Windows.Media.Bms.SkinInfo;assembly=LivreNoirLibrary.Windows.Media.Integrated");
            dic.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            dic.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
            dic.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            _ctx = ctx;
        }

        private string _directory = "";
        private readonly Dictionary<string, Skin> _skins = [];

        public string RootDirectory
        {
            get => _directory;
            set
            {
                if (value != _directory)
                {
                    Load(value);
                }
            }
        }

        public PlaySkinCollection PlaySkins { get; } = new();

        public void Clear()
        {
            _skins.Clear();
            PlaySkins.Clear();
        }

        public void Load(string directory)
        {
            Clear();
            var skins = _skins;
            var playSkins = PlaySkins;
            _directory = directory;
            SendPropertyChanged(nameof(RootDirectory));
            if (Directory.Exists(directory))
            {
                var ctx = _ctx;
                Dictionary<string, SortingSkinNode?> sortingNodes = [];
                Queue<SortingSkinNode> queue = [];
                SortingSkinNode? GetNode(string path)
                {
                    if (!sortingNodes.TryGetValue(path, out var node))
                    {
                        try
                        {
                            using var file = File.OpenRead(path);
                            if (XamlReader.Load(file, ctx) is Skin skin)
                            {
                                var dir = Path.GetDirectoryName(path)!;
                                node = new(skin, dir, path);
                                sortingNodes.Add(path, node);
                                // 依存関係の整理
                                foreach (var refer in skin.Includes.AsSpan())
                                {
                                    var refPath = Path.GetFullPath(refer, dir);
                                    var refNode = GetNode(refPath);
                                    refNode?.Outgoing.Add(node);
                                }
                                // 何にも依存しないノードはキューに追加しておく
                                if (node.Indegree is 0)
                                {
                                    queue.Enqueue(node);
                                }
                                return node;
                            }
                        }
                        catch { }
                        sortingNodes.Add(path, null);
                    }
                    return node;
                }
                foreach (var path in Directory.EnumerateFiles(directory, "*.xaml", SearchOption.AllDirectories))
                {
                    GetNode(path);
                }
                // 入り数0のノードが検出できなくなるまでループ
                while (queue.Count is > 0)
                {
                    var current = queue.Dequeue();
                    var skin = current.Skin;
                    skins.Add(current.FullPath, skin);
                    // 現在のノードの依存関係を解決
                    skin.Refresh(current.Directory, skins);
                    // このノードを参照するノードの入り数を減らす
                    foreach (var neighbor in current.Outgoing)
                    {
                        neighbor.Indegree--;
                        if (neighbor.Indegree is 0)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                    // スキン種別ごとのリストに追加
                    switch (skin)
                    {
                        case PlaySkin p:
                            playSkins.Add(p);
                            break;
                    }
                }
            }
        }

        private class SortingSkinNode(Skin skin, string directory, string fullPath)
        {
            public Skin Skin { get; } = skin;
            public string Directory { get; } = directory;
            public string FullPath { get; } = fullPath;
            public int Indegree { get; set; } = skin.Includes.Count;
            public List<SortingSkinNode> Outgoing { get; } = [];
        }
    }
}
