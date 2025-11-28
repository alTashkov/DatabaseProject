using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DatabaseProject.Services
{
    public class JsonFileService<T>
    {
        private readonly ILogger<JsonFileService<T>> _logger;

        public JsonFileService(ILogger<JsonFileService<T>> logger)
        {
            _logger = logger;
        }

        private static string SerializeData(List<T> data)
        {
            var serializedData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
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
                _logger.LogError("File with path {FilePath} does not exist.",
                jsonPath);

                return null;
            }

            string jsonContent = File.ReadAllText(jsonPath);
            List<T> data = DeserializeData(jsonContent);

            return data;
        }

        public bool WriteDataToFile(string outputJsonPath, List<T> data)
        {
            var serializedData = SerializeData(data);
            File.WriteAllText(outputJsonPath, serializedData);

            return true;
        }
    }
}
