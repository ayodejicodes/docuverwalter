namespace docuverwalter_api.Utility
{
    public class FileUtility
    {
        public static string ExtractBlobNameFromPath(string filePath)
        {
            return Path.GetFileName(filePath);
        }
    }
}
