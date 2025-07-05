namespace ProblemSolvingPlatform.Options {
    public class ProblemConstraintsOption {
        public Range TitleLength { get; set; }
        public Range GeneralDescriptionLength { get; set; }
        public Range InputDescriptionLength { get; set; }
        public Range OutputDescriptionLength { get; set; }
        public Range NoteLength { get; set; }
        public Range TutorialLength { get; set; }
        public Range SolutionCodeLength { get; set; }
        public Range NoTotalTestCases { get; set; }
        public Range NoSampleTestCases { get; set; }
        public Range TimeLimitMS { get; set; }
    }
}
