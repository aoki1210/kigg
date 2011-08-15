namespace Kigg.Service
{
    using System;

    using DomainObjects;

    public interface IStoryWeightCalculator
    {
        string Name
        {
            get;
        }

        double Calculate(DateTime publishingTimestamp, Story story);
    }
}