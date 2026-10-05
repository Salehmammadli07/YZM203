using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Procedural.WorkbookBuilder;

public sealed record Lesson(string Path, LessonMetadata Metadata, string Body, MarkdownDocument Document);
public sealed record LessonMetadata(string Id, int Order, string Title, string Level,
    IReadOnlyList<string> Prerequisites, IReadOnlyList<string> Concepts);

public static class LessonLoader
{
    public static MarkdownPipeline Pipeline { get; } = new MarkdownPipelineBuilder().UsePipeTables().Build();
    public static readonly string[] Sections = ["Problem tanımı", "Girdi ve çıktı", "Algoritma", "C# çözümü",
        "Örnek çalıştırmalar", "Sınır durumları", "Kazanımlar", "Alıştırmalar"];

    public static IReadOnlyList<Lesson> Load(string root)
    {
        root = Path.GetFullPath(root);
        var folder = Path.Combine(root, "problems");
        if (!Directory.Exists(folder)) throw new DirectoryNotFoundException("Problem klasörü bulunamadı: " + folder);
        var lessons = new List<Lesson>();
        foreach (var path in Directory.EnumerateFiles(folder, "*.md").Order(StringComparer.Ordinal))
        {
            try { lessons.Add(Read(path, root)); }
            catch (Exception error) when (error is ArgumentException or FormatException or YamlException)
            { throw new FormatException($"{Path.GetFileName(path)}: {error.Message}", error); }
        }
        if (lessons.Count == 0) throw new FormatException("Derlenecek problem bulunamadı.");
        lessons.Sort((a, b) => a.Metadata.Order.CompareTo(b.Metadata.Order));
        if (lessons.Select(x => x.Metadata.Id).Distinct().Count() != lessons.Count ||
            lessons.Select(x => x.Metadata.Order).Distinct().Count() != lessons.Count)
            throw new FormatException("Problem kimlikleri ve sıra numaraları benzersiz olmalı.");
        var byId = lessons.ToDictionary(x => x.Metadata.Id);
        foreach (var lesson in lessons)
            foreach (var required in lesson.Metadata.Prerequisites)
                if (!byId.TryGetValue(required, out var prior) || prior.Metadata.Order >= lesson.Metadata.Order)
                    throw new FormatException($"{Path.GetFileName(lesson.Path)}: önkoşul {required} mevcut ve daha önce olmalı.");
        return lessons;
    }

    private static Lesson Read(string path, string root)
    {
        var source = File.ReadAllText(path, Encoding.UTF8);
        var match = Regex.Match(source, @"\A---[^\S\r\n]*\r?\n(.*?)\r?\n---[^\S\r\n]*\r?\n([\s\S]*)\z", RegexOptions.Singleline);
        if (!match.Success) throw new FormatException("YAML üst bilgisi eksik.");
        var yaml = new YamlStream();
        yaml.Load(new StringReader(match.Groups[1].Value));
        if (yaml.Documents.Count != 1 || yaml.Documents[0].RootNode is not YamlMappingNode map)
            throw new FormatException("YAML üst bilgisi bir eşleme olmalı.");
        var idNode = Scalar(map, "id");
        if (idNode.Style is not (ScalarStyle.DoubleQuoted or ScalarStyle.SingleQuoted) ||
            !Regex.IsMatch(idNode.Value ?? "", @"^\d{3}$"))
            throw new FormatException("id üç basamaklı ve tırnak içinde olmalı.");
        var id = idNode.Value!;
        if (!int.TryParse(Text(map, "order"), NumberStyles.None, CultureInfo.InvariantCulture, out var order) || order is < 1 or > 100)
            throw new FormatException("order 1 ile 100 arasında tamsayı olmalı.");
        if (!Path.GetFileName(path).StartsWith(id + "-", StringComparison.Ordinal))
            throw new FormatException("Dosya adı id ile başlamalı.");
        var title = Text(map, "title");
        var level = Text(map, "level");
        var prerequisites = TextList(map, "prerequisites");
        var concepts = TextList(map, "concepts");
        if (concepts.Count == 0) throw new FormatException("En az bir kavram gerekli.");
        var body = match.Groups[2].Value;
        var document = Markdown.Parse(body, Pipeline);
        var blocks = Blocks(document).ToList();
        var headings = blocks.OfType<HeadingBlock>().ToList();
        if (!headings.Where(x => x.Level == 1).Select(x => PlainText(x.Inline)).SequenceEqual([title]))
            throw new FormatException("Tek ana başlık title ile aynı olmalı.");
        if (!headings.Where(x => x.Level == 2).Select(x => PlainText(x.Inline)).SequenceEqual(Sections))
            throw new FormatException("Bölüm başlıkları veya sırası şablonla uyuşmuyor.");
        for (var i = 0; i < document.Count; i++)
        {
            if (document[i] is not HeadingBlock { Level: 2 } heading) continue;
            var following = document.Skip(i + 1).TakeWhile(x => x is not HeadingBlock { Level: 2 }).ToList();
            if (!following.Any(HasContent)) throw new FormatException(PlainText(heading.Inline) + " bölümü boş.");
        }
        var fences = blocks.OfType<FencedCodeBlock>().ToList();
        var code = fences.Where(x => x.Info?.Trim() == "csharp").ToList();
        if (code.Count != 1 || string.IsNullOrWhiteSpace(code[0].Lines.ToString()))
            throw new FormatException("Tek, eksiksiz csharp kod bloğu gerekli.");
        var diagrams = fences.Where(x => x.Info?.Trim() == "diagram").ToList();
        var generated = diagrams.Select((_, i) => Path.GetFullPath(Path.Combine(root, "assets", "figures", $"{id}-{i + 1:00}.png")))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var block in blocks)
        {
            switch (block)
            {
                case HeadingBlock heading when heading.Level > 3:
                    throw new FormatException("Yalnız 1 ile 3 arasındaki başlık düzeyleri destekleniyor.");
                case FencedCodeBlock fence:
                    var info = fence.Info?.Trim() ?? "";
                    if (fence.ClosingFencedCharCount == 0) throw new FormatException("Kapatılmamış kod bloğu.");
                    if (info == "diagram") DiagramRenderer.Validate(DiagramRenderer.Parse(fence.Lines.ToString()));
                    else if (info is not ("csharp" or "text" or "")) throw new FormatException("Desteklenmeyen kod bloğu: " + info);
                    break;
                case CodeBlock:
                    throw new FormatException("Kodları çitli csharp veya text bloklarında yazın.");
                case ParagraphBlock or HeadingBlock or ListBlock or ListItemBlock or QuoteBlock or Table or TableRow or TableCell:
                    break;
                default:
                    throw new FormatException("Desteklenmeyen Markdown öğesi: " + block.GetType().Name);
            }
            if (block is LeafBlock { Inline: not null } leaf) ValidateInline(leaf.Inline, path, root, generated);
        }
        return new(path, new(id, order, title, level, prerequisites, concepts), body, document);
    }

    private static bool HasContent(Block block) => block switch
    {
        LeafBlock leaf => !string.IsNullOrWhiteSpace(PlainText(leaf.Inline)) || leaf is CodeBlock code && !string.IsNullOrWhiteSpace(code.Lines.ToString()),
        ContainerBlock container => container.Any(HasContent),
        _ => false
    };

    private static IEnumerable<Block> Blocks(ContainerBlock container)
    {
        foreach (var block in container)
        {
            yield return block;
            if (block is ContainerBlock child)
                foreach (var descendant in Blocks(child)) yield return descendant;
        }
    }

    private static void ValidateInline(ContainerInline container, string source, string root, IReadOnlySet<string> generated)
    {
        for (var item = container.FirstChild; item is not null; item = item.NextSibling)
        {
            switch (item)
            {
                case LiteralInline or CodeInline or LineBreakInline: break;
                case HtmlInline html when Regex.IsMatch(html.Tag, @"^<br\s*/?>$", RegexOptions.IgnoreCase): break;
                case EmphasisInline emphasis: ValidateInline(emphasis, source, root, generated); break;
                case LinkInline link:
                    if (link.IsImage) ResolveImage(source, link.GetDynamicUrl?.Invoke() ?? link.Url ?? "", root, generated);
                    ValidateInline(link, source, root, generated);
                    break;
                default: throw new FormatException("Desteklenmeyen satır içi öğe: " + item.GetType().Name);
            }
        }
    }

    public static string ResolveImage(string source, string src, string root, IReadOnlySet<string>? generated = null)
    {
        if (string.IsNullOrWhiteSpace(src) || Regex.IsMatch(src, @"^[a-z]+:", RegexOptions.IgnoreCase) || Path.IsPathRooted(src))
            throw new FormatException("Resim bağlantısı yerel ve göreli olmalı.");
        var full = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(source)!, Uri.UnescapeDataString(src)));
        var relative = Path.GetRelativePath(Path.GetFullPath(root), full);
        if (Path.IsPathRooted(relative) || relative == ".." || relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new FormatException("Resim procedural-programming altında olmalı.");
        if (!File.Exists(full) && !(generated?.Contains(full) ?? false))
            throw new FormatException("Resim bulunamadı: " + src);
        if (!new[] { ".png", ".jpg", ".jpeg" }.Contains(Path.GetExtension(full), StringComparer.OrdinalIgnoreCase))
            throw new FormatException("PNG veya JPEG resim gerekli.");
        return full;
    }

    public static string PlainText(ContainerInline? container)
    {
        var text = new StringBuilder();
        if (container is null) return "";
        for (var item = container.FirstChild; item is not null; item = item.NextSibling)
            switch (item)
            {
                case LiteralInline literal: text.Append(literal.Content); break;
                case CodeInline code: text.Append(code.Content); break;
                case LineBreakInline: text.Append(' '); break;
                case ContainerInline child: text.Append(PlainText(child)); break;
            }
        return text.ToString();
    }

    private static YamlScalarNode Scalar(YamlMappingNode map, string key)
    {
        if (!map.Children.TryGetValue(new YamlScalarNode(key), out var value) || value is not YamlScalarNode scalar)
            throw new FormatException(key + " metin veya sayı alanı eksik.");
        return scalar;
    }
    private static string Text(YamlMappingNode map, string key)
    {
        var value = Scalar(map, key).Value;
        if (string.IsNullOrWhiteSpace(value)) throw new FormatException(key + " boş olamaz.");
        return value;
    }
    private static IReadOnlyList<string> TextList(YamlMappingNode map, string key)
    {
        if (!map.Children.TryGetValue(new YamlScalarNode(key), out var value) || value is not YamlSequenceNode sequence)
            throw new FormatException(key + " metinlerden oluşan liste olmalı.");
        var items = new List<string>();
        foreach (var item in sequence.Children)
        {
            if (item is not YamlScalarNode scalar || string.IsNullOrWhiteSpace(scalar.Value))
                throw new FormatException(key + " boş olmayan metinlerden oluşmalı.");
            items.Add(scalar.Value);
        }
        return items;
    }
}
