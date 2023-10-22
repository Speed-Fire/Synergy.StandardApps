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
        private static readonly string _path = GetPropertiesDirectory(_FILE_NAME);


		private static Dictionary<string, string> _properties = new();

        #region Constants

        private const string _FILE_NAME = "Properties.json";

        private const string NOTE_MAX_AGE = "NOTE_MAX_AGE"; // 60
        private const string CURRENT_LANGUAGE = "CURRENT_LANGUAGE"; // 

        #endregion

        #region Properties

        public static int NoteMaxAge
        {
            get => int.Parse(_properties[NOTE_MAX_AGE]);
            set => _properties[NOTE_MAX_AGE] = value.ToString();
        }

        public static string CurrentLanguage
        {
            get => _properties[CURRENT_LANGUAGE];
            set => _properties[CURRENT_LANGUAGE] = value;
        }

        #endregion

        static Properties()
        {
            Load();
        }

		#region Serialization

		private static void Load()
        {
            if(!File.Exists(_path))
            {
                SetDefaults();

                return;
            }

            using var fs = new FileStream(_path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs);

            _properties = JsonSerializer.Deserialize<Dictionary<string, string>>(fs) ?? new();
        }

        public static void Save()
        {
            using var fs = new FileStream(_path, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(fs);

            sw.Write(JsonSerializer.Serialize(_properties));
        }

        private static void SetDefaults()
        {
            NoteMaxAge = 60;
            CurrentLanguage = "English";

            Save();
        }

		#endregion

		#region Properties folder

        public static string GetPropertiesDirectory()
        {
			var dir = Path
                .Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synergy");
			dir = Path.Combine(dir, "StandardApps");
			Directory.CreateDirectory(dir);

            return dir;
		}

		public static string GetPropertiesDirectory(string file)
        {
            return Path.Combine(GetPropertiesDirectory(), file);
        }

		#endregion
	}
}
