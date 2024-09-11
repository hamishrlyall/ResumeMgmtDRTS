using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private ApplicationDbContext _Context { get; }
        private IMapper _Mapper { get; }
        public CandidateController( ApplicationDbContext context, IMapper mapper )
        {
            _Context = context;
            _Mapper = mapper;
        }


        // CRUD

        // Create
        [HttpPost]
        [Route( "Create" )]
        public async Task<IActionResult> CreateCandidate( [FromForm] CandidateCreateDto dto, IFormFile pdfFile )
        {
            // First => Save pdf to Server
            // Then => save url into our entity
            var fiveMegaByte = 5 * 1024 * 1024;
            var pdfMimeType = "application/pdf";
            
            // Validation
            if( pdfFile.Length > fiveMegaByte || pdfFile.ContentType != pdfMimeType )
            {
                return BadRequest( "File is not valid" );
            }

            // Saves file to documents/pdfs folder
            var resumeUrl = Guid.NewGuid( ).ToString( ) + ".pdf";
            var filePath = Path.Combine( Directory.GetCurrentDirectory( ), "documents", "pdfs", resumeUrl );
            using(var stream = new FileStream( filePath, FileMode.Create ) )
            {
                await pdfFile.CopyToAsync( stream );
            }

            var newCandidate = _Mapper.Map<Candidate>( dto );
            newCandidate.ResumeUrl = resumeUrl;
            await _Context.Candidates.AddAsync( newCandidate );
            await _Context.SaveChangesAsync( );

            return Ok( "Candidate Created Successfully" );
        }

        // Read
        [HttpGet]
        [Route( "Get" )]
        public async Task<ActionResult<IEnumerable<CandidateGetDto>>> GetCandidate( )
        {
            var candidates = await _Context.Candidates.Include( candidate => candidate.Job ).OrderByDescending( q => q.CreatedAt ).ToListAsync( );
            var convertedCandidates = _Mapper.Map<IEnumerable<CandidateGetDto>>( candidates );

            return Ok( convertedCandidates );
        }

        // Read (Download Pdf File)
        [HttpGet]
        [Route("download/{url}")]
        public IActionResult DownloadPdfFile( string url )
        {
            var filePath = Path.Combine( Directory.GetCurrentDirectory( ), "documents", "pdfs", url );

            //Validation
            if(!System.IO.File.Exists( filePath )) 
            {
                return NotFound( "File Not Found" );
            }

            var pdfBytes = System.IO.File.ReadAllBytes( filePath );
            var file = File( pdfBytes, "application/pdf", url );

            return file;

        }

        // Read (Get Job By ID)

        // Update

        // Delete
    }
}
