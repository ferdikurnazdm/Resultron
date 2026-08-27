# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

## [1.4.1] - 2026-08-27

### Added

- Added async pipeline support with `MapAsync` and `BindAsync` methods.

- Added async matching support with `MatchAsync` overloads for `Result` and `Result<T>`.

- Added async side-effect support with `Result<T>.MapAsync(Func<T, Task>)`.

- Added async chaining support for `Result`, `Result<T>`, and nested result transformations.

- Added async unit tests covering `MapAsync`, `BindAsync`, and `MatchAsync` operations.

- Added async usage examples to the `Resultron.Sample` console application.

### Changed

- Extended `Result` and `Result<T>` APIs with asynchronous equivalents while preserving existing synchronous behavior.

- Improved sample application examples to demonstrate async workflows, failure propagation, and fluent result pipelines.


## [1.3.1] - 2026-08-24

### Changed

- Changed logo.

## [1.2.1] - 2026-07-20

### Added

- Added strict versioning property to the main `.csproj` file.
- edited nuget.yml file.


## [1.1.1] - 2026-05-25

### Added

- Added strict versioning property to the main `.csproj` file.
- Added `<IsPackable>false</IsPackable>` configuration to the sample project to prevent accidental package creation.

### Changed

- Updated the `nuget.yml` workflow file to pack and deploy only the core library.

## [1.1.0] - 2026-05-25

### Added

- Added `docs.yml` workflow file for automated documentation deployment.
- Added `nuget.yml` workflow file for NuGet package deployment.
- Added **DocFX** support for generating the project documentation site.

### Changed

- Updated and edited the build status badge link in the README.
- Updated nuget project URL.


## [1.0.0] - 2026-05-25

### Added

- Initial implementation of `Result` and `Result<T>` types.
- Added `Try`, `TryAsync`, `Map`, `Bind`, and `Match` methods.
- Added `Result.Bind<T>` overload for chaining `Result` to `Result<T>`.
- Added `Result<T>.Bind` overload for chaining `Result<T>` to `Result`.
- Added fluent pipeline support.
- Added exception-safe operations with `Result.Try` and `Result.TryAsync`.
- Added async support with `Result.TryAsync` and `Result<T>.TryAsync`.
- Added structured `Error` record with `Code` and `Description` properties.
- Added unit tests for `Result` and `Result<T>`.
- Added sample console application: `Resultron.Sample`.

### Changed

- Updated project folder structure.
- Refactored core code for clarity and consistency.
- Updated `.Core` namespace.
- Updated README with correct package name, badges, and usage examples.
- Updated DocFX project and sample documentation.
- Updated `global.json` SDK version to `10.0.200`.
- Updated unit tests to use FluentAssertions naming conventions.
- Added `.vs/` folder to `.gitignore`.

### Removed

- Removed `references/` folder.
- Removed `reports/` folder.

[unreleased]: https://github.com/ferdikurnazdm/Resultron/
[1.4.1]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.4.1
[1.3.1]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.3.1
[1.2.1]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.2.1
[1.1.1]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.1.1
[1.1.0]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.1.0
[1.0.0]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.0.0