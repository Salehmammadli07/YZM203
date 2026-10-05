using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using W = DocumentFormat.OpenXml.Wordprocessing;

namespace Procedural.WorkbookBuilder;

/// <summary>Writes the Markdown lessons as a field-aware, editable Word workbook.</summary>
public static class WordBookWriter
{
    public static void Write(IReadOnlyList<Lesson> lessons, string root, string output)
    {
        ArgumentNullException.ThrowIfNull(lessons);
        if (lessons.Count == 0)
            throw new ArgumentException("Derlenecek problem bulunamadı.", nameof(lessons));

        root = Path.GetFullPath(root);
        output = Path.GetFullPath(output);
        var preface = FrontMatterLoader.LoadPreface(root);
        var introduction = FrontMatterLoader.LoadIntroduction(root);
        var chapters = ChapterLoader.Load(root, lessons);
        var plans = lessons.Select(lesson => PrepareLesson(lesson, root)).ToList();
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        using var package = WordprocessingDocument.Create(output, WordprocessingDocumentType.Document);
        var main = package.AddMainDocumentPart();
        main.Document = new W.Document(new W.Body());
        Configure(package, main);
        var compiler = new Compiler(main, root, chapters.Count != 0);
        var body = main.Document.Body!;

        var coverPath = Path.Combine(root, "assets", "cover.png");
        if (File.Exists(coverPath)) WriteCover(main, body, coverPath);
        else
        {
            compiler.AddText(body, "Prosedürel Programlama Çalışma Kitabı", "Title");
            compiler.AddText(body, "C# ile nesneye yönelik programlamaya hazırlık", "Subtitle");
            compiler.AddText(body, $"MYAZ205\n{lessons.Count} çözümlü problem");
            compiler.AddText(body, "Bu kitap, nesneye yönelik programlamadan önce gerekli olan işlem sırası, değişken, koşul, döngü ve dizi becerilerini geliştirir. Her problem kendi girdileri, algoritması, çalışan C# çözümü ve kazanımlarıyla bağımsız olarak incelenebilir.");
            compiler.AddText(body, "Çalışma yöntemi", "FrontMatter");
            compiler.AddText(body, "Önce problemi okuyun ve örneklerin sonucunu tahmin edin. Algoritmayı kendi sözcüklerinizle yazdıktan sonra C# çözümünü çalıştırın. Sınır durumlarını deneyin ve alıştırmaları çözerek aynı fikri farklı girdilere uygulayın.");
            compiler.AddText(body, "Kodlar bir konsol uygulamasının Program.cs dosyasında ayrı ayrı çalıştırılır. Sınıf tasarımı gerektirmeyen üst düzey ifadeler kullanılır. Sayısal girdi doğrulamasını ilk çalışmada hazır bir kontrol olarak okuyabilir, temel işlemi öğrendikten sonra TryParse ve başarısız girdileri inceleyebilirsiniz. Ondalık girdi ve çıktı kullanan problemlerde nokta biçimi açıklanmıştır.");
        }

        if (preface is not null) compiler.WritePreface(body, preface);
        var contents = compiler.AddText(body, "İçindekiler", "FrontMatter");
        contents.ParagraphProperties!.Append(new W.PageBreakBefore());
        var indexEntries = chapters.Count == 0
            ? lessons.Select(lesson => ($"{lesson.Metadata.Id}  {lesson.Metadata.Title}", "P" + lesson.Metadata.Id))
            : chapters.SelectMany(chapter => new[] { (chapter.Heading, chapter.Anchor) }.Concat(
                chapter.Lessons.Select(lesson => ($"{lesson.Metadata.Id}  {lesson.Metadata.Title}", "P" + lesson.Metadata.Id))));
        var tocInstruction = chapters.Count == 0 ? " TOC \\o \"1-1\" \\h \\z \\u " : " TOC \\o \"1-2\" \\h \\z \\u ";
        if (introduction is not null)
            tocInstruction += " \\t \"Introduction Section,2,Introduction Subsection,3\" ";
        CachedIndex(body, tocInstruction,
            (preface is null ? Enumerable.Empty<(string, string)>() : new[] { (preface.Title, "Onsoz") })
                .Concat(introduction is null ? Enumerable.Empty<(string, string)>() :
                    new[] { (introduction.Title, "Giris") }.Concat(introduction.Document.OfType<HeadingBlock>()
                        .Skip(1).Select((heading, index) => (PlainText(heading.Inline!), $"Giris{index + 1:000}"))))
                .Concat(indexEntries));
        var captions = plans.SelectMany(plan => plan.Captions).ToList();
        if (captions.Count != 0)
        {
            var figures = compiler.AddText(body, "Şekiller", "FrontMatter");
            figures.ParagraphProperties!.Append(new W.PageBreakBefore());
            CachedIndex(body, " TOC \\h \\z \\c \"Şekil\" ",
                captions.Select((caption, index) => ($"Şekil {index + 1}  {caption}", $"F{index + 1:000}")));
        }
        if (introduction is not null) compiler.WriteIntroduction(body, introduction);
        var footer = main.AddNewPart<FooterPart>();
        var footerParagraph = new W.Paragraph(new W.ParagraphProperties(new W.Justification { Val = W.JustificationValues.Center }));
        AppendField(footerParagraph, " PAGE ", "1");
        footer.Footer = new W.Footer(footerParagraph);
        footer.Footer.Save();
        W.SectionProperties Section(bool even) => new(
            new W.FooterReference { Type = W.HeaderFooterValues.Default, Id = main.GetIdOfPart(footer) },
            new W.SectionType { Val = even ? W.SectionMarkValues.EvenPage : W.SectionMarkValues.NextPage },
            new W.PageSize { Width = 12240, Height = 15840 },
            new W.PageMargin { Top = 1080, Bottom = 1080, Left = 1224, Right = 1224, Header = 720, Footer = 720, Gutter = 0 });
        if (chapters.Count == 0)
            foreach (var plan in plans) compiler.WriteLesson(body, plan);
        else
            foreach (var chapter in chapters)
            {
                // sectPr describes the section ending here; the following chapter's
                // even-page start belongs to its own section properties.
                // Keep the section break off the last content paragraph: Word
                // otherwise stretches its last line when Normal is justified.
                var end = new W.Paragraph(new W.ParagraphProperties(
                    new W.SpacingBetweenLines { Before = "0", After = "0", Line = "1", LineRule = W.LineSpacingRuleValues.Exact },
                    new W.ParagraphMarkRunProperties(new W.FontSize { Val = "2" }),
                    Section(chapter.Number > 1)));
                body.Append(end);
                var heading = compiler.AddText(body, chapter.Heading, "Heading1");
                Bookmark(heading, chapter.Anchor, 3000 + chapter.Number);
                var group = plans.Skip((chapter.Number - 1) * 10).Take(10).ToArray();
                for (var i = 0; i < group.Length; i++) compiler.WriteLesson(body, group[i], i != 0);
            }
        var backCoverPath = Path.Combine(root, "assets", "back-cover.png");
        if (File.Exists(backCoverPath))
        {
            body.Append(new W.Paragraph(new W.ParagraphProperties(
                new W.SpacingBetweenLines { Before = "0", After = "0", Line = "1", LineRule = W.LineSpacingRuleValues.Exact },
                new W.ParagraphMarkRunProperties(new W.FontSize { Val = "2" }),
                Section(chapters.Count != 0))));
            WriteCover(main, body, backCoverPath, back: true);
        }
        else body.Append(Section(chapters.Count != 0));
        NormalizeProperties(main.Document);
        NormalizeProperties(main.StyleDefinitionsPart!.Styles!);
        NormalizeProperties(footer.Footer);
        main.StyleDefinitionsPart.Styles!.Save();
        footer.Footer.Save();
        main.Document.Save();
    }

    private static void WriteCover(MainDocumentPart main, W.Body body, string path, bool back = false)
    {
        // A4 matches the supplied cover; only this section has zero margins.
        const uint pageWidth = 11906, pageHeight = 16838;
        const long width = pageWidth * 635L, height = pageHeight * 635L;
        var part = main.AddImagePart(ImagePartType.Png);
        using (var stream = File.OpenRead(path)) part.FeedData(stream);
        var picture = new PIC.Picture(
            new PIC.NonVisualPictureProperties(
                new PIC.NonVisualDrawingProperties { Id = 0, Name = Path.GetFileName(path), Description = back ? "Çalışma kitabı arka kapağı" : "Prosedürel Programlama Çalışma Kitabı kapağı" },
                new PIC.NonVisualPictureDrawingProperties(new A.PictureLocks { NoChangeAspect = true })),
            new PIC.BlipFill(new A.Blip { Embed = main.GetIdOfPart(part), CompressionState = A.BlipCompressionValues.Print },
                new A.Stretch(new A.FillRectangle())),
            new PIC.ShapeProperties(new A.Transform2D(new A.Offset { X = 0, Y = 0 }, new A.Extents { Cx = width, Cy = height }),
                new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }));
        var anchor = new DW.Anchor(
            new DW.SimplePosition { X = 0, Y = 0 },
            new DW.HorizontalPosition(new DW.PositionOffset("0")) { RelativeFrom = DW.HorizontalRelativePositionValues.Page },
            new DW.VerticalPosition(new DW.PositionOffset("0")) { RelativeFrom = DW.VerticalRelativePositionValues.Page },
            new DW.Extent { Cx = width, Cy = height },
            new DW.EffectExtent { LeftEdge = 0, TopEdge = 0, RightEdge = 0, BottomEdge = 0 },
            new DW.WrapNone(),
            new DW.DocProperties { Id = back ? 100001U : 100000U, Name = back ? "Arka kapak" : "Kitap kapağı", Description = "C# Prosedürel Programlama Çalışma Kitabı — Prof. Dr. Zafer CÖMERT" },
            new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks { NoChangeAspect = true }),
            new A.Graphic(new A.GraphicData(picture) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
        {
            SimplePos = false, RelativeHeight = 0, BehindDoc = true, Locked = false,
            LayoutInCell = true, AllowOverlap = true,
            DistanceFromTop = 0, DistanceFromBottom = 0, DistanceFromLeft = 0, DistanceFromRight = 0
        };
        var coverSection = new W.SectionProperties(
            new W.SectionType { Val = W.SectionMarkValues.NextPage },
            new W.PageSize { Width = pageWidth, Height = pageHeight },
            new W.PageMargin { Top = 0, Bottom = 0, Left = 0, Right = 0, Header = 0, Footer = 0, Gutter = 0 });
        if (back)
        {
            // An explicit empty footer prevents inheriting the last chapter's PAGE field.
            var emptyFooter = main.AddNewPart<FooterPart>();
            emptyFooter.Footer = new W.Footer(new W.Paragraph());
            emptyFooter.Footer.Save();
            coverSection.PrependChild(new W.FooterReference { Type = W.HeaderFooterValues.Default, Id = main.GetIdOfPart(emptyFooter) });
        }
        var properties = new W.ParagraphProperties(
            new W.ParagraphStyleId { Val = "Title" },
            new W.SpacingBetweenLines { Before = "0", After = "0", Line = "1", LineRule = W.LineSpacingRuleValues.Exact },
            new W.ParagraphMarkRunProperties(new W.FontSize { Val = "2" }));
        if (!back) properties.Append(coverSection);
        body.Append(new W.Paragraph(properties, new W.Run(new W.Drawing(anchor))));
        if (back) body.Append(coverSection);
    }

    private static void NormalizeProperties(OpenXmlElement root)
    {
        foreach (var properties in root.Descendants<OpenXmlCompositeElement>().Where(x =>
                     x.LocalName is "pPr" or "rPr" or "tblPr" or "tcPr" or "trPr"))
        {
            var children = properties.ChildElements.ToArray();
            properties.RemoveAllChildren();
            foreach (var child in children) properties.AddChild(child, true);
        }
    }

    private sealed record DiagramFigure(string Path, string Caption);
    private sealed record LessonPlan(Lesson Lesson, Dictionary<FencedCodeBlock, DiagramFigure> Diagrams,
        HashSet<string> Generated, List<string> Captions);

    private static LessonPlan PrepareLesson(Lesson lesson, string root)
    {
        var diagrams = new Dictionary<FencedCodeBlock, DiagramFigure>();
        var generated = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var number = 0;
        foreach (var fence in DescendantBlocks(lesson.Document).OfType<FencedCodeBlock>().Where(IsDiagram))
        {
            var spec = DiagramRenderer.Parse(fence.Lines.ToString());
            DiagramRenderer.Validate(spec);
            var path = Path.GetFullPath(Path.Combine(root, "assets", "figures", $"{lesson.Metadata.Id}-{++number:00}.png"));
            DiagramRenderer.Render(spec, path);
            diagrams.Add(fence, new DiagramFigure(path, spec.Caption));
            generated.Add(path);
        }

        // Collect before rendering: a Markdown preview can occur before its diagram block.
        var captions = new List<string>();
        foreach (var block in DescendantBlocks(lesson.Document))
        {
            if (block is FencedCodeBlock fence && diagrams.TryGetValue(fence, out var diagram))
                captions.Add(diagram.Caption);
            if (block is LeafBlock { Inline: not null } leaf)
                foreach (var image in InlineImages(leaf.Inline))
                {
                    var path = LessonLoader.ResolveImage(lesson.Path, ImageUrl(image), root, generated);
                    if (!generated.Contains(path))
                        captions.Add(PlainText(image));
                }
        }
        return new LessonPlan(lesson, diagrams, generated, captions);
    }

    private static IEnumerable<Block> DescendantBlocks(ContainerBlock container)
    {
        foreach (var block in container)
        {
            yield return block;
            if (block is ContainerBlock child)
                foreach (var descendant in DescendantBlocks(child))
                    yield return descendant;
        }
    }

    private static bool IsDiagram(FencedCodeBlock fence) => fence.Info?.Trim() == "diagram";
    private static string ImageUrl(LinkInline image) => image.GetDynamicUrl?.Invoke() ?? image.Url ?? "";
    private static IEnumerable<LinkInline> InlineImages(ContainerInline container)
    {
        for (var item = container.FirstChild; item is not null; item = item.NextSibling)
        {
            if (item is LinkInline { IsImage: true } image)
                yield return image;
            else if (item is ContainerInline child)
                foreach (var imageChild in InlineImages(child))
                    yield return imageChild;
        }
    }

    private static string PlainText(ContainerInline container)
    {
        var text = new StringBuilder();
        for (var inline = container.FirstChild; inline is not null; inline = inline.NextSibling)
            switch (inline)
            {
                case LiteralInline literal: text.Append(literal.Content); break;
                case CodeInline code: text.Append(code.Content); break;
                case LineBreakInline: text.Append(' '); break;
                case ContainerInline child: text.Append(PlainText(child)); break;
            }
        return text.ToString();
    }

    private static W.RunProperties Font(string family = "Arial", string size = "22", bool bold = false, bool italic = false)
    {
        var properties = new W.RunProperties(new W.RunFonts { Ascii = family, HighAnsi = family, ComplexScript = family },
            new W.Color { Val = "000000" }, new W.FontSize { Val = size }, new W.FontSizeComplexScript { Val = size });
        if (bold) properties.Append(new W.Bold());
        if (italic) properties.Append(new W.Italic());
        return properties;
    }

    private static void Configure(WordprocessingDocument package, MainDocumentPart main)
    {
        var stylesPart = main.AddNewPart<StyleDefinitionsPart>();
        var styles = new W.Styles();
        styles.Append(new W.DocDefaults(
            new W.RunPropertiesDefault(new W.RunPropertiesBaseStyle(
                new W.RunFonts { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                new W.Color { Val = "000000" }, new W.FontSize { Val = "22" }, new W.FontSizeComplexScript { Val = "22" })),
            new W.ParagraphPropertiesDefault(new W.ParagraphPropertiesBaseStyle(
                new W.SpacingBetweenLines { After = "80", Line = "252", LineRule = W.LineSpacingRuleValues.Auto }))));
        styles.Append(Style("Normal", "Normal", "22", false));
        styles.Append(Style("TableText", "Table Text", "22", false));
        styles.Append(Style("Title", "Title", "56", false, after: 240));
        styles.Append(Style("Subtitle", "Subtitle", "28", false, after: 240));
        styles.Append(Style("Heading1", "heading 1", "38", true, 0, 240, 160));
        styles.Append(Style("Heading2", "heading 2", "26", true, 1, 180, 100));
        styles.Append(Style("Heading3", "heading 3", "22", true, 2, 140, 80));
        styles.Append(Style("ProblemHeading", "Problem Heading", "38", true, 1, 240, 160));
        styles.Append(Style("ProblemSection", "Problem Section", "26", true, 2, 180, 100));
        styles.Append(Style("ProblemSubsection", "Problem Subsection", "22", true, 3, 140, 80));
        styles.Append(Style("Caption", "caption", "20", false, after: 100));
        styles.Append(Style("FrontMatter", "Front Matter", "36", true, after: 240));
        styles.Append(Style("IntroductionSection", "Introduction Section", "26", true, 1, 180, 100));
        styles.Append(Style("IntroductionSubsection", "Introduction Subsection", "22", true, 2, 140, 80));
        var code = Style("Code", "Code", "19", false, after: 0);
        code.StyleRunProperties = new W.StyleRunProperties(new W.RunFonts { Ascii = "Consolas", HighAnsi = "Consolas", ComplexScript = "Consolas" },
            new W.Color { Val = "000000" }, new W.FontSize { Val = "19" }, new W.FontSizeComplexScript { Val = "19" });
        code.StyleParagraphProperties!.Append(new W.Indentation { Left = "144" });
        code.StyleParagraphProperties.SpacingBetweenLines!.Line = "240";
        styles.Append(code);
        var toc = Style("TOC1", "toc 1", "22", false, after: 60);
        toc.StyleParagraphProperties!.Append(new W.Tabs(new W.TabStop { Val = W.TabStopValues.Right, Leader = W.TabStopLeaderCharValues.Dot, Position = 9792 }));
        styles.Append(toc);
        var toc2 = Style("TOC2", "toc 2", "22", false, after: 60);
        toc2.StyleParagraphProperties!.Append(new W.Indentation { Left = "220" },
            new W.Tabs(new W.TabStop { Val = W.TabStopValues.Right, Leader = W.TabStopLeaderCharValues.Dot, Position = 9792 }));
        styles.Append(toc2);
        stylesPart.Styles = styles;
        stylesPart.Styles.Save();
        var settings = main.AddNewPart<DocumentSettingsPart>();
        settings.Settings = new W.Settings(new W.UpdateFieldsOnOpen { Val = true });
        settings.Settings.Save();
        package.PackageProperties.Title = "Prosedürel Programlama Çalışma Kitabı";
        package.PackageProperties.Subject = "C# ile nesneye yönelik programlamaya hazırlık";
        package.PackageProperties.Creator = "MYAZ205";
    }

    private static W.Style Style(string id, string name, string size, bool bold, int? outline = null, int before = 0, int after = 80)
    {
        var paragraph = new W.StyleParagraphProperties(new W.SpacingBetweenLines { Before = before.ToString(CultureInfo.InvariantCulture),
            After = after.ToString(CultureInfo.InvariantCulture), Line = "252", LineRule = W.LineSpacingRuleValues.Auto });
        paragraph.Append(new W.Justification { Val = id == "Normal" ? W.JustificationValues.Both : W.JustificationValues.Left });
        if (outline.HasValue) paragraph.Append(new W.KeepNext(), new W.KeepLines(), new W.OutlineLevel { Val = outline.Value });
        if (id is "Title" or "FrontMatter") paragraph.Append(new W.KeepNext());
        var run = new W.StyleRunProperties(new W.RunFonts { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
            new W.Color { Val = "000000" }, new W.FontSize { Val = size }, new W.FontSizeComplexScript { Val = size });
        if (bold) run.Append(new W.Bold());
        var style = new W.Style { Type = W.StyleValues.Paragraph, StyleId = id, Default = id == "Normal" };
        style.Append(new W.StyleName { Val = name });
        if (id != "Normal") style.Append(new W.BasedOn { Val = "Normal" });
        style.Append(new W.NextParagraphStyle { Val = "Normal" }, new W.PrimaryStyle(), paragraph, run);
        return style;
    }

    private static void Bookmark(W.Paragraph paragraph, string name, int id)
    {
        var idText = id.ToString(CultureInfo.InvariantCulture);
        // ParagraphProperties must remain the first child of a paragraph.
        var start = new W.BookmarkStart { Name = name, Id = idText };
        if (paragraph.ParagraphProperties is not null) paragraph.InsertAfter(start, paragraph.ParagraphProperties);
        else paragraph.PrependChild(start);
        paragraph.Append(new W.BookmarkEnd { Id = idText });
    }

    private static void AppendField(OpenXmlCompositeElement target, string instruction, string cached)
    {
        target.Append(new W.Run(new W.FieldChar { FieldCharType = W.FieldCharValues.Begin }));
        target.Append(new W.Run(new W.FieldCode(instruction) { Space = SpaceProcessingModeValues.Preserve }));
        target.Append(new W.Run(new W.FieldChar { FieldCharType = W.FieldCharValues.Separate }));
        target.Append(new W.Run(new W.Text(cached) { Space = SpaceProcessingModeValues.Preserve }));
        target.Append(new W.Run(new W.FieldChar { FieldCharType = W.FieldCharValues.End }));
    }

    private static void CachedIndex(W.Body body, string instruction, IEnumerable<(string Text, string Anchor)> entries)
    {
        var first = new W.Paragraph(new W.ParagraphProperties(new W.ParagraphStyleId { Val = "TOC1" }));
        body.Append(first);
        first.Append(new W.Run(new W.FieldChar { FieldCharType = W.FieldCharValues.Begin }),
            new W.Run(new W.FieldCode(instruction) { Space = SpaceProcessingModeValues.Preserve }),
            new W.Run(new W.FieldChar { FieldCharType = W.FieldCharValues.Separate }));
        var current = first;
        var index = 0;
        foreach (var entry in entries)
        {
            if (index++ != 0)
            {
                current = new W.Paragraph(new W.ParagraphProperties(new W.ParagraphStyleId { Val = "TOC1" }));
                body.Append(current);
            }
            var hyperlink = new W.Hyperlink { Anchor = entry.Anchor, History = true };
            hyperlink.Append(new W.Run(Font(), new W.Text(entry.Text) { Space = SpaceProcessingModeValues.Preserve }), new W.Run(new W.TabChar()));
            AppendField(hyperlink, $" PAGEREF {entry.Anchor} \\h ", "");
            current.Append(hyperlink);
        }
        current.Append(new W.Run(new W.FieldChar { FieldCharType = W.FieldCharValues.End }));
    }

    private sealed class ListContext(int depth, string prefix)
    {
        public int Depth { get; } = depth;
        public string Prefix { get; set; } = prefix;
        public List<W.Paragraph> Paragraphs { get; } = [];
    }

    private readonly record struct InlineFormat(bool Bold = false, bool Italic = false, bool Code = false, string Size = "22");

    private sealed class Compiler(MainDocumentPart main, string root, bool chapterLayout)
    {
        private uint drawingNumber;
        private int figureNumber;
        private LessonPlan? plan;

        public W.Paragraph AddText(OpenXmlCompositeElement parent, string text, string style = "Normal")
        {
            var paragraph = new W.Paragraph(new W.ParagraphProperties(new W.ParagraphStyleId { Val = style }));
            parent.Append(paragraph);
            var size = style switch
            {
                "Title" => "56", "Subtitle" => "28", "Heading1" or "ProblemHeading" => "38",
                "Heading2" or "ProblemSection" => "26", "FrontMatter" => "36", "Caption" => "20", _ => "22"
            };
            AppendText(paragraph, text, new InlineFormat(Size: size));
            return paragraph;
        }

        public void WritePreface(W.Body body, FrontMatterSection section)
        {
            plan = null;
            var heading = AddText(body, section.Title, "Heading1");
            heading.ParagraphProperties!.Append(new W.PageBreakBefore());
            Bookmark(heading, "Onsoz", 2001);
            foreach (var block in section.Document)
                RenderBlock(body, block, null, 6.8);
        }

        public void WriteIntroduction(W.Body body, FrontMatterSection section)
        {
            plan = null;
            var title = AddText(body, section.Title, "Heading1");
            title.ParagraphProperties!.Append(new W.PageBreakBefore());
            Bookmark(title, "Giris", 2100);
            var index = 0;
            foreach (var block in section.Document.Skip(1))
            {
                if (block is HeadingBlock heading)
                {
                    var paragraph = NewParagraph(body,
                        heading.Level == 2 ? "IntroductionSection" : "IntroductionSubsection", null);
                    RenderInline(paragraph, heading.Inline!, new InlineFormat(Size: heading.Level == 2 ? "26" : "22"));
                    Bookmark(paragraph, $"Giris{++index:000}", 2100 + index);
                }
                else RenderBlock(body, block, null, 6.8);
            }
        }

        public void WriteLesson(W.Body body, LessonPlan lessonPlan, bool pageBreak = true)
        {
            plan = lessonPlan;
            var metadata = plan.Lesson.Metadata;
            var heading = AddText(body, $"{metadata.Id}  {metadata.Title}", chapterLayout ? "ProblemHeading" : "Heading1");
            if (pageBreak) heading.ParagraphProperties!.Append(new W.PageBreakBefore());
            Bookmark(heading, "P" + metadata.Id, int.Parse(metadata.Id, CultureInfo.InvariantCulture));
            AddText(body, $"Düzey: {metadata.Level}    Önkoşullar: {(metadata.Prerequisites.Count == 0 ? "Yok" : string.Join(", ", metadata.Prerequisites))}");
            AddText(body, "Kavramlar: " + string.Join(", ", metadata.Concepts));
            foreach (var block in plan.Lesson.Document)
                RenderBlock(body, block, null, 6.8);
        }

        private void RenderBlock(OpenXmlCompositeElement parent, Block block, ListContext? list, double width)
        {
            switch (block)
            {
                case HeadingBlock heading when heading.Level == 1: break;
                case HeadingBlock heading:
                {
                    var style = chapterLayout && plan is not null
                        ? (heading.Level == 2 ? "ProblemSection" : "ProblemSubsection")
                        : $"Heading{Math.Min(heading.Level, 3)}";
                    var paragraph = NewParagraph(parent, style, list);
                    if (heading.Inline is not null) RenderInline(paragraph, heading.Inline,
                        new InlineFormat(Size: heading.Level == 2 ? "26" : "22"));
                    RenderPictures(parent, heading.Inline, width);
                    break;
                }
                case ParagraphBlock paragraphBlock:
                    RenderParagraph(parent, paragraphBlock, list, width);
                    break;
                case FencedCodeBlock fence when IsDiagram(fence):
                {
                    var diagram = plan!.Diagrams[fence];
                    AddPicture(parent, diagram.Path, diagram.Caption, width);
                    break;
                }
                case CodeBlock code:
                    RenderCode(parent, code, list);
                    break;
                case ListBlock listBlock:
                    RenderList(parent, listBlock, list?.Depth + 1 ?? 1, width);
                    break;
                case Table table:
                    RenderTable(parent, table, width);
                    break;
                case QuoteBlock quote:
                    foreach (var child in quote) RenderBlock(parent, child, list, width);
                    break;
                case ThematicBreakBlock:
                    AddText(parent, "—");
                    break;
                default:
                    throw new InvalidDataException($"Desteklenmeyen Markdown bloğu: {block.GetType().Name}");
            }
        }

        private W.Paragraph NewParagraph(OpenXmlCompositeElement parent, string style, ListContext? list)
        {
            var properties = new W.ParagraphProperties(new W.ParagraphStyleId { Val = style });
            var paragraph = new W.Paragraph(properties);
            parent.Append(paragraph);
            if (list is not null)
            {
                properties.Append(new W.Indentation { Left = (403 + (list.Depth - 1) * 230).ToString(CultureInfo.InvariantCulture), Hanging = "288" },
                    new W.SpacingBetweenLines { After = "60" }, new W.KeepLines());
                if (list.Prefix.Length != 0)
                {
                    AppendText(paragraph, list.Prefix, new InlineFormat(Size: "22"));
                    list.Prefix = "";
                }
                list.Paragraphs.Add(paragraph);
            }
            return paragraph;
        }

        private void RenderParagraph(OpenXmlCompositeElement parent, ParagraphBlock block, ListContext? list, double width, string size = "22", bool bold = false)
        {
            var paragraph = NewParagraph(parent, parent is W.TableCell ? "TableText" : "Normal", list);
            if (block.Inline is not null) RenderInline(paragraph, block.Inline, new InlineFormat(Bold: bold, Size: size));
            if (!paragraph.Descendants<W.Text>().Any() && !paragraph.Descendants<W.Break>().Any())
                paragraph.Remove();
            RenderPictures(parent, block.Inline, width);
        }

        private void RenderPictures(OpenXmlCompositeElement parent, ContainerInline? inline, double width)
        {
            if (inline is null) return;
            foreach (var image in InlineImages(inline))
            {
                var path = LessonLoader.ResolveImage(plan!.Lesson.Path, ImageUrl(image), root, plan.Generated);
                if (!plan.Generated.Contains(path)) AddPicture(parent, path, PlainText(image), width);
            }
        }

        private void RenderCode(OpenXmlCompositeElement parent, CodeBlock code, ListContext? list)
        {
            if (parent.LastChild is W.Paragraph introduction)
            {
                introduction.ParagraphProperties ??= new W.ParagraphProperties();
                introduction.ParagraphProperties.KeepNext = new W.KeepNext();
            }
            var content = code.Lines.ToString().TrimEnd('\r', '\n');
            var lines = content.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
            var keepNext = new bool[lines.Length];
            // Keep short logical blocks together when a complete solution spans pages.
            for (var start = 0; start < lines.Length;)
            {
                if (string.IsNullOrWhiteSpace(lines[start])) { start++; continue; }
                var end = start;
                while (end + 1 < lines.Length && !string.IsNullOrWhiteSpace(lines[end + 1])) end++;
                if (end - start < 48)
                    for (var line = start; line < end; line++) keepNext[line] = true;
                start = end + 1;
            }
            for (var i = 0; i < lines.Length; i++)
            {
                var paragraph = NewParagraph(parent, "Code", list);
                if (i < lines.Length - 1 && (lines.Length <= 48 || keepNext[i] || i < 10 || i >= lines.Length - 4))
                    paragraph.ParagraphProperties!.Append(new W.KeepNext());
                paragraph.ParagraphProperties!.Append(new W.KeepLines());
                AppendText(paragraph, lines[i], new InlineFormat(Code: true, Size: "19"));
            }
            var spacer = NewParagraph(parent, "Normal", null);
            spacer.ParagraphProperties!.Append(new W.SpacingBetweenLines { After = "0", Before = "0", Line = "60", LineRule = W.LineSpacingRuleValues.Exact });
            spacer.Append(new W.Run(Font(size: "2"), new W.Text("")));
        }

        private void RenderList(OpenXmlCompositeElement parent, ListBlock block, int depth, double width)
        {
            var start = 1;
            if (block.IsOrdered && !int.TryParse(Convert.ToString(block.OrderedStart, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out start))
                start = 1;
            var paragraphs = new List<W.Paragraph>();
            foreach (var item in block.OfType<ListItemBlock>())
            {
                var context = new ListContext(depth, block.IsOrdered ? $"{start++}. " : "• ");
                foreach (var child in item) RenderBlock(parent, child, context, width);
                paragraphs.AddRange(context.Paragraphs.Where(paragraph => paragraph.Parent is not null));
            }
            if (paragraphs.Count <= 12)
                for (var i = 0; i < paragraphs.Count - 1; i++)
                    if (paragraphs[i].ParagraphProperties!.KeepNext is null) paragraphs[i].ParagraphProperties!.Append(new W.KeepNext());
            if (paragraphs.Count != 0) paragraphs[^1].ParagraphProperties!.KeepNext?.Remove();
        }

        private void RenderTable(OpenXmlCompositeElement parent, Table markdown, double availableWidth)
        {
            var rows = markdown.OfType<TableRow>().ToList();
            if (rows.Count == 0) return;
            var columns = rows.Max(row => row.Count);
            var widths = columns == 3 ? new[] { availableWidth * 1.25 / 6.8, availableWidth * 1.25 / 6.8, availableWidth * 4.3 / 6.8 }
                : Enumerable.Repeat(availableWidth / columns, columns).ToArray();
            var headers = rows[0].OfType<TableCell>().Select(TableCellText).ToArray();
            var resultColumn = Array.FindIndex(headers, text =>
                text.StartsWith("Sonuç", StringComparison.Ordinal) ||
                text.Equals("Tam sonuç", StringComparison.Ordinal) ||
                text.Equals("Hata çıktısı", StringComparison.Ordinal));
            if (columns == 3 && resultColumn == 1)
                widths = new[] { availableWidth * 1.4 / 6.8, availableWidth * 2.8 / 6.8, availableWidth * 2.6 / 6.8 };
            if (columns == 4 && resultColumn == 3)
            {
                widths = new[] { availableWidth * .95 / 6.8, availableWidth * .95 / 6.8,
                    availableWidth * .95 / 6.8, availableWidth * 3.95 / 6.8 };
                // Preserve a long numeric input as one token, such as a small decimal divisor.
                if (rows.Any(row => row.Count > 1 && row[1] is TableCell cell &&
                        TableCellText(cell).Split(' ', StringSplitOptions.RemoveEmptyEntries).Any(token => token.Length >= 24)))
                    widths = new[] { availableWidth * .8 / 6.8, availableWidth * 2.4 / 6.8,
                        availableWidth * .65 / 6.8, availableWidth * 2.95 / 6.8 };
            }
            var table = new W.Table();
            var borders = new W.TableBorders(new W.TopBorder { Val = W.BorderValues.Single, Size = 4, Color = "D9D9D9" },
                new W.LeftBorder { Val = W.BorderValues.Single, Size = 4, Color = "D9D9D9" },
                new W.BottomBorder { Val = W.BorderValues.Single, Size = 4, Color = "D9D9D9" },
                new W.RightBorder { Val = W.BorderValues.Single, Size = 4, Color = "D9D9D9" },
                new W.InsideHorizontalBorder { Val = W.BorderValues.Single, Size = 4, Color = "D9D9D9" },
                new W.InsideVerticalBorder { Val = W.BorderValues.Single, Size = 4, Color = "D9D9D9" });
            table.Append(new W.TableProperties(new W.TableWidth { Type = W.TableWidthUnitValues.Dxa, Width = InchesTwips(availableWidth) },
                new W.TableJustification { Val = W.TableRowAlignmentValues.Center }, borders,
                new W.TableLayout { Type = W.TableLayoutValues.Fixed }));
            table.Append(new W.TableGrid(widths.Select(width => new W.GridColumn { Width = InchesTwips(width) })));
            if (parent.LastChild is W.Paragraph introduction)
            {
                introduction.ParagraphProperties ??= new W.ParagraphProperties();
                introduction.ParagraphProperties.KeepNext = new W.KeepNext();
            }
            parent.Append(table);
            for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var source = rows[rowIndex];
                var rowProperties = new W.TableRowProperties(new W.CantSplit());
                if (source.IsHeader) rowProperties.Append(new W.TableHeader());
                var row = new W.TableRow(rowProperties);
                table.Append(row);
                for (var column = 0; column < columns; column++)
                {
                    var cell = new W.TableCell(new W.TableCellProperties(
                        new W.TableCellWidth { Type = W.TableWidthUnitValues.Dxa, Width = InchesTwips(widths[column]) },
                        new W.Shading { Val = W.ShadingPatternValues.Clear, Fill = source.IsHeader ? "D9E2F3" : rowIndex % 2 == 0 ? "F5F7FA" : "FFFFFF" },
                        new W.TableCellMargin(new W.TopMargin { Width = "60", Type = W.TableWidthUnitValues.Dxa },
                            new W.LeftMargin { Width = "60", Type = W.TableWidthUnitValues.Dxa },
                            new W.BottomMargin { Width = "60", Type = W.TableWidthUnitValues.Dxa },
                            new W.RightMargin { Width = "60", Type = W.TableWidthUnitValues.Dxa }),
                        new W.TableCellVerticalAlignment { Val = W.TableVerticalAlignmentValues.Center }));
                    row.Append(cell);
                    if (column < source.Count && source[column] is TableCell sourceCell)
                        foreach (var child in sourceCell)
                        {
                            if (child is ParagraphBlock paragraph) RenderParagraph(cell, paragraph, null, widths[column] - .1, "20", source.IsHeader);
                            else RenderBlock(cell, child, null, widths[column] - .1);
                        }
                    if (cell.LastChild is not W.Paragraph) cell.Append(new W.Paragraph());
                    foreach (var paragraph in cell.Elements<W.Paragraph>())
                    {
                        paragraph.ParagraphProperties ??= new W.ParagraphProperties();
                        paragraph.ParagraphProperties.SpacingBetweenLines = new W.SpacingBetweenLines { Before = "20", After = "20" };
                        if (source.IsHeader || (plan is null && rows.Count <= 10 && rowIndex < rows.Count - 1))
                            paragraph.ParagraphProperties.KeepNext = new W.KeepNext();
                    }
                }
            }
            var spacer = NewParagraph(parent, "Normal", null);
            spacer.ParagraphProperties!.Append(new W.SpacingBetweenLines { After = "0" });
        }

        private static string TableCellText(TableCell cell) => string.Join(" ",
            cell.OfType<LeafBlock>().Where(block => block.Inline is not null).Select(block => PlainText(block.Inline!)));

        private static string InchesTwips(double width) => Math.Round(width * 1440).ToString(CultureInfo.InvariantCulture);

        private void RenderInline(OpenXmlCompositeElement parent, ContainerInline container, InlineFormat format)
        {
            var htmlBold = false;
            for (var inline = container.FirstChild; inline is not null; inline = inline.NextSibling)
            {
                var current = format with { Bold = format.Bold || htmlBold };
                switch (inline)
                {
                    case LiteralInline literal: AppendText(parent, literal.Content.ToString(), current); break;
                    case CodeInline code: AppendText(parent, code.Content, current with { Code = true }); break;
                    case LineBreakInline: parent.Append(new W.Run(new W.Break())); break;
                    case EmphasisInline emphasis:
                        RenderInline(parent, emphasis, emphasis.DelimiterCount >= 2 ? current with { Bold = true } : current with { Italic = true });
                        break;
                    case LinkInline { IsImage: true }: break;
                    case LinkInline link:
                    {
                        var url = link.GetDynamicUrl?.Invoke() ?? link.Url ?? "";
                        var hyperlink = new W.Hyperlink { History = true };
                        if (url.StartsWith('#')) hyperlink.Anchor = url[1..];
                        else hyperlink.Id = main.AddHyperlinkRelationship(new Uri(url, UriKind.RelativeOrAbsolute), true).Id;
                        RenderInline(hyperlink, link, current);
                        parent.Append(hyperlink);
                        break;
                    }
                    case HtmlInline html when Regex.IsMatch(html.Tag, @"^<br\s*/?>$", RegexOptions.IgnoreCase):
                        parent.Append(new W.Run(new W.Break()));
                        break;
                    case HtmlInline html when html.Tag.Equals("<b>", StringComparison.OrdinalIgnoreCase): htmlBold = true; break;
                    case HtmlInline html when html.Tag.Equals("</b>", StringComparison.OrdinalIgnoreCase): htmlBold = false; break;
                    case ContainerInline child: RenderInline(parent, child, current); break;
                    default: throw new InvalidDataException($"Desteklenmeyen Markdown satır içi öğesi: {inline.GetType().Name}");
                }
            }
        }

        private static void AppendText(OpenXmlCompositeElement parent, string text, InlineFormat format)
        {
            var run = new W.Run(Font(format.Code ? "Consolas" : "Arial", format.Size, format.Bold, format.Italic));
            var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                if (i != 0) run.Append(new W.Break());
                run.Append(new W.Text(lines[i]) { Space = SpaceProcessingModeValues.Preserve });
            }
            parent.Append(run);
        }

        private void AddPicture(OpenXmlCompositeElement parent, string path, string caption, double availableWidth)
        {
            using var image = System.Drawing.Image.FromFile(path);
            var width = Math.Min(Math.Min(6.4, availableWidth), 5.1 * image.Width / image.Height);
            var height = width * image.Height / image.Width;
            var extentWidth = (long)Math.Round(width * 914400);
            var extentHeight = (long)Math.Round(height * 914400);
            var extension = Path.GetExtension(path).ToLowerInvariant();
            var part = main.AddImagePart(extension switch
            {
                ".png" => ImagePartType.Png,
                ".jpg" or ".jpeg" => ImagePartType.Jpeg,
                _ => throw new InvalidDataException($"Word için resim biçimi desteklenmiyor: {extension}")
            });
            using (var stream = File.OpenRead(path)) part.FeedData(stream);
            var number = ++drawingNumber;
            var picture = new PIC.Picture(
                new PIC.NonVisualPictureProperties(new PIC.NonVisualDrawingProperties { Id = 0, Name = Path.GetFileName(path), Description = caption },
                    new PIC.NonVisualPictureDrawingProperties(new A.PictureLocks { NoChangeAspect = true })),
                new PIC.BlipFill(new A.Blip { Embed = main.GetIdOfPart(part), CompressionState = A.BlipCompressionValues.Print },
                    new A.Stretch(new A.FillRectangle())),
                new PIC.ShapeProperties(new A.Transform2D(new A.Offset { X = 0, Y = 0 }, new A.Extents { Cx = extentWidth, Cy = extentHeight }),
                    new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }));
            var inline = new DW.Inline(
                new DW.Extent { Cx = extentWidth, Cy = extentHeight },
                new DW.EffectExtent { LeftEdge = 0, TopEdge = 0, RightEdge = 0, BottomEdge = 0 },
                new DW.DocProperties { Id = number, Name = $"Şekil {number}", Description = caption },
                new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks { NoChangeAspect = true }),
                new A.Graphic(new A.GraphicData(picture) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
            { DistanceFromTop = 0, DistanceFromBottom = 0, DistanceFromLeft = 0, DistanceFromRight = 0 };
            var paragraph = new W.Paragraph(new W.ParagraphProperties(new W.Justification { Val = W.JustificationValues.Center }, new W.KeepNext()),
                new W.Run(new W.Drawing(inline)));
            parent.Append(paragraph);
            var captionParagraph = new W.Paragraph(new W.ParagraphProperties(new W.ParagraphStyleId { Val = "Caption" },
                new W.Justification { Val = W.JustificationValues.Center }, new W.KeepLines()));
            parent.Append(captionParagraph);
            figureNumber++;
            AppendText(captionParagraph, "Şekil ", new InlineFormat(Size: "20"));
            AppendField(captionParagraph, " SEQ Şekil \\* ARABIC ", figureNumber.ToString(CultureInfo.InvariantCulture));
            AppendText(captionParagraph, "  " + caption, new InlineFormat(Size: "20"));
            Bookmark(captionParagraph, $"F{figureNumber:000}", 1000 + figureNumber);
        }
    }
}
