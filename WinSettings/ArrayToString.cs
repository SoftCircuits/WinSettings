// Copyright (c) 2019-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftCircuits.WinSettings
{
    internal static class ArrayToString
    {
        /// <summary>
        /// Represents the given array as a single string.
        /// </summary>
        /// <param name="array">String array to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string Encode(string[] array)
        {
            StringBuilder builder = new();

            if (array == null)
                return string.Empty;

            for (int i = 0; i < array.Length; i++)
            {
                if (i > 0)
                    builder.Append(',');
                if (array[i].IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0)
                    builder.Append(string.Format("\"{0}\"", array[i].Replace("\"", "\"\"")));
                else
                    builder.Append(array[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Decodes a string created with <see cref="Encode"></see> back to an array.
        /// </summary>
        /// <param name="s">String to decode.</param>
        /// <returns>The decoded array.</returns>
        public static string[] Decode(string s)
        {
            List<string> list = new();
            int pos = 0;

            if (s == null)
                return Array.Empty<string>();

            while (pos < s.Length)
            {
                if (s[pos] == '\"')
                {
                    // Parse quoted value
                    StringBuilder builder = new();

                    // Skip starting quote
                    pos++;
                    while (pos < s.Length)
                    {
                        if (s[pos] == '"')
                        {
                            // Skip quote
                            pos++;
                            // One quote signifies end of value
                            // Two quote signifies single quote literal
                            if (pos >= s.Length || s[pos] != '"')
                                break;
                        }
                        builder.Append(s[pos++]);
                    }
                    list.Add(builder.ToString());
                    // Skip delimiter
                    pos = s.IndexOf(',', pos);
                    if (pos == -1)
                        pos = s.Length;
                    pos++;
                }
                else
                {
                    // Parse value
                    int start = pos;
                    pos = s.IndexOf(',', pos);
                    if (pos == -1)
                        pos = s.Length;
#if NETSTANDARD
                    list.Add(s.Substring(start, pos - start));
#else
                    list.Add(s[start..pos]);
#endif
                    // Skip delimiter
                    pos++;
                }
            }
            return list.ToArray();
        }
    }
}
