# Changelog

All notable changes to this project will be documented in this file.

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

[1.0.0]: https://github.com/ferdikurnazdm/Resultron/releases/tag/v1.0.0