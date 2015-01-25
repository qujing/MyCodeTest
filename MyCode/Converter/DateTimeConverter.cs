using MyCode.Exception;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MyCode.Converter
{
	public class DateTimeConverter
	{
		public static DateTime ConvertDateTimeToUTC(DateTime dateTime, TimeZoneInfo timeZone)
		{
			ArgumentNullExceptionManager.ArgumentNullExceptionThrow(timeZone, "timeZone");

			if (dateTime.Kind == DateTimeKind.Local)
			{
				throw new ArgumentException("The DateTimeKind of datetime should be Unspecified");
			}

			if (!timeZone.IsInvalidTime(dateTime))
			{
				return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
			}

			var date = dateTime.Date;

			var adjustmentRule = timeZone.GetAdjustmentRules().
				FirstOrDefault(ar => ar.DateStart <= date && ar.DateEnd >= date);

			//dateTime.

			return DateTime.MinValue;
		}
	}
}
