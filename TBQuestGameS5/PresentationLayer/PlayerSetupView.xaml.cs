using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBQuestGame.Models;
using System.Collections.ObjectModel;
using TBQuestGame.DataLayer;

namespace TBQuestGame.PresentationLayer
{
    /// <summary>
    /// Interaction logic for PlayerSetupView.xaml
    /// </summary>
    public partial class PlayerSetupView : Window
    { 
        private Player _player;
        


        public PlayerSetupView(Player player)
        {
            _player = player;

            InitializeComponent();

            SetupWindow();
        }

        private void SetupWindow()
        {
            List<string> jobs = Enum.GetNames(typeof(Player.JobType)).ToList();
            List<string> titles = Enum.GetNames(typeof(Player.NPCTitle)).ToList();
            JobComboBox.ItemsSource = jobs;
            TitleComboBox.ItemsSource = titles;

            ErrorMessageTextBlock.Visibility = Visibility.Hidden;
        }

        private bool IsValidInput(out string errorMessage)
        {
            errorMessage = "";

            if(NameTextBox.Text == "")
            {
                errorMessage += "Player name is required.\n";
            }
            else
            {
                _player.Name = NameTextBox.Text;
            }

            return errorMessage == "" ? true : false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage;

            if (IsValidInput(out errorMessage))
            {
                Enum.TryParse(JobComboBox.SelectionBoxItem.ToString(), out Player.JobType job);
                Enum.TryParse(TitleComboBox.ToString(), out Player.NPCTitle title);

                _player.Job = job;
                _player.Title = title;
                
                

                Visibility = Visibility.Hidden;
            }

            else
            {
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                ErrorMessageTextBlock.Text = errorMessage;
            }
        }
    }
}
