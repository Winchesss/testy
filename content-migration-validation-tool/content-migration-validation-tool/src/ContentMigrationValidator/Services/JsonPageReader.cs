using System.Text.Json;
using ContentMigrationValidator.Models;

namespace ContentMigrationValidator.Services;

public sealed class JsonPageReader
{
    public ContentPageReadResult Read(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var document = JsonDocument.Parse(stream);

        var root = document.RootElement;
        var pageElements = GetPageElements(root, out var inputDetails);
        var pages = new List<ContentPage>();
        var index = 1;

        foreach (var element in pageElements)
        {
            pages.Add(new ContentPage
            {
                SourceRowNumber = index,
                PageId = GetStringValue(element, "pageId"),
                Title = GetStringValue(element, "title"),
                Url = GetStringValue(element, "url"),
                ImagePath = GetStringValue(element, "imagePath"),
                AttachmentPath = GetStringValue(element, "attachmentPath"),
                Status = GetStringValue(element, "status")
            });
            index++;
        }

        return new ContentPageReadResult
        {
            Pages = pages,
            InputDetails = inputDetails
        };
    }

    private static IEnumerable<JsonElement> GetPageElements(JsonElement root, out string inputDetails)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            inputDetails = "JSON format detected: root array";
            return root.EnumerateArray();
        }

        if (root.ValueKind == JsonValueKind.Object && TryGetPropertyCaseInsensitive(root, "pages", out var pages)
            && pages.ValueKind == JsonValueKind.Array)
        {
            inputDetails = "JSON format detected: object with pages array";
            return pages.EnumerateArray();
        }

        throw new FormatException("JSON input must be an array or an object with a 'pages' array.");
    }

    private static string? GetStringValue(JsonElement element, string propertyName)
    {
        if (!TryGetPropertyCaseInsensitive(element, propertyName, out var property))
        {
            return null;
        }

        return property.ValueKind switch
        {
            JsonValueKind.String => property.GetString()?.Trim(),
            JsonValueKind.Number => property.GetRawText().Trim(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            JsonValueKind.Null => null,
            _ => property.GetRawText().Trim()
        };
    }

    private static bool TryGetPropertyCaseInsensitive(
        JsonElement element,
        string propertyName,
        out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }
}