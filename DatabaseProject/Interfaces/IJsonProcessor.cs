namespace DatabaseProject.Interfaces
{
    public interface IJsonProcessor<T>
    {
        public List<T>? ReadDataFromFile(string jsonPath);

        public bool WriteDataToFile(string outputJsonPath, List<T> data);
    }
}
