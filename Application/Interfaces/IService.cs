namespace Application.Interfaces;

public interface IService<TDto, in TCreateDto, in TUpdateDto>
{
    Task<TDto> GetByIdAsync(Guid id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> CreateAsync(TCreateDto dto);
    Task<TDto> UpdateAsync(TUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}