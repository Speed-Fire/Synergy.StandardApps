using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Synergy.StandardApps.Settings
{
    public static class Properties
    {
        private static readonly string _path = Path.Combine
            (Directory.GetCurrentDirectory(), _FILE_NAME);

        private static Hashtable _properties = new Hashtable();

        #region Constants

        private const string _FILE_NAME = "Properties";

        private const string NOTE_MAX_AGE = "NOTE_MAX_AGE"; // 60
        private const string CURRENT_LANGUAGE = "CURRENT_LANGUAGE"; // 

        #endregion

        #region Properties

        public static int? NoteMaxAge
        {
            get => (int?)_properties[NOTE_MAX_AGE];
            set => _properties[NOTE_MAX_AGE] = value;
        }

        public static string? CurrentLanguage
        {
            get => (string?)_properties[CURRENT_LANGUAGE];
            set => _properties[CURRENT_LANGUAGE] = value;
        }

        #endregion

        static Properties()
        {
            Load();
        }

        private static void Load()
        {
            if(!File.Exists(_path))
            {
                SetDefaults();

                return;
            }

            using var fs = new FileStream(_path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs);

            _properties = JsonSerializer.Deserialize<Hashtable>(fs) ?? new Hashtable();
        }

        public static void Save()
        {
            using var fs = new FileStream(_path, FileMode.Truncate, FileAccess.Write);
            using var sw = new StreamWriter(fs);

            sw.Write(JsonSerializer.Serialize(_properties));
        }

        private static void SetDefaults()
        {
            NoteMaxAge = 60;
            CurrentLanguage = "English";

            Save();
        }
    }
}
