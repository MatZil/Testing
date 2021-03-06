﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
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

        public SurveysService(
            ISurveysRepository repository,
            IConfiguration configuration,
            IMapper mapper,
            IQuestionsRepository questionsRepository,
            IChoicesRepository choicesRepository
        )
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
                Title = newSurveyDto.Title,
                CreationDate = DateTime.Now
            };

            var surveyDto = _mapper.Map<NewSurveyDto>(newSurvey);
            var surveyId = await _repository.Create(newSurvey);

            if (newSurveyDto.Questions != null)
            {
                foreach (var question in newSurveyDto.Questions)
                {
                    var questionId = await _questionsRepository.Create(new Question()
                    {
                        SurveyId = surveyId,
                        QuestionText = question.QuestionText,
                        Type = question.Type
                    });

                    if (question.Type == QuestionTypeEnum.MultipleChoice && question.Choices != null)
                    {
                        foreach (var choice in question.Choices)
                        {
                            await _choicesRepository.Create(new Choice()
                            {
                                QuestionId = questionId,
                                ChoiceText = choice.ChoiceText
                            });
                        }
                    }
                }
            }

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
            var surveyDto = _mapper.Map<Survey,GetSurveyDto>(survey);

            return surveyDto;
        }
    }
}
