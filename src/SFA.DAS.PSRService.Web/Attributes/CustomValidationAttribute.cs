using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Attributes
{
    public class CustomAnswerValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        private string _propertyName;

        public CustomAnswerValidationAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var queationType = ((SFA.DAS.PSRService.Web.ViewModels.QuestionViewModel)validationContext.ObjectInstance).Type;

            if (value != null)
            {
                switch (queationType)
                {
                    case QuestionType.Number:
                        int result = 0;
                        if (Int32.TryParse(value.ToString(), out result) == false)
                        { return new ValidationResult(GetErrorMessage(queationType)); }
                        break;
                    case QuestionType.ShortText:
                        if (value.ToString().Length > 100)
                        { return new ValidationResult(GetErrorMessage(queationType)); }
                        break;
                    case QuestionType.LongText:
                        if (value.ToString().Length > 250)
                        { return new ValidationResult(GetErrorMessage(queationType)); }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            


            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {

            if (context.ModelMetadata.ContainerType == typeof(QuestionViewModel))
            {


                var propertyValue = "Value";

                var property = context.MetadataProvider.GetMetadataForProperties(typeof(QuestionViewModel))
                    .FirstOrDefault(w => w.PropertyName == "Type");


                var index = int.Parse(context.Attributes["name"].Substring(1, 1));



                var model = ((Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<
                        SFA.DAS.PSRService.Web.ViewModels.SectionViewModel>)
                    ((Microsoft.AspNetCore.Mvc.Rendering.ViewContext) context.ActionContext).ViewData).Model;



                var queationType = model.Questions[index].Type;

                MergeAttribute(context.Attributes, "data-val", "true");

                switch (queationType)
                {
                    case QuestionType.Number:

                        MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(queationType));

                        break;

                    case QuestionType.LongText:
                        MergeAttribute(context.Attributes, "data-val-length", GetErrorMessage(queationType));
                        MergeAttribute(context.Attributes, "data-val-length-max", "250");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private string GetErrorMessage(QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.Number:

                    return  "Must be a number between 0 and 9";

                    break;

                case QuestionType.LongText:
                    return "Text Cannot be longer than 250 characters";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}