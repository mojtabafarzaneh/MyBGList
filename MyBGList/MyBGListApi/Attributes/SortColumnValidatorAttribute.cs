﻿using System.ComponentModel.DataAnnotations;

namespace MyBGListApi.Attributes;

public class SortColumnValidatorAttribute:  ValidationAttribute
{
    public Type EntityType { set; get; }

    public SortColumnValidatorAttribute(Type entityType)
    {
        EntityType = entityType;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (EntityType != null)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue) && EntityType.GetProperties().Any(p => p.Name == strValue))
                return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
    
}