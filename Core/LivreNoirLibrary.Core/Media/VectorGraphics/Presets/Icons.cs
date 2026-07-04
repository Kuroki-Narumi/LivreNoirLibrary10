using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class Icons
    {
        public static ElementGroup Transparent { get; } = new(Elements.Base);

        public static ElementGroup Cross { get; } = new(Elements.Cross);
        public static ElementGroup Check { get; } = new(Elements.Check);
        public static ElementGroup Plus { get; } = new(Elements.Plus);
        public static ElementGroup Minus { get; } = new(Elements.Minus);
        public static ElementGroup Cross_Red { get; } = new(Elements.Cross_Red);
        public static ElementGroup Check_Green { get; } = new(Elements.Check_Green);
        public static ElementGroup Dots { get; } = new(Elements.Dots);
        public static ElementGroup Grid { get; } = new(Elements.Grid);

        public static ElementGroup Selection { get; } = new(Elements.Rect_24, Elements.Dots);

        public static ElementGroup ArrowLeft { get; } = new(Elements.ArrowLeft);
        public static ElementGroup ArrowRight { get; } = new(Elements.ArrowRight);
        public static ElementGroup ArrowUp { get; } = new(Elements.ArrowUp);
        public static ElementGroup ArrowDown { get; } = new(Elements.ArrowDown);

        public static ElementGroup HeadLeft { get; } = new(Elements.Circle_Outer, Elements.Circle_Inner, Elements.HeadLeft_Inner);
        public static ElementGroup HeadRight { get; } = new(Elements.Circle_Outer, Elements.Circle_Inner, Elements.HeadRight_Inner);
        public static ElementGroup HeadUp { get; } = new(Elements.Circle_Outer, Elements.Circle_Inner, Elements.HeadUp_Inner);
        public static ElementGroup HeadDown { get; } = new(Elements.Circle_Outer, Elements.Circle_Inner, Elements.HeadDown_Inner);

        public static ElementGroup Zoom { get; } = new(Elements.Zoom);
        public static ElementGroup ZoomUp { get; } = new(Elements.Zoom, Elements.ZoomUp);
        public static ElementGroup ZoomDown { get; } = new(Elements.Zoom, Elements.ZoomDown);

        public static ElementGroup Maximize { get; } = new(Elements.Maximize);
        public static ElementGroup Minimize { get; } = new(Elements.Minimize);
        public static ElementGroup ShowInTaskbar { get; } = new(Elements.ShowInTaskbar);
        public static ElementGroup Topmost { get; } = new(Elements.Topmost);
        public static ElementGroup SlipThrough { get; } = new(Elements.SlipThrough);

        public static ElementGroup Clock { get; } = new(Elements.Circle_Outer, Elements.Clock);

        public static ElementGroup Download { get; } = new(Elements.Download);
        public static ElementGroup Upload { get; } = new(Elements.Upload);
        public static ElementGroup Json { get; } = new(Elements.Json);
        public static ElementGroup Letter_A { get; } = new(Elements.Letter_A);

        public static ElementGroup VerticalAlign_Top { get; } = new(Elements.VerticalAlign_Top);
        public static ElementGroup VerticalAlign_Center { get; } = new(Elements.VerticalAlign_Center);
        public static ElementGroup VerticalAlign_Bottom { get; } = new(Elements.VerticalAlign_Bottom);
        public static ElementGroup VerticalAlign_Stretch { get; } = new(Elements.VerticalAlign_Stretch);
        public static ElementGroup HorizontalAlign_Left { get; } = new(Elements.HorizontalAlign_Left);
        public static ElementGroup HorizontalAlign_Center { get; } = new(Elements.HorizontalAlign_Center);
        public static ElementGroup HorizontalAlign_Right { get; } = new(Elements.HorizontalAlign_Right);
        public static ElementGroup HorizontalAlign_Stretch { get; } = new(Elements.HorizontalAlign_Stretch);

        public static ElementGroup Scroll_All { get; } = new(Elements.Circle_Outer, Elements.Scroll_Circle, Elements.Scroll_All);
        public static ElementGroup Scroll_Vertical { get; } = new(Elements.Circle_Outer, Elements.Scroll_Circle, Elements.Scroll_Vertical);
        public static ElementGroup Scroll_Horizontal { get; } = new(Elements.Circle_Outer, Elements.Scroll_Circle, Elements.Scroll_Horizontal);

        public static ElementGroup Help { get; } = new(Elements.Circle_Outer, Elements.Circle_Inner, Elements.Question);
        public static ElementGroup Question { get; } = new(Elements.Question_Mono);
        public static ElementGroup Info { get; } = new(Elements.Circle_Outer, Elements.Circle_Inner, Elements.Info);
        public static ElementGroup Caution { get; } = new(Elements.Caution_Outer, Elements.Caution_Inner);
        public static ElementGroup Attention { get; } = new(Elements.Attention_Outer, Elements.Caution_Inner);
        public static ElementGroup Config { get; } = new(Elements.Gear_Outer, Elements.Gear_Inner);
        public static ElementGroup Volume_0 { get; } = new(Elements.Volume_0);
        public static ElementGroup Volume_1 { get; } = new(Elements.Volume_1);
        public static ElementGroup Volume_2 { get; } = new(Elements.Volume_2);
        public static ElementGroup Volume_3 { get; } = new(Elements.Volume_3);
        public static ElementGroup Volume_Mute { get; } = new(Elements.Volume_Mute);
        public static ElementGroup Hamburger { get; } = new(Elements.Hamburger);
        public static ElementGroup Update { get; } = new(Elements.Update_Outer, Elements.Update_Inner);
        public static ElementGroup Update_Disabled { get; } = new(Elements.Update_D_Outer, Elements.Update_D_Inner, Elements.Cross_Red);
        public static ElementGroup Update_Mono { get; } = new(Elements.Update_D_Outer, Elements.Update_D_Inner);

        public static ElementGroup Play { get; } = new(Elements.Play);
        public static ElementGroup Play2 { get; } = new(Elements.Play2);
        public static ElementGroup Pause { get; } = new(Elements.Pause);
        public static ElementGroup Stop { get; } = new(Elements.Stop);
        public static ElementGroup Repeat { get; } = new(Elements.Repeat);
        public static ElementGroup SkipLeft { get; } = new(Elements.SkipLeft);
        public static ElementGroup SkipRight { get; } = new(Elements.SkipRight);

        public static ElementGroup NewWindow { get; } = new(Elements.NewWindow_Background, Elements.NewWindow_Frame, Elements.NewWindow_Arrow);
        public static ElementGroup Console { get; } = new(Elements.Console_Body, Elements.Console_Stroke, Elements.Console_Head);
        public static ElementGroup New { get; } = new(Elements.New);
        public static ElementGroup Folder { get; } = new(Elements.Folder_Background, Elements.Folder_Foreground);
        public static ElementGroup Open { get; } = new(Elements.File_Background, Elements.File_Foreground, Elements.File_Inner);
        public static ElementGroup Save { get; } = new(Elements.Save_Inner, Elements.Save_Frame, Elements.Floppy_Label);
        public static ElementGroup SaveAs { get; } = new(Elements.SaveAs_Inner, Elements.SaveAs_Frame, Elements.Floppy_Label, Elements.Plus_LowerRight_Back, Elements.Plus_LowerRight_Fore);
        public static ElementGroup Delete { get; } = new(Elements.Delete_Inner, Elements.Delete_Frame);
        public static ElementGroup Delete_Mono { get; } = new(Elements.Delete_Inner_Mono, Elements.Delete_Frame_Mono);

        public static ElementGroup Undo { get; } = new(Elements.Undo);
        public static ElementGroup Undo_Mono { get; } = new(Elements.Undo_Mono);
        public static ElementGroup Redo { get; } = new(Elements.Redo);
        public static ElementGroup Redo_Mono { get; } = new(Elements.Redo_Mono);
        public static ElementGroup Edit { get; } = new(Elements.Pencil_Outer, Elements.Pencil_Inner);
        public static ElementGroup Cut { get; } = new(Elements.Cut);
        public static ElementGroup Copy { get; } = new(Elements.Copy_Background, Elements.Copy_Foreground);
        public static ElementGroup Paste { get; } = new(Elements.Clipboard_Background, Elements.Clipboard_Foreground, Elements.Clipboard_Clip, Elements.Clipboard_Paper);
        public static ElementGroup Paste_Mono { get; } = new(Elements.Clipboard_Background_Mono, Elements.Clipboard_Foreground_Mono, Elements.Clipboard_Clip, Elements.Clipboard_Paper);

        public static ElementGroup Picture { get; } = new(Elements.Picture_Background, Elements.Picture_Sky, Elements.Picture_Mountain, Elements.Picture_Sun);
        public static ElementGroup Picture_Mono { get; } = new(Elements.Picture_Background, Elements.Picture_Sky_Mono, Elements.Picture_Mountain_Mono, Elements.Picture_Sun_Mono);

        public static ElementGroup Camera { get; } = new(Elements.Camera_Outer, Elements.Camera_Inner);

        public static ElementGroup Document { get; } = new(Elements.Document_Back, Elements.Document_Frame);

        public static ElementGroup Merge { get; } = new(Elements.Merge_Arrow, Elements.Merge_Outer, Elements.Merge_Inner);
        public static ElementGroup Split { get; } = new(Elements.Split_Arrow, Elements.Split_Outer, Elements.Split_Inner);

        public static ElementGroup Wave { get; } = new(Elements.Wave);
        public static ElementGroup Wave_Gain { get; } = new(Elements.BaseBlack, Elements.Wave_Gain_Mid, Elements.Wave_Gain_Zero);
        public static ElementGroup Wave_Time { get; } = new(Elements.BaseBlack, Elements.Wave_Time);
        public static ElementGroup Wave_Marker { get; } = new(Elements.BaseBlack, Elements.Wave_Marker);
        public static ElementGroup Wave_Marker_Name { get; } = new(Elements.BaseBlack, Elements.Wave_Marker_Name);

        public static ElementGroup Wave_Marker_Add { get; } = new(Elements.Wave_Marker_Outer, Elements.Wave_Marker_Single);
        public static ElementGroup Wave_Marker_Single { get; } = new(Elements.Wave_Marker_Single_Outer, Elements.Wave_Marker_Single, Elements.Wave_Marker_Arrow);
        public static ElementGroup Wave_Marker_Multi { get; } = new(Elements.Wave_Marker_Multi_Outer, Elements.Wave_Marker_Multi, Elements.Wave_Marker_Arrow);
        public static ElementGroup Wave_Marker_Auto { get; } = new(Elements.Wave_Marker_Auto_Inner, Elements.Wave_Marker_Auto_Rect, Elements.Plus_LowerRight_Back, Elements.Plus_LowerRight_Fore);
        public static ElementGroup Wave_Marker_Clear { get; } = new(Elements.Wave_Marker_Auto_Clear, Elements.Wave_Marker_Auto_Rect, Elements.Minus_LowerRight_Back, Elements.Minus_LowerRight_Fore);

        public static ElementGroup Wave_Slice { get; } = new(Elements.Wave_Slice, Elements.Wave);
        public static ElementGroup Wave_Slice_Note { get; } = new(Elements.Wave_Slice_Note1, Elements.Wave, Elements.Wave_Slice_Note2);
        public static ElementGroup Wave_Spectrum { get; } = new(Elements.BaseBlack, Elements.Wave_Spectrum_Frame, Elements.Wave_Spectrum_Figure);

        public static ElementGroup Bms { get; } = new(Elements.Bms_Background, Elements.Bms_Red, Elements.Bms_Scratch, Elements.Bms_Black, Elements.Bms_White);
        public static ElementGroup Bms_Sort { get; } = new(Elements.Bms_Sort_Background1, Elements.Bms_Sort_Background2, Elements.Bms_Sort_Foreground, Elements.Bms_Sort_Highlight);
        public static ElementGroup Bms_Sequential { get; } = new(Elements.Bms_Sequential_Background1, Elements.Bms_Sequential_Background2, Elements.Bms_Sequential_Foreground);
        public static ElementGroup Bms_DefList { get; } = new(Elements.Letter_ZZ);
        public static ElementGroup Bms_MultiDef { get; } = new(Elements.Bms_Sort_Background1, Elements.Bms_MultiDef_Background2, Elements.Bms_MultiDef_Note, Elements.Bms_MultiDef_Wave);

        public static ElementGroup Midi { get; } = new(Elements.Midi_Black, Elements.Midi_White1, Elements.Midi_White2, Elements.Midi_Note1, Elements.Midi_Note2, Elements.Midi_Note3);

        public static ElementGroup Midi_Background { get; } = new(Elements.Midi_Black, Elements.Midi_White1, Elements.Midi_White2);
        public static ElementGroup Midi_BarLine { get; } = new(Elements.BaseBlack, Elements.Midi_BarLine);
        public static ElementGroup Midi_Onion { get; } = new(Elements.BaseBlack, Elements.Midi_Note1, Elements.Midi_Note2, Elements.Midi_Note3);
        public static ElementGroup Midi_Velocity { get; } = new(Elements.BaseBlack, Elements.Midi_Velocity);
        public static ElementGroup Midi_SysEx { get; } = new(Elements.Letter_F0);
        public static ElementGroup Midi_Enchord { get; } = new(Elements.Midi_Enchord_Rect, Elements.Midi_Enchord_Red, Elements.Midi_Enchord_Marker);
        public static ElementGroup Midi_Enchord_NoMark { get; } = new(Elements.Midi_Enchord_Rect, Elements.Midi_Enchord_Red, Elements.Midi_Enchord_Marker2);
        public static ElementGroup Midi_Dechord { get; } = new(Elements.Midi_Enchord_Rect, Elements.Midi_Enchord_Blue);
        public static ElementGroup Midi_MultiGroup { get; } = new(Elements.Midi_Enchord_Red, Elements.Midi_Enchord_Green, Elements.Midi_Enchord_Purple, Elements.Midi_Enchord_Marker);
        public static ElementGroup Midi_Group { get; } = new(Elements.Midi_Group_Marker_Outer, Elements.Midi_Group_Notes, Elements.Midi_Group_Marker);
        public static ElementGroup Midi_AutoGroup { get; } = new(Elements.Midi_AutoGroup_Left, Elements.Midi_AutoGroup_Right);

        public static ElementGroup Midi_Sort { get; } = new(Elements.Midi_Sort_Notes, Elements.Save_Mini_Inner, Elements.Save_Mini_Frame, Elements.Floppy_Mini_Label);

        public static ElementGroup Midi_KeySwitch { get; } = new(Elements.Piano_White, Elements.Piano_Black, Elements.Piano_Outline, Elements.Piano_KeySwtich);
        public static ElementGroup Metronome { get; } = new(Elements.Metronome_Outer, Elements.Metronome_Inner, Elements.Metronome_Scale, Elements.Metronome_Bar);

        public static ElementGroup Browse { get; } = new(Elements.Browse_BackTab, Elements.Browse_FrontTab, Elements.Browse_Content, Elements.Browse_Outline);
    }
}
