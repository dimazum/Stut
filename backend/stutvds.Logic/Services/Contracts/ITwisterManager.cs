using System.Collections.Generic;

namespace stutvds.Logic.Services.Contracts
{
    public interface ITwisterManager
    {
        IEnumerable<IEnumerable<string>> GetAllTwisters();
    }
}