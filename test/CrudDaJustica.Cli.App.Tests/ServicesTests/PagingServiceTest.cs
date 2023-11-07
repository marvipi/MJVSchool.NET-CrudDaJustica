using PP_dotNet.Services;

namespace PP_dotNet_Tests.ServicesTests;

// TODO Rewrite in English.

[TestFixture]
public class PagingServiceTest
{
    [TestCase(10, 10, 1)]
    [TestCase(40, 05, 8)]
    [TestCase(11, 03, 4)]
    [TestCase(01, 07, 1)]
    public void UltimaPagina_TamanhoRepositorioEQuantidadeSaoValoresValidos_CalculaAUltimaPaginaCorretamente
        (int tamanhoRepositorio, int quantidade, int esperado)
    {
        var paginador = new PagingService(tamanhoRepositorio, quantidade);

        var resultado = paginador.LastPage;

        Assert.That(resultado, Is.EqualTo(esperado));
    }

    [Test]
    public void Construtor_TamanhoRepositorioENegativo_LevantaUmArgumentOutOfRangeException()
    {
        var naoEstaSendoTestado = 10;

        Assert.Throws<ArgumentOutOfRangeException>(() => new PagingService(-1, naoEstaSendoTestado));
    }

    [Test]
    public void Construtor_QuantidadeMenorQueUm_LevantaUmArgumentOutOfRangeException()
    {
        var naoEstaSendoTestado = 10;

        Assert.Throws<ArgumentOutOfRangeException>(() => new PagingService(naoEstaSendoTestado, 0));
    }

    [TestCase(10, 5, 20, 4)]
    public void TamanhoRepositorio_Atribuicao_AtualizaOValorDaUltimaPagina
        (int tamanhoRepositorioInicial, int quantidadeValida, int novoTamanho, int esperado)
    {
        var paginador = new PagingService(tamanhoRepositorioInicial, quantidadeValida);

        paginador.RepositorySize = novoTamanho;

        Assert.That(paginador.LastPage, Is.EqualTo(esperado));
    }

    [TestCase(250, 150)]
    public void Atual_NumeroDaPaginaRetornadaEIgualAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new PagingService(tamanhoRepositorioValido, quantidadeValida);

        var paginaAtual = paginador.GetCurrentPage();

        Assert.That(paginaAtual.Number, Is.EqualTo(paginador.CurrentPage));
    }

    [TestCase(5, 98)]
    public void Atual_QuantidadeDaPaginaRetornadaEIgualAQuantidade(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new PagingService(tamanhoRepositorioValido, quantidadeValida);

        var paginaAtual = paginador.GetCurrentPage();

        Assert.That(paginaAtual.Rows, Is.EqualTo(paginador.RowsPerPage));
    }

    [TestCase(200, 20)]
    public void Avancar_PaginaAtualMenorQueAUltima_IncrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new PagingService(tamanhoRepositorioValido, quantidadeValida);
        var paginaAtualInicial = paginador.CurrentPage;

        paginador.NextPage();

        var esperado = paginaAtualInicial + 1;

        Assert.That(paginador.CurrentPage, Is.EqualTo(esperado));
    }

    [TestCase(1, 10)]
    public void Avancar_PaginaAtualEIgualAUltima_NaoIncrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new PagingService(tamanhoRepositorioValido, quantidadeValida);
        var paginaAtualInicial = paginador.CurrentPage;

        paginador.NextPage();

        var novaPaginaAtual = paginador.CurrentPage;
        Assert.That(paginaAtualInicial, Is.EqualTo(novaPaginaAtual));
    }

    [TestCase(10, 6)]
    public void Voltar_PaginaAtualEMaiorQueAPrimeira_DecrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new PagingService(tamanhoRepositorioValido, quantidadeValida);
        var paginaInicial = paginador.CurrentPage;

        paginador.NextPage();
        paginador.PreviousPage();

        var novaPaginaAtual = paginador.CurrentPage;

        Assert.That(novaPaginaAtual, Is.EqualTo(paginaInicial));
    }

    [TestCase(75, 4)]
    public void Voltar_PaginaAtualEIgualAPrimeira_NaoDecrementaAPaginaAtual(int tamanhoRepositorioValido, int quantidadeValida)
    {
        var paginador = new PagingService(tamanhoRepositorioValido, quantidadeValida);
        var numPrimeiraPagina = paginador.CurrentPage;

        paginador.PreviousPage();

        Assert.That(paginador.CurrentPage, Is.EqualTo(numPrimeiraPagina));
    }
}
