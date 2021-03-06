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

namespace Bifrost.Commands
{
	/// <summary>
	/// The exception that is thrown when an expression is not a method call
	/// </summary>
    public class ExpressionNotMethodCallException : Exception
    {
		/// <summary>
		/// Initializes a new instance of <see cref="ExpressionNotMethodCallException"/>
		/// </summary>
		/// <param name="message">Message with details for the exception</param>
        public ExpressionNotMethodCallException(string message) : base(message) {}
    }
}