using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
namespace backend.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class JobController : ControllerBase
    {
        private ApplicationDbContext _Context { get; }
        private IMapper _Mapper { get; }
        public JobController( ApplicationDbContext context, IMapper mapper )
        {
            _Context = context;
            _Mapper = mapper;
        }


        // CRUD

        // Create
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateJob( [FromBody] JobCreateDto dto)
        {
            var newJob = _Mapper.Map<Job>( dto );
            await _Context.Jobs.AddAsync( newJob );
            await _Context.SaveChangesAsync(); 

            return Ok( "Job Created Successfully" );
        }

        // Read
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<JobGetDto>>> GetJobs( )
        {
            var jobs = await _Context.Jobs.Include( job => job.Company ).OrderByDescending( q => q.CreatedAt ).ToListAsync();
            var convertedJobs = _Mapper.Map<IEnumerable<JobGetDto>>( jobs );

            return Ok( convertedJobs );
        }

        // Read (Get Job By ID)

        // Update

        // Delete
    }
}
