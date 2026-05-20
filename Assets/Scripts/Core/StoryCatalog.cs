using PCLoveGame.Data;

namespace PCLoveGame.Core
{
    public static class StoryCatalog
    {
        public static readonly LevelDefinition[] Levels =
        {
            new LevelDefinition(LevelType.Game1, "Game 1：21 点对战", "三局制 21 点，拉开故事序幕。"),
            new LevelDefinition(LevelType.Game2, "Game 2：校园邀约", "在三个邀约中选出真正打动女主的一项。"),
            new LevelDefinition(LevelType.Game3, "Game 3：情侣观影答题", "记住那场特别的电影与时间。"),
            new LevelDefinition(LevelType.Game4, "Game 4：恋爱破碎", "再努力也难以避免的转折时刻。"),
            new LevelDefinition(LevelType.Game5, "Game 5：合照拼图", "拼回回忆，也拼回重新靠近的勇气。"),
            new LevelDefinition(LevelType.Ending, "Ending：生日和好", "在温柔的结局里，为这段关系按下新的开始。")
        };

        public static ChoiceOptionData[] GetCampusChoices()
        {
            return new[]
            {
                new ChoiceOptionData("A", "一起去看电影吧", "女主轻轻摇头：算了吧。", 0, false),
                new ChoiceOptionData("B", "一起去逛街吧", "女主轻轻摇头：算了吧。", 0, false),
                new ChoiceOptionData("C", "一起去上网", "女主眼神一亮：走啊！", 0, true)
            };
        }

        public static ChoiceOptionData[] GetBreakupChoices()
        {
            return new[]
            {
                new ChoiceOptionData("A", "不理她，当作没看到", "女主生气扇巴掌：分手！", -10, true),
                new ChoiceOptionData("B", "回复她，并推荐蜡笔小新", "女主暴怒扇巴掌：分手！", -50, true),
                new ChoiceOptionData("C", "告诉女朋友并解释", "女主失望气愤：分手！", -20, true)
            };
        }

        public static PuzzlePieceDefinition[] GetPuzzlePieces()
        {
            return new[]
            {
                new PuzzlePieceDefinition("P1", "回忆一", 70f),
                new PuzzlePieceDefinition("P2", "回忆二", 70f),
                new PuzzlePieceDefinition("P3", "回忆三", 70f),
                new PuzzlePieceDefinition("P4", "回忆四", 70f)
            };
        }

        public static string GetEndingSummary()
        {
            return "女主捧着生日蛋糕重新出现，画面定格在相视一笑的瞬间。";
        }
    }
}
