/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:10 PM
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Toolbox
{
	//
	// TODO: Remove crap, clean up /// comments
	//
	public static class TypeExtensions
	{
		/// <summary>
		/// Gets a list of the properties with a custom attribute declared on them
		/// </summary>
		/// <typeparam name="T">The type to check for custom attributes</typeparam>
		/// <typeparam name="A">The type of the custom attribute</typeparam>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attribute</param>
		public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<T, A>(bool inherit)
			where T : class
			where A : Attribute
		{
			return GetPropertiesWithCustomAttribute<A>(typeof(T), inherit);
		}
		/// <summary>
		/// Gets a list of the properties with a custom attribute declared on them
		/// </summary>
		/// <typeparam name="A">The type of the custom attribute</typeparam>
		/// <param name="type">The type to check for custom attributes</param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attribute</param>
		/// <returns></returns>
		public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<A>(this Type type, bool inherit)
			where A : Attribute
		{
			return from p in type.GetProperties()
				   from a in p.GetCustomAttributes(typeof(A), inherit)
				   select p;
		}

		/// <summary>
		/// Gets a custom attributes of type A on the specified property
		/// </summary>
		/// <typeparam name="T">The type to check for custom attributes</typeparam>
		/// <typeparam name="A">The type of the custom attribute</typeparam>
		/// <param name="propertyName">The name of the property</param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attribute</param>
		public static IEnumerable<A> GetCustomAttributesOnProperty<T, A>(string propertyName, bool inherit)
			where T : class
			where A : Attribute
		{
			return GetCustomAttributesOnProperty<A>(typeof(T), propertyName, inherit);
		}
		/// <summary>
		/// Gets a custom attributes of type A on the specified property
		/// </summary>
		/// <typeparam name="A">The type of the custom attribute</typeparam>
		/// <param name="type">The type to find the property on</param>
		/// <param name="propertyName">The name of the property</param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attribute</param>
		public static IEnumerable<A> GetCustomAttributesOnProperty<A>(this Type type, string propertyName, bool inherit)
			where A : Attribute
		{
			return from p in type.GetProperties()
				   from a in p.GetCustomAttributes(typeof(A), true).Cast<A>()
				   where p.Name.Equals(propertyName)
				   select a;
		}

		/// <summary>
		/// Gets the first custom attribute that matches A on type
		/// </summary>
		/// <typeparam name="A">The type of the attribute to find</typeparam>
		/// <param name="type">The Type to search</param>
		public static A GetCustomAttribute<A>(this Type type)
			where A : Attribute
		{
			return type.GetCustomAttributes(typeof(A), true).FirstOrDefault() as A;
		}

		/// <summary>
		/// Returns all of the types that derive from type or implement type
		/// </summary>
		/// <param name="type">The parent type</param>
		public static IEnumerable<Type> GetSubTypes(this Type type)
		{
			var assembly = Assembly.GetAssembly(type);
			return from t in assembly.GetExportedTypes()
				   where
					t.BaseType == type ||
					t.GetInterfaces().Contains(type)
				   select t;
		}

		/// <summary>
		/// Gets a concrete interface implementation that matches the ITypeName naming scheme
		/// </summary>
		/// <param name="type">The interface type</param>
		public static Type GetConcreteType(this Type type)
		{
			var assembly = Assembly.GetAssembly(type);
			return (from t in assembly.GetExportedTypes()
					where
					 (t.BaseType == type ||
					  t.GetInterfaces().Contains(type)) &&
					 (t.Name.Equals(type.Name.Replace("I", ""))) // Only get classes that are like 'TypeName' where type.Name = 'ITypeName'
					select t)
					.FirstOrDefault();
		}
	}
}