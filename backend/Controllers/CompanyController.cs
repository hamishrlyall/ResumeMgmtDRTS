using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace backend.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ApplicationDbContext _Context { get; }
        private IMapper _Mapper { get; }
        public CompanyController(ApplicationDbContext context, IMapper mapper )
        {
            _Context = context;
            _Mapper = mapper;
        }

        // CRUD

        // Create
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCompany( [FromBody] CompanyCreateDto dto)
        {
            var newCompany = _Mapper.Map<Company>( dto );
            await _Context.Companies.AddAsync( newCompany );
            await _Context.SaveChangesAsync();

            return Ok( "Company Created Successfully" );
        }

        // Read
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<CompanyGetDto>>> GetCompanies( )
        {
            var companies = await _Context.Companies.OrderByDescending( q => q.CreatedAt).ToListAsync( );
            var convertedCompanies = _Mapper.Map<IEnumerable<CompanyGetDto>>( companies );

            return Ok( convertedCompanies );
        }

        // Read (Get Company By ID)

        // Update

        // Delete
    }
}
