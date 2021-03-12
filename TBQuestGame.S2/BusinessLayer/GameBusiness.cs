using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;
using TBQuestGame.DataLayer;
using TBQuestGame.PresentationLayer;

namespace TBQuestGame.BusinessLayer
{
    public class GameBusiness
    {
        GameSessionViewModel _gameSessionViewModel;
        Player _player = new Player();
        List<string> _messages;
        bool _newPlayer = true;
        PlayerSetupView _playerSetupView;

        public GameBusiness()
        {
            SetupPlayer();
            InitializeDataSet();
            InstantiateAndShowView();
        }

        private void InitializeDataSet()
        {
            _messages = GameData.InitializeMessages();
        }

        private void InstantiateAndShowView()
        {
            _gameSessionViewModel = new GameSessionViewModel(_player, _messages, GameData.GameMap(), GameData.InitialGameMapLocation());
            GameSessionView gameSessionView = new GameSessionView(_gameSessionViewModel);

            gameSessionView.DataContext = _gameSessionViewModel;

            gameSessionView.Show();

            _playerSetupView.Close();
        }

        private void SetupPlayer()
        {
            if (_newPlayer)
            {
                _playerSetupView = new PlayerSetupView(_player);
                _playerSetupView.ShowDialog();

                _player.ExperiencePoints = 0;
                _player.Health = 0;
                _player.Lives = 3;
            }
            else
            {
                _player = GameData.PlayerData();
            }
        }
    }
}
