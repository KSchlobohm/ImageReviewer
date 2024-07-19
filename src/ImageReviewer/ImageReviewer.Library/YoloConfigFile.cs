using YamlDotNet.Serialization;

namespace ImageReviewer
{
    public class YoloConfigFile
    {
        [YamlMember(Alias = "train")]
        public string TrainFolder { get; set; } = string.Empty;

        [YamlMember(Alias = "val")]
        public string ValidationFolder { get; set; } = string.Empty;

        [YamlMember(Alias = "names")]
        public string[] ClassNames { get; set; } = Array.Empty<string>();

        [YamlMember(Alias = "nc")]
        public int NameCount { get; set; }
    }
}
