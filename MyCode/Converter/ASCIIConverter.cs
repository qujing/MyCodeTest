using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Converter
{
	public class ASCIIConverter
	{
		/// <summary>
		/// Encodes non-US-ASCII characters in a string
		/// </summary>
		public static string ToHexString(string s)
		{
			char[] chars = s.ToCharArray();
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < chars.Length; i++)
			{
				bool needToEncode = NeedToEncode(chars[i]);
				if (needToEncode)
				{
					string encodedString = ToHexString(chars[i]);
					builder.Append(encodedString);
				}
				else
				{
					builder.Append(chars[i]);
				}
			}
			return builder.ToString();
		}

		/// <summary>
		///Determines if the character needs to be encoded.
		/// </summary>
		private static bool NeedToEncode(char chr)
		{
			string reservedChars = "$-_.+!*'(),@=&";

			if (chr > 127)
			{
				return true;
			}
			if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Encodes a non-US-ASCII character.
		/// </summary>
		private static string ToHexString(char chr)
		{
			UTF8Encoding utf8 = new UTF8Encoding();
			byte[] encodedBytes = utf8.GetBytes(chr.ToString());
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < encodedBytes.Length; i++)
			{
				builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[i], 16));
			}
			return builder.ToString();
		}
	}
}
