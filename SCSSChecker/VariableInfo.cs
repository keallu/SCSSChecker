namespace SCSSChecker
{
    public class VariableInfo
    {
        public string? Name { get; set; }
        public bool Ignore { get; set; } = false;
        public List<VariableMatch> Matches { get; set; } = new List<VariableMatch>();
    }
}
