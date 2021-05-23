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
        }

        public Piece MoveThePiece(int board_number, int old_row, int old_column, int new_row, int new_column)
        {
            if (board[board_number].board_cells[old_row][old_column].piece.piece_type.Equals(Piece_Type.pawn))
            {
                board[board_number].board_cells[old_row][old_column].piece.moved_once = true;
            }
            board[board_number].board_cells[new_row][new_column].piece = board[board_number].board_cells[old_row][old_column].piece;
            board[board_number].board_cells[new_row][new_column].Image = board[board_number].board_cells[old_row][old_column].Image;
            board[board_number].board_cells[old_row][old_column].piece = null;
            board[board_number].board_cells[old_row][old_column].Image = null;
            if (!board_number.Equals(1) && board[board_number].board_cells[new_row][new_column].piece.piece_type.Equals(Piece_Type.pawn))
            {
                if (board[board_number].board_cells[new_row][new_column].piece.piece_color.Equals(Piece_Color.white))
                {
                    if (new_row == 0)
                    {
                        Promovation promovation = new Promovation(board_number,new_row,new_column);
                        promovation.ShowPromovation(Piece_Color.white);
                        promovation.Show();
                    }
                }
                else
                {
                    if (new_row == 7)
                    {
                        Promovation promovation = new Promovation(board_number, new_row, new_column);
                        promovation.ShowPromovation(Piece_Color.black);
                        promovation.Show();
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
    }
}
