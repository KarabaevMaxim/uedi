namespace UniversalEdiModule.Core
{
    using Newtonsoft.Json;
    using Exceptions;

    internal static class SettingsManager
    {
        internal static void LoadSettings()
        {
            string json = string.Empty;
            json = FileService.ReadTextFile(SettingsManager.SettingsFileName);
            try
            {
                SettingsManager.Settings = JsonConvert.DeserializeObject<Settings>(json);
            }
            catch(JsonException ex)
            {
                throw ex;
            }
        }

        internal static void SaveSettings(Settings settings)
        {
            try
            {
                string result = JsonConvert.SerializeObject(settings);
                FileService.WriteTextFile(SettingsManager.SettingsFileName, result);
            }
            catch (JsonException ex)
            {
                throw ex;
            }
        }

        private static Settings settings;
        internal static Settings Settings
        {
            get
            {
                if(settings == null)
                {
                    throw new NotInitializedException("Инициализация настроек не проведена.");
                }

                return settings;
            }
            private set
            {
                settings = value;
            }
        }

        private static string SettingsFileName { get; set; } = "Settings.json";
    }
}
