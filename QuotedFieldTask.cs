using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TableParser
{
    //Тесты
	[TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    //Класс, генерирующий токены на основе строки и индекса разделителя с учётом всех экранирований.
    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            char quoteType = line[startIndex];
            string subline = null;
            int length = 0;
            if (quoteType == '"')
			{
                var quotedField = Regex.Match(line.Substring(startIndex), @"""(\\.|[^""])*""");
                if (!quotedField.Success)
                {
                    subline = line.Substring(startIndex + 1);
                    length = subline.Length + 1;
                }
                else
                {
                    subline = quotedField.Value.Substring(1, quotedField.Length - 2);
                    length = subline.Length + 2;
                }
            }
            //TODO: DRY!
            else if (quoteType == '\'')
            {
                var quotedField = Regex.Match(line.Substring(startIndex), @"'(\\.|[^'])*'");
                if (!quotedField.Success)
                {
                    subline = line.Substring(startIndex + 1);
                    length = subline.Length + 1;
                }
                else
                {
                    subline = quotedField.Value.Substring(1, quotedField.Length - 2);
                    length = subline.Length + 2;
                }
            }
            return new Token(Regex.Unescape(subline), startIndex, length);
        }
    }
}
