using Detached.Annotations;
using System;

namespace GraphInheritenceTests.ComplexModels
{
    public class Recommendation : IdBase
    {
        public DateTime RecommendationDate { get; set; }

        public int RecommendedById { get; set; }
        [Aggregation]
        public Customer RecommendedBy { get; set; }

        public int RecommendedToId { get; set; }
        [Aggregation]
        public Customer RecommendedTo { get; set; }
    }
}