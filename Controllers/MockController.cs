using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HtecDakarRallyWebApi.DataTransferObjects;
using AutoMapper;
using HtecDakarRallyWebApi.Extensions;
using HtecDakarRallyWebApi.Services;

namespace HtecDakarRallyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly RaceService _service;

        public MockController(RaceService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Mock(){
            try
            {
                return Ok(_service.Mock().Result);
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }

        [HttpPut("Multiplier")]
        public ActionResult SetMultiplier(uint multiplier)
        {
            try
            {
                _service.SetMultiplier(multiplier);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.DrMsgObject());
            }
        }
    }
}
