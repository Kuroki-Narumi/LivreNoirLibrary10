using System;

namespace LivreNoirLibrary.Windows
{
    public interface IUpdateCheck
    {
        bool CheckUpdate { get; set; }
        string VersionUrl { get; }
        string SettingName { get; }

        string GetMessage_NewVersion() => "新しいバージョン({0})が公開されています。\n更新しますか？";
        string GetMessage_NoUpdate() => "更新はありません。";
    }
}
