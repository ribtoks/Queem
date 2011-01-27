using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignDrafts
{
    /// <summary>
    /// Interaction logic for PlayerSelectControl.xaml
    /// </summary>
    public partial class PlayerSelectControl : UserControl
    {
        private double scrollOffset = 0.0;

        public PlayerSelectControl()
        {
            this.InitializeComponent();
        }

        private static void OnItemsSourceChanged(DependencyObject d,
                                                  DependencyPropertyChangedEventArgs args)
        {
            PlayerSelectControl sender = d as PlayerSelectControl;
            sender.PlayerItemsControl.ItemsSource = (args.NewValue as PlayerCollection);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource",
                                         typeof(PlayerCollection),
                                         typeof(PlayerSelectControl),
                                         new PropertyMetadata(new PropertyChangedCallback(OnItemsSourceChanged)));

        public PlayerCollection ItemsSource
        {
            get { return (PlayerCollection)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ServerChessPlayer),
                                         typeof(PlayerSelectControl),
                                         new PropertyMetadata(null));

        public ServerChessPlayer SelectedItem
        {
            get { return (ServerChessPlayer)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public event SelectionChangedEventHandler SelectionChanged;

        private void OnSelectionChanged(FrameworkElement sender, ServerChessPlayer item)
        {
            if (SelectionChanged != null)
            {
                object[] addedItems = new object[1];
                addedItems[0] = sender;
                object[] removedItems = new object[0];
                SelectionChangedEventArgs args = new SelectionChangedEventArgs(new RoutedEventArgs().RoutedEvent, removedItems, addedItems);
                SelectionChanged(this, args);
            }

            SelectedItem = item;
        }

        private void ScrollButtonUp_Click(object sender, RoutedEventArgs e)
        {
            ScrollButtonDown.IsEnabled = true;

            scrollOffset -= PlayerScroller.ActualWidth / 10;
            if (scrollOffset <= 0)
            {
                scrollOffset = 0;
                ScrollButtonUp.IsEnabled = false;
            }
            PlayerScroller.ScrollToVerticalOffset(scrollOffset);
        }

        private void ScrollButtonDown_Click(object sender, RoutedEventArgs e)
        {
            ScrollButtonUp.IsEnabled = true;

            scrollOffset += PlayerScroller.ActualHeight / 10;
            if (scrollOffset >= PlayerScroller.ScrollableHeight)
            {
                scrollOffset = PlayerScroller.ScrollableHeight;
                ScrollButtonDown.IsEnabled = false;
            }

            PlayerScroller.ScrollToVerticalOffset(scrollOffset);
        }
    }
}
