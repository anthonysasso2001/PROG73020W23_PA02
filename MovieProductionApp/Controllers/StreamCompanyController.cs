using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieProductionApp.Entities;
using MovieProductionApp.Models;

namespace MovieProductionApp.Controllers
{
    public class StreamCompanyController : Controller
    {
        public StreamCompanyController(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;

            //kind of weird to have the API handler here but needed to add stream companies
            //properly, and this controller is kind of the "front-end" to that API anyways
            _notifyHandler = new NotifyAPIHandler()
            {
                ProductionStudio = _movieDbContext.ProductionStudios
                .Select(p => new ProductionStudio()
                {
                    Name = p.Name,
                    ProductionStudioId = p.ProductionStudioId
                }).First(),
                StreamCompanies = _movieDbContext.StreamCompanies
                .Select(i => new StreamCompanyInfo()
                {
                    Name = i.Name,
                    webApiUrl = i.webApiUrl
                }).ToList()
            };
        }

        // GET: all streaming companies with their registered movies
        [HttpGet("/streaming-companies")]
        public IActionResult GetAllStreamingCompanies()
        {
            var allStreamCompanies = _movieDbContext.StreamCompanies
                .Include(s => s.RegisteredMovies)
                .OrderByDescending(s => s.Name)
                .Select(s => new StreamCompanyViewModel() { ActiveStreamCompany = s }).ToList();

            return View("Items", allStreamCompanies);
        }

        //GET: one streaming partner with their api information etc.
        [HttpGet("/streaming-companies/{id}")]
        public IActionResult GetStreamingCompanyById(int id)
        {
            var streamingCompany = _movieDbContext.StreamCompanies
                .Where(s => s.StreamCompanyInfoId == id).FirstOrDefault();

            StreamCompanyViewModel outViewModel = new StreamCompanyViewModel() { ActiveStreamCompany = streamingCompany };

            return View("Details", outViewModel);
        }

        [HttpGet("/streaming-companies/register")]
        public IActionResult RegisterStreamingCompanyRequest()
        {
            var newCompany = new StreamCompanyViewModel();
            return View("NewStreamCompany", newCompany);
        }

        [HttpPost("/streaming-companies")]
        public IActionResult AddStreamingCompany(StreamCompanyViewModel streamCompany)
        {
            var companyInfo = streamCompany.ActiveStreamCompany;

            if (null == companyInfo || null == companyInfo.Name || null == companyInfo.webApiUrl || null == companyInfo.challengeUrl)
                return BadRequest();

            //generate a custom guid or regenerate if it already exists
            while (null == companyInfo.StreamGUID)
            {
                companyInfo.StreamGUID = Guid.NewGuid();
                // if it exists re-run loop
                if (_movieDbContext.StreamCompanies.Any(s => s.StreamGUID == companyInfo.StreamGUID))
                    companyInfo.StreamGUID = null;
            }
            _movieDbContext.StreamCompanies.Add(companyInfo);
            //this is redundant but just to ensure notify handler is up to data, even if it is regenerated each time
            _notifyHandler.StreamCompanies.Add(companyInfo);

            return View("StreamCompany", new StreamCompanyViewModel() { ActiveStreamCompany = companyInfo });
        }
        private MovieDbContext _movieDbContext;
        private NotifyAPIHandler _notifyHandler;
    }
}

/*
		// GET api/streamers/exampleStream
		[HttpGet("/api/streamCompanies/{name}")]
		public JsonResult GetStreamingCompanyByName(string name)
		{
			var streamer = _movieDbContext.StreamCompanies
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApiUrl = s.webApiUrl
				}).Where(s => s.Name.Equals(name))
				.FirstOrDefault();

			return Json(streamer);
		}


// POST api/<NotifyAPIController>
		[HttpPost("/api/streamCompanies")]
		public IActionResult AddStreamingCompany([FromBody] StreamCompanyInfo companyInfo)
		{
			if (null == companyInfo || null == companyInfo.Name || null == companyInfo.webApiUrl || null == companyInfo.challengeUrl)
				return BadRequest();

			_movieDbContext.StreamCompanies.Add(companyInfo);
			//this is redundant but just to ensure notify handler is up to data, even if it is regenerated each time
			_notifyHandler.StreamCompanies.Add(companyInfo);

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = companyInfo.Name },
				companyInfo);
		}

// PUT api/<NotifyAPIController>/5
		[HttpPut("/api/streamCompanies/{name}")]
		public IActionResult UpdateStreamingCompany(string name, [FromBody] StreamCompanyInfo updateInfo)
		{
			_movieDbContext.StreamCompanies.Update(updateInfo);
			_movieDbContext.SaveChanges();

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = updateInfo.Name },
				updateInfo);
		}

		// DELETE api/<NotifyAPIController>/5
		[HttpDelete("/api/streamCompanies/{name}")]
		public IActionResult DeleteStreamingCompany(string name)
		{
			var delInfo = _movieDbContext.StreamCompanies
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApiUrl = s.webApiUrl
				}).Where(s => s.Name.Equals(name))
				.FirstOrDefault();

			if (delInfo == null)
				return BadRequest();

			_movieDbContext.Remove(delInfo);

			var deleteCompany = _notifyHandler.StreamCompanies.Find(s => s == delInfo);

			if (deleteCompany == null)
				return BadRequest();

			_notifyHandler.StreamCompanies.Remove(deleteCompany);

			_movieDbContext.SaveChanges();
			return Ok();
		}

 */