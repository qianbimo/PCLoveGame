namespace PCLoveGame.Data
{
    public sealed class LevelDefinition
    {
        public LevelDefinition(LevelType levelType, string title, string subtitle)
        {
            LevelType = levelType;
            Title = title;
            Subtitle = subtitle;
        }

        public LevelType LevelType { get; }

        public string Title { get; }

        public string Subtitle { get; }
    }
}
