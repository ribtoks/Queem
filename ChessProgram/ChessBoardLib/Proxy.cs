using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace ChessBoardVisualLib
{
    public class Proxy : FrameworkElement
    {
        public static readonly DependencyProperty InProperty;
        public static readonly DependencyProperty OutProperty;

        public Proxy()
        {
            Visibility = Visibility.Collapsed;
        }

        static Proxy()
        {
            var inMetadata = new FrameworkPropertyMetadata(
              delegate(DependencyObject p, DependencyPropertyChangedEventArgs args)
              {
                  if (null != BindingOperations.GetBinding(p, OutProperty))
                      (p as Proxy).Out = args.NewValue;
              });

            inMetadata.BindsTwoWayByDefault = false;
            inMetadata.DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            InProperty = DependencyProperty.Register("In",
                                                     typeof(object),
                                                     typeof(Proxy),
                                                     inMetadata);

            var outMetadata = new FrameworkPropertyMetadata(
              delegate(DependencyObject p, DependencyPropertyChangedEventArgs args)
              {
                  ValueSource source = DependencyPropertyHelper.GetValueSource(p, args.Property);

                  if (source.BaseValueSource != BaseValueSource.Local)
                  {
                      Proxy proxy = p as Proxy;
                      object expected = proxy.In;
                      if (!ReferenceEquals(args.NewValue, expected))
                      {
                          Dispatcher.CurrentDispatcher.BeginInvoke(
                            DispatcherPriority.DataBind, new Action(delegate
                                                                      {
                                                                          proxy.Out = proxy.In;
                                                                      }));
                      }
                  }
              });

            outMetadata.BindsTwoWayByDefault = true;
            outMetadata.DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            OutProperty = DependencyProperty.Register("Out", typeof(object), typeof(Proxy), outMetadata);
        }

        public object In
        {
            get { return GetValue(InProperty); }
            set { SetValue(InProperty, value); }
        }

        public object Out
        {
            get { return GetValue(OutProperty); }
            set { SetValue(OutProperty, value); }
        }
    }
}
