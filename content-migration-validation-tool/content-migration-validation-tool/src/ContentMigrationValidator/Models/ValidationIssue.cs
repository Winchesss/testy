namespace ContentMigrationValidator.Models;

public enum ValidationIssueCode
{
    MissingRequiredField,
    MissingUrl,
    InvalidUrl,
    MissingImage,
    MissingAttachment,
    InvalidStatus
}

public enum ValidationSeverity
{
    Warning,
    Error
}

public sealed record ValidationIssue(
    ValidationIssueCode Code,
    int SourceRowNumber,
    string? PageId,
    string Field,
    string Message,
    ValidationSeverity Severity = ValidationSeverity.Error);
