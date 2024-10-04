using System;
using System.Net;
using System.Text;
using System.IO;

namespace MonsterTradingCardGame
{

    public class Server
    {
        private HttpListener _listener; //class that listens to HTTP requests
        private readonly string serverUrl; // stores the Url that the server will listen to (readonly damit es nie verändert wird)
        private volatile bool _isRunning = true; // Flag to control the loop


        public Server(string url) //server constructor initialise den Server 
        { 
            serverUrl = url; // assings the value of 'url' to a member variable so we can access it later 
            _listener = new HttpListener(); // Iniates the listener object
            _listener.Prefixes.Add(url); // tells the listener that this is the adress that the server will respond to 
        }

        public void Start() // methode to start the Server
        { 
            _listener.Start();// listener is allowed to start accepting requests
            Console.WriteLine($"Server started at {_serverUrl}");// shows that the server is running in the console
            Console.WriteLine("Press Enter to stop the server..."); //message to show how to stop the server


            // Start a thread to listen for key press to stop the server
            Thread keyListener = new Thread(() =>
            {
                Console.ReadLine(); // Wait for Enter key
                _isRunning = false; // Set the flag to false
            });

            keyListener.Start(); // Start the thread

            while (_isRunning)
            {
                HttpListenerContext context = _listener.GetContext();// stops the threat until a request is recived (cuz unnecessary CPU use)
                HandleRequest(context);// sends requests to the handler
            }

            // Cleanup after exiting the loop
            _listener.Stop(); // Stop the listener
            Console.WriteLine("Server stopped.");


        }

        private void HandleRequest(HttpListenerContext context)// methode to process requests
        {
            HttpListenerRequest request = context.Request;// recives the request from the context
            HttpListenerResponse response = context.Response;

            if(request.HttpMethod == "POST")
            {
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))//decodes and reads
                {
                    if(request.Url.AbsolutePath == "/register")// checks if user wants to register
                    {
                        Console.WriteLine($"User wants to register");//sends message to terminal to show the users action
                        string requestMessage = reader.ReadToEnd();//reads the entire message
                        var formData = System.Web.HttpUtility.ParseQueryString(requestMessage);//converts the string into a data format
                        string username = formData["username"];//sets the username to be the same as the item "username" from the client request
                        Console.WriteLine($"Received username: {username}");//sends message to terminal to show that the username was recieved correctly
                    }
                    else if(request.Url.AbsolutePath == "/sessions")//checks if user wants to login
                    {
                        Console.WriteLine($"User wants to login");//sends message to terminal to show the users action
                        string requestMessage = reader.ReadToEnd();
                        var formData = System.Web.HttpUtility.ParseQueryString(requestMessage);
                        string username = formData["username"];
                        string password = formData["password"];
                        Console.WriteLine($"Received username: {username}");
                        Console.WriteLine($"Received password: {password}");

                    }
                }
            }

            Console.WriteLine($"Receuved request: {request.HttpMethod} {request.Url}"); // logging for debugging uses

            HttpListenerResponse clientResponse = context.Response;// gets the response to send back to client
            string responseString = "<html><body>Hello, Rigby</body></html>"; // html response that will be sent back to client (you can see this in your browser under http://localhost:10001/)

            byte[] buffer = Encoding.UTF8.GetBytes( responseString );// converts the html string into an array so it can be send over the network
            response.ContentLength64 = buffer.Length;// so client know how much data to expect 

            using (Stream output = clientResponse.OutputStream)// server wirtes response that will be sent back with outputStream (used for sending data over HTTP)
            {
                output.Write( buffer, 0, buffer.Length );// sends said response over to the client
            }
        }

        public void Stop() // methode to stop Server  (not yet in use)
        {
            _listener.Stop(); // stops the listener
            Console.WriteLine("Server stopped."); // message to show thatthe server has stopped
        }


        //-------------------------------------------------------REGISTRATION--UND--LOGIN-----------------------------------------------------------------------

        /*public bool Register(HTTPlistenerContext contexts)// use  here
        {

        }*/

    }
}

/*
CurlScripts,

Login:
curl -X POST http://localhost:10001/sessions ^
-H "Content-Type: application/x-www-form-urlencoded" ^
-d "username=testuser&password=testpass"


Registration:
curl -X POST http://localhost:10001/register ^
-H "Content-Type: application/x-www-form-urlencoded" ^
-d "username=testuser"


*/