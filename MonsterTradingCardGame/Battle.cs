using System;

public class Battle
{
    public User Player1 { get; set; }
    public User Player2 { get; set; }

    public Battle(User player1, User player2)
	{
		Player1 = player1;
        Player2 = player2;
	}

    public void startBattle()
    {
        //add battle logic
    }
}
