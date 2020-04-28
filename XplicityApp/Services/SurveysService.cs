using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class SurveysService : ISurveysService
    {
        private readonly IRepository<Survey> _repository;
        private readonly IMapper _mapper;

        public SurveysService(IRepository<Survey> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

            var newSurvey = _mapper.Map<Survey>(newSurveyDto);
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
    }
}
