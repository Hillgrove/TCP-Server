
using System.Net;
using System.Net.Sockets;

Console.WriteLine("TCP Server");

// Listens for incomming connections
TcpListener listener = new TcpListener(IPAddress.Any, 7);

// Starting server
listener.Start();

while (true)
{
    // Establish connection/socket
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient(socket)); // TODO: Not awaited
}

// Stopping server
listener.Stop();  // TODO: Unreachable code


void HandleClient(TcpClient socket)
{
    // Streams for reading and writing to the connection/socket
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    Console.WriteLine("Debug: Client connected.");
    writer.WriteLine("You're now connected to the TCP Server.");
    writer.Flush();

    while (socket.Connected)
    {
        // Reading what the client sends
        string? message = reader.ReadLine();
        Console.WriteLine($"Debug: Client sent: {message}"); // for debugging purposes

        // Client has disconnected
        if (message == null) 
        {
            Console.WriteLine($"Debug: Client lost connection!"); // for debugging purposes
            socket.Close();
            break;
        }

        // Client wants to stop the connection
        if (message == "stop")
        {
            Console.WriteLine($"Debug: Client stopped connection!"); // for debugging purposes
            writer.WriteLine("Closing down connection!");
            writer.Flush();
            socket.Close();
            break;
        }
    }

    // Close connection/socket and stop listener
    socket.Close();
}