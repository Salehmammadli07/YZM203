using System.Text;
using System.Text.RegularExpressions;

namespace Procedural.WorkbookBuilder;

public sealed record Chapter(int Number, string Title, IReadOnlyList<Lesson> Lessons)
{
    public string Heading => $"Bölüm {Number}: {Title}";
    public string Anchor => $"C{Number:00}";
}

public static class ChapterLoader
{
    public static IReadOnlyList<Chapter> Load(string root, IReadOnlyList<Lesson> lessons)
    {
        var path = Path.Combine(root, "chapters.md");
        if (!File.Exists(path)) return [];
        var headings = Regex.Matches(File.ReadAllText(path, new UTF8Encoding(false, true)),
            @"^# Bölüm ([1-9][0-9]*): ([^\r\n]+)\r?$", RegexOptions.Multiline);
        if (lessons.Count % 10 != 0 || headings.Count != lessons.Count / 10 || headings.Count == 0)
            throw new FormatException("chapters.md: Her bölüm tam 10 problem içermeli ve her bölümün bir başlığı olmalı.");
        var chapters = new List<Chapter>();
        for (var i = 0; i < headings.Count; i++)
        {
            if (headings[i].Groups[1].Value != (i + 1).ToString(System.Globalization.CultureInfo.InvariantCulture))
                throw new FormatException("chapters.md: Bölüm numaraları 1'den başlayarak sıralı olmalı.");
            var group = lessons.Skip(i * 10).Take(10).ToArray();
            for (var j = 0; j < group.Length; j++)
                if (group[j].Metadata.Id != (i * 10 + j + 1).ToString("000") || group[j].Metadata.Order != i * 10 + j + 1)
                    throw new FormatException("chapters.md: Problem kimlikleri ve öğretim sırası bölüm aralıklarıyla eşleşmeli.");
            var title = headings[i].Groups[2].Value.Trim();
            if (title.Length == 0) throw new FormatException("chapters.md: Bölüm başlığı boş olamaz.");
            chapters.Add(new Chapter(i + 1, title, group));
        }
        return chapters;
    }
}
