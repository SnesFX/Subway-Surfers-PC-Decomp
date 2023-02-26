using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class StringUtility
{
	public enum MatchType
	{
		Is = 0,
		EndsWith = 1,
		BeginsWith = 2,
		Contains = 3,
		RegEx = 4,
		Pattern = 5
	}

	public static bool Match(string str, string pattern, MatchType matchType, bool ignoreCase)
	{
		switch (matchType)
		{
		case MatchType.Is:
			return str.Equals(pattern, (!ignoreCase) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
		case MatchType.EndsWith:
			return str.EndsWith(pattern, (!ignoreCase) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
		case MatchType.BeginsWith:
			return str.StartsWith(pattern, (!ignoreCase) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
		case MatchType.Contains:
			if (ignoreCase)
			{
				str = str.ToLower();
				pattern = pattern.ToLower();
			}
			return str.Contains(pattern);
		case MatchType.RegEx:
		{
			RegexOptions options = ((!ignoreCase) ? RegexOptions.Singleline : (RegexOptions.IgnoreCase | RegexOptions.Singleline));
			Regex regex2 = new Regex(pattern, options);
			return regex2.IsMatch(str);
		}
		case MatchType.Pattern:
		{
			Regex regex = new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			return regex.IsMatch(str);
		}
		default:
			return false;
		}
	}

	public static string ArrayToString<MyType>(MyType[] array)
	{
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			MyType val = array[i];
			if (flag)
			{
				flag = false;
			}
			else
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(val.ToString());
		}
		return stringBuilder.ToString();
	}

	public static void ValidateEnumValueString(Type enumType, string enumValueString, bool mayBeNullOrEmpty, string varName)
	{
		//Discarded unreachable code: IL_0025
		if (string.IsNullOrEmpty(enumValueString))
		{
			if (mayBeNullOrEmpty)
			{
				return;
			}
		}
		else
		{
			try
			{
				Enum.Parse(enumType, enumValueString, true);
				return;
			}
			catch (Exception)
			{
			}
		}
		Debug.LogError(varName + " must be one of: " + ArrayToString(Enum.GetNames(enumType)));
	}

	public static int NonEscapedIndexOf(string text, int startIndex, char ch)
	{
		int num = text.IndexOf(ch, startIndex);
		if (num == 0)
		{
			return num;
		}
		if (num > 0 && text[num - 1] != '\\')
		{
			return num;
		}
		return -1;
	}

	public static int GetNextKeyValuePair(string text, int startIndex, out string key, out string value)
	{
		int num = NonEscapedIndexOf(text, startIndex, '[');
		if (num >= 0)
		{
			int num2 = NonEscapedIndexOf(text, num + 1, ']');
			if (num2 >= 0)
			{
				key = text.Substring(num + 1, num2 - num - 1).Trim();
				int num3 = NonEscapedIndexOf(text, num2 + 1, '[');
				if (num3 < 0)
				{
					num3 = text.Length;
				}
				value = text.Substring(num2 + 1, num3 - num2 - 1).Trim();
				return num3;
			}
		}
		key = null;
		value = null;
		return -1;
	}

	public static Dictionary<string, string> ParseProperties(string text)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int num = 0;
		string key;
		string value;
		while ((num = GetNextKeyValuePair(text, num, out key, out value)) >= 0)
		{
			dictionary[key] = value;
			if (num == text.Length)
			{
				break;
			}
		}
		return dictionary;
	}

	public static string FormatProperties(Dictionary<string, string> props)
	{
		StringBuilder stringBuilder = new StringBuilder();
		char[] anyOf = new char[2] { '[', ']' };
		foreach (KeyValuePair<string, string> prop in props)
		{
			if (prop.Key.IndexOfAny(anyOf) >= 0)
			{
				Debug.LogError("Property data should not contain '[' and ']' characters");
			}
			stringBuilder.Append('[').Append(prop.Key).Append(']')
				.Append(prop.Value);
		}
		return stringBuilder.ToString();
	}
}
