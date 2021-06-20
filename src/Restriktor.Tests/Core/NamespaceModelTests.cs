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

            Check.That(model.Parts).CountIs(1);
            Check.That(model.Parts[0]).HasSameValueAs("System");
        }

        [Fact]
        public void Constructor_TwoParts_Ok()
        {
            var model = new NamespaceModel(new[] {"System", "Encoding"});

            Check.That(model.Parts).CountIs(2);
            Check.That(model.Parts[0]).HasSameValueAs("System");
            Check.That(model.Parts[1]).HasSameValueAs("Encoding");
        }

        [Fact]
        public void Constructor_Null_GlobalNamespace()
        {
            var model = new NamespaceModel(null);

            Check.That(model.Parts).CountIs(0);
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

            Check.That(model.Parts).CountIs(1);
            Check.That(model.Parts[0]).HasSameValueAs("System");
        }

        [Fact]
        public void Parse_TwoParts_Ok()
        {
            var model = NamespaceModel.Parse("System.Encoding");

            Check.That(model.Parts).CountIs(2);
            Check.That(model.Parts[0]).HasSameValueAs("System");
            Check.That(model.Parts[1]).HasSameValueAs("Encoding");
        }

        [Fact]
        public void Parse_ThreeParts_Ok()
        {
            var model = NamespaceModel.Parse("System.Encoding.UTF8");

            Check.That(model.Parts).CountIs(3);
            Check.That(model.Parts[0]).HasSameValueAs("System");
            Check.That(model.Parts[1]).HasSameValueAs("Encoding");
            Check.That(model.Parts[2]).HasSameValueAs("UTF8");
        }

        [Fact]
        public void Parse_Null_GlobalNamespace()
        {
            var model = NamespaceModel.Parse(null);

            Check.That(model.IsGlobalNamespace).IsTrue();
        }

        [Fact]
        public void Parse_Empty_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = NamespaceModel.Parse("");
            }).Throws<FormatException>();
        }

        [Fact]
        public void Parent_GlobalNamespace_Null()
        {
            var model = new NamespaceModel(null);

            var parent = model.Parent();

            Check.That(parent).IsNull();
        }

        [Fact]
        public void Parent_OnePart_GlobalNamespace()
        {
            var model = new NamespaceModel(new[] {"System"});

            var parent = model.Parent();

            Check.That(parent.IsGlobalNamespace).IsTrue();
        }

        [Fact]
        public void Parent_TwoParts_FirstPart()
        {
            var model = new NamespaceModel(new[] {"System", "Encoding"});

            var parent = model.Parent();

            Check.That(parent.Parts).CountIs(1);
            Check.That(parent.Parts[0]).HasSameValueAs("System");
        }

        [Fact]
        public void Parent_ThreeParts_SecondPart()
        {
            var model = new NamespaceModel(new[] {"System", "Encoding", "UTF8"});

            var parent = model.Parent();

            Check.That(parent.Parts).CountIs(2);
            Check.That(parent.Parts[0]).HasSameValueAs("System");
            Check.That(parent.Parts[1]).HasSameValueAs("Encoding");
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
        public void Match_Theory(string baseNamespace, string anotherNamespace, bool perfectMatch, bool expectedMatch)
        {
            var match = NamespaceModel.Parse(baseNamespace).Match(anotherNamespace, perfectMatch);

            Check.That(match).HasSameValueAs(expectedMatch);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("System", "System")]
        [InlineData("System.Encoding", "System.Encoding")]
        [InlineData("System.Encoding.UTF8", "System.Encoding.UTF8")]
        public void ToString_Theory(string baseNamespace, string expectedValue)
        {
            var model = NamespaceModel.Parse(baseNamespace);
            var stringValue = model.ToString();

            Check.That(stringValue).HasSameValueAs(expectedValue);
        }
    }
}