using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.ViewModel
{
    public sealed class SingletonViewModelBase : INotifyPropertyChanged
    {
        #region Singleton

        private static readonly SingletonViewModelBase instance = new SingletonViewModelBase();

        private SingletonViewModelBase() { }

        public static SingletonViewModelBase Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

}
