namespace EdiModuleCore
{
    using System.IO;
    using Newtonsoft.Json;
    using Exceptions;

    public static class SettingsManager
    {
        public static void LoadSettings()
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

            if (!SettingsManager.IsCorrectSettings())
                ResetToDefault();
        }

        public static void SaveSettings(Settings settings)
        {
            try
            {
                FileService.CreateDirectory(settings.DestinationWaybillFolder);
                FileService.CreateDirectory(settings.StartWaybillFolder);
                string result = JsonConvert.SerializeObject(settings);
                FileService.WriteTextFile(SettingsManager.SettingsFileName, result);
                SettingsManager.LoadSettings();
            }
            catch (JsonException ex)
            {
                throw ex;
            }
        }

        private static void ResetToDefault()
        {
            Settings newSettings = new Settings
            {
                DestinationWaybillFolder = Settings.DefaultValues[1],
                StartWaybillFolder = Settings.DefaultValues[0]
            };
            SettingsManager.SaveSettings(newSettings);
            FileService.CreateDirectory(newSettings.StartWaybillFolder);
            FileService.CreateDirectory(newSettings.DestinationWaybillFolder);
        }

        private static bool IsCorrectSettings()
        {
            foreach (var item in SettingsManager.Settings.GetType().GetProperties())
            {
                if(item.GetValue(SettingsManager.Settings) == null)
                {
                    return false;
                }
            }

            return true;
        }

        private static Settings settings;
        public static Settings Settings
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
