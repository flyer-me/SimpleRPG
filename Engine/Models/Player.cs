using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Player:INotifyPropertyChanged
    {
        private int _experiencePoints;
        public string? Name { get; set; }
        public string? CharacterClass { get; set; }
        public int HitPoints { get; set; }
        public int ExperiencePoints 
        { 
            get { return _experiencePoints; }
            set { _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
            }
        }
        public int Level { get; set; }
        public int Assets { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}