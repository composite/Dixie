using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Tests.Dixie.Microsoft.ManualServices
{
    public interface ISampleLazyManualService
    {
        string Test();
    }

    public class SampleLazyManualService : ISampleLazyManualService
    {
        public string Test()
        {
            return "OK";
        }
    }
}
