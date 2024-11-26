using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.ML.Data;

namespace SubGS_CarAPI.ML
{
    public class CarData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 
        [LoadColumn(0)]
        public float Mileage { get; set; }
    
        [LoadColumn(1), ColumnName("Label")]
        public bool Label { get; set; } 
    }
}