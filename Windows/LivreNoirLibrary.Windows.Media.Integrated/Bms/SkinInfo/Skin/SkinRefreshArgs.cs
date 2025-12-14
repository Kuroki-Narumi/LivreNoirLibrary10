using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class SkinRefreshArgs
    {
        private readonly Dictionary<string, Skin> _skins = [];

        private readonly List<OptionBase> _options = [];
        private readonly List<Variable> _variables = [];
        private readonly List<Texture> _textures = [];
        private readonly Dictionary<string, Skin> _includeSkins = [];

        public void Clear()
        {
            _skins.Clear();
            _includeSkins.Clear();
        }

        public void RegisterSkin(string fullPath, Skin skin)
        {
            var key = Path.ChangeExtension(fullPath, null).Replace('\\', '/');
            _skins[key] = skin;
        }

        public bool TryGetSkin(string fullPath, [MaybeNullWhen(false)] out Skin skin)
        {
            var key = Path.ChangeExtension(fullPath, null).Replace('\\', '/');
            return _skins.TryGetValue(key, out skin);
        }

        public void BeginResolveInclude(OptionCollection options, VariableCollection variables, TextureCollection textures)
        {
            _includeSkins.Clear();
            _options.AddRange(options);
            _variables.AddRange(variables);
            _textures.AddRange(textures);
            options.Clear();
            variables.Clear();
            textures.Clear();
        }

        public bool TryGetIncludeSource(string directory, IncludeSource include, [MaybeNullWhen(false)] out Skin source)
        {
            if (include.Source is { } path && TryGetSkin(Path.GetFullPath(path, directory), out source))
            {
                _includeSkins[include.Key] = source;
                return true;
            }
            source = null;
            return false;
        }

        public bool TryGetIncludeSource(string key, [MaybeNullWhen(false)] out Skin source) => _includeSkins.TryGetValue(key, out source);

        public void RestoreSkinInfo(OptionCollection options, VariableCollection variables, TextureCollection textures)
        {
            options.AddRange(_options);
            variables.AddRange(_variables);
            textures.AddRange(_textures);
            _options.Clear();
            _variables.Clear();
            _textures.Clear();
        }

        public void FinishResolveInclude()
        {
            _includeSkins.Clear();
        }
    }
}
