using System;
using System.Threading;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Windows
{
    public interface IUpdateCheck
    {
        bool CheckUpdate { get; set; }
        string VersionUrl { get; }
        string UpdaterLocation { get; }

        bool NotifyNewVersion(Version version);
        void NotifyNoUpdate();
    }
}
