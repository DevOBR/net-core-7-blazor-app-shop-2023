using AppShop.API.Data;
using AppShop.Share.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CategoriesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _dataContext.Categories.ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Category category)
        {
            try
            {
                await _dataContext.Categories.AddAsync(category).ConfigureAwait(false);
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The category {category.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Category category)
        {
            try
            {
                _dataContext.Categories.Update(category);
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The category {category.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (category is null)
                return NotFound();

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }

        
	}
}

