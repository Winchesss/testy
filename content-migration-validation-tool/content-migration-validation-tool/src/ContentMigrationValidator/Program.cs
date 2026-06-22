using ContentMigrationValidator.Models;
using ContentMigrationValidator.Services;

namespace ContentMigrationValidator;

internal static class Program
{
    private static int Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                return RunDefaultValidationForCsvAndJson();
            }

            return RunSingleFileValidation(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Validation failed.");
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    private static int RunDefaultValidationForCsvAndJson()
    {
        var csvInputPath = ResolveDefaultInputPath("migrated_pages.csv");
        var jsonInputPath = ResolveDefaultInputPath("migrated_pages.json");

        if (csvInputPath is null || jsonInputPath is null)
        {
            PrintUsage();
            return 1;
        }

        var jobs = new[]
        {
            new ValidationJob(csvInputPath, ResolveDefaultOutputPath("csv_validation_report.txt")),
            new ValidationJob(jsonInputPath, ResolveDefaultOutputPath("json_validation_report.txt"))
        };

        var reports = new List<ValidationReport>();

        foreach (var job in jobs)
        {
            var report = RunValidation(job.InputPath, job.OutputPath);
            reports.Add(report);

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();
        }

        Console.WriteLine("Done. Reports generated:");
        foreach (var job in jobs)
        {
            Console.WriteLine($"- {Path.GetFullPath(job.OutputPath)}");
        }

        return reports.Any(report => report.Issues.Count > 0) ? 2 : 0;
    }

    private static int RunSingleFileValidation(string[] args)
    {
        var inputPath = args[0];

        if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
        {
            PrintUsage();
            return 1;
        }

        var outputPath = args.Length >= 2
            ? args[1]
            : ResolveDefaultOutputPath(GetDefaultReportFileName(inputPath));

        var report = RunValidation(inputPath, outputPath);

        return report.Issues.Count == 0 ? 0 : 2;
    }

    private static ValidationReport RunValidation(string inputPath, string outputPath)
    {
        var reader = new ContentPageFileReader();
        var readResult = reader.Read(inputPath);

        var validationService = new ValidationService();
        var report = validationService.Validate(readResult.Pages, inputPath, readResult.InputDetails);

        var reportWriter = new ReportWriter();
        var reportText = reportWriter.BuildReport(report);
        reportWriter.WriteReport(outputPath, reportText);

        Console.WriteLine(reportText);
        Console.WriteLine();
        Console.WriteLine($"Report saved to: {Path.GetFullPath(outputPath)}");

        return report;
    }

    private static string GetDefaultReportFileName(string inputPath)
    {
        var extension = Path.GetExtension(inputPath).ToLowerInvariant();

        return extension switch
        {
            ".csv" => "csv_validation_report.txt",
            ".json" => "json_validation_report.txt",
            _ => $"validation_report_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
    }

    private static string? ResolveDefaultInputPath(string fileName)
    {
        var candidates = new[]
        {
            Path.Combine("test-data", fileName),
            Path.Combine("..", "..", "..", "test-data", fileName),
            Path.Combine("..", "..", "..", "..", "test-data", fileName),
            Path.Combine("..", "..", "..", "..", "..", "test-data", fileName)
        };

        return candidates.FirstOrDefault(File.Exists);
    }

    private static string ResolveDefaultOutputPath(string fileName)
    {
        var candidateDirectories = new[]
        {
            "reports",
            Path.Combine("..", "..", "..", "reports"),
            Path.Combine("..", "..", "..", "..", "reports"),
            Path.Combine("..", "..", "..", "..", "..", "reports")
        };

        var existingDirectory = candidateDirectories.FirstOrDefault(Directory.Exists);
        return Path.Combine(existingDirectory ?? "reports", fileName);
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Content Migration Validation Tool");
        Console.WriteLine();
        Console.WriteLine("Default usage from the repository root:");
        Console.WriteLine("  dotnet run --project src/ContentMigrationValidator/ContentMigrationValidator.csproj");
        Console.WriteLine();
        Console.WriteLine("This validates both default files:");
        Console.WriteLine("  test-data/migrated_pages.csv");
        Console.WriteLine("  test-data/migrated_pages.json");
        Console.WriteLine();
        Console.WriteLine("And creates two report files:");
        Console.WriteLine("  reports/csv_validation_report.txt");
        Console.WriteLine("  reports/json_validation_report.txt");
        Console.WriteLine();
        Console.WriteLine("Optional single-file usage:");
        Console.WriteLine("  dotnet run -- <input-file.csv|input-file.json> [output-report.txt]");
    }

    private sealed record ValidationJob(string InputPath, string OutputPath);
}