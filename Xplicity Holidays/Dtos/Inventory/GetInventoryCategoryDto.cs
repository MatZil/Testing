using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Dtos.Inventory
{
    public class GetInventoryCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Normative { get; set; }
    }
}
