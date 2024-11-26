# SubGS CarAPI - Documentação Completa

## 1. Introdução

### 1.1 Objetivo do Projeto
O **SubGS CarAPI** é uma API RESTful desenvolvida em .NET 8 para gerenciar o cadastro de carros e prever seu estado com base na quilometragem. Este projeto é voltado para um sistema de gerenciamento de carros em uma aplicação de marketplace ou qualquer solução que necessite monitorar e cadastrar veículos. Ele também usa **Machine Learning (ML.NET)** para prever a condição dos carros com base em seu uso.

### 1.2 Tecnologias Utilizadas
- **.NET 8**: Framework usado para construir a API.
- **MongoDB**: Banco de dados NoSQL para armazenamento dos carros.
- **ML.NET**: Ferramenta usada para prever o estado do carro.

## 2. Arquitetura

### 2.1 Estrutura do Projeto
O projeto segue uma abordagem modular, com camadas bem definidas para garantir manutenção e escalabilidade:
- **Models**: Contêm as classes que representam os dados, como `Car` e `CarData`.
- **DTOs**: Definem as classes usadas para transferir dados entre cliente e servidor, garantindo um contrato claro.
- **Services**: Contêm a lógica de negócio para manipulação dos dados do carro e previsão com Machine Learning.
- **Controllers**: Expõem os endpoints da API.

### 2.2 Design Patterns Utilizados
- **Repository Pattern**: Implementado com MongoDB, centralizando o acesso aos dados.
- **Dependency Injection**: Utilizado para garantir flexibilidade e facilitar os testes.
- **Data Transfer Object (DTO)**: Para padronizar as entradas e saídas dos endpoints.

## 3. Models

### 3.1 Car
A classe `Car` representa um carro cadastrado no sistema. Contém os seguintes campos:
- **Id** (`string`): Identificador único do carro. Geração automática usando MongoDB.
- **Model** (`string`): Modelo do carro. Campo obrigatório.
- **Description** (`string`): Descrição detalhada do carro. Campo obrigatório.
- **Brand** (`string`): Marca do carro. Campo obrigatório.
- **OwnerEmail** (`string`): Email do proprietário. Validação aplicada para garantir formato correto.
- **Mileage** (`int`): Quilometragem do carro. Deve ser um valor positivo.
- **Status** (`string`): Status atual do carro, como "Disponível" ou "Indisponível".
- **RegistrationDate** (`DateTime`): Data de registro no sistema.

### 3.2 CarData e CarPrediction
Essas classes são usadas para o treinamento do modelo e previsão. **CarData** armazena os dados históricos usados para treinar o modelo, enquanto **CarPrediction** é usado para armazenar o resultado da previsão feita pelo ML.NET.

## 4. Endpoints da API

### 4.1 Cadastro de Carros
- **POST /api/cars**
    - **Descrição**: Cria um novo carro no sistema.
    - **Corpo da Requisição**: `CarDTO`
    - **Resposta**: Retorna o carro criado com código de status `201 Created`.
    - **Exemplo**:
      ```json
      {
        "model": "Civic",
        "description": "Carro sedã com motor 2.0",
        "brand": "Honda",
        "ownerEmail": "email@exemplo.com",
        "mileage": 15000,
        "status": "Disponível"
      }
      ```

### 4.2 Consulta de Carros
- **GET /api/cars**
    - **Descrição**: Retorna uma lista de todos os carros cadastrados.
    - **Resposta**: Uma lista contendo os carros com código de status `200 OK`.

- **GET /api/cars/{id}**
    - **Descrição**: Retorna um carro específico pelo seu identificador.
    - **Parâmetros**: `id` - Identificador único do carro.
    - **Resposta**: O carro correspondente ou `404 Not Found` se não existir.

### 4.3 Atualização de Carro
- **PUT /api/cars/{id}**
    - **Descrição**: Atualiza um carro existente com base nos dados fornecidos.
    - **Corpo da Requisição**: `UpdateCarDTO`
    - **Resposta**: Retorna o carro atualizado ou `404 Not Found` se o carro não for encontrado.

### 4.4 Exclusão de Carro
- **DELETE /api/cars/{id}**
    - **Descrição**: Deleta um carro do sistema.
    - **Parâmetros**: `id` - Identificador único do carro.
    - **Resposta**: `200 OK` se a exclusão for bem-sucedida, ou `404 Not Found` caso contrário.

### 4.5 Previsão do Estado do Carro
- **POST /api/cars/predict**
    - **Descrição**: Prever se o carro está em bom estado com base na quilometragem.
    - **Corpo da Requisição**: Um valor de `mileage` representando a quilometragem do carro.
    - **Resposta**: `true` se o carro estiver em bom estado, `false` caso contrário.
    - **Exemplo**:
      ```json
      {
        "mileage": 20000
      }
      ```

## 5. Machine Learning - Previsão da Condição do Carro
A funcionalidade de previsão usa o **ML.NET** para classificar se um carro está em bom estado. O modelo é treinado com dados de quilometragem e rótulos que indicam o estado do carro.

### 5.1 Treinamento do Modelo
O modelo é treinado dinamicamente ao inicializar a API, usando dados armazenados no MongoDB. Utiliza-se um classificador de regressão logística **SdcaLogisticRegression** que se baseia na quilometragem para inferir o estado do carro.

### 5.2 Pipeline de Treinamento
O pipeline de treinamento inclui:
1. **Normalização da Quilometragem**: Para garantir que os dados estejam em um formato compatível.
2. **Concatenar Features**: Para definir os recursos usados para a previsão.
3. **Treinamento**: Utiliza o classificador de regressão logística.

## 6. Como Executar o Projeto
### 6.1 Pré-Requisitos
- **.NET SDK 8**
- **MongoDB**: Banco de dados deve estar em execução para armazenar os carros.
- **Visual Studio Code** ou **Visual Studio**: Para execução e edição do código.

### 6.2 Configuração
1. Clone o repositório do projeto.
2. Configure a string de conexão do MongoDB no arquivo `appsettings.json`.
3. Execute o projeto com o comando:
   ```sh
   dotnet run
   ```

## Vinicius Durce - rm550427
