# PCLoveGame

`PCLoveGame` 是一个基于 Unity 的 PC 恋爱游戏开源项目，目前处于项目初始化阶段。当前仓库主要提供 Unity 工程骨架、基础场景、依赖包配置和项目设置，方便后续继续补充剧情系统、UI、角色表现和存档等核心能力。

## 项目现状

- 项目阶段：原型启动期
- Unity 版本：`2022.3.17f1c1`
- 当前构建场景：`Assets/Scenes/SampleScene.unity`
- 当前内容：默认 Unity 2D 工程结构、基础包配置、项目设置

目前仓库中还没有正式的游戏逻辑脚本、对话系统、美术资源或完整 UI 流程，因此它更适合作为后续开发的起点，而不是可直接运行的完整游戏版本。

## 项目结构

```text
PCLoveGame/
|- Assets/
|  `- Scenes/
|     `- SampleScene.unity
|- Packages/
|  `- manifest.json
|- ProjectSettings/
|  `- ProjectVersion.txt
|- README.md
`- agent.md
```

## 目录说明

### `Assets/`

用于存放游戏内容相关资源，例如场景、脚本、预制体、图片、UI、音频等。目前仅包含一个默认示例场景。

### `Packages/`

用于管理 Unity 包依赖。当前项目启用了 2D 功能包，以及 TextMeshPro、Timeline、UGUI、测试框架和 Visual Scripting 等常用组件。

### `ProjectSettings/`

用于保存 Unity 项目的全局配置，包括编辑器版本、输入、画质、物理、音频和构建场景配置等。

## 已启用的关键依赖

- `com.unity.feature.2d`
- `com.unity.textmeshpro`
- `com.unity.timeline`
- `com.unity.ugui`
- `com.unity.test-framework`
- `com.unity.visualscripting`

这套配置适合作为 2D 叙事类项目的基础工程，后续可继续扩展剧情系统、角色立绘切换、UI 交互和存档逻辑。

## 如何打开项目

1. 安装 Unity Hub
2. 安装 Unity Editor `2022.3.17f1c1`
3. 通过 Unity Hub 添加当前目录
4. 打开项目后载入 `Assets/Scenes/SampleScene.unity`

## 后续开发建议

- 在 `Assets/Scripts/` 下建立代码目录
- 添加剧情数据和对话系统
- 设计主菜单和游戏内 UI 流程
- 用正式场景替换当前 `SampleScene`
- 设计存档/读档和剧情状态管理
- 在核心逻辑出现后补充测试

## 协作说明

- 不要将 Unity 生成目录提交到仓库
- 提交场景、资源和配置时尽量使用清晰的提交信息
- 随着项目成长，建议持续补充剧情系统、美术流程和资源规范文档

## 许可证

当前仓库还没有附带许可证文件。如果后续准备接受外部贡献，建议尽快补充开源许可证。
