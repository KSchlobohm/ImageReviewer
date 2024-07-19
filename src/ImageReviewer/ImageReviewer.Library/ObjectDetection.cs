namespace ImageReviewer
{
    public class ObjectDetection
    {
        public int ClassId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsValid { get; set; }
    }
}
