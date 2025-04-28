# xatlas.NET
This repository contains low-level bindings for [xatlas](https://github.com/jpcy/xatlas) used in Evergine

[![CI](https://github.com/EvergineTeam/XAtlas.NET/actions/workflows/CI.yml/badge.svg)](https://github.com/EvergineTeam/XAtlas.NET/actions/workflows/CI.yml)
[![CD](https://github.com/EvergineTeam/XAtlas.NET/actions/workflows/CD.yml/badge.svg)](https://github.com/EvergineTeam/XAtlas.NET/actions/workflows/CD.yml)
[![Nuget](https://img.shields.io/nuget/v/Evergine.Bindings.XAtlas?logo=nuget)](https://www.nuget.org/packages/Evergine.Bindings.XAtlas)

This repository provides a **lightweight, automatically generated C# binding** for the native [xatlas](https://github.com/jpcy/xatlas) library. xatlas is an open-source library for generating high-quality UV atlases for 3D geometry. This binding allows .NET developers to access xatlas functionality in managed C# code.

## Features

- **Automatic Binding Generation**: All native functions and types from xatlas are exposed through P/Invoke, generated via automated tooling.
- **Lightweight**: Minimal dependencies; just the native xatlas library and the generated C# shim.
- **Cross-Platform**: Works on Windows, Linux, and macOS. Ensure the appropriate native binary is available for your platform.


### NuGet

Install the package from NuGet:

```sh
Install-Package xatlas.NET

```
Or via the .NET CLI:

dotnet add package xatlas.NET

### Usage

Checkout the test project
https://github.com/EvergineTeam/xatlas.NET/blob/main/XAtlasGen/Test/Program.cs

### License
This binding is provided under the MIT License, matching the license of the original xatlas project.

### Acknowledgements
Original xatlas library by Jonathan Pontier (jpcy/xatlas)
EvergineTeam for maintaining this C# wrappe
