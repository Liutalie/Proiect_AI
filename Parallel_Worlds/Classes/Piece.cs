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

        public void ShowAvailableMoves(Chess_Board chess_board, bool show_moves, int board_number)
        {
            bool pawn_first_move = false; // Flag for pawns first move
            available_moves.Clear(); // Clearing list with available moves
            // Start of pawn moves
            if (board_number == 0 || board_number == 2) // If piece is on board 0 or 2 it moves normally
            {
                if (piece_type.Equals(Piece_Type.pawn)) // If piece is pawn
                {
                    if (piece_color.Equals(Piece_Color.white)) // If its color is white
                    {
                        if (row - 1 >= 0 && !chess_board.board_cells[row - 1][column].IsPiece()) // If there is no piece in front 
                        {
                            pawn_first_move = true;
                            available_moves.Add(new Tuple<int, int>(row - 1, column));
                        }
                        if (!moved_once && !chess_board.board_cells[row - 2][column].IsPiece() && pawn_first_move) // If pawn was not moved and second square is empty
                        {
                            available_moves.Add(new Tuple<int, int>(row - 2, column));
                        }
                        if (column != 7 && chess_board.board_cells[row - 1][column + 1].IsPiece()) // If pawn can capture and is not on the right edge
                        {
                            if (chess_board.board_cells[row - 1][column + 1].piece.piece_color != piece_color) // If upper right square contains black piece
                            {
                                available_moves.Add(new Tuple<int, int>(row - 1, column + 1));
                            }
                        }
                        if (row != 0 && column != 0 && chess_board.board_cells[row - 1][column - 1].IsPiece()) // If upper left square contains black piece
                        {
                            if (chess_board.board_cells[row - 1][column - 1].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row - 1, column - 1));
                            }
                        }
                    }
                    else
                    {
                        if (row + 1 < 8 && !chess_board.board_cells[row + 1][column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(row + 1, column));
                        }
                        if (!moved_once && !chess_board.board_cells[row + 2][column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(row + 2, column));
                        }
                        if (column != 0 && chess_board.board_cells[row + 1][column - 1].IsPiece())
                        {
                            if (chess_board.board_cells[row + 1][column - 1].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row + 1, column - 1));
                            }
                        }
                        if (column != 7 && chess_board.board_cells[row + 1][column + 1].IsPiece())
                        {
                            if (chess_board.board_cells[row + 1][column + 1].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row + 1, column + 1));
                            }
                        }
                    }

                }
                // End of pawn moves

                // Rook moves
                if (piece_type.Equals(Piece_Type.rook))
                {
                    // South available moves
                    int temporary = row + 1;
                    while (temporary < 8)
                    {
                        if (!chess_board.board_cells[temporary][column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary, column));
                            temporary++;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary][column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary, column));
                            }
                            break;
                        }
                    }

                    // North available moves
                    temporary = row - 1;
                    while (temporary >= 0)
                    {
                        if (!chess_board.board_cells[temporary][column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary, column));
                            temporary--;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary][column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary, column));
                            }
                            break;
                        }
                    }

                    // East available moves
                    temporary = column + 1;
                    while (temporary < 8)
                    {
                        if (!chess_board.board_cells[row][temporary].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(row, temporary));
                            temporary++;
                        }
                        else
                        {
                            if (chess_board.board_cells[row][temporary].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row, temporary));
                            }
                            break;
                        }
                    }

                    // West available moves
                    temporary = column - 1;
                    while (temporary >= 0)
                    {
                        if (!chess_board.board_cells[row][temporary].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(row, temporary));
                            temporary--;
                        }
                        else
                        {
                            if (chess_board.board_cells[row][temporary].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row, temporary));
                            }
                            break;
                        }
                    }
                }
                // End of rook

                // Knight moves
                if (piece_type.Equals(Piece_Type.knight))
                {
                    int[] x = { 2, 1, -1, -2, -2, -1, 1, 2 };
                    int[] y = { 1, 2, 2, 1, -1, -2, -2, -1 };
                    for (int index = 0; index < 8; index++)
                    {
                        int new_row = row + x[index];
                        int new_column = column + y[index];
                        if (new_row >= 0 && new_column < 8 && new_row < 8 && new_column >= 0)
                        {
                            if (!chess_board.board_cells[new_row][new_column].IsPiece())
                            {
                                available_moves.Add(new Tuple<int, int>(new_row, new_column));
                            }
                            else
                            {
                                if (chess_board.board_cells[new_row][new_column].piece.piece_color != piece_color)
                                {
                                    available_moves.Add(new Tuple<int, int>(new_row, new_column));
                                }
                            }
                        }
                    }
                }
                // End of Knight

                // Bishop moves
                if (piece_type.Equals(Piece_Type.bishop))
                {
                    // South East moves
                    int temporary_row = row - 1;
                    int temporary_column = column + 1;
                    while (temporary_row >= 0 && temporary_column < 8)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row--;
                            temporary_column++;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // South West moves
                    temporary_row = row - 1;
                    temporary_column = column - 1;
                    while (temporary_row >= 0 && temporary_column >= 0)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row--;
                            temporary_column--;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // North East moves
                    temporary_row = row + 1;
                    temporary_column = column + 1;
                    while (temporary_row < 8 && temporary_column < 8)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row++;
                            temporary_column++;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // North West moves
                    temporary_row = row + 1;
                    temporary_column = column - 1;
                    while (temporary_row < 8 && temporary_column >= 0)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row++;
                            temporary_column--;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }
                }
                //End of Bishop

                // Queen moves
                if (piece_type.Equals(Piece_Type.queen))
                {
                    // South East moves
                    int temporary_row = row - 1;
                    int temporary_column = column + 1;
                    while (temporary_row >= 0 && temporary_column < 8)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row--;
                            temporary_column++;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // South West moves
                    temporary_row = row - 1;
                    temporary_column = column - 1;
                    while (temporary_row >= 0 && temporary_column >= 0)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row--;
                            temporary_column--;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // North East moves
                    temporary_row = row + 1;
                    temporary_column = column + 1;
                    while (temporary_row < 8 && temporary_column < 8)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row++;
                            temporary_column++;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // North West moves
                    temporary_row = row + 1;
                    temporary_column = column - 1;
                    while (temporary_row < 8 && temporary_column >= 0)
                    {
                        if (!chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            temporary_row++;
                            temporary_column--;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary_row][temporary_column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                            }
                            break;
                        }
                    }

                    // South available moves
                    int temporary = row + 1;
                    while (temporary < 8)
                    {
                        if (!chess_board.board_cells[temporary][column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary, column));
                            temporary++;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary][column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary, column));
                            }
                            break;
                        }
                    }

                    // North available moves
                    temporary = row - 1;
                    while (temporary >= 0)
                    {
                        if (!chess_board.board_cells[temporary][column].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(temporary, column));
                            temporary--;
                        }
                        else
                        {
                            if (chess_board.board_cells[temporary][column].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(temporary, column));
                            }
                            break;
                        }
                    }

                    // East available moves
                    temporary = column + 1;
                    while (temporary < 8)
                    {
                        if (!chess_board.board_cells[row][temporary].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(row, temporary));
                            temporary++;
                        }
                        else
                        {
                            if (chess_board.board_cells[row][temporary].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row, temporary));
                            }
                            break;
                        }
                    }

                    // West available moves
                    temporary = column - 1;
                    while (temporary >= 0)
                    {
                        if (!chess_board.board_cells[row][temporary].IsPiece())
                        {
                            available_moves.Add(new Tuple<int, int>(row, temporary));
                            temporary--;
                        }
                        else
                        {
                            if (chess_board.board_cells[row][temporary].piece.piece_color != piece_color)
                            {
                                available_moves.Add(new Tuple<int, int>(row, temporary));
                            }
                            break;
                        }
                    }
                }
                // End of Queen

                // King moves
                if (piece_type.Equals(Piece_Type.king))
                {
                    int[] x = { 1, 0, -1, 0, 1, -1, -1, 1 };
                    int[] y = { 0, 1, 0, -1, 1, 1, -1, -1 };
                    for (int index = 0; index < 8; index++)
                    {
                        int new_row = row + x[index];
                        int new_column = column + y[index];
                        if (new_row >= 0 && new_column < 8 && new_row < 8 && new_column >= 0)
                        {
                            if (!chess_board.board_cells[new_row][new_column].IsPiece())
                            {
                                available_moves.Add(new Tuple<int, int>(new_row, new_column));
                            }
                            else
                            {
                                if (chess_board.board_cells[new_row][new_column].piece.piece_color != piece_color)
                                {
                                    available_moves.Add(new Tuple<int, int>(new_row, new_column));
                                }
                            }
                        }
                    }
                }
            }
            else // Piece is on board 1 it moves like a queen
            {
                // South East moves
                int temporary_row = row - 1;
                int temporary_column = column + 1;
                while (temporary_row >= 0 && temporary_column < 8 && !chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                {
                    available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                    temporary_row--;
                    temporary_column++;
                }

                // South West moves
                temporary_row = row - 1;
                temporary_column = column - 1;
                while (temporary_row >= 0 && temporary_column >= 0 && !chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                {
                    available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                    temporary_row--;
                    temporary_column--;
                }

                // North East moves
                temporary_row = row + 1;
                temporary_column = column + 1;
                while (temporary_row < 8 && temporary_column < 8 && !chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                {

                    available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                    temporary_row++;
                    temporary_column++;
                }

                // North West moves
                temporary_row = row + 1;
                temporary_column = column - 1;
                while (temporary_row < 8 && temporary_column >= 0 && !chess_board.board_cells[temporary_row][temporary_column].IsPiece())
                {
                    available_moves.Add(new Tuple<int, int>(temporary_row, temporary_column));
                    temporary_row++;
                    temporary_column--;
                }

                // South available moves
                int temporary = row + 1;
                while (temporary < 8 && !chess_board.board_cells[temporary][column].IsPiece())
                {

                    available_moves.Add(new Tuple<int, int>(temporary, column));
                    temporary++;
                }

                // North available moves
                temporary = row - 1;
                while (temporary >= 0 && !chess_board.board_cells[temporary][column].IsPiece())
                {
                    available_moves.Add(new Tuple<int, int>(temporary, column));
                    temporary--;
                }

                // East available moves
                temporary = column + 1;
                while (temporary < 8 && !chess_board.board_cells[row][temporary].IsPiece())
                {
                    available_moves.Add(new Tuple<int, int>(row, temporary));
                    temporary++;
                }

                // West available moves
                temporary = column - 1;
                while (temporary >= 0 && !chess_board.board_cells[row][temporary].IsPiece())
                {
                    available_moves.Add(new Tuple<int, int>(row, temporary));
                    temporary--;
                }
            }
            if (show_moves) // If flag to highlight the moves is true
            {
                if (board_number == 1) // If board is 1
                {
                    ShowAvailableMoveTransfer();
                }
                if (board_number == 0 || board_number == 2) // If board is 0 or 2
                {
                    ShowAvailableMoveTransferBoard1();
                }
                foreach (var cell_on_board in available_moves) // Iterate through moves and highlight them
                {
                    chess_board.board_cells[cell_on_board.Item1][cell_on_board.Item2].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                }
            }
        }

        public void ShowAvailableMoveTransferBoard1()
        {
            for (int index = 0; index < available_moves.Count; index++) // Iterate through available moves
            {
                if(Game_Logic.board[1].board_cells[available_moves[index].Item1][available_moves[index].Item2].IsPiece()) // If there is a piece on board 1 at legal move coord
                {
                    available_moves.RemoveAt(index); // Remove this legal move
                }
            } 
        }

        public void ShowAvailableMoveTransfer()
        {
            bool first_board = false; // Bool that indicates if the move is legal on board 0
            bool second_board = false; // Bool that indicates if the move is legal on board 2
            for (int index = 0; index < available_moves.Count; index++) // Iterate through available moves
            {
                if (!Game_Logic.board[0].board_cells[available_moves[index].Item1][available_moves[index].Item2].IsPiece()) // If there is no piece on board 0
                {
                    first_board = true; // Move is legal
                }
                if (!Game_Logic.board[2].board_cells[available_moves[index].Item1][available_moves[index].Item2].IsPiece()) // If there is no piece on board 2
                {
                    second_board = true; // Move is legal
                }
                if (!first_board && !second_board) // If move is illegal on board 0 and 2
                {
                    available_moves.RemoveAt(index); // Remove it from legal moves
                    index--;
                }
                if (first_board && !second_board) // If the move is legal on board 0
                {
                    Game_Logic.available_moves_board_0.Add(available_moves[index]); // Add it in to its own Tuple
                }
                if (!first_board && second_board) // If the move is legal on board 2
                {
                    Game_Logic.available_moves_board_2.Add(available_moves[index]); // Add it in to its own Tuple
                }
                first_board = false;
                second_board = false; // Reset the flag
            }
        }
    }
}
