using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Parallel_Worlds
{
    public partial class Promovation : Form
    {
        int board_number;
        int offset;
        int index;
        int new_row;
        int new_column;
        List<Cell> pieces;
        public Promovation(int board_number, int new_row, int new_column)
        {
            InitializeComponent();
            Width = 4 * 68;
            Height = 100;
            this.board_number = board_number;
            pieces = new List<Cell>();
            offset = 0;
            index = 0;
            this.new_row = new_row;
            this.new_column = new_column;
            CenterToScreen();
        }

        public void ShowPromovation(Piece_Color color)
        {
            if (color.Equals(Piece_Color.white))
            {
                while (index < 4)
                {
                    Cell piece_to_show = new Cell();
                    if (index.Equals(0))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.rook, Piece_Color.white, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Rook_white;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    if (index.Equals(1))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.knight, Piece_Color.white, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Knight_white;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    if (index.Equals(2))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.bishop, Piece_Color.white, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Bishop_white;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    if (index.Equals(3))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.queen, Piece_Color.white, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Queen_white;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    index++;
                }
            }
            else
            {
                while (index < 4)
                {
                    Cell piece_to_show = new Cell();
                    if (index.Equals(0))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.rook, Piece_Color.black, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Rook_black;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    if (index.Equals(1))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.knight, Piece_Color.black, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Knight_black;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    if (index.Equals(2))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.bishop, Piece_Color.black, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Bishop_black;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    if (index.Equals(3))
                    {
                        piece_to_show.piece = new Piece(Piece_Type.queen, Piece_Color.black, new_row, new_column, board_number);
                        piece_to_show.Image = Properties.Resources.Queen_black;
                        piece_to_show.Location = new Point(offset, 0);
                        piece_to_show.Size = new Size(64, 64);
                        pieces.Add(piece_to_show);
                        offset = offset + 64;
                    }
                    index++;
                }
            }
            foreach(var element in pieces)
            {
                Controls.Add(element);
                element.Click += ClickPiece;
            }
        }
        
        public void ClickPiece(object sender, EventArgs e)
        {
            if(((Cell)sender).piece.piece_type.Equals(Piece_Type.rook) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.white))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Rook_white;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.rook) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.black))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Rook_black;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.knight) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.white))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Knight_white;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.knight) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.black))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Knight_black;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.bishop) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.white))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Bishop_white;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.bishop) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.black))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Bishop_black;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.queen) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.white))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Queen_white;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            if (((Cell)sender).piece.piece_type.Equals(Piece_Type.queen) && ((Cell)sender).piece.piece_color.Equals(Piece_Color.black))
            {
                Game_Logic.selected_piece = new Piece(((Cell)sender).piece.piece_type, ((Cell)sender).piece.piece_color, new_row, new_column, board_number);
                Game_Logic.board[board_number].board_cells[new_row][new_column].piece = Game_Logic.selected_piece;
                Game_Logic.board[board_number].board_cells[new_row][new_column].Image = Properties.Resources.Queen_black;
                Game_Logic.board[board_number].chess_board.Controls.Add(Game_Logic.board[board_number].board_cells[new_row][new_column]);
            }
            Game_Logic.board[board_number].board_cells[new_row][new_column].BorderStyle = BorderStyle.None;
            Close();
            
        }
    }
}
