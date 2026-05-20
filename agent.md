# agent.md

## 项目摘要

PCLoveGame 是一个基于 Unity `2022.3.17f1c1` 的 PC 恋爱游戏项目。当前仓库仍处于非常早期的骨架阶段，现有内容主要是 Unity 默认工程、一个示例场景、包依赖配置和项目设置，还没有真正落地的游戏逻辑。

## 当前已存在内容

- 一个已注册的场景：`Assets/Scenes/SampleScene.unity`
- 默认的 Unity 相机场景
- Unity 2D 功能包
- TextMeshPro、UGUI 等 UI 相关包
- Timeline、Visual Scripting、Unity Test Framework

## 当前尚不存在内容

- `Assets/Scripts/`
- 对话系统
- 角色交互系统
- 存档/读档系统
- 自定义预制体
- 剧情数据结构
- 正式 UI 界面
- 自动化构建或 CI 配置

## 目录速览

- `Assets/`：游戏运行期资源与场景
- `Assets/Scenes/`：当前场景目录
- `Packages/manifest.json`：Unity 包依赖
- `ProjectSettings/`：引擎与编辑器配置

## 推荐的后续目录约定

- 运行时代码放在 `Assets/Scripts/`
- 按领域拆分脚本目录，如 `Core`、`Dialogue`、`UI`、`Data`、`SaveSystem`
- 预制体放在 `Assets/Prefabs/`
- 美术资源放在 `Assets/Art/`
- 音频资源放在 `Assets/Audio/`
- 剧情或对话数据放在 `Assets/Data/Dialogue/`

## 给 AI 的工作提示

- 每次 AI 生成、修改、重构代码前，必须先遵循 `docs/coding-standards.md` 中定义的代码格式规范
- 如果新增脚本、调整目录或补充模块结构，应优先与 `docs/coding-standards.md` 保持一致
- 将当前仓库视为早期脚手架，不要默认已存在完整玩法系统
- 在提出升级建议前先检查 `ProjectSettings/ProjectVersion.txt`
- 新增、移动、重命名资源时要连同 Unity `.meta` 文件一起处理
- 不要提交 `Library/`、`Temp/`、`Logs/`、`UserSettings/` 等生成目录
- 新增场景后，记得同步检查 `ProjectSettings/EditorBuildSettings.asset`
- 编写说明文档时，应如实描述项目仍处于原型起步阶段

## 适合优先实现的内容

- 创建 `Assets/Scripts/`
- 增加启动场景管理器
- 搭建标题界面
- 做一个对话框 UI 原型
- 定义简单的剧情数据格式
- 后续补充贡献指南和资源流程文档
