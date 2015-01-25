using MyCode.Exception.Resources;
using System;

namespace MyCode.Exception
{
	public static class ArgumentNullExceptionManager
	{
		public static void ArgumentNullExceptionThrow(object value, string argumentName)
		{
			if (value != null)
			{
				return;
			}

			var keyString = string.Format(ExceptionMessages.COCO0001E, argumentName);
			throw new ArgumentNullException(keyString);
		}
	}
}
