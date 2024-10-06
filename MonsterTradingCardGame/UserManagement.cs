using Npgsql;
using System;
using System.Net;
using System.Text;



public class UserManagement
{
    private Database database;

    public UserManagement()
    {
        database = new Database();
    }


    //----------------------GET--USER--DATA----------------------

    public bool GetUser(string username, string password)
    {
        using (var connection = Database.Connection())
        {
            using (var command = new NpgsqlCommand("SELECT * FROM Users WHERE username=@username AND password=@password", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {                       
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool InsertUser(string username, string password)
    {
        using (var connection = Database.Connection())
        {
            using (var command = new NpgsqlCommand("INSERT INTO Users (username, password) VALUES (@username, @password)", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Problem making new user : {ex.Message}");
                    return false;
                }
                
            }
        }
        return false;
    }
    //----------------------EDIT--USER--DATA----------------------

    /*public void UpdateUser(username) { }
    public void DeleteUser(username) { }*/

    //----------------------REGISTRATION--UND--LOGIN----------------------


    public void Login(string username, string password, HttpListenerResponse response)
    {
        response.StatusCode = 200; // code for "OK"
        byte[] buffer = Encoding.UTF8.GetBytes("arrived at Login lets GOOOOOOOOOOOOOO RIGBY");
        response.OutputStream.Write(buffer, 0, buffer.Length);
        bool Valid = GetUser(username, password);
        if (Valid)
        {
            response.StatusCode = 200;
            byte[] loginBuffer = Encoding.UTF8.GetBytes("Login works ;))))");
            response.OutputStream.Write(loginBuffer, 0, loginBuffer.Length);
        }
        else
        {
            response.StatusCode = 401; // code for "Unauthorized"
            byte[] loginBuffer = Encoding.UTF8.GetBytes("Wrong Username or Password, WOMP WOMP");
            response.OutputStream.Write(loginBuffer, 0, loginBuffer.Length);
        }

    }

    public void Register(string username, string password, HttpListenerResponse response)
    {
        response.StatusCode = 200; // code for "OK"
        byte[] buffer = Encoding.UTF8.GetBytes("arrived at Registration. xxx ");
        response.OutputStream.Write(buffer, 0, buffer.Length);
        bool Valid = InsertUser(username, password);
        if (Valid)
        {
            response.StatusCode = 200;
            byte[] registerBuffer = Encoding.UTF8.GetBytes("Registered!");
            response.OutputStream.Write(registerBuffer, 0, registerBuffer.Length);
        }
        else
        {
            response.StatusCode = 401; // code for "Unauthorized"
            byte[] registerBuffer = Encoding.UTF8.GetBytes("Couldnt register:(");
            response.OutputStream.Write(registerBuffer, 0, registerBuffer.Length);
        }
    }



}

//at some point mach ich dann jede SQL usage in eine eigene function zum reusen, für jetzt passt es so