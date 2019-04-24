using StarMap.Core.Abstractions;
using StarMap.Core.Utils;

namespace StarMap.Cll.Test.Stubs
{
    public class Dog : ObservableBase
    {
        public Dog()
        {
            Name = Randomizer.RandomString(5);
            Breed = Randomizer.RandomString(8);
            Age = Randomizer.RandomInt(1, 15);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _breed;
        public string Breed
        {
            get { return _breed; }
            set { SetProperty(ref _breed, value); }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }
    }
}
