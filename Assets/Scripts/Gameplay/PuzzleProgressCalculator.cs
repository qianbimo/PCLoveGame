namespace PCLoveGame.Gameplay
{
    public static class PuzzleProgressCalculator
    {
        private const int InitialFavorability = 30;
        private const int FinalFavorability = 100;

        public static int CalculateFavorability(int placedPieces, int totalPieces)
        {
            if (totalPieces <= 0)
            {
                return InitialFavorability;
            }

            if (placedPieces >= totalPieces)
            {
                return FinalFavorability;
            }

            int gainRange = FinalFavorability - InitialFavorability - 10;
            int stepValue = gainRange / totalPieces;

            return InitialFavorability + (stepValue * placedPieces);
        }

        public static float CalculateProgress(int placedPieces, int totalPieces)
        {
            if (totalPieces <= 0)
            {
                return 0f;
            }

            return (float)placedPieces / totalPieces;
        }
    }
}
