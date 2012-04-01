using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ChessBoardVisualLib.Helpers
{
    public static class MyVisualTreeHelper
    {
        public static T FindChild<T>(this FrameworkElement element) where T : FrameworkElement
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);
            var children = new FrameworkElement[childrenCount];

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                children[i] = child;
                if (child is T)
                {
                    return (T) child;
                }
            }

            for (var i = 0; i < childrenCount; i++)
            {
                if (children[i] != null)
                {
                    var subChild = FindChild<T>(children[i]);
                    if (subChild != null)
                    {
                        return subChild;
                    }
                }
            }

            return null;
        }

        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var parentObject = GetParentObject(child);

            if (parentObject == null)
            {
                return null;
            }

            var parent = parentObject as T;

            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        private static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            var contentElement = child as ContentElement;

            if (contentElement != null)
            {
                var parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                var fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            var frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                var parent = frameworkElement.Parent;
                if (parent != null)
                {
                    return parent;
                }
            }

            return VisualTreeHelper.GetParent(child);
        }
    }
}
