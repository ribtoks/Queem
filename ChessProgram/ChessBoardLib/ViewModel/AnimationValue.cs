using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessBoardVisualLib.ViewModel
{
    public class AnimationValue : ObservableObject
    {
        private double toValue;
        public double ToValue
        {
            get { return this.toValue; }
            set
            {
                if (this.toValue != value)
                {
                    this.toValue = value;
                    OnPropertyChanged("ToValue");
                }
            }
        }
    }
}
