﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestStatus : INotifyPropertyChanged
    {
        private bool _isCompleted;
        public event PropertyChangedEventHandler? PropertyChanged;
        public Quest PlayerQuest { get; }
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
            }
        }

        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
            IsCompleted = false;
        }
    }
}
