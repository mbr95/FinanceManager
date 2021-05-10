using FinanceManager.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FinanceManager.Validation
{
    public class TransactionCategoryAttribute : ValidationAttribute
    {
        private int[] _allowedCategories;

        public TransactionCategoryAttribute()
        {
            _allowedCategories = Enum.GetValues(typeof(TransactionCategoryId)).Cast<int>().ToArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_allowedCategories.Contains((int)value))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"It is not a valid transaction category.");
        }
    }
}
