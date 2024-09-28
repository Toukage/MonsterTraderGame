using System;
using System.Net;
using System.Text;
using System.IO;

namespace MonsterTradingCardGame
{

    public class Server
    {
        private HttpListener _listener; //class that listens to HTTP requests
        private readonly string _url; // stores the Url that the server will listen to (readonly damit es nie verändert wird)

        public Server(string url) 
        { 
            _url = url; // assings the value of 'url' to a member variable so we can access it later 
            _listener = new HttpListener(); // Iniates the listener object
            _listener.Prefixes.Add(url); // tells the listener that this is the adress that the server will respond to 
        }

        public void Start() // methode to start the Server
        { 
            _listener.Start();// listener is allowed to start accepting requests
            Console.WriteLine($"Server started at {_url}");// shows that the server is running in the console
            Console.WriteLine("Press Enter to stop the server..."); //message to show how to stop the server

            while (true)
            {
                HttpListenerContext context = _listener.GetContext();// stops the threat until a request is recived (cuz unnecessary CPU use)
                HandleRequest(context);// sends requests to the handler
            }
        
        }

        private void HandleRequest(HttpListenerContext context)// methode to process requests
        {
            HttpListenerRequest request = context.Request;// recives the request from the context
            Console.WriteLine($"Receuved request: {request.HttpMethod} {request.Url}"); // logging for debugging uses

            HttpListenerResponse response = context.Response;// gets the response to send back to client
            string responseString = "<html><body>Hello, Rigby</body></html>"; // html response that will be sent back to client (you can see this in your browser under http://localhost:10001/)

            byte[] buffer = Encoding.UTF8.GetBytes( responseString );// converts the html string into an array so it can be send over the network
            response.ContentLength64 = buffer.Length;// so client know how much data to expect 

            using (Stream output = response.OutputStream)// server wirtes response that will be sent back with outputStream (used for sending data over HTTP)
            {
                output.Write( buffer, 0, buffer.Length );// sends said response over to the client
            }
        }

        public void Stop() // methode to stop Server  (not yet in use)
        {
            _listener.Stop(); // stops the listener
            Console.WriteLine("Server stopped."); // message to show thatthe server has stopped
        }

    }
}