namespace ImageReviewer
{
    public record ImageData(string Path, List<ObjectDetection> Detections);
}
