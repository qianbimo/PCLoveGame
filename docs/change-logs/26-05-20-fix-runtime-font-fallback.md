# Change Log

**Commit Message**: `fix(ui): 修复运行时字体加载`
**Branch**: `main`
**Date**: `2026-05-20`
**Author**: `qianbimo`

## Changed Files

- `Assets/Scripts/UI/UiFactory.cs` (modified)
- `docs/change-logs/26-05-20-fix-runtime-font-fallback.md` (added)

## Summary

- Replaced the direct `Arial.ttf` built-in font dependency with a safer fallback chain
- Added built-in font probing for `LegacyRuntime.ttf` and `Arial.ttf`
- Added OS font fallback to keep runtime UI available on newer Unity environments

## Notes

This fix addresses the runtime crash caused by `Arial.ttf` no longer being a valid built-in font in the current Unity environment.
