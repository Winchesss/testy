using System.Text;
using ContentMigrationValidator.Models;

namespace ContentMigrationValidator.Services;

public sealed class ReportWriter
{
    public string BuildReport(ValidationReport report)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Validation report");
        builder.AppendLine("=================");
        builder.AppendLine();
        if (!string.IsNullOrWhiteSpace(report.InputDetails))
        {
            builder.AppendLine($"Input details: {report.InputDetails}");
        }
        builder.AppendLine($"Generated at: {report.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
        builder.AppendLine();
        builder.AppendLine($"Total pages: {report.TotalPages}");
        builder.AppendLine($"Pages with missing URL: {report.PagesWithMissingUrl}");
        builder.AppendLine($"Pages with missing image: {report.PagesWithMissingImage}");
        builder.AppendLine($"Pages with missing attachment: {report.PagesWithMissingAttachment}");
        builder.AppendLine($"Invalid URLs: {report.InvalidUrls}");
        builder.AppendLine($"Invalid statuses: {report.InvalidStatuses}");
        builder.AppendLine($"Records with missing required fields: {report.RecordsWithMissingRequiredFields}");
        builder.AppendLine($"Total issues: {report.Issues.Count}");

        if (report.Issues.Count == 0)
        {
            builder.AppendLine();
            builder.AppendLine("No validation issues found.");
            return builder.ToString();
        }

        builder.AppendLine();
        builder.AppendLine("Detailed issues:");

        foreach (var issue in report.Issues.OrderBy(issue => issue.SourceRowNumber).ThenBy(issue => issue.Field))
        {
            var pageLabel = string.IsNullOrWhiteSpace(issue.PageId)
                ? $"row {issue.SourceRowNumber}"
                : $"page {issue.PageId}, row {issue.SourceRowNumber}";

            builder.AppendLine($"- {pageLabel}, field '{issue.Field}': {issue.Message}");
        }

        return builder.ToString();
    }

    public void WriteReport(string outputPath, string reportText)
    {
        var directory = Path.GetDirectoryName(outputPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(outputPath, reportText, Encoding.UTF8);
    }
}