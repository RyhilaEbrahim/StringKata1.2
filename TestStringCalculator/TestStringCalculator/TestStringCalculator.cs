using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TestStringCalculator
{
    [TestFixture]
    public class TestStringCalculator
    {
        [Test]
        public void Add_GivenEmptyString_Should0()
        {
            //---------------Set up test pack-------------------
            const string input = "";
            const int expected = 0;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Add_GivenSingleDigitString_ShouldItself()
        {
            //---------------Set up test pack-------------------
            const string input = "1";
            const int expected = 1;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }
        
        [Test]
        public void Add_GivenCommaDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1,2";
            const int expected = 3;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }
        
        [Test]
        public void Add_GivenNewLineDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1,2\n5";
            const int expected = 8;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Add_GivenCustomDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//;\n5;3";
            const int expected = 8;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Add_GivenNegativeDelimitedString_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            const string input = "5,-10";
            const string expected = "Negative numbers not allowed -10";
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result =Assert.Throws<Exception>(() => calculate.Add(input));
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result.Message);
        }

        [Test]
        public void Add_GivenNumbersGreaterThan1000DelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1500,55,10";
            const int expected = 65;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result =calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }
        [Test]
        public void Add_GivenMultiDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[***]\n55***5***5";
            const int expected = 65;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result =calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Add_GivenMultipleDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[*][%]\n55*5%5";
            const int expected = 65;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result =calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }

        private static Calculate GetCalculate()
        {
            Calculate calculate = new Calculate();
            return calculate;
        }
    }

    public class Calculate  
    {
        public int Add(string input)
        {
            var defaultDemiliters = DefaultDemiliters();
            if (!defaultDemiliters.Any(input.Contains)) return string.IsNullOrEmpty(input) ? 0 : ConvertToInt(input);
            if (StartsWith(input))
            {
                var splitParts = SplitOnNewLine(input);
                var getFirstPart = splitParts.First();
                var customDelimiter = GetCustomDelimiter(getFirstPart);
                defaultDemiliters.Add(customDelimiter);
                if (input.Contains("["))
                {
                    var customDelimiters = GetCustomDelimiters(getFirstPart);
                    defaultDemiliters.AddRange(customDelimiters);
                }

                input = splitParts.Last();
            }
            var listOfNumbers = SplitIntoListOfNumbers(input, defaultDemiliters);
            var filterNegativeNumbers = FilterNegativeNumbers(listOfNumbers);
            ThrowExceptionWhenLessThan0(filterNegativeNumbers);
            var filterNumbersGreaterThan1000 = FilterNumbersGreaterThan1000(listOfNumbers);
            listOfNumbers = filterNumbersGreaterThan1000;
            var sum = Sum(listOfNumbers);
            return sum;
        }

        private List<string> GetCustomDelimiters(string getFirstPart)
        {
            var getDelimiter = getFirstPart.Substring(2,getFirstPart.Length-2);
            var delimiter = getDelimiter.Split('[',']').ToList();
            return delimiter;
        }

        private static bool StartsWith(string input)
        {
            return input.StartsWith("//");
        }

        private List<int> FilterNumbersGreaterThan1000(List<int> listOfNumbers)
        {
            for (int i = 0; i < listOfNumbers.Count; i++)
            {
                if (listOfNumbers[0] > 1000)
                {
                    listOfNumbers[i] = 0;
                }
            }
            return listOfNumbers;
        }

        private void ThrowExceptionWhenLessThan0(List<int> numberLessThan0)
        {
            var negativeValue = "";
            negativeValue = ConcatenateNegativeValues(numberLessThan0, negativeValue);
            if (numberLessThan0.Count > 0)
            {
                throw new Exception("Negative numbers not allowed " + negativeValue);

            }
        }

        private static string ConcatenateNegativeValues(List<int> numberLessThan0, string negativeValue)
        {
            for (var i = 0; i < numberLessThan0.Count; i++)
            {
                negativeValue += numberLessThan0[i];
            }
            return negativeValue;
        }
        private List<int> FilterNegativeNumbers(List<int> listOfNumbers)
        {
            return listOfNumbers.Where(i => i < 0).ToList();
        }

        private string GetCustomDelimiter(string first)
        {
            return first.Last().ToString();
        }

        private static string[] SplitOnNewLine(string input)
        {
            return input.Split('\n');
        }

        private static List<int> SplitIntoListOfNumbers(string input, List<string> defaultDemiliters)
        {
            var stringsOfNumbers = input.Split(defaultDemiliters.ToArray(),StringSplitOptions.RemoveEmptyEntries);
            var convertToIntAndToList = ConvertToIntAndToList(stringsOfNumbers);
            return convertToIntAndToList;
        }

        private static List<int> ConvertToIntAndToList(string[] stringsOfNumbers)
        {
           return stringsOfNumbers.Select(int.Parse).ToList();
        }

        private static List<string> DefaultDemiliters()
        {
            return new List<string> {",","\n"};
        }

        private int Sum(List<int> listOfNumbers)
        {
            if (listOfNumbers == null) throw new ArgumentNullException(nameof(listOfNumbers));
            return listOfNumbers.Sum();
        }

        private static int ConvertToInt(string input)
        {
            return int.Parse(input);
        }
    }
}
      