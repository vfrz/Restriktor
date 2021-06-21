using System;
using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class TypeModelTests
    {
        public class InnerTestClass
        {
        }
        
        [Fact]
        public void FromType_SystemConsole_Ok()
        {
            var model = TypeModel.FromType(typeof(Console));

            Check.That(model.Name).HasSameValueAs("Console");
            Check.That(model.Namespace.ToString()).HasSameValueAs("System");
        }
        
        [Fact]
        public void FromType_InnerTestClass_Ok()
        {
            var model = TypeModel.FromType(typeof(InnerTestClass));

            Check.That(model.Name).HasSameValueAs("InnerTestClass");
            //Check.That(model.Namespace.ToString()).HasSameValueAs("Restriktor.Tests.Core.TypeModelTests");
        }
    }
}