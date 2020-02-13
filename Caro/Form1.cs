using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        #endregion
        public Form1()
        {
            InitializeComponent();

            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName);
            ChessBoard.EndedGame += EndGame;
            ChessBoard.DrawChessBoard();
            
        }
        private void isEndedGame()
        {
            pnlChessBoard.Enabled = false;
            ChessBoard.CurrentPlayer = ChessBoard.CurrentPlayer == 1 ? 0 : 1;
            MessageBox.Show("Kết thúc! người chơi " + ChessBoard.Player[ChessBoard.CurrentPlayer].Name + " thắng");
        }
        void isNewGame()
        {
            ChessBoard.DrawChessBoard();
            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName);
            
        }
        private void EndGame(object sender, EventArgs e)
        {
            isEndedGame();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName);
            ChessBoard.DrawChessBoard();
        }
    }
}
