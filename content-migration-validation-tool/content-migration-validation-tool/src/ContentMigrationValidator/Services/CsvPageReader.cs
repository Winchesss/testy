using System.Text;
using ContentMigrationValidator.Models;

namespace ContentMigrationValidator.Services;

public sealed class CsvPageReader
{
    private static readonly string[] ExpectedHeaders =
    {
        "pageId",
        "title",
        "url",
        "imagePath",
        "attachmentPath",
        "status"
    };

    public ContentPageReadResult Read(string filePath)
    {
        var content = File.ReadAllText(filePath, Encoding.UTF8);
        var delimiter = DetectDelimiter(content);

        var rows = ParseCsv(content, delimiter)
            .Where(row => row.Any(value => !string.IsNullOrWhiteSpace(value)))
            .ToList();

        if (rows.Count == 0)
        {
            return new ContentPageReadResult
            {
                Pages = Array.Empty<ContentPage>(),
                InputDetails = $"CSV delimiter detected: {FormatDelimiterForDisplay(delimiter)}"
            };
        }

        var headers = rows[0]
            .Select((header, index) => new { Header = NormalizeHeader(header), Index = index })
            .Where(item => !string.IsNullOrWhiteSpace(item.Header))
            .GroupBy(item => item.Header, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First().Index, StringComparer.OrdinalIgnoreCase);

        var pages = new List<ContentPage>();

        for (var i = 1; i < rows.Count; i++)
        {
            var row = rows[i];
            pages.Add(new ContentPage
            {
                SourceRowNumber = i + 1,
                PageId = GetValue(row, headers, "pageId"),
                Title = GetValue(row, headers, "title"),
                Url = GetValue(row, headers, "url"),
                ImagePath = GetValue(row, headers, "imagePath"),
                AttachmentPath = GetValue(row, headers, "attachmentPath"),
                Status = GetValue(row, headers, "status")
            });
        }

        return new ContentPageReadResult
        {
            Pages = pages,
            InputDetails = $"CSV delimiter detected: {FormatDelimiterForDisplay(delimiter)}"
        };
    }

    private static char DetectDelimiter(string content)
    {
        var delimiterResults = new[]
        {
            AnalyzeDelimiter(content, ','),
            AnalyzeDelimiter(content, ';')
        };

        return delimiterResults
            .OrderByDescending(result => result.ExpectedHeaderMatches)
            .ThenByDescending(result => result.ColumnCount)
            .ThenBy(result => result.Delimiter == ',' ? 0 : 1)
            .First()
            .Delimiter;
    }

    private static DelimiterAnalysis AnalyzeDelimiter(string content, char delimiter)
    {
        var rows = ParseCsv(content, delimiter)
            .Where(row => row.Any(value => !string.IsNullOrWhiteSpace(value)))
            .ToList();

        if (rows.Count == 0)
        {
            return new DelimiterAnalysis(delimiter, 0, 0);
        }

        var headerValues = rows[0]
            .Select(NormalizeHeader)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var expectedHeaderMatches = ExpectedHeaders.Count(headerValues.Contains);
        var columnCount = rows[0].Count;

        return new DelimiterAnalysis(delimiter, expectedHeaderMatches, columnCount);
    }

    private static string? GetValue(
        IReadOnlyList<string> row,
        IReadOnlyDictionary<string, int> headers,
        string columnName)
    {
        if (!headers.TryGetValue(columnName, out var index) || index >= row.Count)
        {
            return null;
        }

        return row[index].Trim();
    }

    private static string NormalizeHeader(string header)
    {
        return header.Trim().TrimStart('\uFEFF');
    }

    private static IReadOnlyList<IReadOnlyList<string>> ParseCsv(string content, char delimiter)
    {
        var rows = new List<IReadOnlyList<string>>();
        var row = new List<string>();
        var field = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < content.Length; i++)
        {
            var current = content[i];

            if (inQuotes)
            {
                if (current == '"')
                {
                    if (i + 1 < content.Length && content[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    field.Append(current);
                }

                continue;
            }

            if (current == '"')
            {
                inQuotes = true;
            }
            else if (current == delimiter)
            {
                row.Add(field.ToString());
                field.Clear();
            }
            else if (current == '\r')
            {
                if (i + 1 < content.Length && content[i + 1] == '\n')
                {
                    i++;
                }

                AddCurrentRow(rows, row, field);
            }
            else if (current == '\n')
            {
                AddCurrentRow(rows, row, field);
            }
            else
            {
                field.Append(current);
            }
        }

        if (inQuotes)
        {
            throw new FormatException("CSV file contains an unclosed quoted field.");
        }

        if (field.Length > 0 || row.Count > 0)
        {
            AddCurrentRow(rows, row, field);
        }

        return rows;
    }

    private static void AddCurrentRow(
        ICollection<IReadOnlyList<string>> rows,
        List<string> row,
        StringBuilder field)
    {
        row.Add(field.ToString());
        rows.Add(row.ToArray());
        row.Clear();
        field.Clear();
    }

    private static string FormatDelimiterForDisplay(char delimiter)
    {
        return delimiter switch
        {
            ',' => "comma (,)",
            ';' => "semicolon (;)",
            _ => delimiter.ToString()
        };
    }

    private sealed record DelimiterAnalysis(char Delimiter, int ExpectedHeaderMatches, int ColumnCount);
}