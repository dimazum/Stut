using System;
using System.Collections.Generic;
using StopStatAuth_6_0.Entities;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.DAL.Repositories.Contracts
{
    public interface IArticleRepository
    {
        void AddArticle(string content, string title, Language language, AgeGroup ageGroup);
        ArticleEntity GetArticle(Guid id);
        ArticleEntity GetArticleByTitle(string title);
        ArticleEntity GetRandomArticle(Language language, AgeGroup ageGroup = AgeGroup.Adult);
        List<string> GetArticleTitles(Language language);
    }
}