using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parallel_Worlds
{
    public partial class Form1 : Form
    {
        Game_Logic game = new Game_Logic();
        Label[] board_number; // Vector of labels that shows number of each borad
        Label turn_to_move = new Label(); // Label that shows who's turn is to move
        int old_row = 0;
        int old_column = 0;
        public Form1()
        {
            InitializeComponent();
            Height = 600;
            Width = 1920;

            board_number = new Label[3];
            board_number[0] = new Label();
            board_number[0].Location = new Point(256 - board_number[0].Width / 2, 530);
            board_number[0].Text = "Board 1";
            board_number[0].Visible = true;
            board_number[0].Font = new Font(board_number[0].Font.Name, 14);
            Controls.Add(board_number[0]); // Adding label for first board

            board_number[1] = new Label();
            board_number[1].Location = new Point(906 - board_number[1].Width / 2, 530);
            board_number[1].Text = "Board 2";
            board_number[1].Visible = true;
            board_number[1].Font = new Font(board_number[1].Font.Name, 14);
            Controls.Add(board_number[1]); // Adding label for second board

            board_number[2] = new Label();
            board_number[2].Location = new Point(1556 - board_number[2].Width / 2, 530);
            board_number[2].Text = "Board 3";
            board_number[2].Visible = true;
            board_number[2].Font = new Font(board_number[2].Font.Name, 14);
            Controls.Add(board_number[2]); // Adding label for third board
            CenterToScreen();
            InitializeLabel();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Controls.Add(game.board[0].chess_board);
            Controls.Add(game.board[1].chess_board);
            Controls.Add(game.board[2].chess_board);
            Click(game.GetPieceLocation(Game_Logic.who_moves), true);
        }

        private void InitializeLabel()
        {
            turn_to_move.Location = new Point(530, 200);
            turn_to_move.Text = Game_Logic.who_moves.ToString();
            turn_to_move.Visible = true;
            turn_to_move.TextAlign = ContentAlignment.MiddleCenter;
            turn_to_move.Font = new Font(turn_to_move.Font.Name, 14);
            Controls.Add(turn_to_move);
        }

        public void UpdateTurn()
        {
            turn_to_move.Text = Game_Logic.who_moves.ToString();
        }

        private void Board1PickOrDropPiece(object sender, EventArgs e)
        {

            Cell cell_selected = (Cell)sender; // Get selected cell
            //var piece_selected = Game_Logic.selected_piece; // Get selected piece
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++) // Iterate through chess board
                {
                    if (game.board[0].board_cells[row][column].Equals(sender)) // If the cell clicked is found
                    {
                        if (!game.piece_clicked) // If there are no pieces selected
                        {
                            if (cell_selected.IsPiece()) // If there is a piece in the selected cell
                            {
                                DeleteBorder();
                                cell_selected.piece.row = row; // Get the row
                                cell_selected.piece.column = column; // Get the column
                                game.board[0].board_cells[cell_selected.piece.row][cell_selected.piece.column].BorderStyle = BorderStyle.Fixed3D; // Highlight the cell
                                cell_selected.piece.ShowAvailableMoves(game.board[0]); // Shows all possible moves for a piece
                                game.piece_clicked = true; // Set flag to true, indicates the is a selected piece
                                Click(game.GetPieceLocation(Game_Logic.who_moves), false); // Stop allowing to select other pieces
                                Click(cell_selected.piece.available_moves, true);
                                game.board[0].board_cells[row][column].Click -= Board1PickOrDropPiece;
                                old_row = cell_selected.piece.row; // Saving old coordonates 
                                old_column = cell_selected.piece.column;
                            }
                        }
                        else
                        {
                            List<Tuple<int, int>> moved_piece_list = new List<Tuple<int, int>>();
                            Tuple<int, int> moved_piece_tuple = new Tuple<int, int>(row, column);
                            moved_piece_list.Add(moved_piece_tuple);
                            game.MoveThePiece(0, old_row, old_column, row, column);
                            game.piece_clicked = false;
                            DeleteBorder();
                            Click(cell_selected.piece.available_moves, false);
                            Click(moved_piece_list, false);
                            if (Game_Logic.who_moves == Piece_Color.white)
                            {
                                Game_Logic.who_moves = Piece_Color.black;
                            }
                            else
                            {
                                Game_Logic.who_moves = Piece_Color.white;
                            }
                            UpdateTurn();
                            Click(game.GetPieceLocation(Game_Logic.who_moves), true);
                        }
                    }

                }
            }
        }

        private void Click(List<Tuple<int, int>> return_location, bool is_enabled)
        {
            foreach (var item in return_location)
            {
                int row = item.Item1;
                int column = item.Item2;
                if (is_enabled)
                {
                    game.board[0].board_cells[row][column].Click += Board1PickOrDropPiece;
                }
                else
                {
                    game.board[0].board_cells[row][column].Click -= Board1PickOrDropPiece;
                }
            }
        }

        public void DeleteBorder()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    game.board[0].board_cells[row][column].BorderStyle = BorderStyle.None;
                    game.board[1].board_cells[row][column].BorderStyle = BorderStyle.None;
                    game.board[2].board_cells[row][column].BorderStyle = BorderStyle.None;
                }
            }
        }
    }
}
