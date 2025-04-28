using System.ComponentModel.DataAnnotations;

namespace mvc_1.Models.View_Models
{
    public class Addtagrequest
    {
        [Required]
        public string Name {  get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
