# PCLoveGame 代码格式规范

## 1. 适用范围

本规范适用于 `PCLoveGame` 项目中的以下内容：

- Unity C# 脚本
- 配置驱动代码与数据结构
- 编辑器工具脚本
- AI 生成或修改的项目代码

凡是 AI 在本仓库中新增、修改、重构的代码，默认都必须遵循本规范执行，除非项目维护者明确说明某个文件需要保留既有特殊写法。

## 2. 总体原则

- 优先保证可读性，其次再考虑技巧性写法
- 命名要表达业务含义，不使用模糊缩写
- 保持模块职责单一，避免一个脚本承担过多逻辑
- 优先与现有项目结构保持一致，不随意引入新风格
- 代码应服务于 Unity 2D 叙事游戏开发场景，避免过度设计

## 3. 目录与文件规范

- 运行时代码统一放在 `Assets/Scripts/`
- 按领域拆分子目录，例如：
  - `Assets/Scripts/Core/`
  - `Assets/Scripts/Dialogue/`
  - `Assets/Scripts/UI/`
  - `Assets/Scripts/Data/`
  - `Assets/Scripts/SaveSystem/`
  - `Assets/Scripts/Gameplay/`
- 一个 `.cs` 文件只定义一个主要公开类型，文件名必须与类名一致
- 编辑器脚本放在 `Editor/` 目录下，例如 `Assets/Scripts/Editor/`

## 4. 命名规范

### 4.1 类型命名

- 类、结构体、接口、枚举使用 `PascalCase`
- 接口必须以 `I` 开头，例如 `ISaveProvider`
- 枚举使用名词命名，例如 `DialogueState`、`LevelType`

### 4.2 成员命名

- 公有字段、属性、方法使用 `PascalCase`
- 私有字段使用 `_camelCase`
- 局部变量与方法参数使用 `camelCase`
- 常量使用 `PascalCase`
- 布尔变量优先使用可读前缀，例如 `isUnlocked`、`hasSaveData`、`canInteract`

### 4.3 禁止事项

- 禁止使用 `a`、`b`、`temp`、`data2` 这类无语义命名
- 禁止中英混杂命名
- 禁止拼音变量名，专有名词除外

## 5. C# 代码格式

- 使用 4 个空格缩进，不使用 Tab
- 左大括号单独换行，采用 Allman 风格
- `if`、`for`、`foreach`、`while`、`switch` 即使只有一行语句，也保留大括号
- 方法之间保留一个空行
- 字段定义、属性定义、Unity 生命周期方法、业务方法之间应分组排布
- 单行长度尽量控制在 120 个字符以内

示例：

```csharp
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _contentText;
    [SerializeField] private GameObject _dialoguePanel;

    private DialogueData _currentDialogue;

    public void Show(DialogueData dialogueData)
    {
        if (dialogueData == null)
        {
            return;
        }

        _currentDialogue = dialogueData;
        _dialoguePanel.SetActive(true);
        _contentText.text = dialogueData.Content;
    }
}
```

## 6. 类结构顺序

推荐按以下顺序组织类内容：

1. `using` 引用
2. 命名空间
3. 类注解和类型声明
4. 常量
5. 序列化字段
6. 私有字段
7. 公有属性
8. Unity 生命周期方法
9. 公有业务方法
10. 私有辅助方法

## 7. Unity 开发规范

- 能用 `[SerializeField] private` 的字段，不要直接暴露为 `public`
- `Update()` 中不要堆积复杂业务逻辑，优先拆分到独立方法
- 场景切换、存档、对话推进、好感度变化等核心逻辑应拆分到独立管理器
- 避免在多个脚本中复制同一段规则判断
- 对话文本、关卡配置、选项数据、拼图参数应优先配置化，不要全部硬编码
- 引用组件时优先在 Inspector 绑定；只有在明确合适时才在运行时 `GetComponent`
- 对可能为空的对象引用进行判空保护，尤其是 UI、配置和存档数据

## 8. 方法设计规范

- 一个方法只做一件主要事情
- 单个方法过长时应主动拆分，建议控制在 30 到 50 行内
- 方法名应使用“动作 + 对象”形式，例如 `LoadLevel`、`UpdateFavorability`、`PlayTransition`
- 避免编写包含多层嵌套的大方法，嵌套超过 3 层时应考虑重构

## 9. 注释规范

- 只在代码意图不明显时添加注释
- 注释解释“为什么这么做”，不要重复代码表面含义
- 对复杂状态流转、特殊判定、剧情规则可添加简短注释
- 不写无意义注释，例如“给变量赋值”“调用函数”

## 10. 错误处理规范

- 对存档读取失败、配置缺失、资源未绑定等情况提供安全降级
- 错误日志应说明上下文，例如模块名、对象名、失败原因
- 不要吞掉异常或错误状态
- 用户可恢复的问题优先给出提示，不直接让游戏进入不可控状态

## 11. AI 生成代码附加规则

- AI 每次生成代码前，应先检查项目现有结构和命名习惯
- AI 新增脚本时，必须遵循本规范中的命名、目录和格式要求
- AI 不得擅自引入与项目风格冲突的第三方架构或重型框架
- AI 不得为了省事把多个独立职责塞进同一个脚本
- AI 在修改现有代码时，优先延续原有模块边界，再做最小必要改动
- AI 若发现现有文件明显不符合本规范，应在修改时渐进式修正，而不是无关大规模重写

## 12. 推荐实践

- 优先使用枚举与数据对象表达状态，不滥用魔法字符串
- UI 文本显示与业务状态分离
- 存档数据结构与运行时控制逻辑分离
- 将可复用逻辑沉淀为明确的工具类或服务类
- 对剧情驱动项目，优先保证“配置驱动 + 清晰状态流转”

## 13. 待后续补充

- `.editorconfig`
- Unity 场景对象命名规范
- Prefab 命名规范
- ScriptableObject 配置文件命名规范
- UI 层级与 Canvas 拆分规范

