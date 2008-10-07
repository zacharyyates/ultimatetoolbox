/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 12:53 PM
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Toolbox.Reflection
{
	public static class AssemblyExtensions
	{
		/// <summary>
		/// Gets the types annotated with a custom <see cref="System.Attribute">Attribute</see> of type A
		/// </summary>
		/// <typeparam name="A">The type of the custom Attribute</typeparam>
		/// <param name="assembly">The <see cref="System.Reflection.Assembly">Assembly</see> to search</param>
		public static IEnumerable<Type> GetTypes<A>(this Assembly assembly)
			where A : Attribute
		{
			return from t in assembly.GetExportedTypes()
				   where t.GetCustomAttributes(typeof(A), true).Count() > 0
				   select t;
		}
	}
}