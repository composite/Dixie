using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Microsoft.Internal
{
    using global::Microsoft.Extensions.DependencyInjection;

    internal class PluggableServiceElement
    {
        public PluggableServiceElement(ServiceDescriptor desc, int idx)
        {
            Original = desc;
            Index = idx;
        }

        public ServiceDescriptor Original { get; }
        public int Index { get; }
        public bool IsDelete { get; set; }
        public ServiceDescriptor Replaced { get; set; }
    }
}
