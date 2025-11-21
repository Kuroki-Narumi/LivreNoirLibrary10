using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Media.Integrated
{
    public partial class AdditionalSliceOptions : SliceOptions
    {
        public double Detect_Attack { get; set => SetValue(ref field, value); } = -12;
        public double Detect_Release { get; set => SetValue(ref field, value); } = -24;
        public int Detect_Period { get; set => SetValue(ref field, value); } = 3;
        public int Detect_Interval { get; set => SetValue(ref field, value); } = 20;
        public bool Detect_ShowGuide { get; set => SetValue(ref field, value); }

        public Note? Note { get; set => SetValue(ref field, value, [nameof(IsReplaceEnabled), nameof(IsReplaceAllEnabled)]); }
        public BarPosition NotePosition { get; set => SetValue(ref field, value); }
        public int ReferenceIndex { get; set => SetValue(ref field, value, [nameof(IsReplaceAllEnabled)]); }
        public int DefStart { get; set => SetValue(ref field, value); }

        public bool IsReplaceEnabled => Note is not null;
        public bool IsReplaceAllEnabled => IsReplaceEnabled || ReferenceIndex is > 0;
        public SliceReplaceMode ReplaceMode { get; set => SetValue(ref field, value, [nameof(ReplaceMode_Add), nameof(ReplaceMode_Selection), nameof(ReplaceMode_All)]); }

        [JsonIgnore]
        public bool ReplaceMode_Add { get => ReplaceMode is SliceReplaceMode.Add; set => SetReplaceMode(SliceReplaceMode.Add, value); }
        [JsonIgnore]
        public bool ReplaceMode_Selection { get => ReplaceMode is SliceReplaceMode.Selection; set => SetReplaceMode(SliceReplaceMode.Selection, value); }
        [JsonIgnore]
        public bool ReplaceMode_All { get => ReplaceMode is SliceReplaceMode.All; set => SetReplaceMode(SliceReplaceMode.All, value); }

        private void SetReplaceMode(SliceReplaceMode mode, bool value)
        {
            if (value)
            {
                ReplaceMode = mode;
            }
        }

        public AdditionalSliceOptions()
        {
            Offset = 0;
            MarginLeft = 10000;
            Basename = PackUtils.DefaultFormat_Filename;
        }

        public void Unset()
        {
            Note = null;
            NotePosition = default;
            ReferenceIndex = 0;
            ReplaceMode = SliceReplaceMode.Add;
        }

        public void SetNote(BarPosition position, Note note)
        {
            Note = note;
            NotePosition = position;
            ReferenceIndex = (int)note.Value;
            if (ReplaceMode is SliceReplaceMode.Add)
            {
                ReplaceMode = SliceReplaceMode.Selection;
            }
        }

        public void SetDefIndex(int index)
        {
            Note = null;
            NotePosition = default;
            ReferenceIndex = index;
            if (ReplaceMode is SliceReplaceMode.Selection)
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
