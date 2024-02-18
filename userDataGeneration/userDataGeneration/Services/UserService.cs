using Bogus;
using userDataGeneration.Models;

namespace userDataGeneration.Services
{
    public class UserService
    {
        private readonly Random _random;

        public UserService()
        {
            _random = new Random();
        }

        public List<UserViewModel> GenerateUsers(string region, double errorRate, int seed)
        {
            // Create a faker object based on the region
            var _random = new Random(seed);
            var faker = new Faker<UserViewModel>(region);

            // Define how to generate each field of the user record
            faker.RuleFor(u => u.Index, f => f.IndexFaker);
            faker.RuleFor(u => u.RandomId, f => f.Random.Guid());
            faker.RuleFor(u => u.Name, f => f.Name.FullName());
            faker.RuleFor(u => u.Address, f => f.Address.FullAddress());
            faker.RuleFor(u => u.Phone, f => f.Phone.PhoneNumber());

            // Generate a list of user records
            var users = faker.Generate(20);

            // Apply errors to the user records based on the error rate
            foreach (var user in users)
            {
                user.Name = ApplyErrors(user.Name, errorRate);
                user.Address = ApplyErrors(user.Address, errorRate);
                user.Phone = ApplyErrors(user.Phone, errorRate);
            }

            return users;
        }

        private string ApplyErrors(string input, double errorRate)
        {
            // If the error rate is zero, return the input as it is
            if (errorRate == 0) return input;

            // Convert the input to a char array
            var chars = input.ToCharArray();

            // Calculate the number of errors to apply
            var errors = (int)Math.Round(errorRate * chars.Length);

            // Apply errors randomly to the input
            for (int i = 0; i < errors; i++)
            {
                // Choose a random position in the input
                var position = _random.Next(chars.Length);

                // Choose a random type of error
                var errorType = _random.Next(3);

                switch (errorType)
                {
                    case 0: // Delete a character
                        chars[position] = '\0'; // Replace the character with a null character
                        break;
                    case 1: // Add a random character
                        var randomChar = (char)_random.Next(32, 127); // Choose a random character from the ASCII table
                        chars[position] = randomChar; // Replace the character with the random character
                        break;
                    case 2: // Swap near characters
                        if (position < chars.Length - 1) // Check if there is a next character
                        {
                            var temp = chars[position]; // Store the current character in a temporary variable
                            chars[position] = chars[position + 1]; // Replace the current character with the next character
                            chars[position + 1] = temp; // Replace the next character with the temporary variable
                        }
                        break;
                }
            }

            // Convert the char array back to a string
            var output = new string(chars);

            // Remove any null characters from the output
            output = output.Replace("\0", "");

            return output;
        }
    }
}
