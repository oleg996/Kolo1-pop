using System.Data;
using System.Data.Common;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Tutorial9.DTO;

namespace Tutorial9.Services;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;
    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task DoSomethingAsync()
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        await connection.OpenAsync();

        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;

        // BEGIN TRANSACTION
        try
        {
            command.CommandText = "INSERT INTO Animal VALUES (@IdAnimal, @Name);";
            command.Parameters.AddWithValue("@IdAnimal", 1);
            command.Parameters.AddWithValue("@Name", "Animal1");

            await command.ExecuteNonQueryAsync();

            command.Parameters.Clear();
            command.CommandText = "INSERT INTO Animal VALUES (@IdAnimal, @Name);";
            command.Parameters.AddWithValue("@IdAnimal", 2);
            command.Parameters.AddWithValue("@Name", "Animal2");

            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
        // END TRANSACTION
    }

    public async Task ProcedureAsync()
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        await connection.OpenAsync();

        command.CommandText = "NazwaProcedury";
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", 2);

        await command.ExecuteNonQueryAsync();

    }


    public async Task<Progect> GetProgectData(int id)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand("select pp.ProjectId , pp.Objective , pp.StartDate,pp.EndDate,a.Name,a.OriginDate , i.InstitutionId,i.Name,i.FoundedYear ,s.FirstName ,s.LastName ,s.HireDate , sa.[Role]  from Artifact a ,Institution i,Preservation_Project as pp  left join Staff_Assignment as sa on  sa.ProjectId  = pp.ProjectId  left join Staff as s on s.StaffId  = sa.StaffId  where pp.ProjectId  = @id and a.ArtifactId = pp.ArtifactId and i.InstitutionId  = a.InstitutionId ", connection);


        command.Parameters.Add("@id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();

        Progect progect = new Progect();

        using (var reader = await command.ExecuteReaderAsync())
        {

            await reader.ReadAsync();

            progect.ProgectID = reader.GetInt32(0);
            progect.objective = reader.GetString(1);
            progect.startDate = DateOnly.FromDateTime(reader.GetDateTime(2));


            if (!reader.IsDBNull(3))
                progect.endDate = DateOnly.FromDateTime(reader.GetDateTime(3));

            progect.artifact = new Artifact()
            {
                name = reader.GetString(4),
                originDate = DateOnly.FromDateTime(reader.GetDateTime(5)),
                institution = new Institution() { institutionId = reader.GetInt32(6), name = reader.GetString(7), foundedYear = reader.GetInt32(8) }

            };

            progect.staffAssignments = new List<staffAssignments>();

            if (!reader.IsDBNull(9))
            {
                progect.staffAssignments.Add(new staffAssignments()
                {
                    firstName = reader.GetString(9),
                    lastName = reader.GetString(10),
                    hireDate = DateOnly.FromDateTime(reader.GetDateTime(11)),
                    role = reader.GetString(12)
                });
            }





            while (await reader.ReadAsync())
                {
                    progect.staffAssignments.Add(new staffAssignments()
                    {
                        firstName = reader.GetString(9),
                        lastName = reader.GetString(10),
                        hireDate = DateOnly.FromDateTime(reader.GetDateTime(11)),
                        role = reader.GetString(12)
                    });


                }
        }



        return progect;








    }


    public async Task<Boolean> DoesProgectExists(int id)
    {

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand("select count(1) from Preservation_Project pp  where pp.ProjectId  = @id", connection);


        command.Parameters.Add("@id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();


        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {

                if (reader.GetInt32(0) > 0)
                {
                    return true;
                }

            }

        }

        return false;
    }

    public async Task<Boolean> DoesInstitutionExists(int id)
    {

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand("select count(1) from Institution i  where i.InstitutionId   = @id", connection);


        command.Parameters.Add("@id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();


        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {

                if (reader.GetInt32(0) > 0)
                {
                    return true;
                }

            }

        }

        return false;
    }

    public async Task AddProgect(NewArtifactProgect newArtifactProgect)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand("insert into Preservation_Project values(@id , @aid,@sd,@ed,@obg)", connection);

        await connection.OpenAsync();

        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;


        try
        {
            command.CommandText = "insert into Artifact values(@id , @n,@od,@inst)";

            command.Parameters.Add("@id", SqlDbType.Int).Value = newArtifactProgect.artifact.artifactId;
            command.Parameters.Add("@n", SqlDbType.VarChar).Value = newArtifactProgect.artifact.name;
            command.Parameters.Add("@od", SqlDbType.Date).Value = newArtifactProgect.artifact.originDate;
            command.Parameters.Add("@inst", SqlDbType.Int).Value = newArtifactProgect.artifact.institutionId;


            await command.ExecuteNonQueryAsync();



            command.Parameters.Clear();
            command.CommandText = "insert into Preservation_Project values(@id , @aid,@sd,@ed,@obg)";




            command.Parameters.Add("@id", SqlDbType.Int).Value = newArtifactProgect.project.projectId;
            command.Parameters.Add("@aid", SqlDbType.Int).Value = newArtifactProgect.artifact.artifactId;
            command.Parameters.Add("@sd", SqlDbType.Date).Value = newArtifactProgect.project.startDate;
            if (newArtifactProgect.project.endDate != null)
                command.Parameters.Add("@ed", SqlDbType.Date).Value = newArtifactProgect.project.endDate;
            else
                command.Parameters.Add("@ed", SqlDbType.Date).Value = DBNull.Value;
            command.Parameters.Add("@obg", SqlDbType.VarChar).Value = newArtifactProgect.project.objective;

            

            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();

        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }





    }

}