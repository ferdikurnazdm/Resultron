using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance
    .WithOption(ConfigOptions.JoinSummary, true)
    .AddExporter(MarkdownExporter.GitHub)
    .AddExporter(HtmlExporter.Default);

BenchmarkSwitcher
    .FromAssembly(typeof(Program).Assembly)
    .Run(args, config);