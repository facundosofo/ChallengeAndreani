using ChallengeAndreani.Interaface;
using ChallengeAndreani.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChallengeAndreani.Services
{
    public class GeoService : IGeoService
    {
        private readonly Context _context;

        public GeoService(Context context)
        {
            this._context = context;
        }



        public async Task<Geocodificacion> Geocodificacion(long id)
        { 
            try
            {
                Geocodificacion geocodificacion = new Geocodificacion();
                var geolocalizacion =  _context.Geolocalizacion.FirstOrDefault(x => x.IdGeolocalizacion == id);
                if (geolocalizacion != null)
                {
                    var resultadoGeolocalizacion = JsonSerializer.Deserialize<ResponseGeolocalizar>(geolocalizacion.ResultadoGeolocalizacion);
                    geocodificacion.Id = geolocalizacion.IdGeolocalizacion;
                    geocodificacion.Longitud = resultadoGeolocalizacion.lon;
                    geocodificacion.Latitud = resultadoGeolocalizacion.lat;
                    return geocodificacion;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la Geocodificacion {ex.Message},{ex.StackTrace}");
            }

        }

        public async Task<ResponseGeolocalizar> Geolocalizar(SolicitudGeolocalizacion solicitudGeolocalizacion)
        {
            try
            {
                string queryBusqueda = ConvertirAStringSeparadoPor(solicitudGeolocalizacion, " ");
                var url = "https://nominatim.openstreetmap.org/search.php";
                var param = new Dictionary<string, string>() { { "q", queryBusqueda }, { "polygon_geojson","1" },{ "format", "jsonv2" } };
                var newUrl = new Uri(QueryHelpers.AddQueryString(url, param));
                var solicitud = WebRequest.Create(newUrl);
                solicitud.Method = "GET";
                solicitud.Headers.Add("User-Agent", ".NET Framework Test Client");        

                var respuesta = await solicitud.GetResponseAsync();
                var streamRecibido = respuesta.GetResponseStream();
                var lector = new StreamReader(streamRecibido);
                var mensajeRecibido = lector.ReadToEnd();
                var jsonRecibido = JsonSerializer.Deserialize<IEnumerable<ResponseGeolocalizar>>(mensajeRecibido);
                return jsonRecibido.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la Geolocalizacion {ex.Message},{ex.StackTrace}");
            }
        }

        public void AuditarGeolocalizacion(SolicitudGeolocalizacion solicitudGeolocalizacion, ResponseGeolocalizar responseGeolocalizar)
        {
            Geolocalizacion geolocalizacion = new Geolocalizacion();
            geolocalizacion.SolicitudGeolocalizacion = JsonSerializer.Serialize(solicitudGeolocalizacion);
            geolocalizacion.ResultadoGeolocalizacion = JsonSerializer.Serialize(responseGeolocalizar);
            geolocalizacion.IdGeolocalizacion = responseGeolocalizar.place_id;
            _context.Geolocalizacion.Add(geolocalizacion);
            _context.SaveChanges();
        }
        private string ConvertirAStringSeparadoPor(SolicitudGeolocalizacion solicitudGeolocalizacion, string separador)
        {
            return solicitudGeolocalizacion.calle + separador + solicitudGeolocalizacion.numero + separador + 
                   solicitudGeolocalizacion.ciudad + separador + solicitudGeolocalizacion.codigo_postal + separador +
                   solicitudGeolocalizacion.provincia + separador + solicitudGeolocalizacion.pais;
        }

    }
}

