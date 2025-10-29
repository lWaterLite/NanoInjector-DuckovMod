using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using SodaCraft.Localizations;
using UnityEngine;

namespace NanoInjector.ModdingUtils
{
    public class LocalizationText
    {
        public string Description = string.Empty;
        public string DisplayName = string.Empty;
    }
    
    public static class LocalizationUtils
    {
        private static readonly string LanguageDirectory;
        private static Dictionary<string, LocalizationText>? _texts = new Dictionary<string, LocalizationText>();

        private static readonly Dictionary<SystemLanguage, string> LanguageFileNames =
            new Dictionary<SystemLanguage, string>
            {
                { SystemLanguage.ChineseSimplified, "zh-cn.json" },
                { SystemLanguage.ChineseTraditional, "zh-tw.json" },
                { SystemLanguage.English, "en-us.txt" }
            };

        static LocalizationUtils()
        {
            LanguageDirectory =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    "Languages");
        }

        private static void GetLocalizationFile(SystemLanguage language)
        {
            string path =
                LanguageFileNames.GetValueOrDefault(language,
                    "en-us.json"); // If language doesn't support, fall back to English as default.
            path = Path.Combine(LanguageDirectory, path);
            if (!File.Exists(path)) return;
            using StreamReader file = File.OpenText(path);
            JsonSerializer serializer = new JsonSerializer();
            _texts = serializer.Deserialize(file,
                typeof(Dictionary<string, LocalizationText>)) as Dictionary<string, LocalizationText>;
        }

        public static void SetLocalization()
        {
            SystemLanguage currentLanguage = LocalizationManager.CurrentLanguage;
            GetLocalizationFile(currentLanguage);
            if (_texts == null)
            {
                Debug.LogWarning("LMC: Localization text is missing, please check the language files");
                return;
            }

            foreach ((string? key, LocalizationText localizationText) in _texts)
            {
                LocalizationManager.overrideTexts.Add(key, localizationText.DisplayName);
                LocalizationManager.overrideTexts.Add(key + "_Desc",
                    localizationText.Description + "\n\n<color=grey>[Mod: NanoInjector]</color>");
            }
        }
    }
}