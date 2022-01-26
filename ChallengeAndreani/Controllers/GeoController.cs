using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeAndreani.Models;
using ChallengeAndreani.Interaface;

namespace ChallengeAndreani.Controllers
{
    [ApiController]
    public class GeoController : ControllerBase
    {
        private readonly IGeoService _GeoService;
        private readonly ILogger<GeoController> _logger;

        public GeoController(IGeoService geoService, ILogger<GeoController> logger)
        {
            _GeoService = geoService;
            _logger = logger;
        }

        [Produces("application/json")]
        [HttpPost("Geolocalizar")]
        public async Task<IActionResult> Geolocalizar(SolicitudGeolocalizacion solicitudGeolocalizacion)
        {
            try
            {
                var responseGeolocalizar = await _GeoService.Geolocalizar(solicitudGeolocalizacion);
                if (responseGeolocalizar != null)
                {
                    _GeoService.AuditarGeolocalizacion(solicitudGeolocalizacion, responseGeolocalizar);
                    return Accepted(new { Id = responseGeolocalizar.place_id });
                }
                else 
                    return NotFound("Datos para la Geolocalizacion invalidos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Geocodificar")]
        public async Task<IActionResult> Geocodificacion(long Id)
        {
            try
            {
                var geocodificacion = await _GeoService.Geocodificacion(Id);
                if (geocodificacion != null)
                    return Ok(geocodificacion);
                else
                    return NotFound("El Id no existe");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
