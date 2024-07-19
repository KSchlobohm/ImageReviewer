namespace ImageReviewer
{
    public interface IYoloDataReader
    {
        Result<IEnumerable<ImageData>> DiscoverYoloDetections();
    }
}
