using FluentAssertions;
using Xunit;

namespace Opw.HttpExceptions
{
    public class ProblemDetailAttributeTests
    {
        [Fact]
        public void Property_Should_HaveAttribute()
        {
            new Person().GetType().GetProperty(nameof(Person.Id))
                .GetCustomAttributes(false).Should()
                .Contain(new ProblemDetailsAttribute());
        }

        [Fact]
        public void Customer_BaseProperty_Should_HaveAttribute()
        {
            new Customer().GetType().GetProperty(nameof(Person.Name))
                .GetCustomAttributes(false).Should()
                .Contain(new ProblemDetailsAttribute());
        }

        [Fact]
        public void Customer_OverriddenBaseProperty_Should_HaveAttribute()
        {
            new Customer().GetType().GetProperty(nameof(Customer.Id))
                .GetCustomAttributes(false).Should()
                .Contain(new ProblemDetailsAttribute());
        }

        [Fact]
        public void Customer_Property_Should_NotHaveAttribute()
        {
            new Customer().GetType().GetProperty(nameof(Customer.Code))
                .GetCustomAttributes(false).Should()
                .NotContain(new ProblemDetailsAttribute());
        }

        private class Person
        {
            [ProblemDetails]
            public virtual int Id { get; set; }

            [ProblemDetails]
            public string Name { get; set; }
        }

        private class Customer : Person
        {
            [ProblemDetails]
            public override int Id { get; set; }

            public string Code { get; set; }
        }
    }
}
