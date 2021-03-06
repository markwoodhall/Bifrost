#region License
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
using Bifrost.Commands;
using Bifrost.Validation;
using Bifrost.Validation.MetaData;
using FluentValidation;
using Bifrost.Serialization;

namespace Bifrost.Web.Validation
{
    public class ValidationService
    {
        ICommandTypeManager _commandTypeManager;
        ICommandValidatorProvider _commandValidatorProvider;
        IValidationMetaDataGenerator _validationMetaDataGenerator;
        ISerializer _serializer;

        public ValidationService(
            ICommandTypeManager commandTypeManager,
            ICommandValidatorProvider commandValidatorProvider, 
            IValidationMetaDataGenerator validationMetaDataGenerator,
            ISerializer serializer)
        {
            _commandTypeManager = commandTypeManager;
            _commandValidatorProvider = commandValidatorProvider;
            _validationMetaDataGenerator = validationMetaDataGenerator;
            _serializer = serializer;
        }

        public ValidationMetaData GetForCommand(string name)
		{
            var commandType = _commandTypeManager.GetFromName(name);
            var inputValidator = _commandValidatorProvider.GetInputValidatorFor(commandType) as IValidator;
            if (inputValidator != null)
            {
                var metaData = _validationMetaDataGenerator.GenerateFrom(inputValidator);
                return metaData;
            }
            return new ValidationMetaData();
		}
    }
}

