using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Tests.Dixie.Microsoft
{
    using global::Dixie.Microsoft;
    using global::Dixie.Tests.Dixie.Microsoft.AttributeServices;

    using global::Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class AttributeTest
    {
        private readonly IServiceProvider provider;

        public AttributeTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddServiceScan(this.GetType());
            this.provider = services.BuildServiceProvider();
        }

        [Fact]
        public void SampleService_MustExist()
        {
            Assert.NotNull(this.provider.GetService<SampleAttributeService>());
        }

        [Fact]
        public void SampleService_InstanceOK()
        {
            Assert.Equal("OK", this.provider.GetService<SampleAttributeService>().Test());
        }
    }
}
