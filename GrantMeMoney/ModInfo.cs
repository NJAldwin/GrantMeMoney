#region

using System;
using System.Reflection;
using ColossalFramework;
using ICities;

#endregion

[assembly: AssemblyVersion("1.0.0.0")]

namespace GrantMeMoney
{
    public class ModInfo : IUserMod
    {
        public static readonly string FullName = $"GrantMeMoney {Version}";

        public ModInfo()
        {
            try
            {
                if (GameSettings.FindSettingsFileByName(GrantMeMoney.ModName) == null)
                    GameSettings.AddSettingsFile(new SettingsFile {fileName = GrantMeMoney.ModName});
            }
            catch (Exception e)
            {
                Dbg.Err("Could not create settings file", e);
            }
        }

        private static string Version => typeof(ModInfo).Assembly.GetName().Version.ToString();

        public string Name => FullName;

        public string Description => "Allows grants to be awarded (currently unconditionally!)";
    }
}
