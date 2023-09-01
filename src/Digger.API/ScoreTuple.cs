using System;

namespace DiggerAPI
{
    public class ScoreTuple : Tuple<string, int>
    {
        public ScoreTuple(string key, int value) : base(key, value)
        {
        }
    }
}