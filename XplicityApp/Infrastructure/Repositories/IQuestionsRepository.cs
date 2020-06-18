using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IQuestionsRepository : IRepository<Question>
    {
        public Task<ICollection<Question>> GetAllBySurveyId(int surveyId);
    }
}