namespace ContentMigrationValidator.Models;

public sealed class ContentPage
{
    public int SourceRowNumber { get; init; }
    public string? PageId { get; init; }
    public string? Title { get; init; }
    public string? Url { get; init; }
    public string? ImagePath { get; init; }
    public string? AttachmentPath { get; init; }
    public string? Status { get; init; }
}
