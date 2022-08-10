using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using MancalaDataSnipper.Views;
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
        public  int CurrentPlayer=1;
        public const int TotalPits = 14;
        public const int _StorePlayer1 = 6;
        public const int _StorePlayer2 = 13;
        public GameType gameType;
        #endregion

        #region Properties

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

        private int storePlayer1 = 0;
        public int StorePlayer1
        {
            get { return storePlayer1; }
            set { SetProperty(ref storePlayer1, value); }
        }

        private ObservableCollection<int> stonesPlayer1 = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4 };
        public ObservableCollection<int> StonesPlayer1
        {
            get { return stonesPlayer1; }
            set
            {
                if (value != stonesPlayer1)
                {
                    SetProperty(ref stonesPlayer1, value);
                }
            }
        }

        private int storePlayer2 = 0;
        public int StorePlayer2
        {
            get { return storePlayer2; }
            set { SetProperty(ref storePlayer2, value); }
        }

        private ObservableCollection<int> stonesPlayer2 = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4 };
        public ObservableCollection<int> StonesPlayer2
        {
            get { return stonesPlayer2; }
            set
            {
                if (value != stonesPlayer2)
                {
                    SetProperty(ref stonesPlayer2, value);
                }
            }
        }

        private int turn;
        public int Turn
        {
            get { return turn; }
            set { SetProperty(ref turn, value); }
        }

        private bool turnPlayer1;
        public bool TurnPlayer1
        {
            get { return turnPlayer1; }
            set { SetProperty(ref turnPlayer1, value); }
        }

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

        //private void TakeTurnCommandHandler(string PitNumber)
        //{
        //    int pitNumber = Convert.ToUInt16(PitNumber);
        //    int StonesInthePit = StonesPlayer1[pitNumber];
        //    while (StonesInthePit > 0)
        //    {
        //        if (pitNumber < 5 && pitNumber>=0)
        //        {
        //            StonesPlayer1[pitNumber + 1]++;
        //            StonesPlayer1[pitNumber]--;


        //            //int NextPitNumber = pitNumber;
        //            //for (int i = 1; i <= StonesInthePit; i++)
        //            //{
        //            //    if (i <= 5)
        //            //    //{
        //            //        StonesPlayer1[i]++;
        //            //        StonesPlayer1[pitNumber]--;
        //            //        //NextPitNumber++;
        //            //        //StonesInthePit--;
        //            //    //}
        //            //   // else
        //            //     //   break;
        //            //}
        //        }
        //        else if(pitNumber+1>5)
        //        {
        //            StorePlayer1++;
        //            StonesPlayer1[pitNumber]--;
        //        }

        //        else if(pitNumber+1 >5 && StonesInthePit>0)
        //        {

        //            StonesPlayer2[pitNumber]
        //        }



        //        //if (pitNumber + 1 > 5 || pitNumber==5)
        //        //{
        //        //    StonesInthePit = StonesPlayer1[pitNumber];
        //        //    StorePlayer1++;
        //        //    StonesPlayer1[pitNumber]--;

        //        //    int NextPitNumber = 0;
        //        //    StonesInthePit = StonesPlayer1[pitNumber];
        //        //    for (int i = 0; i < StonesInthePit; i++)
        //        //    {
        //        //        if (NextPitNumber+i <= 5)
        //        //        {
        //        //            StonesPlayer2[NextPitNumber + i]++;
        //        //            StonesPlayer1[pitNumber]--;
        //        //            NextPitNumber++;
        //        //            StonesInthePit--;
        //        //        }
        //        //        else
        //        //            break;
        //        //    }

        //        //}





        //        //StonesPlayer1[PitNumber+1]
        //        //StonesPlayer1[0]--;

        //    }

        //}


        private void TakeTurnCommandHandler(string PitNumber)
        {


            int pitNumber = Convert.ToUInt16(PitNumber);

           // int currentPlayer = GetCurrentPlayer(pitNumber);

            Response response = new Response();

            //if (Turn == 1)
            //{
            //    TurnPlayer1 = true;
            //    TurnPlayer2 = false;
            //}
            //else
            //{
            //    TurnPlayer1 = false;
            //    TurnPlayer2 = true;

            //}

            response = Move(Turn, pitNumber);

            Turn = response.PlayerTurn;
            CheckWin(response);
            CheckTurn(Turn);

            if(Turn==2 && gameType== GameType.SinglePlayer)
            {
                pitNumber = RandomNumberGenerator.GetInt32(0, 6);
                response= Move(Turn, pitNumber);
                Turn = response.PlayerTurn;
                CheckWin(response);
                CheckTurn(Turn);

            }

            //if (Turn == 1)
            //{
            //    TurnPlayer1 = true;
            //    TurnPlayer2 = false;
            //}
            //else
            //{
            //    TurnPlayer1 = false;
            //    TurnPlayer2 = true;

            //}





        }

        public Response Move(int playerNo, int pitNo)
        {
            if (pitNo < 0 || pitNo >= TotalPits)
            {
                throw new IndexOutOfRangeException();
            }

            int lastPitNo = RunMancala(playerNo, pitNo);
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
            else if (Board[lastPitNo] == 1)
            {
                int acrossPitNo = Math.Abs(11 - lastPitNo);
                Board[lastPitNo] = 0;
                int mancalaInAcrossPit = Board[acrossPitNo];
                Board[acrossPitNo] = 0;
                if (playerNo == 1)
                {
                    Board[_StorePlayer1] += mancalaInAcrossPit + 1;
                }
                else
                {
                    Board[_StorePlayer2] += mancalaInAcrossPit + 1;
                }
            }

            response.Board = Board;
            response.Status = IsDone();
            response.PlayerTurn = playerNo == 1 ? 2 : 1;
            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerNo"></param>
        /// <param name="pitNo"></param>
        /// <returns></returns>
        private int RunMancala(int playerNo, int pitNo)
        {
            pitNo += ((playerNo - 1) * 7);
            int stones = Board[pitNo];
            int currentPitNo = pitNo + 1;
            int lastPitNo = 0;
            Board[pitNo] = 0;
            while (stones > 0)
            {
                int tempPitNo = currentPitNo % TotalPits;
                lastPitNo = tempPitNo;
                if (tempPitNo == _StorePlayer1 && playerNo == 1)
                {
                    ++Board[_StorePlayer1];
                    stones--;
                }
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
        /// 
        /// </summary>
        /// <returns></returns>
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
                // Game on
                return GameStatus.CotinueGame;
            }

            if (Board[_StorePlayer1] > Board[_StorePlayer2])
            {
                // Player 1 win
                return GameStatus.Player1Win;
            }
            else if (Board[_StorePlayer1] < Board[_StorePlayer2])
            {
                // Player 2 win
                return GameStatus.Player2Win;
            }
            else
            {
                // Draw
                return GameStatus.Draw;
            }
        }

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

        /// <summary>
        /// This method returns the player number based on the pit selected
        /// </summary>
        /// <param name="PitNumber"></param>
        /// <returns>The player number</returns>
        private int GetCurrentPlayer(int PitNumber)
        {
            if (PitNumber >= 0 && PitNumber <= 5)
            {
                return 1;
            }
            else if (PitNumber >= 7 && PitNumber <= 12)
            {
                return 2;
            }
            else
                return 0;

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
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //board = new ObservableCollection<int>() { 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4, 0 };
            //StorePlayer1 = 0;
            //StorePlayer2 = 0;

            // throw new NotImplementedException();
        }
        #endregion

        #region Constructors
        public BoardViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            Turn = 1;


            //regionManager.RegisterViewWithRegion("MainContentRegion")

            TakeTurnCommand = new DelegateCommand<string>(TakeTurnCommandHandler);
            //ExitCommand = new DelegateCommand(ExitCommandHandler);

        }
        #endregion

        
    }
}
