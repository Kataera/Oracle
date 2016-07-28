using System.Windows;
using System.Windows.Controls;

using Oracle.Settings;

namespace Oracle.UI.Pages
{
    /// <summary>
    ///     Interaction logic for Flight.xaml
    /// </summary>
    public partial class Flight : UserControl
    {
        private const bool CoerthasWesternHighlandsDefaultValue = true;
        private const bool SeaOfCloudsDefaultValue = true;
        private const bool DravanianForelandsDefaultValue = true;
        private const bool ChurningMistsDefaultValue = true;
        private const bool DravanianHinterlandsDefaultValue = true;
        private const bool AzysLlaDefaultValue = true;

        public Flight()
        {
            InitializeComponent();
            RetrieveSettings();
        }

        private void AzysLlaDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetAzysLlaSetting(AzysLlaDefaultValue);
        }

        private void AzysLlaSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (AzysLlaSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightAzysLlaEnabled = (bool) AzysLlaSetting.IsChecked;
            }
        }

        private void ChurningMistsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetChurningMistsSetting(ChurningMistsDefaultValue);
        }

        private void ChurningMistsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChurningMistsSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightChurningMistsEnabled = (bool) ChurningMistsSetting.IsChecked;
            }
        }

        private void CoerthasWesternHighlandsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetCoerthasWesternHighlandsSetting(CoerthasWesternHighlandsDefaultValue);
        }

        private void CoerthasWesternHighlandsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (CoerthasWesternHighlandsSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightCoerthasWesternHighlandsEnabled = (bool) CoerthasWesternHighlandsSetting.IsChecked;
            }
        }

        private void DravanianForelandsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetDravanianForelandsSetting(DravanianForelandsDefaultValue);
        }

        private void DravanianForelandsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (DravanianForelandsSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightDravanianForelandsEnabled = (bool) DravanianForelandsSetting.IsChecked;
            }
        }

        private void DravanianHinterlandsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetDravanianHinterlandsSetting(DravanianHinterlandsDefaultValue);
        }

        private void DravanianHinterlandsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (DravanianHinterlandsSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightDravanianHinterlandsEnabled = (bool) DravanianHinterlandsSetting.IsChecked;
            }
        }

        private void ResetAllButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            SetCoerthasWesternHighlandsSetting(CoerthasWesternHighlandsDefaultValue);
            SetSeaOfCloudsSetting(SeaOfCloudsDefaultValue);
            SetDravanianForelandsSetting(DravanianForelandsDefaultValue);
            SetChurningMistsSetting(ChurningMistsDefaultValue);
            SetDravanianHinterlandsSetting(DravanianHinterlandsDefaultValue);
            SetAzysLlaSetting(AzysLlaDefaultValue);
        }

        private void RetrieveSettings()
        {
            AzysLlaSetting.IsChecked = OracleSettings.Instance.FlightAzysLlaEnabled;
            ChurningMistsSetting.IsChecked = OracleSettings.Instance.FlightChurningMistsEnabled;
            CoerthasWesternHighlandsSetting.IsChecked = OracleSettings.Instance.FlightCoerthasWesternHighlandsEnabled;
            DravanianForelandsSetting.IsChecked = OracleSettings.Instance.FlightDravanianForelandsEnabled;
            DravanianHinterlandsSetting.IsChecked = OracleSettings.Instance.FlightDravanianHinterlandsEnabled;
            SeaOfCloudsSetting.IsChecked = OracleSettings.Instance.FlightSeaOfCloudsEnabled;
        }

        private void SeaOfCloudsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetSeaOfCloudsSetting(SeaOfCloudsDefaultValue);
        }

        private void SeaOfCloudsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (SeaOfCloudsSetting.IsChecked != null)
            {
                OracleSettings.Instance.FlightSeaOfCloudsEnabled = (bool) SeaOfCloudsSetting.IsChecked;
            }
        }

        private void SetAzysLlaSetting(bool value)
        {
            AzysLlaSetting.IsChecked = value;
            OracleSettings.Instance.FlightAzysLlaEnabled = value;
        }

        private void SetChurningMistsSetting(bool value)
        {
            ChurningMistsSetting.IsChecked = value;
            OracleSettings.Instance.FlightChurningMistsEnabled = value;
        }

        private void SetCoerthasWesternHighlandsSetting(bool value)
        {
            CoerthasWesternHighlandsSetting.IsChecked = value;
            OracleSettings.Instance.FlightCoerthasWesternHighlandsEnabled = value;
        }

        private void SetDravanianForelandsSetting(bool value)
        {
            DravanianForelandsSetting.IsChecked = value;
            OracleSettings.Instance.FlightDravanianForelandsEnabled = value;
        }

        private void SetDravanianHinterlandsSetting(bool value)
        {
            DravanianHinterlandsSetting.IsChecked = value;
            OracleSettings.Instance.FlightDravanianHinterlandsEnabled = value;
        }

        private void SetSeaOfCloudsSetting(bool value)
        {
            SeaOfCloudsSetting.IsChecked = value;
            OracleSettings.Instance.FlightSeaOfCloudsEnabled = value;
        }
    }
}