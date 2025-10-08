# TechChallenge - API 

[Documentação da API](https://alealencarr.github.io/TechChallenge-app-api/) 

### Descrição
Este repositório contém o código-fonte da aplicação principal (API) da lanchonete. Ele é responsável por toda a lógica de negócio, como gestão de produtos, ingredientes, pedidos, clientes, categorias, checkout e comunicação com o banco de dados.

A aplicação é projetada para ser executada como um contêiner dentro do cluster Azure Kubernetes Service (AKS), garantindo escalabilidade e resiliência.

### Tecnologias Utilizadas
.NET 9: Framework principal da aplicação.

Docker: Para containerização da aplicação.

Kubernetes (Manifestos YAML): Para orquestração dos contêineres no AKS. Os manifestos definem os Deployments, Services, Ingress e HPA (Horizontal Pod Autoscaler).

### Responsabilidades
Implementar todos os endpoints da lógica de negócio (CRUD de produtos, fluxo de pedidos, etc.).

Conectar-se aos serviços de dados (Azure SQL e Azure Blob Storage) para persistir e ler informações.

Ser empacotado num Dockerfile para criar uma imagem executável.

Definir os manifestos Kubernetes que descrevem como a aplicação deve ser executada e exposta dentro do cluster.

### Dependências
O pipeline de CI/CD deste repositório depende da existência da infraestrutura criada pelo TechChallenge-infra-compute, especificamente o Azure Kubernetes Service (AKS) e o Azure Container Registry (ACR).

### Processo de CI/CD
O pipeline de CI/CD configurado neste repositório (.github/workflows/deploy.yml) é acionado a cada merge na branch main e executa os seguintes passos:

Build da Imagem: Constrói a imagem Docker da aplicação.

Push para o ACR: Envia a nova imagem para o Azure Container Registry.

Deploy no AKS: Aplica os manifestos Kubernetes (kubectl apply), instruindo o AKS a descarregar a nova imagem e a atualizar a aplicação no ar.

### Licença

Este projeto está licenciado sob os termos da licença MIT.  
Consulte o arquivo [LICENSE](./LICENSE) para mais detalhes.
