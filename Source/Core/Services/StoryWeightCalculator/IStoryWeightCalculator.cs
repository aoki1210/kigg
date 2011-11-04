namespace Kigg.Service
{
    using System;

    using Domain.Entities;

    public interface IStoryWeightCalculator
    {
        string Name
        {
            get;
        }

        double Calculate(DateTime publishingTimestamp, Story story);
    }
}