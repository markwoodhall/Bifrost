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
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace Bifrost.Extensions
{
	/// <summary>
	/// Provides methods for working with expressions
	/// </summary>
	public static class ExpressionExtensions
	{
		/// <summary>
		/// Get <see cref="MethodInfo">MethodInfo</see> from an <see cref="Expression">expression</see> - if any
		/// </summary>
		/// <param name="expression"><see cref="Expression">Expression</see> to get MethodInfo from</param>
		/// <returns>The <see cref="MethodInfo">MethodInfo</see> found, null if did not find one</returns>
		public static MethodInfo GetMethodInfo(this Expression expression)
		{
			var lambda = expression as LambdaExpression;
			if (null != lambda &&
			    lambda.Body is MethodCallExpression)
			{
				var methodCall = lambda.Body as MethodCallExpression;
				return methodCall.Method;
			}
			return null;
		}

        /// <summary>
        /// Get all argument instances from a method expression
        /// </summary>
        /// <param name="expression"><see cref="Expression"/> to get argument instances from</param>
        /// <returns>Array of argument instances</returns>
        public static object[] GetMethodArguments(this Expression expression)
        {
			var lambda = expression as LambdaExpression;
            if (null != lambda &&
                lambda.Body is MethodCallExpression)
            {
                var methodCall = lambda.Body as MethodCallExpression;
                var arguments = new List<object>();

                foreach (var argument in methodCall.Arguments)
                {
                    var member = argument as MemberExpression;
                    var value = member.GetInstance();
                    arguments.Add(value);
                }

                return arguments.ToArray();
            }
            return new object[0];
        } 


		/// <summary>
		/// Get <see cref="MemberExpression">MemberExpression</see> from an <see cref="Expression">expression</see> - if any
		/// </summary>
		/// <param name="expression"><see cref="Expression">Expression</see> to get <see cref="MemberExpression">MemberExpression</see> from</param>
		/// <returns><see cref="MemberExpression">MemberExpression</see> instance, null if there is none</returns>
		public static MemberExpression GetMemberExpression(this Expression expression)
		{
			var lambda = expression as LambdaExpression;
			MemberExpression memberExpression;
			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = lambda.Body as UnaryExpression;
				memberExpression = unaryExpression.Operand as MemberExpression;
			}
			else
			{
				memberExpression = lambda.Body as MemberExpression;
			}
			return memberExpression;
		}

		/// <summary>
		/// Get <see cref="FieldInfo">FieldInfo</see> from an <see cref="Expression">Expression</see> - if any
		/// </summary>
		/// <param name="expression"><see cref="Expression">Expression</see> to get <see cref="FieldInfo">FieldInfo</see> from</param>
		/// <returns><see cref="FieldInfo">FieldInfo</see> instance, null if there is none</returns>
		public static FieldInfo GetFieldInfo(this Expression expression)
		{
			var memberExpression = GetMemberExpression(expression);
			var fieldInfo = memberExpression.Member as FieldInfo;
			return fieldInfo;
		}

		/// <summary>
		/// Get <see cref="PropertyInfo">PropertyInfo</see> from an <see cref="Expression">Expression</see> - if any
		/// </summary>
		/// <param name="expression"><see cref="Expression">Expression</see> to get <see cref="PropertyInfo">PropertyInfo</see> from</param>
		/// <returns><see cref="PropertyInfo">PropertyInfo</see> instance, null if there is none</returns>
		public static PropertyInfo GetPropertyInfo(this Expression expression)
		{
			var memberExpression = GetMemberExpression(expression);
			var propertyInfo = memberExpression.Member as PropertyInfo;
			return propertyInfo;
		}

		/// <summary>
		/// Get an instance reference from an <see cref="Expression">Expression</see> - if any
		/// </summary>
		/// <param name="expression"><see cref="Expression">Expression</see> to get an instance from</param>
		/// <returns>The instance, null if there is none</returns>
		public static object GetInstance(this Expression expression)
		{
			var memberExpression = GetMemberExpression(expression);
            return GetInstance(memberExpression);
			
		}

		/// <summary>
		/// Get an instance reference from an <see cref="Expression">Expression</see>, with a specific type - if any
		/// </summary>
		/// <typeparam name="T">Type of the instance</typeparam>
		/// <param name="expression"><see cref="Expression">Expression</see> to get an instance from</param>
		/// <returns>The instance, null if there is none</returns>
		public static T GetInstance<T>(this Expression expression)
		{
			return (T)GetInstance(expression);
		}

        static object GetInstance(this MemberExpression memberExpression)
        {
            var constantExpression = memberExpression.Expression as ConstantExpression;
            if (null == constantExpression)
            {
                var innerMember = memberExpression.Expression as MemberExpression;
                constantExpression = innerMember.Expression as ConstantExpression;
                return GetValue(innerMember, constantExpression);
            }
            return GetValue(memberExpression, constantExpression);
        }

        static object GetValue(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (memberExpression.Member is PropertyInfo)
            {
                var value = ((PropertyInfo)memberExpression.Member).GetValue(constantExpression.Value, null);
                return value;
            }
            if (memberExpression.Member is FieldInfo)
            {
                var value = ((FieldInfo)memberExpression.Member).GetValue(constantExpression.Value);
                return value;
            }

            return constantExpression.Value;
        }

	}
}