using System.Text;

namespace ProblemSolvingPlatform.Responses {
    public class BadRequestResponseBody {
        public Dictionary<string, List<string>> errors { get; set; }

        private string _message = null;
        public string message {
            get {
                if (_message != null) return _message;

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in errors) {
                    stringBuilder.AppendLine($"_ {error.Key}");
                    foreach (string item in error.Value) {
                        stringBuilder.AppendLine($"___ {item}");
                    }
                }

                _message = stringBuilder.ToString();
                return _message;
            }
        }

        public BadRequestResponseBody(Dictionary<string, List<string>> errors) {
            this.errors = errors;
        }
    }
}
