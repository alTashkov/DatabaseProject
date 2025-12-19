namespace DatabaseProject.Interfaces
{
    public interface IJsonProcessor<T>
    {
        public List<T>? ReadDataFromFile(string jsonPath);

        /// <summary>
        /// Writes and serializes a data list to a JSON file.
        /// </summary>
        public bool WriteDataToFile(string outputJsonPath, List<T> data);
    }
}
