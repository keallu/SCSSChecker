namespace SCSSChecker.Tests
{
    public class ParserShould
    {
        [Theory]
        [InlineData("$single-variable: #ff0000;", 1, 0)]
        [InlineData("$first-variable: #ff0000;$second-variable: #0000ff;", 2, 0)]
        [InlineData("$first-variable: #ff0000;\r\n$second-variable: #0000ff;", 2, 0)]
        [InlineData("selector{\r\n$first-variable: #ff0000;\r\n$second-variable: #0000ff;\r\n}", 2, 0)]
        [InlineData("single-property: $single-variable;", 0, 1)]
        [InlineData("first-property: $single-variable;second-property: $second-variable;", 0, 2)]
        [InlineData("first-property: $single-variable;\r\nsecond-property: $second-variable;", 0, 2)]
        [InlineData("selector{\r\nfirst-property: $first-variable;\r\nsecond-property: $second-variable;\r\n}", 0, 2)]
        public void FindVariablesWithCorrectCount(string input, int expectedDeclared, int expectedReferenced)
        {
            Parser.FindVariables(input, out List<string> declared, out List<string> referenced);

            Assert.Equal(expectedDeclared, declared.Count);
            Assert.Equal(expectedReferenced, referenced.Count);
        }

        [Theory]
        [InlineData("$single-variable: #ff0000;", new string[] { "single-variable" }, new string[] { })]
        [InlineData("$first-variable: #ff0000;$second-variable: #0000ff;", new string[] { "first-variable", "second-variable" }, new string[] { })]
        [InlineData("$first-variable: #ff0000;\r\n$second-variable: #0000ff;", new string[] { "first-variable", "second-variable" }, new string[] { })]
        [InlineData("selector{\r\n$first-variable: #ff0000;\r\n$second-variable: #0000ff;\r\n}", new string[] { "first-variable", "second-variable" }, new string[] { })]
        [InlineData("single-property: $single-variable;", new string[] { }, new string[] { "single-variable" })]
        [InlineData("first-property: $first-variable;second-property: $second-variable;", new string[] { }, new string[] { "first-variable", "second-variable" })]
        [InlineData("first-property: $first-variable;\r\nsecond-property: $second-variable;", new string[] { }, new string[] { "first-variable", "second-variable" })]
        [InlineData("selector{\r\nfirst-property: $first-variable;\r\nsecond-property: $second-variable;\r\n}", new string[] { }, new string[] { "first-variable", "second-variable" })]
        public void FindVariablesWithCorrectNames(string input, string[] expectedDeclared, string[] expectedReferenced)
        {
            Parser.FindVariables(input, out List<string> declared, out List<string> referenced);

            Assert.Equal(expectedDeclared, declared.ToArray());
            Assert.Equal(expectedReferenced, referenced.ToArray());
        }

        [Theory]
        [InlineData("$single-variable: #ff0000;", "$single-variable: #ff0000;")]
        [InlineData("$first-variable: #ff0000;$second-variable: #0000ff;", "$first-variable: #ff0000;$second-variable: #0000ff;")]
        [InlineData("$first-variable: #ff0000;\r\n$second-variable: #0000ff;", "$first-variable: #ff0000;\r\n$second-variable: #0000ff;")]
        [InlineData("//$single-variable: #ff0000;", "\r\n")]
        [InlineData("//$first-variable: #ff0000;\r\n$second-variable: #0000ff;", "\r\n$second-variable: #0000ff;")]
        [InlineData("$first-variable: #ff0000;//$second-variable: #0000ff;", "$first-variable: #ff0000;\r\n")]
        [InlineData("/*$single-variable: #ff0000;*/", "")]
        [InlineData("$first-variable: #ff0000;/*$second-variable: #0000ff;*/", "$first-variable: #ff0000;")]
        [InlineData("$first-variable: #ff0000;/*$second-variable: #0000ff;*/third-variable: #00ff00;", "$first-variable: #ff0000;third-variable: #00ff00;")]
        [InlineData("$first-variable: #ff0000;\r\n/*$second-variable: #0000ff;*/\r\nthird-variable: #00ff00;", "$first-variable: #ff0000;\r\n\r\nthird-variable: #00ff00;")]
        [InlineData("$first-variable: #ff0000;/*$second-variable: #0000ff;third-variable: #00ff00;*/", "$first-variable: #ff0000;")]
        [InlineData("$first-variable: #ff0000;/*$second-variable: #0000ff;\r\nthird-variable: #00ff00;*/", "$first-variable: #ff0000;\r\n")]
        [InlineData("$first-variable: #ff0000;/*$second-variable: #0000ff;\r\n\r\n\r\nthird-variable: #00ff00;\r\nfourth-variable: #00ff00;\r\n*/", "$first-variable: #ff0000;\r\n\r\n\r\n\r\n\r\n")]
        public void RemoveComments(string input, string expected)
        {
            string output = Parser.RemoveComments(input);

            Assert.Equal(expected, output);
        }
    }
}
