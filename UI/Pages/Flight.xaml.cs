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

        internal Flight()
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
                MovementSettings.Instance.AzysLlaFlight = (bool) AzysLlaSetting.IsChecked;
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
                MovementSettings.Instance.ChurningMistsFlight = (bool) ChurningMistsSetting.IsChecked;
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
                MovementSettings.Instance.CoerthasWesternHighlandsFlight = (bool) CoerthasWesternHighlandsSetting.IsChecked;
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
                MovementSettings.Instance.DravanianForelandsFlight = (bool) DravanianForelandsSetting.IsChecked;
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
                MovementSettings.Instance.DravanianHinterlandsFlight = (bool) DravanianHinterlandsSetting.IsChecked;
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
            AzysLlaSetting.IsChecked = MovementSettings.Instance.AzysLlaFlight;
            ChurningMistsSetting.IsChecked = MovementSettings.Instance.ChurningMistsFlight;
            CoerthasWesternHighlandsSetting.IsChecked = MovementSettings.Instance.CoerthasWesternHighlandsFlight;
            DravanianForelandsSetting.IsChecked = MovementSettings.Instance.DravanianForelandsFlight;
            DravanianHinterlandsSetting.IsChecked = MovementSettings.Instance.DravanianHinterlandsFlight;
            SeaOfCloudsSetting.IsChecked = MovementSettings.Instance.SeaOfCloudsFlight;
        }

        private void SeaOfCloudsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetSeaOfCloudsSetting(SeaOfCloudsDefaultValue);
        }

        private void SeaOfCloudsSetting_OnClick(object sender, RoutedEventArgs e)
        {
            if (SeaOfCloudsSetting.IsChecked != null)
            {
                MovementSettings.Instance.SeaOfCloudsFlight = (bool) SeaOfCloudsSetting.IsChecked;
            }
        }

        private void SetAzysLlaSetting(bool value)
        {
            AzysLlaSetting.IsChecked = value;
            MovementSettings.Instance.AzysLlaFlight = value;
        }

        private void SetChurningMistsSetting(bool value)
        {
            ChurningMistsSetting.IsChecked = value;
            MovementSettings.Instance.ChurningMistsFlight = value;
        }

        private void SetCoerthasWesternHighlandsSetting(bool value)
        {
            CoerthasWesternHighlandsSetting.IsChecked = value;
            MovementSettings.Instance.CoerthasWesternHighlandsFlight = value;
        }

        private void SetDravanianForelandsSetting(bool value)
        {
            DravanianForelandsSetting.IsChecked = value;
            MovementSettings.Instance.DravanianForelandsFlight = value;
        }

        private void SetDravanianHinterlandsSetting(bool value)
        {
            DravanianHinterlandsSetting.IsChecked = value;
            MovementSettings.Instance.DravanianHinterlandsFlight = value;
        }

        private void SetSeaOfCloudsSetting(bool value)
        {
            SeaOfCloudsSetting.IsChecked = value;
            MovementSettings.Instance.SeaOfCloudsFlight = value;
        }
    }
}