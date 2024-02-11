using Tyts.BLambda.WhiteTrash;

namespace Tyts.BLambda.Shell.Domain.Auth;

public class Attr
{
    public const string SEP = "://";

    public static class Scopes
    {
        public class Scope
        {
            internal const string CUSTOM = "custom:";

            public string Name { get; }
            public Func<string, string> Get;
            public Func<string, string> Set;

            /// <summary>
            /// Name without "custom:" prefix
            /// </summary>
            public string CdkName => Name.TrimStart(CUSTOM);            

            public Scope(string name)
            {
                Name = $"{CUSTOM}{name}";
                Get = (val) => val.TrimStart($"{name}{SEP}");
                Set = (val) => $"{name}{SEP}{val}";
            }
        }

        public static Scope Site => new("site");

        public static Scope Tariff => new("tariff")
        {
            Get = (val) => val,
            Set = (val) => val
        };
    }
}
