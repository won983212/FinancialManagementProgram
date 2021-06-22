﻿using FinancialManagementProgram.Dialog;
using System;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class AnalyzeTabVM : TabChild
    {
        public AnalyzeTabVM(TabContainer parent)
            : base(parent)
        { }

        public ICommand EditBudgetCommand => new RelayCommand(() => CommonUtil.ShowDialog(new BudgetModifyDialog(DataManager), null));

        public ICommand EditCategoryCommand => new RelayCommand(() => CommonUtil.ShowDialog(new CategoryManagementDialog(), null));
    }
}
