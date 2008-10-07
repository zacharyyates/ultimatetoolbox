/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:01 PM
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Toolbox
{
	//
	// TODO: Clean up this class, remove references to BAE.ECast
	//
	public static class EnumExtensions
	{
		/// <summary>
		/// Gets the string representation of Enum values
		/// </summary>
		public static IEnumerable<string> GetNames(this Enum me)
		{
			return Enum.GetNames(me.GetType()).ToList();
		}

		static string GetLocalStringFor(Enum me, string value, string resourceBaseName, Assembly assembly)
		{
			ResourceManager manager = new ResourceManager(resourceBaseName, assembly);
			string name = string.Format("Enum_{0}_{1}", me.GetType().Name, value);
			string local = manager.GetString(name);
			return (!string.IsNullOrEmpty(local) ? local : value);
		}

		/// <summary>
		/// Returns the friendly name for this enum value if it exists in the resource
		/// </summary>
		public static string ToLocalString(this Enum me)
		{
			return ToLocalString(me, "BAE.ECast.DataComponents.EnumStrings");
		}
		/// <summary>
		/// Returns the friendly name for this enum value if it exists in the resource
		/// </summary>
		public static string ToLocalString(this Enum me, string resourceBaseName)
		{
			return ToLocalString(me, resourceBaseName, Assembly.GetExecutingAssembly()); // TODO: Maybe Assembly.GetAssembly(Type type) is better?
		}
		/// <summary>
		/// Returns the friendly name for this enum value if it exists in the resource
		/// </summary>
		public static string ToLocalString(this Enum me, string resourceBaseName, Assembly assembly)
		{
			return GetLocalStringFor(me, me.ToString(), resourceBaseName, assembly);
		}

		/// <summary>
		/// Gets all the localized friendly string values of the Enum
		/// </summary>
		public static IEnumerable<string> GetLocalNames(this Enum me)
		{
			return GetLocalNames(me, "BAE.ECast.DataComponents.EnumStrings");
		}
		/// <summary>
		/// Gets all the localized friendly string values of the Enum
		/// </summary>
		/// <param name="resourceBaseName">The name of the Resource class to retrieve the values from</param>
		public static IEnumerable<string> GetLocalNames(this Enum me, string resourceBaseName)
		{
			return GetLocalNames(me, resourceBaseName, Assembly.GetExecutingAssembly());
		}
		/// <summary>
		/// Gets all the localized friendly string values of the Enum
		/// </summary>
		/// <param name="resourceBaseName">The name of the Resource class to retrieve the values from</param>
		/// <param name="assembly">The assembly of the Resource class</param>
		public static IEnumerable<string> GetLocalNames(this Enum me, string resourceBaseName, Assembly assembly)
		{
			ResourceManager manager = new ResourceManager(resourceBaseName, assembly);
			var values = GetNames(me);
			foreach (string value in values)
			{
				string local = manager.GetString(string.Format("Enum_{0}_{1}", me.GetType().Name, value));
				yield return !string.IsNullOrEmpty(local) ? local : value;
			}
		}

		/// <summary>
		/// Gets all the localized friendly values of the Enum, plus the original values
		/// </summary>
		/// <typeparam name="T">The type fo the Enum</typeparam>
		public static IEnumerable<LocalizedEnumValue<T>> GetLocalNames<T>(this Enum me)
		{
			return GetLocalNames<T>(me, "BAE.ECast.DataComponents.EnumStrings");
		}
		/// <summary>
		/// Gets all the localized friendly values of the Enum, plus the original values
		/// </summary>
		/// <param name="resourceBaseName">The name of the Resource class to retrieve the values from</param>
		/// <typeparam name="T">The type fo the Enum</typeparam>
		public static IEnumerable<LocalizedEnumValue<T>> GetLocalNames<T>(this Enum me, string resourceBaseName)
		{
			return GetLocalNames<T>(me, resourceBaseName, Assembly.GetExecutingAssembly());
		}
		/// <summary>
		/// Gets all the localized friendly values of the Enum, plus the original values
		/// </summary>
		/// <param name="resourceBaseName">The name of the Resource class to retrieve the values from</param>
		/// <param name="assembly">The assembly of the Resource class</param>
		/// <typeparam name="T">The type fo the Enum</typeparam>
		public static IEnumerable<LocalizedEnumValue<T>> GetLocalNames<T>(this Enum me, string resourceBaseName, Assembly assembly)
		{
			ResourceManager manager = new ResourceManager(resourceBaseName, assembly);
			var values = GetNames(me);
			foreach (string value in values)
			{
				string local = manager.GetString(string.Format("Enum_{0}_{1}", me.GetType().Name, value));
				yield return new LocalizedEnumValue<T>()
				{
					Value = (T)Enum.Parse(me.GetType(), value),
					LocalName = !string.IsNullOrEmpty(local) ? local : value,
					Name = value
				};
			}
		}

		/// <summary>
		/// Contains the Localized name of an Enum value and the original value
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public struct LocalizedEnumValue<T>
		{
			public T Value { get; set; }
			public string LocalName { get; set; }
			public string Name { get; set; }
		}
	}
}