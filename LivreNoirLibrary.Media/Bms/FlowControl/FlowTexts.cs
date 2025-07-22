using System;

namespace LivreNoirLibrary.Media.Bms
{
    public static class FlowTexts
    {
        public const string Random    = "#RANDOM";
        public const string SetRandom = "#SETRANDOM";
        public const string EndRandom = "#ENDRANDOM";

        public const string If     = "#IF";
        public const string ElseIf = "#ELSEIF";
        public const string Else   = "#ELSE";
        public const string EndIf  = "#ENDIF";

        public const string Switch    = "#SWITCH";
        public const string SetSwitch = "#SETSWITCH";
        public const string EndSwitch = "#ENDSW";

        public const string Case    = "#CASE";
        public const string Default = "#DEF";
        public const string Skip    = "#SKIP";

        public const string Chid_Random  = "LNBRnd";
        public const string Chid_Switch  = "LNBSwt";
        public const string Chid_If      = "LNBIf ";
        public const string Chid_IfChild = "LNBIfc";
        public const string Chid_IfNone  = "LNBIfn";
        public const string Chid_Case    = "LNBCas";

        public const int DefaultIndex = -1;
    }
}
