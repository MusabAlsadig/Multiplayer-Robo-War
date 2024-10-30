using System;

public struct LanConnectionInfo
{
    public string ipAddress;


    //======= Room Info

    public ushort port;
    public string roomName;
    

    public LanConnectionInfo(string ipAddress,ushort port, string roomName)
    {
        this.ipAddress = ipAddress;
        this.port = port;
        this.roomName = roomName;
    }
    
    
    public static LanConnectionInfo FromString(string data)
    {
        string[] items = data.Split(':');
        return new LanConnectionInfo
        {
            ipAddress = items[0],
            port = Convert.ToUInt16(items[1]),
            roomName = items[2]
        };
    }

    public static implicit operator string (LanConnectionInfo lanConnrctionInfo)
    {
        return lanConnrctionInfo.ToString();
    }

    public override readonly string ToString()
    {
        return $"{ipAddress}:{port}:{roomName}";
    }


}
