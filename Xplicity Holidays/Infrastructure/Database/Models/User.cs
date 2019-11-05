using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class User : IdentityUser
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}
