using System;
using System.Collections.Generic;
using System.Linq;
using StopStatAuth_6_0.Entities;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Data;

namespace stutvds.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddArticle(string content, string title, Language language, AgeGroup ageGroup)
        {
            var article = GetArticleByTitle(title);

            if(article != null)
            {
                article.Content = content;
                article.CreatedAt = DateTime.Now;
                article.Language = language;
                article.AgeGroup = ageGroup;

                _dbContext.SaveChanges();
                return;
            }

            _dbContext.Articles.Add(new ArticleEntity()
            {
                CreatedAt = DateTime.Now,
                Content = content,
                Title = title,
                Language = language,
                AgeGroup = ageGroup
            });

            _dbContext.SaveChanges();
        }

        public ArticleEntity GetArticle(Guid id)
        {
            var art =  _dbContext.Articles.FirstOrDefault(a => a.Id == id);

            return art;
        }

        public ArticleEntity GetArticleByTitle(string title)
        {
            var art = _dbContext.Articles.FirstOrDefault(a => a.Title == title);
            return art;
        }

        public List<string> GetArticleTitles(Language language)
        {
            return _dbContext
                .Articles
                .Where(x => x.Language == language)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();
        }

        public ArticleEntity GetRandomArticle(Language language, AgeGroup ageGroup = AgeGroup.Adult)
        {
            var art = _dbContext
                .Articles
                .OrderBy(r => Guid.NewGuid())
                .Where(x => x.Language == language)
                .Where(x => x.AgeGroup == ageGroup)
                .Take(1)
                .FirstOrDefault();

            return art;
        }
    }
}