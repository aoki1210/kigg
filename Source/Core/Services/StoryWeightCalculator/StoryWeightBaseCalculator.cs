namespace Kigg.Service
{
    using System;
    using System.Diagnostics;

    using Domain.Entities;

    public abstract class StoryWeightBaseCalculator : IStoryWeightCalculator
    {
        private readonly string _name;

        protected StoryWeightBaseCalculator(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            _name = name;
        }

        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return _name;
            }
        }

        public abstract double Calculate(DateTime publishingTimestamp, Story story);
    }
}