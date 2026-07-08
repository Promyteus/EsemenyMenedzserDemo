using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.BLL.Modul.Esemeny.DTOs;
using EsemenyMenedzser.BLL.Modul.Esemeny.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EsemenyMenedzser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly ICQRSExecutor _executor;

        public EventsController(ICQRSExecutor executor)
        {
            _executor = executor;
        }

        // 1. GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetEventListQuery();
            var events = await _executor.ExecuteQueryAsync(query);
            return Ok(events);
        }

        // 2. GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetEventByIdQuery(id);
            var esemeny = await _executor.ExecuteQueryAsync(query);

            if (esemeny == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            return Ok(esemeny);
        }

        // 3. POST: api/events
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
        {
            try
            {
                var command = new CreateEventCommand(dto);

                var id = await _executor.ExecuteCommandAsync(command);
                return Ok(new { id, message = "Event created successfully." });
            }
            catch(ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                return BadRequest(new { message = "Validation failed.", errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the event.", error = ex.Message });
            }
        }

        // 4. PUT: api/events
        [HttpPut] 
        public async Task<IActionResult> Update([FromBody] UpdateEventDto dto)
        {
            try
            {
                var command = new UpdateEventCommand(dto);

                var success = await _executor.ExecuteCommandAsync(command);

                if (!success)
                {
                    return NotFound(new { message = "Event not found." });
                }

                return Ok(new { message = "Event updated successfully." });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { message = "Validation failed.", errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the event.", error = ex.Message });
            }
        }

        // 5. DELETE: api/events/{id}
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteEventCommand(id);
                var success = await _executor.ExecuteCommandAsync(command);

                if (!success)
                {
                    return NotFound(new { message = "Event not found." });
                }

                return Ok(new { message = "Event deleted successfully." });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { message = "Validation failed.", errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while Deleting the event.", error = ex.Message });
            }
        }
    }
}
