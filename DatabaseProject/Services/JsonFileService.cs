using DatabaseProject.Models;
using System.Text.Json;

namespace DatabaseProject.Services
{
    public class JsonFileService<T>
    {
        public List<T>? ReadData(string jsonPath)
        {
            if (!File.Exists(jsonPath))
            {
                return null;
            }

            string jsonContent = File.ReadAllText(jsonPath);
            var serializedData = JsonSerializer.Deserialize<List<T>>(jsonContent);

            return serializedData;
        }

        public void WriteData(string outputJsonPath, List<T> data)
        {
            string jsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(outputJsonPath, jsonContent);
        }
    }
}
