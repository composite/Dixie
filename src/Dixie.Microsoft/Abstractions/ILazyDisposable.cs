using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Microsoft.Abstractions
{
    /// <summary>
    /// The service object that only store type infomation and make service disposable later.
    /// </summary>
    /// <typeparam name="T">If its class has inherited with <see cref="ILazyEvent"/>, lazy system will fire defined event delegation.</typeparam>
    public interface ILazyDisposable<out T> : ILazy<T>, IDisposable where T : class, IDisposable
    {

    }
}
