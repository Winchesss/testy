# Content Migration Validation Tool

Simple C#/.NET console application for validating migrated page data from CSV and JSON files.

## What the program does

The program checks migrated page records and reports problems such as:

* missing URL,
* invalid URL,
* missing image path,
* missing attachment path,
* invalid page status,
* missing required fields.

## Input files

Default input files are located in:

```text
test-data/migrated_pages.csv
test-data/migrated_pages.json
```

The CSV file can use either:

```text
,
```

or:

```text
;
```

as a separator. The program detects the separator automatically.

## Output files

The program creates two text reports:

```text
reports/csv_validation_report.txt
reports/json_validation_report.txt
```

## How to run

From the main project folder, run:

```bash
dotnet run --project src/ContentMigrationValidator/ContentMigrationValidator.csproj
```

## Example output

```text
Validation report

Total pages: 3
Pages with missing URL: 1
Pages with missing image: 1
Pages with missing attachment: 1
Invalid URLs: 1
```

## Technologies used

* C#
* .NET 8
* CSV
* JSON
* Console application
