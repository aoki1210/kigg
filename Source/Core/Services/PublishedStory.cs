namespace Kigg.Service
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Domain.Entities;

    public class PublishedStory
    {
        public PublishedStory(Story story)
        {
            Check.Argument.IsNotNull(story, "story");

            Story = story;
            Weights = new Dictionary<string, double>();
        }

        public Story Story
        {
            get;
            private set;
        }

        public int Rank
        {
            get;
            internal set;
        }

        public IDictionary<string, double> Weights
        {
            get;
            private set;
        }

        public double TotalScore
        {
            [DebuggerStepThrough]
            get
            {
                return Weights.Sum(p => p.Value);
            }
        }
    }
}