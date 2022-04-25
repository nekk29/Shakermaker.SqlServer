using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shakermaker.SqlServer.Core.Entity
{
    public class QueryExecution
    {
        public QueryExecution()
        {
            QueryFileHistorys = new HashSet<QueryFileHistory>();
        }

        [Key]
        public Guid QueryExecutionId { get; set; }
        public string Application { get; set; }
        public string Release { get; set; }
        public string Environment { get; set; }
        public string Result { get; set; }
        public string Log { get; set; }
        public string ExecutionUser { get; set; }
        public DateTimeOffset ExecutionStartDate { get; set; }
        public DateTimeOffset? ExecutionEndDate { get; set; }

        public virtual ICollection<QueryFileHistory> QueryFileHistorys { get; set; }
    }
}
