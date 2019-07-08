using System;
using System.Collections.Generic;

namespace Geev.Application.Editions
{
    [Serializable]
    public class EditionfeatureCacheItem
    {
        public const string CacheStoreName = "GeevZeroEditionFeatures";

        public IDictionary<string, string> FeatureValues { get; set; }

        public EditionfeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}