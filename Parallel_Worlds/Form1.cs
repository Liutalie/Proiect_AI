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
        int board_number_clicked;
        bool board_selected = false;
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

            Controls.Add(Game_Logic.board[0].chess_board);
            Controls.Add(Game_Logic.board[1].chess_board);
            Controls.Add(Game_Logic.board[2].chess_board);
            Click(game.GetPieceLocation(Game_Logic.who_moves, 0), true, 0); // Enable clicking the white pices in the beginning
            Click(game.GetPieceLocation(Game_Logic.who_moves, 1), true, 1);
            Click(game.GetPieceLocation(Game_Logic.who_moves, 2), true, 2);
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
            if (!board_selected)
            {
                board_number_clicked = cell_selected.piece.board;
                board_selected = true;
            }
            //var piece_selected = Game_Logic.selected_piece; // Get selected piece
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++) // Iterate through chess board
                {
                    if (Game_Logic.board[board_number_clicked].board_cells[row][column].Equals(sender)) // If the cell clicked is found
                    {
                        if (!game.piece_clicked) // If there are no pieces selected
                        {
                            if (cell_selected.IsPiece()) // If there is a piece in the selected cell
                            {
                                DeleteBorder();
                                cell_selected.piece.row = row; // Get the row
                                cell_selected.piece.column = column; // Get the column
                                Game_Logic.board[board_number_clicked].board_cells[cell_selected.piece.row][cell_selected.piece.column].BorderStyle = BorderStyle.Fixed3D; // Highlight the cell
                                cell_selected.piece.ShowAvailableMoves(Game_Logic.board[board_number_clicked]); // Shows all possible moves for a piece
                                game.piece_clicked = true; // Set flag to true, indicates the is a selected piece
                                Click(game.GetPieceLocation(Game_Logic.who_moves, 0), false, 0); // Stop allowing to select other pieces on board 0
                                Click(game.GetPieceLocation(Game_Logic.who_moves, 1), false, 1); // Stop allowing to select other pieces on board 1
                                Click(game.GetPieceLocation(Game_Logic.who_moves, 2), false, 2); // Stop allowing to select other pieces on board 2
                                Click(cell_selected.piece.available_moves, true, 0); // Allow selection only of valid moves on board 0
                                Click(cell_selected.piece.available_moves, true, 1); // Allow selection only of valid moves on board 1
                                Click(cell_selected.piece.available_moves, true, 2); // Allow selection only of valid moves on board 2
                                Game_Logic.board[board_number_clicked].board_cells[row][column].Click -= Board1PickOrDropPiece;
                                old_row = cell_selected.piece.row; // Saving old coordonates 
                                old_column = cell_selected.piece.column;                               
                            }
                        }
                        else
                        {
                            List<Tuple<int, int>> moved_piece_list = new List<Tuple<int, int>>();
                            Tuple<int, int> moved_piece_tuple = new Tuple<int, int>(row, column);
                            moved_piece_list.Add(moved_piece_tuple);
                            game.MoveThePiece(board_number_clicked, old_row, old_column, row, column);
                            game.piece_clicked = false;
                            DeleteBorder();
                            Click(cell_selected.piece.available_moves, false, 0);
                            Click(cell_selected.piece.available_moves, false, 1);
                            Click(cell_selected.piece.available_moves, false, 2);
                            Click(moved_piece_list, false, board_number_clicked);
                            game.number_of_moves++;
                            if (game.beginng_match)
                            {
                                if (Game_Logic.who_moves == Piece_Color.white && game.number_of_moves == 2)
                                {
                                    Game_Logic.who_moves = Piece_Color.black;
                                    game.beginng_match = false;
                                }
                                else if(Game_Logic.who_moves == Piece_Color.black && game.number_of_moves == 3)
                                {
                                    Game_Logic.who_moves = Piece_Color.white;
                                }
                                if(game.number_of_moves == 2)
                                {
                                    game.number_of_moves = 0;
                                }
                            }
                            else
                            {
                                if (Game_Logic.who_moves == Piece_Color.white && game.number_of_moves == 3)
                                {
                                    Game_Logic.who_moves = Piece_Color.black;
                                }
                                else if(Game_Logic.who_moves == Piece_Color.black && game.number_of_moves == 3)
                                {
                                    Game_Logic.who_moves = Piece_Color.white;
                                }
                                if(game.number_of_moves == 3)
                                {
                                    game.number_of_moves = 0;
                                }
                            }
                            UpdateTurn();
                            Click(game.GetPieceLocation(Game_Logic.who_moves, 0), true, 0);
                            Click(game.GetPieceLocation(Game_Logic.who_moves, 1), true, 1);
                            Click(game.GetPieceLocation(Game_Logic.who_moves, 2), true, 2);
                            board_selected = false;
                        }
                    }
                }
            }
        }

        private void Click(List<Tuple<int, int>> return_location, bool is_enabled, int board_number)
        {
            foreach (var item in return_location)
            {
                int row = item.Item1;
                int column = item.Item2;
                if (is_enabled)
                {
                    Game_Logic.board[board_number].board_cells[row][column].Click += Board1PickOrDropPiece;
                }
                else
                {
                    Game_Logic.board[board_number].board_cells[row][column].Click -= Board1PickOrDropPiece;
                }
            }
        }

        public void DeleteBorder()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Game_Logic.board[0].board_cells[row][column].BorderStyle = BorderStyle.None; // Deleting borders of available moves
                    Game_Logic.board[1].board_cells[row][column].BorderStyle = BorderStyle.None;
                    Game_Logic.board[2].board_cells[row][column].BorderStyle = BorderStyle.None;
                }
            }
        }
    }
}
