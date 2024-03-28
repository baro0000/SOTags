using SOTags.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.ApplicationServices.Components.Connectors.StackOverflow
{
    public interface IStackOverflowConnector
    {
        string Error { get; set; }
        Task DownloadData();
    }
}
