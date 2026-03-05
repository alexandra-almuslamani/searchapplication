
using TestSearchApplication.Helpers;
using TestSearchApplication.Models;
using RapidFuzz.Net;
using TestSearchApplication.Services.SearchService.Interfaces;
using TestSearchApplication.Data;
using TestSearchApplication.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace TestSearchApplication.Services.SearchService
{
    public class SearcherAlgorithm : ISearcher
    {
        private readonly DataMSAccessContext _context;
        private SearchViewModel _searchViewModel;

        public SearcherAlgorithm(DataMSAccessContext context)
        {
            _context = context;
            _searchViewModel = new SearchViewModel
            {
                Results = new List<SearchResultItem>()
            };
        }
        public List<SearchResultItem> Search(string searchTerm, int batchSize, int batchNumber)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var tabs = _context.Tabs.Skip(batchSize * batchNumber).Take(batchSize).ToList();
                foreach (Tab b in tabs)
                {
                    string updatedText = TextHelper.RemoveDiacritics(b.MhNass);
                    string updatedSearchTerm = TextHelper.RemoveDiacritics(searchTerm);
                    string[] textWords = updatedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] searchWords = updatedSearchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    var matchingWords = new List<string>(); 

                    foreach (string searchWord in searchWords)
                    {
                        if (textWords.Any(textWord => FuzzyMatcher.PartialRatio(searchWord, textWord) >= 90))
                        {
                            matchingWords.Add(searchWord);
                        }
                    }

                    bool isSequential = false;
                    if (matchingWords.Count == searchWords.Length)
                    {
                        isSequential = IsSequentialMatch(textWords, searchWords);

                        double accuracy = (searchWords.Length > 0)
                        ? ((double)matchingWords.Count / searchWords.Length) * 100 : 0;

                        _searchViewModel.Results.Add(new SearchResultItem
                        {
                            Text = HighlightTextHelper.HighlightMatches(b.MhNass, searchTerm),
                            Accuracy = accuracy,
                            MNO = b.Mno,
                            IsSequential = isSequential
                        });
                    }
                }
            }
            return _searchViewModel.Results
            .OrderByDescending(item => item.IsSequential ? 1 : 0)
            .ThenByDescending(item => item.Accuracy)
            .Skip(batchSize * batchNumber)
            .Take(batchSize)
            .ToList();
        }
        private bool IsSequentialMatch(string[] textWords, string[] searchWords)
        {
            if (searchWords.Length == 0 || textWords.Length == 0)
            {
                return false;
            }

            int searchIndex = 0;
            int searchLength = searchWords.Length;

            for (int i = 0; i < textWords.Length; i++)
            {
                if (searchIndex < searchLength &&
                    FuzzyMatcher.PartialRatio(searchWords[searchIndex], textWords[i]) == 100)
                {
                    searchIndex++;
                    if (searchIndex == searchLength)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }

}
