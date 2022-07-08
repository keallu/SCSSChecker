using SCSSChecker;
using System.Reflection;

try
{
    Console.WriteLine("");
    Console.WriteLine("SCSS Checker version: " + Assembly.GetExecutingAssembly().GetName().Version);
    Console.WriteLine("");

    string path = string.Empty;
    string ignore = string.Empty;

    string[] arguments = ExtractAndTrimArguments();

    if (arguments.Length > 1)
    {
        if (arguments.Contains("?"))
        {
            PrintHelp();
        }
        else
        {
            path = arguments[1];

            if (arguments.Length > 2)
            {
                ignore = arguments[2];
            }
        }
    }
    else
    {
        path = Directory.GetCurrentDirectory();
    }

    if (path != string.Empty)
    {
        Console.WriteLine("Scanning {0}", path);
        Console.WriteLine("");

        DirectoryInfo directory;
        FileInfo[]? files = null;

        ValidatorProperties validatorProperties = new()
        {
            TrimComments = true,
        };

        Validator validator = new(validatorProperties);
        ValidatorResult validatorResult = new();

        if (File.Exists(path))
        {
            files = new FileInfo[] { new FileInfo(path) };
            validatorResult = validator.Parse(files, false, validatorResult);
        }
        else if (Directory.Exists(path))
        {
            directory = new DirectoryInfo(path);
            files = directory.GetFiles("*.scss", SearchOption.AllDirectories);
            validatorResult = validator.Parse(files, false, validatorResult);
        }
        else
        {
            Console.WriteLine("{0} is not a valid file or directory.", path);
        }

        if (files != null)
        {
            if (files.Length > 0 && validatorResult.Variables.Count > 0)
            {
                if (ignore != string.Empty && File.Exists(ignore))
                {
                    files = new FileInfo[] { new FileInfo(ignore) };
                    validatorResult = validator.Parse(files, true, validatorResult);
                }

                PrintVariables("Unused", validatorResult.GetUnusedVariables());
                PrintVariables("Undeclared", validatorResult.GetUndeclaredVariables());
            }
            else
            {
                Console.WriteLine("No variables found in {0} files.", files.Length);
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Exception: " + ex.Message);
}

static string[] ExtractAndTrimArguments()
{
    string[] arguments = Environment.GetCommandLineArgs();

    for (int i = 0; i < arguments.Length; i++)
    {
        arguments[i] = arguments[i].Trim('/', '-');
    }

    return arguments;
}

static void PrintVariables(string name, List<VariableInfo> variables)
{
    Console.WriteLine(name + " variables:");
    Console.WriteLine($"{"Name",-40} {"File",-60} {"Line Number",-20}");
    Console.WriteLine($"{"----",-40} {"----",-60} {"-----------",-20}");

    foreach (VariableInfo variable in variables)
    {
        foreach (VariableMatch match in variable.Matches)
        {
            Console.WriteLine($"{variable.Name,-40} {match.File,-60} {match.Line,-20}");
        }
    }

    Console.WriteLine("");
}

static void PrintHelp()
{
    Console.WriteLine("Checks SCSS files for unused and undeclared variables.");
    Console.WriteLine("");
    Console.WriteLine("Usage: scsschecker [path]]");
    Console.WriteLine("");
}