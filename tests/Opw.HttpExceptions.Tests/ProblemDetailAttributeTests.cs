using FluentAssertions;
using Opw.HttpExceptions.Attributes;
using Xunit;

namespace Opw.HttpExceptions
{
    public class ProblemDetailAttributeTests
    {
        [Fact]
        public void Attribute_Should_Exist()
        {
            var foo = new BaseClasse();
            foo.GetType().GetProperty(nameof(BaseClasse.Foo))
                .GetCustomAttributes(false).Should()
                .Contain(new ProblemDetailsAttribute());
        }

        [Fact]
        public void DerivedClass_BaseProperty_Attribute_Should_Exist()
        {
            var foo = new DerivedClass();
            foo.GetType().GetProperty(nameof(BaseClasse.Bar))
                .GetCustomAttributes(false).Should()
                .Contain(new ProblemDetailsAttribute());
        }

        [Fact]
        public void DerivedClass_BaseProperty_override_Attribute_Should_Exist()
        {
            var foo = new DerivedClass();
            foo.GetType().GetProperty(nameof(DerivedClass.Foo))
                .GetCustomAttributes(false).Should()
                .Contain(new ProblemDetailsAttribute());
        }

        [Fact]
        public void DerivedClass_Property_Attribute_Should_Not_Exist()
        {
            var foo = new DerivedClass();
            foo.GetType().GetProperty(nameof(DerivedClass.FooBar))
                .GetCustomAttributes(false).Should()
                .NotContain(new ProblemDetailsAttribute());
        }

        private class BaseClasse
        {
            [ProblemDetails]
            public virtual int Foo { get; set; }

            [ProblemDetails]
            public string Bar { get; set; }
        }

        private class DerivedClass : BaseClasse
        {
            [ProblemDetails]
            public override int Foo { get; set; }

            public string FooBar { get; set; }
        }
    }
}
