using System;

namespace DiggerClassic.API
{
    public class ScoreTuple : Tuple<string, int>
    {
        public ScoreTuple(string key, int value) : base(key, value)
        {
        }
    }
}