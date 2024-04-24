using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Data.Dto
{
    public class ResultModel
    {
        public object Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
