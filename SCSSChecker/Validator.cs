namespace SCSSChecker
{
    public class Validator
    {
        public ValidatorProperties Properties { get; }

        public Validator()
        {
            Properties = new();
        }
        public Validator(ValidatorProperties properties)
        {
            Properties = properties;
        }

        public ValidatorResult Parse(string input, string? path = null, bool ignore = false, ValidatorResult? result = null)
        {
            if (result == null)
            {
                result = new ValidatorResult();
            }

            if (Properties.TrimComments)
            {
                input = Parser.RemoveComments(input);
            }

            string[] lines = input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                Parser.FindVariables(lines[i], out List<string> declared, out List<string> referenced);

                result.Add(declared, referenced, path, ignore, i + 1);
            }

            return result;
        }

        public ValidatorResult Parse(FileInfo file, bool ignore = false, ValidatorResult? result = null)
        {
            if (result == null)
            {
                result = new ValidatorResult();
            }

            if (file.Exists)
            {
                result = Parse(File.ReadAllText(file.FullName), file.FullName, ignore, result);
            }

            return result;
        }

        public ValidatorResult Parse(FileInfo[] files, bool ignore = false, ValidatorResult? result = null)
        {
            if (result == null)
            {
                result = new ValidatorResult();
            }

            foreach (FileInfo file in files)
            {
                Parse(file, ignore, result);
            }

            return result;
        }
    }
}
