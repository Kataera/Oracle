using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Oracle.Settings;

namespace Oracle.UI.Pages.Domain
{
    public class SpecificFateGridViewModel : INotifyPropertyChanged
    {
        private bool? isAllItems3Selected;

        public SpecificFateGridViewModel()
        {
            Items = CreateData();
        }

        public bool? IsAllItems3Selected
        {
            get
            {
                return isAllItems3Selected;
            }
            set
            {
                if (isAllItems3Selected == value)
                {
                    return;
                }

                isAllItems3Selected = value;

                if (isAllItems3Selected.HasValue)
                {
                    SelectAll(isAllItems3Selected.Value, Items);
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<FateViewModel> Items { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private static ObservableCollection<FateViewModel> CreateData()
        {
            var specificFates = FateSettings.Instance.SpecificFateList;
            var collection = new ObservableCollection<FateViewModel>();
            foreach (var fate in specificFates)
            {
                collection.Add(new FateViewModel
                {
                    FateId = fate,
                    IsSelected = true,
                    Name = "Test"
                });
            }

            return collection;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void SelectAll(bool select, IEnumerable<FateViewModel> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }
    }
}