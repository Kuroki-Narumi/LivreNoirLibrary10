using System.Windows.Media;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls.IconContent
{
    using GD = GeometryDrawing;

    public static class Drawings
    {
        private static GD Get(StreamGeometry geometry, Brush? brush = null, Pen? pen = null)
        {
            GD gd = new(brush, pen, geometry);
            gd.Freeze();
            return gd;
        }

        public static Pen? Outline { get; } = GetPen(Brushes.Gray_3, 1);

        public static GD Base { get; } = Get(Geometries.Base, Brushes.Transparent);
        public static GD BaseBlack { get; } = Get(Geometries.Base, Brushes.Black);
        public static GD Rect_24 { get; } = Get(Geometries.Rect_24, Brushes.BlueGreen_fc8);

        public static GD Circle_Outer { get; } = Get(Geometries.Circle_16, Brushes.Gray_3);
        public static GD Circle_Inner { get; } = Get(Geometries.Circle_14, Brushes.BlueGreen_c84);

        public static GD HeadLeft_Inner { get; } = Get(Geometries.HeadLeft, Brushes.White, Outline);
        public static GD HeadRight_Inner { get; } = Get(Geometries.HeadRight, Brushes.White, Outline);
        public static GD HeadUp_Inner { get; } = Get(Geometries.HeadUp, Brushes.White, Outline);
        public static GD HeadDown_Inner { get; } = Get(Geometries.HeadDown, Brushes.White, Outline);

        public static GD Cross { get; } = Get(Geometries.Cross, Brushes.Gray_3);
        public static GD Check { get; } = Get(Geometries.Check, Brushes.Gray_3);
        public static GD Plus { get; } = Get(Geometries.Plus, Brushes.Gray_3);
        public static GD Minus { get; } = Get(Geometries.Minus, Brushes.Gray_3);
        public static GD Dots { get; } = Get(Geometries.Dots, Brushes.Gray_3);
        public static GD Grid { get; } = Get(Geometries.Grid, Brushes.Gray_3);

        public static GD Plus_LowerRight_Back { get; } = Get(Geometries.Plus_LowerRight_Back, Brushes.Gray_3);
        public static GD Plus_LowerRight_Fore { get; } = Get(Geometries.Plus_LowerRight_Fore, Brushes.Yellow_f0);
        public static GD Minus_LowerRight_Back { get; } = Get(Geometries.Minus_LowerRight_Back, Brushes.Gray_3);
        public static GD Minus_LowerRight_Fore { get; } = Get(Geometries.Minus_LowerRight_Fore, Brushes.Red_f0);

        public static GD Cross_Red { get; } = Get(Geometries.Cross, Brushes.Red_e4);
        public static GD Check_Green { get; } = Get(Geometries.Check, Brushes.Green_e4);

        public static GD ArrowLeft { get; } = Get(Geometries.ArrowLeft, Brushes.Gray_3);
        public static GD ArrowRight { get; } = Get(Geometries.ArrowRight, Brushes.Gray_3);
        public static GD ArrowUp { get; } = Get(Geometries.ArrowUp, Brushes.Gray_3);
        public static GD ArrowDown { get; } = Get(Geometries.ArrowDown, Brushes.Gray_3);

        public static GD Zoom { get; } = Get(Geometries.Zoom, Brushes.Gray_3);
        public static GD ZoomUp { get; } = Get(Geometries.Zoom_Plus, Brushes.Gray_3);
        public static GD ZoomDown { get; } = Get(Geometries.Zoom_Minus, Brushes.Gray_3);

        public static GD Maximize { get; } = Get(Geometries.Maximize, Brushes.Gray_3);
        public static GD Minimize { get; } = Get(Geometries.Minimize, Brushes.Gray_3);
        public static GD ShowInTaskbar { get; } = Get(Geometries.ShowInTaskbar, Brushes.Gray_3);
        public static GD Topmost { get; } = Get(Geometries.Topmost, Brushes.Gray_3);
        public static GD SlipThrough { get; } = Get(Geometries.SlipThrough, Brushes.Gray_3);

        public static GD Clock { get; } = Get(Geometries.Clock, Brushes.White);

        public static GD Question { get; } = Get(Geometries.Question, Brushes.White, Outline);
        public static GD Question_Mono { get; } = Get(Geometries.Question, Brushes.Gray_3);
        public static GD Info { get; } = Get(Geometries.Info, Brushes.White, Outline);

        public static GD Caution_Outer { get; } = Get(Geometries.Circle_16, Brushes.Red_f0);
        public static GD Caution_Inner { get; } = Get(Geometries.Caution, Brushes.White);

        public static GD Attention_Outer { get; } = Get(Geometries.Circle_16, Brushes.BlueGreen_c84);

        public static GD Gear_Outer { get; } = Get(Geometries.Gear, Brushes.Gray_3);
        public static GD Gear_Inner { get; } = Get(Geometries.Gear_S, Brushes.BlueGreen_b98, Outline);

        public static GD Hamburger { get; } = Get(Geometries.Hamburger, Brushes.Gray_6);

        public static GD Update_Outer { get; } = Get(Geometries.Update, Brushes.Green_40);
        public static GD Update_Inner { get; } = Get(Geometries.Update_S, Brushes.GreenBlue_fb3);

        public static GD Update_D_Outer { get; } = Get(Geometries.Update, Brushes.Gray_3);
        public static GD Update_D_Inner { get; } = Get(Geometries.Update_S, Brushes.Gray_8);

        public static GD Download { get; } = Get(Geometries.Download, Brushes.Gray_3);
        public static GD Upload { get; } = Get(Geometries.Upload, Brushes.Gray_3);
        public static GD Json { get; } = Get(Geometries.Json, Brushes.Gray_3);
        public static GD Letter_A { get; } = Get(Geometries.Letter_A, Brushes.Gray_3);

        public static GD VerticalAlign_Top { get; } = Get(Geometries.VerticalAlign_Top, Brushes.Gray_3);
        public static GD VerticalAlign_Center { get; } = Get(Geometries.VerticalAlign_Center, Brushes.Gray_3);
        public static GD VerticalAlign_Bottom { get; } = Get(Geometries.VerticalAlign_Bottom, Brushes.Gray_3);
        public static GD VerticalAlign_Stretch { get; } = Get(Geometries.VerticalAlign_Stretch, Brushes.Gray_3);
        public static GD HorizontalAlign_Left { get; } = Get(Geometries.HorizontalAlign_Left, Brushes.Gray_3);
        public static GD HorizontalAlign_Center { get; } = Get(Geometries.HorizontalAlign_Center, Brushes.Gray_3);
        public static GD HorizontalAlign_Right { get; } = Get(Geometries.HorizontalAlign_Right, Brushes.Gray_3);
        public static GD HorizontalAlign_Stretch { get; } = Get(Geometries.HorizontalAlign_Stretch, Brushes.Gray_3);

        public static GD Scroll_Circle { get; } = Get(Geometries.Circle_14, Brushes.White);
        public static GD Scroll_All { get; } = Get(Geometries.Scroll_All, Brushes.Gray_3);
        public static GD Scroll_Vertical { get; } = Get(Geometries.Scroll_Vertical, Brushes.Gray_3);
        public static GD Scroll_Horizontal { get; } = Get(Geometries.Scroll_Horizontal, Brushes.Gray_3);

        public static GD Play { get; } = Get(Geometries.Play, Brushes.Blue_e4);
        public static GD Play2 { get; } = Get(Geometries.Play2, Brushes.Blue_e4);
        public static GD Pause { get; } = Get(Geometries.Pause, Brushes.Green_e4);
        public static GD Stop { get; } = Get(Geometries.Stop, Brushes.Red_e4);
        public static GD Repeat { get; } = Get(Geometries.Repeat, Brushes.Gray_3);
        public static GD SkipLeft { get; } = Get(Geometries.SkipLeft, Brushes.Gray_3);
        public static GD SkipRight { get; } = Get(Geometries.SkipRight, Brushes.Gray_3);

        public static GD NewWindow_Background { get; } = Get(Geometries.NewWindow_Background, Brushes.White);
        public static GD NewWindow_Frame { get; } = Get(Geometries.NewWindow_Frame, Brushes.Blue_e4);
        public static GD NewWindow_Arrow { get; } = Get(Geometries.NewWindow_Arrow, Brushes.Gray_3);

        public static GD Console_Head { get; } = Get(Geometries.Console_Head, Brushes.White);
        public static GD Console_Body { get; } = Get(Geometries.Console_Body, Brushes.Black);
        public static GD Console_Stroke { get; } = Get(Geometries.Console_Stroke, Brushes.Gray_8);

        public static GD Folder_Background { get; } = Get(Geometries.Folder_Background, Brushes.RedGreen_420);
        public static GD Folder_Foreground { get; } = Get(Geometries.Folder_Foreground, Brushes.RedGreen_fc8);

        public static GD File_Background { get; } = Get(Geometries.File_Background, Brushes.RedGreen_420);
        public static GD File_Foreground { get; } = Get(Geometries.File_Foreground, Brushes.RedGreen_fc8);
        public static GD File_Inner { get; } = Get(Geometries.File_Inner, Brushes.White);

        public static GD New { get; } = Get(Geometries.New, Brushes.Gray_3);

        public static GD Save_Inner { get; } = Get(Geometries.Floppy_Inner, Brushes.Blue_fb);
        public static GD Save_Frame { get; } = Get(Geometries.Floppy_Frame, Brushes.Blue_a4);
        public static GD SaveAs_Inner { get; } = Get(Geometries.Floppy_Inner, Brushes.GreenBlue_fc8);
        public static GD SaveAs_Frame { get; } = Get(Geometries.Floppy_Frame, Brushes.Green_72);
        public static GD Floppy_Label { get; } = Get(Geometries.Floppy_Label, Brushes.White);

        public static GD Save_Mini_Inner { get; } = Get(Geometries.Floppy_Mini_Inner, Brushes.Blue_fb);
        public static GD Save_Mini_Frame { get; } = Get(Geometries.Floppy_Mini_Frame, Brushes.Blue_a4);
        public static GD Floppy_Mini_Label { get; } = Get(Geometries.Floppy_Mini_Label, Brushes.White);

        public static GD Delete_Inner { get; } = Get(Geometries.Bin_Background, Brushes.Red_f8);
        public static GD Delete_Frame { get; } = Get(Geometries.Bin_Foreground, Brushes.Red_82);

        public static GD Delete_Inner_Mono { get; } = Get(Geometries.Bin_Background, Brushes.White);
        public static GD Delete_Frame_Mono { get; } = Get(Geometries.Bin_Foreground, Brushes.Gray_3);

        public static GD Undo { get; } = Get(Geometries.Undo, Brushes.BlueRed_840);
        public static GD Undo_Mono { get; } = Get(Geometries.Undo, Brushes.Gray_3);
        public static GD Redo { get; } = Get(Geometries.Redo, Brushes.RedBlue_840);
        public static GD Redo_Mono { get; } = Get(Geometries.Redo, Brushes.Gray_3);

        public static GD Pencil_Outer { get; } = Get(Geometries.Pencil_Outer, Brushes.Gray_3);
        public static GD Pencil_Inner { get; } = Get(Geometries.Pencil_Inner, Brushes.White);

        public static GD Cut { get; } = Get(Geometries.Cut, Brushes.Gray_3);

        public static GD Copy_Background { get; } = Get(Geometries.Copy_Background, Brushes.Gray_3);
        public static GD Copy_Foreground { get; } = Get(Geometries.Copy_Foreground, Brushes.White);

        public static GD Clipboard_Background { get; } = Get(Geometries.Clipboard_Background, Brushes.RedGreen_420);
        public static GD Clipboard_Foreground { get; } = Get(Geometries.Clipboard_Foreground, Brushes.RedGreen_fc8);
        public static GD Clipboard_Background_Mono { get; } = Get(Geometries.Clipboard_Background, Brushes.Gray_3);
        public static GD Clipboard_Foreground_Mono { get; } = Get(Geometries.Clipboard_Foreground, Brushes.White);
        public static GD Clipboard_Clip { get; } = Get(Geometries.Clipboard_Clip, Brushes.Gray_6);
        public static GD Clipboard_Paper { get; } = Get(Geometries.Clipboard_Paper, Brushes.White);

        public static GD Picture_Background { get; } = Get(Geometries.Picture_Background, Brushes.Gray_3);
        public static GD Picture_Sky { get; } = Get(Geometries.Picture_Sky, Brushes.BlueGreen_fc8);
        public static GD Picture_Mountain { get; } = Get(Geometries.Picture_Mountain, Brushes.Green_72);
        public static GD Picture_Sun { get; } = Get(Geometries.Picture_Sun, Brushes.Red_e4);

        public static GD Camera_Outer { get; } = Get(Geometries.Camera_Outer, Brushes.Gray_3);
        public static GD Camera_Inner { get; } = Get(Geometries.Camera_Inner, Brushes.White);

        public static GD Picture_Sky_Mono { get; } = Get(Geometries.Picture_Sky, Brushes.White);
        public static GD Picture_Mountain_Mono { get; } = Get(Geometries.Picture_Mountain, Brushes.Gray_8);
        public static GD Picture_Sun_Mono { get; } = Get(Geometries.Picture_Sun, Brushes.Gray_3);

        public static GD Document_Back { get; } = Get(Geometries.Document_Back, Brushes.White);
        public static GD Document_Frame { get; } = Get(Geometries.Document_Frame, Brushes.Gray_3);

        public static GD Letter_F0 { get; } = Get(Geometries.Letter_F0, Brushes.Gray_3);
        public static GD Letter_ZZ { get; } = Get(Geometries.Letter_ZZ, Brushes.Gray_3);

        public static GD Merge_Arrow { get; } = Get(Geometries.Merge_Arrow, Brushes.Gray_3);
        public static GD Merge_Outer { get; } = Get(Geometries.Merge_Outer, Brushes.Gray_6);
        public static GD Merge_Inner { get; } = Get(Geometries.Merge_Inner, Brushes.RedGreen_fc8);

        public static GD Split_Arrow { get; } = Get(Geometries.Split_Arrow, Brushes.Gray_3);
        public static GD Split_Outer { get; } = Get(Geometries.Split_Outer, Brushes.Gray_6);
        public static GD Split_Inner { get; } = Get(Geometries.Split_Inner, Brushes.RedGreen_fc8);

        public static GD Wave { get; } = Get(Geometries.Wave, Brushes.Red_f4);

        public static GD Wave_Gain_Zero { get; } = Get(Geometries.Wave_Gain_Zero, Brushes.BlueGreen_f80);
        public static GD Wave_Gain_Mid { get; } = Get(Geometries.Wave_Gain_Mid, Brushes.BlueRed_fc4);
        public static GD Wave_Time { get; } = Get(Geometries.Wave_Time, Brushes.Blue_fb);
        public static GD Wave_Marker { get; } = Get(Geometries.Wave_Marker, Brushes.Green_e0);
        public static GD Wave_Marker_Name { get; } = Get(Geometries.Wave_Marker_Name, Brushes.Green_e0);
        
        public static GD Wave_Marker_Outer { get; } = Get(Geometries.Wave_Marker_Outer, Brushes.Gray_6);
        public static GD Wave_Marker_Single_Outer { get; } = Get(Geometries.Wave_Marker_Single_Outer, Brushes.Gray_6);
        public static GD Wave_Marker_Single { get; } = Get(Geometries.Wave_Marker_Single, Brushes.Green_e0);
        public static GD Wave_Marker_Multi_Outer { get; } = Get(Geometries.Wave_Marker_Multi_Outer, Brushes.Gray_6);
        public static GD Wave_Marker_Multi { get; } = Get(Geometries.Wave_Marker_Multi, Brushes.Green_e0);
        public static GD Wave_Marker_Arrow { get; } = Get(Geometries.Wave_Marker_Arrow, Brushes.Yellow_f0);
        public static GD Wave_Marker_Auto_Outer { get; } = Get(Geometries.Wave_Marker_Auto_Outer, Brushes.Gray_6);
        public static GD Wave_Marker_Auto_Inner { get; } = Get(Geometries.Wave_Marker_Auto_Inner, Brushes.Green_e0);
        public static GD Wave_Marker_Auto_Clear { get; } = Get(Geometries.Wave_Marker_Auto_Clear, Brushes.BlueGreen_fc8);
        public static GD Wave_Marker_Auto_Rect { get; } = Get(Geometries.Wave_Marker_Auto_Rect, Brushes.Yellow_Tf0);

        public static GD Wave_Slice { get; } = Get(Geometries.Wave_Slice, Brushes.Blue_a4);
        public static GD Wave_Slice_Note1 { get; } = Get(Geometries.Wave_Slice_Note1, Brushes.Green_e0);
        public static GD Wave_Slice_Note2 { get; } = Get(Geometries.Wave_Slice_Note2, Brushes.BlueGreen_fc8);
        public static GD Wave_Spectrum_Frame { get; } = Get(Geometries.Wave_Spectrum_Frame, Brushes.Gray_A);
        public static GD Wave_Spectrum_Figure { get; } = Get(Geometries.Wave_Spectrum_Figure, Brushes.Red_f4);

        public static GD Bms_Background { get; } = Get(Geometries.Bms_Background, Brushes.Gray_6);
        public static GD Bms_Red { get; } = Get(Geometries.Bms_Red, Brushes.Red_80);
        public static GD Bms_Scratch { get; } = Get(Geometries.Bms_Scratch, Brushes.Red_f0);
        public static GD Bms_Black { get; } = Get(Geometries.Bms_Black, Brushes.Blue_84);
        public static GD Bms_White { get; } = Get(Geometries.Bms_White, Brushes.White);

        public static GD Bms_Sort_Background1 { get; } = Get(Geometries.Base, Brushes.Red_40);
        public static GD Bms_Sort_Background2 { get; } = Get(Geometries.Bms_Sort_Background, Brushes.Red_80);
        public static GD Bms_Sort_Foreground { get; } = Get(Geometries.Bms_Sort_Foreground, Brushes.Red_f4);
        public static GD Bms_Sort_Highlight { get; } = Get(Geometries.Bms_Sort_Highlight, Brushes.Red_f8);

        public static GD Bms_Sequential_Background1 { get; } = Get(Geometries.Base, Brushes.Blue_84);
        public static GD Bms_Sequential_Background2 { get; } = Get(Geometries.Bms_Sort_Background, Brushes.Gray_6);
        public static GD Bms_Sequential_Foreground { get; } = Get(Geometries.Bms_Sequential, Brushes.White);

        public static GD Midi_Black { get; } = Get(Geometries.Midi_Black, Brushes.Black);
        public static GD Midi_White1 { get; } = Get(Geometries.Midi_White1, Brushes.Gray_8);
        public static GD Midi_White2 { get; } = Get(Geometries.Midi_White2, Brushes.Gray_6);
        public static GD Midi_Note1 { get; } = Get(Geometries.Midi_Note1, Brushes.Red_f8);
        public static GD Midi_Note2 { get; } = Get(Geometries.Midi_Note2, Brushes.BlueGreen_fc8);
        public static GD Midi_Note3 { get; } = Get(Geometries.Midi_Note3, Brushes.GreenBlue_fc8);

        public static GD Midi_BarLine { get; } = Get(Geometries.Midi_BarLine, Brushes.Gray_A);
        public static GD Midi_Velocity { get; } = Get(Geometries.Midi_Velocity, Brushes.BlueRed_fa6);

        public static GD Midi_Enchord_Blue { get; } = Get(Geometries.Midi_Enchord_Notes, Brushes.Blue_e4);
        public static GD Midi_Enchord_Red { get; } = Get(Geometries.Midi_Enchord_Notes, Brushes.Red_e4);
        public static GD Midi_Enchord_Green { get; } = Get(Geometries.Midi_Enchord_Middle, Brushes.Green_72);
        public static GD Midi_Enchord_Purple { get; } = Get(Geometries.Midi_Enchord_Right, Brushes.BlueRed_fc4);
        public static GD Midi_Enchord_Marker { get; } = Get(Geometries.Midi_Enchord_Marker, Brushes.Green_e0);
        public static GD Midi_Enchord_Marker2 { get; } = Get(Geometries.Midi_Enchord_Marker2, Brushes.Green_e0);
        public static GD Midi_Enchord_Rect { get; } = Get(Geometries.Midi_Enchord_Rect, Brushes.Yellow_Tf0);

        public static GD Midi_Group_Notes { get; } = Get(Geometries.Midi_Group_Notes, Brushes.Red_e4);
        public static GD Midi_Group_Marker_Outer { get; } = Get(Geometries.Midi_Group_Marker_Outer, Brushes.Gray_6);
        public static GD Midi_Group_Marker { get; } = Get(Geometries.Midi_Group_Marker, Brushes.Green_e0);
        public static GD Midi_AutoGroup_Left { get; } = Get(Geometries.Midi_AutoGroup_Left, Brushes.Red_e4);
        public static GD Midi_AutoGroup_Right { get; } = Get(Geometries.Midi_AutoGroup_Right, Brushes.BlueRed_fc4);

        public static GD Midi_Sort_Notes { get; } = Get(Geometries.Midi_Sort_Notes, Brushes.Blue_e4);

        public static GD Piano_White { get; } = Get(Geometries.Piano_White, Brushes.White);
        public static GD Piano_Black { get; } = Get(Geometries.Piano_Black, Brushes.Black);
        public static GD Piano_Outline { get; } = Get(Geometries.Piano_Outline, Brushes.Gray_3);
        public static GD Piano_KeySwtich { get; } = Get(Geometries.Piano_KeySwitch, Brushes.Yellow_Tf0);

        public static GD Metronome_Outer { get; } = Get(Geometries.Metronome_Outer, Brushes.RedGreen_420);
        public static GD Metronome_Inner { get; } = Get(Geometries.Metronome_Inner, Brushes.RedGreen_fc8);
        public static GD Metronome_Scale { get; } = Get(Geometries.Metronome_Scale, Brushes.Gray_3);
        public static GD Metronome_Bar { get; } = Get(Geometries.Metronome_Bar, Brushes.Red_f4);

        public static GD Browse_BackTab { get; } = Get(Geometries.Browse_BackTab, Brushes.RedGreen_c62);
        public static GD Browse_FrontTab { get; } = Get(Geometries.Browse_FrontTab, Brushes.RedGreen_fc8);
        public static GD Browse_Content { get; } = Get(Geometries.Browse_Content, Brushes.White);
        public static GD Browse_Outline { get; } = Get(Geometries.Browse_Outline, Brushes.RedGreen_420);
    }
}
