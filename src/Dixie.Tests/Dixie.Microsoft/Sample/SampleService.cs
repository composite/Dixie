namespace Dixie.Tests.Dixie.Microsoft.Sample
{
    using global::Dixie.Microsoft.Attribute;

    [Service]
    public class SampleService
    {
        public string Test()
        {
            return "OK";
        }
    }
}
