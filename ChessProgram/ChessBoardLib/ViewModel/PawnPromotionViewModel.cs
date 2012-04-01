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
            this.promotionItems = new ObservableCollection<PawnPromotionItem>();
            foreach (var figure in this.promotionFigures)
                this.promotionItems.Add(new PawnPromotionItem(figure, Color.White));
        }

        public ObservableCollection<PawnPromotionItem> Items
        {
            get { return this.promotionItems; }
        }

        public void SetColor(Color color)
        {
            foreach (var item in this.promotionItems)
                item.UpdateChessFigure(item.FigureType, color);
        }
    }
}
