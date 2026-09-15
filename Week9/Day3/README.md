### CI Failure Verification
Deliberately broke `CheckHeartRate_Normal_ReturnsNoAlert` by asserting the 
wrong severity. Pipeline correctly failed at the Test step with:
`Expected: Severity.High, Actual: null`
Confirms the pipeline reliably catches test failures, not just build errors.

# CardioTrack

[![CI Pipeline](https://github.com/hebaayaseh/BinX-Training-2026/actions/workflows/ci.yml/badge.svg)](https://github.com/hebaayaseh/BinX-Training-2026/actions/workflows/ci.yml)

Backend REST API for a Cardiac Patient Monitoring Center...

# Day 3 - Build and Verify the CI Pipeline

## Overview
Built a GitHub Actions CI pipeline that automatically builds and tests 
CardioTrack on every push, verified it correctly detects both success and 
failure, and added a live status badge to the README.

## Steps Completed
-  Wrote `.github/workflows/ci.yml` — checks out code, sets up .NET 8, 
      restores, builds, and runs the full test suite.
-  Pushed and confirmed the pipeline ran successfully on a normal commit.
-  Deliberately broke a test (`CheckHeartRate_Normal_ReturnsNoAlert`) and 
      confirmed the pipeline failed visibly at the Test step with a clear 
      assertion error message.
-  Fixed the test and confirmed the pipeline returned to a passing state.
-  Added a live status badge to the top of the README.

## Pipeline Configuration
- Trigger: push and pull requests to `main`
- Steps: checkout → setup .NET 8 → restore → build (Release) → test (Release)
- All integration tests use EF Core's InMemory provider, so no external 
  MySQL or Redis dependency is required in CI

## Tools Used
- GitHub Actions
- .NET 8 SDK (`actions/setup-dotnet`)
