using Microsoft.EntityFrameworkCore;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class SurveysRepository: RepositoryBase<Survey>
    {
        protected override DbSet<Survey> ItemSet { get; }

        public SurveysRepository(HolidayDbContext context): base(context)
        {
            ItemSet = context.Surveys;
        }
    }
}
