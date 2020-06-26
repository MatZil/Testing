using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface ISurveysService
    {
        Task<GetSurveyDto> GetById(int id);
        Task<ICollection<GetSurveyDto>> GetAll();
        Task<NewSurveyDto> Create(NewSurveyDto newSurvey);
        Task Update(int id, UpdateSurveyDto updateData);
        Task<bool> Delete(int id);
        Task<GetSurveyDto> GetByGuid(string guid);
    }
}
