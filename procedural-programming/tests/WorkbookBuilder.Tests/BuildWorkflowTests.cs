using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Procedural.WorkbookBuilder;
using Xunit;

namespace Procedural.WorkbookBuilder.Tests;

public sealed class BuildWorkflowTests : IDisposable
{
    private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
    private readonly string root = Path.Combine(Path.GetTempPath(), "procedural-csharp-tests-" + Guid.NewGuid().ToString("N"));
    private string Output => Path.Combine(root, "build", "book.docx");
    public BuildWorkflowTests() => Directory.CreateDirectory(Path.Combine(root, "problems"));

    private string Write(string id = "001", int order = 1, string title = "Örnek problem", string prerequisites = "[]", string? diagram = null, bool preview = false)
    {
        var algorithm = "1. Değeri atayın.\n2. Değeri yazdırın.";
        if (diagram is not null) algorithm += "\n\n```diagram\n" + diagram + "\n```";
        if (preview) algorithm += $"\n\n![Atama akışı](../assets/figures/{id}-01.png)";
        var source = $"---\nid: \"{id}\"\norder: {order}\ntitle: {JsonSerializer.Serialize(title)}\nlevel: \"Başlangıç\"\nprerequisites: {prerequisites}\nconcepts: [\"Atama\"]\n---\n\n# {title}\n\n" +
            "## Problem tanımı\n\nBir değeri ekrana yazdırın.\n\n" +
            "## Girdi ve çıktı\n\n| Tür | Açıklama |\n| --- | --- |\n| Çıktı | Bir sayı |\n\n" +
            $"## Algoritma\n\n{algorithm}\n\n" +
            "## C# çözümü\n\n```csharp\nusing System;\nConsole.WriteLine(1);\n```\n\n" +
            "## Örnek çalıştırmalar\n\n| Girdi | Sonuç |\n| --- | --- |\n| Yok | `İlk satır`<br>`İkinci satır` |\n\n" +
            "## Sınır durumları\n\n- Girdi gerektirmez.\n\n" +
            "## Kazanımlar\n\n- İşlem sırasını açıklama.\n\n" +
            "## Alıştırmalar\n\n1. Farklı bir değer yazdırın.\n";
        var path = Path.Combine(root, "problems", id + "-ornek.md");
        File.WriteAllText(path, source);
        return path;
    }

    private XDocument Compile()
    {
        WordBookWriter.Write(LessonLoader.Load(root), root, Output);
        using var zip = ZipFile.OpenRead(Output);
        using var stream = zip.GetEntry("word/document.xml")!.Open();
        return XDocument.Load(stream);
    }

    private static string Diagram(string text = "x = 1") => JsonSerializer.Serialize(new
    {
        caption = "Atama akışı",
        nodes = new[] { new { id = "assign", text, kind = "process", x = 0, y = 0 },
            new { id = "end", text = "Bitir", kind = "terminal", x = 0, y = 1 } },
        edges = new[] { new { from = "assign", to = "end" } }
    });
    private static string Text(XDocument doc) => string.Concat(doc.Descendants(W + "t").Select(x => x.Value));
    private void Change(string path, string from, string to) => File.WriteAllText(path, File.ReadAllText(path).Replace(from, to, StringComparison.Ordinal));

    [Fact] public void LessonsFollowTeachingOrder()
    {
        Write("002", 2, prerequisites: "[\"001\"]"); Write();
        Assert.Equal(new[] { "001", "002" }, LessonLoader.Load(root).Select(x => x.Metadata.Id));
    }

    [Theory]
    [InlineData("[\"999\"]")]
    [InlineData("[\"001\"]")]
    [InlineData("[\"002\"]")]
    public void InvalidPrerequisitesFail(string prerequisites)
    {
        Write(prerequisites: prerequisites); Write("002", 2);
        Assert.Throws<FormatException>(() => LessonLoader.Load(root));
    }

    [Fact] public void DuplicateIdsFail()
    {
        var source = Write(); File.Copy(source, Path.Combine(root, "problems", "001-another.md"));
        Assert.Throws<FormatException>(() => LessonLoader.Load(root));
    }
    [Fact] public void DuplicateOrdersFail()
    {
        Write(); Write("002", 1);
        Assert.Throws<FormatException>(() => LessonLoader.Load(root));
    }

    [Theory]
    [InlineData("prerequisites: []", "")]
    [InlineData("id: \"001\"", "id: 001")]
    [InlineData("order: 1", "order: 101")]
    [InlineData("concepts: [\"Atama\"]", "concepts: []")]
    [InlineData("## Kazanımlar", "## Başka bölüm")]
    [InlineData("# Örnek problem", "# Başka başlık")]
    [InlineData("```csharp", "```python")]
    [InlineData("- İşlem sırasını açıklama.", "")]
    public void InvalidSourceFails(string from, string to)
    {
        Change(Write(), from, to);
        Assert.Throws<FormatException>(() => LessonLoader.Load(root));
    }

    [Fact] public void MissingPlainFigureFails()
    {
        Change(Write(), "Bir değeri ekrana yazdırın.", "![Eksik](../assets/figures/001-01.png)");
        Assert.Throws<FormatException>(() => LessonLoader.Load(root));
    }
    [Fact] public void ImagesCannotEscapeSourceRoot()
    {
        Assert.Throws<FormatException>(() => LessonLoader.ResolveImage(Path.Combine(root, "problems", "001.md"), "../../outside.png", root));
    }

    [Fact] public void WordUsesRealHeadingStylesAndReadableTitle()
    {
        Write(); var doc = Compile();
        Assert.Single(doc.Descendants(W + "p"), p => p.Element(W + "pPr")?.Element(W + "pStyle")?.Attribute(W + "val")?.Value == "Heading1");
        Assert.Equal(8, doc.Descendants(W + "pStyle").Count(s => s.Attribute(W + "val")?.Value == "Heading2"));
        var title = doc.Descendants(W + "p").First(p => p.Element(W + "pPr")?.Element(W + "pStyle")?.Attribute(W + "val")?.Value == "Title");
        Assert.Equal("56", title.Descendants(W + "sz").First().Attribute(W + "val")?.Value);
    }
    [Fact] public void WordFieldsAndPackageAreValid()
    {
        Write(diagram: Diagram(), preview: true); Compile();
        using var package = WordprocessingDocument.Open(Output, false);
        var errors = new OpenXmlValidator().Validate(package).ToList();
        Assert.True(errors.Count == 0, string.Join("\n", errors.Select(x => x.Description + " " + x.Path?.XPath)));
        var fields = package.MainDocumentPart!.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.FieldCode>().Select(x => x.Text).ToList();
        Assert.Contains(fields, x => x.Contains("TOC") && x.Contains("1-1"));
        Assert.Contains(fields, x => x.Contains("TOC") && x.Contains("Şekil"));
        Assert.Contains(fields, x => x.Contains("SEQ Şekil"));
    }
    [Fact] public void CoverUsesItsOwnBorderlessSectionWithoutChangingBodyMargins()
    {
        Write();
        Directory.CreateDirectory(Path.Combine(root, "assets"));
        var path = Path.Combine(root, "assets", "cover.png");
        using (var bitmap = new System.Drawing.Bitmap(2, 3)) bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        var doc = Compile();
        var sections = doc.Descendants(W + "sectPr").ToArray();
        Assert.Equal(2, sections.Length);
        foreach (var name in new[] { "top", "bottom", "left", "right", "header", "footer", "gutter" })
            Assert.Equal("0", sections[0].Element(W + "pgMar")!.Attribute(W + name)?.Value);
        Assert.Empty(sections[0].Elements(W + "footerReference"));
        Assert.Equal("1224", sections[1].Element(W + "pgMar")!.Attribute(W + "left")?.Value);
        Assert.Equal("1080", sections[1].Element(W + "pgMar")!.Attribute(W + "top")?.Value);
        XNamespace dw = "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing";
        var anchor = Assert.Single(doc.Descendants(dw + "anchor"));
        Assert.All(anchor.Elements().Where(e => e.Name == dw + "positionH" || e.Name == dw + "positionV"),
            e => { Assert.Equal("page", e.Attribute("relativeFrom")?.Value); Assert.Equal("0", e.Element(dw + "posOffset")?.Value); });
        Assert.Equal(11906L * 635, (long)anchor.Element(dw + "extent")!.Attribute("cx")!);
        Assert.Equal(16838L * 635, (long)anchor.Element(dw + "extent")!.Attribute("cy")!);
        using var package = WordprocessingDocument.Open(Output, false);
        Assert.Empty(new OpenXmlValidator().Validate(package));
        using var embedded = package.MainDocumentPart!.ImageParts.Single().GetStream();
        using var bytes = new MemoryStream(); embedded.CopyTo(bytes);
        Assert.Equal(File.ReadAllBytes(path), bytes.ToArray());
    }

    [Fact] public void BackCoverEndsTheBookWithAnExplicitEmptyFooter()
    {
        Write();
        Directory.CreateDirectory(Path.Combine(root, "assets"));
        using (var bitmap = new System.Drawing.Bitmap(2, 3))
            bitmap.Save(Path.Combine(root, "assets", "back-cover.png"), System.Drawing.Imaging.ImageFormat.Png);
        var doc = Compile();
        var body = doc.Root!.Element(W + "body")!;
        var lastSection = body.Elements().Last();
        Assert.Equal(W + "sectPr", lastSection.Name);
        Assert.Equal("nextPage", lastSection.Element(W + "type")?.Attribute(W + "val")?.Value);
        foreach (var name in new[] { "top", "bottom", "left", "right" })
            Assert.Equal("0", lastSection.Element(W + "pgMar")!.Attribute(W + name)?.Value);
        XNamespace dw = "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing";
        Assert.Equal("Arka kapak", body.Elements().Reverse().Skip(1).First().Descendants(dw + "docPr").Single().Attribute("name")?.Value);
        using var package = WordprocessingDocument.Open(Output, false);
        Assert.Empty(new OpenXmlValidator().Validate(package));
        XNamespace r = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        var footerId = (string)lastSection.Element(W + "footerReference")!.Attribute(r + "id")!;
        var footer = Assert.IsType<FooterPart>(package.MainDocumentPart!.GetPartById(footerId));
        Assert.Empty(footer.Footer!.Descendants<DocumentFormat.OpenXml.Wordprocessing.FieldCode>());
        Assert.Empty(footer.Footer.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>());
    }

    [Fact] public void TableLineBreaksRemainLineBreaks()
    {
        Write(); var doc = Compile();
        Assert.Single(doc.Descendants(W + "tc"), c => c.Descendants(W + "br").Any());
    }
    [Fact] public void DiagramPreviewIsNotDuplicated()
    {
        Write(diagram: Diagram(), preview: true); var doc = Compile();
        Assert.Single(doc.Descendants(W + "drawing"));
    }
    [Fact] public void EditedDiagramIsRegeneratedAndEmbedded()
    {
        Write(diagram: Diagram(), preview: true); Compile();
        var png = Path.Combine(root, "assets", "figures", "001-01.png");
        var original = SHA256.HashData(File.ReadAllBytes(png));
        Write(diagram: Diagram("x = 2"), preview: true); var doc = Compile();
        var revised = File.ReadAllBytes(png);
        Assert.False(original.SequenceEqual(SHA256.HashData(revised)));
        Assert.Single(doc.Descendants(W + "drawing"));
        using var package = WordprocessingDocument.Open(Output, false);
        using var source = package.MainDocumentPart!.ImageParts.Single().GetStream();
        using var bytes = new MemoryStream(); source.CopyTo(bytes);
        Assert.Equal(revised, bytes.ToArray());
    }
    [Fact] public void PreviewBeforeDiagramUsesFreshPicture()
    {
        var path = Write(diagram: Diagram(), preview: true);
        var preview = "![Atama akışı](../assets/figures/001-01.png)";
        Change(path, "\n\n" + preview, ""); Change(path, "```diagram", preview + "\n\n```diagram");
        Assert.Single(Compile().Descendants(W + "drawing"));
    }
    [Fact] public void MarkdownTitleEditUpdatesHeadingAndIndex()
    {
        Write(title: "İlk başlık"); Compile(); Write(title: "Güncel başlık"); var doc = Compile();
        Assert.DoesNotContain("İlk başlık", Text(doc)); Assert.Contains("Güncel başlık", Text(doc));
    }
    [Fact] public void InlinePicturePreservesSurroundingText()
    {
        Directory.CreateDirectory(Path.Combine(root, "assets"));
        using (var png = new System.Drawing.Bitmap(40, 20)) png.Save(Path.Combine(root, "assets", "example.png"), System.Drawing.Imaging.ImageFormat.Png);
        Change(Write(), "Bir değeri ekrana yazdırın.", "Şekli inceleyin: ![Örnek](../assets/example.png) ve adımları izleyin.");
        var doc = Compile(); Assert.Contains("Şekli inceleyin:", Text(doc)); Assert.Contains("ve adımları izleyin.", Text(doc));
        Assert.Single(doc.Descendants(W + "drawing"));
    }
    [Fact] public void DiagramCaptionIsRequired()
    {
        var json = System.Text.Json.Nodes.JsonNode.Parse(Diagram())!.AsObject();
        json.Remove("caption");
        Assert.Throws<FormatException>(() => DiagramRenderer.Parse(json.ToJsonString()));
    }
    [Fact] public void NonfiniteDiagramPositionFails()
    {
        var diagram = DiagramRenderer.Parse(Diagram());
        diagram.Nodes[0] = diagram.Nodes[0] with { X = double.NaN };
        Assert.Throws<ArgumentException>(() => DiagramRenderer.Validate(diagram));
    }
    [Fact] public void DiagramRenderingIsDeterministic()
    {
        var spec = DiagramRenderer.Parse(Diagram()); var png = Path.Combine(root, "figure.png");
        DiagramRenderer.Render(spec, png); var bytes = File.ReadAllBytes(png);
        DiagramRenderer.Render(spec, png); Assert.Equal(bytes, File.ReadAllBytes(png));
    }

    private void WritePreface(string source)
    {
        Directory.CreateDirectory(Path.Combine(root, "front-matter"));
        File.WriteAllText(Path.Combine(root, "front-matter", "01-onsoz.md"), source);
    }

    private void WriteIntroduction(string source)
    {
        Directory.CreateDirectory(Path.Combine(root, "front-matter"));
        File.WriteAllText(Path.Combine(root, "front-matter", "02-giris.md"), source);
    }

    [Fact] public void IntroductionIsOptionalAndSeparateFromNumberedLessons()
    {
        Write();
        Assert.Null(FrontMatterLoader.LoadIntroduction(root));
        WriteIntroduction("# Giriş\n\nKavramsal açıklama.\n\n## İşlem sırası\n\nAdımları izleyin.\n");
        Assert.Equal("Giriş", FrontMatterLoader.LoadIntroduction(root)!.Title);
        Assert.Single(LessonLoader.Load(root));
        var doc = Compile();
        Assert.Contains("1 çözümlü problem", Text(doc));
        Assert.Single(doc.Descendants(W + "bookmarkStart"), b => b.Attribute(W + "name")?.Value == "P001");
    }

    [Fact] public void IntroductionFollowsIndexesAndKeepsChapterHierarchyAndFigures()
    {
        for (var i = 1; i <= 100; i++) Write(i.ToString("000"), i, diagram: i == 1 ? Diagram() : null);
        File.WriteAllText(Path.Combine(root, "chapters.md"), string.Join("\n", Enumerable.Range(1, 10).Select(i => $"# Bölüm {i}: Konu {i}")));
        WritePreface("# Önsöz\n\nKorunacak önsöz.\n");
        WriteIntroduction("# Giriş\n\nTanım.\n\n## Yaklaşımlar\n\n### Karşılaştırma\n\n| Yaklaşım | Açıklama |\n| --- | --- |\n| Prosedürel | Görevler |\n\n```csharp\nConsole.WriteLine(2);\n```\n\n[Kaynak](https://learn.microsoft.com/en-us/dotnet/csharp/methods)\n");
        var doc = Compile();
        var paragraphs = doc.Descendants(W + "p").ToList();
        int Position(string text) => paragraphs.FindIndex(p =>
            p.Element(W + "pPr")?.Element(W + "pStyle")?.Attribute(W + "val")?.Value != "TOC1" &&
            string.Concat(p.Descendants(W + "t").Select(t => t.Value)) == text);
        Assert.True(Position("Önsöz") < Position("İçindekiler"));
        Assert.True(Position("İçindekiler") < Position("Şekiller"));
        Assert.True(Position("Şekiller") < Position("Giriş"));
        Assert.True(Position("Giriş") < Position("Bölüm 1: Konu 1"));
        Assert.NotNull(paragraphs[Position("Giriş")].Element(W + "pPr")!.Element(W + "pageBreakBefore"));
        Assert.Equal("IntroductionSection", paragraphs[Position("Yaklaşımlar")].Element(W + "pPr")!.Element(W + "pStyle")!.Attribute(W + "val")!.Value);
        Assert.Equal("IntroductionSubsection", paragraphs[Position("Karşılaştırma")].Element(W + "pPr")!.Element(W + "pStyle")!.Attribute(W + "val")!.Value);
        var introductionTable = doc.Descendants(W + "tbl").First(t => t.Value.Contains("Prosedürel"));
        Assert.All(introductionTable.Elements(W + "tr").First().Descendants(W + "p"),
            p => Assert.NotNull(p.Element(W + "pPr")!.Element(W + "keepNext")));
        Assert.All(introductionTable.Elements(W + "tr").Last().Descendants(W + "p"),
            p => Assert.Null(p.Element(W + "pPr")!.Element(W + "keepNext")));
        Assert.Contains(doc.Descendants(W + "instrText"), f => f.Value.Contains("Introduction Section,2,Introduction Subsection,3"));
        Assert.All(new[] { "Giris", "Giris001", "Giris002" }, anchor =>
            Assert.Contains(doc.Descendants(W + "hyperlink"), h => h.Attribute(W + "anchor")?.Value == anchor));
        Assert.Equal(100, doc.Descendants(W + "bookmarkStart").Count(b => b.Attribute(W + "name")?.Value?.StartsWith('P') == true));
        Assert.Single(doc.Descendants(W + "bookmarkStart"), b => b.Attribute(W + "name")?.Value == "F001");
        Assert.Equal(10, doc.Descendants(W + "sectPr").Count(s => s.Element(W + "type")?.Attribute(W + "val")?.Value == "evenPage"));
        Assert.Empty(doc.Descendants(W + "pgNumType"));
        using var package = WordprocessingDocument.Open(Output, false);
        Assert.Empty(new OpenXmlValidator().Validate(package));
        Assert.Single(package.MainDocumentPart!.HyperlinkRelationships);
    }

    [Fact] public void IntroductionChangesAreRebuiltWithoutChangingPrefaceOrProblems()
    {
        Write(); WritePreface("# Önsöz\n\nKorunacak önsöz.\n");
        var prefacePath = Path.Combine(root, "front-matter", "01-onsoz.md");
        var before = File.ReadAllBytes(prefacePath);
        WriteIntroduction("# Giriş\n\nİlk açıklama.\n\n## İlk alt başlık\n\nMetin.\n"); Compile();
        WriteIntroduction("# Giriş\n\nYeni açıklama.\n\n## Yeni alt başlık\n\nMetin.\n");
        var doc = Compile();
        Assert.Contains("Yeni açıklama.", Text(doc)); Assert.Contains("Yeni alt başlık", Text(doc));
        Assert.DoesNotContain("İlk açıklama.", Text(doc)); Assert.DoesNotContain("İlk alt başlık", Text(doc));
        Assert.Equal(before, File.ReadAllBytes(prefacePath));
        Assert.Single(LessonLoader.Load(root));
    }

    [Theory]
    [InlineData("")]
    [InlineData("# Giriş\n")]
    [InlineData("# Yanlış\n\nMetin.\n")]
    [InlineData("# Giriş\n\nMetin.\n\n# İkinci\n")]
    [InlineData("# Giriş\n\nMetin.\n\n#### Çok derin\n")]
    [InlineData("# Giriş\n\nMetin.\n\n```diagram\n{}\n```\n")]
    [InlineData("# Giriş\n\n![Resim](../assets/resim.png)\n")]
    [InlineData("# Giriş\n\n<script>kod</script>\n")]
    [InlineData("# Giriş\n\n[Kaynak](javascript:alert)\n")]
    public void InvalidIntroductionFailsBeforeReplacingExistingOutput(string source)
    {
        Write(); Compile(); var before = File.ReadAllBytes(Output);
        WriteIntroduction(source);
        Assert.Throws<FormatException>(() => Compile());
        Assert.Equal(before, File.ReadAllBytes(Output));
    }

    [Fact] public void PrefaceIsOptionalAndDoesNotChangeProblemCount()
    {
        Write();
        Assert.Null(FrontMatterLoader.LoadPreface(root));
        WritePreface("# Önsöz\n\nProgramlama düşüncesini geliştirin.\n\n**Prof. Dr. Zafer CÖMERT**\n");
        Assert.Single(LessonLoader.Load(root));
        var doc = Compile();
        var paragraphs = doc.Descendants(W + "p").ToList();
        int preface = paragraphs.FindIndex(p => string.Concat(p.Descendants(W + "t").Select(x => x.Value)) == "Önsöz");
        int index = paragraphs.FindIndex(p => string.Concat(p.Descendants(W + "t").Select(x => x.Value)) == "İçindekiler");
        int problem = paragraphs.FindIndex(p => string.Concat(p.Descendants(W + "t").Select(x => x.Value)) == "001  Örnek problem");
        Assert.True(preface > 0 && preface < index && index < problem);
        Assert.Equal("Heading1", paragraphs[preface].Element(W + "pPr")?.Element(W + "pStyle")?.Attribute(W + "val")?.Value);
        Assert.Contains(doc.Descendants(W + "hyperlink"), h => h.Attribute(W + "anchor")?.Value == "Onsoz");
        Assert.Single(doc.Descendants(W + "bookmarkStart"), b => b.Attribute(W + "name")?.Value == "P001");
        Assert.Contains("1 çözümlü problem", Text(doc));
        using var package = WordprocessingDocument.Open(Output, false);
        Assert.Empty(new OpenXmlValidator().Validate(package));
    }

    [Fact] public void PrefaceMarkdownEditIsIncludedOnRebuild()
    {
        Write(); WritePreface("# Önsöz\n\nİlk metin.\n"); Compile();
        WritePreface("# Önsöz\n\nGüncel metin.\n");
        var doc = Compile(); Assert.Contains("Güncel metin.", Text(doc)); Assert.DoesNotContain("İlk metin.", Text(doc));
    }

    [Theory]
    [InlineData("")]
    [InlineData("# Önsöz\n")]
    [InlineData("# Başka başlık\n\nMetin.\n")]
    [InlineData("# Önsöz\n\n# İkinci başlık\n\nMetin.\n")]
    [InlineData("# Önsöz\n\n![Resim](../assets/existing.png)\n")]
    public void InvalidPrefaceIsRejectedBeforeOutputIsReplaced(string source)
    {
        Write(); Compile(); var before = File.ReadAllBytes(Output);
        WritePreface(source);
        Assert.Throws<FormatException>(() => Compile());
        Assert.Equal(before, File.ReadAllBytes(Output));
    }

    [Fact] public void ChaptersGroupTenLessonsAndUseEvenPageSectionsWithContinuousNumbering()
    {
        for (var i = 1; i <= 20; i++) Write(i.ToString("000"), i);
        File.WriteAllText(Path.Combine(root, "chapters.md"), "# Bölüm 1: Temeller\n# Bölüm 2: Kararlar\n");
        var chapters = ChapterLoader.Load(root, LessonLoader.Load(root));
        Assert.Equal(new[] { "001", "011" }, chapters.Select(c => c.Lessons[0].Metadata.Id));
        Assert.All(chapters, c => Assert.Equal(10, c.Lessons.Count));
        var doc = Compile();
        Assert.Equal(new[] { "nextPage", "evenPage", "evenPage" },
            doc.Descendants(W + "sectPr").Select(s => s.Element(W + "type")!.Attribute(W + "val")!.Value));
        Assert.Empty(doc.Descendants(W + "pgNumType"));
        var problemTitles = doc.Descendants(W + "p").Where(p => p.Element(W + "pPr")?.Element(W + "pStyle")?.Attribute(W + "val")?.Value == "ProblemHeading").ToArray();
        Assert.Equal(20, problemTitles.Length);
        Assert.Null(problemTitles[0].Element(W + "pPr")!.Element(W + "pageBreakBefore"));
        Assert.Null(problemTitles[10].Element(W + "pPr")!.Element(W + "pageBreakBefore"));
        Assert.All(problemTitles.Where((_, i) => i % 10 != 0), p => Assert.NotNull(p.Element(W + "pPr")!.Element(W + "pageBreakBefore")));
        Assert.Contains(doc.Descendants(W + "hyperlink"), p => p.Attribute(W + "anchor")?.Value == "C02");
        using var package = WordprocessingDocument.Open(Output, false);
        Assert.Empty(new OpenXmlValidator().Validate(package));
        var styles = package.MainDocumentPart!.StyleDefinitionsPart!.Styles!;
        Assert.Equal(DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Both,
            styles.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>().Single(s => s.StyleId == "Normal").StyleParagraphProperties!.Justification!.Val!.Value);
        Assert.All(new[] { "Code", "TableText", "ProblemHeading" }, id =>
            Assert.Equal(DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Left,
                styles.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>().Single(s => s.StyleId == id).StyleParagraphProperties!.Justification!.Val!.Value));
        Assert.All(doc.Descendants(W + "p").Where(p => p.Element(W + "pPr")?.Element(W + "sectPr") is not null),
            p => Assert.Empty(p.Descendants(W + "t")));
        Assert.Equal(1, styles.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>().Single(s => s.StyleId == "ProblemHeading").StyleParagraphProperties!.OutlineLevel!.Val!.Value);
        Assert.Equal(2, styles.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>().Single(s => s.StyleId == "ProblemSection").StyleParagraphProperties!.OutlineLevel!.Val!.Value);
    }

    [Fact] public void ChapterTitleChangesAreLoadedOnRebuild()
    {
        for (var i = 1; i <= 10; i++) Write(i.ToString("000"), i);
        var path = Path.Combine(root, "chapters.md");
        File.WriteAllText(path, "# Bölüm 1: İlk başlık\n"); Compile();
        File.WriteAllText(path, "# Bölüm 1: Güncel başlık\n");
        var text = Text(Compile());
        Assert.Contains("Bölüm 1: Güncel başlık", text);
        Assert.DoesNotContain("İlk başlık", text);
    }

    [Theory]
    [InlineData("# Bölüm 2: Yanlış sıra\n", 10)]
    [InlineData("# Bölüm 1: Eksik problem\n", 9)]
    [InlineData("# Bölüm 1: Fazla problem\n", 11)]
    [InlineData("# Bölüm 1: İlk\n# Bölüm 1: Yinelenen\n", 20)]
    public void InvalidChapterGroupingDoesNotReplaceExistingBook(string source, int count)
    {
        for (var i = 1; i <= count; i++) Write(i.ToString("000"), i);
        Compile(); var before = File.ReadAllBytes(Output);
        File.WriteAllText(Path.Combine(root, "chapters.md"), source);
        Assert.Throws<FormatException>(() => Compile());
        Assert.Equal(before, File.ReadAllBytes(Output));
    }

    public void Dispose()
    {
        var full = Path.GetFullPath(root);
        if (full.StartsWith(Path.GetFullPath(Path.GetTempPath()), StringComparison.OrdinalIgnoreCase) &&
            Path.GetFileName(full).StartsWith("procedural-csharp-tests-", StringComparison.Ordinal))
            Directory.Delete(full, true);
    }
}
