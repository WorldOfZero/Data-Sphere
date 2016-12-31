using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataSphere.Plugins.Twitter
{
    public class TweetViewModel
    {
        public TweetPlaceViewModel place;
    }

    public class TweetPlaceViewModel
    {
        [JsonProperty("bounding_box")]
        public BoundingBoxViewModel boundingBox;
    }

    public class BoundingBoxViewModel
    {
        public IEnumerable<IEnumerable<double[]>> coordinates;
    }
}
