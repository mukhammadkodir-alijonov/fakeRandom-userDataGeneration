using System.ComponentModel.DataAnnotations;

namespace userDataGeneration.Models
{
    public class User
    {
        [Key]
        public int Index { get; set; } // no errors here
        public Guid RandomId { get; set; } // no errors here
        public string Name { get; set; } // name + middle name + last name (in region format)
        public string Address { get; set; } // address (in several possible formats)
        public string Phone { get; set; } // phone (in several possible formats)
    }
}
