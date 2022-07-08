namespace SCSSChecker.Tests
{
    public class ValidatorShould
    {
        [Fact]
        public void ParseEmptyStringToEmptyResult()
        {
            Validator sut = new();

            ValidatorResult result = sut.Parse("");

            Assert.Empty(result.Variables);
        }

        [Fact]
        public void ParseSingleVariableToSingleResult()
        {
            Validator sut = new();

            ValidatorResult result = sut.Parse("$single-variable: #ff0000;");

            Assert.Single(result.Variables);
        }

        [Fact]
        public void ParseTwoVariablesToTwoResultsWithoutEndOfLine()
        {
            Validator sut = new();

            ValidatorResult result = sut.Parse("$first-variable: #ff0000;$second-variable: #0000ff;");

            Assert.Equal(2, result.Variables.Count);
        }

        [Fact]
        public void ParseTwoVariablesToTwoResultsWithtEndOfLine()
        {
            Validator sut = new();

            ValidatorResult result = sut.Parse("$first-variable: #ff0000;\r\n$second-variable: #0000ff;");

            Assert.Equal(2, result.Variables.Count);
        }

        [Fact]
        public void ParseVariablesTrimmingLineComments()
        {
            ValidatorProperties properties = new()
            {
                TrimComments = true,
            };

            Validator sut = new(properties);

            ValidatorResult result = sut.Parse("$first-variable: #ff0000;\r\n//$second-variable: #0000ff;\r\n$third-variable: #0000ff;");

            Assert.Equal(2, result.Variables.Count);
        }

        [Fact]
        public void ParseVariablesIgnoringLineComments()
        {
            ValidatorProperties properties = new()
            {
                TrimComments = false,
            };

            Validator sut = new(properties);

            ValidatorResult result = sut.Parse("$first-variable: #ff0000;\r\n//$second-variable: #0000ff;\r\n$third-variable: #0000ff;");

            Assert.Equal(3, result.Variables.Count);
        }

        [Fact]
        public void ParseVariablesTrimmingBlockComments()
        {
            ValidatorProperties properties = new()
            {
                TrimComments = true,
            };

            Validator sut = new(properties);

            ValidatorResult result = sut.Parse("$first-variable: #ff0000;\r\n/*$second-variable: #0000ff;\r\n$third-variable: #0000ff;\r\n$fourth-variable: #0000ff;*/\r\n$fifth-variable: #0000ff;");

            Assert.Equal(2, result.Variables.Count);
        }

        [Fact]
        public void ParseVariablesIgnoringBlockComments()
        {
            ValidatorProperties properties = new()
            {
                TrimComments = false,
            };

            Validator sut = new(properties);

            ValidatorResult result = sut.Parse("$first-variable: #ff0000;\r\n/*$second-variable: #0000ff;\r\n$third-variable: #0000ff;\r\n$fourth-variable: #0000ff;*/\r\n$fifth-variable: #0000ff;");

            Assert.Equal(5, result.Variables.Count);
        }
    }
}