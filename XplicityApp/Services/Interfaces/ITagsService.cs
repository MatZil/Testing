using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Tags;

namespace XplicityApp.Services.Interfaces
{
    public interface ITagsService
    {
        Task<TagDto> GetById(int id);
        Task<ICollection<TagDto>> GetAll();
        Task<int> Create(NewTagDto newTagDto);
        Task<ICollection<TagDto>> FindByTitle(string tagTitle);
    }
}
