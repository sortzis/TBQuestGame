using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace TBQuestGame.PresentationLayer
{
    public class GameSessionViewModel : ObservableObject
    {
        #region ENUMS

        #endregion

        #region CONSTANTANTS

        const string TAB = "\t";
        const string NEW_LINE = "\n";

        #endregion

        #region FIELDS

        private Player _player;
        private List<string> _messages;

        private Map _gameMap;
        private Location _currentLocation;
        private Location _northLocation, _southLocation;
        private string _currentLocationInformation;

        private GameItem _currentGameItem;
        private Npc _currentNpc;

        private Random random = new Random();

        #endregion

        #region PROPERTIES

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string MessageDisplay
        {
            get { return string.Join("\n\n", _messages); }
        }

        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }

        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                _currentLocationInformation = _currentLocation.Description;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        public Location NorthLocation
        {
            get { return _northLocation; }
            set
            {
                _northLocation = value;
                OnPropertyChanged(nameof(NorthLocation));
                OnPropertyChanged(nameof(HasNorthLocation));
            }
        }

        public Location SouthLocation
        {
            get { return _southLocation; }
            set
            {
                _southLocation = value;
                OnPropertyChanged(nameof(SouthLocation));
                OnPropertyChanged(nameof(HasSouthLocation));
            }
        }

        public string CurrentLocationInformation
        {
            get { return _currentLocationInformation; }
            set
            {
                _currentLocationInformation = value;
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        public bool HasNorthLocation { get { return NorthLocation != null; } }
        public bool HasSouthLocation { get { return SouthLocation != null; } }

        public GameItem CurrentGameItem
        {
            get { return _currentGameItem; }
            set
            {
                _currentGameItem = value;
                OnPropertyChanged(nameof(CurrentGameItem));
                if(_currentGameItem != null && _currentGameItem is Weapon)
                {
                    _player.CurrentWeapon = _currentGameItem as Weapon;
                }
            }
        }

        public Npc CurrentNpc
        {
            get { return _currentNpc; }
            set
            {
                _currentNpc = value;
                OnPropertyChanged(nameof(CurrentNpc));
            }
        }
        #endregion

        #region CONSTRUCTORS

        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(
            Player player, 
            List<string> initializeMessages,
            Map gameMap,
            GameMapCoordinates currentLocationCoordinates)
        {
            _player = player;
            _messages = initializeMessages;
            _gameMap = gameMap;
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            _currentLocation = _gameMap.CurrentLocation;
            InitializeView();

        }

        #endregion

        #region METHODS

        private void UpdateAvailableTravelPoints()
        {
            NorthLocation = null;
            SouthLocation = null;

            //
            // north location exists
            //
            if (_gameMap.NorthLocation() != null)
            {
                Location nextNorthLocation = _gameMap.NorthLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextNorthLocation.Accessible == true || PlayerCanAccessLocation(nextNorthLocation))
                {
                    NorthLocation = nextNorthLocation;
                }
            }

            //
            // south location exists
            //
            if (_gameMap.SouthLocation() != null)
            {
                Location nextSouthLocation = _gameMap.SouthLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextSouthLocation.Accessible == true || PlayerCanAccessLocation(nextSouthLocation))
                {
                    SouthLocation = nextSouthLocation;
                }
            }
        }

        private bool PlayerCanAccessLocation(Location nextLocation)
        {
            //
            // check access by experience points
            //
            if (nextLocation.IsAccessibleByExperiencePoints(_player.ExperiencePoints))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnPlayerMove()
        {
            //
            // update player stats when in new location
            //
            if (!_player.HasVisited(_currentLocation))
            {
                //
                // add location to list of visited locations
                //
                _player.LocationsVisited.Add(_currentLocation);

                // 
                // update experience points
                //
                _player.ExperiencePoints += _currentLocation.ModifyExperiencePoints;

                //
                // update health
                //
                _player.Health += _currentLocation.ModifyHealth;

                //
                // update lives
                //
                if (_currentLocation.ModifyLives != 0) _player.Lives += _currentLocation.ModifyLives;

                //
                // display a new message if available
                //
                OnPropertyChanged(nameof(MessageDisplay));
            }
        }

        public void MoveNorth()
        {
            if (HasNorthLocation)
            {
                _gameMap.MoveForward();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                _player.UpdateMissionStatus();
            }
        }

        public void MoveSouth()
        {
            if (HasSouthLocation)
            {
                _gameMap.MoveBackward();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                _player.UpdateMissionStatus();
            }
        }

        private void InitializeView()
        {
            UpdateAvailableTravelPoints();
            _player.UpdateInventoryCategories();
        }

        public void AddItemToInventory()
        {
            if (_currentGameItem != null && _currentLocation.GameItems.Contains(_currentGameItem))
            {
                GameItem selectedGameItem = _currentGameItem as GameItem;

                _currentLocation.RemoveGameItemFromLocation(selectedGameItem);
                _player.AddGameItemToInventory(selectedGameItem);

                OnPlayerPickUp(selectedGameItem);
            }
        }

        public void RemoveItemFromInventory()
        {
            //
            // confirm a game item selected
            // subtract from inventory and add to location
            //
            if (_currentGameItem != null)
            {
                //
                // cast selected game item 
                //
                GameItem selectedGameItem = _currentGameItem as GameItem;

                _currentLocation.AddGameItemToLocation(selectedGameItem);
                _player.RemoveGameItemFromInventory(selectedGameItem);

            }
        }

        private void OnPlayerPickUp(GameItem gameItem)
        {
            _player.ExperiencePoints += gameItem.ExperiencePoints;
            _player.UpdateMissionStatus();
        }

        private void OnPlayerPutDown(GameItem gameItem)
        {
            _player.UpdateMissionStatus();
        }

        public void OnUseGameItem()
        {
            switch (_currentGameItem)
            {
                case Health healthObjects:
                    ProcessHealthUse(healthObjects);
                    break;
                case Idol idol:
                    ProcessIdolUse(idol);
                    break;
                default:
                    break;
            }
        }

        private void ProcessIdolUse(Idol idol)
        {
            string message;

            switch (idol.UseAction)
            {
                case Idol.UseActionType.OPENLOCATION:
                    message = _gameMap.OpenLocationWithIdols(idol.Id);
                    CurrentLocationInformation = idol.UseMessage;
                    break;
                default:
                    break;
            }
        }

        private void ProcessHealthUse(Health healthObject)
        {
            _player.Health += healthObject.HealthChange;
            _player.Lives += healthObject.LivesChange;
            _player.RemoveGameItemFromInventory(_currentGameItem);
        }

        public void OnPlayerTalkTo()
        {
            if (CurrentNpc != null && CurrentNpc is ISpeak)
            {
                ISpeak speakingNpc = CurrentNpc as ISpeak;
                CurrentLocationInformation = speakingNpc.Speak();
                _player.NpcsEngaged.Add(_currentNpc);
                _player.UpdateMissionStatus();
            }
        }

        private void OnPlayerDies(string message)
        {
            string messagetext = message +
                "\n\nWould you like to play again?";

            string titleText = "Death";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messagetext, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuitApplication();
                    break;
            }
        }

        private string GenerateMissionTravelDetail(MissionTravel mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required Locations");
            foreach (var location in mission.RequiredLocations)
            {
                sb.AppendLine(TAB + location.Name);
            }

            if(mission.Status == Mission.MissionStatus.Incomplete)
            {
                sb.AppendLine("Locations Yet to Visit");
                foreach (var location in mission.LocationsNotCompleted(_player.LocationsVisited))
                {
                    sb.AppendLine(TAB + location.Name);
                }
            }

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        public void OpenMissionStatusView()
        {
            MissionStatusView missionStatusView = new MissionStatusView(InitializeMissionStatusViewModel());

            missionStatusView.Show();
        }

        private MissionStatusViewModel InitializeMissionStatusViewModel()
        {
            MissionStatusViewModel missionStatusViewModel = new MissionStatusViewModel();

            missionStatusViewModel.MissionInformation = GenerateMissionStatusInformation();

            missionStatusViewModel.Missions = new List<Mission>(_player.Missions);
            foreach (Mission mission in missionStatusViewModel.Missions)
            {
                if (mission is MissionTravel)
                    mission.StatusDetail = GenerateMissionTravelDetail((MissionTravel)mission);

                if (mission is MissionEngage)
                    mission.StatusDetail = GenerateMissionEngageDetail((MissionEngage)mission);

                if (mission is MissionGather)
                    mission.StatusDetail = GenerateMissionGatherDetail((MissionGather)mission);
            }

            return missionStatusViewModel;
        }

        private string GenerateMissionStatusInformation()
        {
            string missionStatusInformation;

            double totalMissions = _player.Missions.Count();
            double missionsCompleted = _player.Missions.Where(m => m.Status == Mission.MissionStatus.Complete).Count();

            int percentMissionsCompleted = (int)((missionsCompleted / totalMissions) * 100);
            missionStatusInformation = $"Missions Complete: {percentMissionsCompleted}%" + NEW_LINE;

            if (percentMissionsCompleted == 0)
            {
                missionStatusInformation += "Looks like you are just starting out. No missions complete at this point and you better get on it!";
            }
            else if (percentMissionsCompleted <= 33)
            {
                missionStatusInformation += "Well, you have some of your missions complete, but this is just a start. You have your work cut out for you for sure.";
            }
            else if (percentMissionsCompleted <= 66)
            {
                missionStatusInformation += "You are making great progress with your missions. Keep at it.";
            }
            else if (percentMissionsCompleted == 100)
            {
                missionStatusInformation += "Congratulations, you have completed all missions assigned to you.";
            }

            return missionStatusInformation;
        }

        private string GenerateMissionGatherDetail(MissionGather mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required Game Items");
            foreach (var gameItem in mission.RequiredGameItem)
            {
                sb.Append(TAB + gameItem.Name);
                sb.AppendLine($"  ( {gameItem} )");
            }

            if (mission.Status == Mission.MissionStatus.Incomplete)
            {
                sb.AppendLine("Game Items Yet to Gather");
                foreach (var gameItem in mission.GameItemNotCompleted(_player.Inventory.ToList()))
                {
                    sb.Append(TAB + gameItem.Name);
                }
            }

            sb.Remove(sb.Length - 2, 2); // remove the last two characters that generate a blank line

            return sb.ToString(); ;
        }

        private string GenerateMissionEngageDetail(MissionEngage mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required NPCs");
            foreach (var location in mission.RequiredNpcs)
            {
                sb.AppendLine(TAB + location.Name);
            }

            if (mission.Status == Mission.MissionStatus.Incomplete)
            {
                sb.AppendLine("NPCs Yet to Engage");
                foreach (var location in mission.NpcsNotCompleted(_player.NpcsEngaged))
                {
                    sb.AppendLine(TAB + location.Name);
                }
            }

            sb.Remove(sb.Length - 2, 2); // remove the last two characters that generate a blank line

            return sb.ToString(); ;
        }

        private BattleModeName NpcBattleResponse()
        {
            BattleModeName npcBattleResponse = BattleModeName.RETREAT;

            switch (DieRoll(3))
            {
                case 1:
                    npcBattleResponse = BattleModeName.ATTACK;
                    break;
                case 2:
                    npcBattleResponse = BattleModeName.DEFEND;
                    break;
                case 3:
                    npcBattleResponse = BattleModeName.RETREAT;
                    break;
            }
            return npcBattleResponse;
        }

        private int CalculateNpcHitPoints(IBattle battleNpc)
        {
            int battleNpcHitPoints = 0;

            switch (NpcBattleResponse())
            {
                case BattleModeName.ATTACK:
                    battleNpcHitPoints = battleNpc.Attack();
                    break;
                case BattleModeName.DEFEND:
                    battleNpcHitPoints = battleNpc.Defend();
                    break;
                case BattleModeName.RETREAT:
                    battleNpcHitPoints = battleNpc.Retreat();
                    break;
            }

            return battleNpcHitPoints;
        }

        private int CalculatePlayerHitPoints()
        {
            int playerHitPoints = 0;

            switch (_player.BattleMode)
            {
                case BattleModeName.ATTACK:
                    playerHitPoints = _player.Attack();
                    break;
                case BattleModeName.DEFEND:
                    playerHitPoints = _player.Defend();
                    break;
                case BattleModeName.RETREAT:
                    playerHitPoints = _player.Retreat();
                    break;
            }

            return playerHitPoints;
        }

        private void Battle()
        {
            //
            // check to see if an NPC can battle
            //
            if (_currentNpc is IBattle)
            {
                IBattle battleNpc = _currentNpc as IBattle;
                int playerHitPoints = 0;
                int battleNpcHitPoints = 0;
                string battleInformation = "";

                //
                // calculate hit points if the player and NPC have weapons
                //
                if (_player.CurrentWeapon != null)
                {
                    playerHitPoints = CalculatePlayerHitPoints();
                }
                else
                {
                    battleInformation = "It appears you are entering into battle without a weapon." + Environment.NewLine;
                }

                if (battleNpc.CurrentWeapon != null)
                {
                    battleNpcHitPoints = CalculateNpcHitPoints(battleNpc);
                }
                else
                {
                    battleInformation = $"It appears you are entering into battle with {_currentNpc.Name} who has no weapon." + Environment.NewLine;
                }

                //
                // build out the text for the current location information
                //
                battleInformation +=
                    $"Player: {_player.BattleMode}     Hit Points: {playerHitPoints}" + Environment.NewLine +
                    $"NPC: {battleNpc.BattleMode}     Hit Points: {battleNpcHitPoints}" + Environment.NewLine;

                //
                // determine results of battle
                //
                if (playerHitPoints >= battleNpcHitPoints)
                {
                    battleInformation += $"You have slain {_currentNpc.Name}.";
                    _currentLocation.Npcs.Remove(_currentNpc);
                }
                else
                {
                    battleInformation += $"You have been slain by {_currentNpc.Name}.";
                    _player.Lives--;
                }

                CurrentLocationInformation = battleInformation;
                if (_player.Lives <= 0) OnPlayerDies("You have been slain and have no lives left.");
            }
            else
            {
                CurrentLocationInformation = "The current NPC will is not battle ready. Seems you are a bit jumpy and your experience suffers.";
                _player.ExperiencePoints -= 10;
            }

        }

        public void OnPlayerAttack()
        {
            _player.BattleMode = BattleModeName.ATTACK;
            Battle();
        }

        public void OnPlayerDefend()
        {
            _player.BattleMode = BattleModeName.DEFEND;
            Battle();
        }

        public void OnPlayerRetreat()
        {
            _player.BattleMode = BattleModeName.RETREAT;
            Battle();
        }

        private void QuitApplication()
        {
            Environment.Exit(0);
        }

        private void ResetPlayer()
        {
            Environment.Exit(0);
        }
        #endregion

        #region HELPER METHODS

        private int DieRoll(int sides)
        {
            return random.Next(1, sides + 1);
        }

        #endregion
    }
}
