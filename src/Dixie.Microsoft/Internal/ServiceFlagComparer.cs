namespace Dixie.Microsoft.Internal
{
    using Dixie.Microsoft.Abstractions;
    using Dixie.Microsoft.Attribute;
    using System;
    using System.Collections.Generic;

    internal class ServiceFlagComparer : IComparer<Tuple<Type, ServiceAttribute>>
    {
        private static ServiceFlagComparer instance;
        public static ServiceFlagComparer Instance => instance ?? (instance = new ServiceFlagComparer());

        private ServiceFlagComparer()
        {
        }

        public int Compare(Tuple<Type, ServiceAttribute> x, Tuple<Type, ServiceAttribute> y)
        {
            if (!(x.Item2 is ServiceFlagAttribute) && !(x.Item2 is ServiceFlagAttribute)) return 0;
            if (!(x.Item2 is ServiceFlagAttribute)) return -1;
            if (!(y.Item2 is ServiceFlagAttribute)) return 1;
            return (((ServiceFlagAttribute)x.Item2).Order).CompareTo(((ServiceFlagAttribute)y.Item2).Order);
        }
    }
}