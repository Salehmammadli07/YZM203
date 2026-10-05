using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Markdig.Extensions.Tables;

namespace Procedural.WorkbookBuilder;

public sealed record FrontMatterSection(string Path, string Title, string Body, MarkdownDocument Document);

/// <summary>Loads optional front matter separately from the numbered problem collection.</summary>
public static class FrontMatterLoader
{
    public static FrontMatterSection? LoadIntroduction(string root)
    {
        var path = System.IO.Path.Combine(System.IO.Path.GetFullPath(root), "front-matter", "02-giris.md");
        if (!File.Exists(path)) return null;
        var body = File.ReadAllText(path, new UTF8Encoding(false, true));
        var document = Markdown.Parse(body, LessonLoader.Pipeline);
        if (document.Count < 2 || document[0] is not HeadingBlock { Level: 1 } heading ||
            LessonLoader.PlainText(heading.Inline) != "Giriş" ||
            document.OfType<HeadingBlock>().Count(h => h.Level == 1) != 1 ||
            !document.OfType<ParagraphBlock>().Any())
            throw new FormatException("02-giris.md: # Giriş başlığı ve boş olmayan açıklama gerekli.");
        foreach (var block in document.Skip(1)) ValidateIntroductionBlock(block);
        return new(path, "Giriş", body, document);
    }

    private static void ValidateIntroductionBlock(Block block)
    {
        switch (block)
        {
            case HeadingBlock heading when heading.Level is 2 or 3:
                if (string.IsNullOrWhiteSpace(LessonLoader.PlainText(heading.Inline)))
                    throw new FormatException("02-giris.md: alt başlık boş olamaz.");
                break;
            case ParagraphBlock: break;
            case FencedCodeBlock code when code.Info?.Trim() is "csharp" or "text": break;
            case ListBlock or ListItemBlock or QuoteBlock or Table or TableRow or TableCell:
                foreach (var child in (ContainerBlock)block) ValidateIntroductionBlock(child);
                break;
            default: throw new FormatException("02-giris.md: desteklenmeyen blok: " + block.GetType().Name);
        }
        if (block is LeafBlock { Inline: not null } leaf) ValidateIntroductionInline(leaf.Inline);
    }

    private static void ValidateIntroductionInline(ContainerInline container)
    {
        foreach (var item in container)
            switch (item)
            {
                case LiteralInline or LineBreakInline or CodeInline: break;
                case EmphasisInline emphasis: ValidateIntroductionInline(emphasis); break;
                case LinkInline { IsImage: false } link when Uri.TryCreate(link.Url, UriKind.Absolute, out var uri) &&
                                                           uri.Scheme == Uri.UriSchemeHttps:
                    ValidateIntroductionInline(link); break;
                default: throw new FormatException("02-giris.md: desteklenmeyen metin veya bağlantı: " + item.GetType().Name);
            }
    }

    public static FrontMatterSection? LoadPreface(string root)
    {
        var path = System.IO.Path.Combine(System.IO.Path.GetFullPath(root), "front-matter", "01-onsoz.md");
        if (!File.Exists(path)) return null;
        var body = File.ReadAllText(path, new UTF8Encoding(false, true));
        var document = Markdown.Parse(body, LessonLoader.Pipeline);
        if (document.Count == 0 || document[0] is not HeadingBlock { Level: 1 } heading ||
            LessonLoader.PlainText(heading.Inline) != "Önsöz" ||
            document.OfType<HeadingBlock>().Count() != 1)
            throw new FormatException("01-onsoz.md: ilk ve tek başlık # Önsöz olmalı.");
        var paragraphs = document.Skip(1).OfType<ParagraphBlock>().ToList();
        if (paragraphs.Count == 0 || document.Skip(1).Any(block => block is not ParagraphBlock))
            throw new FormatException("01-onsoz.md: başlıktan sonra boş olmayan metin paragrafları gerekli.");
        foreach (var paragraph in paragraphs)
            if (paragraph.Inline is not null) ValidateInline(paragraph.Inline);
        return new(path, "Önsöz", body, document);
    }

    private static void ValidateInline(ContainerInline container)
    {
        foreach (var item in container)
            switch (item)
            {
                case LiteralInline or LineBreakInline or CodeInline: break;
                case EmphasisInline emphasis: ValidateInline(emphasis); break;
                default: throw new FormatException("01-onsoz.md: desteklenmeyen metin öğesi: " + item.GetType().Name);
            }
    }
}
