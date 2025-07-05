using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Exceptions {
    public class CustomValidationException : Exception {
        public Dictionary<string, List<string>> errors { get; set; } = new Dictionary<string, List<string>>();
        public CustomValidationException() { }
        public CustomValidationException(string message) : base(message) {
            this.errors = new Dictionary<string, List<string>> { ["Unknown"] = new List<string> { message } };
        }
        public CustomValidationException(Dictionary<string, List<string>> errors) : base(JsonConvert.SerializeObject(errors)) {
            this.errors = errors;
        }

        public CustomValidationException(string PropertyName, List<string> ValidationErrors)
            : base(PropertyName + ":" + Environment.NewLine + string.Join(",\n", ValidationErrors)) {
            this.errors = new Dictionary<string, List<string>> { [PropertyName] = ValidationErrors };
        }
    }
}
