namespace ContentMigrationValidator.Models;

public sealed class ContentPageReadResult
{
    public required IReadOnlyList<ContentPage> Pages { get; init; }
    public string? InputDetails { get; init; }
}