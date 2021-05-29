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
        int piece_selected_board;
        int check_chess_board;
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
            _Click(game.GetPieceLocation(Game_Logic.who_moves, 0), true, 0); // Enable clicking the white pices in the beginning
            _Click(game.GetPieceLocation(Game_Logic.who_moves, 1), true, 1);
            _Click(game.GetPieceLocation(Game_Logic.who_moves, 2), true, 2);
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
            turn_to_move.Text = Game_Logic.who_moves.ToString(); // Changing who's turn to move
        }

        private void Board1PickOrDropPiece(object sender, EventArgs e)
        {
            Cell cell_selected = (Cell)sender; // Get selected cellg
            piece_selected_board = FindBoardByCell(cell_selected); // Getting the board number pressed when moving pieces
            if (!board_selected) // If there is no board selected when clicking on a piece
            {
                board_number_clicked = cell_selected.piece.board; // saving the board number where the piece is
                board_selected = true;
            }
            for (int i = 0; i < 3; i++) // Searching in all the boards
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int column = 0; column < 8; column++) // Iterate through chess board
                    {
                        if (Game_Logic.board[i].board_cells[row][column].Equals(sender)) // If the cell clicked is found
                        {
                            if (!game.piece_clicked) // If there are no pieces selected
                            {
                                if (cell_selected.IsPiece()) // If there is a piece in the selected cell
                                {
                                    DeleteBorder(); // Deleting the highlights of the piece and possible moves
                                    cell_selected.piece.row = row; // Get the row
                                    cell_selected.piece.column = column; // Get the column
                                    Game_Logic.board[board_number_clicked].board_cells[cell_selected.piece.row][cell_selected.piece.column].BorderStyle = BorderStyle.Fixed3D; // Highlight the cell
                                    cell_selected.piece.ShowAvailableMoves(Game_Logic.board[board_number_clicked], true, board_number_clicked); // Shows all possible moves for a piece
                                    Game_Logic.available_moves_stored.AddRange(cell_selected.piece.available_moves); // Keeping available moves into a temp
                                    game.piece_clicked = true; // Set flag to true, indicates the is a selected piece
                                    _Click(game.GetPieceLocation(Game_Logic.who_moves, 0), false, 0); // Stop allowing to select other pieces on board 1
                                    _Click(game.GetPieceLocation(Game_Logic.who_moves, 1), false, 1); // Stop allowing to select other pieces on board 1
                                    _Click(game.GetPieceLocation(Game_Logic.who_moves, 2), false, 2); // Stop allowing to select other pieces on board 2                                                                                                 
                                    _Click(cell_selected.piece.available_moves, true, 0); // Allow selection only of valid moves on board 1
                                    _Click(cell_selected.piece.available_moves, true, 1); // Allow selection only of valid moves on board 1
                                    _Click(cell_selected.piece.available_moves, true, 2); // Allow selection only of valid moves on board 2
                                    Game_Logic.board[board_number_clicked].board_cells[row][column].Click -= Board1PickOrDropPiece; // Disable moving the piece on its position
                                    old_row = cell_selected.piece.row; // Saving old coordonates 
                                    old_column = cell_selected.piece.column;
                                }
                            }
                            else
                            {
                                Tuple<int, int> search_location = Tuple.Create(row, column);
                                bool is_found = false; // Flag the cheks is the coordonates are found on the board 0 or 2
                                if (board_number_clicked == 0 && piece_selected_board == 0) // If piece is on board 0 and moved on board 0
                                {
                                    game.MoveThePiece(1, 0, old_row, old_column, row, column); // Move it on board 1
                                }
                                if (board_number_clicked == 2 && piece_selected_board == 2) // If piece is on board 2 and moved on board 2
                                {
                                    game.MoveThePiece(1, 2, old_row, old_column, row, column); // Move it on board 1
                                }
                                // Moving the piece if it is on board 1
                                foreach (var board_0 in Game_Logic.available_moves_board_0) // Iterating through legal moves of board 0
                                {
                                    if (board_0.Equals(search_location)) // If there is a find
                                    {
                                        is_found = true; // Set the flag to true
                                    }
                                }
                                if (is_found) // If flag is true
                                {
                                    game.MoveThePiece(0, 1, old_row, old_column, row, column); // Move the piece on board 0
                                    check_chess_board = 0;
                                }
                                else // If not true
                                {
                                    foreach (var board_2 in Game_Logic.available_moves_board_2) // Iterate it through legal moves on board 2
                                    {
                                        if (board_2.Equals(search_location)) // If there is a find 
                                        {
                                            is_found = true; // Set flag to true
                                        }
                                    }
                                    if (is_found) // If flag is true
                                    {
                                        game.MoveThePiece(2, 1, old_row, old_column, row, column); // Move it on board 2
                                        check_chess_board = 2;
                                    }
                                    else // If not true
                                    {
                                        if (board_number_clicked == 1 && piece_selected_board == 0) // If piece is on board 1 and moved to board 0
                                        {
                                            game.MoveThePiece(0, 1, old_row, old_column, row, column); // Move it to board 0
                                        }
                                        if (board_number_clicked == 1 && piece_selected_board == 2) // If piece is on board 1 and moved on board 2
                                        {
                                            game.MoveThePiece(2, 1, old_row, old_column, row, column); // Move it on board 2
                                        }
                                    }
                                }

                                List<Tuple<int, int>> moved_piece_list = new List<Tuple<int, int>>();
                                Tuple<int, int> moved_piece_tuple = new Tuple<int, int>(row, column);
                                moved_piece_list.Add(moved_piece_tuple); // Saving piece location
                                game.piece_clicked = false; // Set flag to know piece was moved
                                DeleteBorder(); // Delete hightlights
                                _Click(Game_Logic.available_moves_stored, false, 0); // Blocking click for available moves
                                _Click(Game_Logic.available_moves_stored, false, 1);
                                _Click(Game_Logic.available_moves_stored, false, 2);
                                Game_Logic.available_moves_stored.Clear(); // Deleting 
                                _Click(moved_piece_list, false, board_number_clicked); // Disable clicking on the piece location
                                game.number_of_moves++; // Incrementing number of moves
                                if (game.beginng_match) // If the match beggins
                                {
                                    if (Game_Logic.who_moves == Piece_Color.white && game.number_of_moves == 2) // If white moves and counter is 2
                                    {
                                        Game_Logic.who_moves = Piece_Color.black; // Black moves next
                                        game.beginng_match = false; // set begin match flag to false
                                    }
                                    else if (Game_Logic.who_moves == Piece_Color.black && game.number_of_moves == 3) // If black moves and counter is 3
                                    {
                                        Game_Logic.who_moves = Piece_Color.white; // White moves next
                                    }
                                    if (game.number_of_moves == 2) // If counter is 2 
                                    {
                                        game.number_of_moves = 0; // Reset it 
                                    }
                                }
                                else
                                {
                                    if (Game_Logic.who_moves == Piece_Color.white && game.number_of_moves == 3) // If white moves and counter is 3
                                    {
                                        Game_Logic.who_moves = Piece_Color.black; // Black moves next
                                    }
                                    else if (Game_Logic.who_moves == Piece_Color.black && game.number_of_moves == 3) // If black moves and counter is 3
                                    {
                                        Game_Logic.who_moves = Piece_Color.white; // White moves next
                                    }
                                    if (game.number_of_moves == 3) // If counter is 3
                                    {
                                        game.number_of_moves = 0; // Reset it
                                    }
                                }
                                UpdateTurn(); // Update label that shows who moves
                                _Click(game.GetPieceLocation(Game_Logic.who_moves, 0), true, 0); // Enable clicking on the pieces that have the same color of the player
                                _Click(game.GetPieceLocation(Game_Logic.who_moves, 1), true, 1);
                                _Click(game.GetPieceLocation(Game_Logic.who_moves, 2), true, 2);
                                board_selected = false; // Reset flag 
                                if (piece_selected_board != 1)
                                {
                                    game.CheckForChess(piece_selected_board); // Check if king is in chess
                                }
                                else
                                {
                                    game.CheckForChess(check_chess_board);
                                }
                                if (game.king_in_chess.Item1) // If white king is in chess
                                {
                                    if (piece_selected_board != 1)
                                    {
                                        Game_Logic.board[piece_selected_board].board_cells[game.white_kings_position.Item1][game.white_kings_position.Item2].BorderStyle = BorderStyle.Fixed3D; // Highlighti it
                                    }
                                    else
                                    {
                                        Game_Logic.board[check_chess_board].board_cells[game.white_kings_position.Item1][game.white_kings_position.Item2].BorderStyle = BorderStyle.Fixed3D; // Highlighti it
                                    }
                                }
                                else // Else
                                {
                                    if (piece_selected_board != 1)
                                    {
                                        Game_Logic.board[piece_selected_board].board_cells[game.white_kings_position.Item1][game.white_kings_position.Item2].BorderStyle = BorderStyle.None; // Remove highlight
                                    }
                                    else
                                    {
                                        Game_Logic.board[check_chess_board].board_cells[game.white_kings_position.Item1][game.white_kings_position.Item2].BorderStyle = BorderStyle.None; // Remove highlight
                                    }
                                }
                                if (game.king_in_chess.Item2) // If black king is in chess
                                {
                                    if (piece_selected_board != 1)
                                    {
                                        Game_Logic.board[piece_selected_board].board_cells[game.black_kings_position.Item1][game.black_kings_position.Item2].BorderStyle = BorderStyle.Fixed3D;
                                    }
                                    else
                                    {
                                        Game_Logic.board[check_chess_board].board_cells[game.black_kings_position.Item1][game.black_kings_position.Item2].BorderStyle = BorderStyle.Fixed3D;
                                    }
                                }
                                else
                                {
                                    if (piece_selected_board != 1)
                                    {
                                        Game_Logic.board[piece_selected_board].board_cells[game.black_kings_position.Item1][game.black_kings_position.Item2].BorderStyle = BorderStyle.None;
                                    }
                                    else
                                    {
                                        Game_Logic.board[check_chess_board].board_cells[game.black_kings_position.Item1][game.black_kings_position.Item2].BorderStyle = BorderStyle.None;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void _Click(List<Tuple<int, int>> return_location, bool is_enabled, int board_number) // Method that Enables/Disables clicks on the cells
        {
            foreach (var item in return_location) // Iterate through the list
            {
                int row = item.Item1; // Save row
                int column = item.Item2; // Save column
                if (is_enabled) // If flag for click is enable
                {
                    Game_Logic.board[board_number].board_cells[row][column].Click += Board1PickOrDropPiece; // Enable click on that cell
                }
                else
                {
                    Game_Logic.board[board_number].board_cells[row][column].Click -= Board1PickOrDropPiece; // Disable click on that cell
                }
            }
        }

        public void DeleteBorder() // Method that deletes the highlights
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

        private int FindBoardByCell(Cell cell_selected) // Method that returns board number when moving the piece
        {
            for (int i = 0; i < 3; i++)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int column = 0; column < 8; column++)
                    {
                        if (Game_Logic.board[i].board_cells[row][column].Equals(cell_selected))
                        {
                            return i;
                        }
                    }
                }
            }
            return 9;
        }
    }
}
