using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HtecDakarRallyWebApi.DataTransferObjects;
using AutoMapper;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly DrService _service;

        public VehicleController(DrService service)
        {
            _service = service;
        }

        //3. Update vehicle info
        //available only prior to the race start
        //(parameters: vehicle)
        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(int id, VehicleRequestDTO vehicle)
        {
            try
            {
                await _service.UpdateVehicle(id, vehicle);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //4. Remove vehicle from the race
        //available only prior to the race start
        //(parameters: vehicle identifier)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.RemoveVehicle(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //8. Get vehicle statistics:
        //distance,
        //malfunction statistics,
        //status,
        //finish time
        //(parameters: vehicle identifier)
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleStatDTO>> GetStatistics(int id)
        {
            try
            {
                return Ok(await _service.GetStatistics(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //9. Find vehicle(s)
        //(parameters: team
        //AND/OR model
        //AND/OR manufacturing date
        //AND/OR status
        //AND/OR distance,
        //sort order)
        [HttpGet("find")]
        public async Task<ActionResult<SearchResultDTO>> Find([FromQuery] SearchParmsDTO search)
        {
            try
            {
                return Ok(await _service.FindVehicles(search));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }
    }
}
