using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageOnPlayerCorrect : MatchMessage<MatchMessageOnPlayerCorrect>
{
	public readonly string PlayerId;

	public MatchMessageOnPlayerCorrect(string playerId)
	{
		PlayerId = playerId;
	}
}
