using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Data.Dto
{
    public class VersionColorDetailDto
    {
        public string VersionName { get; set; }

        public Guid ColorId {  get; set; }
        
        public int Quantity { get; set; }

        public string Image {  get; set; }
    }
}
