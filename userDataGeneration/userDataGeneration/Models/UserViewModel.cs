namespace userDataGeneration.Models
{
    public class UserViewModel
    {
        public int Index { get; set; } // no errors here
        public Guid RandomId { get; set; } // no errors here
        public string Name { get; set; } // name + middle name + last name (in region format)
        public string Address { get; set; } // address (in several possible formats)
        public string Phone { get; set; } // phone (in several possible formats)
        public static implicit operator UserViewModel(User model)
        {
            return new UserViewModel()
            {
                Index = model.Index,
                RandomId = model.RandomId,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone
            };
        }
    }
}
