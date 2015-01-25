using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Utils
{
	public static class EnumeratorUtils
	{
		public static bool IsNullOrEmpty<T>(IEnumerable<T> collection) 
		{
			return collection == null || !collection.Any();
		}
	}
}
