namespace SocketActions;

internal interface ISocketActions
{
    Task ConnectAsync();
    Task JoinRoom(string room);
    Task LeaveRoom(string room);
    Task SendMessage(string room, string msg);
    Task BroadcastToRoom(string room, string msg);
    Task BroadcastToAll(string message);
    Task DisconnectAsync();
    void OnMessageReceived(Action<string> handler);
    void OnBroadcastReceived(Action<string> callback);
    Task SendTyping(string room, string username);
    void OnTypingReceived(Action<string> callback);
}
