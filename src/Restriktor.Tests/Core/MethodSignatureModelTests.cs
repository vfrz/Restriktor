using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class MethodSignatureModelTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("*", "*")]
        [InlineData("System.String", "System.String")]
        [InlineData("System.String,System.Int32", "System.String,System.Int32")]
        [InlineData("System.String, System.Int32 ", "System.String,System.Int32")]
        public void StringConstructor_ToString(string methodSignature, string expected)
        {
            Check.That(new MethodSignatureModel(methodSignature).ToString()).HasSameValueAs(expected);
        }
    }
}