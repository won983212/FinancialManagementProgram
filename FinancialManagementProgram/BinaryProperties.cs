using FinancialManagementProgram.Data;
using System;
using System.IO;
using System.Reflection;

namespace FinancialManagementProgram
{
    public static class BinaryProperties
    {
        public const int ConfigVersion = 3; // ConfigVersion이 다르면 Reset하니 주의.
        private static string _filePath;
        private static IPropertiesSerializable[] properties;
        private static readonly object _lock = new object();
        private static bool _loadedOnce = false;

        static BinaryProperties()
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "fmp_cfg.dat");
            properties = new IPropertiesSerializable[]
            {
                new CategorySerializer(), DataManager.Current
            };
        }

        public static void Load()
        {
            if (File.Exists(_filePath))
            {
                bool resetConfig = false;
                lock (_lock)
                {
                    try
                    {
                        using (BinaryReader reader = new BinaryReader(File.OpenRead(_filePath)))
                        {
                            int cfg_ver = reader.ReadInt32();
                            if (cfg_ver != ConfigVersion)
                                resetConfig = true;
                            else
                                foreach (IPropertiesSerializable obj in properties)
                                    obj.Deserialize(reader);
                        }
                    }
                    catch
                    {
                        resetConfig = true;
                    }
                }
                if (resetConfig)
                    Save();
            }
            else
            {
                Save();
            }
            _loadedOnce = true;
        }

        public static void Save()
        {
            lock (_lock)
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(_filePath)))
                {
                    writer.Write(ConfigVersion);
                    foreach (IPropertiesSerializable obj in properties)
                        obj.Serialize(writer);
                }
            }
        }

        public static string Version
        {
            get => Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        public static bool LoadedOnce
        {
            get => _loadedOnce;
        }
    }
}
