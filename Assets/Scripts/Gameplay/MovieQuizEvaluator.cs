namespace PCLoveGame.Gameplay
{
    public static class MovieQuizEvaluator
    {
        public const string ExpectedMovie = "如果声音不记得";
        public const string ExpectedTime = "2020年12月9号 19:10";

        public static MovieQuizResult Evaluate(string movieName, string watchTime)
        {
            string sanitizedMovieName = Sanitize(movieName);
            string sanitizedWatchTime = Sanitize(watchTime);

            bool isMovieCorrect = sanitizedMovieName == ExpectedMovie;
            bool isTimeCorrect = sanitizedWatchTime == ExpectedTime;

            if (isMovieCorrect && isTimeCorrect)
            {
                return MovieQuizResult.Success;
            }

            if (!isMovieCorrect && !isTimeCorrect)
            {
                return MovieQuizResult.BothIncorrect;
            }

            return isMovieCorrect ? MovieQuizResult.TimeIncorrect : MovieQuizResult.MovieIncorrect;
        }

        private static string Sanitize(string rawValue)
        {
            return string.IsNullOrWhiteSpace(rawValue) ? string.Empty : rawValue.Trim();
        }
    }
}
