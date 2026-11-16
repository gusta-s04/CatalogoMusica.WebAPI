namespace CatalogoMusica.WebAPI.Utils;

public static class Criptografia
{
    public static string GerarHash(string senha) =>
        BCrypt.Net.BCrypt.HashPassword(senha);

    public static bool CompararHash(string senhaInformada, string senhaBanco)
    {
        if (string.IsNullOrWhiteSpace(senhaInformada) ||
            string.IsNullOrWhiteSpace(senhaBanco) ||
            !senhaBanco.StartsWith("$2", StringComparison.Ordinal))
        {
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }
}