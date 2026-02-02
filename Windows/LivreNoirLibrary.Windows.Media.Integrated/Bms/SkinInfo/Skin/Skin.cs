using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Skin : SkinContainer
    {
        [TypeConverter(typeof(VersionConverter))]
        public Version Version { get; set => SetValue(ref field, value); } = new(1, 0);
        public string? Author { get; set => SetValue(ref field, value); }
        public System.Drawing.Size BaseSize { get; set => SetValue(ref field, value); }
        public LnColor Background { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);
        public ValueExpression FadeInTime { get; set => SetValue(ref field, value); } = 0.5;
        public ValueExpression FadeOutTime { get; set => SetValue(ref field, value); } = 1;

        public IncludeCollection Includes { get; } = [];
        public OptionCollection Options { get; } = [];
        public VariableCollection Variables { get; } = [];
        public TextureCollection Textures { get; } = [];
        public LaneDefinitionCollection LaneDefinitions { get; } = [];

        private string? _directory;

        internal void Refresh(string directory, SkinRefreshArgs args)
        {
            if (_directory is not null)
            {
                throw new StackOverflowException($"a circular dependencie detected (this node is already initialized).");
            }
            _directory = directory;
            var includes = Includes;
            var options = Options;
            var variables = Variables;
            var textures = Textures;
            var lanes = LaneDefinitions;
            var children = Children;
            // include
            if (includes.Count is > 0)
            {
                // このスキンでの設定値を保存
                args.BeginResolveInclude(options, variables, textures, lanes);
                foreach (var include in includes)
                {
                    if (args.TryGetIncludeSource(directory, include, out var parent))
                    {
                        options.AddRange(parent.Options);
                        variables.AddRange(parent.Variables);
                        textures.AddRange(parent.Textures);
                        lanes.AddRange(parent.LaneDefinitions);
                    }
                }
                // このスキンでの設定値で上書きしなおす
                args.RestoreSkinInfo(options, variables, textures, lanes);
                // スキン要素のインクルード解決
                ApplyInclude(this, args);
                args.FinishResolveInclude();
            }
            // texture inheritance
            foreach (var texture in textures)
            {
                texture._baseDirectory ??= directory;
                var baseKey = texture.BasedOn;
                if (!string.IsNullOrEmpty(baseKey) && textures.TryGetValue(baseKey, out var parent))
                {
                    texture._base = parent;
                }
            }
            if (textures.Find(t => t.IsCircularReference()) is { } t)
            {
                throw new StackOverflowException($"a circular reference detected: \"{t.Key}\" based on \"{t.BasedOn}\"");
            }
        }

        private static void ApplyInclude(SkinContainer container, SkinRefreshArgs args)
        {
            var children = container.Children;
            for (var i = 0; i < children.Count;)
            {
                switch (children[i])
                {
                    case SkinContainer c:
                        ApplyInclude(c, args);
                        break;
                    case Include include:
                        // <Include/>要素をインクルード元のスキン要素で置き換えるイメージ
                        children.RemoveAt(i);
                        if (args.TryGetIncludeSource(include.Key, out var skin))
                        {
                            // この時点で参照されるインクルード元のスキンは、既にインクルードを解決済みである
                            foreach (var item in skin.Children.AsSpan())
                            {
                                if (item is SkinElement element)
                                {
                                    children.Insert(i, element);
                                    i++;
                                }
                            }
                        }
                        continue;
                }
                i++;
            }
        }

        public bool TryResolveReflection(ValueExpression? expr, IVariableProvider? provider, [MaybeNullWhen(false)]out string value)
        {
            if (expr is null)
            {
                value = null;
                return false;
            }
            using var o = ObjectPool.Rent<HashSet<string>>();
            var reflected = o.Value;
            return TryResolveReflection(expr, provider, reflected, out value);
        }

        private bool TryResolveReflection(ValueExpression? expr, IVariableProvider? provider, HashSet<string> reflection, [MaybeNullWhen(false)] out string value)
        {
            value = null;
            if (expr is null)
            {
                return false;
            }
            var key = expr.Key;
            if (key is not null)
            {
                switch (expr.Type)
                {
                    case ReflectionType.Options:
                        if (provider is not null && provider.TryGetOption(key, out value))
                        {
                            return true;
                        }
                        if (Options.TryGetValue(key, out var item))
                        {
                            value = item.SelectedValue;
                            goto Return;
                        }
                        ExConsole.Write($"ERROR: $Options.{key} is not found.");
                        break;
                    case ReflectionType.Variables:
                        if (provider is not null && provider.TryGetVariable(key, out value))
                        {
                            return true;
                        }
                        if (reflection.Contains(key))
                        {
                            return false;
                        }
                        reflection.Add(key);
                        if (Variables.TryGetValue(key, out var variable) &&
                            TryResolveReflection(variable.Source, provider, reflection, out var sourceValue))
                        {
                            if (variable.Converters.TryGetValue(sourceValue, out var result))
                            {
                                value = result.To;
                            }
                            else
                            {
                                TryResolveReflection(variable.DefaultValue, provider, reflection, out value);
                            }
                            goto Return;
                        }
                        ExConsole.Write($"ERROR: $Variables.{key} is not found.");
                        break;
                }
            }
            value = expr.Value;
        Return:
            return value is not null;
        }

        public bool TryResolveValue<T>(ValueExpression? expr, IVariableProvider? provider, [MaybeNullWhen(false)]out T value)
            where T : IParsable<T>
        {
            if (TryResolveReflection(expr, provider, out var strValue) &&
                T.TryParse(strValue, null, out value))
            {
                return true;
            }
            value = default;
            return false;
        }

        public T ResolveValue<T>(ValueExpression? expr, IVariableProvider? provider, T defaultValue)
            where T : IParsable<T>
        {
            if (TryResolveValue<T>(expr, provider, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        public bool TryGetTextureData(ValueExpression? expr, IVariableProvider? provider, out TextureData data)
        {
            data = default;
            if (!TryResolveReflection(expr, provider, out var key) || string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (Texture.IsReservedKey(key))
            {
                data = new(key, 0, 0, 0, 0, 1, 1, 0);
                return true;
            }

            if (Textures.TryGetValue(key, out var texture) && TryResolveReflection(texture.Source, provider, out var source))
            {
                source = Path.GetFullPath(source, texture._baseDirectory ?? _directory ?? General.GetAssemblyDir());
                var x = Resolve(texture.X);
                var y = Resolve(texture.Y);
                var w = Resolve(texture.Width);
                var h = Resolve(texture.Height);
                var divX = Math.Max(Resolve(texture.DivX), 1);
                var divY = Math.Max(Resolve(texture.DivY), 1);
                var period = Math.Max(ResolveValue(texture.LoopPeriod, provider, 0d), 0);
                data = new(source, x, y, w, h, divX, divY, period);
                return true;
            }
            return false;

            int Resolve(ValueExpression? expr)
            {
                if (TryResolveValue<double>(expr, provider, out var value))
                {
                    return (int)value;
                }
                return 0;
            }
        }
    }
}
