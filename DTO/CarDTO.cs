using System;
using System.ComponentModel.DataAnnotations;

namespace SubGS_CarAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object para a criação e atualização de carros.
    /// </summary>
    public class CarDTO
    {
        /// <summary>
        /// Modelo do carro.
        /// </summary>
        [Required(ErrorMessage = "O modelo do carro é obrigatório.")]
        public string Model { get; set; }

        /// <summary>
        /// Descrição detalhada do carro.
        /// </summary>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Description { get; set; }

        /// <summary>
        /// Marca do carro.
        /// </summary>
        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Brand { get; set; }

        /// <summary>
        /// Email do proprietário para contato.
        /// </summary>
        [Required(ErrorMessage = "O email do proprietário é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve estar em um formato válido.")]
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Quilometragem do carro.
        /// </summary>
        [Required(ErrorMessage = "A quilometragem é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quilometragem deve ser um valor positivo.")]
        public int Mileage { get; set; }

        /// <summary>
        /// Status atual do carro.
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = "Disponível";
    }
}