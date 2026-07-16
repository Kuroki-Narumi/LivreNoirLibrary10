using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

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
        private readonly SkinRefreshArgs _refreshArgs = new();

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
            _refreshArgs.Clear();
            PlaySkins.Clear();
        }

        public void Load(string directory)
        {
            Clear();
            var args = _refreshArgs;
            var playSkins = PlaySkins;
            _directory = directory;
            this.NotifyPropertyChanged(nameof(RootDirectory));
            if (Directory.Exists(directory))
            {
                var ctx = _ctx;
                Dictionary<string, SortingSkinNode?> sortingNodes = [];
                Queue<SortingSkinNode> queue = [];
                foreach (var path in Directory.EnumerateFiles(directory, "*.xml", SearchOption.AllDirectories))
                {
                    GetNode(path);
                }
                // 入り数0のノードが検出できなくなるまでループ
                while (queue.Count is > 0)
                {
                    var current = queue.Dequeue();
                    var skin = current.Skin;
                    args.RegisterSkin(current.FullPath, skin);
                    // 現在のノードの依存関係を解決
                    skin.Refresh(current.Directory, args);
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

                SortingSkinNode? GetNode(string path)
                {
                    if (!path.EndsWith(Exts.Xml, StringComparison.OrdinalIgnoreCase))
                    {
                        path = Path.ChangeExtension(path, Exts.Xml);
                    }
                    if (!sortingNodes.TryGetValue(path, out var node))
                    {
                        if (File.Exists(path))
                        {
                            using var file = File.OpenRead(path);
                            Skin? skin = null;
                            try
                            {
                                skin = XamlReader.Load(file, ctx) as Skin;
                            }
                            catch (Exception e)
                            {
                                ExConsole.Write(e);
                            }
                            if (skin is not null)
                            {
                                var dir = Path.GetDirectoryName(path)!;
                                node = new(skin, dir, path);
                                sortingNodes.Add(path, node);
                                // 依存関係の整理
                                foreach (var refer in skin.Includes)
                                {
                                    if (refer.Source is { } includePath)
                                    {
                                        var refPath = Path.GetFullPath(includePath, dir);
                                        if (GetNode(refPath) is { } refNode)
                                        {
                                            refNode.Outgoing.Add(node);
                                        }
                                        else
                                        {
                                            node.Indegree--;
                                        }
                                    }
                                }
                                // 何にも依存しないノードはキューに追加しておく
                                if (node.Indegree is 0)
                                {
                                    queue.Enqueue(node);
                                }
                                return node;
                            }
                        }
                        ExConsole.Write($"error: \"{path}\" is not found.");
                        sortingNodes.Add(path, null);
                    }
                    return node;
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
