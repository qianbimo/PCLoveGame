namespace PCLoveGame.Data
{
    public sealed class PuzzlePieceDefinition
    {
        public PuzzlePieceDefinition(string pieceId, string label, float snapDistance)
        {
            PieceId = pieceId;
            Label = label;
            SnapDistance = snapDistance;
        }

        public string PieceId { get; }

        public string Label { get; }

        public float SnapDistance { get; }
    }
}
