using System;
using System.Net;
using System.Text;
using System.IO;

namespace MonsterTradingCardGame
{

    public class Server
    {
        //-----------Other--Classes-----------
        private UserManagement userManagement;

        //----------Actual---Server---Code----------
        private HttpListener listener; //class that listens to HTTP requests
        private readonly string serverUrl; // stores the Url that the server will listen to (readonly damit es nie verändert wird)
        private volatile bool _isRunning = true; // Flag to control the loop

        public Server(string url) //server constructor initialise den Server 
        { 
            serverUrl = url; // assings the value of 'url' to a member variable so we can access it later (stores server url)
            listener = new HttpListener(); // Iniates the listener object
            listener.Prefixes.Add(url); // tells the listener that this is the adress that the server will respond to 

            //-----------Other--Classes-----------
            userManagement = new UserManagement();
        }

        //--------Server--Starting--and--Stopping--------

        public void Start() // methode to start the Server
        { 
            listener.Start();// listener is allowed to start accepting requests
            Console.WriteLine($"Server started at {serverUrl}");// shows that the server is running in the console

            Thread serverThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    HttpListenerContext context = listener.GetContext();// stops the threat until a request is recived (cuz unnecessary CPU use)
                    HandleRequest(context);// sends requests to the handler
                }
            });

            serverThread.Start(); //started den server thread
            while (true)
            {
                string input = Console.ReadLine();
                if(input == "exit")
                {
                    _isRunning = false;
                    listener.Stop();
                    Console.WriteLine("Server stopped! xoxo");
                    break;
                }
            }

        }

        //-----------Request--Handler-----------

        private void HandleRequest(HttpListenerContext context)// methode to process requests
        {
            HttpListenerRequest request = context.Request;// recives the request/response from the context
            HttpListenerResponse response = context.Response;

            string username = null;
            string password = null;

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(request.InputStream, request.ContentEncoding);
                string requestBody = reader.ReadToEnd();
                var requestData = System.Web.HttpUtility.ParseQueryString(requestBody);
                username = requestData["username"];
                password = requestData["password"];
            }
            finally
            {
               reader?.Close();
            }

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                BadReq(response);
                return;
            }

            switch (request.HttpMethod)//switch case für routing, if else war sehr messy, curl script wird hier auch beachtet (obv)
            {
                case "POST": //für die methods die was am server 'ändern'
                    if(request.Url.AbsolutePath == "/register") // liest welche rout angegeben wurde
                    {
                        userManagement.Register(username, password, response); //routet weiter an die richtige function 
                    }
                    else if (request.Url.AbsolutePath == "/session")
                    {
                        userManagement.Login(username, password, response);
                    }
                    else if (request.Url.AbsolutePath == "/battle")
                    {
                        Battle(request, response);
                    }
                    else if (request.Url.AbsolutePath == "/trade")
                    {
                        Trade(request, response);
                    }
                    else
                    {
                        NotFound(response);//function zur anzeige von notfound error code
                    }
                    break;

                case "GET": //für methodes die am server nur 'lesen' möchten (d.h. no changes)
                    if (request.Url.AbsolutePath == "/stack")
                    {
                        GetStack(request, response);
                    }
                    else if (request.Url.AbsolutePath == "/deck")
                    {
                        GetDeck(request, response);
                    }
                    else
                    {
                        NotFound(response);//function zur anzeige von notfound error code
                    }
                    break;
                default:
                    BadReq(response);//function zur anzeige von BadRequest error code
                    break;
            }

            Console.WriteLine($"Recieved request: {request.HttpMethod} {request.Url}"); // logging for debugging uses
            response.OutputStream.Close();
        }

        //-------------Error--Codes-------------

        private void NotFound(HttpListenerResponse response) //route not found (404 error)
        {
            response.StatusCode = 404; // wenn die route nicht gefunden wird
            byte[] buffer = Encoding.UTF8.GetBytes("Route not found :(((");//byte array um dem server die nachricht im richtigen format schicken zu können
            response.OutputStream.Write(buffer, 0, buffer.Length);//schreibt den umgewandelten byte array in die response rein
        }
        private void BadReq(HttpListenerResponse response) // bad request (400 error)
        {
            response.StatusCode = 400;
            byte[] buffer = Encoding.UTF8.GetBytes("Bad Request: Wrong Http methode :/");
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        //---------------------User-data-Get---------------------

        public void GetStack(HttpListenerRequest request, HttpListenerResponse response)
        {
            response.StatusCode = 200; // code for "OK"
            byte[] buffer = Encoding.UTF8.GetBytes("user getting theis stacks ? pls work");
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        public void GetDeck(HttpListenerRequest request, HttpListenerResponse response)
        {
            response.StatusCode = 200; // code for "OK"
            byte[] buffer = Encoding.UTF8.GetBytes("user trying to get deck...pleaseeeee");
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        //---------------------Game----Logic---------------------

        public void Battle(HttpListenerRequest request, HttpListenerResponse response)
        {
            response.StatusCode = 200; // code for "OK"
            byte[] buffer = Encoding.UTF8.GetBytes("TRYNA BATTLE !!!");
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        public void Trade(HttpListenerRequest request, HttpListenerResponse response)
        {
            response.StatusCode = 200; // code for "OK"
            byte[] buffer = Encoding.UTF8.GetBytes("Tradingggg.............");
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}

/*
CurlScripts,

Login:
curl -X POST http://localhost:10001/session ^
-H "Content-Type: application/x-www-form-urlencoded" ^
-d "username=testuser&password=testpass"


Registration:
curl -X POST http://localhost:10001/register ^
-H "Content-Type: application/x-www-form-urlencoded" ^
-d "username=testuser"


*/