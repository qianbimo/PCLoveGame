using PCLoveGame.Data;

namespace PCLoveGame.Core
{
    public static class GameConstants
    {
        public const string SaveFileName = "pclovegame-save.json";
        public const int MinimumFavorability = 0;
        public const int MaximumFavorability = 100;

        public static readonly ResolutionOption[] SupportedResolutions =
        {
            new ResolutionOption(1280, 720),
            new ResolutionOption(1366, 768),
            new ResolutionOption(1600, 900),
            new ResolutionOption(1920, 1080)
        };
    }
}
