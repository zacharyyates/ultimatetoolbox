/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:03 PM
 */

using System;

namespace Toolbox
{
	public static class GuidExtensions
	{
		public static bool TryParse(string s, out Guid result)
		{
			try
			{
				result = new Guid(s);
				return true;
			}
			catch (Exception) // TODO: Handle specific exceptions?
			{
				result = Guid.Empty;
				return false;
			}
		}
	}
}