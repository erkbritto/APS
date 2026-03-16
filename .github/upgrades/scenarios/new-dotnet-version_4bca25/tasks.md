# APS ASP.NET Core net8.0 Upgrade Tasks

## Overview

This document tracks the execution of an atomic upgrade of the `APS` solution to `net8.0`. The upgrade follows an all-at-once approach: prerequisites verification, one atomic project/package/compilation change, then automated testing/verification per the plan.

**Progress**: 0/3 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [ ] TASK-001: Verify prerequisites
**References**: Plan §Phase 0 — Preparation, Plan §Migration Steps

- [ ] (1) Verify .NET 8.0 SDK is installed on the build machine per Plan §Phase 0 — Preparation (e.g., `dotnet --list-sdks`)  
- [ ] (2) Runtime/SDK meets minimum requirements for `net8.0` (**Verify**)  
- [ ] (3) Check `global.json` in the repository root (if present) for a compatible SDK version and update it only if required per Plan §Migration Steps  
- [ ] (4) Verify configuration files (`Directory.Build.props`, `Directory.Packages.props`, `Directory.Build.targets`) compatibility with target framework per Plan §Detailed Dependency Analysis (**Verify**)

### [ ] TASK-002: Atomic framework and package upgrade with compilation fixes
**References**: Plan §Project-by-Project Plans (Project: `APS.csproj`), Plan §Migration Strategy, Plan §Package Update Reference, Plan §Breaking Changes Catalog, Plan §Source Control Strategy

- [ ] (1) Update `TargetFramework` in `APS.csproj` to `net8.0` and update any `Directory.Build.props`, `Directory.Build.targets`, or `Directory.Packages.props` entries that reference framework or package versions per Plan §Project-by-Project Plans  
- [ ] (2) All project/MSBuild import files updated to target `net8.0` (**Verify**)  
- [ ] (3) Apply package reference updates only if specified in Plan §Package Update Reference (assessment lists none); update packages listed in the Plan if build-time errors require it  
- [ ] (4) Restore dependencies (`dotnet restore`) for the solution per Plan §Migration Strategy  
- [ ] (5) Build the solution and fix all compilation errors found (address issues referenced in Plan §Breaking Changes Catalog)  
- [ ] (6) Solution builds with 0 errors (**Verify**)  
- [ ] (7) Commit changes with message: "TASK-002: Atomic upgrade to net8.0"

### [ ] TASK-003: Run automated tests and smoke verification (per Plan)
**References**: Plan §Project-by-Project Plans, Plan §Testing & Validation Strategy

- [ ] (1) Per Plan §Project-by-Project Plans and Plan §Testing & Validation Strategy, no test projects were discovered for `APS.csproj`; therefore skip automated `dotnet test` runs (no test projects listed in the Plan) (**Verify**)  
- [ ] (2) If test projects are later added and explicitly listed in the Plan, run `dotnet test` only for the test projects enumerated in Plan §Testing & Validation Strategy and ensure all tests pass with 0 failures (**Verify**)