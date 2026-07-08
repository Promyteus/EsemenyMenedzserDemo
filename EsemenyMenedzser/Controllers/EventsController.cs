using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.BLL.Modul.Esemeny.DTOs;
using EsemenyMenedzser.BLL.Modul.Esemeny.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EsemenyMenedzser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly ICQRSExecutor _executor;
        private readonly ILogger<EventsController> _logger;

        public EventsController(ICQRSExecutor executor, ILogger<EventsController> logger)
        {
            _executor = executor;
            _logger = logger;
        }

        // 1. GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll request started.");
            var query = new GetEventListQuery();
            var events = await _executor.ExecuteQueryAsync(query);
            _logger.LogInformation("GetAll request completed successfully.");
            return Ok(events);
        }

        // 2. GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById request started. Id: {Id}", id);
            var query = new GetEventByIdQuery(id);
            var esemeny = await _executor.ExecuteQueryAsync(query);

            if (esemeny == null)
            {
                _logger.LogWarning("Event not found. Id: {Id}", id);
                throw new KeyNotFoundException($"Event with id {id} not found.");
            }

            _logger.LogInformation("GetById request completed successfully. Id: {Id}", id);
            return Ok(esemeny);
        }

        // 3. POST: api/events
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
        {
            _logger.LogInformation("Create request started.");
            var command = new CreateEventCommand(dto);
            var id = await _executor.ExecuteCommandAsync(command);
            _logger.LogInformation("Create request completed successfully. Id: {Id}", id);
            return Ok(new { id, message = "Event created successfully." });
        }

        // 4. PUT: api/events
        [HttpPut] 
        public async Task<IActionResult> Update([FromBody] UpdateEventDto dto)
        {
            _logger.LogInformation("Update request started. Id: {Id}", dto.Id);
            var command = new UpdateEventCommand(dto);
            var success = await _executor.ExecuteCommandAsync(command);

            if (!success)
            {
                _logger.LogWarning("Event not found for update. Id: {Id}", dto.Id);
                throw new KeyNotFoundException($"Event with id {dto.Id} not found.");
            }

            _logger.LogInformation("Update request completed successfully. Id: {Id}", dto.Id);
            return Ok(new { message = "Event updated successfully." });
        }

        // 5. DELETE: api/events/{id}
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete request started. Id: {Id}", id);
            var command = new DeleteEventCommand(id);
            var success = await _executor.ExecuteCommandAsync(command);

            if (!success)
            {
                _logger.LogWarning("Event not found for delete. Id: {Id}", id);
                throw new KeyNotFoundException($"Event with id {id} not found.");
            }

            _logger.LogInformation("Delete request completed successfully. Id: {Id}", id);
            return Ok(new { message = "Event deleted successfully." });
        }
    }
}
