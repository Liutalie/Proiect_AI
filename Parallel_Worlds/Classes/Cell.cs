using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Parallel_Worlds
{
    class Cell : PictureBox
    {
        public Piece piece;

        public Cell()
        {
            if(piece != null)
            {
                piece.column = Location.X / 64;
                piece.row = Location.Y / 64;
            }
        }
        
        public bool IsPiece()
        {
            if(piece != null)
            {
                return true;
            }
            else 
                return false;
        }
    }
}
