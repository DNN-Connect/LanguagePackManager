namespace Connect.LanguagePackManager.Core.Common
{
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Settings;

    public class ModuleSettings
    {
        [ModuleSetting]
        public string OwnerName { get; set; } = "";

        [ModuleSetting]
        public string OwnerOrganization { get; set; } = "";

        [ModuleSetting]
        public string OwnerUrl { get; set; } = "";

        [ModuleSetting]
        public string OwnerEmail { get; set; } = "";

        [ModuleSetting]
        public string License { get; set; } = "";

        public static ModuleSettings GetSettings(ModuleInfo module)
        {
            var repo = new ModuleSettingsRepository();
            return repo.GetSettings(module);
        }

        public void SaveSettings(ModuleInfo module)
        {
            var repo = new ModuleSettingsRepository();
            repo.SaveSettings(module, this);
        }
    }
    public class ModuleSettingsRepository : SettingsRepository<ModuleSettings>
    {
    }
}