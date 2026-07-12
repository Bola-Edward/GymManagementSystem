using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GymManagementSystem.DAL.Data.Seeder
{
    public class SeedJsonLoader
    {
        public static List<TEntity>? LoadSeedData<TEntity>(string fileName) where TEntity : class
        {

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(basePath, "SeedData", fileName);


            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "GymManagementSystem.DAL", "Data", "Seeder", fileName);
            }


            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"🚀 Seed file not found at: {filePath}");
            }


            var jsonData = File.ReadAllText(filePath);


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            return JsonSerializer.Deserialize<List<TEntity>>(jsonData, options);
        }

    }
}
