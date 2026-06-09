namespace Celticstech.Services
{
    public class CoordenadasService
    {
        public (double Latitude, double Longitude) ObterCoordenadasPorUf(string uf)
        {
            uf = uf.Trim().ToUpper();

            return uf switch
            {
                "BA" => (-12.9714, -38.5014),
                "PE" => (-8.0476, -34.8770),
                "CE" => (-3.7319, -38.5267),
                "MA" => (-2.5307, -44.3068),
                "PI" => (-5.0892, -42.8019),
                "RN" => (-5.7945, -35.2110),
                "PB" => (-7.1195, -34.8450),
                "AL" => (-9.6498, -35.7089),
                "SE" => (-10.9472, -37.0731),

                _ => throw new ArgumentException("UF inválida. Informe uma UF do Nordeste: BA, PE, CE, MA, PI, RN, PB, AL ou SE.")
            };
        }
    }
}