using System.ComponentModel;
using Engine.Services;
namespace Engine.Models
{
    public class PlayerAttribute : INotifyPropertyChanged
    {
        private int _modifiedValue;
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Key { get; }
        public string DisplayName { get; }
        public string ValueRange { get; }
        public int BaseValue { get; set; }
        public int ModifiedValue
        {
            get => _modifiedValue;
            set
            {
                _modifiedValue = value;
            }
        }
        // The constructor this calls will put that same value into BaseValue and ModifiedValue
        public PlayerAttribute(string key, string displayName, string valueRange)
            : this(key, displayName, valueRange, RandomGenerate.NumberBetween(int.Parse(valueRange.Split('-')[0]), int.Parse(valueRange.Split('-')[1])))
        {
        }
        // Constructor that takes a baseValue and also uses it for modifiedValue,
        // for when we're creating a new attribute
        public PlayerAttribute(string key, string displayName, string valueRange,
                               int baseValue) :
            this(key, displayName, valueRange, baseValue, baseValue)
        {
        }
        // This constructor is eventually called by the others,
        // or used when reading a Player's attributes from a saved game file.
        public PlayerAttribute(string key, string displayName, string valueRange,
                               int baseValue, int modifiedValue)
        {
            Key = key;
            DisplayName = displayName;
            ValueRange = valueRange;
            BaseValue = baseValue;
            ModifiedValue = modifiedValue;
        }
        public void ReRoll()
        {
            BaseValue = RandomGenerate.NumberBetween(int.Parse(ValueRange.Split('-')[0]), int.Parse(ValueRange.Split('-')[1]));
            ModifiedValue = BaseValue;
        }
    }
}