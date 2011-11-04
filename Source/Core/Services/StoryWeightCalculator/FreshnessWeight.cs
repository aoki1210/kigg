namespace Kigg.Service
{
    using System;

    using Domain.Entities;

    public class FreshnessWeight : StoryWeightBaseCalculator
    {
        private readonly float _freshnessThresholdInDays;
        private readonly float _intervalInHours;

        public FreshnessWeight(float freshnessThresholdInDays, float intervalInHours) : base("Freshness")
        {
            Check.Argument.IsNotZeroOrNegative(freshnessThresholdInDays, "freshnessThresholdInDays");
            Check.Argument.IsNotZeroOrNegative(intervalInHours, "intervalInHours");

            _freshnessThresholdInDays = freshnessThresholdInDays;
            _intervalInHours = intervalInHours;
        }

        public override double Calculate(DateTime publishingTimestamp, Story story)
        {
            Check.Argument.IsNotInFuture(publishingTimestamp, "publishingTimestamp");
            Check.Argument.IsNotNull(story, "story");

            TimeSpan difference = (publishingTimestamp - story.CreatedAt);

            return (difference.TotalDays <= _freshnessThresholdInDays) ? difference.TotalHours / _intervalInHours : 0;
        }
    }
}