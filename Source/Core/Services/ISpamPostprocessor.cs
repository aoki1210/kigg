namespace Kigg.Service
{
    using Domain.Entities;

    public interface ISpamPostprocessor
    {
        void Process(string source, bool isSpam, string detailUrl, Story story);

        void Process(string source, bool isSpam, string detailUrl, Comment comment);
    }
}