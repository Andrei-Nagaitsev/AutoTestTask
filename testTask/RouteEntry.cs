using System;

namespace testTask
{
    internal class RouteEntry
    {
        public string SourceCity { get; set; }
        public string DistanationCity { get; set; }
        public DateTime SourceTime { get; set; }
        public DateTime DistanationTime { get; set; }
        public DateTime TravelTime { get; set; }
        public decimal PriceRu { get; set; }
        public decimal PriceUs { get; set; }
        public string LinkToInfoPage { get; set; }
        public string RouteIndex { get; set; }

    }
}
