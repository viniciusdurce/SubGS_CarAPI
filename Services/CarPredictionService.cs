using Microsoft.ML;
using Microsoft.ML.Data;
using MongoDB.Driver;
using SubGS_CarAPI.ML;

namespace SubGS_CarAPI.Services
{
    public class CarPredictionService
    {
        private readonly IMongoCollection<CarData> _carDataCollection;
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        public CarPredictionService(IMongoDatabase database)
        {
            // Inicializa a conexão com o MongoDB e o MLContext
            _carDataCollection = database.GetCollection<CarData>("carData");
            _mlContext = new MLContext();
        }

        /// <summary>
        /// Treina o modelo dinamicamente com os dados armazenados no MongoDB.
        /// </summary>
        private void TrainModel()
        {
            // Carrega os dados do MongoDB
            var carDataList = _carDataCollection.Find(FilterDefinition<CarData>.Empty).ToList();

            if (carDataList.Count == 0)
            {
                throw new InvalidOperationException("Nenhum dado encontrado no banco de dados para treinamento.");
            }

            // Converte os dados em um formato compatível para o ML.NET
            var dataView = _mlContext.Data.LoadFromEnumerable(carDataList);

            // Define o pipeline de treinamento
            var pipeline = _mlContext.Transforms.NormalizeMinMax("Mileage", "Mileage") // Normaliza a quilometragem
                .Append(_mlContext.Transforms.Concatenate("Features", "Mileage"))      // Cria a coluna de features
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label",        // Define a coluna de rótulos
                    featureColumnName: "Features")); // Define a coluna de features

            // Treina o modelo
            _model = pipeline.Fit(dataView);
        }

        /// <summary>
        /// Faz uma previsão com base na quilometragem fornecida.
        /// </summary>
        /// <param name="carData">Dados do carro (quilometragem).</param>
        /// <returns>True se o carro estiver em bom estado, false caso contrário.</returns>
        public bool Predict(CarData carData)
        {
            if (_model == null)
            {
                throw new InvalidOperationException("O modelo não foi treinado. Execute o método TrainModel() antes de fazer previsões.");
            }

            // Cria o mecanismo de previsão
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CarData, CarPrediction>(_model);

            // Faz a previsão
            var prediction = predictionEngine.Predict(carData);
            return prediction.PredictedLabel;
        }
    }
}
