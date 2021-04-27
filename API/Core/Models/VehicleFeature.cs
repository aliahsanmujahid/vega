using System.ComponentModel.DataAnnotations.Schema;

namespace API.Core.Models
{
    [Table("VehicleFeatures")]
    public class VehicleFeature
    {
        public int VehicleId { get; set; }
        public int FeatureId { get; set; }
        public NewVehicle Vehicle { get; set; }
        public Feature Feature { get; set; }
    }
}