# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

### Added

- Initial implementation of `Result` and `Result<T>` types
- `Try`, `TryAsync`, `Map`, `Bind`, and `Match` methods
- `Result.Bind<T>` overload for chaining `Result` to `Result<T>`
- `Result<T>.Bind` overload for chaining `Result<T>` to `Result`
- Fluent pipeline support
- Exception-safe operations with `Result.Try` and `Result.TryAsync`
- Async support with `Result.TryAsync` and `Result<T>.TryAsync`
- Structured `Error` record with `Code` and `Description` properties
- Unit tests added (`ResultTests`, `ResultOfTTests`)
- Sample console application added (`Resultron.Sample`)
- `docs.yml` workflow added for GitHub Pages

### Changed

- Updated project folder structure
- Refactored core code for clarity and consistency
- Updated `.Core` namespace
- Updated README with correct package name, badges, and usage examples
- Updated DocFX project and sample documentation
- Updated `global.json` SDK version to `10.0.200`
- Updated unit tests to use FluentAssertions naming conventions
- `.vs/` folder added to `.gitignore`

### Fixed

- Fixed `docs.yml` `--force` flag on `Build Documentation` step

### Removed

- `references/` folder removed
- `reports/` folder removed



[unreleased]: https://github.com/ferdikurnazdm/Resultron

