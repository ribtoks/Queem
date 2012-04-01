using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;
using System.Collections.ObjectModel;

namespace ChessBoardVisualLib.ViewModel
{
    public class PawnPromotionViewModel : ObservableObject
    {
        private Figure[] promotionFigures = 
            new Figure[] { Figure.Knight, Figure.Bishop, Figure.Rook, Figure.Queen };

        private ObservableCollection<PawnPromotionItem> promotionItems;

        public PawnPromotionViewModel()
        {

        }

        public ObservableCollection<PawnPromotionItem> Items
        {
            get { return this.promotionItems; }
        }
    }
}
