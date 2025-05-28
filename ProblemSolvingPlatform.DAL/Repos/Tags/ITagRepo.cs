using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Tags {
    public interface ITagRepo {
        public Task<IEnumerable<TagModel>> GetAllTagsAsync ();
    }
}
