using YamlDotNet.Serialization;

namespace ImageReviewer
{
    public class YoloData
    {
        [YamlMember(Alias = "train")]
        public string TrainFolder { get; set; }

        [YamlMember(Alias = "val")]
        public string ValidationFolder { get; set; }

        [YamlMember(Alias = "names")]
        public string[] ClassNames { get; set; }

        [YamlMember(Alias = "nc")]
        public int NameCount { get; set; }

        public string YoloConfigFilePath { get; set; }
    }
}
