namespace Dixie.Microsoft.Internal
{
    using Dixie.Microsoft.Abstractions;
    using global::Microsoft.Extensions.DependencyInjection;
    using System;

    internal class InternalLazyService<TService> : ILazy<TService> where TService : class
    {
        private readonly IServiceProvider provider;

        private TService instance;

        private readonly Type bodyType;
        private readonly ObjectFactory factory;

        private bool haveEvent = false;

        public InternalLazyService(IServiceProvider provider, Type bodyType)
        {
            this.provider = provider;
            this.bodyType = bodyType;
        }

        public InternalLazyService(IServiceProvider provider, ObjectFactory factory)
        {
            this.provider = provider;
            this.factory = factory;
        }

        public TService Value
        {
            get
            {
                if (this.instance != null && this.haveEvent) ((ILazyEvent)this.instance).OnValue();
                return this.instance ?? (this.instance = this.Create());
            }
        }

        public TService Create(params object[] param)
        {
            this.instance = this.factory != null
                                ? (TService)this.factory.Invoke(this.provider, param)
                                : (TService)ActivatorUtilities.CreateInstance(this.provider, this.bodyType, param);

            if (this.instance != null)
            {
                if ((this.haveEvent = this.instance is ILazyEvent)) ((ILazyEvent)this.instance).OnCreate();
            }

            return this.instance;
        }
    }
}