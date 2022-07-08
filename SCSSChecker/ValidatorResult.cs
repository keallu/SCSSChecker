namespace SCSSChecker
{
    public class ValidatorResult
    {
        public Dictionary<string, VariableInfo> Variables { get; }

        public ValidatorResult()
        { 
            Variables = new Dictionary<string, VariableInfo>();            
        }

        public void Add(VariableMatchType type, string name, string? path = null, bool ignore = false, int? line = null)
        {
            if (!Variables.ContainsKey(name))
            {
                Variables.Add(name, new VariableInfo
                {
                    Name = name,
                    Ignore = ignore,
                });
            }

            VariableMatch variableMatch = new()
            {
                Type = type,
                File = path,
                Line = line,
            };

            Variables[name].Matches.Add(variableMatch);
        }
        public void Add(List<string> declared, List<string> referenced, string? path = null, bool ignore = false, int? line = null)
        {
            foreach (string variable in declared)
            {
                Add(VariableMatchType.Declaration, variable, path, ignore, line);
            }

            foreach (string variable in referenced)
            {
                Add(VariableMatchType.Reference, variable, path, ignore, line);
            }
        }

        public List<VariableInfo> GetUnusedVariables()
        {
            return Variables.Where(o => !o.Value.Ignore && o.Value.Matches.All(o => o.Type == VariableMatchType.Declaration)).Select(o => o.Value).ToList();
        }

        public List<VariableInfo> GetUndeclaredVariables()
        {
            return Variables.Where(o => !o.Value.Ignore && o.Value.Matches.All(o => o.Type == VariableMatchType.Reference)).Select(o => o.Value).ToList();
        }

        public List<VariableInfo> GetMultiDeclaredVariables()
        {
            throw new NotImplementedException();
        }
    }   
}
