using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    using Dr = IconContent.Drawings;
    using DG = DrawingGroup;

    public static class Icons
    {
        private static readonly Rect Bounds = new(0, 0, 32, 32);

        private static DG Get(params ReadOnlySpan<GeometryDrawing> args)
        {
            DG dg = new();
            foreach (var arg in args)
            {
                dg.Children.Add(arg);
            }
            if (dg.Bounds != Bounds)
            {
                dg.Children.Insert(0, Dr.Base);
            }
            dg.Freeze();
            return dg;
        }

        public static DG Transparent { get; } = Get();

        public static DG Cross { get; } = Get(Dr.Cross);
        public static DG Check { get; } = Get(Dr.Check);
        public static DG Plus { get; } = Get(Dr.Plus);
        public static DG Minus { get; } = Get(Dr.Minus);
        public static DG Cross_Red { get; } = Get(Dr.Cross_Red);
        public static DG Check_Green { get; } = Get(Dr.Check_Green);
        public static DG Dots { get; } = Get(Dr.Dots);
        public static DG Grid { get; } = Get(Dr.Grid);

        public static DG Selection { get; } = Get(Dr.Rect_24, Dr.Dots);

        public static DG ArrowLeft { get; } = Get(Dr.ArrowLeft);
        public static DG ArrowRight { get; } = Get(Dr.ArrowRight);
        public static DG ArrowUp { get; } = Get(Dr.ArrowUp);
        public static DG ArrowDown { get; } = Get(Dr.ArrowDown);

        public static DG HeadLeft { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner, Dr.HeadLeft_Inner);
        public static DG HeadRight { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner, Dr.HeadRight_Inner);
        public static DG HeadUp { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner, Dr.HeadUp_Inner);
        public static DG HeadDown { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner, Dr.HeadDown_Inner);

        public static DG Zoom { get; } = Get(Dr.Zoom);
        public static DG ZoomUp { get; } = Get(Dr.Zoom, Dr.ZoomUp);
        public static DG ZoomDown { get; } = Get(Dr.Zoom, Dr.ZoomDown);

        public static DG Maximize { get; } = Get(Dr.Maximize);
        public static DG Minimize { get; } = Get(Dr.Minimize);
        public static DG ShowInTaskbar { get; } = Get(Dr.ShowInTaskbar);
        public static DG Topmost { get; } = Get(Dr.Topmost);
        public static DG SlipThrough { get; } = Get(Dr.SlipThrough);

        public static DG Clock { get; } = Get(Dr.Circle_Outer, Dr.Clock);

        public static DG Download { get; } = Get(Dr.Download);
        public static DG Upload { get; } = Get(Dr.Upload);
        public static DG Json { get; } = Get(Dr.Json);
        public static DG Letter_A { get; } = Get(Dr.Letter_A);

        public static DG VerticalAlign_Top { get; } = Get(Dr.VerticalAlign_Top);
        public static DG VerticalAlign_Center { get; } = Get(Dr.VerticalAlign_Center);
        public static DG VerticalAlign_Bottom { get; } = Get(Dr.VerticalAlign_Bottom);
        public static DG VerticalAlign_Stretch { get; } = Get(Dr.VerticalAlign_Stretch);
        public static DG HorizontalAlign_Left { get; } = Get(Dr.HorizontalAlign_Left);
        public static DG HorizontalAlign_Center { get; } = Get(Dr.HorizontalAlign_Center);
        public static DG HorizontalAlign_Right { get; } = Get(Dr.HorizontalAlign_Right);
        public static DG HorizontalAlign_Stretch { get; } = Get(Dr.HorizontalAlign_Stretch);

        public static DG Scroll_All { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner_Mono, Dr.Scroll_All);
        public static DG Scroll_Vertical { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner_Mono, Dr.Scroll_Vertical);
        public static DG Scroll_Horizontal { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner_Mono, Dr.Scroll_Horizontal);

        public static DG Help { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner, Dr.Question);
        public static DG Question { get; } = Get(Dr.Question_Mono);
        public static DG Info { get; } = Get(Dr.Circle_Outer, Dr.Circle_Inner, Dr.Info);
        public static DG Caution { get; } = Get(Dr.Caution_Outer, Dr.Caution_Inner);
        public static DG Attention { get; } = Get(Dr.Attention_Outer, Dr.Caution_Inner);
        public static DG Config { get; } = Get(Dr.Gear_Outer, Dr.Gear_Inner);
        public static DG Hamburger { get; } = Get(Dr.Hamburger);
        public static DG Update { get; } = Get(Dr.Update_Outer, Dr.Update_Inner);
        public static DG Update_Disabled { get; } = Get(Dr.Update_D_Outer, Dr.Update_D_Inner, Dr.Cross_Red);
        public static DG Update_Mono { get; } = Get(Dr.Update_D_Outer, Dr.Update_D_Inner);

        public static DG Play { get; } = Get(Dr.Play);
        public static DG Play2 { get; } = Get(Dr.Play2);
        public static DG Pause { get; } = Get(Dr.Pause);
        public static DG Stop { get; } = Get(Dr.Stop);
        public static DG Repeat { get; } = Get(Dr.Repeat);
        public static DG SkipLeft { get; } = Get(Dr.SkipLeft);
        public static DG SkipRight { get; } = Get(Dr.SkipRight);

        public static DG NewWindow { get; } = Get(Dr.NewWindow_Background, Dr.NewWindow_Frame, Dr.NewWindow_Arrow);
        public static DG Console { get; } = Get(Dr.Console_Body, Dr.Console_Stroke, Dr.Console_Head);
        public static DG New { get; } = Get(Dr.New);
        public static DG Folder { get; } = Get(Dr.Folder_Background, Dr.Folder_Foreground);
        public static DG Open { get; } = Get(Dr.File_Background, Dr.File_Foreground, Dr.File_Inner);
        public static DG Save { get; } = Get(Dr.Save_Inner, Dr.Save_Frame, Dr.Floppy_Label);
        public static DG SaveAs { get; } = Get(Dr.SaveAs_Inner, Dr.SaveAs_Frame, Dr.Floppy_Label, Dr.Plus_LowerRight_Back, Dr.Plus_LowerRight_Fore);
        public static DG Delete { get; } = Get(Dr.Delete_Inner, Dr.Delete_Frame);
        public static DG Delete_Mono { get; } = Get(Dr.Delete_Inner_Mono, Dr.Delete_Frame_Mono);

        public static DG Undo { get; } = Get(Dr.Undo);
        public static DG Undo_Mono { get; } = Get(Dr.Undo_Mono);
        public static DG Redo { get; } = Get(Dr.Redo);
        public static DG Redo_Mono { get; } = Get(Dr.Redo_Mono);
        public static DG Edit { get; } = Get(Dr.Pencil_Outer, Dr.Pencil_Inner);
        public static DG Cut { get; } = Get(Dr.Cut);
        public static DG Copy { get; } = Get(Dr.Copy_Background, Dr.Copy_Foreground);
        public static DG Paste { get; } = Get(Dr.Clipboard_Background, Dr.Clipboard_Foreground, Dr.Clipboard_Clip, Dr.Clipboard_Paper);
        public static DG Paste_Mono { get; } = Get(Dr.Clipboard_Background_Mono, Dr.Clipboard_Foreground_Mono, Dr.Clipboard_Clip, Dr.Clipboard_Paper);

        public static DG Picture { get; } = Get(Dr.Picture_Background, Dr.Picture_Sky, Dr.Picture_Mountain, Dr.Picture_Sun);
        public static DG Picture_Mono { get; } = Get(Dr.Picture_Background, Dr.Picture_Sky_Mono, Dr.Picture_Mountain_Mono, Dr.Picture_Sun_Mono);

        public static DG Camera { get; } = Get(Dr.Camera_Outer, Dr.Camera_Inner);

        public static DG Document { get; } = Get(Dr.Document_Back, Dr.Document_Frame);

        public static DG Merge { get; } = Get(Dr.Merge_Arrow, Dr.Merge_Outer, Dr.Merge_Inner);
        public static DG Split { get; } = Get(Dr.Split_Arrow, Dr.Split_Outer, Dr.Split_Inner);

        public static DG Wave { get; } = Get(Dr.Wave);
        public static DG Wave_Gain { get; } = Get(Dr.BaseBlack, Dr.Wave_Gain_Mid, Dr.Wave_Gain_Zero);
        public static DG Wave_Time { get; } = Get(Dr.BaseBlack, Dr.Wave_Time);
        public static DG Wave_Marker { get; } = Get(Dr.BaseBlack, Dr.Wave_Marker);
        public static DG Wave_Marker_Name { get; } = Get(Dr.BaseBlack, Dr.Wave_Marker_Name);
        
        public static DG Wave_Marker_Add { get; } = Get(Dr.Wave_Marker_Outer, Dr.Wave_Marker_Single);
        public static DG Wave_Marker_Single { get; } = Get(Dr.Wave_Marker_Single_Outer, Dr.Wave_Marker_Single, Dr.Wave_Marker_Arrow);
        public static DG Wave_Marker_Multi { get; } = Get(Dr.Wave_Marker_Multi_Outer, Dr.Wave_Marker_Multi, Dr.Wave_Marker_Arrow);
        public static DG Wave_Marker_Auto { get; } = Get(Dr.Wave_Marker_Auto_Inner, Dr.Wave_Marker_Auto_Rect, Dr.Plus_LowerRight_Back, Dr.Plus_LowerRight_Fore);
        public static DG Wave_Marker_Clear { get; } = Get(Dr.Wave_Marker_Auto_Clear, Dr.Wave_Marker_Auto_Rect, Dr.Minus_LowerRight_Back, Dr.Minus_LowerRight_Fore);
        
        public static DG Wave_Slice { get; } = Get(Dr.Wave_Slice, Dr.Wave);
        public static DG Wave_Slice_Note { get; } = Get(Dr.Wave_Slice_Note1, Dr.Wave, Dr.Wave_Slice_Note2);
        public static DG Wave_Spectrum { get; } = Get(Dr.BaseBlack, Dr.Wave_Spectrum_Frame, Dr.Wave_Spectrum_Figure);

        public static DG Bms { get; } = Get(Dr.Bms_Background, Dr.Bms_Red, Dr.Bms_Scratch, Dr.Bms_Black, Dr.Bms_White);
        public static DG Bms_Sort { get; } = Get(Dr.Bms_Sort_Background1, Dr.Bms_Sort_Background2, Dr.Bms_Sort_Foreground, Dr.Bms_Sort_Highlight);
        public static DG Bms_Sequential { get; } = Get(Dr.Bms_Sequential_Background1, Dr.Bms_Sequential_Background2, Dr.Bms_Sequential_Foreground);
        public static DG Bms_DefList { get; } = Get(Dr.Letter_ZZ);

        public static DG Midi { get; } = Get(Dr.Midi_Black, Dr.Midi_White1, Dr.Midi_White2, Dr.Midi_Note1, Dr.Midi_Note2, Dr.Midi_Note3);

        public static DG Midi_Background { get; } = Get(Dr.Midi_Black, Dr.Midi_White1, Dr.Midi_White2);
        public static DG Midi_BarLine { get; } = Get(Dr.BaseBlack, Dr.Midi_BarLine);
        public static DG Midi_Onion { get; } = Get(Dr.BaseBlack, Dr.Midi_Note1, Dr.Midi_Note2, Dr.Midi_Note3);
        public static DG Midi_Velocity { get; } = Get(Dr.BaseBlack, Dr.Midi_Velocity);
        public static DG Midi_SysEx { get; } = Get(Dr.Letter_F0);
        public static DG Midi_Enchord { get; } = Get(Dr.Midi_Enchord_Rect, Dr.Midi_Enchord_Red, Dr.Midi_Enchord_Marker);
        public static DG Midi_Enchord_NoMark { get; } = Get(Dr.Midi_Enchord_Rect, Dr.Midi_Enchord_Red, Dr.Midi_Enchord_Marker2);
        public static DG Midi_Dechord { get; } = Get(Dr.Midi_Enchord_Rect, Dr.Midi_Enchord_Blue);
        public static DG Midi_MultiGroup { get; } = Get(Dr.Midi_Enchord_Red, Dr.Midi_Enchord_Green, Dr.Midi_Enchord_Purple, Dr.Midi_Enchord_Marker);
        public static DG Midi_Group { get; } = Get(Dr.Midi_Group_Marker_Outer, Dr.Midi_Group_Notes, Dr.Midi_Group_Marker);
        public static DG Midi_AutoGroup { get; } = Get(Dr.Midi_AutoGroup_Left, Dr.Midi_AutoGroup_Right);

        public static DG Midi_Sort { get; } = Get(Dr.Midi_Sort_Notes, Dr.Save_Mini_Inner, Dr.Save_Mini_Frame, Dr.Floppy_Mini_Label);

        public static DG Midi_KeySwitch { get; } = Get(Dr.Piano_White, Dr.Piano_Black, Dr.Piano_Outline, Dr.Piano_KeySwtich);
        public static DG Metronome { get; } = Get(Dr.Metronome_Outer, Dr.Metronome_Inner, Dr.Metronome_Scale, Dr.Metronome_Bar);

        public static DG Browse { get; } = Get(Dr.Browse_BackTab, Dr.Browse_FrontTab, Dr.Browse_Content, Dr.Browse_Outline);
    }
}
