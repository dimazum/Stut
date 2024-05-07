using System.Collections.Generic;

namespace stutvds.Logic.Twisters
{
    public interface ITwister
    {
        string Template { get; }

        IEnumerable<string> GetTwister(int rows = 40, int columns = 4);
    }
}