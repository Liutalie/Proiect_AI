using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Worlds
{
    public enum Piece_Type
    {
        pawn = 0,
        rook = 1,
        knight = 2,
        bishop = 3,
        king = 4,
        queen = 5,
        empty = 6
    };

    public enum Piece_Color
    {
        white = 0,
        black = 1
    };
    class Piece
    {
        public Piece_Type piece_type;
        public Piece_Color piece_color;
        public int row, column, board;
        public bool moved_once;
        public List<Tuple<int, int>> available_moves;
        public Piece(Piece_Type piece_type, Piece_Color piece_color, int row, int column, int board)
        {
            available_moves = new List<Tuple<int, int>>();
            this.piece_type = piece_type;
            this.piece_color = piece_color;
            this.row = row;
            this.column = column;
            this.board = board;
            if (piece_type.Equals("pawn"))
            {
                moved_once = false;
            }
        }

        public void ShowAvailableMoves(Chess_Board chess_board)
        {
            available_moves.Clear(); // Clearing list with available moves
            // Start of pawn moves
            if (piece_type.Equals(Piece_Type.pawn)) // If piece is pawn
            {
                if (piece_color.Equals(Piece_Color.white)) // If its color is white
                {
                    if (!chess_board.board_cells[row - 1][column].IsPiece()) // If there is no piece in front 
                    {
                        available_moves.Add(new Tuple<int, int>(row - 1, column));
                    }
                    if (!moved_once && !chess_board.board_cells[row - 2][column].IsPiece()) // If pawn was not moved and second square is empty
                    {
                        available_moves.Add(new Tuple<int, int>(row - 2, column));
                    }
                    if (chess_board.board_cells[row - 1][column + 1].IsPiece() && column != 7) // If pawn can capture and is not on the right edge
                    {
                        if (chess_board.board_cells[row - 1][column + 1].piece.piece_color.Equals(Piece_Color.black)) // If upper right square contains black piece
                        {
                            available_moves.Add(new Tuple<int, int>(row - 1, column + 1));
                        }
                    }
                    if (chess_board.board_cells[row - 1][column - 1].IsPiece() && column != 0) // If upper left square contains black piece
                    {
                        if (chess_board.board_cells[row - 1][column - 1].piece.piece_color.Equals(Piece_Color.black))
                        {
                            available_moves.Add(new Tuple<int, int>(row - 1, column - 1));
                        }
                    }
                }
                else
                {
                    if (!chess_board.board_cells[row + 1][column].IsPiece())
                    {
                        available_moves.Add(new Tuple<int, int>(row + 1, column));
                    }
                    if (!moved_once && !chess_board.board_cells[row + 2][column].IsPiece())
                    {
                        available_moves.Add(new Tuple<int, int>(row + 2, column));
                    }
                    if (chess_board.board_cells[row + 1][column - 1].IsPiece() && column != 0)
                    {
                        if (chess_board.board_cells[row + 1][column - 1].piece.piece_color.Equals(Piece_Color.white))
                        {
                            available_moves.Add(new Tuple<int, int>(row + 1, column - 1));
                        }
                    }
                    if(chess_board.board_cells[row+1][column+1].IsPiece() && column != 7)
                    {
                        if (chess_board.board_cells[row + 1][column + 1].piece.piece_color.Equals(Piece_Color.white))
                        {
                            available_moves.Add(new Tuple<int, int>(row + 1, column + 1));
                        }
                    }
                }
                // End of pawn moves

                // Rook moves
            }
            foreach (var cell_on_board in available_moves)
            {
                chess_board.board_cells[cell_on_board.Item1][cell_on_board.Item2].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            }
        }
    }
}
