namespace PCLoveGame.Data
{
    public sealed class ResolutionOption
    {
        public ResolutionOption(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public override string ToString()
        {
            return $"{Width} x {Height}";
        }
    }
}
