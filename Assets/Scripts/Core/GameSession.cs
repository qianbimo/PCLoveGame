using PCLoveGame.Data;

namespace PCLoveGame.Core
{
    public sealed class GameSession
    {
        private readonly GameSaveData _saveData;

        public GameSession(GameSaveData saveData)
        {
            _saveData = saveData ?? new GameSaveData();
        }

        public GameSaveData SaveData => _saveData;

        public LevelType CurrentLevel => (LevelType)_saveData.CurrentLevel;

        public LevelType HighestUnlockedLevel => (LevelType)_saveData.UnlockedLevel;

        public int Favorability => _saveData.Favorability;

        public bool HasClearData => _saveData.IsGameCleared || _saveData.CurrentLevel > 0;

        public void StartNewGame()
        {
            _saveData.CurrentLevel = (int)LevelType.Game1;
            _saveData.UnlockedLevel = (int)LevelType.Game1;
            _saveData.Favorability = GameConstants.MaximumFavorability;
            _saveData.IsGameCleared = false;
        }

        public void SetCurrentLevel(LevelType levelType)
        {
            _saveData.CurrentLevel = (int)levelType;
        }

        public void CompleteLevel(LevelType levelType)
        {
            if (levelType == LevelType.Game5)
            {
                _saveData.CurrentLevel = (int)LevelType.Ending;
                _saveData.UnlockedLevel = (int)LevelType.Ending;
                _saveData.IsGameCleared = true;
                return;
            }

            int nextLevelValue = (int)levelType + 1;

            if (nextLevelValue > _saveData.UnlockedLevel)
            {
                _saveData.UnlockedLevel = nextLevelValue;
            }

            _saveData.CurrentLevel = nextLevelValue;
        }

        public void SetFavorability(int favorability)
        {
            _saveData.Favorability = ClampFavorability(favorability);
        }

        public void ChangeFavorability(int deltaValue)
        {
            SetFavorability(_saveData.Favorability + deltaValue);
        }

        public void UpdateAudioSettings(float musicVolume, float sfxVolume)
        {
            _saveData.MusicVolume = musicVolume;
            _saveData.SfxVolume = sfxVolume;
        }

        public void UpdateDisplaySettings(bool isFullScreen, int width, int height)
        {
            _saveData.IsFullScreen = isFullScreen;
            _saveData.ResolutionWidth = width;
            _saveData.ResolutionHeight = height;
        }

        private static int ClampFavorability(int favorability)
        {
            if (favorability < GameConstants.MinimumFavorability)
            {
                return GameConstants.MinimumFavorability;
            }

            if (favorability > GameConstants.MaximumFavorability)
            {
                return GameConstants.MaximumFavorability;
            }

            return favorability;
        }
    }
}
