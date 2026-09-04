using System;
using System.IO;
using System.Text.Json;

namespace POS_System
{
    public class ServerConfig
    {
        public string ServerName { get; set; } = @".\SQLEXPRESS";
        public string DatabaseName { get; set; } = "POS_InventoryDB";
        public bool UseWindowsAuth { get; set; } = true;
        public string SqlUser { get; set; } = "sa";
        public string SqlPassword { get; set; } = "";

        // حفظ الإعدادات في مجلد عام مفتوح الصلاحيات
        private static readonly string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PharmaTech");
        private static readonly string ConfigPath = Path.Combine(AppDataFolder, "db_config.json");

        public static string GetConnectionString()
        {
            ServerConfig config = Load();
            if (config.UseWindowsAuth)
            {
                return $@"Data Source={config.ServerName};Initial Catalog={config.DatabaseName};Integrated Security=True;TrustServerCertificate=True;";
            }
            else
            {
                return $@"Data Source={config.ServerName};Initial Catalog={config.DatabaseName};User ID={config.SqlUser};Password={config.SqlPassword};TrustServerCertificate=True;";
            }
        }

        public static ServerConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var cfg = JsonSerializer.Deserialize<ServerConfig>(json);
                    if (cfg != null) return cfg;
                }
            }
            catch { }

            return new ServerConfig();
        }

        public static void Save(ServerConfig config)
        {
            try
            {
                if (!Directory.Exists(AppDataFolder))
                {
                    Directory.CreateDirectory(AppDataFolder);
                }

                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("فشل في حفظ إعدادات السيرفر: " + ex.Message);
            }
        }
    }
}