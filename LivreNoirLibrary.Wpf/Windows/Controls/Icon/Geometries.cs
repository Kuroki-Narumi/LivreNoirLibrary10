using System;
using System.Windows.Media;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls.IconContent
{
    using SG = StreamGeometry;

    public static class Geometries
    {
        public static SG Base { get; } = CreateGeometry("M0,0 h32 v32 h-32 Z");
        public static SG Rect_24 { get; } = CreateGeometry("M4,4 h24 v24 h-24 Z");

        public static SG Circle(double r) => CreateGeometry($"M{16-r},16 a{r},{r},0,0,0,{r*2},0 a{r},{r},0,0,0,{-r*2},0 Z");

        public static SG Circle_16 { get; } = Circle(16);
        public static SG Circle_15 { get; } = Circle(15);
        public static SG Circle_14 { get; } = Circle(14);
        public static SG Circle_12 { get; } = Circle(12);

        public static SG Cross { get; } = CreateGeometry("M0,4 l12,12 -12,12 4,4 12,-12 12,12 4,-4 -12,-12 12,-12 -4,-4 -12,12 -12,-12 Z");
        public static SG Cross_S { get; } = CreateGeometry("M2,4 l12,12 -12,12 2,2 12,-12 12,12 2,-2 -12,-12 12,-12 -2,-2 -12,12 -12,-12 Z");
        public static SG Check { get; } = CreateGeometry("M0,18 l11,11 l21,-21 l-4,-4 l-17,17 l-7,-7 Z");
        public static SG Check_S { get; } = CreateGeometry("M2,18 l9,9 l19,-19 l-2,-2 l-17,17 l-7,-7 Z");

        public static SG Plus { get; } = CreateGeometry("M0,13 v6 h13 v13 h6 v-13 h13 v-6 h-13 v-13 h-6 v13 Z");
        public static SG Plus_S { get; } = CreateGeometry("M2,14.5 v3 h12.5 v12.5 h3 v-12.5 h12.5 v-3 h-12.5 v-12.5 h-3 v12.5 Z");
        public static SG Minus { get; } = CreateGeometry("M0,13 v6 h32 v-6 Z");
        public static SG Minus_S { get; } = CreateGeometry("M2,14.5 v3 h28 v-3 Z");

        public static SG Dots { get; } = CreateGeometry("M2,2 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m2,2 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-2,-2 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z");
        public static SG Grid { get; } = CreateGeometry("M0,4 h4 v-4 h2 v4 h6 v-4 h2 v4 h6 v-4 h2 v4 h6 v-4 h2 v4 h2 v2 h-2 v6 h2 v2 h-2 v6 h2 v2 h-2 v6 h2 v2 h-2 v2 h-2 v-2 h-6 v2 h-2 v-2 h-6 v2 h-2 v-2 h-6 v2 h-2 v-2 h-4 v-2 h4 v-6 h-4 v-2 h4 v-6 h-4 v-2 h4 v-6 h-4 Z M6,6 h6 v6 h-6 Z M14,6 h6 v6 h-6 Z M22,6 h6 v6 h-6 Z M6,14 h6 v6 h-6 Z M14,14 h6 v6 h-6 Z M22,14 h6 v6 h-6 Z M6,22 h6 v6 h-6 Z M14,22 h6 v6 h-6 Z M22,22 h6 v6 h-6 Z");

        public static SG ArrowLeft { get; } = CreateGeometry("M0,16 l15,15 l4,-4 l-8,-8 h20 v-6 h-20 l8,-8 l-4,-4 Z");
        public static SG ArrowRight { get; } = CreateGeometry("M32,16 l-15,15 l-4,-4 l8,-8 h-20 v-6 h20 l-8,-8 l4,-4 Z");
        public static SG ArrowUp { get; } = CreateGeometry("M16,0 l15,15 l-4,4 l-8,-8 v20 h-6 v-20 l-8,8 l-4,-4 Z");
        public static SG ArrowDown { get; } = CreateGeometry("M16,32 l15,-15 l-4,-4 l-8,8 v-20 h-6 v20 l-8,-8 l-4,4 Z");

        public static SG HeadLeft { get; } = CreateGeometry("M8,16 l10,10 l4,-4 l-6,-6 l6,-6 l-4,-4 Z");
        public static SG HeadRight { get; } = CreateGeometry("M24,16 l-10,10 l-4,-4 l6,-6 l-6,-6 l4,-4 Z");
        public static SG HeadUp { get; } = CreateGeometry("M16,8 l10,10 l-4,4 l-6,-6 l-6,6 l-4,-4 Z");
        public static SG HeadDown { get; } = CreateGeometry("M16,24 l10,-10 l-4,-4 l-6,6 l-6,-6 l-4,4 Z");

        public static SG Zoom { get; } = CreateGeometry("M17.714,19.714 A11,11,0,0,1,3.223,3.223 A11,11,0,0,1,19.714,17.714 L22,20 h2 L32,28 L28,32 L20,24 v-2 Z M11,2 a9,9,0,0,0,0,18 a9,9,0,0,0,0,-18 Z");
        public static SG Zoom_Plus { get; } = CreateGeometry("M5,10 h5 v-5 h2 v5 h5 v2 h-5 v5 h-2 v-5 h-5 Z");
        public static SG Zoom_Minus { get; } = CreateGeometry("M4,10 h14 v2 h-14 Z");

        public static SG Maximize { get; } = CreateGeometry("M0,0 v10 h3 v-5 l9,9 l2,-2 l-9,-9 h5 v-3 Z M32,0 v10 h-3 v-5 l-9,9 l-2,-2 l9,-9 h-5 v-3 Z M0,32 v-10 h3 v5 l9,-9 l2,2 l-9,9 h5 v3 Z M32,32 v-10 h-3 v5 l-9,-9 l-2,2 l9,9 h-5 v3 Z");
        public static SG Minimize { get; } = CreateGeometry("M15,17 v10 h-3 v-5 l-9,9 l-2,-2 l9,-9 h-5 v-3 Z M17,15 v-10 h3 v5 l9,-9 l2,2 l-9,9 h5 v3 Z");
        public static SG ShowInTaskbar { get; } = CreateGeometry("M0,32 h32 v-12 h-32 Z M2,30 h18 v-8 h-18 Z M16,20 l10,-10 l-3,-3 l-5,5 v-12 h-4 v12 l-5,-5 l-3,3 Z");
        public static SG Topmost { get; } = CreateGeometry("M0,0 h32 v12 h-32 Z M2,4 h28 v6 h-28 Z M16,12 l10,10 l-3,3 l-5,-5 v12 h-4 v-12 l-5,5 l-3,-3 Z");
        public static SG SlipThrough { get; } = CreateGeometry("M2,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m2,2 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m-16,16 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z  m-2,-2 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z M16,9 l1.072,17.856 4.196,-4.732 5.5,9.876 2.232,0 1.232,-2.349 -5.5,-9.527 6.196,-1.268 Z");

        public static SG Clock { get; } = CreateGeometry("M15,2.036 V4 h-1 v14 h8 v-4 h-4 v-10 h-1 V2.036 A14,14,0,0,1,29.964,15 H24 v2 H29.964 A14,14,0,0,1,17,29.964 V24 h-2 V29.964 A14,14,0,0,1,2.036,17 H8 v-2 H2.036 A14,14,0,0,1,15,2.036 Z");

        public static SG Question { get; } = CreateGeometry("M6,12 C6,8,10,3,16,3 22,3,26,8,26,12 26,16,21,17,19,19 V22 H13 V19 C13,17,14,16,16,15 18,14,20,13,20,12 20,10,18,9,16,9 14,9,12,10,12,12 Z M16,23 a3,3,0,0,0,0,6 3,3,0,0,0,0,-6 Z");
        public static SG Caution { get; } = CreateGeometry("M2,16 a14,14,0,0,0,28,0 a14,14,0,0,0,-28,0 Z M4,16 a12,12,0,0,0,24,0 a12,12,0,0,0,-24,0 M13,20 h6 L20,6 h-8 Z M16,21 a3,3,0,0,0,0,6 3,3,0,0,0,0,-6 Z");
        public static SG Info { get; } = CreateGeometry("M12,6 a4,4,0,0,0,8,0 a4,4,0,0,0,-8,0 Z M12,16 v10 a4,4,0,0,0,8,0 v-10 a4,4,0,0,0,-8,0 Z");

        public static SG Gear { get; } = CreateGeometry("M32,19.069 26.563,19.069 26.163,20.21 25.639,21.299 29.484,25.144 25.144,29.484 21.299,25.639 20.21,26.163 19.069,26.563 19.069,32 12.931,32 12.931,26.563 11.79,26.163 10.701,25.639 6.856,29.484 2.516,25.144 6.361,21.299 5.837,20.21 5.437,19.069 0,19.069 0,12.931 5.437,12.931 5.837,11.79 6.361,10.701 2.516,6.856 6.856,2.516 10.701,6.361 11.79,5.837 12.931,5.437 12.931,0 19.069,0 19.069,5.437 20.21,5.837 21.299,6.361 25.144,2.516 29.484,6.856 25.639,10.701 26.163,11.79 26.563,12.931 32,12.931 Z M11,16 a5,5,0,0,0,10,0 a5,5,0,0,0,-10,0 Z");
        public static SG Gear_S { get; } = CreateGeometry("M31,17.951 25.808,17.951 25.239,19.827 24.315,21.556 27.986,25.227 25.227,27.986 21.556,24.315 19.827,25.239 17.951,25.808 17.951,31 14.049,31 14.049,25.808 12.173,25.239 10.444,24.315 6.773,27.986 4.014,25.227 7.685,21.556 6.761,19.827 6.192,17.951 1,17.951 1,14.049 6.192,14.049 6.761,12.173 7.685,10.444 4.014,6.773 6.773,4.014 10.444,7.685 12.173,6.761 14.049,6.192 14.049,1 17.951,1 17.951,6.192 19.827,6.761 21.556,7.685 25.227,4.014 27.986,6.773 24.315,10.444 25.239,12.173 25.808,14.049 31,14.049 Z M10,16 a6,6,0,0,0,12,0 a6,6,0,0,0,-12,0 Z");

        public static SG Hamburger { get; } = CreateGeometry("M4,4 h24 a2,2,0,0,1,0,4 h-24 a2,2,0,0,1,0,-4 Z M4,14 h24 a2,2,0,0,1,0,4 h-24 a2,2,0,0,1,0,-4 Z M4,24 h24 a2,2,0,0,1,0,4 h-24 a2,2,0,0,1,0,-4 Z");
        public static SG Update { get; } = CreateGeometry("M0,16 h10 l-3,-3 c0,0 2,-6 9,-6 c7,0 9,6 9,6 h4 v-4 c0,0 -5,-8 -13,-8 c-8,0 -13,8 -13,8 l-3,-3 Z M32,16 h-10 l3,3 c0,0 -2,6 -9,6 c-7,0 -9,-6 -9,-6 h-4 v4 c0,0 5,8 13,8 c8,0 13,-8 13,-8 l3,3 Z");
        public static SG Update_S { get; } = CreateGeometry("M1,15 h7 l-2,-2 c0,0 2,-7 10,-7 c7.5,0 9.5,6 9.5,6 h2.5 v-2.5 c0,0 -4.5,-7.5 -12,-7.5 c-8,0 -13,9 -13,8 l-2,-2 Z M31,17 h-7 l2,2 c0,0 -2,7 -10,7 c-7.5,0 -9.5,-6 -9.5,-6 h-2.5 v2.5 c0,0 4.5,7.5 12,7.5 c8,0 13,-9 13,-8 l2,2 Z");
        public static SG Download { get; } = CreateGeometry("M4,28 h24 v4 h-24 Z M16,28 l12,-12 l-3,-3 l-7,7 v-20 h-4 v20 l-7,-7 l-3,3 Z");
        public static SG Upload { get; } = CreateGeometry("M4,0 h24 v4 h-24 Z M16,4 l12,12 l-3,3 l-7,-7 v20 h-4 v-20 l-7,7 l-3,-3 Z");
        public static SG Json { get; } = CreateGeometry("M13,0 v5 h-3 c-1,0 -2,1 -2,2 v5 c0,2 -1,3 -3,4 c2,1 3,2 3,4 v5 c0,1 1,2 2,2 h3 v5 h-3 c-4,0 -7,-3 -7,-7 v-5 c0,-2 -1,-3 -3,-4 c2,-1 3,-2 3,-4 v-5 c0,-4 3,-7 7,-7 Z M19,0 v5 h3 c1,0 2,1 2,2 v5 c0,2 1,3 3,4 c-2,1 -3,2 -3,4 v5 c0,1 -1,2 -2,2 h-3 v5 h3 c4,0 7,-3 7,-7 v-5 c0,-2 1,-3 3,-4 c-2,-1 -3,-2 -3,-4 v-5 c0,-4 -3,-7 -7,-7 Z");
        public static SG Letter_A { get; } = CreateGeometry("M12,0 L2,32 H9 L11,25 H21 L23,32 H30 L20,0 Z M16,6 L12,19 H20 Z");

        public static SG VerticalAlign_Top { get; } = CreateGeometry("M2,0 h28 v2 h-28 Z M2,6 h28 v2 h-28 Z M2,12 h28 v2 h-28 Z M2,18 h28 v2 h-28 Z");
        public static SG VerticalAlign_Center { get; } = CreateGeometry("M2,6 h28 v2 h-28 Z M2,12 h28 v2 h-28 Z M2,18 h28 v2 h-28 Z M2,24 h28 v2 h-28 Z");
        public static SG VerticalAlign_Bottom { get; } = CreateGeometry("M2,12 h28 v2 h-28 Z M2,18 h28 v2 h-28 Z M2,24 h28 v2 h-28 Z M2,30 h28 v2 h-28 Z");
        public static SG VerticalAlign_Stretch { get; } = CreateGeometry("M2,0 h28 v2 h-28 Z M2,10 h28 v2 h-28 Z M2,20 h28 v2 h-28 Z M2,30 h28 v2 h-28 Z");
        public static SG HorizontalAlign_Left { get; } = CreateGeometry("M0,2 h32 v2 h-32 Z M0,8 h24 v2 h-24 Z M0,14 h32 v2 h-32 Z M0,20 h24 v2 h-24 Z M0,26 h32 v2 h-32 Z");
        public static SG HorizontalAlign_Center { get; } = CreateGeometry("M0,2 h32 v2 h-32 Z M4,8 h24 v2 h-24 Z M0,14 h32 v2 h-32 Z M4,20 h24 v2 h-24 Z M0,26 h32 v2 h-32 Z");
        public static SG HorizontalAlign_Right { get; } = CreateGeometry("M0,2 h32 v2 h-32 Z M8,8 h24 v2 h-24 Z M0,14 h32 v2 h-32 Z M8,20 h24 v2 h-24 Z M0,26 h32 v2 h-32 Z");
        public static SG HorizontalAlign_Stretch { get; } = CreateGeometry("M0,2 h32 v2 h-32 Z M0,8 h32 v2 h-32 Z M0,14 h32 v2 h-32 Z M0,20 h32 v2 h-32 Z M0,26 h32 v2 h-32 Z");

        public static SG Scroll_All { get; } = CreateGeometry("M14,16 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 Z M16,4 l4,4 v2 h-1 l-3,-3 l-3,3 h-1 v-2 Z M16,28 l4,-4 v-2 h-1 l-3,3 l-3,-3 h-1 v2 Z M4,16 l4,-4 h2 v1 l-3,3 l3,3 v1 h-2 Z M28,16 l-4,-4 h-2 v1 l3,3 l-3,3 v1 h2 Z");
        public static SG Scroll_Vertical { get; } = CreateGeometry("M14,16 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 Z M16,4 l4,4 v2 h-1 l-3,-3 l-3,3 h-1 v-2 Z M16,28 l4,-4 v-2 h-1 l-3,3 l-3,-3 h-1 v2 Z");
        public static SG Scroll_Horizontal { get; } = CreateGeometry("M14,16 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 Z M4,16 l4,-4 h2 v1 l-3,3 l3,3 v1 h-2 Z M28,16 l-4,-4 h-2 v1 l3,3 l-3,3 v1 h2 Z");

        public static SG Play { get; } = CreateGeometry("M2,0 30,16 2,32 Z");
        public static SG Play2 { get; } = CreateGeometry("M0,0 h4 v32 h-4 Z M6,0 32,16 6,32 Z");
        public static SG Pause { get; } = CreateGeometry("M1,1 H14 V31 H1 Z M18,1 H31 V31 H18 Z");
        public static SG Stop { get; } = CreateGeometry("M1,1 H31 V31 H1 Z");
        public static SG Repeat { get; } = CreateGeometry("M2,18 a14,14,0,0,0,28,0 h-6 a8,8,0,0,1,-16,0 a8,8,0,0,1,8,-8 v4 l10,-7 -10,-7 v4 a14,14,0,0,0,-14,14 Z");
        public static SG SkipLeft { get; } = CreateGeometry("M0,0 v32 h4 V0 M4,16 18,32 18,0 M 18,16 32,32 32,0 Z");
        public static SG SkipRight { get; } = CreateGeometry("M28,0 v32 h32 V0 M28,16 14,32 14,0 M 14,16 0,32 0,0 Z");

        public static SG NewWindow_Background { get; } = CreateGeometry("M0,4 H30 V32 H0 Z");
        public static SG NewWindow_Frame { get; } = CreateGeometry("M0,4 H18 v2 H2 v4 H14 v2 H2 V30 H28 V14 h2 V32 H0 Z");
        public static SG NewWindow_Arrow { get; } = CreateGeometry("M32,0 V13 L27,8 L17,18 H14 V15 L24,5 L19,0 Z");

        public static SG Console_Head { get; } = CreateGeometry("M2,2 v4 h28 v-4");
        public static SG Console_Body { get; } = CreateGeometry("M2,6 v24 h28 v-24");
        public static SG Console_Stroke { get; } = CreateGeometry("M2,2 V30 H30 V2 H2 V1 H31 V31 H1 V1 H2 Z M4,8 v2 h20 v-2 M4,12 v2 h14 v-2 M4,16 v2 h18 v-2 M4,20 v2 h8 v-2 M4,24 v2 h24 v-2");

        public static SG Folder_Background { get; } = CreateGeometry("M0,2 h10 l4,4 h14 v10 h4 l-7,14 h-25 Z");
        public static SG Folder_Foreground { get; } = CreateGeometry("M1.5,3.5 h8 l4,4 h13 v8.5 h-21 l-4,8 Z M6.5,17.5 h23 l-5.5,11 h-22.5 v-1 Z");

        public static SG File_Background { get; } = CreateGeometry("M0,2 h10 l4,4 h14 v4 h2 v6 h2 l-7,14 h-25 Z");
        public static SG File_Foreground { get; } = CreateGeometry("M1.5,3.5 h8 l4,4 h13 v2.5 h-22.5 v9 l-2.5,5 Z M6.5,17.5 h23 l-5.5,11 h-22.5 v-1 Z");
        public static SG File_Inner { get; } = CreateGeometry("M5,11 h24 v5 h-23.5 l-0.5,1");

        public static SG New { get; } = CreateGeometry("M0,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m4,0 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m0,4 h2 v2 h-2 z m-12,12 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m-4,0 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z m0,-4 h2 v2 h-2 z M32,26 h-6 v6 h-4 v-6 h-6 v-4 h6 v-6 h4 v6 h6 z");

        public static SG Floppy_Inner { get; } = CreateGeometry("M3,1 1,3 V29 L3,31 H29 L31,29 V8 L24,1 Z");
        public static SG Floppy_Frame { get; } = CreateGeometry("M4,0 a4,4,0,0,0,-4,4 V28 a4,4,0,0,0,4,4 H28 a4,4,0,0,0,4,-4 V8 L24,0 Z M4,2 a2,2,0,0,0,-2,2 V28 a2,2,0,0,0,2,2 H28 a2,2,0,0,0,2,-2 V9 L23,2 V9 H9 V2 Z M18,2 h3 v5 h-3 Z");
        public static SG Floppy_Label { get; } = CreateGeometry("M6,16 h20 v14 h-20 Z");
        public static SG Plus_LowerRight_Back { get; } = CreateGeometry("M32,26 h-6 v6 h-5 v-6 h-6 v-5 h6 v-6 h5 v6 h6 z");
        public static SG Plus_LowerRight_Fore { get; } = CreateGeometry("M31,25 h-6 v6 h-3 v-6 h-6 v-3 h6 v-6 h3 v6 h6 z");
        public static SG Minus_LowerRight_Back { get; } = CreateGeometry("M32,26 h-17 v-5 h17 z");
        public static SG Minus_LowerRight_Fore { get; } = CreateGeometry("M31,25 h-15 v-3 h15 z");

        public static SG Floppy_Mini_Inner { get; } = CreateGeometry("M18,17 l-1,1 v12 l1,1 h12 l1,-1 v-9 l-4,-4 Z");
        public static SG Floppy_Mini_Frame { get; } = CreateGeometry("M18,16 a2,2,0,0,0,-2,2 v12 a2,2,0,0,0,2,2 h12 a2,2,0,0,0,2,-2 v-10 l-4,-4 M19,18 a1,1,0,0,0,-1,1 v10 a1,1,0,0,0,1,1 h10 a1,1,0,0,0,1,-1 v-8 l-3,-3 v4 h-6 v-4 Z");
        public static SG Floppy_Mini_Label { get; } = CreateGeometry("M20,24 h8 v4 h-8 Z");

        public static SG Bin_Background { get; } = CreateGeometry("m4,8 2,21 2,2 16,0 2,-2 2,-21");
        public static SG Bin_Foreground { get; } = CreateGeometry("M3,7 L5,29 c0,2 1,3 3,3 h16 c2,0 3,-1 3,-3 L29,7 Z M5,9 7,29 c0,0,0,1,1,1 h16 c0,0,1,0,1,-1 L27,9 Z M15,13.5 v13 a1,1,0,0,0,2,0 v-13 a1,1,0,0,0,-2,0 Z M9.38,13.565 l0.794,12.976 a1,1,0,0,0,1.996,-0.122 l-0.794,-12.976 a1,1,0,0,0,-1.996,0.122 Z M20.624,13.443 l-0.794,12.976 a1,1,0,0,0,1.996,0.122 l0.794,-12.976 a1,1,0,0,0,-1.996,-0.122Z M4,3 l8,-1 l0.5,-1 c0.1,-0.5 0.4,-1 1,-1 h5 c0.6,0 0.9,0.5 1,1 L20,2 L28,3 c1,0.2 2,1 2,2 v1 h-28 v-1 c0,-1 1,-1.8, 2,-2 Z M14.5,1 h3 a0.5,0.5,0,0,1,0,1 h-3 a0.5,0.5,0,0,1,0,-1 Z");

        public static SG Undo { get; } = CreateGeometry("M12,32 L27,17 Q33,9,27,3 Q21,-3,13,3 L8,8 l-4,-4 v11 h11 l-4,-4 L16,6 Q21,3,24,6 Q27,9,24,14 L9,29 Z");
        public static SG Redo { get; } = CreateGeometry("M20,32 L5,17 Q-1,9,5,3 Q11,-3,19,3 L24,8 l4,-4 v11 h-11 l4,-4 L16,6 Q11,3,8,6 Q5,9,8,14 L23,29 Z");

        public static SG Pencil_Outer { get; } = CreateGeometry("M0,32 l10,-2 l20,-20 c2,-2,2,-6,0,-8 c-2,-2,-6,-2,-8,0 l-20,20 Z");
        public static SG Pencil_Inner { get; } = CreateGeometry("M2,30 l6.4,-1.6 c0,0,-0.8,-4,-4.8,-4.8 Z M22,5 l5,5 l-17,17 c0,0,-1,-4,-5,-5 Z M23.5,3.5 l5,5 C31,4,28,1,23.5,3.5 Z");
        public static SG Cut { get; } = CreateGeometry("M6,28 a4,4,0,0,0,8,0 a4,4,0,0,0,-8,0 Z M8,28 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 M18,28 a4,4,0,0,0,8,0 a4,4,0,0,0,-8,0 Z M20,28 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 M10,0 l-2,4 L14,16 10.488,23.024 13.688,24.623 16,20 18.312,24.624 21.512,23.024 18,16 24,4 l-2,-4 L16,12 Z");

        public static SG Copy_Background { get; } = CreateGeometry("M2,8 l8,-8 h12 a2,2,0,0,1,2,2 v4 h4 a2,2,0,0,1,2,2 v22 a2,2,0,0,1,-2,2 h-18 a2,2,0,0,1,-2,-2 v-4 h-4 a2,2,0,0,1,-2,-2 Z");
        public static SG Copy_Foreground { get; } = CreateGeometry("M3.5,9.5 h6 a2,2,0,0,0,2,-2 v-6 h10 a1,1,0,0,1,1,1 v21 a1,1,0,0,1,-1,1 h-17 a1,1,0,0,1,-1,-1 Z M4,8 h5 a1,1,0,0,0,1,-1 v-5 Z M24,7.5 h3.5 a1,1,0,0,1,1,1 v21 a1,1,0,0,1,-1,1 h-17 a1,1,0,0,1,-1,-1 v-3.5 h12.5 a2,2,0,0,0,2,-2 Z M14,6 h6 v1.5 h-6 Z M6,10.5 h14 v1.5 h-14 Z M6,15 h14 v1.5 h-14 Z M6,19.5 h14 v1.5 h-14 Z");

        public static SG Clipboard_Background { get; } = CreateGeometry("M2,2 h5 v4 h14 v-4 h5 l2,2 v8 l4,4 v16 h-16 v-2 h-14 l-2,-2 v-24 Z");
        public static SG Clipboard_Foreground { get; } = CreateGeometry("M2.5,3.5 h3 v4 h17 v-4 h3 l1,1 v6 l-1,-1 h-9.5 v19 h-13.5 l-1,-1 v-23 Z");
        public static SG Clipboard_Clip { get; } = CreateGeometry("M5,8 v-4 l1,-1 h4 v-2 l1,-1 h6 l1,1 v2 h4 l1,1 v4 h-18 Z M12.5,2.5 v3 h3 v-3 Z");
        public static SG Clipboard_Paper { get; } = CreateGeometry("M17.5,11 h7 v6.5 h6 v13 h-13 Z M26,12 v4 h4 Z");

        public static SG Picture_Background { get; } = CreateGeometry("M0,4 h32 v24 h-32 Z");
        public static SG Picture_Sky { get; } = CreateGeometry("M1,5 h30 V24 L20,13 16,17 8,9 1,16 Z M21,10 a3,3,0,0,0,6,0 a3,3,0,0,0,-6,0");
        public static SG Picture_Mountain { get; } = CreateGeometry("M1,18 8,11 24,27 H1 Z M17,18 20,15 31,26 V27 H26");
        public static SG Picture_Sun { get; } = CreateGeometry("M22,10 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 Z");

        public static SG Camera_Outer { get; } = CreateGeometry("M2,7 h1 v-2 h4 v2 H8 l4,-4 H20 l4,4 H30 a2,2,0,0,1,2,2 V27 a2,2,0,0,1,-2,2 H2 a2,2,0,0,1,-2,-2 V9 a2,2,0,0,1,2,-2 Z");
        public static SG Camera_Inner { get; } = CreateGeometry("M16,10 a4,4,0,0,0,0,16 a4,4,0,0,0,0,-16 Z M16,12 a3,3,0,0,0,0,12 a3,3,0,0,0,0,-12 M26,10 h4 v2 h-4 Z");

        public static SG Document_Back { get; } = CreateGeometry("M5,1 h15 l7,7 V31 h-22 Z");
        public static SG Document_Frame { get; } = CreateGeometry("M4,0 h16 l8,8 V32 h-24 Z M6,2 h12 v8 h8 V30 h-20 Z M20,2 v6 h6 Z M8,8 v2 h8 v-2 Z M8,14 v2 h14 v-2 Z M8,20 v2 h12 v-2 Z M8,26 v2 h14 v-2 Z");

        public static SG Letter_F0 { get; } = CreateGeometry("M2,2 h13 v4 h-9 v8 h9 v4 h-9 V30 h-4 Z M17,2 h13 v28 h-13 Z M21,6 h5 v20 h-5 Z");
        public static SG Letter_ZZ { get; } = CreateGeometry("M2,2 h13 v4 l-9,20 h9 v4 h-13 v-4 l9,-20 h-9 Z M17,2 h13 v4 l-9,20 h9 v4 h-13 v-4 l9,-20 h-9 Z");

        public static SG Merge_Arrow { get; } = CreateGeometry("M7,5 h7 a2,2,0,0,1,2,2 v7 a1,1,0,0,0,1,1 h4 v-3 l4,4 -4,4 v-3 h-4 a1,1,0,0,0,-1,1 v7 a2,2,0,0,1,-2,2 h-7 v-2 h6 a1,1,0,0,0,1,-1 v-6 a1,1,0,0,0,-1,-1 h-6 v-2 h6 a1,1,0,0,0,1,-1 v-6 a1,1,0,0,0,-1,-1 h-6 Z");
        public static SG Merge_Outer { get; } = CreateGeometry("M0,2 h6 v8 h-6 Z M0,12 h6 v8 h-6 Z M0,22 h6 v8 h-6 Z M26,12 h6 v8 h-6 Z");
        public static SG Merge_Inner { get; } = CreateGeometry("M1,3 h4 v6 h-4 Z M1,13 h4 v6 h-4 Z M1,23 h4 v6 h-4 Z M27,13 h4 v6 h-4 Z");

        public static SG Split_Arrow { get; } = CreateGeometry("M7,15 h6 a1,1,0,0,0,1,-1 v-7 a2,2,0,0,1,2,-2 h5 v-3 l4,4 -4,4 v-3 h-4 a1,1,0,0,0,-1,1 v6 a1,1,0,0,0,1,1 h4 v-3 l4,4 -4,4 v-3 h-4 a1,1,0,0,0,-1,1 v6 a1,1,0,0,0,1,1 h4 v-3 l4,4 -4,4 v-3 h-5 a2,2,0,0,1,-2,-2 v-7 a1,1,0,0,0,-1,-1 h-6 Z");
        public static SG Split_Outer { get; } = CreateGeometry("M0,12 h6 v8 h-6 Z M26,2 h6 v8 h-6 Z M26,12 h6 v8 h-6 Z M26,22 h6 v8 h-6 Z");
        public static SG Split_Inner { get; } = CreateGeometry("M1,13 h4 v6 h-4 Z M27,3 h4 v6 h-4 Z M27,13 h4 v6 h-4 Z M27,23 h4 v6 h-4 Z");

        public static SG Wave { get; } = CreateGeometry("M0,15 H1.5 L3,12 a1,1,0,0,1,2,0 L7.92,20.72 13,2 a1,1,0,0,1,2,0 L18.32,25.12 23,8 a1,1,0,0,1,2,0 L28.2,17.44 29,16 a1,1,0,0,1,1,-1 H32 v2 L30.6,17 29,20 a1,1,0,0,1,-2,0 L24.08,11.28 19,30 a1,1,0,0,1,-2,0 L13.68,6.88 9,24 a1,1,0,0,1,-2,0 L3.84,14.6 3,16 a1,1,0,0,1,-1,1 H0 Z");

        public static SG Wave_Gain_Zero { get; } = CreateGeometry("M0,2 v2 h32 v-2 Z M0,26 v2 h32 v-2 Z");
        public static SG Wave_Gain_Mid { get; } = CreateGeometry("M0,8 v2 h5 v-2 m3,0 v2 h5 v-2 m3,0 v2 h5 v-2 m3,0 v2 h5 v-2 Z M0,14  v2 h5 v-2 m3,0 v2 h5 v-2 m3,0 v2 h5 v-2 m3,0 v2 h5 v-2 Z M0,20  v2 h5 v-2 m3,0 v2 h5 v-2 m3,0 v2 h5 v-2 m3,0 v2 h5 v-2 Z");
        public static SG Wave_Time { get; } = CreateGeometry("M2,0 h2 v6 h-2 m0,3 h2 v5 h-2 m0,3 h2 v5 h-2 m0,3 h2 v6 h-2 Z M10,0 h2 v6 h-2 m0,3 h2 v5 h-2 m0,3 h2 v5 h-2 m0,3 h2 v6 h-2 Z M18,0 h2 v6 h-2 m0,3 h2 v5 h-2 m0,3 h2 v5 h-2 m0,3 h2 v6 h-2 Z M26,0 h2 v6 h-2 m0,3 h2 v5 h-2 m0,3 h2 v5 h-2 m0,3 h2 v6 h-2 Z");
        public static SG Wave_Marker { get; } = CreateGeometry("M2,0 h2 v32 h-2 Z M12,0 h2 v32 h-2 Z M26,0 h2 v32 h-2 Z");
        public static SG Wave_Marker_Name { get; } = CreateGeometry("M2,0 h2 v32 h-2 Z M6,30 v-12 h8 v2 h-6 v3 h4 v2 h-4 v5 M13,26 a4,4,0,0,0,8,0 a4,4,0,0,0,-8,0 Z m2,0 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 Z M22,26 a4,4,0,0,0,8,0 a4,4,0,0,0,-8,0 Z m2,0 a2,2,0,0,0,4,0 a2,2,0,0,0,-4,0 Z");

        public static SG Wave_Marker_Outer { get; } = CreateGeometry("M10,1 v1 l4,4 v25 h4 v-25 l4,-4 v-1 Z");
        public static SG Wave_Marker_Single_Outer { get; } = CreateGeometry("M0,16 l7,-7 h5 v1 l-6,6 v2 l6,6 v1 h-5 l-7,-7 Z M32,16 l-7,-7 h-5 v1 l6,6 v2 l-6,6 v1 h5 l7,-7 Z M10,1 v1 l4,4 v25 h4 v-25 l4,-4 v-1 Z");
        public static SG Wave_Marker_Single { get; } = CreateGeometry("M11,2 l4,4 v24 h2 v-24 l4,-4 Z");
        public static SG Wave_Marker_Arrow { get; } = CreateGeometry("M1,16 l6,-6 h4 l-6,6 v2 l6,6 h-4 l-6,-6 Z M31,16 l-6,-6 h-4 l6,6 v2 l-6,6 h4 l6,-6 Z");
        public static SG Wave_Marker_Multi_Outer { get; } = CreateGeometry("M0,16 l7,-7 h3 v-3 l-4,-4 v-1 h20 v1 l-4,4 v3 h3 l7,7 v2 l-7,7 h-3 v6 h-4 v-25 l-2,-2 l-2,2 v25 h-4 v-6 h-3 l-7,-7 Z M6,16 l4,-4 v10 l-4,-4 Z M26,16 l-4,-4 v10 l4,-4 Z");
        public static SG Wave_Marker_Multi { get; } = CreateGeometry("M7,2 l4,4 v24 h2 v-24 l3,-3 l3,3 v24 h2 v-24 l4,-4 Z");
        public static SG Wave_Marker_Auto_Outer { get; } = CreateGeometry("M0,1 h32 v4 h-2 v26 h-4 v-26 h-4 v26 h-4 v-26 h-4 v26 h-4 v-26 h-4 v26 h-4 v-26 h-2 Z");
        public static SG Wave_Marker_Auto_Inner { get; } = CreateGeometry("M1,2 h6 v2 h-2 v26 h-2 v-26 h-2 Z M9,2 h6 v2 h-2 v26 h-2 v-26 h-2 Z M17,2 h6 v2 h-2 v26 h-2 v-26 h-2 Z M25,2 h6 v2 h-2 v26 h-2 v-26 h-2 Z");
        public static SG Wave_Marker_Auto_Clear { get; } = CreateGeometry("M1,2 h2 v2 h-2 Z M5,2 h2 v2 h-2 Z M9,2 h2 v2 h-2 Z M13,2 h2 v2 h-2 Z M17,2 h2 v2 h-2 Z M21,2 h2 v2 h-2 Z M25,2 h2 v2 h-2 Z M29,2 h2 v2 h-2 Z M3,4 h2 v2 h-2 Z M11,4 h2 v2 h-2 Z M19,4 h2 v2 h-2 Z M27,4 h2 v2 h-2 Z M3,10 h2 v2 h-2 Z M11,10 h2 v2 h-2 Z M19,10 h2 v2 h-2 Z M27,10 h2 v2 h-2 Z M3,16 h2 v2 h-2 Z M11,16 h2 v2 h-2 Z M19,16 h2 v2 h-2 Z M27,16 h2 v2 h-2 Z M3,22 h2 v2 h-2 Z M11,22 h2 v2 h-2 Z M19,22 h2 v2 h-2 Z M27,22 h2 v2 h-2 Z M3,28 h2 v2 h-2 Z M11,28 h2 v2 h-2 Z M19,28 h2 v2 h-2 Z M27,28 h2 v2 h-2 Z");
        public static SG Wave_Marker_Auto_Rect { get; } = CreateGeometry("M0,6 h32 v24 h-32 Z");

        public static SG Wave_Slice { get; } = CreateGeometry("M8,0 h2 v32 h-2 Z M22,0 h2 v32 h-2 Z");
        public static SG Wave_Slice_Note1 { get; } = CreateGeometry("M0,0 h2 v32 h-2 Z M8,0 h2 v32 h-2 Z M24,0 h2 v32 h-2 Z");
        public static SG Wave_Slice_Note2 { get; } = CreateGeometry("M0,20 v4 h8 v-4 Z M8,16 v4 h16 v-4 Z M24,12 v4 h8 v-4 Z");
        public static SG Wave_Spectrum_Frame { get; } = CreateGeometry("M2,0 v7 h-2 v2 h2 v5 h-2 v2 h2 v5 h-2 v2 h2 v5 h-2 v2 h2 v2 h2 v-2 h5 v2 h2 v-2 h5 v2 h2 v-2 h5 v2 h2 v-2 h7 v-2 h-28 v-28 Z");
        public static SG Wave_Spectrum_Figure { get; } = CreateGeometry("M4,28 v-12 h4 v-4 h4 v8 h4 v-10 h4 v-4 h4 v6 h4 v4 h4 v12 Z");

        public static SG Bms_Background { get; } = CreateGeometry("M9,0 h21 v17 h-21 Z M0.5,17.5 h31 v14 h-31 Z");
        public static SG Bms_Red { get; } = CreateGeometry("M2,0 h7 v17 h-7 Z");
        public static SG Bms_Scratch { get; } = CreateGeometry("M1,24.5 a5,5,0,0,0,10,0 a5,5,0,0,0,-10,0 Z");
        public static SG Bms_Black { get; } = CreateGeometry("M12,0 v17 h3 v-17 Z M18,0 v17 h3 v-17 Z M24,0 v17 h3 v-17 Z M14,18 v6 h4 v-6 Z M19,18 v6 h4 v-6 Z M24,18 v6 h4 v-6 Z");
        public static SG Bms_White { get; } = CreateGeometry("M3,24.5 a3,3,0,0,0,6,0 a3,3,0,0,0,-6,0 Z M12,25 v6 h4 v-6 Z M17,25 v6 h4 v-6 Z M22,25 v6 h4 v-6 Z M27,25 v6 h4 v-6 Z");

        public static SG Bms_Sort_Background { get; } = CreateGeometry("M8,0 h8 v32 h-8 Z M24,0 h8 v32 h-8 Z");
        public static SG Bms_Sort_Foreground { get; } = CreateGeometry("M0,6 h8 v4 h-8 z M0,18 h8 v4 h-8 Z M0,26 h8 v4 h-8 Z M8,2 h8 v4 h-8 Z M8,14 h8 v4 h-8 Z M8,26 h8 v4 h-8 Z M16,8 h8 v4 h-8 Z M16,18 h8 v4 h-8 Z");
        public static SG Bms_Sort_Highlight { get; } = CreateGeometry("M6,6 h2 v4 h-2 z M6,18 h2 v4 h-2 Z M6,26 h2 v4 h-2 Z M14,2 h2 v4 h-2 Z M14,14 h2 v4 h-2 Z M14,26 h2 v4 h-2 Z M22,8 h2 v4 h-2 Z M22,18 h2 v4 h-2 Z");

        public static SG Bms_Sequential { get; } = CreateGeometry("M8,2 h8 v4 h-8 Z M8,10 h8 v4 h-8 Z M8,18 h8 v4 h-8 Z M8,26 h8 v4 h-8 Z");

        public static SG Midi_Black { get; } = CreateGeometry("M0,2 v28 a2,2,0,0,0,2,2 h28 a2,2,0,0,0,2,-2 v-28 a2,2,0,0,0,-2,-2 h-28 a2,2,0,0,0,-2,2 Z");
        public static SG Midi_White1 { get; } = CreateGeometry("M0,2 v2 h32 v-2 Z M0,6 v2 h32 v-2 Z M0,12 v2 h32 v-2 Z M0,16 v2 h32 v-2 Z M0,20 v2 h32 v-2 Z M0,26 v2 h32 v-2 Z M0,30 a2,2,0,0,0,2,2 h28 a2,2,0,0,0,2,-2 Z");
        public static SG Midi_White2 { get; } = CreateGeometry("M0,8 v2 h32 v-2 Z M0,24 v2 h32 v-2 Z");
        public static SG Midi_Note1 { get; } = CreateGeometry("M4,8 v2 h4 v-2 Z M8,2 v2 h4 v-2 Z M12,8 v2 h4 v-2 Z M20,4 v2 h4 v-2 Z M24,2 v2 h4 v-2 Z");
        public static SG Midi_Note2 { get; } = CreateGeometry("M0,18 v2 h8 v-2 Z M8,16 v2 h8 v-2 Z M16,12 v2 h8 v-2 Z M24,10 v2 h8 v-2 Z");
        public static SG Midi_Note3 { get; } = CreateGeometry("M0,26 v2 h8 v-2 Z M8,24 v2 h4 v-2 Z M12,20 v2 h4 v-2 Z M16,18 v2 h8 v-2 Z M24,24 v2 h4 v-2 Z M28,28 v2 h4 v-2 Z");

        public static SG Midi_BarLine { get; } = CreateGeometry("M2,0 h2 v32 h-2 Z M10,0 h2 v32 h-2 Z M18,0 h2 v32 h-2 Z M26,0 h2 v32 h-2 Z");
        public static SG Midi_Velocity { get; } = CreateGeometry("M0,8 h8 v8 h4 v-8 h4 v16 h12 v-12 h4 V32 H0 Z");

        public static SG Midi_Enchord_Notes { get; } = CreateGeometry("M2,12 h8 v4 h-8 Z M2,24 h8 v4 h-8 Z M10,4 h8 v4 h-8 Z M10,20 h8 v4 h-8 Z M18,8 h12 v4 h-12 Z M18,24 h12 v4 h-12 Z");
        public static SG Midi_Enchord_Middle { get; } = CreateGeometry("M10,4 h8 v4 h-8 Z M10,20 h8 v4 h-8 Z");
        public static SG Midi_Enchord_Right { get; } = CreateGeometry("M18,8 h12 v4 h-12 Z M18,24 h12 v4 h-12 Z");
        public static SG Midi_Enchord_Marker { get; } = CreateGeometry("M2,2 h2 v28 h-2 Z M10,2 h2 v28 h-2 Z M18,2 h2 v28 h-2 Z");
        public static SG Midi_Enchord_Marker2 { get; } = CreateGeometry("M2,2 h2 v28 h-2 Z");
        public static SG Midi_Enchord_Rect { get; } = CreateGeometry("M0,2 h32 v28 h-32 Z");

        public static SG Midi_Group_Notes { get; } = CreateGeometry("M3,14 h8 v4 h-8 Z M3,26 h8 v4 h-8 Z M11,10 h8 v4 h-8 Z M11,22 h8 v4 h-8 Z M19,18 h12 v4 h-12 Z M19,26 h12 v4 h-12 Z");
        public static SG Midi_Group_Marker_Outer { get; } = CreateGeometry("M0,0 h24 v4 l-2,1 v27 h-4 v-27 l-2,-1 -2,1 v27 h-4 v-27 l-2,-1 -2,1 v27 h-4 v-27 l-2,-1 Z");
        public static SG Midi_Group_Marker { get; } = CreateGeometry("M1,1 h6 v2 l-2,1 v27 h-2 v-27 l-2,-1 Z M9,1 h6 v2 l-2,1 v27 h-2 v-27 l-2,-1 Z M17,1 h6 v2 l-2,1 v27 h-2 v-27 l-2,-1 Z");

        public static SG Midi_AutoGroup_Left { get; } = CreateGeometry("M0,8 h6 v-4 h6 v4 h4 v4 h-4 v-4 h-6 v4 h-6 Z M0,24 h6 v-4 h6 v4 h4 v4 h-4 v-4 h-6 v4 h-6 Z");
        public static SG Midi_AutoGroup_Right { get; } = CreateGeometry("M16,8 h6 v-4 h6 v4 h4 v4 h-4 v-4 h-6 v4 h-6 Z M16,24 h6 v-4 h6 v4 h4 v4 h-4 v-4 h-6 v4 h-6 Z");

        public static SG Midi_Sort_Notes { get; } = CreateGeometry("M0,24 h4 v-4 h6 v-4 h6 v-4 h6 v-4 h6 v-4 h4 v4 h-4 v4 h-6 v4 h-6 v4 h-6 v4 h-6 v4 h-4 Z");

        public static SG Piano_White { get; } = CreateGeometry("M0,2 v28 a2,2,0,0,0,2,2 h28 a2,2,0,0,0,2,-2 v-28 a2,2,0,0,0,-2,-2 h-28 a2,2,0,0,0,-2,2 Z");
        public static SG Piano_Black { get; } = CreateGeometry("M4,0 v18 h24 v-18 Z");
        public static SG Piano_Outline { get; } = CreateGeometry("M2,0 h28 a2,2,0,0,1,2,2 v28 a2,2,0,0,1,-2,2 h-28 a2,2,0,0,1,-1.732,-1 H7 v-13 h1 v13 h7 v-13 h1 v13 h7 v-13 h1 v13 h7 v-30 h-3 v17 h-1 v-18 h-7 v18 h-1 v-18 h-7 v18 h-1 v-18 h-7 v1 H0.268 A2,2,0,0,1,2,0 Z");
        public static SG Piano_KeySwitch { get; } = CreateGeometry("M0,2 v29 h7 v-13 h1 v13 h7 v-13 h4 v-18 h-7 v18 h-1 v-18 h-9 a2,2,0,0,0,-2,2 Z");

        public static SG Metronome_Outer { get; } = CreateGeometry("M12,0 h8 L28,28 v4 h-24 v-4 Z");
        public static SG Metronome_Inner { get; } = CreateGeometry("M12.5,1 h7 L25.8,23 h-19.6 Z M6,24 h20 l1,4 v3 h-22 v-3 Z");
        public static SG Metronome_Scale { get; } = CreateGeometry("M15,1 h2 v3 h3 v1 h-3 v3 h3 v1 h-3 v3 h3 v1 h-3 v3 h3 v1 h-3 v3 h3 v1 h-3 v2 h-2 v-2 h-3 v-1 h3 v-3 h-3 v-1 h3 v-3 h-3 v-1 h3 v-3 h-3 v-1 h3 v-3 h-3 v-1 h3 Z");
        public static SG Metronome_Bar { get; } = CreateGeometry("M15,23 L24,4 L26,5 L17,23");

        public static SG Browse_BackTab { get; } = CreateGeometry("M9,5 v4 h17 v-4 Z");
        public static SG Browse_FrontTab { get; } = CreateGeometry("M1,3 v26 h30 v-20 h-22 v-6 Z");
        public static SG Browse_Content { get; } = CreateGeometry("M3,11 v16 h26 v-16 Z");
        public static SG Browse_Outline { get; } = CreateGeometry("M0,2 v28 h32 v-22 h-22 v-6 Z M1.5,3.5 v25 h29 v-19 h-22 v-6 Z M10,4 h17 v4 h-1.5 v-2.5 h-7 v2.5 h-1.5 v-2.5 h-7 Z M5,12 v2 h18 v-2 M5,16 v2 h14 v-2 M5,20 v2 h22 v-2 M5,24 v2 h10 v-2");

    }
}
