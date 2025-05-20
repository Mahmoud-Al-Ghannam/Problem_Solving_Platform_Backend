using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Validation.ValidationAttributes {
    public class MinLengthAfterTrimAttribute : ValidationAttribute {

        private readonly int _minLength;

        public MinLengthAfterTrimAttribute(int minLength) {
            _minLength = minLength;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            if (value == null) return ValidationResult.Success;
            if(value is not string) return ValidationResult.Success;

            string DisplayName = validationContext.DisplayName;
            string strValue = value?.ToString()??"";

            if (string.IsNullOrEmpty(strValue)) return ValidationResult.Success;


            if (strValue.Trim().Length < _minLength) {
                if (string.IsNullOrEmpty(ErrorMessage)) {
                    ErrorMessage = $"The field {DisplayName} must be a string type with a minimum length of {_minLength} after trim operation.";
                }
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName ?? "UnknowProperty" });
            }

            return ValidationResult.Success;
        }
    }
}
