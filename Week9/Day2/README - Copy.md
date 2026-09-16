name: CI

on:
  push:
    branches: [ main, master, develop ]
  pull_request:
    branches: [ main, master, develop ]
  workflow_dispatch:

jobs:
  build-and-test:
    name: Build & Test
    runs-on: ubuntu-latest
    defaults:
      run:
        working-directory: Week9/Day2/CardioTrack

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup .NET 8
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      - name: Cache NuGet packages
        uses: actions/cache@v4
        with:
          path: ~/.nuget/packages
          key: ${{ runner.os }}-nuget-${{ hashFiles('Week9/Day2/CardioTrack/**/*.csproj') }}
          restore-keys: |
            ${{ runner.os }}-nuget-

      - name: Restore dependencies
        run: dotnet restore CardioTrack.sln

      - name: Build
        run: dotnet build CardioTrack.sln --configuration Release --no-restore

      - name: Test
        run: >
          dotnet test CardioTrack.sln
          --configuration Release
          --no-build
          --verbosity normal
          --logger "trx;LogFileName=test-results.trx"
          --collect:"XPlat Code Coverage"

      - name: Upload test results
        uses: actions/upload-artifact@v4
        if: always()
        with:
          name: test-results
          path: |
            Week9/Day2/CardioTrack/**/TestResults/**/*.trx
            Week9/Day2/CardioTrack/**/TestResults/**/coverage.cobertura.xml
          retention-days: 7