# Advent of Code 2024

This project is designed to be run in a GitHub `Codespace` or Docker `Devcontainer`

It can of course be run on any machine with [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed

## Bootstrap

```sh
dotnet new sln

dotnet new console -o ./src/A01

dotnet sln add ./src/A01
```

## Run

```sh
dotnet run --project ./src/A01/A01.csproj
```
