namespace Dixie.Microsoft.Internal
{
    using Dixie.Microsoft.Attribute;
    using System;
    using System.Collections.Generic;

    internal class ScanDescriptor
    {
        public List<Tuple<Type, ServiceAttribute>> Types { get; } = new List<Tuple<Type, ServiceAttribute>>();

        public ScanDescriptor Sort()
        {
            if (this.Types.Count > 1) this.Types.Sort(ServiceFlagComparer.Instance);
            this.Types.Capacity = this.Types.Count;
            return this;
        }
    }
}