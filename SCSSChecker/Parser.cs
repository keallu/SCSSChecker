using System.Text.RegularExpressions;

namespace SCSSChecker
{
    public static class Parser
    {
        public static void FindVariables(string input, out List<string> declared, out List<string> referenced)
        {
            declared = new();
            referenced = new();

            Regex variables = new(@"[$] ?((?!\d)[\w_-][\w\d_-]*)(\([^\)]+.)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches;
            GroupCollection groups;

            string[] lines = input.Split(new string[] { Environment.NewLine, ";" } , StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                matches = variables.Matches(line);

                foreach (Match match in matches)
                {
                    groups = match.Groups;

                    if (match.Groups[1].Index > 1)
                    {
                        referenced.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        declared.Add(match.Groups[1].Value);
                    }
                }
            }            
        }

        public static string RemoveComments(string input)
        {
            string output = string.Empty;

            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)(\r?\n|$)";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";

            output = Regex.Replace(input,
            blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
            o =>
            {
                if (o.Value.StartsWith(@"//"))
                {
                    return Environment.NewLine;
                }

                if (o.Value.StartsWith(@"/*"))
                {
                    int newLineCount = o.Value.Split(Environment.NewLine).Length - 1;

                    return string.Concat(Enumerable.Repeat(Environment.NewLine, newLineCount));
                }

                return o.Value;
            },
            RegexOptions.Singleline);

            return output;
        }
    }
}
