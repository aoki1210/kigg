namespace Kigg.Infrastructure
{
    using Domain.Entities;
    using Repository;

    public class TagMatchSpamWeightCalculator : ISpamWeightCalculator
    {
        private readonly int _matchValue;

        private readonly int _topTags;
        private readonly ITagRepository _tagRepository;

        public TagMatchSpamWeightCalculator(int matchValue, int topTags, ITagRepository tagRepository)
        {
            Check.Argument.IsNotZeroOrNegative(topTags, "topTags");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");

            _matchValue = matchValue;
            _topTags = topTags;
            _tagRepository = tagRepository;
        }

        public int Calculate(string content)
        {
            int total = 0;

            content = content.StripHtml().ToUpperInvariant();

            foreach (Tag tag in _tagRepository.FindByUsage(_topTags))
            {
                if (content.Contains(tag.Name.ToUpperInvariant()))
                {
                    total += _matchValue;
                }
            }

            return total;
        }
    }
}