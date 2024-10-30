
namespace Data
{
    [System.Serializable]
    public class PlayerInfo
    {
        
        public string name;
        public bool HaveName => name != Defaults.PlayerName;
        public PlayerInfo()
        {
            name = Defaults.PlayerName;
        }

        public PlayerInfo(string _name)
        {
            name = _name;
        }

    }
}