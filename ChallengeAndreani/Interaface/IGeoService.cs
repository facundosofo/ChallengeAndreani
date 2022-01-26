using ChallengeAndreani.Models;
using System.Threading.Tasks;

namespace ChallengeAndreani.Interaface
{
    public interface IGeoService
    {
        Task<Geocodificacion> Geocodificacion(long Id);
        Task<ResponseGeolocalizar> Geolocalizar(SolicitudGeolocalizacion solicitudGeolocalizacion);
        void AuditarGeolocalizacion(SolicitudGeolocalizacion solicitudGeolocalizacion, ResponseGeolocalizar responseGeolocalizar);
    }
}
