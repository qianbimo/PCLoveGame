using UnityEngine;

namespace PCLoveGame.Gameplay
{
    public sealed class BlackjackMatch
    {
        private const int MaxPoints = 21;
        private const int AiStandThreshold = 17;
        private const int MaxRounds = 3;

        private int _playerWins;
        private int _opponentWins;
        private int _currentRound;
        private int _playerTotal;
        private int _opponentTotal;
        private bool _isRoundResolved;
        private BlackjackRoundResult _roundResult;
        private string _roundSummary;

        public BlackjackMatch()
        {
            StartMatch();
        }

        public int CurrentRound => _currentRound;

        public int PlayerWins => _playerWins;

        public int OpponentWins => _opponentWins;

        public int PlayerTotal => _playerTotal;

        public int OpponentTotal => _opponentTotal;

        public bool IsRoundResolved => _isRoundResolved;

        public bool IsMatchCompleted => _currentRound > MaxRounds;

        public BlackjackRoundResult RoundResult => _roundResult;

        public string RoundSummary => _roundSummary;

        public void StartMatch()
        {
            _playerWins = 0;
            _opponentWins = 0;
            _currentRound = 1;
            ResetRound();
        }

        public void DrawCard()
        {
            if (_isRoundResolved || IsMatchCompleted)
            {
                return;
            }

            _playerTotal += DrawRandomCard();

            if (_playerTotal > MaxPoints)
            {
                ResolveRound();
            }
        }

        public void Stand()
        {
            if (_isRoundResolved || IsMatchCompleted)
            {
                return;
            }

            while (_opponentTotal < AiStandThreshold)
            {
                _opponentTotal += DrawRandomCard();
            }

            ResolveRound();
        }

        public void AdvanceRound()
        {
            if (!_isRoundResolved)
            {
                return;
            }

            _currentRound += 1;

            if (!IsMatchCompleted)
            {
                ResetRound();
            }
        }

        private void ResetRound()
        {
            _playerTotal = 0;
            _opponentTotal = 0;
            _isRoundResolved = false;
            _roundResult = BlackjackRoundResult.Pending;
            _roundSummary = "点击抽牌开始本局。";
        }

        private void ResolveRound()
        {
            _isRoundResolved = true;

            if (_playerTotal > MaxPoints)
            {
                _roundResult = BlackjackRoundResult.OpponentWin;
                _opponentWins += 1;
                _roundSummary = "男主爆牌，女主拿下本局。";
                return;
            }

            if (_opponentTotal > MaxPoints)
            {
                _roundResult = BlackjackRoundResult.PlayerWin;
                _playerWins += 1;
                _roundSummary = "女主爆牌，男主拿下本局。";
                return;
            }

            if (_playerTotal > _opponentTotal)
            {
                _roundResult = BlackjackRoundResult.PlayerWin;
                _playerWins += 1;
                _roundSummary = "男主更接近 21 点，本局获胜。";
                return;
            }

            if (_playerTotal < _opponentTotal)
            {
                _roundResult = BlackjackRoundResult.OpponentWin;
                _opponentWins += 1;
                _roundSummary = "女主更接近 21 点，本局获胜。";
                return;
            }

            _roundResult = BlackjackRoundResult.Draw;
            _roundSummary = "双方平局，再接再厉。";
        }

        private static int DrawRandomCard()
        {
            return Random.Range(1, 11);
        }
    }
}
