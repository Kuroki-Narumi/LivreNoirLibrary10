using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Skin : SkinContainer
    {
        public string? DisplayName { get; set => SetValue(ref field, value); }
        [TypeConverter(typeof(VersionConverter))]
        public Version Version { get; set => SetValue(ref field, value); } = new(1, 0);
        public string? Author { get; set => SetValue(ref field, value); }
        public System.Drawing.Size BaseSize { get; set => SetValue(ref field, value); }
        public LnColor Background { get; set => SetValue(ref field, value); } = new(0, 0, 0);

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

        private bool TryResolveReflection(ValueExpression expr, IVariableProvider? provider, HashSet<string> reflection, [MaybeNullWhen(false)]out string value)
        {
            value = null;
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
                            variable.Value is { } expr2 && 
                            TryResolveReflection(expr2, provider, reflection, out value))
                        {
                            value = variable.GetActualValue(value);
                            goto Return;
                        }
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

        public bool TryGetTexture(string? key, IVariableProvider? provider, out TextureData data)
        {
            data = default;
            if (key is null || !Textures.TryGetValue(key, out var texture) || !TryResolveReflection(texture.Source, provider, out var source))
            {
                return false;
            }
            if (!Texture.IsReservedKey(source))
            {
                source = Path.GetFullPath(source, texture._baseDirectory ?? _directory ?? General.GetAssemblyDir());
            }
            int Resolve(ValueExpression? expr)
            {
                if (TryResolveValue<double>(expr, provider, out var value))
                {
                    return (int)value;
                }
                return 0;
            }
            var x = Resolve(texture.X);
            var y = Resolve(texture.Y);
            var w = Resolve(texture.Width);
            var h = Resolve(texture.Height);
            var divX = Math.Max(Resolve(texture.DivX), 1);
            var divY = Math.Max(Resolve(texture.DivY), 1);
            var period = Math.Max(TimeUtils.Seconds2Ticks(TryResolveValue<double>(texture.LoopPeriod, provider, out var v) ? v : 0), 0);
            data = new(source, x, y, w, h, divX, divY, period);
            return true;
        }
    }
}
