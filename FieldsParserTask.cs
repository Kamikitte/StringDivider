using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TableParser
{
    //Тесты
    [TestFixture]
    public class FieldParserTaskTests
    {
        [TestCase("text", new[] { "text" })]
        [TestCase("''", new[] { "" })]
        [TestCase("''bc", new[] { "", "bc" })]
        [TestCase("\"hello\"\'world\'", new[] { "hello", "world" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("   test", new[] { "test" })]
        [TestCase("a\"b", new[] { "a", "b" })]
        [TestCase("hello   world", new[] { "hello", "world" })]
        [TestCase("\"hello world\"", new[] { "hello world" })]
        [TestCase("\'hello world", new[] { "hello world" })]
        [TestCase("\"hello  ", new[] { "hello  " })]
        [TestCase("\"hello \\\"\"", new[] { "hello \"" })]
        [TestCase("\'hello \\\'\'", new[] { "hello \'" })]
        [TestCase("\"hello \'\"", new[] { "hello \'" })]
        [TestCase("\'hello \"\'", new[] { "hello \"" })]
        [TestCase("\"hello \\\\\"", new[] { "hello \\" })]
        [TestCase("", new string[0])]
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }
    }

    //Класс для разбиения введённной строки на токены по разделителям
    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var result = new List<Token>();
            for(int charIndex = 0; charIndex < line.Length;)
			{
                if (line[charIndex] == ' ')
				{
                    charIndex++;
				}
                else if(line[charIndex] == '\'' || line[charIndex] == '"')
				{
                    var currentToken = ReadQuotedField(line, charIndex);
                    result.Add(currentToken);
                    charIndex += currentToken.Length;
				}
                else
				{
                    var currentToken = ReadField(line, charIndex);
                    result.Add(currentToken);
                    charIndex += currentToken.Length;
				}                    
			}
            return result;
        }
        private static Token ReadField(string line, int startIndex)
        {
            var field = Regex.Match(line.Substring(startIndex), @"[^'"" ]*");
            return new Token(field.Value.Trim(), startIndex, field.Length); ;
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}