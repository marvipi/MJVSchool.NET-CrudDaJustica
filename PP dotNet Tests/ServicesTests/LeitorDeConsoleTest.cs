using PP_dotNet.Services;
using System.Globalization;

namespace PP_dotNet_Tests.ServicesTests;

[TestFixture]
public class LeitorDeConsoleTest
{
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    public void LerNome_EntradaEmBranco_RetornaAnonimo(string entradaDoUsuario)
    {
        var entrada = new StringReader(entradaDoUsuario);
        var saida = new StringWriter();
        Console.SetIn(entrada);
        Console.SetOut(saida);
        var leitorDeConsole = new LeitorDeConsole();

        var retorno = leitorDeConsole.LerNome();
        var esperado = "Anônimo";

        Assert.That(retorno, Is.EqualTo(esperado));
    }

    [TestCase("M")]
    [TestCase("Roberto")]
    [TestCase("%$^6921Ma")]
    public void LerNome_EntradaNaoEVazia_RetornaAEntrada(string entradaDoUsuario)
    {
        var entrada = new StringReader(entradaDoUsuario);
        var saida = new StringWriter();
        Console.SetIn(entrada);
        Console.SetOut(saida);
        var leitorDeConsole = new LeitorDeConsole();

        var retorno = leitorDeConsole.LerNome();

        Assert.That(retorno, Is.EqualTo(entradaDoUsuario));
    }

    [TestCase(" Bill")]
    [TestCase("Mays ")]
    [TestCase(" Who? ")]
    public void LerNome_EntradaContemQualquerCaracterAlphanumerico_RemoveEspacosAntesEDepois(string entradaDeUsuario)
    {
        var entrada = new StringReader(entradaDeUsuario);
        var saida = new StringWriter();
        Console.SetIn(entrada);
        Console.SetOut(saida);
        var leitorDeConsole = new LeitorDeConsole();

        var retorno = leitorDeConsole.LerNome();
        var esperado = entradaDeUsuario.Trim();

        Assert.That(retorno, Is.EqualTo(esperado));
    }

    [TestCase("15/12/2023", "pt-br")]
    [TestCase("8/30/2023", "en-us")]
    [TestCase("31.10.2023", "de-de")]
    [TestCase("2008/12/31", "ja-ja")]
    public void LerDataDeNascimento_LeDatasDeQualquerCultura_RetornaUmDateOnlyDeCulturaInvariante(string entradaDoUsuario, string culturaAtual)
    {
        var entrada = new StringReader(entradaDoUsuario);
        var saida = new StringWriter();
        Console.SetIn(entrada);
        Console.SetOut(saida);
        var leitorDeConsole = new LeitorDeConsole();
        var cultureInfo = new CultureInfo(culturaAtual);
        var formatoLocal = cultureInfo.DateTimeFormat.ShortDatePattern;

        var resultado = leitorDeConsole.LerDataDeNascimento(cultureInfo);
        var esperado = DateOnly.ParseExact(entradaDoUsuario, formatoLocal, CultureInfo.InvariantCulture);

        Assert.That(resultado, Is.EqualTo(esperado));
    }
}