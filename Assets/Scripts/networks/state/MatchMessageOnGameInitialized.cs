using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageOnGameInitialized : MatchMessage<MatchMessageOnGameInitialized>
{
	public readonly int Rounds;

    public MatchMessageOnGameInitialized(int rounds)
    {
        Rounds = rounds;
    }
}
