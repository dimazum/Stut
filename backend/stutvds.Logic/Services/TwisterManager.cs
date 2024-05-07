using System.Collections.Generic;
using stutvds.Logic.Services.Contracts;
using stutvds.Logic.Twisters;

namespace stutvds.Logic.Services
{
    public class TwisterManager : ITwisterManager
    {
        private readonly IEnumerable<ITwister> _twisters;

        public TwisterManager(IEnumerable<ITwister> twisters)
        {
            _twisters = twisters;
        }

        public IEnumerable<IEnumerable<string>> GetAllTwisters()
        {
            foreach (var twister in _twisters)
            {
                yield return twister.GetTwister();
            }
        }
    }
}