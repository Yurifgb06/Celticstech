using Celticstech.DTOs;
using System.Globalization;
using System.Text.Json;

namespace Celticstech.Services
{
    public class OpenMeteoService
    {
        private readonly HttpClient _httpClient;

        public OpenMeteoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenMeteoResponseDTO?> ObterDadosClimaticos(
            double latitude,
            double longitude,
            CancellationToken cancellationToken = default)
        {
            var latitudeFormatada = latitude.ToString(CultureInfo.InvariantCulture);
            var longitudeFormatada = longitude.ToString(CultureInfo.InvariantCulture);
            var url =
                $"https://api.open-meteo.com/v1/forecast" +
                $"?latitude={latitudeFormatada}" +
                $"&longitude={longitudeFormatada}" +
                $"&current=temperature_2m,relative_humidity_2m,wind_speed_10m,rain";

            using var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var document = JsonDocument.Parse(json);
            var current = document.RootElement.GetProperty("current");

            var temperatura = current.GetProperty("temperature_2m").GetDouble();
            var umidade = current.GetProperty("relative_humidity_2m").GetDouble();
            var vento = current.GetProperty("wind_speed_10m").GetDouble();
            var chuva = current.GetProperty("rain").GetDouble();

            var scoreRisco = CalcularScoreRisco(temperatura, umidade, vento, chuva);
            var nivelRisco = ClassificarNivelRisco(scoreRisco);

            return new OpenMeteoResponseDTO
            {
                Temperatura = temperatura,
                Umidade = umidade,
                VelocidadeVento = vento,
                Chuva = chuva,
                ScoreRisco = scoreRisco,
                NivelRisco = nivelRisco
            };
        }

        public static int CalcularScoreRisco(
            double temperatura,
            double umidade,
            double velocidadeVento,
            double chuva)
        {
            var score = 0;

            score += temperatura switch
            {
                > 40 => 15,
                > 35 => 12,
                > 30 => 8,
                > 28 => 5,
                _ => 0
            };

            score += chuva switch
            {
                < 0.5 => 70,
                < 2 => 60,
                < 5 => 20,
                _ => 0
            };

            score += umidade switch
            {
                < 25 => 10,
                < 40 => 8,
                < 55 => 4,
                _ => 0
            };

            score += velocidadeVento switch
            {
                > 40 => 5,
                > 25 => 4,
                > 15 => 2,
                _ => 0
            };

            return Math.Clamp(score, 0, 100);
        }

        public static string ClassificarNivelRisco(int scoreRisco)
        {
            return scoreRisco switch
            {
                >= 70 => "ALTO",
                >= 40 => "MODERADO",
                _ => "BAIXO"
            };
        }
    }
}
