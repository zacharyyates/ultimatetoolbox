/* Zachary Yates
 * Copyright © 2008 YatesMorrison Software Company
 * 10/7/2008 1:30 PM
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Toolbox.Linq
{
	//
	// TODO: Convert this to an interface and return a different implementation based on the Linq provider
	// TODO: Cleanup and merge functionality
	//
	/// <summary>
	/// Hierarchy node class which contains a nested collection of hierarchy nodes
	/// </summary>
	/// <typeparam name="T">Entity</typeparam>
	public class HierarchyNode<T> where T : class
	{
		public T Entity { get; set; }
		public IEnumerable<HierarchyNode<T>> ChildNodes { get; set; }
		public int Depth { get; set; }
		public T Parent { get; set; }
	}

	public static class LinqExtensionMethods
	{
		//#region HierarchyLinqToObjectExtensions

		///// <summary>
		///// Custom iterator to walk the given list of entities and construct the HierarchyNode tree
		///// </summary>
		///// <typeparam name="TEntity"></typeparam>
		///// <typeparam name="TProperty"></typeparam>
		///// <param name="allItems"></param>
		///// <param name="parentItem"></param>
		///// <param name="idProperty"></param>
		///// <param name="parentIdProperty"></param>
		///// <param name="rootItemId"></param>
		///// <param name="maxDepth"></param>
		///// <param name="depth"></param>
		///// <returns></returns>
		//private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity, TProperty>(
		//        IEnumerable<TEntity> allItems, 
		//        TEntity parentItem, 
		//        Func<TEntity, TProperty> idProperty, 
		//        Func<TEntity, TProperty> parentIdProperty,
		//        object rootItemId, 
		//        int maxDepth, 
		//        int depth,
		//        IList<TEntity> previousChildren) 
		//    where TEntity : class
		//{
		//    IEnumerable<TEntity> childs;

		//    if (rootItemId != null) 
		//    {
		//      childs = allItems.Where(i => idProperty(i).Equals(rootItemId));
		//    }
		//    else
		//    {
		//        if (parentItem == null)
		//        {
		//            childs = allItems.Where(i => parentIdProperty(i).Equals(default(TEntity)));
		//        }
		//        else
		//        {
		//            childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));
		//        }
		//    }

		//    if ((childs != null) && childs.Count() > 0)
		//    {
		//        depth++;

		//        if ((depth <= maxDepth) || (maxDepth == 0))
		//        {
		//            foreach (var item in childs)
		//            {
		//                if (!previousChildren.Contains(item))
		//                {
		//                    previousChildren.Add(item);
		//                    yield return new HierarchyNode<TEntity>()
		//                                    {
		//                                        Entity = item,
		//                                        ChildNodes = CreateHierarchy(allItems, item, idProperty, parentIdProperty, null, maxDepth, depth, previousChildren),
		//                                        Depth = depth,
		//                                        Parent = parentItem
		//                                    };
		//                }
		//            }
		//        }
		//    }
		//}

		///// <summary>
		///// LINQ to Objects (IEnumerable) AsHierachy() extension method
		///// </summary>
		///// <typeparam name="TEntity">Entity class</typeparam>
		///// <typeparam name="TProperty">Property of entity class</typeparam>
		///// <param name="allItems">Flat collection of entities</param>
		///// <param name="idProperty">Func delegete to Id/Key of entity</param>
		///// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
		///// <returns>Hierarchical structure of entities</returns>
		//public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		//        this IEnumerable<TEntity> allItems, 
		//        Func<TEntity, TProperty> idProperty, 
		//        Func<TEntity, TProperty> parentIdProperty) 
		//    where TEntity : class
		//{
		//    return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, null, 0, 0, new List<TEntity>());
		//}

		///// <summary>
		///// LINQ to Objects (IEnumerable) AsHierachy() extension method
		///// </summary>
		///// <typeparam name="TEntity">Entity class</typeparam>
		///// <typeparam name="TProperty">Property of entity class</typeparam>
		///// <param name="allItems">Flat collection of entities</param>
		///// <param name="idProperty">Func delegete to Id/Key of entity</param>
		///// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
		///// <param name="rootItemId">Value of root item Id/Key</param>
		///// <returns>Hierarchical structure of entities</returns>
		//public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		//        this IEnumerable<TEntity> allItems, 
		//        Func<TEntity, TProperty> idProperty, 
		//        Func<TEntity, TProperty> parentIdProperty, 
		//        object rootItemId) 
		//    where TEntity : class
		//{
		//    return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, 0, 0, new List<TEntity>());
		//}

		///// <summary>
		///// LINQ to Objects (IEnumerable) AsHierachy() extension method
		///// </summary>
		///// <typeparam name="TEntity">Entity class</typeparam>
		///// <typeparam name="TProperty">Property of entity class</typeparam>
		///// <param name="allItems">Flat collection of entities</param>
		///// <param name="idProperty">Func delegete to Id/Key of entity</param>
		///// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
		///// <param name="rootItemId">Value of root item Id/Key</param>
		///// <param name="maxDepth">Maximum depth of tree</param>
		///// <returns>Hierarchical structure of entities</returns>
		//public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
		//        this IEnumerable<TEntity> allItems, 
		//        Func<TEntity, TProperty> idProperty, 
		//        Func<TEntity, TProperty> parentIdProperty, 
		//        object rootItemId, 
		//        int maxDepth) 
		//    where TEntity : class
		//{
		//    return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, maxDepth, 0, new List<TEntity>());
		//}

		///// <summary>
		///// Flatten the Hierarchy and strip off the HierarchyNode's <see cref="FindType(HierarchyNode)"/>
		///// </summary>
		///// <typeparam name="TEntity"></typeparam>
		///// <param name="hierarchy"></param>
		///// <returns></returns>
		//public static System.Collections.Generic.IEnumerable<TEntity> ToList<TEntity>(this System.Collections.Generic.IEnumerable<HierarchyNode<TEntity>> hierarchy)
		//    where TEntity : class
		//{
		//    List<TEntity> list = new List<TEntity>();
		//    if ((hierarchy != null) && (hierarchy.Count() > 0))
		//    {
		//        foreach (var item in hierarchy)
		//        {
		//            list.Add(item.Entity);
		//            list.AddRange(item.ChildNodes.ToList());
		//        }
		//    }
		//    return list;
		//}

		///// <summary>
		///// count the HierarchyNode's <see cref="FindType(HierarchyNode)"/>
		///// </summary>
		///// <typeparam name="TEntity"></typeparam>
		///// <param name="hierarchy"></param>
		///// <returns></returns>
		//public static long Count<TEntity>(this System.Collections.Generic.IEnumerable<HierarchyNode<TEntity>> hierarchy)
		//    where TEntity : class
		//{
		//    long count = 0;
		//    if (hierarchy != null)
		//    {
		//        foreach (var item in hierarchy)
		//        {
		//            if (item != null)
		//            {
		//                count++;
		//                count += item.ChildNodes.Count();
		//            }
		//            else
		//                break;
		//        }
		//    }
		//    return count;
		//}
		//#endregion

		//#region HierarchyLinqToIQueryableExtensions

		///// <summary>
		///// Custom iterator for creating an HierarchyNode structure from an IQueryable 
		///// </summary>
		///// <typeparam name="TEntity"></typeparam>
		///// <param name="allItems"></param>
		///// <param name="parentItem"></param>
		///// <param name="propertyNameId"></param>
		///// <param name="propertyNameParentId"></param>
		///// <param name="rootItemId"></param>
		///// <param name="maxDepth"></param>
		///// <param name="depth"></param>
		///// <param name="previousChildren">The previous children returned by this hierarchical call, protects against stack overflow</param>
		///// <returns></returns>
		//private static IEnumerable<HierarchyNode<TEntity>>
		//  CreateHierarchy<TEntity>(IQueryable<TEntity> allItems,
		//    TEntity parentItem,
		//    string propertyNameId,
		//    string propertyNameParentId,
		//    object rootItemId,
		//    int maxDepth,
		//    int depth,
		//    IList<TEntity> previousChildren) where TEntity : class
		//{
		//    ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
		//    Expression<Func<TEntity, bool>> predicate;

		//    if (rootItemId != null)
		//    {
		//        Expression left = Expression.Property(parameter, propertyNameId);
		//        left = Expression.Convert(left, rootItemId.GetType());
		//        Expression right = Expression.Constant(rootItemId);

		//        predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
		//    }
		//    else
		//    {
		//        if (parentItem == null)
		//        {
		//            predicate =
		//              Expression.Lambda<Func<TEntity, bool>>(
		//                Expression.Equal(Expression.Property(parameter, propertyNameParentId),
		//                                 Expression.Constant(null)), parameter);
		//        }
		//        else
		//        {
		//            Expression left = Expression.Property(parameter, propertyNameParentId);
		//            left = Expression.Convert(left, parentItem.GetType().GetProperty(propertyNameId).PropertyType);
		//            Expression right = Expression.Constant(parentItem.GetType().GetProperty(propertyNameId).GetValue(parentItem, null));

		//            predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
		//        }
		//    }

		//    IEnumerable<TEntity> childs = allItems.Where(predicate).ToList();

		//    if (childs.Count() > 0)
		//    {
		//        depth++;

		//        if ((depth <= maxDepth) || (maxDepth == 0))
		//        {
		//            foreach (var item in childs)
		//            {
		//                if (!previousChildren.Contains(item))
		//                {
		//                    previousChildren.Add(item);
		//                    yield return
		//                      new HierarchyNode<TEntity>()
		//                      {
		//                          Entity = item,
		//                          ChildNodes =
		//                            CreateHierarchy(allItems, item, propertyNameId, propertyNameParentId, null, maxDepth, depth, previousChildren),
		//                          Depth = depth,
		//                          Parent = parentItem
		//                      };
		//                }
		//            }
		//        }
		//    }
		//}

		///// <summary>
		///// LINQ to SQL (IQueryable) AsHierachy() extension method
		///// </summary>
		///// <typeparam name="TEntity">Entity class</typeparam>
		///// <param name="allItems">Flat collection of entities</param>
		///// <param name="propertyNameId">String with property name of Id/Key</param>
		///// <param name="propertyNameParentId">String with property name of parent Id/Key</param>
		///// <returns>Hierarchical structure of entities</returns>
		//public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
		//  this IQueryable<TEntity> allItems,
		//  string propertyNameId,
		//  string propertyNameParentId) where TEntity : class
		//{
		//    return CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, null, 0, 0, new List<TEntity>());
		//}

		///// <summary>
		///// LINQ to SQL (IQueryable) AsHierachy() extension method
		///// </summary>
		///// <typeparam name="TEntity">Entity class</typeparam>
		///// <param name="allItems">Flat collection of entities</param>
		///// <param name="propertyNameId">String with property name of Id/Key</param>
		///// <param name="propertyNameParentId">String with property name of parent Id/Key</param>
		///// <param name="rootItemId">Value of root item Id/Key</param>
		///// <returns>Hierarchical structure of entities</returns>
		//public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
		//  this IQueryable<TEntity> allItems,
		//  string propertyNameId,
		//  string propertyNameParentId,
		//  object rootItemId) where TEntity : class
		//{
		//    return CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, rootItemId, 0, 0, new List<TEntity>());
		//}

		///// <summary>
		///// LINQ to SQL (IQueryable) AsHierachy() extension method
		///// </summary>
		///// <typeparam name="TEntity">Entity class</typeparam>
		///// <param name="allItems">Flat collection of entities</param>
		///// <param name="propertyNameId">String with property name of Id/Key</param>
		///// <param name="propertyNameParentId">String with property name of parent Id/Key</param>
		///// <param name="rootItemId">Value of root item Id/Key</param>
		///// <param name="maxDepth">Maximum depth of tree</param>
		///// <returns>Hierarchical structure of entities</returns>
		//public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
		//  this IQueryable<TEntity> allItems,
		//  string propertyNameId,
		//  string propertyNameParentId,
		//  object rootItemId,
		//  int maxDepth) where TEntity : class
		//{
		//    return CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, rootItemId, maxDepth, 0, new List<TEntity>());
		//}
		//#endregion

		#region Zachary's Hackery

		public static IEnumerable<TEntity> ToList<TEntity>(this HierarchyNode<TEntity> node)
			where TEntity : class
		{
			List<TEntity> list = new List<TEntity>();
			BuildList(node, list);
			return list;
		}
		static void BuildList<TEntity>(HierarchyNode<TEntity> start, List<TEntity> list)
			where TEntity : class
		{
			list.Add(start.Entity);
			foreach (var child in start.ChildNodes)
			{
				BuildList<TEntity>(child, list);
			}
		}

		public static HierarchyNode<TEntity> SelectHierarchy<TEntity, TProperty>(
			this IQueryable<TEntity> queryable,
			TEntity parent,
			string parentProperty,
			string childProperty)
			where TEntity : class
		{
			return SelectHierarchy<TEntity, TProperty>(queryable, parent, parentProperty, childProperty, 0);
		}

		public static HierarchyNode<TEntity> SelectHierarchy<TEntity, TProperty>(
			this IQueryable<TEntity> queryable,
			TEntity parent,
			string parentProperty,
			string childProperty,
			int maxDepth)
			where TEntity : class
		{
			return SelectHierarchy<TEntity, TProperty>(queryable, parent, parentProperty, childProperty, new List<TEntity>(), 0, maxDepth);
		}

		static HierarchyNode<TEntity> SelectHierarchy<TEntity, TProperty>(
			this IQueryable<TEntity> queryable,
			TEntity parent,
			string parentProperty,
			string childProperty,
			IList<TEntity> ancestry,
			int currentDepth,
			int maxDepth)
			where TEntity : class
		{
			HierarchyNode<TEntity> node = null;
			if (parent != null)
			{
				currentDepth++;

				node = new HierarchyNode<TEntity>();
				node.Entity = parent;
				node.ChildNodes = new List<HierarchyNode<TEntity>>();

				if (currentDepth <= maxDepth || maxDepth == 0)
				{
					ancestry.Add(parent);
					// Create child query
					ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
					Expression left = Expression.Property(parameter, childProperty);
					Expression right = Expression.Constant(parent.GetPropertyValue(parentProperty), typeof(TProperty));
					Expression equals = Expression.Equal(left, right);
					Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

					// The ToList() call is necessary to evaluate the SQL part, the Except() call is handled in memory
					var children = queryable.Where(lambda).ToList().Except(ancestry);
					foreach (var child in children)
					{
						// Recursion
						var newChild = SelectHierarchy<TEntity, TProperty>(
							queryable, child, parentProperty, childProperty, ancestry, currentDepth, maxDepth);
						newChild.Parent = parent;
						(node.ChildNodes as List<HierarchyNode<TEntity>>).Add(newChild);
					}
				}
			}
			return node;
		}

		/// <summary>
		/// Returns the value of an objects property
		/// </summary>
		/// <param name="obj">The object</param>
		/// <param name="name">The name of the property</param>
		/// <returns></returns>
		public static object GetPropertyValue(this object obj, string name)
		{
			PropertyInfo info = obj.GetType().GetProperty(name);
			if (info != null)
			{
				return info.GetValue(obj, null);
			}
			return null;
		}
		/// <summary>
		/// Returns the value of an objects property
		/// </summary>
		/// <param name="obj">The object</param>
		/// <param name="name">The name of the property</param>
		/// <param name="index">The index if it is an indexed property</param>
		/// <returns></returns>
		public static object GetPropertyValue(this object obj, string name, object[] index)
		{
			PropertyInfo info = obj.GetType().GetProperty(name);
			if (info != null)
			{
				return info.GetValue(obj, index);
			}
			return null;
		}

		#endregion
	}
}