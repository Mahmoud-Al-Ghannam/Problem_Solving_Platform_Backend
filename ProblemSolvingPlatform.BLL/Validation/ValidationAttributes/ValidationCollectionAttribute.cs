using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Validation.ValidationAttributes {
    public class ValidationCollectionAttribute : ValidationAttribute {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            if (value == null) return ValidationResult.Success;
            string DisplayName = validationContext.DisplayName;
            List<ValidationResult> endResults = new List<ValidationResult>();

            int index = 0;
            if (value is IEnumerable<object> items) {
                foreach (var item in items) {
                    List<ValidationResult> itemResults = ValidationHelper.Validate(item, out bool isValid);
                    if(!isValid) {
                        foreach (var res in itemResults) {
                            string memberName = (res.MemberNames.Any() ? res.MemberNames.ToArray()[0].ToString() : "UnknowProperty");
                            memberName = $"{DisplayName}[{index}].{memberName}";
                            endResults.Add(new ValidationResult(res.ErrorMessage,new string[]{ memberName }));
                        }
                    }
                    index++;
                }

                if (endResults.Count > 0)
                    return new CompositeValidationResult("Invalid a collection data.", endResults);

                return ValidationResult.Success;
            }
            else return new ValidationResult($"The {DisplayName} is not a collection.");

        }
    }
}
