using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Row
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class JsonRoot
    {
        public Root root { get; set; }
    }

    public class Root
    {
        public List<Row> row { get; set; }
    }
}
