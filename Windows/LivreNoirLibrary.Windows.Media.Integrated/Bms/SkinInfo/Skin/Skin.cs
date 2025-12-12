using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Skin : SkinContainer
    {
        [TypeConverter(typeof(VersionConverter))]
        public Version Version { get; set => SetValue(ref field, value); } = new(1, 0);
        public string? Author { get; set => SetValue(ref field, value); }
        public System.Drawing.Size BaseSize { get; set => SetValue(ref field, value); }
        public LnColor Background { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);

        public ObservableList<string> Includes { get; } = [];
        public OptionCollection Options { get; } = [];
        public VariableCollection Variables { get; } = [];
        public TextureCollection Textures { get; } = [];

        private string? _directory;

        internal void Refresh(string directory, Dictionary<string, Skin> skins)
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
            var children = Children;
            // include
            if (includes.Count is > 0)
            {
                // このスキンでの設定値を保存
                OptionBase[] optionBuffer = [.. options];
                Variable[] variableBuffer = [.. variables];
                Texture[] textureBuffer = [.. textures];
                var childIndex = 0;
                foreach (var include in includes.AsSpan())
                {
                    if (skins.TryGetValue(Path.GetFullPath(include, directory), out var parent))
                    {
                        options.AddRange(parent.Options);
                        variables.AddRange(parent.Variables);
                        textures.AddRange(parent.Textures);
                        children.InsertRange(childIndex, parent.Children);
                        childIndex += parent.Children.Count;
                    }
                }
                // このスキンでの設定値で上書きしなおす
                options.AddRange(optionBuffer);
                variables.AddRange(variableBuffer);
                textures.AddRange(textureBuffer);
            }
            // texture inheritance
            foreach (var texture in textures.AsSpan())
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

        public bool TryResolveReflection(ValueExpression? expr, IVariableProvider? provider, [MaybeNullWhen(false)]out string value)
        {
            if (expr is null)
            {
                value = null;
                return false;
            }
            var reflected = ObjectPool.Rent<HashSet<string>>();
            try
            {
                return TryResolveReflection(expr, provider, reflected, out value);
            }
            finally
            {
                ObjectPool.Return(reflected);
            }
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

        public bool TryGetTextureData(string? key, IVariableProvider? provider, out TextureData data)
        {
            data = default;
            if (string.IsNullOrEmpty(key))
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
