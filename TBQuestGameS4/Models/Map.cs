using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Map
    {
        #region ENUMS

        #endregion

        #region FIELDS

        private Location[] _mapLocations;
        private int _maxRows;
        private GameMapCoordinates _currentLocationCoordinates;
        private bool _canMoveForward;
        private bool _canMoveBackward;
        private List<GameItem> _standardGameItems;

        #endregion

        #region PROPERTIES

        public Location[] MapLocations
        {
            get { return _mapLocations; }
            set { _mapLocations = value; }
        }

        public GameMapCoordinates CurrentLocationCoordinates
        {
            get { return _currentLocationCoordinates; }
            set { _currentLocationCoordinates = value; }
        }

        public Location CurrentLocation
        {
            get { return _mapLocations[_currentLocationCoordinates.Row]; }
        }

        public bool CanMoveForward
        {
            get { return _canMoveForward; }
            set { _canMoveForward = value; }
        }

        public bool CanMoveBackward
        {
            get { return _canMoveBackward; }
            set { _canMoveBackward = value; }
        }

        public List<GameItem> StandardGameItems
        {
            get { return _standardGameItems; }
            set { _standardGameItems = value; }
        }
       

        #endregion

        #region CONSTRUCTORS

        public Map(int rows)
        {
            _maxRows = rows;
            _mapLocations = new Location[rows];
        }

        #endregion

        #region METHODS

        public void MoveForward()
        {
            //
            // not north
            // 
            if (_currentLocationCoordinates.Row < _maxRows - 1)
            {
                _currentLocationCoordinates.Row += 1;
            }
        }
        public void MoveBackward()
        {
            //
            // not north
            // 
            if (_currentLocationCoordinates.Row > 0)
            {
                _currentLocationCoordinates.Row -= 1;
            }
        }


        public Location NorthLocation()
        {
            Location northLocation = null;

            //
            // not on north border
            //
            if (_currentLocationCoordinates.Row < _maxRows - 1)                
            {
                Location nextNorthLocation = _mapLocations[_currentLocationCoordinates.Row + 1];

                //
                // location exists
                //
                if (nextNorthLocation != null)
                {
                    northLocation = nextNorthLocation;
                }
            }


            return northLocation;
        }

        public Location SouthLocation()
        {
            Location southLocation = null;

            //
            // not on south border
            //
            if (_currentLocationCoordinates.Row > 0)
            {
                Location nextSouthLocation = _mapLocations[_currentLocationCoordinates.Row - 1];

                //
                // location exists and player can access location
                //
                if (nextSouthLocation != null)
                {
                    southLocation = nextSouthLocation;
                }
            }

            return southLocation;
        }

        public string OpenLocationWithIdols(int idolId)
        {
            string message = "The Idol did nothing here";
            Location mapLocation = new Location();

            for (int row = 0; row < _maxRows; row++)
            {
                mapLocation = _mapLocations[row];

                if (mapLocation != null && mapLocation.RequiredIdolId == idolId)
                {
                    mapLocation.Accessible = true;
                    message = $"{mapLocation.Name} is now unlocked!";
                }
            }

            return message;
        }

        #endregion
    }
}
