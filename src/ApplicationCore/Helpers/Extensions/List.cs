using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers
{
	public static class ListHelpers
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			var collection = enumerable as ICollection<T>;
			if (collection != null)
			{
				return collection.Count < 1;
			}
			return !enumerable.Any();
		}

		public static bool HasItems<T>(this IEnumerable<T> enumerable) => !IsNullOrEmpty(enumerable);

		

		public static IEnumerable<T> AddIfNotExists<T>(this IEnumerable<T> list, T value)
			=> list.Contains(value) ? list : list.Append(value);


	}
}
