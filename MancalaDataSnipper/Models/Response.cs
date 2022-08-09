using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MancalaDataSnipper.Models
{
    public class Response
    {
        public GameStatus Status { get; set; }
        public int PlayerTurn;
        public ObservableCollection<int> Board;
    }

    public enum GameStatus
    {
        CotinueGame,
        Player1Win,
        Player2Win,
        Draw
    }
}
