using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageOnGameOver : MatchMessage<MatchMessageOnGameOver>
{
	public readonly string WinnerPlayerId;

	public MatchMessageOnGameOver(string winnerPlayerId, int enemyId, int damage)
	{
		WinnerPlayerId = winnerPlayerId;
	}
}
