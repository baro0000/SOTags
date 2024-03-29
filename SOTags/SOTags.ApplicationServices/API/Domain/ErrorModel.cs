using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.ApplicationServices.API.Domain
{
    public class ErrorModel
    {
        public string Error { get; }
        public ErrorModel(string error)
        {
            Error = error;
        }
    }
}
