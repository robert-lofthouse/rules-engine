using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Domain.RulesEngine.Business
{
	public static class StringExtensions
	{
		public static string ToBase64(this string input)
		{
			var data = Encoding.UTF8.GetBytes(input);
			return Convert.ToBase64String(data);
		}

		public static T ToEnum<T>(this string input)
		{
			return (T)Enum.Parse(typeof(T), input, true);
		}

		public static string FromBase64(this string input)
		{
			var data = Convert.FromBase64String(input);
			return Encoding.UTF8.GetString(data);
		}

		public static T DeserializeXml<T>(this string input, string schemaInfo = "<BAPS_Message>")
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			input = RemoveXMLSchemaInfo(input, schemaInfo);
			TextReader reader = new StringReader(input);
			T result = (T)serializer.Deserialize(reader);
			return result;
		}

		public static string RemoveXMLSchemaInfo(this string input, string rootNodeName)
		{
			if (!rootNodeName.EndsWith(">"))
			{
				rootNodeName += ">";
			}
			string searchRootName = rootNodeName.Substring(0, rootNodeName.Length - 2);
			int nsStart = input.IndexOf(searchRootName);
			string tempXML = input.Substring(nsStart + searchRootName.Length);
			int nsEnd = tempXML.IndexOf('>') + nsStart + searchRootName.Length + 1;
			var xml1 = input.Substring(0, nsStart);
			var xml2 = rootNodeName;
			var xml3 = input.Substring(nsEnd);
			input = xml1 + xml2 + xml3;
			return input;
		}

		public static string RemoveXMLSignature(this string input, string rootNodeName, bool addDefault = false)
		{
			string origXML = input;
			try
			{
				if (!rootNodeName.EndsWith(">"))
				{
					rootNodeName += ">";
				}
				string searchRootName = rootNodeName.Substring(0, rootNodeName.Length - 2);
				int nsStart = input.IndexOf(searchRootName);
				string tempxml = input.Substring(nsStart);
				string sigXML = "<?xml version=\"1.0\"?>";
				if (!addDefault)
				{
					sigXML = "";
				}
				input = sigXML + tempxml;
			}
			catch
			{
				input = origXML;
			}
			return input;
		}

		public static string RemoveSpecialCharactersAccount(this string input)
		{
			return Regex.Replace(input ?? "", "[^0-9]", string.Empty);
		}

		public static string RemovePrefixingDigitsFromName(this string input)
		{
			return Regex.Replace(input ?? "", @"(\d\/)", string.Empty).Trim();
		}

		public static string CleanAccountNumber(this string input)
		{
			StringBuilder account = new StringBuilder(input);
			account.Replace(" ", "")
					.Replace("-", "")
					.Replace("/", "")
					.Replace(@"\", "")
					.Replace("#", "")
					.Replace("?", "");
			return account.ToString();
		}

		/// <summary>
		/// Just to remove some special chars.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Clean(this string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return (input ?? "").Trim();
			}
			return new StringBuilder(input)
					.Replace("-", "")
					.Replace("/", "")
					.Replace(@"\", "")
					.Replace("#", "")
					.Replace("?", "")
					.ToString();
		}

		public static bool IsStringNumeric(this string input)
		{
			return Regex.IsMatch(input ?? "", @"^\d*$");
		}

		public static string GetAcountCCY(this string input)
		{
			Regex reg = new Regex(@"[A-Z]{3}");
			string curr = reg.Match(input ?? "".ToUpperInvariant()).Value;
			if (string.IsNullOrEmpty(curr))
			{
				curr = "";
			}
			return curr.ToUpperInvariant();
		}

		public static string GetMidasAccountWithoutZeros(this string input)
		{
			//create a variable to hold the midas account number
			string sMidasAccount = input;
			try
			{
				//get the currency
				string sCurrency = GetAcountCCY(input);
				//get the value up to the currency
				if (input.IndexOf(sCurrency, StringComparison.CurrentCulture) >= 1)
				{
					int ccyIndex = input.IndexOf(sCurrency, StringComparison.CurrentCultureIgnoreCase);
					if (ccyIndex < 6)
					{
						input = "0" + input;
						ccyIndex = input.IndexOf(sCurrency, StringComparison.CurrentCultureIgnoreCase);
					}
					sMidasAccount = input.Substring(ccyIndex - 6, ccyIndex);
					sMidasAccount += sCurrency;
					if (input.Length >= ccyIndex + 9)
					{
						string rest = input.Substring(ccyIndex + 3, 6);
						if (rest != null)
						{
							sMidasAccount += rest;
						}
					}
					else
					{
						//do what was done as failsafe
						//get the last 6 digits
						string sLastSixChars = "";
						sLastSixChars = string.Join("", input.Reverse().ToArray()).Substring(0, 6);
						sLastSixChars = string.Join("", sLastSixChars.Reverse());
						sMidasAccount += sLastSixChars;
					}
				}
			}
			catch
			{
				//. Ignore error
			}
			//return the account number
			return sMidasAccount;
		}

		public static string TruncateLongString(this string input, int maxLength)
		{
			return input.Substring(0, Math.Min(input.Length, maxLength));
		}

		public static DateTime? StringToDateTime(this string input)
		{
			DateTime? retValue = null;
			DateTime parseDate;
			if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out parseDate))
			{
				retValue = parseDate;
			}
			return retValue;
		}

		public static string GetCountryCodeFromBIC(this string input)
		{
			if (!string.IsNullOrWhiteSpace(input) && input.Length >= 6)
			{
				return input.Substring(4, 2);
			}
			return null;
		}

		public static string RemoveEscapeCodes(this string input, bool addSpace = false)
		{
			StringBuilder sb = new StringBuilder();
			string[] parts = input.Trim().Split(new char[] { '\n', '\t', '\r', '\f', '\v', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			int size = parts.Length;
			for (int i = 0; i < size; i++)
			{
				sb.AppendFormat(addSpace ? "{0} " : "{0}", parts[i]);
			}
			string result = sb.ToString();
			if (result.LastOrDefault() == ' ')
			{
				result = result.Remove(result.Length - 1);
			}
			return sb.ToString().Trim();
		}

		public static string ToSpacedJson(this string input)
		{
			return string.IsNullOrWhiteSpace(input) ? input
					: new StringBuilder(input).Replace("\",\"", "\" , \"")
					.Replace("\":\"", "\" : \"")
					.ToString();
		}

		public static string AccountNumberOnly(this string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			input = input.Trim();
			if (input.Length <= 7)
			{
				return input;
			}
			var indx = input.Length - 7;
			return input.Substring(indx);
		}

		public static string ToBICIdCode(this string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			else
			{
				return input.Trim().Length > 8 ? input.Trim().Substring(0, 8) : input.Trim();
			}
		}

		public static string ToBICIdCode(this string input, bool checkBranch)
		{
			if (checkBranch && !string.IsNullOrWhiteSpace(input) && !(input.Trim().Length == 8))
			{
				var regex = new Regex(@"\w{8}(XXX)");
				if (!regex.IsMatch(input))
				{
					return input.Trim();
				}
			}
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			else
			{
				return input.Trim().Length > 8 ? input.Trim().Substring(0, 8) : input.Trim();
			}
		}

		/// <summary>
		/// Character substitution based on string based dictionary
		/// </summary>
		/// <param name="input">Input string</param>
		/// <param name="substitutionCharValues">Substitution list delimited by comma(,) i.e '0':'1', '/':'-' </param>
		/// <returns></returns>
		public static string Substitution(this string input, string substitutionCharValues)
		{
			if (string.IsNullOrWhiteSpace(substitutionCharValues))
			{
				return input;
			}
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			var dictionary = substitutionCharValues
					.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(c => c.Split(new[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries))
					.ToDictionary(r => r[0].Trim(), r => r[1].Trim());
			return dictionary.Aggregate(input, (current, item) => Regex.Replace(current, item.Key.Replace("'", ""), item.Value.Replace("'", ""), RegexOptions.IgnoreCase)).RemoveExtraSpaces();
		}

		public static string RemoveExtraSpaces(this string input)
		{
			return string.IsNullOrWhiteSpace(input) ? input : new Regex(@"( {2,})").Replace(input, " ");
		}

		public static bool In(this string input, params string[] expressions)
		{
			return input.In(StringComparison.CurrentCultureIgnoreCase, expressions);
		}

		public static bool In(this string input, StringComparison stringComparison, params string[] expressions)
		{
			return expressions.Any(e => string.Equals(e, input, stringComparison));
		}

		public static bool NotIn(this string input, params string[] expressions)
		{
			return input.NotIn(StringComparison.CurrentCultureIgnoreCase, expressions);
		}

		public static bool NotIn(this string input, StringComparison stringComparison, params string[] expressions)
		{
			return !expressions.Any(e => string.Equals(e, input, stringComparison));
		}

		public static double ToDouble(this string input, double defaultValue = 0)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return defaultValue;
			}
			return double.TryParse(input, out var convertedValue) ? convertedValue : defaultValue;
		}

		public static int ToInt(this string input, int defaultValue = 0)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return defaultValue;
			}
			return int.TryParse(input, out var convertedValue) ? convertedValue : defaultValue;
		}
	}
}
