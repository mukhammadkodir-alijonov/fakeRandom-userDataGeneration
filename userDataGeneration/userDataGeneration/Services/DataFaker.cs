using Bogus;
using System.Text;
using userDataGeneration.Models;

namespace userDataGeneration.Services
{
    public class DataFaker
    {
        private int _seed;
        private string _lang;
        private int _startIds;
        private float _totalErrors;
        private Faker _faker;
        private int _remainingErrors;

        public DataFaker(int seed, string lang, int startIds, float totalErrors)
        {
            _startIds = startIds;
            _seed = seed;
            _lang = lang;
            _totalErrors = totalErrors;
            _faker = new Faker(_lang);
            _faker.Random = new Randomizer(_seed);
        }

        public List<Data> Get(int n)
        {
            Faker<Data> generator = new Faker<Data>(locale: _lang).UseSeed(seed: _seed)
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => _startIds++)
                 .RuleFor(o => o.Number, f => f.Random.Guid())
                 .RuleFor(o => o.Name, (f, a) => f.PickRandom(new[]
                 {
                     f.Name.JobDescriptor() + " " + f.Name.FirstName() +" "+ f.Name.LastName(),
                     f.Name.JobTitle() + " " + f.Name.LastName() + " " + f.Name.FirstName(),
                     f.Name.JobArea() +" " + f.Name.JobTitle() + " " + f.Name.FullName()
                 }))
                 .RuleFor(o => o.Address, (f, a) => f.PickRandom(new[]
                 {
                    f.Address.ZipCode() + ", " + f.Address.City() + ", " + f.Address.StreetAddress(),
                    f.Address.StreetAddress() + ", " + f.Address.BuildingNumber() + "/" +f.Random.Int(1,50) ,
                    f.Address.City() + ", " + f.Address.ZipCode() + ", " + f.Address.Country(),
                    f.Address.Country() + ", " + f.Address.City(),
                    f.Address.FullAddress()
                 }))
                 .RuleFor(o => o.Telephone, f => f.Phone.PhoneNumber());

            return generator.Generate(n);
        }
        public void ErrorsGenerator(ref List<Data> data)
        {
            if (_totalErrors > 0)
            {
                StringBuilder stringBuilder;
                List<Data> errors = new List<Data>();
                foreach (var e in data)
                {
                    _remainingErrors = (int)_totalErrors;
                    stringBuilder = new StringBuilder(e.Name + ";" + e.Address + ";" + e.Telephone);
                    string errorData = ErrorsAction(stringBuilder);
                    var words = errorData.Split(";");
                    Data obj = new Data() { Id = e.Id, Number = e.Number, Name = words[0], Address = words[1], Telephone = words[2] };
                    errors.Add(obj);
                }
                data = errors;
            }

        }
        private string ErrorsAction(StringBuilder builder)
        {
            int count = CalculateErrors();
            for (int i = 0; i < count; i++) { AddSymbolError(ref builder); }
            _remainingErrors -= CalculateErrors();
            count = CalculateErrors();
            for (int i = 0; i < count; i++) { ReplaceSymbolError(ref builder); }
            _remainingErrors -= CalculateErrors();
            count = CalculateErrors();
            for (int i = 0; i < count; i++) { DeleteSymbolError(ref builder); }
            return builder.ToString();
        }



        private void AddSymbolError(ref StringBuilder builder)
        {
            bool flag = true;
            int index = _faker.Random.Int(0, builder.Length - 1);
            while (flag)
            {
                if (builder[index] == ';') { index = _faker.Random.Int(0, builder.Length - 1); }
                else
                {
                    AddSymbol(ref builder, index);
                    flag = false;
                }
            }

            void AddSymbol(ref StringBuilder builder, int index)
            {
                var Symbol = _faker.Lorem.Letter();
                var select = _faker.Random.Int(0, 2);
                var addictiveSymbol = select == 2 ? Symbol.ToUpper() : select == 1 ? Symbol.ToLower() : _faker.Random.Int(0, 9).ToString();
                builder.Insert(index, addictiveSymbol);
            }
        }

        private void ReplaceSymbolError(ref StringBuilder builder)
        {
            bool flag = true;
            int index = _faker.Random.Int(0, builder.Length - 1);
            while (flag)
            {
                if (builder[index] == ';') { index = _faker.Random.Int(0, builder.Length - 1); }
                else
                {
                    ReplaceSymbol(ref builder, index);
                    flag = false;
                }
            }

        }

        private static void ReplaceSymbol(ref StringBuilder builder, int index)
        {
            int replacerIndex = index + 1;
            if (replacerIndex >= builder.Length)
            {
                replacerIndex = index - 1;
            }
            if (builder[replacerIndex] == ';') { replacerIndex = index + 1; }
            (builder[index], builder[replacerIndex]) = (builder[replacerIndex], builder[index]);
        }

        private void DeleteSymbolError(ref StringBuilder builder)
        {
            bool flag = true;
            int index = _faker.Random.Int(0, builder.Length - 1);
            while (flag)
            {
                if (builder[index] == ';') { index = _faker.Random.Int(0, builder.Length - 1); }
                else
                {
                    builder.Remove(index, 1);
                    flag = false;
                }
            }
        }

        private int CalculateErrors()
        {
            var errors = (int)Math.Round(_totalErrors * 0.33);
            errors = Math.Min(errors, _remainingErrors);
            return errors;
        }
    }
}
