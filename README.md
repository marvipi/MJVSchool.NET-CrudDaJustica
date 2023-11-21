# MJV School .NET - CRUD da Justiça
Desenvolvi este projeto para colocar em prática tudo que aprendemos durante a primeira MJV School de .NET, que foi ao ar entre 23/10/2023 e 23/11/2023. 

O CRUD da Justiça é uma aplicação simples que lida com informações sobre super-heróis. Programas desse tipo são maçantes por natureza, e por isso, escolhi um tema leve para tentar torná-lo menos entediante.
Ele possui duas interfaces de usuário: uma web implementada com o ASP.NET MVC e uma de console. Você pode vê-las na seção de imagens deste readme.

### Sumário
- [Introdução](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica#introdu%C3%A7%C3%A3o)
- [Design patterns e padrões de projeto](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica#design-patterns-e-padr%C3%B5es-de-projeto)
- [Conceitos abordados](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica#conceitos-abordados)
- [Diagramas UML](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica#diagramas-uml)
- [Imagens](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica#imagens)

### Design patterns e padrões de projeto
Estes conceitos não foram abordados durante o bootcamp, porém apliquei alguns deles para assegurar a qualidade da implementação.
- MVC: o website é estruturado neste padrão.
- MVP e Mediator: a comunicação entre a interface de linha de comando e o backend é mediada pelos controladores.
- Repository ou Façade: os controladores não sabem com quais repositórios eles se comunicam.
- Decorator: a interface de linha de comando é implementada neste padrão.

### Conceitos abordados
Durante o curso foram abordados diversas características e funcionalidades do C# e .NET. A seguir estão aquelas que melhor se encaixaram no escopo deste projeto.
#### ASP.NET
- [x] ASP.NET MVC: a interface web do projeto. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/tree/stable/src/CrudDaJustica.Website)
- [x] ADO.NET: o projeto consegue armazenar dados por meio do SQL Server. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Data.Lib/Repository/SqlServerRepository.cs)
- [x] Leitura do arquivo appsettings.json: a string de conexão do banco de dados é lida deste arquivo. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Website/Program.cs)
- [x] Injeção de dependências: tanto o website quanto o aplicativo de console fazem injeção de dependências. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Website/Program.cs)

#### C#
- [x] String e StringBuilder: a interface de linha de comando é desenhada usando strings. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Decoration/Frame.cs)
- [x] Array: o projeto tem um repositório virtual que armazena informações em arrays. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Data.Lib/Repository/VirtualRepository.cs)
- [x] Serialização e manipulação de arquivos: o projeto consegue salvar dados em arquivos JSON. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Data.Lib/Repository/JsonRepository.cs)
- [x] Listas: atalhos de teclado são armazenados dentro de uma lista. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Window/Listing.cs)
- [x] Queues e tipagem genérica: usados para gerar e preencher formulários de linha de comando. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Window/Form.cs)
- [x] Records: os modelos de exibição do website são records. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Website/Models/HeroViewModel.cs)
- [x] Structs: as páginas de dados são encapsuladas em um struct. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Data.Lib/Service/DataPage.cs)
- [x] Classes: quase todos os tipos do projeto são classes. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.App/Controller/HeroController.cs)
- [x] Interfaces: os repositórios de dados implementam interfaces. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Data.Lib/Repository/IHeroRepository.cs)
- [x] Classes abstratas e herança: ambos são usados para desenhar decorações de janela na linha de comando. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Window/Window.cs)

### Diagramas UML
#### Website
![Design do website](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/CrudDaJustica.Website.png)
#### Camada de dados
![Design da camada de dados](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/CrudDaJustica.Data.Lib.png)
#### Aplicativo de linha de comando
![Design do aplicativo de linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/CrudDaJustica.Cli.App.png)
#### Interface de linha de comando
![Design da interface de linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/CrudDaJustica.Cli.Lib.png)

### Imagens
#### Website
![Listagem no website](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/web-heroes.png)
![Prompt de confirmação](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/web-confirmation-prompt.png)
![Validação nos formulários de atualização de hérois](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/web-updatehero-validation.png)
![Validação nos formulários de criação de hérois](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/web-createhero-validation.png)

#### Linha de comando
![Listagens na linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/cli.png)
![Formulários na linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/cli-form.png)
![Validação de formulários na linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/cli-form-validation.png)
