using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using V = LivreNoirLibrary.Media.VectorGraphics;

namespace LivreNoirLibrary.Windows.Media
{
    public static class Icons
    {
        private static readonly Rect _bounds = new(0, 0, 32, 32);
        private static readonly GeometryDrawing _base = CreateBase();

        private static GeometryDrawing CreateBase()
        {
            var rect = new RectangleGeometry(_bounds);
            rect.Freeze();
            var d = new GeometryDrawing(Brushes.Transparent, null, rect);
            d.Freeze();
            return d;
        }

        public static DrawingGroup Create(V.ElementGroup elements, bool freeze = true)
        {
            DrawingGroup dg = new();
            foreach (var element in elements.Children)
            {
                var geometry = MediaUtils.CreateGeometry(element.Geometry);
                var fill = MediaUtils.GetBrush(element.Fill);
                var pen = MediaUtils.GetPen(element.Pen);
                var drawing = new GeometryDrawing(fill, pen, geometry);
                drawing.Freeze();
                dg.Children.Add(drawing);
            }
            if (freeze)
            {
                dg.Freeze();
            }
            return dg;
        }

        private static DrawingGroup CreateInternal(V.ElementGroup elements)
        {
            var dg = Create(elements, false);
            if (dg.Bounds != _bounds)
            {
                dg.Children.Insert(0, _base);
            }
            dg.Freeze();
            return dg;
        }

        public static DrawingGroup Transparent { get; } = CreateInternal(V.Icons.Transparent);

        public static DrawingGroup Cross { get; } = CreateInternal(V.Icons.Cross);
        public static DrawingGroup Check { get; } = CreateInternal(V.Icons.Check);
        public static DrawingGroup Plus { get; } = CreateInternal(V.Icons.Plus);
        public static DrawingGroup Minus { get; } = CreateInternal(V.Icons.Minus);
        public static DrawingGroup Cross_Red { get; } = CreateInternal(V.Icons.Cross_Red);
        public static DrawingGroup Check_Green { get; } = CreateInternal(V.Icons.Check_Green);
        public static DrawingGroup Dots { get; } = CreateInternal(V.Icons.Dots);
        public static DrawingGroup Grid { get; } = CreateInternal(V.Icons.Grid);

        public static DrawingGroup Selection { get; } = CreateInternal(V.Icons.Selection);

        public static DrawingGroup ArrowLeft { get; } = CreateInternal(V.Icons.ArrowLeft);
        public static DrawingGroup ArrowRight { get; } = CreateInternal(V.Icons.ArrowRight);
        public static DrawingGroup ArrowUp { get; } = CreateInternal(V.Icons.ArrowUp);
        public static DrawingGroup ArrowDown { get; } = CreateInternal(V.Icons.ArrowDown);
        public static DrawingGroup RightLeft { get; } = CreateInternal(V.Icons.RightLeft);
        public static DrawingGroup UpDown { get; } = CreateInternal(V.Icons.UpDown);

        public static DrawingGroup HeadLeft { get; } = CreateInternal(V.Icons.HeadLeft);
        public static DrawingGroup HeadRight { get; } = CreateInternal(V.Icons.HeadRight);
        public static DrawingGroup HeadUp { get; } = CreateInternal(V.Icons.HeadUp);
        public static DrawingGroup HeadDown { get; } = CreateInternal(V.Icons.HeadDown);

        public static DrawingGroup Zoom { get; } = CreateInternal(V.Icons.Zoom);
        public static DrawingGroup ZoomUp { get; } = CreateInternal(V.Icons.ZoomUp);
        public static DrawingGroup ZoomDown { get; } = CreateInternal(V.Icons.ZoomDown);

        public static DrawingGroup Maximize { get; } = CreateInternal(V.Icons.Maximize);
        public static DrawingGroup Minimize { get; } = CreateInternal(V.Icons.Minimize);
        public static DrawingGroup ShowInTaskbar { get; } = CreateInternal(V.Icons.ShowInTaskbar);
        public static DrawingGroup Topmost { get; } = CreateInternal(V.Icons.Topmost);
        public static DrawingGroup SlipThrough { get; } = CreateInternal(V.Icons.SlipThrough);

        public static DrawingGroup Clock { get; } = CreateInternal(V.Icons.Clock);

        public static DrawingGroup Download { get; } = CreateInternal(V.Icons.Download);
        public static DrawingGroup Upload { get; } = CreateInternal(V.Icons.Upload);
        public static DrawingGroup Json { get; } = CreateInternal(V.Icons.Json);
        public static DrawingGroup Letter_A { get; } = CreateInternal(V.Icons.Letter_A);

        public static DrawingGroup VerticalAlign_Top { get; } = CreateInternal(V.Icons.VerticalAlign_Top);
        public static DrawingGroup VerticalAlign_Center { get; } = CreateInternal(V.Icons.VerticalAlign_Center);
        public static DrawingGroup VerticalAlign_Bottom { get; } = CreateInternal(V.Icons.VerticalAlign_Bottom);
        public static DrawingGroup VerticalAlign_Stretch { get; } = CreateInternal(V.Icons.VerticalAlign_Stretch);
        public static DrawingGroup HorizontalAlign_Left { get; } = CreateInternal(V.Icons.HorizontalAlign_Left);
        public static DrawingGroup HorizontalAlign_Center { get; } = CreateInternal(V.Icons.HorizontalAlign_Center);
        public static DrawingGroup HorizontalAlign_Right { get; } = CreateInternal(V.Icons.HorizontalAlign_Right);
        public static DrawingGroup HorizontalAlign_Stretch { get; } = CreateInternal(V.Icons.HorizontalAlign_Stretch);

        public static DrawingGroup Scroll_All { get; } = CreateInternal(V.Icons.Scroll_All);
        public static DrawingGroup Scroll_Vertical { get; } = CreateInternal(V.Icons.Scroll_Vertical);
        public static DrawingGroup Scroll_Horizontal { get; } = CreateInternal(V.Icons.Scroll_Horizontal);

        public static DrawingGroup Help { get; } = CreateInternal(V.Icons.Help);
        public static DrawingGroup Question { get; } = CreateInternal(V.Icons.Question);
        public static DrawingGroup Info { get; } = CreateInternal(V.Icons.Info);
        public static DrawingGroup Caution { get; } = CreateInternal(V.Icons.Caution);
        public static DrawingGroup Attention { get; } = CreateInternal(V.Icons.Attention);
        public static DrawingGroup Config { get; } = CreateInternal(V.Icons.Config);
        public static DrawingGroup Volume_0 { get; } = CreateInternal(V.Icons.Volume_0);
        public static DrawingGroup Volume_1 { get; } = CreateInternal(V.Icons.Volume_1);
        public static DrawingGroup Volume_2 { get; } = CreateInternal(V.Icons.Volume_2);
        public static DrawingGroup Volume_3 { get; } = CreateInternal(V.Icons.Volume_3);
        public static DrawingGroup Volume_Mute { get; } = CreateInternal(V.Icons.Volume_Mute);
        public static DrawingGroup Hamburger { get; } = CreateInternal(V.Icons.Hamburger);
        public static DrawingGroup Update { get; } = CreateInternal(V.Icons.Update);
        public static DrawingGroup Update_Disabled { get; } = CreateInternal(V.Icons.Update_Disabled);
        public static DrawingGroup Update_Mono { get; } = CreateInternal(V.Icons.Update_Mono);

        public static DrawingGroup Play { get; } = CreateInternal(V.Icons.Play);
        public static DrawingGroup Play2 { get; } = CreateInternal(V.Icons.Play2);
        public static DrawingGroup Pause { get; } = CreateInternal(V.Icons.Pause);
        public static DrawingGroup Stop { get; } = CreateInternal(V.Icons.Stop);
        public static DrawingGroup Repeat { get; } = CreateInternal(V.Icons.Repeat);
        public static DrawingGroup SkipLeft { get; } = CreateInternal(V.Icons.SkipLeft);
        public static DrawingGroup SkipRight { get; } = CreateInternal(V.Icons.SkipRight);

        public static DrawingGroup NewWindow { get; } = CreateInternal(V.Icons.NewWindow);
        public static DrawingGroup Console { get; } = CreateInternal(V.Icons.Console);
        public static DrawingGroup New { get; } = CreateInternal(V.Icons.New);
        public static DrawingGroup Folder { get; } = CreateInternal(V.Icons.Folder);
        public static DrawingGroup Open { get; } = CreateInternal(V.Icons.Open);
        public static DrawingGroup Save { get; } = CreateInternal(V.Icons.Save);
        public static DrawingGroup SaveAs { get; } = CreateInternal(V.Icons.SaveAs);
        public static DrawingGroup Delete { get; } = CreateInternal(V.Icons.Delete);
        public static DrawingGroup Delete_Mono { get; } = CreateInternal(V.Icons.Delete_Mono);

        public static DrawingGroup Undo { get; } = CreateInternal(V.Icons.Undo);
        public static DrawingGroup Undo_Mono { get; } = CreateInternal(V.Icons.Undo_Mono);
        public static DrawingGroup Redo { get; } = CreateInternal(V.Icons.Redo);
        public static DrawingGroup Redo_Mono { get; } = CreateInternal(V.Icons.Redo_Mono);
        public static DrawingGroup Edit { get; } = CreateInternal(V.Icons.Edit);
        public static DrawingGroup Cut { get; } = CreateInternal(V.Icons.Cut);
        public static DrawingGroup Copy { get; } = CreateInternal(V.Icons.Copy);
        public static DrawingGroup Paste { get; } = CreateInternal(V.Icons.Paste);
        public static DrawingGroup Paste_Mono { get; } = CreateInternal(V.Icons.Paste_Mono);

        public static DrawingGroup Picture { get; } = CreateInternal(V.Icons.Picture);
        public static DrawingGroup Picture_Mono { get; } = CreateInternal(V.Icons.Picture_Mono);

        public static DrawingGroup Camera { get; } = CreateInternal(V.Icons.Camera);

        public static DrawingGroup Document { get; } = CreateInternal(V.Icons.Document);

        public static DrawingGroup Merge { get; } = CreateInternal(V.Icons.Merge);
        public static DrawingGroup Split { get; } = CreateInternal(V.Icons.Split);

        public static DrawingGroup Wave { get; } = CreateInternal(V.Icons.Wave);
        public static DrawingGroup Wave_Gain { get; } = CreateInternal(V.Icons.Wave_Gain);
        public static DrawingGroup Wave_Time { get; } = CreateInternal(V.Icons.Wave_Time);
        public static DrawingGroup Wave_Marker { get; } = CreateInternal(V.Icons.Wave_Marker);
        public static DrawingGroup Wave_Marker_Name { get; } = CreateInternal(V.Icons.Wave_Marker_Name);

        public static DrawingGroup Wave_Marker_Add { get; } = CreateInternal(V.Icons.Wave_Marker_Add);
        public static DrawingGroup Wave_Marker_Single { get; } = CreateInternal(V.Icons.Wave_Marker_Single);
        public static DrawingGroup Wave_Marker_Multi { get; } = CreateInternal(V.Icons.Wave_Marker_Multi);
        public static DrawingGroup Wave_Marker_Auto { get; } = CreateInternal(V.Icons.Wave_Marker_Auto);
        public static DrawingGroup Wave_Marker_Clear { get; } = CreateInternal(V.Icons.Wave_Marker_Clear);

        public static DrawingGroup Wave_Slice { get; } = CreateInternal(V.Icons.Wave_Slice);
        public static DrawingGroup Wave_Slice_Note { get; } = CreateInternal(V.Icons.Wave_Slice_Note);
        public static DrawingGroup Wave_Spectrum { get; } = CreateInternal(V.Icons.Wave_Spectrum);

        public static DrawingGroup Bms { get; } = CreateInternal(V.Icons.Bms);
        public static DrawingGroup Bms_Sort { get; } = CreateInternal(V.Icons.Bms_Sort);
        public static DrawingGroup Bms_Sequential { get; } = CreateInternal(V.Icons.Bms_Sequential);
        public static DrawingGroup Bms_DefList { get; } = CreateInternal(V.Icons.Bms_DefList);
        public static DrawingGroup Bms_MultiDef { get; } = CreateInternal(V.Icons.Bms_MultiDef);

        public static DrawingGroup Midi { get; } = CreateInternal(V.Icons.Midi);

        public static DrawingGroup Midi_Background { get; } = CreateInternal(V.Icons.Midi_Background);
        public static DrawingGroup Midi_BarLine { get; } = CreateInternal(V.Icons.Midi_BarLine);
        public static DrawingGroup Midi_Onion { get; } = CreateInternal(V.Icons.Midi_Onion);
        public static DrawingGroup Midi_Velocity { get; } = CreateInternal(V.Icons.Midi_Velocity);
        public static DrawingGroup Midi_SysEx { get; } = CreateInternal(V.Icons.Midi_SysEx);
        public static DrawingGroup Midi_Enchord { get; } = CreateInternal(V.Icons.Midi_Enchord);
        public static DrawingGroup Midi_Enchord_NoMark { get; } = CreateInternal(V.Icons.Midi_Enchord_NoMark);
        public static DrawingGroup Midi_Dechord { get; } = CreateInternal(V.Icons.Midi_Dechord);
        public static DrawingGroup Midi_MultiGroup { get; } = CreateInternal(V.Icons.Midi_MultiGroup);
        public static DrawingGroup Midi_Group { get; } = CreateInternal(V.Icons.Midi_Group);
        public static DrawingGroup Midi_AutoGroup { get; } = CreateInternal(V.Icons.Midi_AutoGroup);

        public static DrawingGroup Midi_Sort { get; } = CreateInternal(V.Icons.Midi_Sort);

        public static DrawingGroup Midi_KeySwitch { get; } = CreateInternal(V.Icons.Midi_KeySwitch);
        public static DrawingGroup Metronome { get; } = CreateInternal(V.Icons.Metronome);

        public static DrawingGroup Browse { get; } = CreateInternal(V.Icons.Browse);

        private static IconInfo[] CreateIconList()
        {
            var list = new List<IconInfo>();
            foreach (var prop in typeof(Icons).GetProperties())
            {
                if (prop.PropertyType.IsAssignableTo(typeof(Drawing)))
                {
                    var icon = (prop.GetValue(null) as Drawing)!;
                    list.Add(new(prop.Name, icon));
                }
            }
            return [.. list];
        }

        private static IconInfo[] _iconList = CreateIconList();
        public static IEnumerable<IconInfo> IconList => _iconList;
    }

    public record IconInfo(string Name, Drawing Drawing);
}
