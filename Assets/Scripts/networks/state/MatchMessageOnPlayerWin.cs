using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageOnPlayerWin : MatchMessage<MatchMessageOnPlayerWin>
{
	public readonly string PlayerId;

	public MatchMessageOnPlayerWin(string playerId)
	{
		PlayerId = playerId;
	}
}
