using System;
using System.Linq;
using Prism.Regions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using MancalaDataSnipper.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Security.Cryptography;

namespace MancalaDataSnipper.ViewModels
{
    public class BoardViewModel : BindableBase, INavigationAware
    {
        #region Private Members
        private readonly IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        /// <summary>
        /// By default, player 1 will have the first turn
        /// That is why initializing current player to 1
        /// </summary>
        public  int CurrentPlayer=1;
        /// <summary>
        /// Total number of pits in the game
        /// </summary>
        public const int TotalPits = 14;

        /// <summary>
        /// Index for player 1's store
        /// </summary>
        public const int _StorePlayer1 = 6;

        /// <summary>
        /// Index for player 2's store
        /// </summary>
        public const int _StorePlayer2 = 13;

        /// <summary>
        /// Game type selected by the user
        /// </summary>
        public GameType gameType;
        #endregion

        #region Properties

        /// <summary>
        /// Collection which holds the pits and the store
        /// </summary>
        private ObservableCollection<int> board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
        public ObservableCollection<int> Board
        {
            get { return board; }
            set
            {
                if (value != board)
                {
                    SetProperty(ref board, value);
                }
            }
        }

        /// <summary>
        /// Store for player 1
        /// </summary>
        private int storePlayer1 = 0;
        public int StorePlayer1
        {
            get { return storePlayer1; }
            set { SetProperty(ref storePlayer1, value); }
        }

        /// <summary>
        /// Store for player 2 and in case game type is single player, it will be the
        /// store for CPU
        /// </summary>
        private int storePlayer2 = 0;
        public int StorePlayer2
        {
            get { return storePlayer2; }
            set { SetProperty(ref storePlayer2, value); }
        }

        /// <summary>
        /// Player 1 or 2 turn
        /// </summary>
        private int turn;
        public int Turn
        {
            get { return turn; }
            set { SetProperty(ref turn, value); }
        }

        /// <summary>
        /// Bool to enable and disable player 1's pits
        /// </summary>
        private bool turnPlayer1;
        public bool TurnPlayer1
        {
            get { return turnPlayer1; }
            set { SetProperty(ref turnPlayer1, value); }
        }


        /// <summary>
        /// Bool to enable and disable player 2's or computer's pits
        /// </summary>
        private bool turnPlayer2;
        public bool TurnPlayer2
        {
            get { return turnPlayer2; }
            set { SetProperty(ref turnPlayer2, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand<string> TakeTurnCommand { get; set; }
        #endregion

        #region CommandHandlers
        /// <summary>
        /// Command handler when we take turn
        /// </summary>
        /// <param name="PitNumber"></param>
        private void TakeTurnCommandHandler(string PitNumber)
        {
            int pitNumber = Convert.ToUInt16(PitNumber);
            Response response = new Response();

            response = Move(Turn, pitNumber);
            Turn = response.PlayerTurn;
            CheckWin(response);
            CheckTurn(Turn);

            // For single player game, the player 2 would be computer
            if(Turn==2 && gameType== GameType.SinglePlayer)
            {
                pitNumber = RandomNumberGenerator.GetInt32(0, 6);
                response= Move(Turn, pitNumber);
                Turn = response.PlayerTurn;
                CheckWin(response);
                CheckTurn(Turn);

            }
        }

        /// <summary>
        /// Make the move
        /// </summary>
        /// <param name="playerNo">
        /// Player number, it can be either 1 or 2
        /// </param>
        /// <param name="pitNo">
        /// Pit number that is selected between 0 to 5
        /// </param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public Response Move(int playerNo, int pitNo)
        {
            if (pitNo < 0 || pitNo >= TotalPits)
            {
                throw new IndexOutOfRangeException();
            }

            int lastPitNo = Runner(playerNo, pitNo);
            Response response = new Response();
            if (lastPitNo == _StorePlayer1)
            {
                response.Board = Board;
                response.Status = IsDone();
                response.PlayerTurn = 1;
                return response;
            }
            else if (lastPitNo == _StorePlayer2)
            {
                response.Board = Board;
                response.Status = IsDone();
                response.PlayerTurn = 2;
                return response;
            }
            // if last pit no is between 0-5 of player 1, includes player 1s store but not the player 2 store
            else if (playerNo == 1 && lastPitNo > _StorePlayer1 && lastPitNo < _StorePlayer2)
            {
                response.Board = Board;
                response.Status = IsDone();
                response.PlayerTurn = 2;
                return response;
            }
            else if (playerNo == 2 && lastPitNo >= 0 && lastPitNo < _StorePlayer1)
            {
                response.Board = Board;
                response.Status = IsDone();
                response.PlayerTurn = 1;
                return response;
            }

            // Changing the stones across the pits if there is one stone in the last pit
            else if (Board[lastPitNo] == 1)
            {
                int acrossPitNo = Math.Abs(11 - lastPitNo);
                Board[lastPitNo] = 0;
                int stonesInAcrossPit = Board[acrossPitNo];
                Board[acrossPitNo] = 0;
                if (playerNo == 1)
                {
                    Board[_StorePlayer1] += stonesInAcrossPit + 1;
                }
                else
                {
                    Board[_StorePlayer2] += stonesInAcrossPit + 1;
                }
            }

            response.Board = Board;
            response.Status = IsDone();
            response.PlayerTurn = playerNo == 1 ? 2 : 1;
            return response;
        }


        /// <summary>
        /// To put the stones in the respective pits and stores on the board
        /// </summary>
        /// <param name="playerNo"></param>
        /// It can be player 1 or 2
        /// <param name="pitNo"></param>
        /// The pit number which is selected.
        /// For both the users it would be 0 to 5
        /// <returns></returns>
        private int Runner(int playerNo, int pitNo)
        {
            pitNo += ((playerNo - 1) * 7);
            int stones = Board[pitNo];
            int currentPitNo = pitNo + 1;
            int lastPitNo = 0;
            Board[pitNo] = 0;

            /// While there are stones in the selected pit, we shall check in which pits we should put them in
            while (stones > 0)
            {
                int tempPitNo = currentPitNo % TotalPits;
                lastPitNo = tempPitNo;
                ///if the stone belongs in the store of player 1
                if (tempPitNo == _StorePlayer1 && playerNo == 1)
                {
                    ++Board[_StorePlayer1];
                    stones--;
                }
                /// if the stone belongs to the store of player 2
                else if (tempPitNo == _StorePlayer2 && playerNo == 2)
                {
                    ++Board[_StorePlayer2];
                    stones--;
                }

                if (tempPitNo == _StorePlayer1 || tempPitNo == _StorePlayer2)
                {
                    currentPitNo++;
                    continue;
                }
                ++Board[tempPitNo];
                stones--;
                currentPitNo++;
            }

            return lastPitNo;
        }


        /// <summary>
        /// Check the game status
        /// </summary>
        /// <returns> Returns the status that either to continue game
        /// or a player won or its a draw</returns>
        private GameStatus IsDone()
        {
            bool isDone = false;
            if (!Board.Take(6).Any(x => x > 0))
            {
                Board[_StorePlayer2] += Board.Skip(7).Take(6).Sum();
                isDone = true;
            }
            else if (!Board.Skip(7).Take(6).Any(x => x > 0))
            {
                Board[_StorePlayer1] += Board.Take(6).Sum();
                isDone = true;
            }

            if (!isDone)
            {
                // Continue game
                return GameStatus.CotinueGame;
            }

            if (Board[_StorePlayer1] > Board[_StorePlayer2])
            {
                // Player 1 has won
                return GameStatus.Player1Win;
            }
            else if (Board[_StorePlayer1] < Board[_StorePlayer2])
            {
                // Player 2 has won
                return GameStatus.Player2Win;
            }
            else
            {
                // It is a draw
                return GameStatus.Draw;
            }
        }

        /// <summary>
        /// Show message based on the response
        /// </summary>
        /// <param name="response"></param>
        private void CheckWin(Response response)
        {
            if (response.Status == GameStatus.Player1Win)
            {
                MessageBox.Show("Player 1 has won!");
            }

            else if (response.Status == GameStatus.Player2Win)
            {
                MessageBox.Show("Player 2 has won!");

            }

            else if (response.Status == GameStatus.Draw)
            {
                MessageBox.Show("Its a draw!");
            }

        }

        /// <summary>
        /// Check if its player 1 turn or player 2
        /// </summary>
        /// <param name="turn"></param>
        private void CheckTurn(int turn)
        {

            if (Turn == 1)
            {
                TurnPlayer1 = true;
                TurnPlayer2 = false;
            }
            else
            {
                TurnPlayer1 = false;
                TurnPlayer2 = true;

            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            gameType = (GameType)navigationContext.Parameters["GameType"];
            CurrentPlayer = 1;
            Turn = 1;
            TurnPlayer1 = true;
            TurnPlayer2 = false;
            Board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
            StorePlayer1 = 0;
            StorePlayer2 = 0;

            // throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion

        #region Constructors
        public BoardViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            Turn = 1;
            TakeTurnCommand = new DelegateCommand<string>(TakeTurnCommandHandler);
        }
        #endregion 
    }
}
