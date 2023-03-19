using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectCMS.Data;
using ProjectCMS.Models;
using ProjectCMS.ViewModels;

namespace ProjectCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public EventController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetEvent()
        {
            //List<Event> events = await _dbContext._events.ToListAsync();
            //List<Category> cates = await _dbContext._categories.ToListAsync();
            var listEvent = await _dbContext._events
                .ToListAsync();
            return Ok(listEvent);
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateEvent(EventViewModel evt)
        {
            if (ModelState.IsValid)
            {
                Event newEvt = new Event();
                newEvt.Name = evt.Name;
                newEvt.Content = evt.Content;
                newEvt.First_Closure= evt.First_Closure;
                newEvt.Last_Closure = evt.First_Closure.AddDays(7);
                //newEvt.CateId = evt.CateId;
                _dbContext._events.Add(newEvt);
                _dbContext.SaveChanges();
                return Ok(await _dbContext._events.ToListAsync());
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            var evt = await _dbContext._events.FindAsync(id);
            if (evt != null)
            {
                _dbContext._events.Remove(evt);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            return Ok(await _dbContext._events.FindAsync(id));

        }
        [HttpPut, Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateEvent(EventViewModel evtUpdate, [FromRoute] int id)
        {
            var evt = await _dbContext._events.FindAsync(id);
            if(evt != null)
            {
                if(ModelState.IsValid)
                {
                    evt.Name = evtUpdate.Name;
                    evt.Content = evtUpdate.Content;
                    evt.First_Closure = evtUpdate.First_Closure;
                    evt.Last_Closure = evtUpdate.First_Closure.AddDays(7);
                    //evt.CateId = evtUpdate.CateId;
                    _dbContext.SaveChanges();
                    return Ok(await _dbContext._events.ToListAsync());
                }
            }
                return BadRequest();
        }

    }
}
