using ContentMigrationValidator.Models;

namespace ContentMigrationValidator.Services;

public sealed class ContentPageFileReader
{
    private readonly CsvPageReader _csvPageReader = new();
    private readonly JsonPageReader _jsonPageReader = new();

    public ContentPageReadResult Read(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        return extension switch
        {
            ".csv" => _csvPageReader.Read(filePath),
            ".json" => _jsonPageReader.Read(filePath),
            _ => throw new NotSupportedException($"Unsupported file type '{extension}'. Use .csv or .json.")
        };
    }
}