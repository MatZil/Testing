using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Dtos.Surveys.Questions;
using XplicityApp.Dtos.Surveys.Questions.Choices;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class SurveysService : ISurveysService
    {
        private readonly ISurveysRepository _repository;
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IChoicesRepository _choicesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SurveysService(ISurveysRepository repository, IQuestionsRepository questionsRepository, IChoicesRepository choicesRepository,
                              IConfiguration configuration, IMapper mapper)
        {
            _repository = repository;
            _questionsRepository = questionsRepository;
            _choicesRepository = choicesRepository;
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
            //var a = "a";

            await _choicesRepository.Create(new Choice()
            {
                QuestionId = 1,
                ChoiceText = "kkkkk",
                Id = 10
            });

            if (newSurveyDto == null)
            {
                throw new ArgumentNullException();
            }

            var guid = Guid.NewGuid().ToString();

            var newSurvey = new Survey
            {
                Guid = guid,
                AuthorId = newSurveyDto.AuthorId,
                AnonymousAnswers = newSurveyDto.AnonymousAnswers,
                Title = newSurveyDto.Title
            };

            var surveyId = await _repository.Create(newSurvey);

            var surveyDto = _mapper.Map<NewSurveyDto>(newSurvey);



            //if (newSurveyDto.Questions != null)
            //{
            //    foreach (var question in newSurveyDto.Questions)
            //    {
            //        //var questionId = await _questionsRepository.Create(new Question()
            //        //{
            //        //    SurveyId = surveyId,
            //        //});

            //        if (question.Choices != null)
            //        {
            //            foreach (var choiceDto in question.Choices)
            //            {
            //                choiceDto.QuestionId = 1;
            //                var choice = _mapper.Map<Choice>(choiceDto);
            //                await _choicesRepository.Create(choice);
            //            }
            //        }
            //    }
            //}

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
