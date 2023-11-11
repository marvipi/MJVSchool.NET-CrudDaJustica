# MJV School .NET - CRUD da Justiça

Desenvolvi este projeto para colocar em prática tudo que aprendemos durante a primeira MJV School de .NET, que foi ao ar entre 23/10/2023 e 23/11/2023.

### Introdução
O CRUD da Justiça é uma aplicação simples que lida com informações sobre super-heróis. Programas desse tipo são maçantes por natureza, e por isso, escolhi um tema leve para tentar torná-lo menos entediante.

### Design da aplicação de console
Design patterns não foram abordados durante o bootcamp, porém apliquei algumas delas para assegurar a qualidade da implementação.
- MVP e Mediator: a comunicação entre a interface de linha de comando e o back-end é mediada pelos controladores.
- Repository ou Façade: os controladores não sabem com quais repositórios eles se comunicam.
 
### Conceitos abordados
Durante o curso foram abordados diversas características e funcionalidades do C# e .NET. A seguir estão aquelas que melhor se encaixaram no escopo deste projeto.

- ✔ String: a interface de linha de comando é desenhada usando strings. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Views/Frame.cs)
- ✔ Array: o projeto tem um repositório virtual que armazena informações em arrays. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.App/Data/VirtualRepository.cs)
- ✔ Serialização e manipulação de arquivos: o projeto consegue salvar dados em arquivos JSON. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.App/Data/JsonRepository.cs)
- ✔ Listas: atalhos de teclado são armazenados dentro de uma lista. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Views/View.cs)
- ✔ Queues e tipagem genérica: usados para gerar e preencher formulários de linha de comando. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Forms/Form.cs)
- ⏳ Records: Adiado até a implementação de Web APIs.
- ✔ Structs: as páginas de dados são encapsuladas em um struct. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.App/Services/DataPage.cs)
- ✔ Classes: quase todos os tipos do projeto são classes. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.App/Controller/HeroController.cs)
- ✔ Interfaces: os repositórios de dados implementam interfaces. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.App/Data/IHeroRepository.cs)
- ✔ Classes abstratas e herança: a interface de linha de comando utiliza ambos para desenhar decorações de janela. [Link](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/src/CrudDaJustica.Cli.Lib/Views/Frame.cs)

### Diagrama UML
![design.png](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/design.png)

### Imagens
![Listagens na linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/cli.png)
![Formulários na linha de comando](https://github.com/marvipi/MJVSchool.NET-CrudDaJustica/blob/stable/res/cli-form.png)
