using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class AdditionalSliceOptions : ObservableObjectBase
    {
        [ObservableProperty(Related = [nameof(IsReplaceEnabled), nameof(IsReplaceAllEnabled)])]
        private Note? _note;
        [ObservableProperty]
        private BarPosition _notePosition;
        [ObservableProperty(Related = [nameof(IsReplaceAllEnabled)])]
        private int _defIndex;
        [ObservableProperty(Related = [nameof(ReplaceMode_Add), nameof(ReplaceMode_Selection), nameof(ReplaceMode_All)])]
        private SliceReplaceMode _replaceMode;

        public bool IsReplaceEnabled => _note is not null;
        public bool IsReplaceAllEnabled => IsReplaceEnabled || _defIndex is > 0;

        [JsonIgnore]
        public bool ReplaceMode_Add { get => _replaceMode is SliceReplaceMode.Add; set => SetReplaceMode(SliceReplaceMode.Add, value); }
        [JsonIgnore]
        public bool ReplaceMode_Selection { get => _replaceMode is SliceReplaceMode.Selection; set => SetReplaceMode(SliceReplaceMode.Selection, value); }
        [JsonIgnore]
        public bool ReplaceMode_All { get => _replaceMode is SliceReplaceMode.All; set => SetReplaceMode(SliceReplaceMode.All, value); }

        private void SetReplaceMode(SliceReplaceMode mode, bool value)
        {
            if (value)
            {
                ReplaceMode = mode;
            }
        }

        public void Unset()
        {
            Note = null;
            NotePosition = default;
            DefIndex = 0;
            ReplaceMode = SliceReplaceMode.Add;
        }

        public void SetNote(BarPosition position, Note note)
        {
            Note = note;
            NotePosition = position;
            DefIndex = note.Id;
            if (_replaceMode is SliceReplaceMode.Add)
            {
                ReplaceMode = SliceReplaceMode.Selection;
            }
        }

        public void SetDefIndex(int index)
        {
            Note = null;
            NotePosition = default;
            DefIndex = index;
            if (_replaceMode is SliceReplaceMode.Selection)
            {
                ReplaceMode = SliceReplaceMode.All;
            }
        }
    }

    public enum SliceReplaceMode
    {
        Add,
        Selection,
        All,
    }
}
