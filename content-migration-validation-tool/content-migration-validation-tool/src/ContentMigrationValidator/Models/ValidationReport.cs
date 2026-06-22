namespace ContentMigrationValidator.Models;

public sealed class ValidationReport
{
    public required string InputFilePath { get; init; }
    public string? InputDetails { get; init; }
    public required IReadOnlyList<ContentPage> Pages { get; init; }
    public required IReadOnlyList<ValidationIssue> Issues { get; init; }
    public DateTime GeneratedAt { get; init; } = DateTime.Now;

    public int TotalPages => Pages.Count;

    public int PagesWithMissingUrl => CountDistinctPages(ValidationIssueCode.MissingUrl);
    public int PagesWithMissingImage => CountDistinctPages(ValidationIssueCode.MissingImage);
    public int PagesWithMissingAttachment => CountDistinctPages(ValidationIssueCode.MissingAttachment);
    public int InvalidUrls => Issues.Count(issue => issue.Code == ValidationIssueCode.InvalidUrl);
    public int InvalidStatuses => Issues.Count(issue => issue.Code == ValidationIssueCode.InvalidStatus);

    public int RecordsWithMissingRequiredFields => Issues
        .Where(issue => issue.Code is ValidationIssueCode.MissingRequiredField
            or ValidationIssueCode.MissingUrl
            or ValidationIssueCode.MissingImage
            or ValidationIssueCode.MissingAttachment)
        .Select(issue => issue.SourceRowNumber)
        .Distinct()
        .Count();

    private int CountDistinctPages(ValidationIssueCode code)
    {
        return Issues
            .Where(issue => issue.Code == code)
            .Select(issue => issue.SourceRowNumber)
            .Distinct()
            .Count();
    }
}