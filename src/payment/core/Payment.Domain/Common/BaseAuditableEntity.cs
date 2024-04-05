using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Common
{
    public class BaseAuditableEntity
    {
        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? LastUpdatedBy { get; set;}

        public DateTime? LastUpdateDate {  get; set; }
    }
}
