using System.Collections.Generic;
using System.Linq;

namespace stutvds.Mappers;

public static class ArticleCategoryMapper
{
    private static readonly Dictionary<string, string> EnToRu = new()
    {
        { "Countries", "Страны" }
    };

    private static readonly Dictionary<string, string> RuToEn =
        EnToRu.ToDictionary(x => x.Value, x => x.Key);

    public static string ToRu(string en)
    {
        return en == null ? null : EnToRu.GetValueOrDefault(en, en);
    }

    public static string ToEn(string ru)
    {
        return ru == null ? null : RuToEn.GetValueOrDefault(ru, ru);
    }
}
