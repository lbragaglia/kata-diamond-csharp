using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace kata_diamond
{
    public class KataDiamonTests
    {
        private readonly Diamond _diamond;

        public KataDiamonTests() => _diamond = new Diamond();

        [Theory]
        [InlineData("A", "A")]
        [InlineData("B", "AB")]
        [InlineData("C", "ABC")]
        [InlineData("D", "ABCD")]
        public void ReturnsAnOrderedSequenceOfCharsStartingFromAToInputLetter(string inputChar, string expectedSequence)
        {
            var aDiamond = _diamond.Print(inputChar);
            Assert.Equal(expectedSequence, SequenceOfLettersUpToTheWidest(aDiamond));
        }

        [Theory]
        [InlineData("A", "A\n")]
        [InlineData("B", "A\nB\n")]
        [InlineData("C", "A\nB\nC\n")]
        [InlineData("D", "A\nB\nC\nD\n")]
        public void ReturnsAnOrderedSequenceOfLinesStartingFromAToInputLetter(string inputChar, string expectedSequence)
        {
            var aDiamond = _diamond.Print(inputChar);
            Assert.Equal(expectedSequence, Joined(SequenceOfLinesOfLettersUpToTheWidest(aDiamond)));
        }

        [Theory]
        [InlineData("A", "A\n")]
        [InlineData("B", "A \n B\n")]
        [InlineData("C", "A  \n B \n  C\n")]
        [InlineData("D", "A   \n B  \n  C \n   D\n")]
        public void ReturnsAnOrderedSequenceOfPaddedLinesStartingFromAToInputLetter(string inputChar, string expectedSequence)
        {
            var aDiamond = _diamond.Print(inputChar);
            Assert.Equal(expectedSequence, Joined(SequenceOfPaddedLinesOfLettersUpToTheWidest(aDiamond)));
        }

        [Theory]
        [InlineData("A", "A\n")]
        [InlineData("B", "A \n B\nA \n")]
        [InlineData("C", "A  \n B \n  C\n B \nA  \n")]
        [InlineData("D", "A   \n B  \n  C \n   D\n  C \n B  \nA   \n")]
        public void ReturnsAReflectedDownOrderedSequenceOfPaddedLinesStartingFromAToInputLetter(string inputChar, string expectedSequence)
        {
            var aDiamond = _diamond.Print(inputChar);
            Assert.Equal(expectedSequence, Joined(ReflectedDownSequenceOfPaddedLinesOfLettersUpToTheWidest(aDiamond)));
        }

        [Theory]
        [InlineData("A", "A\n")]
        [InlineData("B", " A \nB B\n A \n")]
        [InlineData("C", "  A  \n B B \nC   C\n B B \n  A  \n")]
        [InlineData("D", "   A   \n  B B  \n C   C \nD     D\n C   C \n  B B  \n   A   \n")]
        public void ReturnsAReflectedDownAndLeftOrderedSequenceOfPaddedLinesStartingFromAToInputLetter(string inputChar, string expectedSequence)
        {
            var aDiamond = _diamond.Print(inputChar);
            Assert.Equal(expectedSequence, Joined(ReflectedDownAndLeftSequenceOfPaddedLinesOfLettersUpToTheWidest(aDiamond)));
        }

        private IEnumerable<char> SequenceOfLettersUpToTheWidest(string aDiamond)
        {
            return string.Join("", SequenceOfLinesOfLettersUpToTheWidest(aDiamond).Select(line => line.Replace("\n", "")));
        }


        private IEnumerable<string> SequenceOfLinesOfLettersUpToTheWidest(string aDiamond)
        {
            return SequenceOfPaddedLinesOfLettersUpToTheWidest(aDiamond).Select(line => line.Replace(" ", ""));
        }

        private IEnumerable<string> SequenceOfPaddedLinesOfLettersUpToTheWidest(string aDiamond)
        {
            var lines = ReflectedDownSequenceOfPaddedLinesOfLettersUpToTheWidest(aDiamond);
            return lines.SkipLast(lines.Length / 2);
        }

        private string[] ReflectedDownSequenceOfPaddedLinesOfLettersUpToTheWidest(string aDiamond)
        {
            return ReflectedDownAndLeftSequenceOfPaddedLinesOfLettersUpToTheWidest(aDiamond).Select(line => line.Substring((line.Length - 1) / 2)).ToArray();
        }

        private string[] ReflectedDownAndLeftSequenceOfPaddedLinesOfLettersUpToTheWidest(string aDiamond)
        {
            return Splitted(aDiamond);
        }

        private IEnumerable<char> Joined(IEnumerable<string> lines) => string.Join("", lines);
        private string[] Splitted(string diamond) => diamond.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => $"{line}\n").ToArray();

    }

    internal class Diamond
    {
        private const string charSequence = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        internal string Print(string widestChar) => string.Join("", ReflectDown(ToPaddedLines(CharsUpTo(widestChar))).Select(ReflectLeft));

        private static string CharsUpTo(string widestChar) => charSequence.Substring(0, charSequence.IndexOf(widestChar) + 1);

        private static IEnumerable<string> ToPaddedLines(string chars)
        {
            var lineSize = chars.Length;
            return chars.Select((ch, index) => ch.ToString().PadLeft(index + 1).PadRight(lineSize) + "\n");
        }

        private static IEnumerable<string> ReflectDown(IEnumerable<string> lines) => lines.Concat(lines.Reverse().Skip(1));

        private static string ReflectLeft(string line) => string.Join("", line.Reverse().Skip(1).SkipLast(1)) + line);
    }
}
