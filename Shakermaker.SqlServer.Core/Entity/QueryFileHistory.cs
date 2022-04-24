using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shakermaker.SqlServer.Core.Entity
{
    public class QueryFileHistory
    {
        [Key]
        public Guid QueryFileHistoryId { get; set; }
        [ForeignKey("QueryExecution")]
        public Guid QueryExecutionId { get; set; }
        public QueryExecution QueryExecution { get; set; }
        public string QueryType { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public DateTimeOffset ExecutionStartDate { get; set; }
        public DateTimeOffset? ExecutionEndDate { get; set; }
    }
}
