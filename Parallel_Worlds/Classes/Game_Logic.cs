using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Worlds
{
    class Game_Logic
    {
        static public Chess_Board[] board;
        static public Piece_Color who_moves;
        public static Piece selected_piece;
        public bool piece_clicked;
        public bool beginng_match;
        public int number_of_moves;
        public Tuple<int, int> white_kings_position;
        public Tuple<int, int> black_kings_position;
        public Tuple<bool, bool> king_in_chess;
        public Tuple<bool, bool> king_in_mate;
        static public List<Tuple<int, int>> available_moves_stored;
        static public List<Tuple<int, int>> available_moves_board_0;
        static public List<Tuple<int, int>> available_moves_board_2;
        public Game_Logic()
        {
            board = new Chess_Board[3];
            board[0] = new Chess_Board(0);
            board[1] = new Chess_Board(1);
            board[2] = new Chess_Board(2);
            who_moves = Piece_Color.white; // White moves first
            beginng_match = true;
            number_of_moves = 0;
            piece_clicked = false;
            white_kings_position = new Tuple<int, int>(7, 4);
            black_kings_position = new Tuple<int, int>(0, 4);
            king_in_chess = new Tuple<bool, bool>(false, false);
            king_in_mate = new Tuple<bool, bool>(false, false);
            available_moves_stored = new List<Tuple<int, int>>();
            available_moves_board_0 = new List<Tuple<int, int>>();
            available_moves_board_2 = new List<Tuple<int, int>>();
        }

        public Piece MoveThePiece(int board_number, int former_board, int old_row, int old_column, int new_row, int new_column)
        {
            if (!former_board.Equals(1)) // If board is not 1
            {
                if (board[former_board].board_cells[old_row][old_column].piece.piece_type.Equals(Piece_Type.pawn)) // If piece is pawn
                {
                    board[former_board].board_cells[old_row][old_column].piece.moved_once = true; // Set flag that indicates that pawn moved once
                }
            }
            if (board[former_board].board_cells[old_row][old_column].piece.piece_type.Equals(Piece_Type.king) && board[former_board].board_cells[old_row][old_column].piece.piece_color
                .Equals(Piece_Color.white)) // If white king moved, save his new coord
            {
                white_kings_position = new Tuple<int, int>(new_row, new_column);
            }
            if (board[former_board].board_cells[old_row][old_column].piece.piece_type.Equals(Piece_Type.king) && board[former_board].board_cells[old_row][old_column].piece.piece_color
                .Equals(Piece_Color.black)) // If black king moved, save his new coord
            {
                black_kings_position = new Tuple<int, int>(new_row, new_column);
            }
            if (!board[former_board].board_cells[old_row][old_column].piece.piece_type.Equals(Piece_Type.king)) // If the piece moved is not king, move it on the new borad
            {
                board[former_board].board_cells[new_row][new_column].piece = board[former_board].board_cells[old_row][old_column].piece;
                board[former_board].board_cells[new_row][new_column].piece.row = new_row;
                board[former_board].board_cells[new_row][new_column].piece.column = new_column;
                board[former_board].board_cells[new_row][new_column].Image = board[former_board].board_cells[old_row][old_column].Image;

                board[board_number].board_cells[new_row][new_column].piece = board[former_board].board_cells[old_row][old_column].piece;
                board[board_number].board_cells[new_row][new_column].piece.row = new_row;
                board[board_number].board_cells[new_row][new_column].piece.column = new_column;
                board[board_number].board_cells[new_row][new_column].Image = board[former_board].board_cells[old_row][old_column].Image;
                board[board_number].board_cells[new_row][new_column].piece.board = board_number;

                board[former_board].board_cells[old_row][old_column].piece = null;
                board[former_board].board_cells[old_row][old_column].Image = null;
                board[former_board].board_cells[new_row][new_column].piece = null;
                board[former_board].board_cells[new_row][new_column].Image = null;
            }
            else // If the piece is king, keep it on the same board
            {
                board[former_board].board_cells[new_row][new_column].piece = board[former_board].board_cells[old_row][old_column].piece;
                board[former_board].board_cells[new_row][new_column].piece.row = new_row;
                board[former_board].board_cells[new_row][new_column].piece.column = new_column;
                board[former_board].board_cells[new_row][new_column].Image = board[former_board].board_cells[old_row][old_column].Image;
                board[former_board].board_cells[new_row][new_column].piece.board = board_number;
                board[former_board].board_cells[old_row][old_column].piece = null;
                board[former_board].board_cells[old_row][old_column].Image = null;
            }
            if (!board_number.Equals(1)) // If the piece is not on board 1
            {
                if (board[board_number].board_cells[new_row][new_column].piece != null) // If there is no piece on the new coord
                {
                    if (board[board_number].board_cells[new_row][new_column].piece.piece_type.Equals(Piece_Type.pawn)) // If the piece is pawn
                    {
                        if (board[board_number].board_cells[new_row][new_column].piece.piece_color.Equals(Piece_Color.white)) // If piece color is white
                        {
                            if (new_row == 0) // If it reached at the end
                            {
                                // Start the promotion for white pieces
                                Promovation promovation = new Promovation(board_number, new_row, new_column);
                                promovation.ShowPromovation(Piece_Color.white);
                                promovation.Show();
                            }
                        }
                        else // If its black
                        {
                            if (new_row == 7) // If it reached the end
                            {
                                // Start promotion for black pieces
                                Promovation promovation = new Promovation(board_number, new_row, new_column);
                                promovation.ShowPromovation(Piece_Color.black);
                                promovation.Show();
                            }
                        }
                    }
                }
            }

            return null;
        }

        public List<Tuple<int, int>> GetPieceLocation(Piece_Color color, int board_number) // Method that takes board number and color of the piece
        {
            List<Tuple<int, int>> return_location = new List<Tuple<int, int>>(); // Make a local list 
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++) // Iterating through the board
                {
                    if (board[board_number].board_cells[row][column].IsPiece()) // If there is a piece
                    {
                        if (board[board_number].board_cells[row][column].piece.piece_color == color) // If the color matches
                        {
                            return_location.Add(new Tuple<int, int>(row, column)); // Add its coordonates into the list 
                        }
                    }
                }
            }
            return return_location; // Return pieces coordonates
        }

        public Tuple<bool, bool> CheckForChess(int board_number) // Method that cheks if king is in chess
        {
            bool king_in_chess_white = false; // Flag for white king
            bool king_in_chess_black = false; // Flag for black king
            //bool king_in_mate_white = false;
            //bool king_in_mate_black = false;
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (board[board_number].board_cells[row][column].IsPiece()) // Iterating through every piece
                    {
                        board[board_number].board_cells[row][column].piece.ShowAvailableMoves(board[board_number], false, board_number); // Calculate available moves
                        foreach (var all_moves in board[board_number].board_cells[row][column].piece.available_moves) // Iterate through available moves
                        {
                            if (all_moves.Equals(white_kings_position) && board[board_number].board_cells[row][column].piece.piece_color.Equals(Piece_Color.black)) // If any piece can reach the king
                            {
                                king_in_chess_white = true; // Set white king flag to true
                            }
                            if (all_moves.Equals(black_kings_position) && board[board_number].board_cells[row][column].piece.piece_color.Equals(Piece_Color.white))
                            {
                                king_in_chess_black = true;
                            }
                        }
                    }
                }
            }
            king_in_chess = new Tuple<bool, bool>(king_in_chess_white, king_in_chess_black); // Save the flags in a tuple 
            return king_in_chess;
        }
    }
}
