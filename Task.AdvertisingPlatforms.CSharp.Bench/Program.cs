using System.Diagnostics;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Running;
using Task.AdvertisingPlatforms.CSharp.Core.Interfaces;
using Task.AdvertisingPlatforms.CSharp.Core.Services;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using Spectre.Console;

public class StorageBenchmark
{
    private IAdvertPlatformStorage _storage;
    private readonly string[] _locations = new[]
    {
        "/ru/msk",
        "/ru/svrd",
        "/ru/svrd/revda",
        "/ru"
    };

    [GlobalSetup]
    public void Setup()
    {
        _storage = new AdvertPlatformStorage();
        var parser = new AdventPlatformParser();
        var content_ok = File.ReadAllText("../../../../../../../../TestFiles/File1_ok");
        var data = parser.Parse(content_ok);
        _storage.Add(data);
    }

    [Benchmark]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public int GetAdvertisingPlatformsByLocation(int index)
    {
        var result = _storage.GetPlatforms(_locations[index]);
        return result.Count();
    }
}

public class Program
{
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
        string currentDirectory = Environment.CurrentDirectory;
        string relativePath = "../Task.AdvertisingPlatforms.CSharp.Tests/Task.AdvertisingPlatforms.CSharp.Tests.csproj";
        string testProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, relativePath));
        bool testsPassed = RunDotNetTest(testProjectPath);

        if (testsPassed)
        {

            
            Console.WriteLine("Все тесты успешны! Запускаем Benchmark...");

            var config = ManualConfig.CreateEmpty()
                .AddExporter(MarkdownExporter.Console) // обязательно, чтобы summary попал в md
                .AddColumnProvider(DefaultColumnProviders.Instance);

            // Запустить анимацию. Внутри анимации ждём завершения задачи.
            var benchmarkTask = System.Threading.Tasks.Task.Run(() =>
            {
                // Тяжёлая задача
                return BenchmarkRunner.Run<StorageBenchmark>(config);
            });

            await AnsiConsole.Status()
                .StartAsync("Выполняется Benchmark, ожидайте   ", async ctx =>
                {
                    // Тут анимация включена, ждём завершения задачи
                    await benchmarkTask;
                    // Можете в процессе обновлять статус через ctx.Status()
                });

            AnsiConsole.MarkupLine("Benchmark выполнен");
            
            

            // Находим файл артефакта
            string artifactsDir = Path.Combine(Environment.CurrentDirectory, "BenchmarkDotNet.Artifacts", "results");
            string fileName = $"StorageBenchmark-report-console.md";
            string filePath = Path.Combine(artifactsDir, fileName);

            if (File.Exists(filePath))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("====== Benchmark сводка ======");
                string[] lines = File.ReadAllLines(filePath);

                bool inSummary = false;
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine("================================");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Не найден файл с результатами: {filePath}");
            }
        }
        else
        {
            Console.WriteLine("Некоторые тесты провалились, бенчмарк не будет запущен.");
        }
    }

    
    public static bool RunDotNetTest(string projectOrDllPath)
    {
        Console.WriteLine($"Запускаем тесты из: {projectOrDllPath}");
        
        var process = new Process
        {
            StartInfo =
            {
                FileName = "dotnet",
                Arguments = $"test \"{projectOrDllPath}\" --no-build --logger \"console;verbosity=detailed\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(output);
        Console.ResetColor();
        if (!string.IsNullOrEmpty(error))
            Console.WriteLine("Errors: " + error);

        return process.ExitCode == 0;
    }
}