using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public class ChessBoardManager
    {

        #region Properties
        private Panel chessBoard;

        public Panel ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }

        private List<Player> player;
        public List<Player> Player { get => player; set => player = value; }

        private int currentPlayer;
        public int CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }
        
        private TextBox playerName;
        public TextBox PlayerName { get => playerName; set => playerName = value; } 

        private List<List<Button>> matrix;
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }

        private event EventHandler endedGame;
        public event EventHandler  EndedGame
        {
            add { endedGame += value; }
            remove { endedGame -= value; }
        }
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.Player = new List<Player>()
            {
                new Player("Player1", Image.FromFile(Application.StartupPath + "\\o.png")),
                new Player("Player2", Image.FromFile(Application.StartupPath + "\\x.png"))
            };
            CurrentPlayer = 0;
            ChangePlayer();
        }
        #endregion
        #region Methods
        public void DrawChessBoard()
        {
            Matrix = new List<List<Button>>();

            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };

            for (int i = 0; i < Content.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j <= Content.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Content.CHESS_WIDTH,
                        Height = Content.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    btn.Click += btn_Click;
                    ChessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);

                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Content.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn.BackgroundImage != null)
                return;

            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            ChangePlayer();

            

            if (isEndGame(btn))
            {
                EndGame();
            }        
        }

        public void EndGame()
        {   
            if (endedGame != null)
                endedGame(this, new EventArgs());
            //MessageBox.Show("End Game!");
        }
        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn); 
        }

        private Point GetChessPoint(Button btn)
        {
            
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);

            Point point = new Point(horizontal, vertical);

            return point;

        }

        private bool isEndHorizontal(Button btn) //ngang
        {
            Point point = GetChessPoint(btn);
            int coutLeft = 0;
            int temp = 0;
            //int temp2 = 0;
            for (int i = point.X; i >= 0; i--)
            //for (int i = point.X; i >= 1; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                    coutLeft++;
                else if (Matrix[point.Y][i].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }

            int coutRight = 0;
            for (int i = point.X + 1; i < Content.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                    coutRight++;
                else if (Matrix[point.Y][i].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }
                return coutLeft + coutRight == 5 && temp != 2;

            

        }
        private bool isEndVertical(Button btn) //doc
        {
            Point point = GetChessPoint(btn);
            int coutTop = 0;
            int temp = 0;
            for (int i = point.Y; i >= 0; i--)
            //for (int i = point.Y; i >= 1; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                    coutTop++;
                else if (Matrix[i][point.X].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }

            int coutBottom = 0;
            for (int i = point.Y + 1; i < Content.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                    coutBottom++;
                else if (Matrix[i][point.X].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }
            return coutTop + coutBottom == 5 && temp != 2;
        }
        private bool isEndPrimary(Button btn) //cheochinh
        {
            Point point = GetChessPoint(btn);
            int coutTop = 0;
            int temp = 0;
            for (int i = 0; i <= point.X; i++)
            //for (int i = 1; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                    coutTop++;
                else if (Matrix[point.Y- i][point.X - i].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }

            int coutBottom = 0;
            for (int i = 1; i <= Content.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.X + i >= Content.CHESS_BOARD_WIDTH || point.Y+ i >= Content.CHESS_BOARD_HEIGHT)
                    break;
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                    coutBottom++;
                else if (Matrix[point.Y + i][point.X + i].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }
            return coutTop + coutBottom == 5 && temp != 2;
        }
        private bool isEndSub(Button btn) //cheophu
        {
            Point point = GetChessPoint(btn);
            int coutTop = 0;
            int temp = 0;
            for (int i = 0; i <= point.Y; i++)
            //for (int i = 1; i <= point.X; i++)
            {
                if (point.X + i >= Content.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                    coutTop++;

                else if (Matrix[point.Y - i][point.X + i].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }

            int coutBottom = 0;
            for (int i = 1; i <= Content.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.X - i < 0 || point.Y + i >= Content.CHESS_BOARD_HEIGHT)
                    break;
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                    coutBottom++;
                else if (Matrix[point.Y + i][point.X - i].BackgroundImage != null)
                {
                    temp += 1;
                    break;
                }
                else
                    break;
            }
            return coutTop + coutBottom == 5 && temp != 2;
        }

        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurrentPlayer].Name;
        }
        #endregion
    }
}