﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Dtos.EmailTemplates
{
    public class NewEmailTemplateDto
    {
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
    }
}
