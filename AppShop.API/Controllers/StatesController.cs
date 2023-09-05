using AppShop.API.Data;
using AppShop.Share.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Controllers
{
    [ApiController]
    [Route("/api/states")]
    public class StatesController : ControllerBase
    {
        private readonly DataContext _context;

        public StatesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFullAsync()
        {
            return Ok(await _context.States
                .Include(x => x.Cities!).ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {

            var state = await _context.States.Include(x => x.Cities)
                .FirstAsync(x => x.Id == id).ConfigureAwait(false);

            if (state is null)
                return NotFound();


            return Ok(state);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] State state)
        {
            try
            {
                await _context.States.AddAsync(state).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The {nameof(State)} {state.Name} already exist.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] State state)
        {
            try
            {
                var stateResult = await _context.States.FirstAsync(x => x.Id == state.Id).ConfigureAwait(false);

                if (state is null)
                    return NotFound();

                stateResult.Name = state.Name;

                _context.States.Update(stateResult);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest($"The {nameof(State)} {state.Name} already exist.");
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

            var state = await _context.States.FirstAsync(x => x.Id == id).ConfigureAwait(false);

            if (state is null)
                return NotFound();

            _context.States.Remove(state);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
    }
}

