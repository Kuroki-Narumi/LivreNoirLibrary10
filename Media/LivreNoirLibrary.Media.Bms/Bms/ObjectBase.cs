using System.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class ObjectBase
    {
        public string? Note { get; set; }

        protected void WriteNote(BinaryWriter writer)
        {
            if (string.IsNullOrEmpty(Note))
            {
                writer.Write7BitEncodedInt(0);
            }
            else
            {
                writer.Write(Note);
            }
        }

        protected static string? ReadNote(BinaryReader reader) => reader.ReadString().GetNullIfEmpty();
    }
}
