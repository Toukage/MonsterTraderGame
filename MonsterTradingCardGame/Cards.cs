using System;
using System.Collections.Generic;//library for lists

public class Card
{
    public enum CardType //enum collection of Card Types
    {
        Monster,
        Spell
    }

    public enum ElementType //enum collection Elements for cards
    {
        Fire,
        Water,
        Air,
        Earth,
        Normal
    }

    public string Name
    {
        get;
        set;
    }
    public int Damage
    {
        get;
        set;
    }
    public ElementType Element
    {
        get;
        set;
    }
    public CardType Type
    {
        get;
        set;
    }
    

    public Card(string name, int damage, CardType type, ElementType element)//assignes the porperties to a Card
    {
        Name = name;
        Damage = damage;
        Element = element;
        Type = type;
    }
	
}

public class Package // package is a list of 5 Cards
{
    public List<Card> Cards // list that stores objects from the class Cards, and allows new ones to be added
    { 
        get; 
        private set; 
    } 
    
    public Package(List<Card> cards)
    {
        Cards = cards;
    } 
}