using FluentDynamoDb.Extensions;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase("Bars", "_bars")]
        [TestCase("CompleteNames", "_completeNames")]
        public void ConvertToCamelCaseUnderscore_GivenAName_ShouldConvertToCamelCaseUnderscore(string name, string expectedName)
        {
            Assert.AreEqual(expectedName, name.ConvertToCamelCaseUnderscore());
        }
    }
}
