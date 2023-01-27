# Technical Overview

## Tools

### Hardware

| Minimal Configuration | Windows                                      | MacOS                          |
|-----------------------|----------------------------------------------|--------------------------------|
| OS Version            | Windows 7 (SP1+), 64-bit, Windows 10, 64-bit | Sierra 10.12.6+                |
| CPU                   | x64 with SSE2 instruction set                | x64 with SSE2 instruction set  |
| Graphics              | Compatible DX10, DX11 & DX12                 | Compatible Metal (Intel & AMD) |

### Software

| Product                 | Usage                                        |
|-------------------------|----------------------------------------------|
| Unity 2021.3.17f1 (LTS) | Game Engine & Editor                         |
| Git                     | Versioning                                   |
| Blender 2.8+            | Modeling, Rigging, Animating                 |
| Adobe CC                | Concept Art, Painting, Texturing, UI         |


## Development Environment

### Versioning

* Source code versioning is done using Git.
* Assets versioning is done using Git LFS.
* The repository is hosted on Github.

### Workflow

For the source code, the project follow the Gitflow workflow.
Assets are integrated on the `asset_integration` branch.

### Project Structure

```
📂Docs
📂Game
 ┣ 📂External
 ┣ 📂Content
 ┃  ┣ 📂[Shared]
 ┃  ┣ 📂Characters
 ┃  ┃   ┗ 🧊Player.prefab
 ┃  ┗ 📂Levels
 ┃      ┗ 🪐Level1.scene
 ┣ 📂ContentSettings
 ┗ 📂Sources
    ┗ 📜MyScript.cs
```

Important folders are named between square brackets `[]`. 
Example : `[Shared]`, `[Test]`, `[Editor]`, etc...

### Code Guidelines

We follow the Microsoft C# coding style :  
https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
