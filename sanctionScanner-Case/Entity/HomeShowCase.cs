using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanctionScanner_Case.Entity
{
    public class HomeShowCase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AdvertName { get; set; }
        public decimal Price { get; set; }
    }
}
