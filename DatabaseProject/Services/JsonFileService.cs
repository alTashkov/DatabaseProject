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

        /// <summary>
        /// Serializes the given data.
        /// </summary>
        /// <param name="data">The data list to be serialized.</param>
        /// <returns>
        /// A JSON string containing the serialized data.
        /// </returns>
        private static string SerializeData(List<T> data)
        {
            var serializedData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            return serializedData;
        }

        /// <summary>
        /// Deserializes the given JSON string.
        /// </summary>
        /// <param name="jsonContent">The JSON string to be deserialized.</param>
        /// <returns>
        /// A list string representing the deserialized data.
        /// </returns>
        private static List<T>? DeserializeData(string jsonContent)
        {
            var deserializedData = JsonSerializer.Deserialize<List<T>>(jsonContent);
            return deserializedData;
        }

        /// <summary>
        /// Reads data from a JSON file.
        /// </summary>
        /// <param name="jsonPath">The path to the file that data will be read from.</param>
        /// <returns>A list contaiing the deserialized data of the given file.</returns>
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

        /// <summary>
        /// Writes and serializes a data list to a JSON file.
        /// </summary>
        /// <param name="outputJsonPath">The path to the JSON file.</param>
        /// <param name="data">The data to be written.</param>
        /// <returns>
        /// <b>True</b> if the serialized data was successfully written to the JSON file;<br></br>
        /// <b>False</b> if the data was not serialized properly and wasn't written to the JSON file.
        /// </returns>
        public bool WriteDataToFile(string outputJsonPath, List<T> data)
        {
            var serializedData = SerializeData(data);
            if (serializedData != null)
            {
                File.WriteAllText(outputJsonPath, serializedData);
                return true;
            }
            else
            {
                _logger.LogError("Data was not serialized successfully.");
                return false;
            }
        }
    }
}
