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
using System.Collections.Generic;
using System.Linq;
using Bifrost.Commands;
using Bifrost.Execution;
using Bifrost.Extensions;
#if(NETFX_CORE)
using System.Reflection;
#endif

namespace Bifrost.Validation
{
    /// <summary>
    /// Represents an instance of an <see cref="ICommandValidatorProvider">ICommandValidatorProvider.</see>
    /// </summary>
    [Singleton]
    public class CommandValidatorProvider : ICommandValidatorProvider
    {
        static ICommandInputValidator NullInputValidator = new NullCommandInputValidator();
        static ICommandBusinessValidator NullBusinessValidator = new NullCommandBusinessValidator();
        static Type _inputValidatorType = typeof (ICommandInputValidator);
        static Type _businessValidatorType = typeof (ICommandBusinessValidator);
        static Type _validatesType = typeof (ICanValidate<>);

        ITypeDiscoverer _typeDiscoverer;
        IContainer _container;

        Dictionary<Type, Type> _inputValidators;
        Dictionary<Type, Type> _businessValidators;

        /// <summary>
        /// Initializes an instance of <see cref="CommandValidatorProvider"/> CommandValidatorProvider
        /// </summary>
        /// <param name="typeDiscoverer">
        /// An instance of ITypeDiscoverer to help identify and register <see cref="ICommandInputValidator"/> implementations
        /// and  <see cref="ICommandBusinessValidator"/> implementations
        /// </param>
        /// <param name="container">An instance of <see cref="IContainer"/> to manage instances of any <see cref="ICommandInputValidator"/></param>
        public CommandValidatorProvider(ITypeDiscoverer typeDiscoverer, IContainer container)
        {
            _typeDiscoverer = typeDiscoverer;
            _container = container;

            Initialize();
        }

#pragma warning disable 1591 // Xml Comments
        public ICanValidate GetInputValidatorFor(ICommand command)
        {
            return GetInputValidatorFor(command.GetType());
        }

        public ICanValidate GetBusinessValidatorFor(ICommand command)
        {
            return GetBusinessValidatorFor(command.GetType());
        }

        public ICanValidate GetInputValidatorFor(Type type)
        {
            Type registeredType;
            _inputValidators.TryGetValue(type, out registeredType);

            var inputValidator = (registeredType != null ? _container.Get(registeredType) : NullInputValidator) as ICanValidate;
            return inputValidator;
        }

        public ICanValidate GetBusinessValidatorFor(Type type)
        {
            Type registeredType;
            _businessValidators.TryGetValue(type, out registeredType);

            var businessValidator = (registeredType != null ? _container.Get(registeredType) : NullBusinessValidator) as ICanValidate;
            return businessValidator;
        }
#pragma warning restore 1591 // Xml Comments

        /// <summary>
        /// Gets a list of registered input validator types
        /// </summary>
        public IEnumerable<Type> RegisteredInputValidators
        {
            get { return _inputValidators.Values; }
        }

        /// <summary>
        ///  Gets a list of registered business validator types
        /// </summary>
        public IEnumerable<Type> RegisteredBusinessValidators
        {
            get { return _businessValidators.Values; }
        }


        void Initialize()
        {
            _inputValidators = new Dictionary<Type, Type>();
            _businessValidators = new Dictionary<Type, Type>();

            var inputValidators = _typeDiscoverer.FindMultiple(_inputValidatorType);
            var businessValidators = _typeDiscoverer.FindMultiple(_businessValidatorType);

            inputValidators.ForEach(type => Register(type, _inputValidatorType));
            businessValidators.ForEach(type => Register(type, _businessValidatorType));
        }

        void Register(Type typeToRegister, Type registerFor)
        {
            var validatorRegistry = registerFor == _inputValidatorType
                                        ? _inputValidators
                                        : _businessValidators;

            var commandType = GetCommandType(typeToRegister);

            if (commandType == null || 
#if(NETFX_CORE)
                commandType.GetTypeInfo().IsInterface ||
#else
                commandType.IsInterface ||
#endif
                validatorRegistry.ContainsKey(commandType))
                return;

            validatorRegistry.Add(commandType, typeToRegister);
        }

        Type GetCommandType(Type typeToRegister)
        {
            var types = from interfaceType in typeToRegister
#if(NETFX_CORE)
                                    .GetTypeInfo().ImplementedInterfaces
#else
                                    .GetInterfaces()
#endif
                        where interfaceType
#if(NETFX_CORE)
                                    .GetTypeInfo().IsGenericType
#else
                                    .IsGenericType
#endif
                        let baseInterface = interfaceType.GetGenericTypeDefinition()
                        where baseInterface == _validatesType
                        select interfaceType
#if(NETFX_CORE)
                                    .GetTypeInfo().GenericTypeParameters
#else
                                    .GetGenericArguments()
#endif
                            .FirstOrDefault();

            return types.FirstOrDefault();
        }
    }
}