/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:07 PM
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace BAE.ECast
{
	//
	// TODO: Clean up and remove uneccessary crap
	//
	public static class StringExtensions
	{
		public static DateTime? ToDateTimeNullable(this String dateTimeString)
		{
			if (String.IsNullOrEmpty(dateTimeString))
			{
				return null;
			}

			if (String.IsNullOrEmpty(dateTimeString.Trim()))
			{
				return null;
			}

			DateTime output;
			bool parseSucceeded = DateTime.TryParse(dateTimeString, out output);
			return (parseSucceeded) ? (DateTime?)output : null;
		}

		public static IEnumerable<String> GetSearchWordEnumerator(this String searchText)
		{
			if (String.IsNullOrEmpty(searchText))
			{
				yield break;
			}

			IEnumerable<String> searchWords = searchText.Split(new char[] { ' ' })
				.Where(w => !String.IsNullOrEmpty(w.Trim()));

			foreach (String word in searchWords)
			{
				yield return word;
			}
		}

		/// <summary>
		/// Converts a string to Title Case
		/// </summary>
		public static string ToTitleCase(this string str)
		{
			if (str != null)
			{
				CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
				TextInfo textInfo = cultureInfo.TextInfo;
				return textInfo.ToTitleCase(str);
			}
			else
			{
				return null;
			}
		}

		public static string ToPlainText(this string str)
		{
			// Remove any html markup
			Regex removeHtmlTags = new Regex(@"<[^>]*>");
			Regex removeHtmlEntityRefs = new Regex(@"&[a-z]+;");
			str = removeHtmlTags.Replace(str, "");
			str = removeHtmlEntityRefs.Replace(str, "");
			str = str.Replace("\t", "");
			str = str.Trim();

			while (str.Contains("  ")) // Make the text mono-space
			{
				str = str.Replace("  ", " ");
			}
			return str;
		}

		public static Dictionary<string, string> ToDictionary(
			this string str,
			char itemDelimeter,
			char fieldDelimeter)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			var items = str.Split(new char[] { itemDelimeter }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var item in items)
			{
				var fields = item.Split(new char[] { fieldDelimeter }, StringSplitOptions.RemoveEmptyEntries);
				if (fields.Length == 2)
				{
					dictionary.Add(fields[0], fields[1]);
				}
			}
			return dictionary;
		}
	}
}