using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Logic.Common;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Logic.Services
{
    public class WIkiService : IWikiService
    {
        private const string UrlWiki =
            @"https://{0}.wikipedia.org/w/api.php?action=opensearch&limit=2&format=xml&search={1}\";

        private Dictionary<Language, string> _languageMap = new Dictionary<Language, string>()
        {
            { Language.English, "eng" },
            { Language.Russian, "ru" }
        };

        public string GetUrl(string title, Language language)
        {
            string result = string.Empty;

            WebClient webClient = new WebClient();

            string text = webClient.DownloadString(string.Format(UrlWiki, _languageMap[language], title));

            webClient.Dispose();

            XmlDocument xmltest = new XmlDocument();
            xmltest.LoadXml(text);

            XmlNodeList elemlist = xmltest.GetElementsByTagName("Url");

            if (elemlist.Count != 0)
            {
                result = elemlist[0].InnerXml;
            }

            return result;
        }

        public string GetArticle(string url)
        {
            HtmlDocument HD = new HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            HD = web.Load(url);

            var node = HD.DocumentNode.SelectSingleNode("//div[@class='mw-parser-output']");
            string outputText = "";

            string newStr = "";

            try
            {
                if (node != null && node.HasChildNodes)
                {
                    var modes = node.SelectNodes("//p");
                    foreach (HtmlNode HN in modes)
                    {
                        outputText += HN?.InnerText;
                    }
                }

                var decodedString1 = HttpUtility.HtmlDecode(outputText);
                var decodedString2 = Regex.Replace(decodedString1, @"\[(?<content>.+?)\]", "");

                var arr = decodedString2.ToCharArray();

                if (arr.Length >= 25000)
                {
                    newStr = new string(arr.Take(25000).ToArray());
                    newStr += " ...";
                    newStr += System.Environment.NewLine;

                    return newStr;
                }

                newStr = decodedString2;
            }
            catch (Exception e)
            {
            }


            return newStr.CleanBraketsWithContent();
        }

        public string GetRandomTitle(Language language)
        {
            string path = string.Format(@"wwwroot/Topics/{0}/Countries.txt", _languageMap[language]);

            string[] lines = File.ReadAllLines(path);

            var random = new Random();
            var index = random.Next(0, lines.Length - 1);

            return lines[index];
        }

        public IEnumerable<string> GetAllTitles(Language language)
        {
            string path = string.Format(@"wwwroot/Topics/{0}/Countries.txt", _languageMap[language]);


            var lines = File.ReadAllLines(path)
                .OrderBy(x => x);


            return lines;
        }
    }
}