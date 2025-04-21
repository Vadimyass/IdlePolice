using System.Collections.Generic;
using System.Linq;
using yutokun;

namespace Gameplay.Configs
{
    public class DefaultParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<IPhrase>();
            
            var sheet = CSVParser.LoadFromString(csvString);
            foreach (var row in sheet)
            {
                var phrase = new DefaultConfigInitializer();
                var valueList = new List<string>();
                if (row.All(string.IsNullOrEmpty))
                {
                    continue;
                }
                for (int i = 0; i < row.Count ; i++)
                {
                    if (i == 0)
                    {
                        phrase.Key = row[i];
                    }
                    else
                    {
                        valueList.Add(row[i]);
                    }
                }

                phrase.Pair = valueList;
                newPhrases.Add(phrase);
            }

            return newPhrases;
        }
    }
}