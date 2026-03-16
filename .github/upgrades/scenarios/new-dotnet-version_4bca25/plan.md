# .NET 8.0 Upgrade Plan (All-At-Once)

## Table of Contents
- Executive Summary
- Migration Strategy
- Detailed Dependency Analysis
- Project-by-Project Plans
- Package Update Reference
- Breaking Changes Catalog
- Testing & Validation Strategy
- Risk Management
- Complexity & Effort Assessment
- Source Control Strategy
- Detailed Execution Steps
- Success Criteria

---

## Executive Summary

Selected Strategy
**All-At-Once Strategy** — All projects (the entire solution) will be upgraded simultaneously in a single coordinated operation.

Rationale
- Solution size: 1 project (`APS.csproj`) — small and homogeneous.
- Assessment indicates `APS.csproj` is compatible with `net8.0` (proposed target framework).
- No NuGet package updates or API issues were reported by the assessment.
- Team objective: move to the most stable LTS supported runtime (`.NET 8.0`).

Scope
- Projects to upgrade: `APS.csproj` (ASP.NET Core / Razor Pages)
- Target framework: `net8.0` for all projects
- Package updates: none suggested by assessment
- Tests: none discovered in assessment; recommend adding or discovering test projects before execution

Key deliverable
- A single atomic upgrade that results in the solution targeting `net8.0` and building with zero compilation errors.

---

## Migration Strategy

Approach
- All-At-Once: perform an atomic upgrade of the entire repository so that all project files, imports, and package references (if any) are updated in a single coordinated change.

Why All-At-Once
- Single small project: low risk to perform a unified upgrade.
- Simplifies dependency resolution and avoids multi-targeting complexity.
- Faster path to using an LTS runtime (`.NET 8.0`).

Strategy notes (All-At-Once)
- Update every project file and any shared MSBuild imports simultaneously.
- Validate global SDK constraints (global.json) before making project changes.
- Restore and build solution immediately after updates to capture all compilation issues in one pass.
- Fix compilation issues and re-build until solution builds without errors.
- Run automated tests (if present) after the atomic upgrade completes.

---

## Detailed Dependency Analysis

Summary
- Total projects: 1 (`APS.csproj`)
- Dependency depth: 0 (no project-to-project references)
- Circular dependencies: none
- Shared MSBuild imports: none detected in assessment (verify `Directory.Build.props` and `Directory.Packages.props` in repo root before execution)

Migration order (dependency-respecting)
- All-At-Once requires no phased ordering because there is a single project. The upgrade is atomic and applied to `APS.csproj`.

---

## Project-by-Project Plans

### Project: `APS.csproj`

Current State (from assessment)
- Current/Proposed Target Framework: `net8.0` (assessment indicates compatibility)
- Project type: ASP.NET Core (Razor Pages)
- SDK-style: Yes
- Files: 13
- LOC: 1234
- Dependencies: none reported
- NuGet packages: none reported in assessment
- Test projects: none discovered

Target State
- Target framework: `net8.0`
- All package references updated per assessment (none required)

Migration Steps (what the executor must do)
1. Prerequisites
   - Ensure .NET 8.0 SDK is installed on the build machine.
   - Verify `global.json` (if present) has a compatible SDK version or update it to an SDK that supports `net8.0`.
   - Ensure working tree is clean or follow the chosen pending-changes action (assessment default: commit pending changes).
   - Create and switch to branch `upgrade-to-NET8`.

2. Atomic project change (single coordinated operation)
   - Update `TargetFramework` in `APS.csproj` to `net8.0` (if not already set).
   - Inspect repository for `Directory.Build.props`, `Directory.Build.targets`, and `Directory.Packages.props` and update any `TargetFramework` or package versions there if present.
   - Update any conditional MSBuild logic that relies on framework version constants.

3. Package and import updates
   - Apply all package updates from assessment (none recommended). If subsequent analysis finds packages, update them to the suggested versions and ensure compatibility.

4. Restore & Build
   - Restore dependencies (`dotnet restore`) and build the full solution (`dotnet build`) to surface compilation errors.
   - Fix any compilation errors discovered (see Breaking Changes Catalog for likely areas).

5. Tests and verification
   - Discover and run test projects (if any) with `dotnet test`. Address test failures.
   - Validate Razor Pages startup: ensure Program.cs/Startup changes (if any) are compatible and the app starts successfully in a test environment.

Validation checklist
- [ ] `APS.csproj` targets `net8.0`.
- [ ] Solution builds with 0 compilation errors.
- [ ] No package dependency conflicts.
- [ ] Automated tests pass (if present).

---

## Package Update Reference

- Assessment returned no NuGet package updates. Keep this section as the authoritative list during execution; if package updates are discovered during the build, include them here with exact current and target versions.

Template to use if packages are found during upgrade:
- `PackageName` — Current: `x.y.z` → Target: `a.b.c` — Projects affected: `APS.csproj` — Reason: compatibility/security

---

## Breaking Changes Catalog

Assessment found no API incompatibilities; however, expect and prepare for the following common migration areas when moving modern ASP.NET Core projects to `net8.0` from earlier versions (for executor awareness):

1. Hosting model / Program.cs
   - Minimal API/host bootstrap patterns used in .NET 6 are still supported; verify code compiles and startup lifetimes behave as expected.

2. Obsoleted/removed APIs
   - Some obsolete APIs may have been removed between .NET 6 and .NET 8; watch for compilation errors referencing removed members.

3. Configuration & Options
   - Changes in configuration binding behavior or in default JSON serializer settings (System.Text.Json) — verify startup deserialization behavior.

4. Authentication & Identity
   - Authentication middleware or identity library versions may require configuration adjustments when packages are updated.

5. HTTP/2, Kestrel and TLS defaults
   - Default server settings or transports can change. Verify hosting and platform-specific behaviors in staging.

6. Entity Framework Core (if present)
   - EF Core provider updates may require migrations or API adjustments. (Not applicable per current assessment but include if EF is added.)

Notes
- Most breaking changes will be discovered during the restore/build step. Catalog actual errors at that point and add them to this section with file/line references.

---

## Testing & Validation Strategy

Levels
- Per-Project: build and unit tests (if any)
- Integration: run integration tests that exercise Razor Pages endpoints (if present)
- Smoke: automated sanity check that the app starts and responds to a small set of requests

Actions
- Discover test projects (if none were found in assessment, search repo for `*.Tests` or test framework references) and include them in automated test runs.
- Run `dotnet test` for each test project.
- Run a minimal smoke test (automated script or integration test) that issues a GET against `/` and checks for a 200 response.

Automatable checks only
- Include only automated checks in execution tasks. Manual UI verification may be listed as optional validation but not part of automated tasks.

---

## Risk Management

Risk summary
- Overall risk: Low (single small project, assessment shows compatibility)

Potential risks
- Missing or undetected NuGet packages that require updates
- Undiscovered runtime behavioral changes not caught by compilation
- Missing automated tests increases detection time for regressions

Mitigations
- Validate SDK and global.json before changes
- Run a full solution build immediately after atomic change to capture all compile-time issues
- If packages are discovered, update them as part of the same atomic operation
- Create a rollback plan (branch-level): keep `master` intact and apply changes on `upgrade-to-NET8` branch; if critical issues arise, revert branch

Contingency
- If the build fails with many blocking errors, revert plan to investigate specific compatibility issues and consider adding a temporary multi-targeting approach

---

## Complexity & Effort Assessment

Per-project complexity (relative)
- `APS.csproj` — Complexity: Low
  - Small LOC footprint, no package updates reported, SDK-style project, Razor Pages

Notes
- No time estimates provided (per constraints). Use relative complexity only.

---

## Source Control Strategy

Branching
- Start from source branch: `master` (as provided by assessment)
- Create and switch to: `upgrade-to-NET8`
- Commit all pending changes before starting the upgrade (assessment default: commit pending changes)

Commit strategy
- Single atomic commit for the upgrade changes, if feasible, to ensure the change is atomic and easy to review. Commit should include:
  - Project file TargetFramework updates
  - Any shared MSBuild imports updates
  - Package reference updates (if any)

Pull Request / Review
- Open a PR from `upgrade-to-NET8` into `master` with a clear description listing what was changed and why (target framework, package versions).
- Include checklist: build successful, tests passed, smoke test results.

Rollback
- If issues are discovered after merge, revert the merge commit and re-open the upgrade branch for troubleshooting.

---

## Detailed Execution Steps

Phase 0 — Preparation (pre-conditions)
- Ensure `.NET 8.0 SDK` installed and available on CI/build agents.
- Validate `global.json` compatibility (update if necessary).
- Ensure working tree is clean; commit pending changes as assessment recommended.
- Create `upgrade-to-NET8` branch and switch to it.

Phase 1 — Atomic Upgrade (single coordinated operation)
- Update `APS.csproj` TargetFramework to `net8.0`.
- Update any MSBuild imports (`Directory.Build.props`, `Directory.Packages.props`) that reference framework or package versions.
- Restore packages (`dotnet restore`).
- Build the solution (`dotnet build`) and fix compilation errors discovered.
- Run static analyzers (optional) and address critical warnings if applicable.
- Commit the atomic upgrade changes in a single commit.

Phase 2 — Test Validation
- Discover and run tests with `dotnet test`.
- Run smoke tests against the running app in a test environment (automated requests checking expected responses).
- If tests fail, fix issues and re-run tests on the same branch.

Phase 3 — Review and Merge
- Create PR with the atomic upgrade commit.
- Include build/test badges and checklist.
- After review and CI passing, merge to `master`.
- Monitor application in staging.

---

## Success Criteria

The upgrade is complete when all of the following are true:
1. All projects (here `APS.csproj`) target `net8.0`.
2. All package updates listed in assessment have been applied (none in assessment).
3. Solution builds with 0 compilation errors.
4. Automated tests pass (if present). If no tests exist, add smoke/integration tests before merging.
5. No unresolved security vulnerabilities flagged by dependency scanning remain.

---

*Notes & Next steps*
- The assessment indicates zero package or API issues; still validate at build time as discovery may reveal additional items.
- If you want, I can now open this `plan.md` in the editor for review or proceed to generate TASKS markdown for execution planning (TASKs will follow All-At-Once batching rules).

*Planner agent — planning-only. I will not execute any of the steps above.*