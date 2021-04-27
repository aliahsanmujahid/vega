using System.ComponentModel.DataAnnotations;

namespace API.Core.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public int NewVehicleId { get; set; }

    }
}