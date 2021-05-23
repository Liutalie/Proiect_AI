using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Parallel_Worlds
{
    class Chess_Board
    {
        public Panel chess_board;
        public Cell[][] board_cells; 

        public Chess_Board(int board_number)
        {
            chess_board = new Panel();
            board_cells = new Cell[8][];
            chess_board.BackgroundImage = Properties.Resources.Chess_board; // Load chess board img
            chess_board.Location = new Point(board_number * 650, 0);
            chess_board.Size = new Size(512, 512);
            chess_board.BackColor = Color.Transparent;
            chess_board.Visible = true;
            if (board_number == 1) // if board is 2 init all 
            {
                for (int row = 0; row < 8; row++)
                {
                    board_cells[row] = new Cell[8];
                    for (int column = 0; column < 8; column++)
                    {
                        board_cells[row][column] = new Cell();
                        board_cells[row][column].Location = new Point(column * 64, row * 64);
                        board_cells[row][column].Size = new Size(64, 64);
                        board_cells[row][column].Visible = true;
                        board_cells[row][column].BackColor = Color.Transparent;
                        board_cells[row][column].piece = null;
                        chess_board.Controls.Add(board_cells[row][column]);
                    }
                }
            }
            if (board_number == 0 || board_number == 2) // If board is 1 or 3
            {
                for (int row = 0; row < 8; row++)
                {
                    board_cells[row] = new Cell[8]; // Init rows
                    for (int column = 0; column < 8; column++)
                    {
                        bool flag = true;
                        board_cells[row][column] = new Cell(); // Init every col from row
                        board_cells[row][column].Location = new Point(column * 64, row * 64);
                        board_cells[row][column].Size = new Size(64, 64);
                        board_cells[row][column].Visible = true;
                        board_cells[row][column].BackColor = Color.Transparent;
                        if (row == 1) // If row = 1 populate it with black pawns
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.pawn, Piece_Color.black, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Pawn_black;
                            flag = false;
                        }
                        if ((row == 0 && column == 0) || (row == 0 && column == 7)) // Adding black rooks at their starting position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.rook, Piece_Color.black, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Rook_black;
                            flag = false;
                        }
                        if ((row == 0 && column == 1) || (row == 0 && column == 6)) // Adding black knights at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.knight, Piece_Color.black, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Knight_black;
                            flag = false;
                        }
                        if ((row == 0 && column == 2) || (row == 0 && column == 5)) // Adding black bishops at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.bishop, Piece_Color.black, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Bishop_black;
                            flag = false;
                        }
                        if ((row == 0 && column == 3)) // Adding black queen at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.queen, Piece_Color.black, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Queen_black;
                            flag = false;
                        }
                        if ((row == 0 && column == 4)) // Adding black king at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.king, Piece_Color.black, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.King_black;
                            flag = false;
                        }
                        if (row == 6) // If row = 6 populate it with white pawns
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.pawn, Piece_Color.white, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Pawn_white;
                            flag = false;
                        }
                        if ((row == 7 && column == 0) || (row == 7 && column == 7)) // Adding white rooks at their starting position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.rook, Piece_Color.white, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Rook_white;
                            flag = false;
                        }
                        if ((row == 7 && column == 1) || (row == 7 && column == 6)) // Adding white knights at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.knight, Piece_Color.white, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Knight_white;
                            flag = false;
                        }
                        if ((row == 7 && column == 2) || (row == 7 && column == 5)) // Adding white bishops at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.bishop, Piece_Color.white, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Bishop_white;
                            flag = false;
                        }
                        if ((row == 7 && column == 3)) // Adding white queen at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.queen, Piece_Color.white, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.Queen_white;
                            flag = false;
                        }
                        if ((row == 7 && column == 4)) // Adding white king at their position
                        {
                            board_cells[row][column].piece = new Piece(Piece_Type.king, Piece_Color.white, row, column, board_number);
                            board_cells[row][column].Image = Properties.Resources.King_white;
                            flag = false;
                        }
                        if(flag)
                        {
                            board_cells[row][column].piece = null;
                        }
                        chess_board.Controls.Add(board_cells[row][column]);
                    }
                }
            }

        }
    }
}

