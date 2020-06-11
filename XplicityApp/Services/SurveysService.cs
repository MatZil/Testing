using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class SurveysService : ISurveysService
    {
        private readonly ISurveysRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SurveysService(ISurveysRepository repository, IConfiguration configuration, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<GetSurveyDto> GetById(int id)
        {
            var survey = await _repository.GetById(id);
            var surveyDto = _mapper.Map<GetSurveyDto>(survey);

            return surveyDto;
        }

        public async Task<ICollection<GetSurveyDto>> GetAll()
        {
            var surveys = await _repository.GetAll();
            var surveysDto = _mapper.Map<GetSurveyDto[]>(surveys);

            return surveysDto;
        }

        public async Task<NewSurveyDto> Create(NewSurveyDto newSurveyDto)
        {
            if (newSurveyDto == null)
            {
                throw new ArgumentNullException(nameof(newSurveyDto));
            }

            var guid = Guid.NewGuid().ToString();
            var newSurvey = _mapper.Map<Survey>(newSurveyDto);

            newSurvey.Guid = guid;
            newSurvey.CreationDate = DateTime.Now;

            await _repository.Create(newSurvey);

            var surveyDto = _mapper.Map<NewSurveyDto>(newSurvey);

            return surveyDto;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
            {
                return false;
            }

            var deleted = await _repository.Delete(item);
            return deleted;
        }

        public async Task Update(int id, UpdateSurveyDto updateData)
        {
            if (updateData == null)
            {
                throw new ArgumentNullException(nameof(updateData));
            }

            var itemToUpdate = await _repository.GetById(id);

            if (itemToUpdate == null)
            {
                throw new InvalidOperationException();
            }

            _mapper.Map(updateData, itemToUpdate);
            await _repository.Update(itemToUpdate);
        }

        public async Task<GetSurveyDto> GetByGuid(string guid)
        {
            var survey = await _repository.GetByGuid(guid);
            var surveyDto = _mapper.Map<GetSurveyDto>(survey);

            return surveyDto;
        }
    }
}
