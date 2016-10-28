using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixie.Tests.Dixie.Microsoft
{
    using global::Dixie.Microsoft;
    using global::Dixie.Tests.Dixie.Microsoft.Sample;

    using global::Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class SimpleTest
    {
        private IServiceCollection services;
        private IServiceProvider provider;

        public SimpleTest()
        {
            this.services = new ServiceCollection();
            this.services.AddServiceScan(this.GetType());
            this.provider = this.services.BuildServiceProvider();
        }

        [Fact]
        public void SampleService_MustExist()
        {
            Assert.NotNull(this.provider.GetService<SampleService>());
        }

        [Fact]
        public void SampleService_InstanceOK()
        {
            Assert.Equal("OK", this.provider.GetService<SampleService>().Test());
        }
    }
}
