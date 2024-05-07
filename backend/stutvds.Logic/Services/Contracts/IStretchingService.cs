using System.Collections.Generic;

namespace stutvds.Logic.Services.Contracts
{
    public interface IStretchingService
    {
        IEnumerable<IEnumerable<string>> GetAll();
    }
}