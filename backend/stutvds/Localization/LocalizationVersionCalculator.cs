using System;
using System.IO;
using System.Security.Cryptography;

namespace stutvds.Localization;

public class LocalizationVersionCalculator
{
    public string Calculate(string resourcePath)
    {
        var files = Directory
            .GetFiles(resourcePath, "*.resx", SearchOption.AllDirectories);

        using var sha = SHA256.Create();

        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file);
            sha.TransformBlock(bytes, 0, bytes.Length, null, 0);
        }

        sha.TransformFinalBlock([], 0, 0);

        return Convert.ToHexString(sha.Hash!);
    }
}