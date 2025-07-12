using Energomera.DTO;

namespace Energomera.DAL.Repositories;

public interface IFieldRepository
{
    List<FieldDto> GetAll();
    FieldDto? GetById(int id);
}
