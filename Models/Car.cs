using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace SubGS_CarAPI.Models
{
    /// <summary>
    /// Cadastro de um Carro.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Identificador único do Carro.
        /// </summary>
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public String Id { get; set; }

        /// <summary>
        /// Modelo do Carro.
        /// </summary>
        [BsonElement("model"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "O modelo do carro é obrigatório.")]
        public string Model { get; set; }

        /// <summary>
        /// Descrição detalhada do Carro.
        /// </summary>
        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Description { get; set; }
        
        /// <summary>
        /// Marca do carro.
        /// </summary>
        [BsonElement("brand"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Brand { get; set; }

        /// <summary>
        /// Email do proprietário para contato.
        /// </summary>
        [BsonElement("owner_email"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "O email do proprietário é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve estar em um formato válido.")]
        public String OwnerEmail { get; set; }

        /// <summary>
        /// Quilometragem do Carro.
        /// </summary>
        [BsonElement("mileage"), BsonRepresentation(BsonType.Double)]
        [Required(ErrorMessage = "A quilometragem é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quilometragem deve ser um valor positivo.")]
        public int Mileage { get; set; }

        /// <summary>
        /// Status atual do Carro.
        /// </summary>
        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = "Disponível";

        /// <summary>
        /// Data e hora de registro do Carro.
        /// </summary>
        [BsonElement("registration_date"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "A data de registro é obrigatória.")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}
