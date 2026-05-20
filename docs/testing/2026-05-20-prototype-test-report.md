# PCLoveGame 原型测试报告

## 1. 测试目标

验证当前 Unity 原型是否已覆盖需求分析中的核心流程，包括：

- 主菜单、设置、关卡选择、继续游戏入口
- 五个关卡的主要玩法逻辑
- 好感度与关卡推进
- 本地存档与设置持久化
- 核心纯逻辑规则是否可自动验证

## 2. 测试环境

- 项目路径：`E:\Project\unity\game\PCLoveGame`
- Unity 版本：`2022.3.17f1c1`
- 自动化逻辑验证：`.NET 8.0.405`
- Unity 导入验证方式：复制临时工程到 `Temp/UnityBatchProject` 后批处理导入

## 3. 执行记录

### 3.1 纯逻辑自动验证

执行方式：

```powershell
dotnet run --project Temp\Verification\Verification.csproj
```

结果：

- [x] `GameSession.StartNewGame()` 重置关卡和好感度逻辑正确
- [x] `GameSession` 好感度上下限钳制正确
- [x] `GameSession.CompleteLevel()` 解锁下一关与结局逻辑正确
- [x] `MovieQuizEvaluator` 支持标准答案和 `Trim` 后输入判定
- [x] `PuzzleProgressCalculator` 的进度与好感度恢复规则正确

验证输出：

```text
Verification passed.
```

### 3.2 Unity 批处理导入与编译验证

执行方式：

- 将项目复制到 `Temp/UnityBatchProject`
- 使用 Unity `-batchmode -quit` 打开临时工程并完成首轮导入与脚本编译

结果：

- [x] 运行时代码程序集 `PCLoveGame.Runtime` 成功参与 Unity 编译
- [x] 编辑器测试程序集 `PCLoveGame.EditModeTests` 成功参与 Unity 编译
- [x] Unity 批处理最终正常退出，日志包含 `Exiting batchmode successfully now!`

说明：

- 本次验证重点确认“工程可被 Unity 正常导入并完成脚本编译”
- 由于当前环境中原始项目存在打开中的 Unity 实例，批处理验证在临时复制工程上完成

## 4. 需求实现勾选

- [x] 主菜单支持开始游戏、继续游戏、关卡选择、设置、退出
- [x] 关卡选择支持按解锁状态进入关卡
- [x] 设置界面支持背景音乐、音效、全屏/窗口、分辨率切换与保存
- [x] Game 1 已实现三局制 21 点基础玩法、抽牌、停牌与胜场结算
- [x] Game 2 已实现校园邀约三选一与正确选项推进
- [x] Game 3 已实现电影名称与观影时间输入判定及差异化反馈
- [x] Game 4 已实现三种选择分支与好感度扣减
- [x] Game 5 已实现拼图拖拽、吸附、回弹、进度与好感度恢复
- [x] 已实现结局界面与通关后回到主菜单/重开/关卡选择
- [x] 已实现本地 JSON 存档和基础设置持久化
- [x] 已补充 Unity EditMode 测试代码

## 5. 当前实现说明

- 当前版本为“代码驱动的可运行原型”，重点在于玩法骨架、状态管理和流程打通
- 视觉资源仍以程序化 UI 和占位文本为主，尚未替换为正式像素风美术资源
- 拼图、剧情、角色动画与 UI 均已预留可继续替换资源和扩展数据的结构

## 6. 建议的下一轮验证

- 在 Unity 编辑器中进行一次完整人工走关
- 替换正式像素资源后复测 UI 对齐与拖拽手感
- 增加剧情数据与角色动画资源后补做体验回归测试

