using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tutorial9.Services;
using Tutorial9.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Tutorial9.Controllers
{

    [Route("api")]
    [ApiController]
    public class Controller : ControllerBase
    {
        private readonly IConfiguration _confyguration;

        private readonly IDbService _dbservice;

        public Controller(IConfiguration configuration, IDbService Dbservice)
        {
            _confyguration = configuration;
            _dbservice = Dbservice;
        }



        [HttpGet("projects/{id}")]
        public async Task<IActionResult> getProgect(int id)
        {

            if (!await _dbservice.DoesProgectExists(id))
                return NotFound("Project does not exists");

            return Ok(await _dbservice.GetProgectData(id));
        }

        [HttpPost("artifacts")]

        public async Task<IActionResult> addProgect(NewArtifactProgect progect)
        {

            if (await _dbservice.DoesProgectExists(progect.project.projectId))
                return BadRequest("Project with such id exists");

            if (!await _dbservice.DoesInstitutionExists(progect.artifact.institutionId))
                return BadRequest("institution with such id not exists");

            try
            {
                await _dbservice.AddProgect(progect);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Created();
        }



    }
}