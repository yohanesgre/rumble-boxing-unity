using Nakama;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleConnection
{
	public string MatchId { get; set; }
	public string HostId { get; set; }
	public IUserPresence Self { get; set; }
	public List<IUserPresence> Opponents { get; set; }

	public BattleConnection(IUserPresence self = null, List<IUserPresence> opponents = null)
	{
		Self = self;
		Opponents = opponents;
	}
}