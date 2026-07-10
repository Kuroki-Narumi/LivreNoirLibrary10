using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public static class Elements
    {
        private static GeometryElement Create(string geometry, string fill, bool stroke = false)
        {
            var brush = Brushes.Get(fill);
            var pen = stroke ? Outline : null;
            return new(geometry, brush, pen);
        }

        public static Pen Outline { get; } = new(Brushes.Get("#333"), 1);

        public static GeometryElement Base { get; } = Create(Geometries.Base, "#0000");
        public static GeometryElement BaseBlack { get; } = Create(Geometries.Base, "#000");
        public static GeometryElement Rect_24 { get; } = Create(Geometries.Rect_24, "#8cf");

        public static GeometryElement Circle_Outer { get; } = Create(Geometries.Circle_16, "#333");
        public static GeometryElement Circle_Inner { get; } = Create(Geometries.Circle_14, "#48c");

        public static GeometryElement HeadLeft_Inner { get; } = Create(Geometries.HeadLeft, "#fff", true);
        public static GeometryElement HeadRight_Inner { get; } = Create(Geometries.HeadRight, "#fff", true);
        public static GeometryElement HeadUp_Inner { get; } = Create(Geometries.HeadUp, "#fff", true);
        public static GeometryElement HeadDown_Inner { get; } = Create(Geometries.HeadDown, "#fff", true);

        public static GeometryElement Cross { get; } = Create(Geometries.Cross, "#333");
        public static GeometryElement Check { get; } = Create(Geometries.Check, "#333");
        public static GeometryElement Plus { get; } = Create(Geometries.Plus, "#333");
        public static GeometryElement Minus { get; } = Create(Geometries.Minus, "#333");
        public static GeometryElement Dots { get; } = Create(Geometries.Dots, "#333");
        public static GeometryElement Grid { get; } = Create(Geometries.Grid, "#333");

        public static GeometryElement Plus_LowerRight_Back { get; } = Create(Geometries.Plus_LowerRight_Back, "#333");
        public static GeometryElement Plus_LowerRight_Fore { get; } = Create(Geometries.Plus_LowerRight_Fore, "#ff0");
        public static GeometryElement Minus_LowerRight_Back { get; } = Create(Geometries.Minus_LowerRight_Back, "#333");
        public static GeometryElement Minus_LowerRight_Fore { get; } = Create(Geometries.Minus_LowerRight_Fore, "#f00");

        public static GeometryElement Cross_Red { get; } = Create(Geometries.Cross, "#e44");
        public static GeometryElement Check_Green { get; } = Create(Geometries.Check, "#4e4");

        public static GeometryElement ArrowLeft { get; } = Create(Geometries.ArrowLeft, "#333");
        public static GeometryElement ArrowRight { get; } = Create(Geometries.ArrowRight, "#333");
        public static GeometryElement ArrowUp { get; } = Create(Geometries.ArrowUp, "#333");
        public static GeometryElement ArrowDown { get; } = Create(Geometries.ArrowDown, "#333");
        public static GeometryElement RightLeft { get; } = Create(Geometries.RightLeft, "#333");
        public static GeometryElement UpDown { get; } = Create(Geometries.UpDown, "#333");

        public static GeometryElement Zoom { get; } = Create(Geometries.Zoom, "#333");
        public static GeometryElement ZoomUp { get; } = Create(Geometries.Zoom_Plus, "#333");
        public static GeometryElement ZoomDown { get; } = Create(Geometries.Zoom_Minus, "#333");

        public static GeometryElement Maximize { get; } = Create(Geometries.Maximize, "#333");
        public static GeometryElement Minimize { get; } = Create(Geometries.Minimize, "#333");
        public static GeometryElement ShowInTaskbar { get; } = Create(Geometries.ShowInTaskbar, "#333");
        public static GeometryElement Topmost { get; } = Create(Geometries.Topmost, "#333");
        public static GeometryElement SlipThrough { get; } = Create(Geometries.SlipThrough, "#333");

        public static GeometryElement Clock { get; } = Create(Geometries.Clock, "#fff");

        public static GeometryElement Question { get; } = Create(Geometries.Question, "#fff", true);
        public static GeometryElement Question_Mono { get; } = Create(Geometries.Question, "#333");
        public static GeometryElement Info { get; } = Create(Geometries.Info, "#fff", true);

        public static GeometryElement Caution_Outer { get; } = Create(Geometries.Circle_16, "#f00");
        public static GeometryElement Caution_Inner { get; } = Create(Geometries.Caution, "#fff");

        public static GeometryElement Attention_Outer { get; } = Create(Geometries.Circle_16, "#48c");

        public static GeometryElement Gear_Outer { get; } = Create(Geometries.Gear, "#333");
        public static GeometryElement Gear_Inner { get; } = Create(Geometries.Gear_S, "#89b", true);

        public static GeometryElement Volume_0 { get; } = Create(Geometries.Volume_0, "#333");
        public static GeometryElement Volume_1 { get; } = Create(Geometries.Volume_1, "#333");
        public static GeometryElement Volume_2 { get; } = Create(Geometries.Volume_2, "#333");
        public static GeometryElement Volume_3 { get; } = Create(Geometries.Volume_3, "#333");
        public static GeometryElement Volume_Mute { get; } = Create(Geometries.Volume_Mute, "#333");

        public static GeometryElement Hamburger { get; } = Create(Geometries.Hamburger, "#666");

        public static GeometryElement Update_Outer { get; } = Create(Geometries.Update, "#040");
        public static GeometryElement Update_Inner { get; } = Create(Geometries.Update_S, "#3fb");

        public static GeometryElement Update_D_Outer { get; } = Create(Geometries.Update, "#333");
        public static GeometryElement Update_D_Inner { get; } = Create(Geometries.Update_S, "#888");

        public static GeometryElement Download { get; } = Create(Geometries.Download, "#333");
        public static GeometryElement Upload { get; } = Create(Geometries.Upload, "#333");
        public static GeometryElement Json { get; } = Create(Geometries.Json, "#333");
        public static GeometryElement Letter_A { get; } = Create(Geometries.Letter_A, "#333");

        public static GeometryElement VerticalAlign_Top { get; } = Create(Geometries.VerticalAlign_Top, "#333");
        public static GeometryElement VerticalAlign_Center { get; } = Create(Geometries.VerticalAlign_Center, "#333");
        public static GeometryElement VerticalAlign_Bottom { get; } = Create(Geometries.VerticalAlign_Bottom, "#333");
        public static GeometryElement VerticalAlign_Stretch { get; } = Create(Geometries.VerticalAlign_Stretch, "#333");
        public static GeometryElement HorizontalAlign_Left { get; } = Create(Geometries.HorizontalAlign_Left, "#333");
        public static GeometryElement HorizontalAlign_Center { get; } = Create(Geometries.HorizontalAlign_Center, "#333");
        public static GeometryElement HorizontalAlign_Right { get; } = Create(Geometries.HorizontalAlign_Right, "#333");
        public static GeometryElement HorizontalAlign_Stretch { get; } = Create(Geometries.HorizontalAlign_Stretch, "#333");

        public static GeometryElement Scroll_Circle { get; } = Create(Geometries.Circle_14, "#fff");
        public static GeometryElement Scroll_All { get; } = Create(Geometries.Scroll_All, "#333");
        public static GeometryElement Scroll_Vertical { get; } = Create(Geometries.Scroll_Vertical, "#333");
        public static GeometryElement Scroll_Horizontal { get; } = Create(Geometries.Scroll_Horizontal, "#333");

        public static GeometryElement Play { get; } = Create(Geometries.Play, "#44e");
        public static GeometryElement Play2 { get; } = Create(Geometries.Play2, "#44e");
        public static GeometryElement Pause { get; } = Create(Geometries.Pause, "#4e4");
        public static GeometryElement Stop { get; } = Create(Geometries.Stop, "#e44");
        public static GeometryElement Repeat { get; } = Create(Geometries.Repeat, "#333");
        public static GeometryElement SkipLeft { get; } = Create(Geometries.SkipLeft, "#333");
        public static GeometryElement SkipRight { get; } = Create(Geometries.SkipRight, "#333");

        public static GeometryElement NewWindow_Background { get; } = Create(Geometries.NewWindow_Background, "#fff");
        public static GeometryElement NewWindow_Frame { get; } = Create(Geometries.NewWindow_Frame, "#44e");
        public static GeometryElement NewWindow_Arrow { get; } = Create(Geometries.NewWindow_Arrow, "#333");

        public static GeometryElement Console_Head { get; } = Create(Geometries.Console_Head, "#fff");
        public static GeometryElement Console_Body { get; } = Create(Geometries.Console_Body, "#000");
        public static GeometryElement Console_Stroke { get; } = Create(Geometries.Console_Stroke, "#888");

        public static GeometryElement Folder_Background { get; } = Create(Geometries.Folder_Background, "#420");
        public static GeometryElement Folder_Foreground { get; } = Create(Geometries.Folder_Foreground, "#fc8");

        public static GeometryElement File_Background { get; } = Create(Geometries.File_Background, "#420");
        public static GeometryElement File_Foreground { get; } = Create(Geometries.File_Foreground, "#fc8");
        public static GeometryElement File_Inner { get; } = Create(Geometries.File_Inner, "#fff");

        public static GeometryElement New { get; } = Create(Geometries.New, "#333");

        public static GeometryElement Save_Inner { get; } = Create(Geometries.Floppy_Inner, "#bbf");
        public static GeometryElement Save_Frame { get; } = Create(Geometries.Floppy_Frame, "#44a");
        public static GeometryElement SaveAs_Inner { get; } = Create(Geometries.Floppy_Inner, "#8fc");
        public static GeometryElement SaveAs_Frame { get; } = Create(Geometries.Floppy_Frame, "#272");
        public static GeometryElement Floppy_Label { get; } = Create(Geometries.Floppy_Label, "#fff");

        public static GeometryElement Save_Mini_Inner { get; } = Create(Geometries.Floppy_Mini_Inner, "#bbf");
        public static GeometryElement Save_Mini_Frame { get; } = Create(Geometries.Floppy_Mini_Frame, "#44a");
        public static GeometryElement Floppy_Mini_Label { get; } = Create(Geometries.Floppy_Mini_Label, "#fff");

        public static GeometryElement Delete_Inner { get; } = Create(Geometries.Bin_Background, "#f88");
        public static GeometryElement Delete_Frame { get; } = Create(Geometries.Bin_Foreground, "#822");

        public static GeometryElement Delete_Inner_Mono { get; } = Create(Geometries.Bin_Background, "#fff");
        public static GeometryElement Delete_Frame_Mono { get; } = Create(Geometries.Bin_Foreground, "#333");

        public static GeometryElement Undo { get; } = Create(Geometries.Undo, "#408");
        public static GeometryElement Undo_Mono { get; } = Create(Geometries.Undo, "#333");
        public static GeometryElement Redo { get; } = Create(Geometries.Redo, "#804");
        public static GeometryElement Redo_Mono { get; } = Create(Geometries.Redo, "#333");

        public static GeometryElement Pencil_Outer { get; } = Create(Geometries.Pencil_Outer, "#333");
        public static GeometryElement Pencil_Inner { get; } = Create(Geometries.Pencil_Inner, "#fff");

        public static GeometryElement Cut { get; } = Create(Geometries.Cut, "#333");

        public static GeometryElement Copy_Background { get; } = Create(Geometries.Copy_Background, "#333");
        public static GeometryElement Copy_Foreground { get; } = Create(Geometries.Copy_Foreground, "#fff");

        public static GeometryElement Clipboard_Background { get; } = Create(Geometries.Clipboard_Background, "#420");
        public static GeometryElement Clipboard_Foreground { get; } = Create(Geometries.Clipboard_Foreground, "#fc8");
        public static GeometryElement Clipboard_Background_Mono { get; } = Create(Geometries.Clipboard_Background, "#333");
        public static GeometryElement Clipboard_Foreground_Mono { get; } = Create(Geometries.Clipboard_Foreground, "#fff");
        public static GeometryElement Clipboard_Clip { get; } = Create(Geometries.Clipboard_Clip, "#666");
        public static GeometryElement Clipboard_Paper { get; } = Create(Geometries.Clipboard_Paper, "#fff");

        public static GeometryElement Picture_Background { get; } = Create(Geometries.Picture_Background, "#333");
        public static GeometryElement Picture_Sky { get; } = Create(Geometries.Picture_Sky, "#8cf");
        public static GeometryElement Picture_Mountain { get; } = Create(Geometries.Picture_Mountain, "#272");
        public static GeometryElement Picture_Sun { get; } = Create(Geometries.Picture_Sun, "#e44");

        public static GeometryElement Camera_Outer { get; } = Create(Geometries.Camera_Outer, "#333");
        public static GeometryElement Camera_Inner { get; } = Create(Geometries.Camera_Inner, "#fff");

        public static GeometryElement Picture_Sky_Mono { get; } = Create(Geometries.Picture_Sky, "#fff");
        public static GeometryElement Picture_Mountain_Mono { get; } = Create(Geometries.Picture_Mountain, "#888");
        public static GeometryElement Picture_Sun_Mono { get; } = Create(Geometries.Picture_Sun, "#333");

        public static GeometryElement Document_Back { get; } = Create(Geometries.Document_Back, "#fff");
        public static GeometryElement Document_Frame { get; } = Create(Geometries.Document_Frame, "#333");

        public static GeometryElement Letter_F0 { get; } = Create(Geometries.Letter_F0, "#333");
        public static GeometryElement Letter_ZZ { get; } = Create(Geometries.Letter_ZZ, "#333");

        public static GeometryElement Merge_Arrow { get; } = Create(Geometries.Merge_Arrow, "#333");
        public static GeometryElement Merge_Outer { get; } = Create(Geometries.Merge_Outer, "#666");
        public static GeometryElement Merge_Inner { get; } = Create(Geometries.Merge_Inner, "#fc8");

        public static GeometryElement Split_Arrow { get; } = Create(Geometries.Split_Arrow, "#333");
        public static GeometryElement Split_Outer { get; } = Create(Geometries.Split_Outer, "#666");
        public static GeometryElement Split_Inner { get; } = Create(Geometries.Split_Inner, "#fc8");

        public static GeometryElement Wave { get; } = Create(Geometries.Wave, "#f44");

        public static GeometryElement Wave_Gain_Zero { get; } = Create(Geometries.Wave_Gain_Zero, "#08f");
        public static GeometryElement Wave_Gain_Mid { get; } = Create(Geometries.Wave_Gain_Mid, "#c4f");
        public static GeometryElement Wave_Time { get; } = Create(Geometries.Wave_Time, "#bbf");
        public static GeometryElement Wave_Marker { get; } = Create(Geometries.Wave_Marker, "#0e0");
        public static GeometryElement Wave_Marker_Name { get; } = Create(Geometries.Wave_Marker_Name, "#0e0");

        public static GeometryElement Wave_Marker_Outer { get; } = Create(Geometries.Wave_Marker_Outer, "#666");
        public static GeometryElement Wave_Marker_Single_Outer { get; } = Create(Geometries.Wave_Marker_Single_Outer, "#666");
        public static GeometryElement Wave_Marker_Single { get; } = Create(Geometries.Wave_Marker_Single, "#0e0");
        public static GeometryElement Wave_Marker_Multi_Outer { get; } = Create(Geometries.Wave_Marker_Multi_Outer, "#666");
        public static GeometryElement Wave_Marker_Multi { get; } = Create(Geometries.Wave_Marker_Multi, "#0e0");
        public static GeometryElement Wave_Marker_Arrow { get; } = Create(Geometries.Wave_Marker_Arrow, "#ff0");
        public static GeometryElement Wave_Marker_Auto_Outer { get; } = Create(Geometries.Wave_Marker_Auto_Outer, "#666");
        public static GeometryElement Wave_Marker_Auto_Inner { get; } = Create(Geometries.Wave_Marker_Auto_Inner, "#0e0");
        public static GeometryElement Wave_Marker_Auto_Clear { get; } = Create(Geometries.Wave_Marker_Auto_Clear, "#8cf");
        public static GeometryElement Wave_Marker_Auto_Rect { get; } = Create(Geometries.Wave_Marker_Auto_Rect, "#8ff0");

        public static GeometryElement Wave_Slice { get; } = Create(Geometries.Wave_Slice, "#44a");
        public static GeometryElement Wave_Slice_Note1 { get; } = Create(Geometries.Wave_Slice_Note1, "#0e0");
        public static GeometryElement Wave_Slice_Note2 { get; } = Create(Geometries.Wave_Slice_Note2, "#8cf");
        public static GeometryElement Wave_Spectrum_Frame { get; } = Create(Geometries.Wave_Spectrum_Frame, "#aaa");
        public static GeometryElement Wave_Spectrum_Figure { get; } = Create(Geometries.Wave_Spectrum_Figure, "#f44");

        public static GeometryElement Bms_Background { get; } = Create(Geometries.Bms_Background, "#666");
        public static GeometryElement Bms_Red { get; } = Create(Geometries.Bms_Red, "#800");
        public static GeometryElement Bms_Scratch { get; } = Create(Geometries.Bms_Scratch, "#f00");
        public static GeometryElement Bms_Black { get; } = Create(Geometries.Bms_Black, "#448");
        public static GeometryElement Bms_White { get; } = Create(Geometries.Bms_White, "#fff");

        public static GeometryElement Bms_Sort_Background1 { get; } = Create(Geometries.Base, "#400");
        public static GeometryElement Bms_Sort_Background2 { get; } = Create(Geometries.Bms_Sort_Background, "#800");
        public static GeometryElement Bms_Sort_Foreground { get; } = Create(Geometries.Bms_Sort_Foreground, "#f44");
        public static GeometryElement Bms_Sort_Highlight { get; } = Create(Geometries.Bms_Sort_Highlight, "#f88");

        public static GeometryElement Bms_MultiDef_Background2 { get; } = Create(Geometries.Bms_MultiDef_Background, "#800");
        public static GeometryElement Bms_MultiDef_Note { get; } = Create(Geometries.Bms_MultiDef_Note, "#f44");
        public static GeometryElement Bms_MultiDef_Wave { get; } = Create(Geometries.Bms_MultiDef_Wave, "#ccc");

        public static GeometryElement Bms_Sequential_Background1 { get; } = Create(Geometries.Base, "#448");
        public static GeometryElement Bms_Sequential_Background2 { get; } = Create(Geometries.Bms_Sort_Background, "#666");
        public static GeometryElement Bms_Sequential_Foreground { get; } = Create(Geometries.Bms_Sequential, "#fff");

        public static GeometryElement Midi_Black { get; } = Create(Geometries.Midi_Black, "#000");
        public static GeometryElement Midi_White1 { get; } = Create(Geometries.Midi_White1, "#888");
        public static GeometryElement Midi_White2 { get; } = Create(Geometries.Midi_White2, "#666");
        public static GeometryElement Midi_Note1 { get; } = Create(Geometries.Midi_Note1, "#f88");
        public static GeometryElement Midi_Note2 { get; } = Create(Geometries.Midi_Note2, "#8cf");
        public static GeometryElement Midi_Note3 { get; } = Create(Geometries.Midi_Note3, "#8fc");

        public static GeometryElement Midi_BarLine { get; } = Create(Geometries.Midi_BarLine, "#aaa");
        public static GeometryElement Midi_Velocity { get; } = Create(Geometries.Midi_Velocity, "#a6f");

        public static GeometryElement Midi_Enchord_Blue { get; } = Create(Geometries.Midi_Enchord_Notes, "#44e");
        public static GeometryElement Midi_Enchord_Red { get; } = Create(Geometries.Midi_Enchord_Notes, "#e44");
        public static GeometryElement Midi_Enchord_Green { get; } = Create(Geometries.Midi_Enchord_Middle, "#272");
        public static GeometryElement Midi_Enchord_Purple { get; } = Create(Geometries.Midi_Enchord_Right, "#c4f");
        public static GeometryElement Midi_Enchord_Marker { get; } = Create(Geometries.Midi_Enchord_Marker, "#0e0");
        public static GeometryElement Midi_Enchord_Marker2 { get; } = Create(Geometries.Midi_Enchord_Marker2, "#0e0");
        public static GeometryElement Midi_Enchord_Rect { get; } = Create(Geometries.Midi_Enchord_Rect, "#8ff0");

        public static GeometryElement Midi_Group_Notes { get; } = Create(Geometries.Midi_Group_Notes, "#e44");
        public static GeometryElement Midi_Group_Marker_Outer { get; } = Create(Geometries.Midi_Group_Marker_Outer, "#666");
        public static GeometryElement Midi_Group_Marker { get; } = Create(Geometries.Midi_Group_Marker, "#0e0");
        public static GeometryElement Midi_AutoGroup_Left { get; } = Create(Geometries.Midi_AutoGroup_Left, "#e44");
        public static GeometryElement Midi_AutoGroup_Right { get; } = Create(Geometries.Midi_AutoGroup_Right, "#c4f");

        public static GeometryElement Midi_Sort_Notes { get; } = Create(Geometries.Midi_Sort_Notes, "#44e");

        public static GeometryElement Piano_White { get; } = Create(Geometries.Piano_White, "#fff");
        public static GeometryElement Piano_Black { get; } = Create(Geometries.Piano_Black, "#000");
        public static GeometryElement Piano_Outline { get; } = Create(Geometries.Piano_Outline, "#333");
        public static GeometryElement Piano_KeySwtich { get; } = Create(Geometries.Piano_KeySwitch, "#8ff0");

        public static GeometryElement Metronome_Outer { get; } = Create(Geometries.Metronome_Outer, "#420");
        public static GeometryElement Metronome_Inner { get; } = Create(Geometries.Metronome_Inner, "#fc8");
        public static GeometryElement Metronome_Scale { get; } = Create(Geometries.Metronome_Scale, "#333");
        public static GeometryElement Metronome_Bar { get; } = Create(Geometries.Metronome_Bar, "#f44");

        public static GeometryElement Browse_BackTab { get; } = Create(Geometries.Browse_BackTab, "#c62");
        public static GeometryElement Browse_FrontTab { get; } = Create(Geometries.Browse_FrontTab, "#fc8");
        public static GeometryElement Browse_Content { get; } = Create(Geometries.Browse_Content, "#fff");
        public static GeometryElement Browse_Outline { get; } = Create(Geometries.Browse_Outline, "#420");
    }
}
