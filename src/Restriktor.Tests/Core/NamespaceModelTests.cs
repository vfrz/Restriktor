using System;
using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class NamespaceModelTests
    {
        [Fact]
        public void Constructor_OnePart_Ok()
        {
            var model = new NamespaceModel(new[] {"System"});

            Check.That(model.Parts.Length).HasSameValueAs(1);
            Check.That(model.Parts[0]).HasSameValueAs("System");
        }

        [Fact]
        public void Constructor_TwoParts_Ok()
        {
            var model = new NamespaceModel(new[] {"System", "Encoding"});

            Check.That(model.Parts.Length).HasSameValueAs(2);
            Check.That(model.Parts[0]).HasSameValueAs("System");
            Check.That(model.Parts[1]).HasSameValueAs("Encoding");
        }

        [Fact]
        public void Constructor_Null_GlobalNamespace()
        {
            var model = new NamespaceModel(null);

            Check.That(model.Parts.IsDefaultOrEmpty).IsTrue();
            Check.That(model.IsGlobalNamespace).IsTrue();
        }

        [Fact]
        public void Constructor_NullPart_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = new NamespaceModel(new[] {"System", null, "Encoding"});
            }).Throws<FormatException>();
        }

        [Fact]
        public void Constructor_EmptyPart_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = new NamespaceModel(new[] {"System", "", "Encoding"});
            }).Throws<FormatException>();
        }

        [Fact]
        public void Constructor_InvalidIdentifierPart_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = new NamespaceModel(new[] {"System", "444", "Encoding"});
            }).Throws<FormatException>();
        }

        [Fact]
        public void Parse_OnePart_Ok()
        {
            var model = NamespaceModel.Parse("System");

            Check.That(model.Parts.Length).HasSameValueAs(1);
            Check.That(model.Parts[0]).HasSameValueAs("System");
        }

        [Theory]
        [InlineData("System", "System", true, true)]
        [InlineData("System.Console", "System.Console", true, true)]
        [InlineData("System", "System.Console", true, false)]
        [InlineData("System.Console", "System", true, false)]
        [InlineData("System", "Restriktor", true, false)]
        [InlineData(null, null, true, true)]
        [InlineData(null, "System", true, false)]
        [InlineData("System", null, true, false)]
        [InlineData("System", "System", false, true)]
        [InlineData("System.Console", "System.Console", false, true)]
        [InlineData("System", "System.Console", false, false)]
        [InlineData("System.Console", "System", false, true)]
        [InlineData(null, null, false, true)]
        [InlineData("System", null, false, true)]
        [InlineData(null, "System", false, false)]
        public void Match_Nominal(string baseNamespace, string anotherNamespace, bool perfectMatch, bool expectedMatch)
        {
            Check.That(NamespaceModel.Parse(baseNamespace).Match(anotherNamespace, perfectMatch)).HasSameValueAs(expectedMatch);
        }

        [Theory]
        [InlineData("System", null)]
        [InlineData("System.Console", "System")]
        [InlineData("System.Console.Debug", "System.Console")]
        public void Parent_Nominal(string ns, string expectedParent)
        {
            var parent = NamespaceModel.Parse(ns).Parent();
            Check.That(parent.Match(expectedParent, true)).IsTrue();
        }

        [Theory]
        [InlineData(null, "<global namespace>")]
        [InlineData("System", "System")]
        [InlineData("System.Console", "System.Console")]
        public void Parse_ToString(string ns, string expectedValue)
        {
            Check.That(NamespaceModel.Parse(ns).ToString()).HasSameValueAs(expectedValue);
        }
    }
}