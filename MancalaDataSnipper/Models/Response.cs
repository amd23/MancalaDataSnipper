using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MancalaDataSnipper.Models
{
    /// <summary>
    /// Reponse of a certain move in the game
    /// </summary>
    public class Response
    {
        public GameStatus Status { get; set; }
        public int PlayerTurn;
        public ObservableCollection<int> Board;
    }

    /// <summary>
    /// Enum for the game status after a certain move
    /// </summary>
    public enum GameStatus
    {
        CotinueGame,
        Player1Win,
        Player2Win,
        Draw
    }

    /// <summary>
    /// Enum for the game type
    /// </summary>
    public enum GameType
    {
        SinglePlayer,
        DoublePlayer
    }
}
