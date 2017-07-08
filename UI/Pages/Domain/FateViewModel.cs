using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Oracle.UI.Pages.Domain
{
    internal class FateViewModel : INotifyPropertyChanged
    {
        private uint fateId;
        private bool isSelected;
        private string name;

        public uint FateId
        {
            get => fateId;
            set
            {
                if (fateId == value)
                {
                    return;
                }

                fateId = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                {
                    return;
                }

                isSelected = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}