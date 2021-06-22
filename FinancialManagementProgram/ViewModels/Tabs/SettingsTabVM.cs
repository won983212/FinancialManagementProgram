using System;
using System.ComponentModel;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class SettingsTabVM : TabChild
    {
        private string _TestProperty = "";

        public SettingsTabVM(TabContainer parent)
            : base(parent)
        { }

        public string TestProperty
        {
            get => _TestProperty;
            set
            {
                _TestProperty = value;
                OnPropertyChanged();
            }
        }
    }
}
