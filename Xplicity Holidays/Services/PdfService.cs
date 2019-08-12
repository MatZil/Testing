using System;
using System.Text;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.PdfGeneration;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class PdfService : IPdfService
    {
        private readonly IPdfGenerator _generator;
        private readonly ITimeService _timeService;

        public PdfService(IPdfGenerator generator, ITimeService timeService)
        {
            _generator = generator;
            _timeService = timeService;
        }
        public void CreateRequestPdf(Holiday holiday, Employee employee)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat(@"
<html>
    <body>
        <br />
        <p class='center over'>
            {0}
        </p>
        <p class='small center'>
            (pareigos)
        </p>
        <p class='center over'>
            {1} {2}      
        </p>
        <p class='small center'>
            (vardas pavardė)
        </p>
        <p class='left'>
            Uždarosios akcinės bendrovės „Xplicity“
        </p>
        <p class='left'>
            Operacijų ir personalo vadovei Ingai Ranai
        </p>
        <br /><br /><br /> 
        <p class='center bold big over'>
            PRAŠYMAS
        </p>
        <p class='center bold big'>
            DĖL {3} ATOSTOGŲ
        </p>
        <br /><br />
        <p class='center over'>
            {4}
        </p>
        <p class='center small'>
            (data)
        </p>
        <p class='center'>
            Kaunas
        </p>
        <br /><br /><br />
        <p class='center'>
            Prašau išleisti mane {5} atostogų nuo {6} iki {7}. Viso: {8} d.d. 
            Atostoginius prašau sumokėti kartu su atlyginimu.
        </p>
        <br /><br /><br /><br />
        <p class='center over'>
            ................................ {9} {10}
        </p>
        <p class='center small moveleft'>
            (parašas)
        </p>
        <br /><br /><br /><br /><br /><br />
        <p class='center over'>
            Tvirtinu: ..................................... Inga Rana
        </p>
        <p class='center small'>
            (parašas)
        </p>
    </body>
</html>", employee.Position, employee.Name, employee.Surname,
         (holiday.Type.ToString() == "Annual") ? "KASMETINIŲ" : (holiday.Type.ToString() == "Science") ? "MOKSLO" : "TĖVYSTĖS/MOTINYSTĖS", 
         _timeService.GetCurrentTime().ToShortDateString(), 
         (holiday.Type.ToString() == "Annual") ? "kasmetinių" : (holiday.Type.ToString() == "Science") ? "mokslo" : "tėvystės/motinystės",
          holiday.FromInclusive.ToShortDateString(), holiday.ToExclusive.ToShortDateString(),
         (holiday.ToExclusive.Date - holiday.FromInclusive.Date).TotalDays, employee.Name, employee.Surname);

            _generator.GeneratePdf(strBuilder.ToString(), holiday.Id, "request");
        }

        public void CreateOrderPdf(Holiday holiday, Employee employee)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat(@"
<html>
    <body>
        <br />
        <p class='big bold center over'>
            UŽDAROSIOS AKCINĖS BENDROVĖS „XPLICITY“
        </p>
        <p class='big bold center'>
            OPERACIJŲ IR PERSONALO VADOVĖ
        </p>
        <br /><br />
        <p class='big bold center over'>
            ĮSAKYMAS
        </p>
        <p class='big bold center'>
            DĖL ATOSTOGŲ SUTEIKIMO NR. {0}
        </p>
        <br />
        <p class='center over'>
            {1} m. {2} mėn {3} d.
        <p class='center'>
            KAUNAS
        </p>
        <br /><br /><br /><br />
        <p class='center'>
            Atsižvelgiant į {4} {5} {6} prašymą, suteikiu {7} {8}
            neapmokamas {9} atostogas nuo {10} iki {11} ({12} d.d. )   
        </p>
        <br /><br /><br /><br /><br /><br /><br />
        <p class='floatL'>
            Operacijų ir personalo vadovė
        </p>
        <p class='floatL moveright'>
            (Parašas)
        </p>
        <p class='floatR moveleft'>
            Inga Rana
        </p>
        <br />
        <p class='right small'> 
            (Vardas ir pavardė)
        </p>
    </body>
</html>", holiday.Id, _timeService.GetCurrentTime().Year, _timeService.GetCurrentTime().Month, _timeService.GetCurrentTime().Day,
          holiday.RequestCreatedDate.ToShortDateString(), employee.Name, employee.Surname, employee.Name, employee.Surname, 
          (holiday.Type.ToString() == "Annual" ? "kasmetines" :  (holiday.Type.ToString() == "Science" ? "mokslo" : "tėvystės/motinystės")),
          holiday.FromInclusive.ToShortDateString(), holiday.ToExclusive.ToShortDateString(), 
          (holiday.ToExclusive - holiday.FromInclusive).Days);
            _generator.GeneratePdf(strBuilder.ToString(), holiday.Id, "order");
        }
    }
}
