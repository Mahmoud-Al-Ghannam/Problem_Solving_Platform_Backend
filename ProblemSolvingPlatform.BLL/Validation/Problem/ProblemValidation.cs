using Microsoft.IdentityModel.Tokens;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.BLL.Options.Constraint.TestCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Validation.Problem {
    public class ProblemValidation {
        private readonly ConstraintsOption _constraintsOption;

        public ProblemValidation(ConstraintsOption constraintsOption) {
            _constraintsOption = constraintsOption;
        }

        public Dictionary<string, List<string>> ValidateNewProblem(NewProblemDTO newProblem) {
            ProblemConstraintsOption conProblem = _constraintsOption.Problem;
            TestCaseConstraintsOption conTestCase = _constraintsOption.TestCase;
            Dictionary<string, List<string>> errors = new();
            errors["Title"] = [];
            errors["GeneralDescription"] = [];
            errors["InputDescription"] = [];
            errors["OutputDescription"] = [];
            errors["Note"] = [];
            errors["Tutorial"] = [];
            errors["SolutionCode"] = [];
            errors["TimeLimitMilliseconds"] = [];
            errors["TestCases"] = [];

            if (string.IsNullOrEmpty(newProblem.Title)) {
                if (conProblem.TitleLength.Start.Value > 0)
                    errors["Title"].Add($"The title of problem is required");
            }
            else {
                if (newProblem.Title.Length > conProblem.TitleLength.End.Value || newProblem.Title.Length < conProblem.TitleLength.Start.Value)
                    errors["Title"].Add($"The length of title of problem must to be in range [{conProblem.TitleLength.Start.Value},{conProblem.TitleLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(newProblem.GeneralDescription)) {
                if (conProblem.GeneralDescriptionLength.Start.Value > 0)
                    errors["GeneralDescription"].Add($"The general description of problem is required");
            }
            else {
                if (newProblem.GeneralDescription.Length > conProblem.GeneralDescriptionLength.End.Value || newProblem.GeneralDescription.Length < conProblem.GeneralDescriptionLength.Start.Value)
                    errors["GeneralDescription"].Add($"The length of general description of problem must to be in range [{conProblem.GeneralDescriptionLength.Start.Value},{conProblem.GeneralDescriptionLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(newProblem.InputDescription)) {
                if (conProblem.InputDescriptionLength.Start.Value > 0)
                    errors["InputDescription"].Add($"The input description of problem is required");
            }
            else {
                if (newProblem.InputDescription.Length > conProblem.InputDescriptionLength.End.Value || newProblem.InputDescription.Length < conProblem.InputDescriptionLength.Start.Value)
                    errors["InputDescription"].Add($"The length of input description of problem must to be in range [{conProblem.InputDescriptionLength.Start.Value},{conProblem.InputDescriptionLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(newProblem.OutputDescription)) {
                if (conProblem.OutputDescriptionLength.Start.Value > 0)
                    errors["OutputDescription"].Add($"The output description of problem is required");
            }
            else {
                if (newProblem.OutputDescription.Length > conProblem.OutputDescriptionLength.End.Value || newProblem.OutputDescription.Length < conProblem.OutputDescriptionLength.Start.Value)
                    errors["OutputDescription"].Add($"The length of output description of problem must to be in range [{conProblem.OutputDescriptionLength.Start.Value},{conProblem.OutputDescriptionLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(newProblem.Note)) {
                if (conProblem.NoteLength.Start.Value > 0)
                    errors["Note"].Add($"The note of problem is required");
            }
            else {
                if (newProblem.Note.Length > conProblem.NoteLength.End.Value || newProblem.Note.Length < conProblem.NoteLength.Start.Value)
                    errors["Note"].Add($"The length of note of problem must to be in range [{conProblem.NoteLength.Start.Value},{conProblem.NoteLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(newProblem.Tutorial)) {
                if (conProblem.TutorialLength.Start.Value > 0)
                    errors["Tutorial"].Add($"The tutorial of problem is required");
            }
            else {
                if (newProblem.Tutorial.Length > conProblem.TutorialLength.End.Value || newProblem.Tutorial.Length < conProblem.TutorialLength.Start.Value)
                    errors["Tutorial"].Add($"The length of tutorial of problem must to be in range [{conProblem.TutorialLength.Start.Value},{conProblem.TutorialLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(newProblem.SolutionCode)) {
                if (conProblem.SolutionCodeLength.Start.Value > 0)
                    errors["SolutionCode"].Add($"The solution code of problem is required");
            }
            else {
                if (newProblem.SolutionCode.Length > conProblem.SolutionCodeLength.End.Value || newProblem.SolutionCode.Length < conProblem.SolutionCodeLength.Start.Value)
                    errors["SolutionCode"].Add($"The length of solution code of problem must to be in range [{conProblem.SolutionCodeLength.Start.Value},{conProblem.SolutionCodeLength.End.Value}]");
            }

            if (newProblem.TimeLimitMilliseconds > conProblem.TimeLimitMS.End.Value || newProblem.TimeLimitMilliseconds < conProblem.TimeLimitMS.Start.Value)
                errors["TimeLimitMilliseconds"].Add($"The time limit (ms) of problem must to be in range [{conProblem.TimeLimitMS.Start.Value},{conProblem.TimeLimitMS.End.Value}]");

            if (newProblem.TestCases == null || newProblem.TestCases.Count() == 0) {
                if (conProblem.NoTotalTestCases.Start.Value > 0)
                    errors["TestCases"].Add($"The test cases of problem is required");
            }
            else {
                if (newProblem.TestCases.Count() > conProblem.NoTotalTestCases.End.Value || newProblem.TestCases.Count() < conProblem.NoTotalTestCases.Start.Value)
                    errors["TestCases"].Add($"The number of total test cases of problem must to be in range [{conProblem.NoTotalTestCases.Start.Value},{conProblem.NoTotalTestCases.End.Value}]");

                int noSampleTestCases = newProblem.TestCases.Count(e => e.IsSample);
                if (noSampleTestCases > conProblem.NoSampleTestCases.End.Value || noSampleTestCases < conProblem.NoSampleTestCases.Start.Value)
                    errors["TestCases"].Add($"The number of sample test cases of problem must to be in range [{conProblem.NoSampleTestCases.Start.Value},{conProblem.NoSampleTestCases.End.Value}]");

                for (int i = 0; i < newProblem.TestCases.Count(); i++) {
                    errors[$"TestCases[{i}].Input"] = [];

                    if (newProblem.TestCases[i].IsSample) {
                        newProblem.TestCases[i].IsPublic = true;
                        int noLines = newProblem.TestCases[i].Input.Count(c => c == '\n') + 1;

                        if (string.IsNullOrEmpty(newProblem.TestCases[i].Input)) {
                            if (conTestCase.Sample.InputLength.Start.Value > 0)
                                errors[$"TestCases[{i}].Input"].Add($"The input of test case of problem is required");
                        }
                        else {
                            if (newProblem.TestCases[i].Input.Length > conTestCase.Sample.InputLength.End.Value || newProblem.TestCases[i].Input.Length < conTestCase.Sample.InputLength.Start.Value)
                                errors[$"TestCases[{i}].Input"].Add($"The length of input of test case of problem must to be in range [{conTestCase.Sample.InputLength.Start.Value},{conTestCase.Sample.InputLength.End.Value}]");

                            if (noLines > conTestCase.Sample.InputNoLines.End.Value || noLines < conTestCase.Sample.InputNoLines.Start.Value)
                                errors[$"TestCases[{i}].Input"].Add($"The number of lines of input of test case of problem must to be in range [{conTestCase.Sample.InputNoLines.Start.Value},{conTestCase.Sample.InputNoLines.End.Value}]");
                        }
                    }
                    else {
                        if (string.IsNullOrEmpty(newProblem.TestCases[i].Input)) {
                            if (conTestCase.General.InputLength.Start.Value > 0)
                                errors[$"TestCases[{i}].Input"].Add($"The input of test case of problem is required");
                        }
                        else {
                            if (newProblem.TestCases[i].Input.Length > conTestCase.General.InputLength.End.Value || newProblem.TestCases[i].Input.Length < conTestCase.General.InputLength.Start.Value)
                                errors[$"TestCases[{i}].Input"].Add($"The length of input of test case of problem must to be in range [{conTestCase.General.InputLength.Start.Value},{conTestCase.General.InputLength.End.Value}]");
                        }
                    }

                }
            }


            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            return errors;
        }

        public Dictionary<string,List<string>> ValidateUpdateProblem (UpdateProblemDTO updateProblem) {
            ProblemConstraintsOption conProblem = _constraintsOption.Problem;
            TestCaseConstraintsOption conTestCase = _constraintsOption.TestCase;
            Dictionary<string, List<string>> errors = new();
            errors["Title"] = [];
            errors["GeneralDescription"] = [];
            errors["InputDescription"] = [];
            errors["OutputDescription"] = [];
            errors["Note"] = [];
            errors["Tutorial"] = [];

            if (string.IsNullOrEmpty(updateProblem.Title)) {
                if (conProblem.TitleLength.Start.Value > 0)
                    errors["Title"].Add($"The title of problem is required");
            }
            else {
                if (updateProblem.Title.Length > conProblem.TitleLength.End.Value || updateProblem.Title.Length < conProblem.TitleLength.Start.Value)
                    errors["Title"].Add($"The length of title of problem must to be in range [{conProblem.TitleLength.Start.Value},{conProblem.TitleLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(updateProblem.GeneralDescription)) {
                if (conProblem.GeneralDescriptionLength.Start.Value > 0)
                    errors["GeneralDescription"].Add($"The general description of problem is required");
            }
            else {
                if (updateProblem.GeneralDescription.Length > conProblem.GeneralDescriptionLength.End.Value || updateProblem.GeneralDescription.Length < conProblem.GeneralDescriptionLength.Start.Value)
                    errors["GeneralDescription"].Add($"The length of general description of problem must to be in range [{conProblem.GeneralDescriptionLength.Start.Value},{conProblem.GeneralDescriptionLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(updateProblem.InputDescription)) {
                if (conProblem.InputDescriptionLength.Start.Value > 0)
                    errors["InputDescription"].Add($"The input description of problem is required");
            }
            else {
                if (updateProblem.InputDescription.Length > conProblem.InputDescriptionLength.End.Value || updateProblem.InputDescription.Length < conProblem.InputDescriptionLength.Start.Value)
                    errors["InputDescription"].Add($"The length of input description of problem must to be in range [{conProblem.InputDescriptionLength.Start.Value},{conProblem.InputDescriptionLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(updateProblem.OutputDescription)) {
                if (conProblem.OutputDescriptionLength.Start.Value > 0)
                    errors["OutputDescription"].Add($"The output description of problem is required");
            }
            else {
                if (updateProblem.OutputDescription.Length > conProblem.OutputDescriptionLength.End.Value || updateProblem.OutputDescription.Length < conProblem.OutputDescriptionLength.Start.Value)
                    errors["OutputDescription"].Add($"The length of output description of problem must to be in range [{conProblem.OutputDescriptionLength.Start.Value},{conProblem.OutputDescriptionLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(updateProblem.Note)) {
                if (conProblem.NoteLength.Start.Value > 0)
                    errors["Note"].Add($"The note of problem is required");
            }
            else {
                if (updateProblem.Note.Length > conProblem.NoteLength.End.Value || updateProblem.Note.Length < conProblem.NoteLength.Start.Value)
                    errors["Note"].Add($"The length of note of problem must to be in range [{conProblem.NoteLength.Start.Value},{conProblem.NoteLength.End.Value}]");
            }

            if (string.IsNullOrEmpty(updateProblem.Tutorial)) {
                if (conProblem.TutorialLength.Start.Value > 0)
                    errors["Tutorial"].Add($"The tutorial of problem is required");
            }
            else {
                if (updateProblem.Tutorial.Length > conProblem.TutorialLength.End.Value || updateProblem.Tutorial.Length < conProblem.TutorialLength.Start.Value)
                    errors["Tutorial"].Add($"The length of tutorial of problem must to be in range [{conProblem.TutorialLength.Start.Value},{conProblem.TutorialLength.End.Value}]");
            }


            return errors;
        }
    }
}
