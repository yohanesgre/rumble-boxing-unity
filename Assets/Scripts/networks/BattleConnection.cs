using Nakama;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleConnection
{
	public string MatchId { get; set; }
	public string HostId { get; set; }
	public string OpponentId { get; set; }
	public IMatchmakerMatched Matched { get; set; }

	public BattleConnection(IMatchmakerMatched matched)
	{
		Matched = matched;
	}
}