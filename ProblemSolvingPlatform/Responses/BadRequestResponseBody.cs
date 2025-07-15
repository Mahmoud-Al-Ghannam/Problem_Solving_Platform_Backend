namespace ProblemSolvingPlatform.Responses {
    public class BadRequestResponseBody {
        public Dictionary<string, List<string>> errors { get; set; }

        public BadRequestResponseBody(Dictionary<string, List<string>> errors) {
            this.errors = errors;
        }
    }
}
