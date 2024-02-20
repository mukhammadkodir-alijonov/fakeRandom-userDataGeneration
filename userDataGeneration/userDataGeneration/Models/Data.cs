using System.ComponentModel.DataAnnotations;

namespace userDataGeneration.Models
{
    public class Data
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Number")]
        public Guid Number { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Telephone")]
        public string Telephone { get; set; }
    }
}
