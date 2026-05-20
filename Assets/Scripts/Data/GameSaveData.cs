namespace PCLoveGame.Data
{
    [System.Serializable]
    public sealed class GameSaveData
    {
        public int CurrentLevel = (int)LevelType.Game1;
        public int UnlockedLevel = (int)LevelType.Game1;
        public int Favorability = 100;
        public bool IsGameCleared;
        public float MusicVolume = 0.8f;
        public float SfxVolume = 0.8f;
        public bool IsFullScreen = true;
        public int ResolutionWidth = 1280;
        public int ResolutionHeight = 720;
    }
}
