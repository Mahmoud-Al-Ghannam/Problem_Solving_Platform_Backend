using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Options {
    public class JwtOption {
        public string Issuer {  get; set; }
        public string Audience {  get; set; }
        public int LifeTimeMin {  get; set; }
        public string Key {  get; set; }
    }
}
