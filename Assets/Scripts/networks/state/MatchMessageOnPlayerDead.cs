using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageOnPlayerDead : MatchMessage<MatchMessageOnPlayerDead>
{
	public readonly string PlayerId;

	public MatchMessageOnPlayerDead(string playerId)
	{
		PlayerId = playerId;
	}
}
