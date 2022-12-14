using Xunit;
using Prism.Events;
using Prism.Regions;
using MancalaDataSnipper.ViewModels;
using MancalaDataSnipper.Models;
using System.Collections.ObjectModel;

namespace MancalaDataSnipper.Tests
{
    public class MancalaDataSnipperTests
    {
        private BoardViewModel boardVM;
        IRegionManager regionManager;
        IEventAggregator eventAggregator;
        public MancalaDataSnipperTests()
        {
            boardVM = new MancalaDataSnipper.ViewModels.BoardViewModel(regionManager, eventAggregator);
        }

        /// <summary>
        /// If its player 1's turn and the pit selected is 0
        /// </summary>
        [Fact]
        public void Player1Move0()
        {
            
            boardVM.Board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
            Response response = boardVM.Move(1, 0);

            //Checking the game status and change of turn
            Assert.Equal(GameStatus.CotinueGame, response.Status);
            Assert.Equal(2, response.PlayerTurn);

            // Checking the pits in whole board
            Assert.Equal(0, response.Board[0]);
            Assert.Equal(5, response.Board[1]);
            Assert.Equal(5, response.Board[2]);
            Assert.Equal(5, response.Board[3]);
            Assert.Equal(5, response.Board[4]);
            Assert.Equal(4, response.Board[5]);

            Assert.Equal(0, response.Board[6]);

            Assert.Equal(4, response.Board[7]);
            Assert.Equal(4, response.Board[8]);
            Assert.Equal(4, response.Board[9]);
            Assert.Equal(4, response.Board[10]);
            Assert.Equal(4, response.Board[11]);
            Assert.Equal(4, response.Board[12]);

            Assert.Equal(0, response.Board[13]);
        }

        /// <summary>
        /// If its player 1's turn and the pit selected is 5
        /// </summary>
        [Fact]
        public void Player1Move5()
        {
            boardVM.Board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
            Response response = boardVM.Move(1, 5);

            //Checking the game status and change of turn
            Assert.Equal(GameStatus.CotinueGame, response.Status);
            Assert.Equal(2, response.PlayerTurn);

            
            Assert.Equal(4, response.Board[0]);
            Assert.Equal(4, response.Board[1]);
            Assert.Equal(4, response.Board[2]);
            Assert.Equal(4, response.Board[3]);
            Assert.Equal(4, response.Board[4]);
            Assert.Equal(0, response.Board[5]);

            Assert.Equal(1, response.Board[6]);

            Assert.Equal(5, response.Board[7]);
            Assert.Equal(5, response.Board[8]);
            Assert.Equal(5, response.Board[9]);
            Assert.Equal(4, response.Board[10]);
            Assert.Equal(4, response.Board[11]);
            Assert.Equal(4, response.Board[12]);

            Assert.Equal(0, response.Board[13]);
        }

        /// <summary>
        /// If its player 2's turn and the pit selected is 0
        /// </summary>
        [Fact]
        public void Player2Move0()
        {
            boardVM.Board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
            Response response = boardVM.Move(2, 0);
            //Checking the game status and change of turn
            Assert.Equal(GameStatus.CotinueGame, response.Status);
            Assert.Equal(1, response.PlayerTurn);

            // Checking the pits in whole board
            Assert.Equal(4, response.Board[0]);
            Assert.Equal(4, response.Board[1]);
            Assert.Equal(4, response.Board[2]);
            Assert.Equal(4, response.Board[3]);
            Assert.Equal(4, response.Board[4]);
            Assert.Equal(4, response.Board[5]);

            Assert.Equal(0, response.Board[6]);

            Assert.Equal(0, response.Board[7]);
            Assert.Equal(5, response.Board[8]);
            Assert.Equal(5, response.Board[9]);
            Assert.Equal(5, response.Board[10]);
            Assert.Equal(5, response.Board[11]);
            Assert.Equal(4, response.Board[12]);

            Assert.Equal(0, response.Board[13]);
        }

        /// <summary>
        /// If its player 2's turn and the pit selected is 5
        /// </summary>
        [Fact]
        public void Player2Move5()
        {
            boardVM.Board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
            Response response = boardVM.Move(2, 5);

            //Checking the game status and change of turn
            Assert.Equal(GameStatus.CotinueGame, response.Status);
            Assert.Equal(1, response.PlayerTurn);

            // Checking the pits in whole board
            Assert.Equal(5, response.Board[0]);
            Assert.Equal(5, response.Board[1]);
            Assert.Equal(5, response.Board[2]);
            Assert.Equal(4, response.Board[3]);
            Assert.Equal(4, response.Board[4]);
            Assert.Equal(4, response.Board[5]);

            Assert.Equal(0, response.Board[6]);

            Assert.Equal(4, response.Board[7]);
            Assert.Equal(4, response.Board[8]);
            Assert.Equal(4, response.Board[9]);
            Assert.Equal(4, response.Board[10]);
            Assert.Equal(4, response.Board[11]);
            Assert.Equal(0, response.Board[12]);

            Assert.Equal(1, response.Board[13]);
        }


    }
}
