using PP_dotNet.Data;
using PP_dotNet.Services;

namespace PP_dotNet_Tests.ServicesTests;

[TestFixture]
public class PaginadorTest
{
    [TestCase(10, 10, 1)]
    [TestCase(40, 05, 8)]
    [TestCase(11, 03, 4)]
    [TestCase(01, 07, 1)]
    public void UltimaPagina_TamanhoRepositorioEQuantidadeSaoValoresValidos_CalculaAUltimaPaginaCorretamente
        (int tamanhoRepositorio, int quantidade, int esperado)
    {
        var paginador = new Paginador(tamanhoRepositorio, quantidade);

        var resultado = paginador.UltimaPagina;

        Assert.That(resultado, Is.EqualTo(esperado));
    }

    [Test]
    public void Construtor_TamanhoRepositorioENegativo_LevantaUmArgumentOutOfRangeException()
    {
        var naoEstaSendoTestado = 10;

        Assert.Throws<ArgumentOutOfRangeException>(() => new Paginador(-1, naoEstaSendoTestado));
    }

    [Test]
    public void Construtor_QuantidadeMenorQueUm_LevantaUmArgumentOutOfRangeException()
    {
        var naoEstaSendoTestado = 10;

        Assert.Throws<ArgumentOutOfRangeException>(() => new Paginador(naoEstaSendoTestado, 0));
    }

    [TestCase(10, 5, 20, 4)]
    public void TamanhoRepositorio_Atribuicao_AtualizaOValorDaUltimaPagina
        (int tamanhoRepositorioInicial, int quantidadeValida, int novoTamanho, int esperado)
    {
        var paginador = new Paginador(tamanhoRepositorioInicial, quantidadeValida);

        paginador.TamanhoRepositorio = novoTamanho;

        Assert.That(paginador.UltimaPagina, Is.EqualTo(esperado));
    }

    [TestCase(250, 150)]
    public void Atual_NumeroDaPaginaRetornadaEIgualAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new Paginador(tamanhoRepositorioValido, quantidadeValida);

        var paginaAtual = paginador.Atual();

        Assert.That(paginaAtual.Numero, Is.EqualTo(paginador.PaginaAtual));
    }

    [TestCase(5, 98)]
    public void Atual_QuantidadeDaPaginaRetornadaEIgualAQuantidade(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new Paginador(tamanhoRepositorioValido, quantidadeValida);

        var paginaAtual = paginador.Atual();

        Assert.That(paginaAtual.Quantidade, Is.EqualTo(paginador.Quantidade));
    }

    [TestCase(200, 20)]
    public void Avancar_PaginaAtualMenorQueAUltima_IncrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new Paginador(tamanhoRepositorioValido, quantidadeValida);
        var paginaAtualInicial = paginador.PaginaAtual;

        paginador.Avancar();

        var esperado = paginaAtualInicial + 1;

        Assert.That(paginador.PaginaAtual, Is.EqualTo(esperado));
    }

    [TestCase(1, 10)]
    public void Avancar_PaginaAtualEIgualAUltima_NaoIncrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new Paginador(tamanhoRepositorioValido, quantidadeValida);
        var paginaAtualInicial = paginador.PaginaAtual;

        paginador.Avancar();

        var novaPaginaAtual = paginador.PaginaAtual;
        Assert.That(paginaAtualInicial, Is.EqualTo(novaPaginaAtual));
    }

    [TestCase(10, 6)]
    public void Voltar_PaginaAtualEMaiorQueAPrimeira_DecrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new Paginador(tamanhoRepositorioValido, quantidadeValida);
        var paginaInicial = paginador.PaginaAtual;

        paginador.Avancar();
        paginador.Voltar();

        var novaPaginaAtual = paginador.PaginaAtual;

        Assert.That(novaPaginaAtual, Is.EqualTo(paginaInicial));
    }

    [TestCase(75, 4)]
    public void Voltar_PaginaAtualEIgualAPrimeira_NaoDecrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new Paginador(tamanhoRepositorioValido, quantidadeValida);
        var numPrimeiraPagina = paginador.PaginaAtual;

        paginador.Voltar();

        Assert.That(paginador.PaginaAtual, Is.EqualTo(numPrimeiraPagina));
    }
}
