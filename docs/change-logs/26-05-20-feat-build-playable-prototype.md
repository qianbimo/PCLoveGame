# Change Log

**Commit Message**: `feat(game): 完成可运行原型开发`
**Branch**: `main`
**Date**: `2026-05-20`
**Author**: `qianbimo`

## Changed Files

- `Assets/Scripts/Bootstrap/GameBootstrap.cs` (added)
- `Assets/Scripts/Core/` (added)
- `Assets/Scripts/Data/` (added)
- `Assets/Scripts/Gameplay/` (added)
- `Assets/Scripts/SaveSystem/` (added)
- `Assets/Scripts/UI/` (added)
- `Assets/Scripts/PCLoveGame.Runtime.asmdef` (added)
- `Assets/Tests/EditMode/` (added)
- `docs/testing/2026-05-20-prototype-test-report.md` (added)
- `docs/change-logs/26-05-20-feat-build-playable-prototype.md` (added)

## Summary

- Built a code-driven playable prototype that runs in the current Unity project without depending on external art assets
- Implemented main menu, settings, level select, five level flows, favorability updates, ending flow, and JSON save persistence
- Added edit-mode test coverage, external logic verification, and a test report documenting completed checks

## Notes

The prototype focuses on shipping a runnable gameplay framework first. Visual presentation is intentionally placeholder-driven so the project can iterate on final pixel-art assets later without rewriting the core flow.
