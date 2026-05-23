namespace LivreNoirLibrary.Updater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Updater.AlertAndWait(Updater.Message_NeedArgs);
                return;
            }
            Updater.Run(args[0], args[1]);
        }
    }
}
