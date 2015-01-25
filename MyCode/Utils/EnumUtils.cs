using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyCode.Utils
{
	public class EnumUtils
	{
		/// <summary>
		/// Gets values of an enum that is specified by generic type parameter.
		/// </summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <returns>All the values of <typeparamref name="T"/>.</returns>
		/// <exception cref="ArgumentException">
		/// <typeparamref name="T"/> is not an enum type.
		/// </exception>
		/// <remarks>
		/// Since the order of return values is undefined,
		/// your code must not depend on the order.
		/// </remarks>
		public static IEnumerable<T> GetValues<T>()
		{
			try
			{
				return ValueCache<T>.Values;
			}
			catch (TypeInitializationException ex)
			{
				throw new ArgumentException("Type parameter T should be an enum", ex);
			}
		}

		private class ValueCache<T>
		{
			static ValueCache()
			{
				Type type = typeof(T);

				if (!type.IsEnum)
				{
					throw new ArgumentException("Type parameter T should be an enum");
				}

				var fields = type.GetFields().Where(fi => fi.IsLiteral);
				Values = new ReadOnlyCollection<T>(fields.Select(fi => (T)fi.GetValue(type)).ToList());
			}

			public static ReadOnlyCollection<T> Values;
		}
	}
}
