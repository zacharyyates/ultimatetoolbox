/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:28 PM
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Toolbox.Reflection
{
	//
	// TODO: Hopefully merge functionality with TypeExtensions
	//

	/// <summary>
	/// Reusable extension methods for common reflection operations
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// <see cref="CreateInstanceOfGenericType(String, Type[])"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="typeNames"></param>
		/// <returns></returns>
		public static object CreateInstanceOfGenericType(this String name, params string[] typeNames)
		{
			List<Type> types = new List<Type>();
			foreach (string typeName in typeNames)
			{
				types.Add(Type.GetType(typeName));
			}
			return CreateInstanceOfGenericType(name, types.ToArray());
		}

		/// <summary>
		/// <see cref="FindType(String)"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		/// <seealso cref="Type.MakeGenericType"/>
		/// <seealso cref="Activator.CreateInstance(Type)"/>
		public static object CreateInstanceOfGenericType(this String name, params Type[] types)
		{
			Trace.TraceInformation("Trying to create Type: {0}", name);
			String genericTypeName = String.Format("{0}{1}{2}",
				name, Convert.ToChar(96), types.Length); // name`number

			Type genericType = FindType(genericTypeName).MakeGenericType(types);
			return Activator.CreateInstance(genericType);
		}

		/// <summary>
		/// <see cref="FindChildTypes(Type)"/>
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		public static List<Type> FindChildTypes(this String typeName)
		{
			return FindChildTypes(Type.GetType(typeName));
		}

		/// <summary>
		/// Returns a collection of types that implement a given type in the same assembly
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <seealso cref="Assembly.GetAssembly(Type)"/>
		public static List<Type> FindChildTypes(this Type type)
		{
			// Find the source assembly
			Assembly assembly = Assembly.GetAssembly(type);
			var childTypes = (from t in assembly.GetTypes()
							  where
							   t.GetInterfaces().Contains<Type>(type)
							  select t).ToList();
			return childTypes;
		}

		/// <summary>
		/// Returns a resolved <see cref="Type"/> for a given <see cref="Type.FullName"/>
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		public static Type FindType(this String typeName)
		{
			// Check executing assembly
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			foreach (Type type in executingAssembly.GetExportedTypes())
			{
				Trace.TraceInformation("Found Type: " + type.FullName);
				if (type.FullName == typeName)
				{
					Trace.TraceInformation("Matched Type: " + typeName);
					return type;
				}
			}

			// Check referenced assemblies
			List<Assembly> referencedAssemblies = GetReferencedAssemblies(executingAssembly);
			foreach (Assembly assembly in referencedAssemblies)
			{
				foreach (Type type in assembly.GetExportedTypes())
				{
					Trace.TraceInformation("Found Type: " + type.FullName);
					if (type.FullName == typeName)
					{
						Trace.TraceInformation("Matched Type: " + typeName);
						return type;
					}
				}
			}
			return null; // No result found
		}

		/// <summary>
		/// Returns a collection of referenced assemblies for a given <see cref="Assembly"/>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <seealso cref="Assembly.Load(String)"/>
		/// <seealso cref="Assembly.GetReferencedAssemblies"/>
		public static List<Assembly> GetReferencedAssemblies(this Assembly assembly)
		{
			List<AssemblyName> referencedAssemblyNames = assembly.GetReferencedAssemblies().ToList<AssemblyName>();
			List<Assembly> referencedAssemblies = new List<Assembly>();
			referencedAssemblyNames.ForEach(
				delegate(AssemblyName assemblyName)
				{
					referencedAssemblies.Add(Assembly.Load(assemblyName));
				});
			return referencedAssemblies;
		}
	}
}
