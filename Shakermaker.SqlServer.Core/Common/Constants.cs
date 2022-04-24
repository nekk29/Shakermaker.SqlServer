namespace Shakermaker.SqlServer.Core.Common
{
    public class Constants
    {
        public class Schema
        {
            public const string MainSchema = "sci";
        }

        public class Table
        {
            public const string QueryExecution = "QueryExecution";
            public const string QueryFileHistory = "QueryFileHistory";
        }

        public class QueryDirectory
        {
            public const string Code_Functions = "Code/Functions";
            public const string Code_Procedures = "Code/Procedures";
            public const string Data_Environments = "Data/Environments";
            public const string Data_Fixes = "Data/Fixes";
            public const string Data_Settings = "Data/Settings";
            public const string Schema_Sequences = "Schema/Sequences";
            public const string Schema_Synonyms = "Schema/Synonyms";
            public const string Schema_Tables = "Schema/Tables";
            public const string Schema_Types = "Schema/Types";
            public const string Schema_Views = "Schema/Views";
        }

        public class Result
        {
            public class QueryExecution
            {
                public const string Created = "Created";
                public const string InProgress = "InProgress";
                public const string Error = "Error";
                public const string Succeded = "Succeded";
            }
        }
    }
}
