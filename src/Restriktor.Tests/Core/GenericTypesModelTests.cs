using System;
using NFluent;
using Restriktor.Core;
using Xunit;

namespace Restriktor.Tests.Core
{
    public class GenericTypesModelTests
    {
        private class TestNonGenericClass
        {
        }

        private class TestGenericOneTypeClass<T>
        {
        }

        private class TestGenericTwoTypesClass<T, U>
        {
        }

        [Fact]
        public void Parse_Wildcard_Ok()
        {
            var model = GenericTypesModel.Parse("*");

            Check.That(model.IsWildcard).IsTrue();
            Check.That(model.Types).CountIs(0);
        }

        [Fact]
        public void Parse_Null_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = GenericTypesModel.Parse(null);
            }).Throws<FormatException>();
        }

        [Fact]
        public void Parse_Whitespace_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = GenericTypesModel.Parse(" ");
            }).Throws<FormatException>();
        }

        [Fact]
        public void Parse_OneType_Ok()
        {
            var model = GenericTypesModel.Parse("System.Console");

            Check.That(model.IsWildcard).IsFalse();
            Check.That(model.Types).CountIs(1);
            Check.That(model.Types[0].ToString()).HasSameValueAs("System.Console");
        }

        [Fact]
        public void Parse_TwoTypes_Ok()
        {
            var model = GenericTypesModel.Parse("System.Console, Restriktor.Core.GenericTypesModel");

            Check.That(model.IsWildcard).IsFalse();
            Check.That(model.Types).CountIs(2);
            Check.That(model.Types[0].ToString()).HasSameValueAs("System.Console");
            Check.That(model.Types[1].ToString()).HasSameValueAs("Restriktor.Core.GenericTypesModel");
        }

        [Fact]
        public void Parse_ThreeTypes_Ok()
        {
            var model = GenericTypesModel.Parse("System.Console,Restriktor.Core.GenericTypesModel,Restriktor.Restrictor");

            Check.That(model.IsWildcard).IsFalse();
            Check.That(model.Types).CountIs(3);
            Check.That(model.Types[0].ToString()).HasSameValueAs("System.Console");
            Check.That(model.Types[1].ToString()).HasSameValueAs("Restriktor.Core.GenericTypesModel");
            Check.That(model.Types[2].ToString()).HasSameValueAs("Restriktor.Restrictor");
        }

        [Fact]
        public void FromType_NonGeneric_Throws()
        {
            Check.ThatCode(() =>
            {
                var _ = GenericTypesModel.FromType(typeof(TestNonGenericClass));
            }).Throws<ArgumentException>();
        }

        [Fact]
        public void FromType_GenericOneType_Wildcard()
        {
            var model = GenericTypesModel.FromType(typeof(TestGenericOneTypeClass<>));

            Check.That(model.IsWildcard).IsTrue();
            Check.That(model.Types).CountIs(0);
        }

        [Fact]
        public void FromType_GenericTwoTypes_Wildcard()
        {
            var model = GenericTypesModel.FromType(typeof(TestGenericTwoTypesClass<,>));

            Check.That(model.IsWildcard).IsTrue();
            Check.That(model.Types).CountIs(0);
        }

        [Fact]
        public void FromType_GenericOneType_Int()
        {
            var model = GenericTypesModel.FromType(typeof(TestGenericOneTypeClass<int>));

            Check.That(model.IsWildcard).IsFalse();
            Check.That(model.Types).CountIs(1);
            Check.That(model.Types[0].ToString()).HasSameValueAs("System.Int32");
        }

        [Fact]
        public void FromType_GenericTwoTypes_IntAndString()
        {
            var model = GenericTypesModel.FromType(typeof(TestGenericTwoTypesClass<int, string>));

            Check.That(model.IsWildcard).IsFalse();
            Check.That(model.Types).CountIs(2);
            Check.That(model.Types[0].ToString()).HasSameValueAs("System.Int32");
            Check.That(model.Types[1].ToString()).HasSameValueAs("System.String");
        }
    }
}