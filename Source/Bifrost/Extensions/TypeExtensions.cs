﻿#region License
//
// Copyright (c) 2008-2013, Dolittle (http://www.dolittle.com)
//
// Licensed under the MIT License (http://opensource.org/licenses/MIT)
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://github.com/dolittle/Bifrost/blob/master/MIT-LICENSE.txt
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System;
using System.Reflection;
using System.Linq;
using Bifrost.Concepts;

namespace Bifrost.Extensions
{
	/// <summary>
	/// Provides a set of methods for working with <see cref="Type">types</see>
	/// </summary>
	public static class TypeExtensions
	{
#pragma warning disable 1591 // Xml Comments
        static ITypeInfo GetTypeInfo(Type type)
        {
            var typeInfoType = typeof(TypeInfo<>).MakeGenericType(type);
#if(NETFX_CORE)
            return typeInfoType.GetRuntimeFields().Where(f => f.Name == "Instance" && f.IsStatic && f.IsPublic).Single().GetValue(null) as ITypeInfo;
#else
            return typeInfoType.GetField("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) as ITypeInfo;
#endif
        }
#pragma warning restore 1591 // Xml Comments

        /// <summary>
        /// Check if a type is nullable or not
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check</param>
        /// <returns>True if type is nullable, false if not</returns>
        public static bool IsNullable(this Type type)
        {
#if(NETFX_CORE)
            return (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
#else
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
#endif
        }

        /// <summary>
        /// Check if a type has a default constructor that does not take any arguments
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>true if it has a default constructor, false if not</returns>
        public static bool HasDefaultConstructor(this Type type)
        {
            return GetTypeInfo(type).HasDefaultConstructor;
        }


        /// <summary>
        /// Check if a type has a non default constructor
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>true if it has a non default constructor, false if not</returns>
        public static bool HasNonDefaultConstructor(this Type type)
        {
#if(NETFX_CORE)
            return type.GetTypeInfo().DeclaredConstructors.Any(c => c.GetParameters().Length > 0);
#else
            return type.GetConstructors().Any(c => c.GetParameters().Length > 0);
#endif
        }


        /// <summary>
        /// Get the default constructor from a type
        /// </summary>
        /// <param name="type">Type to get from</param>
        /// <returns>The default <see cref="ConstructorInfo"/></returns>
        public static ConstructorInfo GetDefaultConstructor(this Type type)
        {
#if(NETFX_CORE)
            return type.GetTypeInfo().DeclaredConstructors.Where(c => c.GetParameters().Length == 0).Single();
#else
            return type.GetConstructors().Where(c => c.GetParameters().Length == 0).Single();
#endif
        }

        /// <summary>
        /// Get the non default constructor, assuming there is only one
        /// </summary>
        /// <param name="type">Type to get from</param>
        /// <returns>The <see cref="ConstructorInfo"/> for the constructor</returns>
        public static ConstructorInfo GetNonDefaultConstructor(this Type type)
        {
#if(NETFX_CORE)
            return type.GetTypeInfo().DeclaredConstructors.Where(c => c.GetParameters().Length > 0).Single();
#else
            return type.GetConstructors().Where(c => c.GetParameters().Length > 0).Single();
#endif
        }


		/// <summary>
		/// Check if a type implements a specific interface
		/// </summary>
		/// <typeparam name="T">Interface to check for</typeparam>
		/// <param name="type">Type to check</param>
		/// <returns>True if the type implements the interface, false if not</returns>
		public static bool HasInterface<T>(this Type type)
		{
		    var hasInterface = type.HasInterface(typeof (T));
			return hasInterface;
		}

        /// <summary>
        /// Check if a type implements a specific interface
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="interfaceType">Interface to check for</param>
        /// <returns>True if the type implements the interface, false if not</returns>
        public static bool HasInterface(this Type type, Type interfaceType)
        {
#if(NETFX_CORE)
            var hasInterface = type.GetTypeInfo().ImplementedInterfaces.Where(t => t.FullName == interfaceType.FullName).Count() == 1;
#else
            var hasInterface = type.GetInterface(interfaceType.FullName, false) != null;
#endif
            return hasInterface;
        }

	}
}