using Unity.Netcode;

/// <summary>
/// basicly a <see cref="NetworkVariable{T}"/> where client can write to it
/// </summary>
/// <typeparam name="T"></typeparam>
public class ClientNetworkVariable<T> : NetworkVariable<T>
{
    public ClientNetworkVariable(T _value)
        : base(_value, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner)
    {
        // nothing needed here
    }
}
