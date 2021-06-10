using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

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
        public static Cell piece_selected;
        int number_of_white_kings;
        int number_of_black_kings;

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
            number_of_black_kings = 0;
            number_of_white_kings = 0;
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
            if(board[former_board].board_cells[old_row][old_column].piece.piece_type.Equals(Piece_Type.king))
            {
                var piece_color = board[former_board].board_cells[old_row][old_column].piece.piece_color;
                if(piece_color.Equals(Piece_Color.black))
                {
                    black_kings_position = new Tuple<int, int>(new_row, new_column);
                }
                else
                {
                    white_kings_position = new Tuple<int, int>(new_row, new_column);
                }
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
            CheckNumberOfKings();
            return null;
        }

        public void CheckNumberOfKings()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    if(board[0].board_cells[row][column].IsPiece())
                    {
                        if(board[0].board_cells[row][column].piece.piece_type.Equals(Piece_Type.king))
                        {
                            if(board[0].board_cells[row][column].piece.piece_color.Equals(Piece_Color.white))
                            {
                                number_of_white_kings++;
                            }
                            else
                            {
                                number_of_black_kings++;
                            }
                        }
                    }
                    if (board[2].board_cells[row][column].IsPiece())
                    {
                        if (board[2].board_cells[row][column].piece.piece_type.Equals(Piece_Type.king))
                        {
                            if (board[2].board_cells[row][column].piece.piece_color.Equals(Piece_Color.white))
                            {
                                number_of_white_kings++;
                            }
                            else
                            {
                                number_of_black_kings++;
                            }
                        }
                    }
                }
            }
            if (number_of_white_kings != 2)
            {
                MessageBox.Show("Black Won!", "Game Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread.Sleep(2000);
                Application.Exit();
            }
            if (number_of_black_kings != 2)
            {
                MessageBox.Show("White Won!", "Game Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread.Sleep(2000);
                Application.Exit();
            }
            number_of_white_kings = 0;
            number_of_black_kings = 0;
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

        public void RandomAI()
        {
            List<int> choose_board = new List<int>();
            choose_board.Add(1);
            choose_board.Add(3);
            var black_pieces_location_board0 = GetPieceLocation(Piece_Color.black, 0);
            var black_pieces_location_board1 = GetPieceLocation(Piece_Color.black, 1);
            var black_pieces_location_board2 = GetPieceLocation(Piece_Color.black, 2);
            Tuple<int, int, int> temporary_list_attack;
            Dictionary<Piece, List<Tuple<int,int, int>>> black_moves = new Dictionary<Piece, List<Tuple<int,int, int>>>();
            Dictionary<Piece, Tuple<int,int, int>> attack_moves = new Dictionary<Piece, Tuple<int, int, int>>();
            foreach(var piece in black_pieces_location_board0)
            {
                Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece.ShowAvailableMoves(Game_Logic.board[0], false, 0);
                if(Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece.available_moves.Count!=0)
                {
                    List<Tuple<int, int, int>> temporary_list_moves = new List<Tuple<int, int, int>>();
                    List<Tuple<int, int>> temporary = new List<Tuple<int, int>>();
                    temporary.AddRange(Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece.available_moves);
                    foreach (var move in temporary)
                    {
                        temporary_list_moves.Add(Tuple.Create(0, move.Item1, move.Item2));
                    }
                    black_moves[Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece] = temporary_list_moves;
                    foreach (var move in Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece.available_moves)
                    {
                        if (Game_Logic.board[0].board_cells[move.Item1][move.Item2].IsPiece())
                        {
                            temporary_list_attack = Tuple.Create(0, move.Item1, move.Item2);
                            attack_moves[Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece] = temporary_list_attack;
                        }
                    }
                    
                }
                Game_Logic.board[0].board_cells[piece.Item1][piece.Item2].piece.available_moves.Clear();
            }
            foreach (var piece in black_pieces_location_board1)
            {
                Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece.ShowAvailableMoves(Game_Logic.board[1], false, 1);
                if (Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece.available_moves.Count != 0)
                {
                    List<Tuple<int, int, int>> temporary_list_moves = new List<Tuple<int, int, int>>();
                    List<Tuple<int, int>> temporary = new List<Tuple<int, int>>();
                    temporary.AddRange(Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece.available_moves);
                    foreach (var move in temporary)
                    {
                        temporary_list_moves.Add(Tuple.Create(1, move.Item1, move.Item2));
                    }
                    black_moves[Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece] = temporary_list_moves;
                    foreach (var move in Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece.available_moves)
                    {
                        if (Game_Logic.board[1].board_cells[move.Item1][move.Item2].IsPiece())
                        {
                            temporary_list_attack = Tuple.Create(1, move.Item1, move.Item2);
                            attack_moves[Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece] = temporary_list_attack;
                        }
                    }
                }
                Game_Logic.board[1].board_cells[piece.Item1][piece.Item2].piece.available_moves.Clear();
            }
            foreach (var piece in black_pieces_location_board2)
            {
                Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece.ShowAvailableMoves(Game_Logic.board[2], false, 2);
                if (Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece.available_moves.Count != 0)
                {
                    List<Tuple<int, int, int>> temporary_list_moves = new List<Tuple<int, int, int>>();
                    List<Tuple<int, int>> temporary = new List<Tuple<int, int>>();
                    temporary.AddRange(Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece.available_moves);
                    foreach (var move in temporary)
                    {
                        temporary_list_moves.Add(Tuple.Create(2, move.Item1, move.Item2));
                    }
                    black_moves[Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece] = temporary_list_moves;
                    foreach (var move in Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece.available_moves)
                    {
                        if (Game_Logic.board[2].board_cells[move.Item1][move.Item2].IsPiece())
                        {
                            temporary_list_attack = Tuple.Create(2, move.Item1, move.Item2);
                            attack_moves[Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece] = temporary_list_attack;
                        }
                    }
                }
                Game_Logic.board[2].board_cells[piece.Item1][piece.Item2].piece.available_moves.Clear();
            }

            Random random_move = new Random();
            Piece key;
            int index;
            int old_row, old_column, old_board, new_index, new_row, new_column;
            Tuple<int, int, int> new_coordonates;
            if(attack_moves.Count != 0)
            {
                index = random_move.Next(attack_moves.Count);
                key = attack_moves.Keys.ElementAt(index);
                old_row = key.row;
                old_column = key.column;
                old_board = key.board;
                new_coordonates = attack_moves[key];
                new_row = new_coordonates.Item2;
                new_column = new_coordonates.Item3;
            }
            else
            {
                index = random_move.Next(black_moves.Count);
                key = black_moves.Keys.ElementAt(index);
                new_index = random_move.Next(black_moves[key].Count);
                old_row = key.row;
                old_column = key.column;
                old_board = key.board;
                new_coordonates = black_moves[key][new_index];
                new_row = new_coordonates.Item2;
                new_column = new_coordonates.Item3;
            }
            if(old_board == 0 || old_board == 2)
            {
                MoveThePiece(1, old_board, old_row, old_column, new_row, new_column);
            }
            else
            {
                MoveThePiece(random_move.Next(choose_board.Count), old_board, old_row, old_column, new_row, new_column);
            }
        }

        public Tuple<bool, bool> CheckForChess(int board_number) // Method that cheks if king is in chess
        {
            bool king_in_chess_white = false; // Flag for white king
            bool king_in_chess_black = false; // Flag for black king
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
