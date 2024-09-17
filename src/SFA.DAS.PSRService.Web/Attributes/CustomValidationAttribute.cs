using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Attributes;

public class CustomAnswerValidationAttribute : ValidationAttribute, IClientModelValidator
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var questionType = ((QuestionViewModel)validationContext.ObjectInstance).Type;

        if (value == null)
        {
            return ValidationResult.Success;
        }

        switch (questionType)
        {
            case QuestionType.Number:
                if (!int.TryParse(value.ToString(), NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("en-GB"), out _))
                    return new ValidationResult(GetErrorMessage(questionType));
                break;

            case QuestionType.ShortText:
                if (CountWords(value.ToString()) > 100)
                    return new ValidationResult(GetErrorMessage(questionType));
                break;

            case QuestionType.LongText:
                if (CountWords(value.ToString()) > 500)
                    return new ValidationResult(GetErrorMessage(questionType));
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        if (context.ModelMetadata.ContainerType != typeof(QuestionViewModel))
        {
            return;
        }

        var index = int.Parse(context.Attributes["name"].Substring(1, 1));
        var model = ((Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<SectionViewModel>)
            ((Microsoft.AspNetCore.Mvc.Rendering.ViewContext)context.ActionContext).ViewData).Model;

        var questionType = model.Questions[index].Type;

        MergeAttribute(context.Attributes, "data-val", "true");

        switch (questionType)
        {
            case QuestionType.Number:
                MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(questionType));
                break;
            case QuestionType.ShortText:
                MergeAttribute(context.Attributes, "data-word-limit", "100");
                MergeAttribute(context.Attributes, "data-val-maxwords", GetErrorMessage(questionType));
                MergeAttribute(context.Attributes, "data-val-maxwords-wordcount", "100");
                break;
            case QuestionType.LongText:
                MergeAttribute(context.Attributes, "data-word-limit", "500");
                MergeAttribute(context.Attributes, "data-val-maxwords", GetErrorMessage(questionType));
                MergeAttribute(context.Attributes, "data-val-maxwords-wordcount", "500");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static int CountWords(string value)
    {
        var wordCount = 0;
        
        for (var index = 1; index < value.Length; index++)
        {
            if (!char.IsWhiteSpace(value[index - 1]))
            {
                continue;
            }

            if (char.IsLetterOrDigit(value[index]) || char.IsPunctuation(value[index]))
            {
                wordCount++;
            }
        }
        
        if (value.Length > 2)
        {
            wordCount++;
        }
        
        return wordCount;
    }

    private static string GetErrorMessage(QuestionType questionType)
    {
        return questionType switch
        {
            QuestionType.Number => "Must be a whole number",
            QuestionType.ShortText => "Text cannot be longer than 100 words",
            QuestionType.LongText => "Text cannot be longer than 500 words",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key))
        {
            return false;
        }

        attributes.Add(key, value);
        return true;
    }
}