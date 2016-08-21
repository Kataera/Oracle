using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Oracle.UI.Pages
{
    /// <summary>
    ///     Interaction logic for Flight.xaml
    /// </summary>
    public partial class OracleMode : UserControl
    {
        private const bool CoerthasWesternHighlandsDefaultValue = true;

        internal OracleMode()
        {
            InitializeComponent();
            RetrieveSettings();
        }

        private void CoerthasWesternHighlandsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            //SetCoerthasWesternHighlandsSetting(CoerthasWesternHighlandsDefaultValue);
        }

        private void CoerthasWesternHighlandsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            /*
            if (CoerthasWesternHighlandsSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightCoerthasWesternHighlandsEnabled = (bool)CoerthasWesternHighlandsSetting.IsChecked;
            }
            */
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private void ResetAllButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            //SetCoerthasWesternHighlandsSetting(CoerthasWesternHighlandsDefaultValue);
        }

        private void RetrieveSettings()
        {
            //CoerthasWesternHighlandsSetting.IsChecked = OracleSettings.Instance.FlightCoerthasWesternHighlandsEnabled;
        }

        private void SetCoerthasWesternHighlandsSetting(bool value)
        {
            //CoerthasWesternHighlandsSetting.IsChecked = value;
            //OracleSettings.Instance.FlightCoerthasWesternHighlandsEnabled = value;
        }
    }
}