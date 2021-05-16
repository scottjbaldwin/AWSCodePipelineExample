
using System;
using System.Globalization;
using Xunit;

namespace CovidAPI.Tests
{
    public class DateParseTest
    {
        [Fact]
        public void ParseExactShouldWork()
        {
            // Arrange
            var dateStr = "2021-03-10";

            // Act
            DateTime date;
            var success = DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            // Assert
            Assert.True(success);
        }
    }
}