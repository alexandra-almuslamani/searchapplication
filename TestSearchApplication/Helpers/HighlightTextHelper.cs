using RapidFuzz.Net;

namespace TestSearchApplication.Helpers
{
    public class HighlightTextHelper
    {
        public static string HighlightMatches(string text, string searchTerm)
        {
            string cleanText = TextHelper.RemoveDiacritics(text);
            string cleanSearch = TextHelper.RemoveDiacritics(searchTerm);

            var textWords = text.Split(' ');
            var cleanWords = cleanText.Split(' ');
            var searchWords = cleanSearch.Split(' ');

            for (int i = 0; i < cleanWords.Length; i++)
            {
                foreach (var searchWord in searchWords)
                {
                    if (FuzzyMatcher.PartialRatio(searchWord, cleanWords[i]) >= 90)
                    {
                        textWords[i] = $"<span class='search-highlight'>{textWords[i]}</span>";
                        break;
                    }
                }
            }

            return string.Join(" ", textWords);
        }
    }
}
