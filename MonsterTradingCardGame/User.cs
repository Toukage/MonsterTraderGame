using System;
using System.Collections.Generic;
using System.Xml.Serialization;//library for lists

public class User
{
    //users info
    public string Username 
    { 
        get; 
        private set; 
    }
    public string Password //needs to be hashed 
    { 
        get; 
        private set; 
    }

    public int Coins
    {
        get;
        private set;
    } = 20;//every user starts with 20 coins
    public int ELO
    {
        get;
        private set;
    } = 100; //starting score is 100


    //users cards
    public List<Card> Stack 
    { 
        get; 
        private set; 
    }
    public List<Card> Deck 
    { 
        get; 
        private set; 
    }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        Stack = new List<Card>();
        Deck = new List<Card>();
    }


    private string HashPassword(string password) //implement Hashing logic
    {

    }

    public void AddStack(Card card)//adds card to stack
    {
        Stack.Add(card);
    }
    public void RemoveStack(Card card)//removes card from stack
    {
        Stack.Remove(card);
    }
    public void AddDeck(Card card)//adds card to deck (implement max cards!!!)
    {
        Deck.Add(card);
    }
    public void RemoveDeck(Card card)//removes card from deck
    {
        Deck.Remove(card);
    }

    //add pack purchasing logic
    //add elo system
}
