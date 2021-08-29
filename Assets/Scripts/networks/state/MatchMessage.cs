using Nakama.TinyJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatchMessage<T>
{
	/// <summary>
	/// Parses json gained from server to MatchMessage class object
	/// </summary>
	/// <param name="json"></param>
	/// <returns></returns>
	public static T Parse(string json)
	{
		return JsonParser.FromJson<T>(json);
	}

	/// <summary>
	/// Creates string with json to be send as match state message
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public static string ToJson(T message)
	{
		return JsonWriter.ToJson(message);
	}
}