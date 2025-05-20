using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Validation {
    public class ValidationHelper {
        public static List<ValidationResult> Validate(object value, out bool isValid) {
            ValidationContext context = new ValidationContext(value);
            List<ValidationResult> results = new List<ValidationResult>();
            isValid = Validator.TryValidateObject(value, context, results, true);
            return results;
        }

        public static string ValidateToString(object value, out bool isValid) {
            List<ValidationResult> results = Validate(value, out isValid);
            return string.Join("\n", results);
        }

        public static Dictionary<string, List<string>> ValidateToDictionary(object value, out bool isValid) {
            List<ValidationResult> results = Validate(value, out isValid);
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

            foreach (ValidationResult result in results) {
                if (result is CompositeValidationResult compositeValidationResult) {
                    foreach (ValidationResult res in compositeValidationResult.Results) {
                        _AddValidationResultToDictionary(ref errors, res);
                    }
                }
                else {
                    _AddValidationResultToDictionary(ref errors, result);
                }

            }
            return errors;
        }

        private static void _AddValidationResultToDictionary (ref Dictionary<string, List<string>> dictionary, ValidationResult validationResult) {
            string propertyName = "UnknownPropertyName";
            if (validationResult.MemberNames.Any()) {
                propertyName = validationResult.MemberNames.ToArray()[0];
            }
            dictionary[propertyName].Add(validationResult.ErrorMessage??"Unknown Error");
        }

    }
}
