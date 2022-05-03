using System;
using System.Linq;
using CronParser.Lib;
using FluentAssertions;
using Xunit;

namespace CronParser.Tests
{
    public class CronFieldTests
    {

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldThrowIfInputIsNullOrEmpty(string input)
        {
            var field = new CronField(CronFieldType.DayOfMonth, 1, 31);

            Action act = () => field.Parse(input);
            act.Should()
                .Throw<CronException>()
                .WithMessage($"Input is Null or Empty for field of type {GetEnumName(field.Type)}");
        }

        [Theory]
        [InlineData("1-5*")]
        [InlineData("*/5-")]
        [InlineData("/*5-")]
        [InlineData("1,3,5-")]
        [InlineData("1,3,5-*")]
        public void ShouldThrowIfInputContainsMultipleSymbols(string input)
        {
            var field = new CronField(CronFieldType.Minute, 0, 59);

            Action act = () => field.Parse(input);
            act.Should()
                .Throw<CronException>()
                .WithMessage($"Invalid Expression: Input '{input}' has multiple special characters - field type : {GetEnumName(field.Type)}");
        }

        [Theory]
        [InlineData("*/2")]
        [InlineData("0")]
        [InlineData("11,23")]
        [InlineData("*")]
        [InlineData("6-9")]
        public void ShouldNotThrowIfInputIsValid(string input)
        {
            var field = new CronField(CronFieldType.Hour, 0, 23);

            Action act = () => field.Parse(input);
            act.Should().NotThrow();
        }

        [Fact]
        public void ValidWildStarShouldWork()
        {
            var field = new CronField(CronFieldType.Month, 1, 12);

            var result = field.Parse("*");
            result.Should().BeTrue();
            field.IsAllValues.Should().BeTrue();
            field.IsRange.Should().BeFalse();
            field.IsInterval.Should().BeFalse();
            field.IsListOfValues.Should().BeFalse();


            var values = field.Values;
            var expectedValues = Enumerable.Range(1, 12).ToList();
            foreach (var val in values)
            {
                expectedValues.Contains(val).Should().BeTrue();
            }
        }

        [Fact]
        public void ValidRangeShouldWork()
        {
            var field = new CronField(CronFieldType.DayOfWeek, 0, 6);

            var result = field.Parse("0-4");
            result.Should().BeTrue();
            field.IsAllValues.Should().BeFalse();
            field.IsRange.Should().BeTrue();
            field.IsInterval.Should().BeFalse();
            field.IsListOfValues.Should().BeFalse();


            var values = field.Values;
            var expectedValues = new[] { 0, 1, 2, 3, 4 };
            foreach (var val in values)
            {
                expectedValues.Contains(val).Should().BeTrue();
            }
        }

        [Fact]
        public void ValidListOfValuesShouldWork()
        {
            var field = new CronField(CronFieldType.DayOfWeek, 0, 6);

            var result = field.Parse("0,4");
            result.Should().BeTrue();
            field.IsAllValues.Should().BeFalse();
            field.IsRange.Should().BeFalse();
            field.IsInterval.Should().BeFalse();
            field.IsListOfValues.Should().BeTrue();


            var values = field.Values;
            var expectedValues = new[] { 0, 4 };
            foreach (var val in values)
            {
                expectedValues.Contains(val).Should().BeTrue();
            }
        }

        [Fact]
        public void ValidSingleValueShouldWork()
        {
            var field = new CronField(CronFieldType.DayOfMonth, 1, 31);

            var result = field.Parse("31");
            result.Should().BeTrue();
            field.IsAllValues.Should().BeFalse();
            field.IsRange.Should().BeFalse();
            field.IsInterval.Should().BeFalse();
            field.IsListOfValues.Should().BeFalse();


            var values = field.Values;
            var expectedValues = new[] { 31 };
            foreach (var val in values)
            {
                expectedValues.Contains(val).Should().BeTrue();
            }
        }

        [Fact]
        public void ValidIntervalExprShouldWork()
        {
            var field = new CronField(CronFieldType.Minute, 0, 59);

            var result = field.Parse("*/20");
            result.Should().BeTrue();
            field.IsAllValues.Should().BeFalse();
            field.IsRange.Should().BeFalse();
            field.IsInterval.Should().BeTrue();
            field.IsListOfValues.Should().BeFalse();


            var values = field.Values;
            var expectedValues = new[] { 0,20,40 };
            foreach (var val in values)
            {
                expectedValues.Contains(val).Should().BeTrue();
            }
        }

        private string GetEnumName(CronFieldType type)
        {
            return Enum.GetName(typeof(CronFieldType), type);
        }
    }
}
