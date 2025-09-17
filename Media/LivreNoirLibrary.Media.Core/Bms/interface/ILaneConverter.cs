using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ILaneConverter
    {
        public int Convert(int lane);
        public int ConvertBack(int index);
    }
}
