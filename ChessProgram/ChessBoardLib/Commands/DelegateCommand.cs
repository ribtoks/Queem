using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ChessBoardVisualLib.Commands
{
    public class DelegateCommand<T> : ICommand
    {
        protected DelegateCommand()
        {
        }
        
        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        { }

        public DelegateCommand(Action<T> execute, Func<T, Boolean> canExecute)
        {
            m_Execute = execute;
            m_CanExecute = canExecute;
        }

        public void Execute(Object parameter)
        {
            if (m_Execute != null)
            {
                m_Execute((T)parameter);
            }
        }

        public Boolean CanExecute(Object parameter)
        {
            return m_CanExecute != null ? m_CanExecute((T)parameter) : true;
        }
          
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }      

        protected Action<T> m_Execute;
        protected Func<T, Boolean> m_CanExecute;
        private List<WeakReference> m_CanExecuteChangedHandlers;
    }
}
