using ContentMigrationValidator.Models;

namespace ContentMigrationValidator.Services;

public sealed class ValidationService
{
    private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "OK",
        "ERROR",
        "WARNING"
    };

    public ValidationReport Validate(IReadOnlyList<ContentPage> pages, string inputFilePath, string? inputDetails = null)
    {
        var issues = new List<ValidationIssue>();

        foreach (var page in pages)
        {
            ValidateRequiredText(page, page.PageId, "pageId", issues);
            ValidateRequiredText(page, page.Title, "title", issues);
            ValidateUrl(page, issues);
            ValidateImage(page, issues);
            ValidateAttachment(page, issues);
            ValidateStatus(page, issues);
        }

        return new ValidationReport
        {
            InputFilePath = inputFilePath,
            InputDetails = inputDetails,
            Pages = pages,
            Issues = issues
        };
    }

    private static void ValidateRequiredText(
        ContentPage page,
        string? value,
        string field,
        ICollection<ValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.MissingRequiredField,
                page,
                field,
                $"Required field '{field}' is missing."));
        }
    }

    private static void ValidateUrl(ContentPage page, ICollection<ValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(page.Url))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.MissingUrl,
                page,
                "url",
                "URL is missing."));
            return;
        }

        if (!page.Url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            && !page.Url.StartsWith('/'))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.InvalidUrl,
                page,
                "url",
                "URL must start with 'https://' or '/'."));
        }
    }

    private static void ValidateImage(ContentPage page, ICollection<ValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(page.ImagePath))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.MissingImage,
                page,
                "imagePath",
                "Image path is missing."));
        }
    }

    private static void ValidateAttachment(ContentPage page, ICollection<ValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(page.AttachmentPath))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.MissingAttachment,
                page,
                "attachmentPath",
                "Attachment path is missing."));
        }
    }

    private static void ValidateStatus(ContentPage page, ICollection<ValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(page.Status))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.MissingRequiredField,
                page,
                "status",
                "Required field 'status' is missing."));
            return;
        }

        if (!ValidStatuses.Contains(page.Status))
        {
            issues.Add(CreateIssue(
                ValidationIssueCode.InvalidStatus,
                page,
                "status",
                "Status must be one of: OK, ERROR, WARNING."));
        }
    }

    private static ValidationIssue CreateIssue(
        ValidationIssueCode code,
        ContentPage page,
        string field,
        string message)
    {
        return new ValidationIssue(
            code,
            page.SourceRowNumber,
            page.PageId,
            field,
            message);
    }
}