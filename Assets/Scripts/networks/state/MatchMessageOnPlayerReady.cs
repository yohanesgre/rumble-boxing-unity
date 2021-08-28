using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageOnPlayerReady : MatchMessage<MatchMessageOnPlayerReady>
{
	public readonly string PlayerId;

	public MatchMessageOnPlayerReady(string playerId)
	{
		PlayerId = playerId;
	}
}
