using System;

namespace DiggerClassic
{
    public class ScoreTuple : Tuple<string, int>
    {
        public ScoreTuple(string key, int value) : base(key, value)
        {
        }
    }
}