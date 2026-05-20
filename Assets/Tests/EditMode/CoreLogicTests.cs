using NUnit.Framework;
using PCLoveGame.Core;
using PCLoveGame.Data;
using PCLoveGame.Gameplay;

namespace PCLoveGame.Tests.EditMode
{
    public sealed class CoreLogicTests
    {
        [Test]
        public void StartNewGame_ShouldResetCoreProgress()
        {
            GameSession session = new GameSession(new GameSaveData());

            session.StartNewGame();

            Assert.That(session.CurrentLevel, Is.EqualTo(LevelType.Game1));
            Assert.That(session.HighestUnlockedLevel, Is.EqualTo(LevelType.Game1));
            Assert.That(session.Favorability, Is.EqualTo(100));
        }

        [Test]
        public void ChangeFavorability_ShouldClampWithinRange()
        {
            GameSession session = new GameSession(new GameSaveData());

            session.ChangeFavorability(-120);
            Assert.That(session.Favorability, Is.EqualTo(0));

            session.SetFavorability(130);
            Assert.That(session.Favorability, Is.EqualTo(100));
        }

        [Test]
        public void CompleteLevel_ShouldUnlockNextLevel()
        {
            GameSession session = new GameSession(new GameSaveData());

            session.StartNewGame();
            session.CompleteLevel(LevelType.Game1);

            Assert.That(session.CurrentLevel, Is.EqualTo(LevelType.Game2));
            Assert.That(session.HighestUnlockedLevel, Is.EqualTo(LevelType.Game2));
        }

        [Test]
        public void EvaluateMovieQuiz_ShouldSupportTrimmedAnswers()
        {
            MovieQuizResult result = MovieQuizEvaluator.Evaluate(" 如果声音不记得 ", " 2020年12月9号 19:10 ");

            Assert.That(result, Is.EqualTo(MovieQuizResult.Success));
        }

        [Test]
        public void PuzzleProgress_ShouldMatchExpectedRatio()
        {
            Assert.That(PuzzleProgressCalculator.CalculateFavorability(0, 4), Is.EqualTo(30));
            Assert.That(PuzzleProgressCalculator.CalculateFavorability(4, 4), Is.EqualTo(100));
            Assert.That(PuzzleProgressCalculator.CalculateProgress(2, 4), Is.EqualTo(0.5f).Within(0.001f));
        }
    }
}
