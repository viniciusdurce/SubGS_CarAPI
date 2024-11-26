using SubGS_CarAPI.Models;

namespace SubGS_CarAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object para atualização de carros.
    /// </summary>
    public class UpdateCarDTO
    {
        /// <summary>
        /// Modelo do carro.
        /// </summary>
        public string? Model { get; set; }

        /// <summary>
        /// Descrição detalhada do carro.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Marca do carro.
        /// </summary>
        public string? Brand { get; set; }

        /// <summary>
        /// Email do proprietário para contato.
        /// </summary>
        public string? OwnerEmail { get; set; }

        /// <summary>
        /// Quilometragem do carro.
        /// </summary>
        public int? Mileage { get; set; }

        /// <summary>
        /// Status atual do carro.
        /// </summary>
        public string? Status { get; set; } = "Disponível";
    }
}