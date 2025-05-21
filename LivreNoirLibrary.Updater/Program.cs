using static LivreNoirLibrary.Updater.Updater;

namespace LivreNoirLibrary.Updater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                AlertAndWait(Message_NeedArgs);
                return;
            }
            Run(args[0], args[1]);
        }
    }
}
