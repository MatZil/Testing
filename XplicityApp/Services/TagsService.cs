using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Tags;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;
using XplicityApp.Infrastructure.Database.Models;
using AutoMapper;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Services
{
    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _repository;
        private readonly IMapper _mapper;

        public TagsService(ITagsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Create(NewTagDto newTagDto)
        {
            await ValidateTag(newTagDto);

            var tag = _mapper.Map<Tag>(newTagDto);
            var newTagId = await _repository.Create(tag);

            return newTagId;
        }

        public async Task<ICollection<TagDto>> FindByTitle(string tagTitle)
        {
            if (tagTitle == null)
            {
                throw new ArgumentNullException();
            }

            var tags = await _repository.FindByTitle(tagTitle);
            var tagsDto = _mapper.Map<TagDto[]>(tags);

            return tagsDto;
        }

        public async Task<ICollection<TagDto>> GetAll()
        {
            var tags = await _repository.GetAll();
            var tagsDto = _mapper.Map<TagDto[]>(tags);

            return tagsDto;
        }

        public async Task<TagDto> GetById(int id)
        {
            var tag = await _repository.GetById(id);
            var tagDto = _mapper.Map<TagDto>(tag);

            return tagDto;
        }

        private async Task ValidateTag(NewTagDto newTagDto)
        {
            if (newTagDto == null)
            {
                throw new ArgumentNullException(nameof(newTagDto));
            }
            else if (await _repository.TagExists(newTagDto.Title))
            {
                throw new ValidationException("Tag already exists");
            }
            else if (newTagDto.Title.Length < 3)
            {
                throw new ValidationException("Tag is too short");
            }
            else if (newTagDto.Title.Length > 10)
            {
                throw new ValidationException("Tag is too long");
            }

            var regexForTagValidation = new Regex("^[a-zA-Z0-9#-]*$");

            if (!regexForTagValidation.IsMatch(newTagDto.Title))
            {
                throw new ValidationException("Tag has invalid characters");
            }
        }
    }
}
