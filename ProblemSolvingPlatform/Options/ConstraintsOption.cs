using ProblemSolvingPlatform.Options.TestCase;

namespace ProblemSolvingPlatform.Options {
    public class ConstraintsOption {
        public ProblemConstraintsOption Problem { get; set; } = new ProblemConstraintsOption();
        public TestCaseConstraintsOption TestCase { get; set; } = new TestCaseConstraintsOption(); 
        public TagConstraintsOption Tag { get; set; } = new TagConstraintsOption();
        public UserConstraintsOption User { get; set; } = new UserConstraintsOption();
        public SubmissionConstraintsOption Submission { get; set; } = new SubmissionConstraintsOption();
    }
}
