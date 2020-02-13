using Lego_api_data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Lego_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThemesController : ControllerBase
    {
        private readonly LegoDbContext _dbContext;

        public ThemesController(LegoDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet("{themeid}")]        
        public IActionResult GetTheme([FromRoute]int themeId)
        {
            
            var values = _dbContext.Themes.Where(x => x.ThemeId <= themeId).ToList();
            return Ok(values);
        }
    }
}
