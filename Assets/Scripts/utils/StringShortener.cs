using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

public static class StringShortener
{
    public static string ShortenGuid(string _guid)
    {
        var deletedDot = _guid.Replace(".", "");
        string modifiedBase64 = Convert.ToBase64String(Guid.Parse(deletedDot).ToByteArray())
            .Replace('+', '-').Replace('/', '_') // avoid invalid URL characters
            .Substring(0, 22);
        return modifiedBase64;
    }

    public static string ParseShortGuid(string shortGuid)
    {
        string base64 = shortGuid.Replace('-', '+').Replace('_', '/') + "==";
        Byte[] bytes = Convert.FromBase64String(base64);
        return new Guid(bytes).ToString() + ".";
    }
}