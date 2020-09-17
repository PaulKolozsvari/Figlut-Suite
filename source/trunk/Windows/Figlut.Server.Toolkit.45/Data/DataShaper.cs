namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion //Using Directives

    public class DataShaper
    {
        #region Methods

        public static string ShapeCamelCaseString(string inputString)
        {
            List<int> spaceIndexes = new List<int>();
            for (int i = 0; i < inputString.Length; i++)
            {
                if (char.IsUpper(inputString[i]))
                {
                    spaceIndexes.Add(i);
                }
            }
            for (int i = 1; i < spaceIndexes.Count; i++)
            {
                inputString = inputString.Insert(spaceIndexes[i], " ");
                for (int j = i; j < spaceIndexes.Count; j++)
                {
                    spaceIndexes[j] += 1;
                }
            }
            return inputString;
        }

        public static int GetIndexOfFirstUpperCaseLetter(string inputString)
        {
            for (int i = 0; i < inputString.Length; i++)
            {
                if (char.IsUpper(inputString[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static string RestoreStringToCamelCase(string inputString)
        {
            return inputString.Replace(" ", string.Empty);
        }

        public static string MaskPasswordString(string passwordString, char passwordChar)
        {
            string result = string.Empty;
            return result.PadRight(passwordString.Length, passwordChar);
        }

        public static string GetUniqueIdentifier()
        {
            return string.Format("ID{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
        }

        private static bool invalid = false;

        public static bool IsValidPhoneNumber(string phoneNumber, out string formattedPhoneNumber)
        {
            return new PhoneNumberValidator().IsValidPhoneNumber(phoneNumber, out formattedPhoneNumber);
        }

        public static bool IsValidEmail(string emailAddress)
        {
            return (new EmailAddressAttribute().IsValid(emailAddress));
        }

        /// <summary>
        /// Generates password with special characters.
        /// </summary>
        public static string GeneratePassword(int passwordLength, int numberOfSpecialCharacters)
        {
            return System.Web.Security.Membership.GeneratePassword(passwordLength, numberOfSpecialCharacters);
        }

        /// <summary>
        /// Generates password without special characters.
        /// </summary>
        public static string GenerateSimplePassword(int passwordLength, int numberOfExtraSpecialCharacters)
        {
            string result = GeneratePassword(passwordLength, 1);
            result = Regex.Replace(result, @"[^a-zA-Z0-9]", m => "9");
            string specialCharacters = GeneratePassword(numberOfExtraSpecialCharacters, numberOfExtraSpecialCharacters);
            return string.Concat(result, specialCharacters);
        }

        //Generates a password from a Guid removing all the dashes and appends the specified number of special characters.
        public static string GenerateGuidPassword(int numberOfExtraSpecialCharacters)
        {
            string result = Guid.NewGuid().ToString().Replace("-", string.Empty);
            string specialCharacters = GeneratePassword(numberOfExtraSpecialCharacters, numberOfExtraSpecialCharacters);
            return string.Concat(result, specialCharacters);
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static bool StringContainsKeywords(string stringToSearchIn, bool caseSensitive, List<string> keywords)
        {
            string originatinValueToSearch = caseSensitive ? stringToSearchIn : stringToSearchIn.ToLower();
            for (int k = 0; k < keywords.Count; k++)
            {
                string originatingValueToSearchFor = caseSensitive ? keywords[k] : keywords[k].ToLower();
                if (!originatinValueToSearch.Contains(originatingValueToSearchFor))
                {
                    break;
                }
                if (k == (keywords.Count - 1)) //This is the last of keywords to search for in string; hence this string contains all the keywords.
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetCurrencyValueString(double currencyValue, string currencySymbol)
        {
            string result = null;
            if (!string.IsNullOrEmpty(currencySymbol))
            {
                CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                NumberFormatInfo numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                numberFormatInfo.CurrencySymbol = currencySymbol; // Replace with "$" or "£" or whatever you need
                result = currencyValue.ToString("C2", numberFormatInfo);
            }
            else
            {
                result = currencyValue.ToString("C2", CultureInfo.CurrentCulture);
            }
            return result;
        }

        public static string GetCurrencyValueString(double currencyValue, bool includeWindowsRegionalCurrencySymbol)
        {
            if (includeWindowsRegionalCurrencySymbol)
            {
                return string.Format("{0:C}", currencyValue);
            }
            else
            {
                return string.Format("{0:N2}", currencyValue);
            }
        }

        //public static string GetCurrencyValueString(double currencyValue, string currencySymbol)
        //{
        //    string numericValue = 
        //}

        /// <summary>
        /// Increments a string by treating it like a number and increments it as per UTF-16 table (default .NET encoding) e.g. ABC becomes ABD.
        /// Default .NET encoding for a char is UTF-16 (2 byte/16 bit)
        /// FYI: http://csharpindepth.com/Articles/General/Unicode.aspx
        /// </summary>
        /// <param name="input">The input string to be incremented.</param>
        /// <returns>The incremented result</returns>
        //public static string IncrementUnicodeString(string input)
        //{
        //    string result = string.Empty;
        //    for (int i = (input.Length - 1); i >= 0; i--) //Work backwards from the last character, increment the last character and rolling over the previous if required (last char is at char.MaxValue).
        //    {
        //        char c = input[i];
        //        if (c < char.MaxValue) //This character must be rolled over and prepended to the result. The loop must also stop i.e. no need to go through the rest of the characters.
        //        {
        //            char newChar = Convert.ToChar(c + 1);
        //            result = string.Format("{0}{1}", newChar, result);
        //            break;
        //        }
        //        else //This character must be set to reset (set to char.MinValue) and prepended to the result.
        //        {
        //            result = string.Format("{0}{1}", char.MinValue, result);
        //            if (i == 0) //This is the first character, which was just reset, therefore an extra character needs to be prepended.
        //            {
        //                char nextChar = Convert.ToChar(char.MinValue + 1);
        //                result = string.Format("{0}{1}", nextChar, result);
        //            }
        //        }
        //    }
        //    if (input.Length > result.Length) //Not all the characters in the input string have been replaced, therefore prepend the ones that haven't been replaced to the result.
        //    {
        //        result = string.Format("{0}{1}",
        //            input.Substring(0, (input.Length - result.Length)),
        //            result);
        //    }
        //    return result;
        //}

        /// <summary>
        /// Increments a string by treating it like a number and increments it as per UTF-16 table (default .NET encoding) e.g. ABC becomes ABD.
        /// Default .NET encoding for a char is UTF-16 (2 byte/16 bit)
        /// FYI: http://csharpindepth.com/Articles/General/Unicode.aspx
        /// </summary>
        /// <param name="input">The input string to be incremented.</param>
        /// <param name="minimumChar">The minimum character to be allowed in the range. Used in combination with the maximum char, it allows you to set a unicode character boundary for each character. If set to null it defaults to char.MinValue.</param>
        /// <param name="maximumChar">The maximum character to be allowed in the range. Used in combination with the minimum char, it allows you to set a unicode character boundary for each character. If set to null it defaults to char.MaxValue.</param>
        /// <param name="validCharacters"></param>
        /// <returns></returns>
        public static string IncrementUnicodeString(string input, Nullable<char> minimumChar, Nullable<char> maximumChar, List<char> validCharacters)
        {
            if (!minimumChar.HasValue)
            {
                minimumChar = char.MinValue;
            }
            if (!maximumChar.HasValue)
            {
                maximumChar = char.MaxValue;
            }
            ValidateInputUnicodeStringForIncrement(input, minimumChar, maximumChar, validCharacters);
            string result = string.Empty;
            for (int i = (input.Length - 1); i >= 0; i--) //Work backwards from the last character, increment the last character and rolling over the previous if required (last char is at maximumChar).
            {
                char c = input[i];
                if (c < maximumChar.Value) //This character must be rolled over and prepended to the result. The loop must also stop i.e. no need to go through the rest of the characters.
                {
                    char newChar = Convert.ToChar(c + 1);
                    while (!validCharacters.Contains(newChar) && (newChar < maximumChar)) //Increment character until a valid character is created.
                    {
                        newChar = Convert.ToChar(newChar + 1);
                    }
                    result = newChar > maximumChar ? RollOverString(result, minimumChar.Value, i) : string.Format("{0}{1}", newChar, result);
                    break;
                }
                else //This character must be reset (set to minimumChar) and prepended to the result.
                {
                    result = RollOverString(result, minimumChar.Value, i);
                }
            }
            if (input.Length > result.Length) //Not all the characters in the input string have been replaced, therefore prepend the ones that haven't been replaced to the result.
            {
                result = string.Format("{0}{1}",
                    input.Substring(0, (input.Length - result.Length)),
                    result);
            }
            return result;
        }

        /// <summary>
        /// Validates that an input string for incrementing is valid: that it does not contain invalid characters. 
        /// Also that the minimum char is smaller than the maximum char. Lastly that the minimum and maximum characters are not invalid characters.
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <param name="minimumChar">The minimum character.</param>
        /// <param name="maximumChar">The maximum character.</param>
        /// <param name="validCharacters">The list of valid characters.</param>
        private static void ValidateInputUnicodeStringForIncrement(string input, Nullable<char> minimumChar, Nullable<char> maximumChar, List<char> validCharacters)
        {
            if (minimumChar > maximumChar)
            {
                throw new ArgumentOutOfRangeException("minimumChar may not be greater than maximumChar when incrementing a string.");
            }
            if (StringContainsInvalidCharacters(input, validCharacters))
            {
                throw new Exception(string.Format("Cannot increment input string '{0}'. It contains invalid characters.", input));
            }
            if (!validCharacters.Contains(minimumChar.Value))
            {
                throw new ArgumentOutOfRangeException("minimumChar does not exist in the validCharacters list.");
            }
            if (!validCharacters.Contains(maximumChar.Value))
            {
                throw new ArgumentOutOfRangeException("maximumChar does not exist in the validCharacters list.");
            }
        }

        /// <summary>
        /// Rolls over a string as part of the string increment algorithm e.g. 1A becomes 20.
        /// </summary>
        /// <param name="input">The input string to roll over.</param>
        /// <param name="minimumChar">The minimum character in the unicode table/range to inckude in the roll over e.g. if input is 1A and and minimum character is 0, then the result will be 20.</param>
        /// <param name="currentCharIndex">The current index of the charectsr we're working on for incrementing. This is to check if it is the first character, in which case an extra character needs to be prepended.</param>
        /// <returns></returns>
        private static string RollOverString(string input, char minimumChar, int currentCharIndex)
        {
            string result = string.Format("{0}{1}", minimumChar, input);
            if (currentCharIndex == 0) //This is the first character, which was just reset, therefore an extra character needs to be prepended.
            {
                char nextChar = Convert.ToChar(minimumChar + 1);
                result = string.Format("{0}{1}", nextChar, result);
            }
            return result;
        }

        /// <summary>
        /// Gets a list of strings from the start string to the end string by incrementing the numeric part of the start string until the end string
        /// e.g. if the startString is B009 and end string is B012, the result list will contain strings B009, B010, B011 and B012. It will not increment
        /// the letters in the string if the numeric part rolls over, but instead it will expand the length of the numeric part e.g. If the startString is
        /// B9 and the endString is B12, the result list will contain the strings B9, B10, B11 and B12.
        /// </summary>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <returns></returns>
        public static List<string> GetNumericStringRange(string startString, string endString)
        {
            string numericPartStart = GetNumericPartOfString(startString, out int startInt);
            string numericPartEnd = GetNumericPartOfString(endString, out int endInt);
            string alphaPartStart = GetAplhaPartOfString(startString);
            string alphaPartEnd = GetAplhaPartOfString(endString);
            if (!alphaPartStart.Equals(alphaPartEnd))
            {
                throw new Exception("Alpha letters in start string need to match alpha letters in end string when creating numeric string range.");
            }
            if (startInt > endInt)
            {
                throw new Exception("Numeric part of start string needs to be less than numeric part of end string when creating numeric string range.");
            }
            List<string> result = new List<string>() { startString };
            string nextString = startString;
            int nextInt = startInt;
            while (nextInt < endInt) //i.e. while the next string is smaller than the endString, keep incrementing and adding nextString to the result.
            {
                nextString = IncrementNumericPartOfString(nextString, out nextInt);
                result.Add(nextString);
            }
            return result;
        }

        public const string ALPHA_REGEX_PATTERN = @"^[a-zA-Z]+";
        /// <summary>
        /// Increments the numeric part of a string e.g. if the input is B009, the incremented output will be B010.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="nextNumber"></param>
        /// <returns></returns>
        public static string IncrementNumericPartOfString(string input, out int nextNumber)
        {
            string alphaPart = Regex.Match(input, ALPHA_REGEX_PATTERN).Value;
            string numberPart = Regex.Replace(input, ALPHA_REGEX_PATTERN, string.Empty);
            int number = int.Parse(numberPart);
            nextNumber = number + 1;
            int length = numberPart.Length;
            length = nextNumber / (Math.Pow(10, length)) == 1 ? length + 1 : length;
            string result = alphaPart + nextNumber.ToString("D" + length);
            return result;
        }

        /// <summary>
        /// Gets the alphabetic part of string i.e. all the letters without the numbers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetAplhaPartOfString(string input)
        {
            return Regex.Match(input, ALPHA_REGEX_PATTERN).Value;
        }

        /// <summary>
        /// Gets the numeric (integer) part of a string i.e. all the numbers without the letters.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetNumericPartOfString(string input, out int number)
        {
            string pattern = ALPHA_REGEX_PATTERN;
            string result = Regex.Replace(input, ALPHA_REGEX_PATTERN, "");
            number = int.Parse(result);
            return result;
        }

        /// <summary>
        /// Compares two strings, by treating them like numbers as per the UTF/ASCII table i.e. ABC is smaller than ABD.
        /// Default .NET encoding for a char is UTF-16 (2 byte/16 bit)
        /// FYI: http://csharpindepth.com/Articles/General/Unicode.aspx
        /// </summary>
        /// <param name="input">The input string in question.</param>
        /// <param name="toCompareAgainst">The string to compare the input string to.</param>
        /// <returns>Result of the comparison.</returns>
        public static bool IsUnicodeStringGreaterThan(string input, string toCompareAgainst)
        {
            if (input == toCompareAgainst)
            {
                return false;
            }
            if (input.Length > toCompareAgainst.Length) //The input string has more characters, thus it is greater i.e. has more weight.
            {
                return true;
            }
            else if (input.Length < toCompareAgainst.Length) //The input string has less characters, thus it is smaller i.e. has less weight.
            {
                return false;
            }
            //We know that they are of the same length i.e. same number or characters.
            bool result = false; //Whether or not the input string is greater.
            for (int i = 0; i < input.Length; i++) //Check if longer string is smaller than the shorter string.
            {
                char a = input[i];
                char b = toCompareAgainst[i];
                if (a == b) //Same characters, so continue with the loop and checking the next character e.g. with 3A vs 3B we check the first character which are equals then we check the next character.
                {
                    continue;
                }
                result = a > b; //longer string is smaller than the shorter string, or otherwise it's bigger. Break out of the loop, because we know which is greater or smaller.
                break;
            }
            return result;
        }

        /// <summary>
        /// Checks whether a string contains characters that are not in the provided list of valid characters.
        /// </summary>
        /// <param name="input">The input string to be checked.</param>
        /// <param name="validCharacters">The list of characters that are valid i.e. any character in the input that is not in this list is invalid. If a null is passed, the result of this method will always be false.</param>
        /// <returns>Returns a bool indicating whether or not the input string contains invalid characters i.e. characters that are not included in the validCharacters list.</returns>
        public static bool StringContainsInvalidCharacters(string input, List<char> validCharacters)
        {
            if (validCharacters == null)
            {
                return false;
            }
            foreach (char c in input.ToCharArray())
            {
                if (!validCharacters.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a list of all the characters from lower case a - z and then 0 - 9.
        /// </summary>
        /// <returns>Returns a list of all the characters from lower case a - z and then 0 - 9.</returns>
        public static List<char> GetValidAlphaNumericRangeCharacters()
        {
            List<char> result = new List<char>()
            {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'g',
                'i',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'v',
                'v',
                'w',
                'x',
                'y',
                'z',
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            return result;
        }

        /// <summary>
        /// Gets a list of all the characters from lower case a - z.
        /// </summary>
        /// <returns>Returns a list of all the characters from lower case a - z.</returns>
        public static List<char> GetValidAlphabetRangeCharacters()
        {
            List<char> result = new List<char>()
            {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'g',
                'i',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'v',
                'v',
                'w',
                'x',
                'y',
                'z'
            };
            return result;
        }

        /// <summary>
        /// Gets a list of all characters from 0 - 9.
        /// </summary>
        /// <returns>Returns a list of all characters from 0 - 9.</returns>
        public static List<char> GetValidNumericRangeCharacters()
        {
            List<char> result = new List<char>()
            {
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            return result;
        }

        /// <summary>
        /// Returns a range of unicode strings from the start string to the end string 
        /// For example, setting the start string to 1A and the end string to 2F it will return a range of: 1A, 1B, 1C ... 1Z, 20, 21, 22 ... 29, 2A, 2B, 2C ... 2F.
        /// </summary>
        /// <param name="startString">The start of the string range.</param>
        /// <param name="endString">The end of the string range.</param>
        /// <param name="validCharacters">A list of characters that will considered valid. Any characters outside of this list will not be included in the range. If this parameter is set to null, then all characters in the UTF-16 table are valid and included in the result range.</param>
        /// <returns>Returns the range of unicode (UTF-16) strings as a list.</returns>
        public static List<string> GetUnicodeStringRange(
            string startString, 
            string endString, 
            Nullable<char> minimumChar, 
            Nullable<char> maximumChar,
            List<char> validCharacters)
        {
            if (IsUnicodeStringGreaterThan(startString, endString))
            {
                throw new Exception(string.Format("Start string '{0}' may not be greater than End string '{1}' when specifying a string range.",
                    startString,
                    endString));
            }
            if (StringContainsInvalidCharacters(startString, validCharacters)) //Returns false if the validCharacters is null.
            {
                throw new Exception(string.Format("Start string '{0}' contains invalid characters in the specified range.", startString));
            }
            if (StringContainsInvalidCharacters(endString, validCharacters)) //Returns false if the validCharacters is null.
            {
                throw new Exception(string.Format("End string '{0}' contains invalid characters in the specified range.", endString));
            }
            List<string> result = new List<string>() { startString };
            string nextString = startString;
            ValidateInputUnicodeStringForIncrement(nextString, minimumChar, maximumChar, validCharacters);
            while (IsUnicodeStringGreaterThan(endString, nextString)) //i.e. while the next string is smaller than the endString, keep incrementing and adding nextString to the result.
            {
                nextString = IncrementUnicodeString(nextString, minimumChar, maximumChar, validCharacters); //Increment the string to the next unicode string.
                result.Add(nextString);
            }
            return result;
        }

        public static string GetDefaultDateString(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            if (month.Length < 2)
            {
                month = string.Format("0{0}", month);
            }
            if (day.Length < 2)
            {
                day = string.Format("0{0}", day);
            }
            string result = string.Format("{0}-{1}-{2}", year, month, day);
            return result;
        }

        /// <summary>
        /// Checks each character of a string to determine whether all characters are digits.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStringOnlyDigits(string input)
        {
            foreach (char c in input.ToCharArray())
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the the given searchFilter text is null and sets it to an empty string, otherwise converts it to lowercase.
        /// </summary>
        public static string GetSearchFilterLowered(string searchFilter)
        {
            return searchFilter == null ? string.Empty : searchFilter.ToLower();
        }

        /// <summary>
        /// Checks if the given guid is set has the value of an empty Guid and if so, returns null, otherwise returns the passed in Guid unchanged.
        /// </summary>
        public static Nullable<Guid> ConvertEmptyGuidToNull(Nullable<Guid> guid)
        {
            if (guid.HasValue && guid == Guid.Empty)
            {
                guid = null;
            }
            return guid;
        }

        #endregion //Methods
    }
}