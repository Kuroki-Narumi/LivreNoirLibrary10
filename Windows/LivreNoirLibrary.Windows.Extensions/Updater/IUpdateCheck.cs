using System;

namespace LivreNoirLibrary.Windows
{
    public interface IUpdateCheck
    {
        public bool CheckUpdate { get; set; }
        public string VersionUrl { get; }
        public string SettingName { get; }

        public string GetMessage_NewVersion() => "新しいバージョン({0})が公開されています。\n更新しますか？";
        public string GetMessage_NoUpdate() => "更新はありません。";
    }
}
