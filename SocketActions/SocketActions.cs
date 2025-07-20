namespace SocketActions;

public class SocketActions : ISocketActions
{
    private readonly SocketIOClient.SocketIO _client;
    public SocketActions(string url)
    {
        _client = new SocketIOClient.SocketIO(url);
        _client.OnConnected += async (sender, e) => Console.WriteLine($"Socket.IO connected with {_client.Id}");

        //_client.On("joinRoom", response => Console.WriteLine("Dot Net Service => Joined room: {0}", response.GetValue<string>()));
        //_client.On("message", response => Console.WriteLine("Dot Net Service => Receiving a message: {0}", response.GetValue<string>()));
        //_client.On("roomMessage", response => Console.WriteLine("Dot Net Service => Receiving a roomMessage: {0}", response.GetValue<string>()));
        //_client.On("notifyAll", response =>  Console.WriteLine("Dot Net Service => Receiving a notifyAll: {0}", response.GetValue<string>()));

        _client.ConnectAsync().Wait();
    }
    public async Task ConnectAsync()
    {
        if (!_client.Connected)
            await _client.ConnectAsync();
    }
    public async Task DisconnectAsync()
      => await _client.DisconnectAsync();
    public async Task JoinRoom(string room)
       => await _client.EmitAsync("joinRoom", room);
    public async Task LeaveRoom(string room)
        => await _client.EmitAsync("leaveRoom", room);
    public Task SendMessage(string room, string msg)
        => _client.EmitAsync("message", new { room, msg });
    public Task BroadcastToRoom(string room, string msg)
        => _client.EmitAsync("roomMessage", new { room, msg });
    public async Task BroadcastToAll(string message)
        => await _client.EmitAsync("broadcastToAll", message);
    public void OnMessageReceived(Action<string> handler)
        => _client.On("message", response =>
        {
            var msg = response.GetValue<string>();
            handler?.Invoke(msg);
        });
    public void OnBroadcastReceived(Action<string> callback)
        => _client.On("message", res =>
        {
            var msg = res.GetValue<string>();
            callback?.Invoke(msg);
        });
    public Task SendTyping(string room, string username)
        => _client.EmitAsync("typing", room, username);
    public void OnTypingReceived(Action<string> callback)
        => _client.On("typing", res =>
        {
            string msg = res.GetValue<string>();
            callback?.Invoke(msg);
        });
}
