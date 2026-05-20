namespace PCLoveGame.Data
{
    public sealed class ChoiceOptionData
    {
        public ChoiceOptionData(string optionId, string label, string resultText, int favorabilityChange, bool isCorrect)
        {
            OptionId = optionId;
            Label = label;
            ResultText = resultText;
            FavorabilityChange = favorabilityChange;
            IsCorrect = isCorrect;
        }

        public string OptionId { get; }

        public string Label { get; }

        public string ResultText { get; }

        public int FavorabilityChange { get; }

        public bool IsCorrect { get; }
    }
}
