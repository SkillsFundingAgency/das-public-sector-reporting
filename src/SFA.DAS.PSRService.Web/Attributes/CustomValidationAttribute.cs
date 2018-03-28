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
      var questionType = ((SFA.DAS.PSRService.Web.ViewModels.QuestionViewModel)validationContext.ObjectInstance).Type;

      if (value != null)
      {
        switch (questionType)
        {
          case QuestionType.Number:
            int result = 0;
            if (Int32.TryParse(value.ToString(), out result) == false)
            { return new ValidationResult(GetErrorMessage(questionType)); }
            break;
          case QuestionType.ShortText:
            if (CountWords(value.ToString()) > 100)
            { return new ValidationResult(GetErrorMessage(questionType)); }
            break;
          case QuestionType.LongText:
            if (CountWords(value.ToString()) > 250)
            { return new ValidationResult(GetErrorMessage(questionType)); }
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
            MergeAttribute(context.Attributes, "data-word-limit", "250");
            MergeAttribute(context.Attributes, "data-val-maxwords", GetErrorMessage(questionType));
            MergeAttribute(context.Attributes, "data-val-maxwords-wordcount", "250");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public static int CountWords(string s)
    {
      int c = 0;
      for (int i = 1; i < s.Length; i++)
      {
        if (char.IsWhiteSpace(s[i - 1]) == true)
        {
          if (char.IsLetterOrDigit(s[i]) == true ||
              char.IsPunctuation(s[i]))
          {
            c++;
          }
        }
      }
      if (s.Length > 2)
      {
        c++;
      }
      return c;
    }

    private string GetErrorMessage(QuestionType questionType)
    {
      switch (questionType)
      {
        case QuestionType.Number:
          return "Must be a number between 0 and 9";
          break;
        case QuestionType.ShortText:
          return "Text cannot be longer than 100 words";
          break;
        case QuestionType.LongText:
          return "Text cannot be longer than 250 words";
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