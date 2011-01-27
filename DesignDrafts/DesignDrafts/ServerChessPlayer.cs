using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DesignDrafts
{
    public enum PlayerGender { Male, Female }
    public enum PlayerAgeType { Kid, Young, Adult }

    public class ServerChessPlayer //: INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public ServerChessPlayer()
        {
        }

        public PlayerGender Gender
        { get; set; }

        public PlayerAgeType AgeType
        { get; set; }

        public string Name
        { get; set; }

        public string Country
        { get; set; }

        public int Age
        { get; set; }

        public string IP
        { get; set; }
    }

    public class PlayerCollection : ObservableCollection<ServerChessPlayer>
    {
    }
}
