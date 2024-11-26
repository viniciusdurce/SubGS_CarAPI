using System.Collections.Generic;
using System.Threading.Tasks;
using SubGS_CarAPI.Data;
using MongoDB.Driver;
using SubGS_CarAPI.DTOs;
using SubGS_CarAPI.Models;

namespace SubGS_CarAPI.Services
{
    public class CarService
    {
        private readonly IMongoCollection<Car> _cars;

        public CarService(MongoDbService mongoDbService)
        {
            _cars = mongoDbService.Database?.GetCollection<Car>("cars");
        }

        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            return await _cars.Find(FilterDefinition<Car>.Empty).ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(string id)
        {
            var filter = Builders<Car>.Filter.Eq(x => x.Id, id);
            return await _cars.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Car?> UpdateCarAsync(string id, UpdateCarDTO carDto)
        {
            var car = await GetCarByIdAsync(id);
            if (car == null) return null;

            if (!string.IsNullOrEmpty(carDto.Model)) car.Model = carDto.Model;
            if (!string.IsNullOrEmpty(carDto.Description)) car.Description = carDto.Description;
            if (!string.IsNullOrEmpty(carDto.Brand)) car.Brand = carDto.Brand;
            if (carDto.Mileage.HasValue) car.Mileage = carDto.Mileage.Value;
            if (!string.IsNullOrEmpty(carDto.OwnerEmail)) car.OwnerEmail = carDto.OwnerEmail;
            if (!string.IsNullOrEmpty(carDto.Status)) car.Status = carDto.Status;

            await _cars.ReplaceOneAsync(c => c.Id == id, car);
            return car;
        }

        public async Task<Car> CreateCarAsync(CarDTO carDto)
        {
            var car = new Car()
            {
                Model = carDto.Model,
                Description = carDto.Description,
                Brand = carDto.Brand,
                Mileage = carDto.Mileage,
                OwnerEmail = carDto.OwnerEmail,
                Status = carDto.Status
            };

            await _cars.InsertOneAsync(car);
            return car;
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            var filter = Builders<Car>.Filter.Eq(x => x.Id, id);
            var result = await _cars.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
