using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Microsoft.Internal
{
    using System.Collections;

    using global::Microsoft.Extensions.DependencyInjection;

    internal class PluggableServiceCollection<TFilter> : IList<ServiceDescriptor>, IDisposable
    {
        private readonly IServiceCollection services;

        private readonly List<PluggableServiceElement> table;

        private bool disposed = false;

        public PluggableServiceCollection(IServiceCollection services)
        {
            this.services = services;
            this.table =
                new List<PluggableServiceElement>(
                    services.Where(d => d.ServiceType == typeof(TFilter))
                        .Select((d, i) => new PluggableServiceElement(d, i)));
        }

        private bool Contains(PluggableServiceElement plug, ServiceDescriptor item)
        {
            return !plug.IsDelete && (plug.Original == item || plug.Replaced == item);
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed) throw new ObjectDisposedException(nameof(this.table));
        }

        private void EnsureNotLessThan0(string param, int idx)
        {
            if (idx < 0) throw new ArgumentOutOfRangeException(param, "inded cannot be less than 0");
        }

        private void EnsureNotGreaterThenTable(string param, int idx)
        {
            if (idx >= this.services.Count)
                throw new ArgumentOutOfRangeException(
                          param,
                          $"index {idx} is too high. service length is {this.services.Count}.");
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            EnsureNotDisposed();
            return this.services.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnsureNotDisposed();
            return this.services.GetEnumerator();
        }

        public void Add(ServiceDescriptor item)
        {
            EnsureNotDisposed();
            this.table.Add(new PluggableServiceElement(null, -1) { Replaced = item });
        }

        public void Clear()
        {
            EnsureNotDisposed();
            foreach (var element in this.table)
            {
                element.IsDelete = true;
            }
        }

        public bool Contains(ServiceDescriptor item)
        {
            EnsureNotDisposed();
            return this.table.Any(p => Contains(p, item));
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            EnsureNotDisposed();
            Array.Copy(this.table.Select(p => p.Replaced ?? p.Original).ToArray(), array, this.table.Count);
        }

        public bool Remove(ServiceDescriptor item)
        {
            EnsureNotDisposed();
            var target = this.table.FirstOrDefault(p => Contains(p, item));
            if (target != null) return target.IsDelete = true;
            return false;
        }

        public int Count => this.table.Count(p => !p.IsDelete);

        public bool IsReadOnly => false;

        public int IndexOf(ServiceDescriptor item)
        {
            EnsureNotDisposed();
            for (int i = 0, len = this.table.Count; i < len; i++)
            {
                if (Contains(this.table[i], item)) return i;
            }
            return -1;
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            EnsureNotDisposed();
            EnsureNotLessThan0(nameof(index), index);
            EnsureNotGreaterThenTable(nameof(index), index);
            this.table.Add(new PluggableServiceElement(null, index) { Replaced = item });
        }

        public void RemoveAt(int index)
        {
            EnsureNotDisposed();
            EnsureNotLessThan0(nameof(index), index);
            EnsureNotGreaterThenTable(nameof(index), index);
            var targets = this.table.Where(p => !p.IsDelete && p.Index == index);
            if (targets.Any()) targets.First().IsDelete = true;
        }

        public ServiceDescriptor this[int index]
        {
            get
            {
                EnsureNotDisposed();
                EnsureNotLessThan0(nameof(index), index);
                EnsureNotGreaterThenTable(nameof(index), index);
                var target = this.table.FirstOrDefault(p => !p.IsDelete && p.Index == index);
                return target?.Replaced ?? target?.Original;
            }
            set
            {
                EnsureNotDisposed();
                EnsureNotLessThan0(nameof(index), index);
                EnsureNotGreaterThenTable(nameof(index), index);
                var target = this.table.FirstOrDefault(p => !p.IsDelete && p.Index == index && p.Original != null);
                target.Replaced = value;
            }
        }

        public void Dispose()
        {
            EnsureNotDisposed();
            this.table.Capacity = this.table.Count;

            foreach (var element in this.table)
            {
                if (element.Original != null && element.Replaced != null && element.Index > -1)
                {
                    this.services.RemoveAt(element.Index);
                    this.services.Insert(element.Index, element.Replaced);
                }
                else if (element.Original != null && element.IsDelete && element.Index > -1)
                {
                    this.services.RemoveAt(element.Index);
                }
                else if (element.Original == null && element.Replaced != null && !element.IsDelete)
                {
                    if (element.Index < 0) this.services.Add(element.Replaced);
                    else this.services.Insert(element.Index, element.Replaced);
                }
            }

            this.table.Clear();
            this.table.Capacity = 0;
            this.disposed = true;
        }
    }
}
