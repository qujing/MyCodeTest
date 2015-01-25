using MyCode.Exception;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyCode
{
	public static class ObjectMapper
	{

		public static TTo Map<TFrom, TTo>(TFrom from, TTo to)
		{
			return Map(from, to, new Dictionary<string, object>());
		}


		/// <summary>
		/// Maps properties of <paramref name="from"/> object into <paramref name="to"/> object.
		/// </summary>
		/// <typeparam name="TFrom">The type of <paramref name="from"/>.</typeparam>
		/// <typeparam name="TTo">The type of <paramref name="to"/>.</typeparam>
		/// <param name="from">An object mapped from.</param>
		/// <param name="to">An object mapped to.</param>
		/// <param name="explicitMappings">Explicit mappings.</param>
		/// <returns>Mapped <paramref name="to"/> object.</returns>
		public static TTo Map<TFrom, TTo>(TFrom from, TTo to, IDictionary<string, object> explicitMappings)
		{
			ArgumentNullExceptionManager.ArgumentNullExceptionThrow(from, "ObjectMapper(From)");
			ArgumentNullExceptionManager.ArgumentNullExceptionThrow(to, "ObjectMapper(To)");

			var sourceProps = from.GetType().GetProperties();

			foreach (var destinationProp in to.GetType().GetProperties().Where(p => p.CanWrite))
			{
				object destinationValue;

				// Apply implicit mapping if there is no explicit mapping.
				if (!explicitMappings.TryGetValue(destinationProp.Name, out destinationValue))
				{
					var sourceProp = sourceProps.FirstOrDefault(p => p.Name == destinationProp.Name);

					// Ignore if there is no such property in the source type.
					if (sourceProp == null)
						continue;

					// Get the source value.
					destinationValue = sourceProp.GetValue(from, null);

					// Apply implicit type conversion.
					if (sourceProp.PropertyType != destinationProp.PropertyType)
					{
						if (destinationProp.PropertyType == typeof(int))
							destinationValue = Convert.ToInt32(destinationValue, CultureInfo.InvariantCulture);
						else if (destinationProp.PropertyType == typeof(float))
							destinationValue = Convert.ToSingle(destinationValue, CultureInfo.InvariantCulture);
						else if (destinationProp.PropertyType == typeof(double))
							destinationValue = Convert.ToDouble(destinationValue, CultureInfo.InvariantCulture);
						else if (destinationProp.PropertyType == typeof(decimal))
							destinationValue = Convert.ToDecimal(destinationValue, CultureInfo.InvariantCulture);
						else if (destinationProp.PropertyType == typeof(DateTime))
							destinationValue = Convert.ToDateTime(destinationValue, CultureInfo.InvariantCulture);
					}
				}

				// Map the value to the destination type.
				destinationProp.SetValue(to, destinationValue, null);
			}

			return to;
		}
	}
}
