using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace DesignDrafts
{
    public class PlayersLoadedEventArgs : EventArgs
    {
        public IEnumerable<ServerChessPlayer> Collection
        { get; set; }

        public Exception Error
        { get; set; }
    }

    public class PlayersXMLFile
    {
        private XDocument document;
        
        public const string PlayersFileName = "players.xml";
        public const string PlayersFileFolder = "etc";

        public event EventHandler Loaded;

        private void OnPlayersLoaded(IEnumerable<ServerChessPlayer> xmlPlayers,
            Exception lastError)
        {
            // if someone is subscribed to 
            // this event than raise it
            if (Loaded != null)
            {
                var args = new PlayersLoadedEventArgs();
                args.Collection = xmlPlayers;
                args.Error = lastError;
                Loaded(this, args);
            }
        }

        /// <summary>
        /// Loads players from file
        /// </summary>
        public void LoadPlayersFile()
        {
            // TODO make this code async!
            IEnumerable<ServerChessPlayer> output = null;
            Exception lastError = null;

            try
            {
                string path = string.Format(".{1}{0}{1}{2}", PlayersFileFolder, Path.DirectorySeparatorChar, PlayersFileName);
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                document = XDocument.Load(stream);
                var descendants = document.Descendants("player");

                output = from playersInfo in descendants
                         select new ServerChessPlayer()
                         {
                             Age = (int)playersInfo.Attribute("age"),
                             AgeType = (PlayerAgeType)Enum.Parse(typeof(PlayerAgeType), (string)playersInfo.Attribute("agetype")),
                             Country = (string)playersInfo.Attribute("country"),
                             Gender = (PlayerGender)Enum.Parse(typeof(PlayerGender), (string)playersInfo.Attribute("gender")),
                             IP = (string)playersInfo.Attribute("ip"),
                             Name = (string)playersInfo.Attribute("name")
                         };

                stream.Close();
            }
            catch (Exception ex)
            {
                lastError = ex;
            }

            // raise event
            OnPlayersLoaded(output, lastError);
        }
    }
}
