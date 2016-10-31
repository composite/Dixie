using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Tests.Dixie.Microsoft
{
    using global::Dixie.Microsoft;
    using global::Dixie.Microsoft.Abstractions;
    using global::Dixie.Tests.Dixie.Microsoft.AttributeServices;
    using global::Dixie.Tests.Dixie.Microsoft.ManualServices;

    using global::Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class ManualTest
    {
        private readonly IServiceProvider provider;

        public ManualTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLazyScoped<ISampleLazyManualService, SampleLazyManualService>();
            this.provider = services.BuildServiceProvider();
        }

        [Fact]
        public void ISampleLazyManualService_MustExist()
        {
            Assert.NotNull(this.provider.GetService<ILazy<ISampleLazyManualService>>());
        }

        [Fact]
        public void ISampleLazyManualService_MustValueExist()
        {
            Assert.NotNull(this.provider.GetService<ILazy<ISampleLazyManualService>>().Value);
        }

        [Fact]
        public void ISampleLazyManualService_InstanceOK()
        {
            Assert.Equal("OK", this.provider.GetService<ILazy<ISampleLazyManualService>>().Value.Test());
        }
    }
}
