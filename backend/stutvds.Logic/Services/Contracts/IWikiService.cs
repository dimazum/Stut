using System.Collections.Generic;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.Logic.Services.Contracts
{
    public interface IWikiService
    {
        string GetUrl(string title, Language language);
        string GetArticle(string url);
        string GetRandomTitle(Language language);
        IEnumerable<string> GetAllTitles(Language language);
    }
}
