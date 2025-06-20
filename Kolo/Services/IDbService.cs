namespace Tutorial9.Services;
using Tutorial9.DTO;

public interface IDbService
{
    Task DoSomethingAsync();
    Task ProcedureAsync();

    Task<Progect> GetProgectData(int id);

    Task<Boolean> DoesProgectExists(int id);

    Task<Boolean> DoesInstitutionExists(int id);

    Task AddProgect(NewArtifactProgect newArtifactProgect);
}