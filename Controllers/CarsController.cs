using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SubGS_CarAPI.DTOs;
using SubGS_CarAPI.ML;
using SubGS_CarAPI.Models;
using SubGS_CarAPI.Services;
using Microsoft.ML;
using MongoDB.Driver;

namespace SubGS_CarAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciar cadastro de Carros no sistema e previsão de estado com base na quilometragem.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;
        private readonly IMongoCollection<CarData> _carDataCollection;
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CarsController"/>.
        /// </summary>
        /// <param name="carService">O serviço responsável pelas operações do carro.</param>
        /// <param name="database">O banco de dados MongoDB.</param>
        public CarsController(CarService carService, IMongoDatabase database)
        {
            _carService = carService;
            _carDataCollection = database.GetCollection<CarData>("carData");
            _mlContext = new MLContext();
            TrainModel(); // Treina o modelo automaticamente ao iniciar o controlador
        }

        /// <summary>
        /// Obtém uma lista de todos os Carros.
        /// </summary>
        /// <returns>Uma lista de carros.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }

        /// <summary>
        /// Obtém um carro específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do carro a ser obtido.</param>
        /// <returns>O carro correspondente ao ID fornecido, ou um status 404 se não for encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Car?>> GetCarById(string id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            return car is not null ? Ok(car) : NotFound();
        }

        /// <summary>
        /// Atualiza um carro existente.
        /// </summary>
        /// <param name="id">O ID do carro a ser atualizado.</param>
        /// <param name="carDto">O objeto contendo os dados atualizados do carro.</param>
        /// <returns>Um status 200 se a atualização for bem-sucedida, um status 404 se o carro não for encontrado, ou um status 400/409 em caso de erro.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(string id, UpdateCarDTO carDto)
        {
            try
            {
                var updatedCar = await _carService.UpdateCarAsync(id, carDto);
                return updatedCar != null ? Ok(updatedCar) : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo carro.
        /// </summary>
        /// <param name="carDto">Os dados do novo carro a ser criado.</param>
        /// <returns>Um status 201 se o carro for criado com sucesso, ou um status 400/409 em caso de erro.</returns>
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(CarDTO carDto)
        {
            try
            {
                var car = await _carService.CreateCarAsync(carDto);
                return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um carro específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do carro a ser deletado.</param>
        /// <returns>Um status 200 se o carro for deletado com sucesso, ou um status 404 se o carro não for encontrado.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(string id)
        {
            var deleted = await _carService.DeleteCarAsync(id);
            return deleted ? Ok() : NotFound();
        }

        /// <summary>
        /// Prever se o carro está em bom ou ruim estado com base na quilometragem.
        /// </summary>
        /// <param name="mileage">Quilometragem do carro.</param>
        /// <returns>Boolean indicando se o carro está em bom estado (true) ou ruim (false).</returns>
        [HttpPost("predict")]
        public ActionResult<bool> PredictCarCondition([FromBody] float mileage)
        {
            if (_model == null)
            {
                return BadRequest(new { message = "O modelo não foi treinado." });
            }

            var carData = new CarData { Mileage = mileage };
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CarData, CarPrediction>(_model);
            var prediction = predictionEngine.Predict(carData);
            return Ok(prediction.PredictedLabel);
        }

        /// <summary>
        /// Treina o modelo dinamicamente com os dados armazenados no MongoDB.
        /// </summary>
        private void TrainModel()
        {
            var carDataList = _carDataCollection.Find(FilterDefinition<CarData>.Empty).ToList();

            if (carDataList.Count == 0)
            {
                throw new InvalidOperationException("Nenhum dado encontrado no banco de dados para treinamento.");
            }
            var dataView = _mlContext.Data.LoadFromEnumerable(carDataList);

            var pipeline = _mlContext.Transforms.NormalizeMinMax("Mileage", "Mileage") // Normaliza a quilometragem
                .Append(_mlContext.Transforms.Concatenate("Features", "Mileage"))      // Cria a coluna de features
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label",        // Define a coluna de rótulos
                    featureColumnName: "Features")); // Define a coluna de features
            
            _model = pipeline.Fit(dataView);
        }

    }
}
