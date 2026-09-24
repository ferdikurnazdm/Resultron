```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9448/25H2/2025Update/HudsonValley2)
Intel Core Ultra 7 255U 2.00GHz, 1 CPU, 14 logical and 12 physical cores
.NET SDK 10.0.301
  [Host]     : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3


```
| Type                     | Method                        | Mean          | Error       | StdDev      | Median        | Ratio  | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------------------------------ |--------------:|------------:|------------:|--------------:|-------:|--------:|-----:|-------:|----------:|------------:|
| ResultAsyncBenchmarks    | TryAsync_CompletedTask        |     17.528 ns |   0.4159 ns |   0.4085 ns |     17.556 ns |   1.00 |    0.03 |    6 | 0.0166 |     104 B |        1.00 |
| ResultChainBenchmarks    | FullChainSuccess              |     44.097 ns |   1.1009 ns |   3.2462 ns |     43.665 ns |   2.52 |    0.19 |    8 | 0.0331 |     208 B |        2.00 |
| ResultCreationBenchmarks | Success                       |      6.488 ns |   0.2891 ns |   0.8479 ns |      6.133 ns |   0.37 |    0.05 |    4 | 0.0051 |      32 B |        0.31 |
| ResultGenericBenchmarks  | MapSuccess                    |      6.818 ns |   0.2773 ns |   0.8046 ns |      6.590 ns |   0.39 |    0.05 |    4 | 0.0051 |      32 B |        0.31 |
| ResultTryBenchmarks      | TrySuccess                    |      7.293 ns |   0.1752 ns |   0.3617 ns |      7.202 ns |   0.42 |    0.02 |    5 | 0.0051 |      32 B |        0.31 |
| ResultAsyncBenchmarks    | TryAsync_TaskYield            |  1,013.808 ns |  18.6560 ns |  49.7966 ns |  1,004.311 ns |  57.87 |    3.12 |    9 | 0.0362 |     231 B |        2.22 |
| ResultChainBenchmarks    | FullChainFailure              |  2,796.883 ns |  55.3075 ns | 105.2283 ns |  2,796.131 ns | 159.64 |    6.97 |   12 | 0.1526 |     976 B |        9.38 |
| ResultCreationBenchmarks | Failure                       |      5.235 ns |   0.1740 ns |   0.1542 ns |      5.195 ns |   0.30 |    0.01 |    2 | 0.0051 |      32 B |        0.31 |
| ResultGenericBenchmarks  | MapFailure                    |      5.978 ns |   0.1384 ns |   0.1594 ns |      5.962 ns |   0.34 |    0.01 |    3 | 0.0051 |      32 B |        0.31 |
| ResultTryBenchmarks      | TryFailure                    |  1,913.215 ns |   6.0759 ns |   5.3861 ns |  1,914.858 ns | 109.21 |    2.51 |   11 | 0.0610 |     384 B |        3.69 |
| ResultAsyncBenchmarks    | TryAsync_Failure              | 14,084.682 ns | 271.6966 ns | 656.1780 ns | 14,030.142 ns | 803.95 |   41.47 |   13 | 0.2136 |    1432 B |       13.77 |
| ResultCreationBenchmarks | GenericSuccess                |      6.397 ns |   0.1471 ns |   0.1228 ns |      6.359 ns |   0.37 |    0.01 |    4 | 0.0051 |      32 B |        0.31 |
| ResultGenericBenchmarks  | BindSuccess                   |      7.384 ns |   0.8071 ns |   2.3797 ns |      6.514 ns |   0.42 |    0.14 |    5 | 0.0051 |      32 B |        0.31 |
| ResultTryBenchmarks      | GenericTrySuccess             |      6.842 ns |   0.1691 ns |   0.3417 ns |      6.676 ns |   0.39 |    0.02 |    4 | 0.0051 |      32 B |        0.31 |
| ResultAsyncBenchmarks    | GenericTryAsync_CompletedTask |     23.359 ns |   0.6150 ns |   1.7941 ns |     23.284 ns |   1.33 |    0.11 |    7 | 0.0280 |     176 B |        1.69 |
| ResultCreationBenchmarks | GenericFailure                |      7.511 ns |   0.2075 ns |   0.3351 ns |      7.473 ns |   0.43 |    0.02 |    5 | 0.0051 |      32 B |        0.31 |
| ResultGenericBenchmarks  | BindFailure                   |      4.879 ns |   0.1765 ns |   0.3525 ns |      4.824 ns |   0.28 |    0.02 |    1 | 0.0051 |      32 B |        0.31 |
| ResultTryBenchmarks      | GenericTryFailure             |  1,898.590 ns |  18.2008 ns |  17.0250 ns |  1,902.414 ns | 108.37 |    2.65 |   11 | 0.0610 |     384 B |        3.69 |
| ResultAsyncBenchmarks    | GenericTryAsync_TaskYield     |  1,100.427 ns |  21.9188 ns |  58.5056 ns |  1,092.984 ns |  62.81 |    3.62 |   10 | 0.0362 |     231 B |        2.22 |
| ResultAsyncBenchmarks    | GenericTryAsync_Failure       | 13,451.888 ns | 264.7378 ns | 234.6831 ns | 13,512.780 ns | 767.83 |   21.78 |   13 | 0.2289 |    1432 B |       13.77 |
