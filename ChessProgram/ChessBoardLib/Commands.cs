using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ChessBoardVisualLib.Commands
{
    public static class Commands
    {
        public static RoutedCommand PromotePawnCommand = new RoutedCommand("PromotePawnCommand", typeof(Commands));
    }
}
