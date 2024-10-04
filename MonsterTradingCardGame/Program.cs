using MonsterTradingCardGame;

class Program 
{
    static void Main(string[] args)
    {
        Server server = new Server("http://localhost:10001/"); // Passes the base URL with a port
        server.Start(); // Start the server
    }
}

