# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Keep a Changelog] and this project adheres to [Semantic Versioning][Semantic Versioning].

## [Unreleased]

### Added
- Cancellation to API calls

## [1.0.0-alpha2] - 2021-01-03

### Added
- API Wrapper for GetImage
- Unit test project
- Unit tests for GetTag and GetImage

### Changed
- Return TagResponseModel from GetTag instead of TagModel
- Restructure project and namespaces
- Configure build stages

## [1.0.0-alpha1] - 2020-12-19

### Added
- API wrappers
    - SearchImages
    - SearchTags
    - GetTag
- Examples of searching for images and tags
- CI build and deploy

<!-- Links -->
[Keep a Changelog]: https://keepachangelog.com/
[Semantic Versioning]: https://semver.org/

<!-- Versions -->
[Unreleased]: https://github.com/Sibusten/philomena-api/compare/v1.0.0-alpha2...HEAD
[1.0.0-alpha2]: https://github.com/Sibusten/philomena-api/compare/v1.0.0-alpha1..v1.0.0-alpha2
[1.0.0-alpha1]: https://github.com/Sibusten/philomena-api/releases/v1.0.0-alpha1
