using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Validation {
    public class CompositeValidationResult : ValidationResult {
        private List<ValidationResult> _results = new List<ValidationResult>();
        public IEnumerable<ValidationResult> Results => _results;
        public CompositeValidationResult (string? ErrorMessage) : base (ErrorMessage) { }
        public CompositeValidationResult (string? ErrorMessage,IEnumerable<string>? MemberNames) : base (ErrorMessage, MemberNames) { }
        public CompositeValidationResult (ValidationResult validationResult) : base (validationResult) { }
        public CompositeValidationResult (string? ErrorMessage,IEnumerable<ValidationResult> validationResults) : base (ErrorMessage) {
            _results.AddRange(validationResults);
        }

        public void AddResult(ValidationResult result) { 
            _results.Add(result);
        }
    }
}
