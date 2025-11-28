using DatabaseProject.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DatabaseProject.Services
{
    public class JsonFileService<T>
    {
        private static string SerializeData(List<T> data)
        {
            var serializedData = JsonSerializer.Serialize<List<T>>(data, new JsonSerializerOptions { WriteIndented = true });
            return serializedData;
        }

        private static List<T>? DeserializeData(string jsonContent)
        {
            var deserializedData = JsonSerializer.Deserialize<List<T>>(jsonContent);
            return deserializedData;
        }

        public List<T>? ReadDataFromFile(string jsonPath)
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"No file exists with given path: {jsonPath}");
                return null;
            }

            string jsonContent = File.ReadAllText(jsonPath);
            List<T> data = DeserializeData(jsonContent);

            return data;
        }

        public void WriteDataToFile(string outputJsonPath, List<T> data)
        {
            var serializedData = SerializeData(data);
            File.WriteAllText(outputJsonPath, serializedData);
        }
    }
}
