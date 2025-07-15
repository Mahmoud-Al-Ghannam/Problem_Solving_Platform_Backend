namespace ProblemSolvingPlatform.Responses {
    public class ErrorResponseBody {
        public string error {  get; set; }

        public ErrorResponseBody(string error) {
            this.error = error;
        }
    }
}
