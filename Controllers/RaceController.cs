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
    public class RaceController : ControllerBase
    {
        private readonly DrService _service;

        public RaceController(DrService service)
        {
            _service = service;
        }

        //1. Create race
        //(parameters: year)
        [HttpPost]
        public async Task<ActionResult<CreateRaceResponseDTO>> Create(CreateRaceRequestDTO race)
        {
            try
            {
                return CreatedAtAction(nameof(Create), await _service.CreateRace(race));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //2. Add vehicle to the race
        //available only prior to the race start
        //(parameters: vehicle)
        [HttpPost("{raceId}/AddVehicle")]
        public async Task<ActionResult<VehicleResponseDTO>> AddVehicle(int raceId, VehicleRequestDTO vehicle)
        {
            try
            {
                return CreatedAtAction(nameof(AddVehicle), await _service.AddVehicle(raceId, vehicle));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //5. Start the race
        //only one race can run at the time
        //(parameters: race identifier)
        [HttpPost("{id}/Start")]
        public IActionResult Start(int id)
        {
            try
            {
                _service.StartRace(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //6. Get leaderboard including all vehicles
        [HttpGet("Leaderboard")]
        public async Task<ActionResult<LeaderboardDTO>> GetLeaderboard()
        {
            try
            {
                return Ok(await _service.GetLeaderboard());
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //6. Get leaderboard including all vehicles
        [HttpGet("{id}/Leaderboard")]
        public async Task<ActionResult<LeaderboardDTO>> GetLeaderboard(int id)
        {
            try
            {
                return Ok(await _service.GetLeaderboard(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //7. Get leaderboard for specific vehicle type: cars, trucks, motorcycles
        //(parameters: type)
        [HttpGet("Leaderboard/{type}")]
        public async Task<ActionResult<LbByTypeDTO>> GetLeaderboard(string type)
        {
            try
            {
                return Ok(await _service.GetLeaderboard(null, type));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //7. Get leaderboard for specific vehicle type: cars, trucks, motorcycles
        //(parameters: type)
        [HttpGet("{id}/Leaderboard/{type}")]
        public async Task<ActionResult<LbByTypeDTO>> GetLeaderboard(string type, int id)
        {
            try
            {
                return Ok(await _service.GetLeaderboard(id, type));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        //10. Get race status that includes:
        //race status (pending, running, finished),
        //number of vehicles grouped by vehicle status,
        //number of vehicles grouped by vehicle type
        //(parameters: race identifier)
        [HttpGet("{id}")]
        public async Task<ActionResult<RaceStatDTO>> GetStatus(int id){
            try
            {
                return Ok(await _service.GetStatus(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateRaceResponseDTO>>> GetAllRaces()
        {
            return Ok(await _service.GetAllRaces());
        }
    }
}
