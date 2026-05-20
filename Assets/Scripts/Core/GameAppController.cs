using System.Collections.Generic;
using PCLoveGame.Data;
using PCLoveGame.Gameplay;
using PCLoveGame.SaveSystem;
using PCLoveGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PCLoveGame.Core
{
    public sealed class GameAppController : MonoBehaviour
    {
        private readonly List<GameObject> _screenObjects = new List<GameObject>();
        private readonly List<DraggablePuzzlePieceView> _puzzlePieces = new List<DraggablePuzzlePieceView>();

        private SaveService _saveService;
        private GameSession _session;
        private Canvas _canvas;
        private RectTransform _contentRoot;
        private Text _titleText;
        private Text _subtitleText;
        private Text _statusText;
        private Text _favorabilityText;
        private BlackjackMatch _blackjackMatch;
        private string _game2Feedback;
        private string _game3Feedback;
        private string _game4Feedback;
        private bool _game2Completed;
        private bool _game3Completed;
        private bool _game4Completed;
        private bool _game5Completed;
        private int _placedPuzzleCount;
        private InputField _movieInputField;
        private InputField _timeInputField;
        private ResolutionOption _selectedResolution;
        private LevelType? _activeLevel;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            UiFactory.EnsureEventSystem();
            _saveService = new SaveService();
            _session = new GameSession(_saveService.Load());
            _selectedResolution = new ResolutionOption(_session.SaveData.ResolutionWidth, _session.SaveData.ResolutionHeight);
            ApplyDisplaySettings();
            BuildShell();
            ShowMainMenu();
        }

        private void BuildShell()
        {
            _canvas = UiFactory.CreateCanvas(transform);
            Image background = UiFactory.CreatePanel(_canvas.transform, "Background", new Color(0.08f, 0.09f, 0.12f, 1f), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            background.raycastTarget = false;

            Image header = UiFactory.CreatePanel(_canvas.transform, "Header", new Color(0.12f, 0.14f, 0.18f, 0.95f), new Vector2(0f, 0.78f), new Vector2(1f, 1f), new Vector2(20f, -20f), new Vector2(-20f, -20f));
            _titleText = UiFactory.CreateText(header.transform, "PCLoveGame", 30, TextAnchor.UpperLeft, Color.white);
            _subtitleText = UiFactory.CreateText(header.transform, "像素风互动闯关原型", 18, TextAnchor.MiddleLeft, new Color(0.86f, 0.89f, 0.95f, 1f));
            _favorabilityText = UiFactory.CreateText(header.transform, "好感度：100/100", 20, TextAnchor.UpperRight, new Color(1f, 0.82f, 0.86f, 1f));
            _statusText = UiFactory.CreateText(header.transform, "准备开始。", 18, TextAnchor.LowerLeft, new Color(0.84f, 0.91f, 1f, 1f));

            _titleText.rectTransform.anchorMin = new Vector2(0f, 0.45f);
            _titleText.rectTransform.anchorMax = new Vector2(0.6f, 1f);
            _titleText.rectTransform.offsetMin = new Vector2(20f, 0f);
            _titleText.rectTransform.offsetMax = new Vector2(-20f, -10f);

            _subtitleText.rectTransform.anchorMin = new Vector2(0f, 0f);
            _subtitleText.rectTransform.anchorMax = new Vector2(0.7f, 0.45f);
            _subtitleText.rectTransform.offsetMin = new Vector2(20f, 12f);
            _subtitleText.rectTransform.offsetMax = new Vector2(-20f, -8f);

            _favorabilityText.rectTransform.anchorMin = new Vector2(0.68f, 0.45f);
            _favorabilityText.rectTransform.anchorMax = new Vector2(1f, 1f);
            _favorabilityText.rectTransform.offsetMin = new Vector2(0f, 0f);
            _favorabilityText.rectTransform.offsetMax = new Vector2(-20f, -10f);

            _statusText.rectTransform.anchorMin = new Vector2(0.55f, 0f);
            _statusText.rectTransform.anchorMax = new Vector2(1f, 0.45f);
            _statusText.rectTransform.offsetMin = new Vector2(0f, 12f);
            _statusText.rectTransform.offsetMax = new Vector2(-20f, -8f);

            Image contentPanel = UiFactory.CreatePanel(_canvas.transform, "ContentRoot", new Color(0.13f, 0.15f, 0.2f, 0.92f), new Vector2(0f, 0f), new Vector2(1f, 0.78f), new Vector2(20f, 20f), new Vector2(-20f, -20f));
            _contentRoot = contentPanel.rectTransform;
        }

        private void ShowMainMenu()
        {
            ResetLevelState();
            ClearScreen();
            UpdateHeader("PCLoveGame", "主菜单", "选择一个入口开始这段故事。");

            RectTransform layoutRoot = CreateScreenLayout();

            CreateDescriptionCard(layoutRoot, "项目原型说明", "当前版本使用纯代码搭建 UI 与玩法骨架，可直接在 SampleScene 中运行。");
            CreateActionButton(layoutRoot, "开始游戏", StartNewGame);
            CreateActionButton(layoutRoot, "继续游戏", ContinueGame);
            CreateActionButton(layoutRoot, "关卡选择", ShowLevelSelect);
            CreateActionButton(layoutRoot, "设置", ShowSettings);
            CreateActionButton(layoutRoot, "退出游戏", ExitGame);
        }

        private void ShowLevelSelect()
        {
            ClearScreen();
            UpdateHeader("关卡选择", "选择已解锁的关卡", "未解锁关卡无法进入。");

            RectTransform layoutRoot = CreateScreenLayout();

            foreach (LevelDefinition definition in StoryCatalog.Levels)
            {
                bool isUnlocked = definition.LevelType <= _session.HighestUnlockedLevel;
                string buttonLabel = isUnlocked ? $"{definition.Title} - 进入" : $"{definition.Title} - 未解锁";
                CreateActionButton(layoutRoot, buttonLabel, () =>
                {
                    if (isUnlocked)
                    {
                        _session.SetCurrentLevel(definition.LevelType);
                        SaveProgress();
                        ShowCurrentLevel();
                    }
                }, isUnlocked);
            }

            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void ShowSettings()
        {
            ClearScreen();
            UpdateHeader("设置", "基础运行设置", "调整后会立即应用并写入本地存档。");

            RectTransform layoutRoot = CreateScreenLayout();
            CreateDescriptionCard(layoutRoot, "背景音乐", $"当前音量：{_session.SaveData.MusicVolume:0.0}");
            CreateDualButtonRow(layoutRoot, "降低背景音乐", () => AdjustMusicVolume(-0.1f), "提高背景音乐", () => AdjustMusicVolume(0.1f));

            CreateDescriptionCard(layoutRoot, "音效音量", $"当前音量：{_session.SaveData.SfxVolume:0.0}");
            CreateDualButtonRow(layoutRoot, "降低音效", () => AdjustSfxVolume(-0.1f), "提高音效", () => AdjustSfxVolume(0.1f));

            CreateDescriptionCard(layoutRoot, "显示模式", _session.SaveData.IsFullScreen ? "当前：全屏" : "当前：窗口");
            CreateActionButton(layoutRoot, "切换全屏 / 窗口", ToggleFullScreen);

            CreateDescriptionCard(layoutRoot, "分辨率", $"当前：{_selectedResolution}");
            CreateActionButton(layoutRoot, "切换分辨率", CycleResolution);
            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void StartNewGame()
        {
            _session.StartNewGame();
            ResetLevelState();
            SaveProgress();
            ShowCurrentLevel();
        }

        private void ContinueGame()
        {
            ShowCurrentLevel();
        }

        private void ShowCurrentLevel()
        {
            PrepareStateForLevel(_session.CurrentLevel);

            switch (_session.CurrentLevel)
            {
                case LevelType.Game1:
                    ShowBlackjackLevel();
                    break;
                case LevelType.Game2:
                    ShowCampusLevel();
                    break;
                case LevelType.Game3:
                    ShowMovieQuizLevel();
                    break;
                case LevelType.Game4:
                    ShowBreakupLevel();
                    break;
                case LevelType.Game5:
                    ShowPuzzleLevel();
                    break;
                case LevelType.Ending:
                    ShowEndingLevel();
                    break;
                default:
                    ShowMainMenu();
                    break;
            }
        }

        private void ShowBlackjackLevel()
        {
            ClearScreen();
            UpdateHeader("Game 1：21 点对战", "三局制小游戏", _blackjackMatch.RoundSummary);

            RectTransform layoutRoot = CreateScreenLayout();
            CreateDescriptionCard(layoutRoot, "当前局数", $"{Mathf.Min(_blackjackMatch.CurrentRound, 3)}/3");
            CreateDescriptionCard(layoutRoot, "胜场信息", $"男主 {_blackjackMatch.PlayerWins} : {_blackjackMatch.OpponentWins} 女主");
            CreateDescriptionCard(layoutRoot, "牌面信息", $"男主：{_blackjackMatch.PlayerTotal}    女主：{_blackjackMatch.OpponentTotal}");

            if (_blackjackMatch.IsMatchCompleted)
            {
                string result = _blackjackMatch.PlayerWins >= _blackjackMatch.OpponentWins ? "男主成功带动节奏，进入下一关。" : "女主略胜一筹，但故事依然继续。";
                CreateDescriptionCard(layoutRoot, "对局结算", result);
                CreateActionButton(layoutRoot, "进入下一关", () => CompleteLevel(LevelType.Game1));
                return;
            }

            if (_blackjackMatch.IsRoundResolved)
            {
                CreateActionButton(layoutRoot, "进入下一局", AdvanceBlackjackRound);
                return;
            }

            CreateDualButtonRow(layoutRoot, "抽牌", DrawBlackjackCard, "停牌", StandBlackjack);
            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void ShowCampusLevel()
        {
            ClearScreen();
            UpdateHeader("Game 2：校园邀约", "选项推动剧情", _game2Completed ? "网吧剧情播放完成，可以进入下一关。" : (_game2Feedback ?? "女主独自站在校园里，等你给出一个真正合适的邀约。"));

            RectTransform layoutRoot = CreateScreenLayout();

            if (_game2Completed)
            {
                CreateDescriptionCard(layoutRoot, "网吧剧情", "男女主并排坐在电脑前，女主偷偷侧头看向男主，随后害羞低头。");
                CreateActionButton(layoutRoot, "进入下一关", () => CompleteLevel(LevelType.Game2));
                return;
            }

            CreateDescriptionCard(layoutRoot, "女主台词", "今天自己来学校，好无聊。");

            foreach (ChoiceOptionData choice in StoryCatalog.GetCampusChoices())
            {
                CreateActionButton(layoutRoot, choice.Label, () => ResolveCampusChoice(choice));
            }

            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void ShowMovieQuizLevel()
        {
            ClearScreen();
            UpdateHeader("Game 3：观影答题", "输入电影与时间", _game3Feedback ?? "休闲约会时刻来临，浪漫影院约会之行即将开始。");

            RectTransform layoutRoot = CreateScreenLayout();

            if (_game3Completed)
            {
                CreateDescriptionCard(layoutRoot, "影院牵手剧情", "男主主动伸手牵住女主，女主脸红却没有挣脱，画面停留在并肩而坐的温柔瞬间。");
                CreateActionButton(layoutRoot, "进入下一关", () => CompleteLevel(LevelType.Game3));
                return;
            }

            CreateDescriptionCard(layoutRoot, "题目要求", "请输入电影名称与观影时间，两项都正确才可通关。");
            _movieInputField = UiFactory.CreateInputField(layoutRoot, "电影名称：如果声音不记得");
            _timeInputField = UiFactory.CreateInputField(layoutRoot, "观影时间：2020年12月9号 19:10");

            CreateActionButton(layoutRoot, "提交答案", ResolveMovieQuiz);
            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void ShowBreakupLevel()
        {
            ClearScreen();
            UpdateHeader("Game 4：恋爱破碎", "注定破碎的转折点", _game4Feedback ?? "深夜 23:40，微信弹窗出现：睡不着啊。");

            RectTransform layoutRoot = CreateScreenLayout();

            if (_game4Completed)
            {
                CreateDescriptionCard(layoutRoot, "剧情结果", "无论你如何选择，这段感情都已经出现了裂痕。");
                CreateActionButton(layoutRoot, "进入下一关", () => CompleteLevel(LevelType.Game4));
                return;
            }

            CreateDescriptionCard(layoutRoot, "当前好感度", $"{_session.Favorability}/100");

            foreach (ChoiceOptionData choice in StoryCatalog.GetBreakupChoices())
            {
                CreateActionButton(layoutRoot, choice.Label, () => ResolveBreakupChoice(choice));
            }

            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void ShowPuzzleLevel()
        {
            ClearScreen();
            UpdateHeader("Game 5：合照拼图", "拖拽碎片完成和好", _game5Completed ? "拼图完成，回忆重新拼合。" : "拖拽下方碎片到上方对应位置，逐步恢复好感度。");

            _session.SetFavorability(PuzzleProgressCalculator.CalculateFavorability(_placedPuzzleCount, StoryCatalog.GetPuzzlePieces().Length));
            SaveProgress();
            RefreshFavorabilityLabel();

            RectTransform layoutRoot = CreateScreenLayout();

            if (_game5Completed)
            {
                CreateDescriptionCard(layoutRoot, "结局前夜", "完整合照亮起，女主轻声说：其实我一直都在等你。");
                CreateActionButton(layoutRoot, "进入结局", () => CompleteLevel(LevelType.Game5));
                return;
            }

            CreateDescriptionCard(layoutRoot, "拼图进度", $"{_placedPuzzleCount}/{StoryCatalog.GetPuzzlePieces().Length} 已完成");
            CreatePuzzleBoard(layoutRoot);
            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private void ShowEndingLevel()
        {
            ClearScreen();
            UpdateHeader("Ending：生日和好", "故事完成", StoryCatalog.GetEndingSummary());

            RectTransform layoutRoot = CreateScreenLayout();
            CreateDescriptionCard(layoutRoot, "Happy Birthday", "女主捧着生日蛋糕重新登场，画面停留在相视一笑的瞬间。");
            CreateDescriptionCard(layoutRoot, "通关状态", _session.SaveData.IsGameCleared ? "本地记录：已完成整段流程。" : "等待通关记录写入。");
            CreateActionButton(layoutRoot, "重新开始", StartNewGame);
            CreateActionButton(layoutRoot, "关卡选择", ShowLevelSelect);
            CreateActionButton(layoutRoot, "返回主菜单", ShowMainMenu);
        }

        private RectTransform CreateScreenLayout()
        {
            GameObject layoutObject = new GameObject("LayoutRoot", typeof(RectTransform));
            layoutObject.transform.SetParent(_contentRoot, false);
            RectTransform rectTransform = layoutObject.GetComponent<RectTransform>();
            UiFactory.Stretch(rectTransform);

            ContentSizeFitter sizeFitter = UiFactory.CreateSizeFitter(layoutObject.transform);
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layoutGroup = UiFactory.CreateVerticalLayout(layoutObject.transform, 12f, TextAnchor.UpperCenter);
            layoutGroup.padding = new RectOffset(22, 22, 22, 22);

            _screenObjects.Add(layoutObject);
            return rectTransform;
        }

        private void CreateDescriptionCard(Transform parent, string title, string content)
        {
            Image card = UiFactory.CreatePanel(parent, title, new Color(0.17f, 0.2f, 0.25f, 0.96f), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            LayoutElement layoutElement = card.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 88f;

            VerticalLayoutGroup layoutGroup = UiFactory.CreateVerticalLayout(card.transform, 6f, TextAnchor.UpperLeft);
            layoutGroup.padding = new RectOffset(14, 14, 10, 10);

            Text titleText = UiFactory.CreateText(card.transform, title, 20, TextAnchor.UpperLeft, Color.white);
            Text contentText = UiFactory.CreateText(card.transform, content, 18, TextAnchor.UpperLeft, new Color(0.9f, 0.92f, 0.98f, 1f));

            titleText.gameObject.AddComponent<LayoutElement>().preferredHeight = 28f;
            contentText.gameObject.AddComponent<LayoutElement>().preferredHeight = 42f;
        }

        private void CreateActionButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick, bool isInteractable = true)
        {
            Button button = UiFactory.CreateButton(parent, label, () => onClick?.Invoke());
            button.interactable = isInteractable;
        }

        private void CreateDualButtonRow(Transform parent, string leftLabel, UnityEngine.Events.UnityAction leftAction, string rightLabel, UnityEngine.Events.UnityAction rightAction)
        {
            GameObject rowObject = new GameObject("ButtonRow", typeof(RectTransform));
            rowObject.transform.SetParent(parent, false);
            HorizontalLayoutGroup layoutGroup = UiFactory.CreateHorizontalLayout(rowObject.transform, 12f, TextAnchor.MiddleCenter);
            layoutGroup.padding = new RectOffset(0, 0, 0, 0);

            LayoutElement rowLayout = rowObject.AddComponent<LayoutElement>();
            rowLayout.preferredHeight = 56f;

            CreateActionButton(rowObject.transform, leftLabel, leftAction);
            CreateActionButton(rowObject.transform, rightLabel, rightAction);
        }

        private void ResolveCampusChoice(ChoiceOptionData choice)
        {
            _game2Feedback = choice.ResultText;

            if (choice.IsCorrect)
            {
                _game2Completed = true;
            }

            ShowCampusLevel();
        }

        private void ResolveMovieQuiz()
        {
            MovieQuizResult result = MovieQuizEvaluator.Evaluate(_movieInputField.text, _timeInputField.text);

            switch (result)
            {
                case MovieQuizResult.Success:
                    _game3Feedback = "女主甜甜微笑：没想到你全都记得，太用心啦。影院牵手剧情已触发。";
                    _game3Completed = true;
                    ShowMovieQuizLevel();
                    return;
                case MovieQuizResult.MovieIncorrect:
                    _game3Feedback = "女主歪头失望：这你都不记得？";
                    break;
                case MovieQuizResult.TimeIncorrect:
                    _game3Feedback = "女主抿嘴摇头：时间都能记错，你是何居心？！";
                    break;
                case MovieQuizResult.BothIncorrect:
                    _game3Feedback = "女主低头失落：居然全都忘记了？？？？";
                    break;
            }

            ShowMovieQuizLevel();
        }

        private void ResolveBreakupChoice(ChoiceOptionData choice)
        {
            _session.ChangeFavorability(choice.FavorabilityChange);
            _game4Feedback = choice.ResultText + $" 好感度变化：{choice.FavorabilityChange}";
            _game4Completed = true;
            SaveProgress();
            ShowBreakupLevel();
        }

        private void CreatePuzzleBoard(Transform parent)
        {
            GameObject boardRoot = new GameObject("PuzzleRoot", typeof(RectTransform));
            boardRoot.transform.SetParent(parent, false);
            LayoutElement boardLayout = boardRoot.AddComponent<LayoutElement>();
            boardLayout.preferredHeight = 420f;

            Image targetsPanel = UiFactory.CreatePanel(boardRoot.transform, "Targets", new Color(0.11f, 0.12f, 0.16f, 1f), new Vector2(0.05f, 0.48f), new Vector2(0.95f, 0.95f), Vector2.zero, Vector2.zero);
            Image piecesPanel = UiFactory.CreatePanel(boardRoot.transform, "Pieces", new Color(0.1f, 0.11f, 0.15f, 1f), new Vector2(0.05f, 0.05f), new Vector2(0.95f, 0.4f), Vector2.zero, Vector2.zero);

            PuzzlePieceDefinition[] definitions = StoryCatalog.GetPuzzlePieces();
            _puzzlePieces.Clear();

            for (int index = 0; index < definitions.Length; index++)
            {
                RectTransform target = CreatePuzzleTarget(targetsPanel.transform, definitions[index].Label, index, definitions.Length);
                DraggablePuzzlePieceView pieceView = CreatePuzzlePiece(piecesPanel.transform, definitions[index], target, index, definitions.Length);
                _puzzlePieces.Add(pieceView);
            }
        }

        private RectTransform CreatePuzzleTarget(Transform parent, string label, int index, int totalCount)
        {
            Image target = UiFactory.CreatePanel(parent, $"Target_{index}", new Color(0.27f, 0.3f, 0.36f, 1f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
            RectTransform rectTransform = target.rectTransform;
            rectTransform.sizeDelta = new Vector2(150f, 90f);
            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(110f + (index * 180f), 0f);

            Text text = UiFactory.CreateText(target.transform, label, 18, TextAnchor.MiddleCenter, new Color(0.92f, 0.96f, 1f, 1f));
            UiFactory.Stretch(text.rectTransform);
            return rectTransform;
        }

        private DraggablePuzzlePieceView CreatePuzzlePiece(Transform parent, PuzzlePieceDefinition definition, RectTransform targetRect, int index, int totalCount)
        {
            GameObject pieceObject = new GameObject(definition.PieceId, typeof(RectTransform), typeof(Image), typeof(DraggablePuzzlePieceView));
            pieceObject.transform.SetParent(parent, false);

            Image image = pieceObject.GetComponent<Image>();
            image.color = new Color(0.38f, 0.54f, 0.67f, 1f);

            RectTransform rectTransform = pieceObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150f, 90f);
            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);

            Text label = UiFactory.CreateText(pieceObject.transform, definition.Label, 18, TextAnchor.MiddleCenter, Color.white);
            UiFactory.Stretch(label.rectTransform);

            DraggablePuzzlePieceView pieceView = pieceObject.GetComponent<DraggablePuzzlePieceView>();
            pieceView.Initialize(definition.PieceId, definition.Label, _canvas, targetRect, definition.SnapDistance, HandlePuzzlePiecePlaced);
            pieceView.SetInitialPosition(new Vector2(110f + (index * 180f), 0f));
            return pieceView;
        }

        private void HandlePuzzlePiecePlaced(DraggablePuzzlePieceView pieceView)
        {
            _placedPuzzleCount += 1;
            _session.SetFavorability(PuzzleProgressCalculator.CalculateFavorability(_placedPuzzleCount, StoryCatalog.GetPuzzlePieces().Length));
            SaveProgress();

            if (_placedPuzzleCount >= StoryCatalog.GetPuzzlePieces().Length)
            {
                _session.SetFavorability(GameConstants.MaximumFavorability);
                _game5Completed = true;
                SaveProgress();
                ShowPuzzleLevel();
                return;
            }

            RefreshFavorabilityLabel();
            UpdateHeader("Game 5：合照拼图", "拖拽碎片完成和好", $"已正确放置 {_placedPuzzleCount} 个碎片。");
        }

        private void DrawBlackjackCard()
        {
            _blackjackMatch.DrawCard();
            ShowBlackjackLevel();
        }

        private void StandBlackjack()
        {
            _blackjackMatch.Stand();
            ShowBlackjackLevel();
        }

        private void AdvanceBlackjackRound()
        {
            _blackjackMatch.AdvanceRound();
            ShowBlackjackLevel();
        }

        private void CompleteLevel(LevelType levelType)
        {
            if (levelType == LevelType.Game4)
            {
                _session.SetFavorability(30);
            }

            _session.CompleteLevel(levelType);
            SaveProgress();
            ShowCurrentLevel();
        }

        private void AdjustMusicVolume(float deltaValue)
        {
            float nextValue = Mathf.Clamp01(_session.SaveData.MusicVolume + deltaValue);
            _session.UpdateAudioSettings(nextValue, _session.SaveData.SfxVolume);
            SaveProgress();
            ShowSettings();
        }

        private void AdjustSfxVolume(float deltaValue)
        {
            float nextValue = Mathf.Clamp01(_session.SaveData.SfxVolume + deltaValue);
            _session.UpdateAudioSettings(_session.SaveData.MusicVolume, nextValue);
            SaveProgress();
            ShowSettings();
        }

        private void ToggleFullScreen()
        {
            bool nextState = !_session.SaveData.IsFullScreen;
            _session.UpdateDisplaySettings(nextState, _selectedResolution.Width, _selectedResolution.Height);
            ApplyDisplaySettings();
            SaveProgress();
            ShowSettings();
        }

        private void CycleResolution()
        {
            ResolutionOption[] resolutions = GameConstants.SupportedResolutions;
            int currentIndex = 0;

            for (int index = 0; index < resolutions.Length; index++)
            {
                if (resolutions[index].Width == _selectedResolution.Width && resolutions[index].Height == _selectedResolution.Height)
                {
                    currentIndex = index;
                    break;
                }
            }

            int nextIndex = (currentIndex + 1) % resolutions.Length;
            _selectedResolution = resolutions[nextIndex];
            _session.UpdateDisplaySettings(_session.SaveData.IsFullScreen, _selectedResolution.Width, _selectedResolution.Height);
            ApplyDisplaySettings();
            SaveProgress();
            ShowSettings();
        }

        private void ApplyDisplaySettings()
        {
            Screen.fullScreen = _session.SaveData.IsFullScreen;
            Screen.SetResolution(_selectedResolution.Width, _selectedResolution.Height, _session.SaveData.IsFullScreen);
        }

        private void SaveProgress()
        {
            RefreshFavorabilityLabel();
            _saveService.Save(_session.SaveData);
        }

        private void RefreshFavorabilityLabel()
        {
            if (_favorabilityText != null)
            {
                _favorabilityText.text = $"好感度：{_session.Favorability}/100";
            }
        }

        private void UpdateHeader(string title, string subtitle, string status)
        {
            _titleText.text = title;
            _subtitleText.text = subtitle;
            _statusText.text = status;
            RefreshFavorabilityLabel();
        }

        private void ClearScreen()
        {
            foreach (GameObject screenObject in _screenObjects)
            {
                if (screenObject != null)
                {
                    Destroy(screenObject);
                }
            }

            _screenObjects.Clear();
        }

        private void ResetLevelState()
        {
            _blackjackMatch = null;
            _game2Feedback = null;
            _game3Feedback = null;
            _game4Feedback = null;
            _game2Completed = false;
            _game3Completed = false;
            _game4Completed = false;
            _game5Completed = false;
            _placedPuzzleCount = 0;
            _activeLevel = null;
        }

        private void PrepareStateForLevel(LevelType levelType)
        {
            if (_activeLevel == levelType)
            {
                return;
            }

            _activeLevel = levelType;

            switch (levelType)
            {
                case LevelType.Game1:
                    _blackjackMatch = new BlackjackMatch();
                    break;
                case LevelType.Game2:
                    _game2Feedback = null;
                    _game2Completed = false;
                    break;
                case LevelType.Game3:
                    _game3Feedback = null;
                    _game3Completed = false;
                    break;
                case LevelType.Game4:
                    _game4Feedback = null;
                    _game4Completed = false;
                    break;
                case LevelType.Game5:
                    _placedPuzzleCount = 0;
                    _game5Completed = false;
                    _session.SetFavorability(30);
                    break;
            }
        }

        private static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
