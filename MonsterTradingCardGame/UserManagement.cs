using System;
using System.Net;
using System.Text;


public class UserManagement
{
	public UserManagement()
	{
       
    }

    public void Login(HttpListenerRequest request, HttpListenerResponse response)
    {
        response.StatusCode = 200; // code for "OK"
        byte[] buffer = Encoding.UTF8.GetBytes("Login works LETS GO RIGBY");
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}   
