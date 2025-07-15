using ProblemSolvingPlatform.BLL.Options.Constraint.TestCase;

namespace ProblemSolvingPlatform.BLL.Options.Constraint
{
    public class ConstraintsOption
    {

        public int MinPageNumber { get; set; }
        public Range PageSize { get; set; }
        public ProblemConstraintsOption Problem { get; set; } = new ProblemConstraintsOption();
        public TestCaseConstraintsOption TestCase { get; set; } = new TestCaseConstraintsOption();
        public TagConstraintsOption Tag { get; set; } = new TagConstraintsOption();
        public UserConstraintsOption User { get; set; } = new UserConstraintsOption();
        public SubmissionConstraintsOption Submission { get; set; } = new SubmissionConstraintsOption();
    }
}
