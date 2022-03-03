using simple_api.Extensions;

namespace simple_api.Helpers
{
    public class BoardingCardsHelper
    {
        /// <summary>
        /// Sorts boarding passes
        /// </summary>
        /// <param name="collection">list of strings</param>
        /// <returns>A collection of sorted boarding cards</returns>
        public List<string> OrganizeBoardingCards(List<string> collection)
        {
            // Example data
            // JSON: [ "GOT-ARN", "HEL-GOT", "CPH-HEL" ]
            var result = new List<string>();
            var resultRaw = new List<KeyValuePair<string, string>>();

            var from = new List<string>();
            var to = new List<string>();

            var dict = new Dictionary<string, string>();

            // Try to separate the data while making the wild assumption that user wont try to break this (┛ಠ_ಠ)┛彡┻━┻
            foreach (var item in collection)
            {
                var pair = item.Split("-");
                dict.Add(pair[0], pair[1]);

                from.Add(pair[0]);
                to.Add(pair[1]);
            }


            var output = TryGetStartingPoint(from, to, dict);
            if (output.Key == default && output.Value == default)
            {
                return new List<string>();
            }
            resultRaw.Add(output);


            // Add the rest
            TryAddBoardingCards(ref resultRaw, dict, from);
            if (resultRaw.Count != dict.Count)
            {
                return new List<string>();
            }


            // Clean up so we can present nicely :) 
            resultRaw.ForEach(kvp => result.Add($"{kvp.Key}-{kvp.Value}"));

            return result;
        }

        private KeyValuePair<string, string> TryGetStartingPoint(List<string> from, List<string> to, Dictionary<string, string> dict)
        {
            // Add/Find starting point
            foreach (var f in from)
            {
                if (!to.Contains(f))
                {
                    return dict.GetEntry(f);
                }
            }

            return default;
        }

        private void TryAddBoardingCards(
            ref List<KeyValuePair<string, string>> resultRaw,
            Dictionary<string, string> dict,
            List<string> from)
        {
            var currentBoardingCard = resultRaw[0];

            // Add the rest
            for (int i = 0; i < dict.Count; i++)
            {
                // Check if the current boarding pass has a destination
                if (from.Contains(currentBoardingCard.Value))
                {
                    currentBoardingCard = dict.GetEntry(currentBoardingCard.Value);
                    resultRaw.Add(currentBoardingCard);
                }
            }
        }
    }
}
