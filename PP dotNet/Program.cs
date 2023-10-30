using PP_dotNet.Controller;
using PP_dotNet.Data;
using PP_dotNet.Services;
using PP_dotNet.View;
using System.Globalization;

// A quantidade de elementos que serão exibidos em cada paginação.
const int QTD_PAGINACAO_INICIAL = 10;

var cabecalho = new Cabecalho("J'ONN J'ONNZ", "INICIO", "NONE");

IRepositorio repositorio = new RepositorioVirtual();
var paginador = new Paginador(repositorio.QtdUsuarios, QTD_PAGINACAO_INICIAL);
var usuarioController = new UsuarioController(repositorio, paginador);
var escritorDeConsole = new EscritorDeConsole(cabecalho);

var tui = new TUI(usuarioController, escritorDeConsole, CultureInfo.CurrentUICulture);
tui.Iniciar();

