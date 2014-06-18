using System.ComponentModel.DataAnnotations;

namespace OpenRastaSwagger.SampleApi.Resources
{
    public class ComplexRequest
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}