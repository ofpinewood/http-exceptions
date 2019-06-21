using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    public class HttpResponseMapperTests
    {
        private readonly HttpResponseMapper _mapper;

        public HttpResponseMapperTests()
        {
            var optionsMock = TestHelper.CreateHttpExceptionsOptionsMock(false);
            _mapper = new HttpResponseMapper(optionsMock.Object);
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails()
        {
            var response = new DefaultHttpContext().Response;
            var problemDetails = _mapper.Map(response);

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetails.Instance.Should().BeNull();

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void CanMap_Should_ReturnTrue_WhenMatchingStatus()
        {
            _mapper.Status = 500;
            var result = _mapper.CanMap(500);

            result.Should().BeTrue();
        }

        [Fact]
        public void CanMap_Should_ReturnFalse_WhenNotMatchingStatus()
        {
            _mapper.Status = 500;
            var result = _mapper.CanMap(418);

            result.Should().BeFalse();
        }

        [Fact]
        public void CanMap_Should_ReturnTrue_ForAllStatuses_WhenMapperStatusIsNotSet_WhenStatusIntMinValue()
        {
            for (int i = 400; i < 600; i++)
            {
                _mapper.CanMap(i).Should().BeTrue();
            }
        }
    }
}
