using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Procedural.WorkbookBuilder;

public sealed record DiagramPoint(double X, double Y);

public sealed record DiagramNode
{
    public string Id { get; init; } = "";
    public string Text { get; init; } = "";
    public string Kind { get; init; } = "";
    public double X { get; init; }
    public double Y { get; init; }
}

public sealed record DiagramEdge
{
    public string From { get; init; } = "";
    public string To { get; init; } = "";
    public string? Label { get; init; }
    public List<DiagramPoint> Via { get; init; } = [];
    public DiagramPoint? LabelAt { get; init; }
}

public sealed record DiagramSpec
{
    public string Caption { get; init; } = "";
    public List<DiagramNode> Nodes { get; init; } = [];
    public List<DiagramEdge> Edges { get; init; } = [];
}

/// <summary>
/// Renders author-positioned JSON flowcharts. Coordinates and intermediate route
/// points use a 240 by 130 pixel grid; figure captions are handled by the writer.
/// </summary>
public static class DiagramRenderer
{
    private const double GridX = 240;
    private const double GridY = 130;
    private const double Padding = 90;
    private const int Scale = 2;
    private const double Epsilon = 1e-8;
    private static readonly Color Ink = ColorTranslator.FromHtml("#233C52");
    private static readonly Dictionary<string, Shape> Shapes = new(StringComparer.Ordinal)
    {
        ["terminal"] = new(150, 60, 124, 44, ColorTranslator.FromHtml("#E4EFF8")),
        ["process"] = new(220, 80, 194, 64, ColorTranslator.FromHtml("#F5F8FC")),
        ["decision"] = new(220, 110, 110, 58, ColorTranslator.FromHtml("#FFF0D2")),
        ["io"] = new(220, 80, 168, 64, ColorTranslator.FromHtml("#E8F4F0"))
    };

    public static DiagramSpec Parse(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        try
        {
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = RequireObject(document.RootElement, "Diagram");
            string caption = RequiredText(root, "caption", "Diagram caption");
            JsonElement nodeElements = Required(root, "nodes", "Diagram");
            if (nodeElements.ValueKind != JsonValueKind.Array)
                throw new FormatException("Diagram nodes must be a non-empty list.");
            List<DiagramNode> nodes = [];
            foreach (JsonElement item in nodeElements.EnumerateArray())
            {
                string context = $"Node {nodes.Count + 1}";
                JsonElement node = RequireObject(item, context);
                string id = RequiredText(node, "id", context + " id");
                nodes.Add(new DiagramNode
                {
                    Id = id,
                    Text = RequiredText(node, "text", $"Node {id} text"),
                    Kind = RequiredText(node, "kind", $"Node {id} kind"),
                    X = Coordinate(Required(node, "x", $"Node {id}"), $"Node {id}.x"),
                    Y = Coordinate(Required(node, "y", $"Node {id}"), $"Node {id}.y")
                });
            }

            List<DiagramEdge> edges = [];
            if (root.TryGetProperty("edges", out JsonElement edgeElements))
            {
                if (edgeElements.ValueKind != JsonValueKind.Array)
                    throw new FormatException("Diagram edges must be a list.");
                foreach (JsonElement item in edgeElements.EnumerateArray())
                {
                    string context = $"Edge {edges.Count + 1}";
                    JsonElement edge = RequireObject(item, context);
                    List<DiagramPoint> via = [];
                    if (edge.TryGetProperty("via", out JsonElement points))
                    {
                        if (points.ValueKind != JsonValueKind.Array)
                            throw new FormatException($"{context}: via must be a list of [x, y] points.");
                        foreach (JsonElement point in points.EnumerateArray())
                            via.Add(ParsePoint(point, $"{context}.via[{via.Count}]"));
                    }
                    edges.Add(new DiagramEdge
                    {
                        From = RequiredText(edge, "from", context + " from"),
                        To = RequiredText(edge, "to", context + " to"),
                        Label = edge.TryGetProperty("label", out JsonElement label)
                            ? Text(label, context + " label", allowEmpty: true) : null,
                        Via = via,
                        LabelAt = edge.TryGetProperty("label_at", out JsonElement labelAt)
                            ? ParsePoint(labelAt, context + ".label_at") : null
                    });
                }
            }
            DiagramSpec spec = new() { Caption = caption, Nodes = nodes, Edges = edges };
            Validate(spec);
            return spec;
        }
        catch (JsonException error)
        {
            throw new FormatException($"Diagram contains invalid JSON: {error.Message}", error);
        }
    }

    public static void Validate(DiagramSpec spec)
    {
        ArgumentNullException.ThrowIfNull(spec);
        if (string.IsNullOrWhiteSpace(spec.Caption))
            throw new ArgumentException("Diagram caption must be non-empty text.");
        if (spec.Nodes is null || spec.Nodes.Count == 0)
            throw new ArgumentException("Diagram nodes must be a non-empty list.");
        HashSet<string> ids = new(StringComparer.Ordinal);
        for (int index = 0; index < spec.Nodes.Count; index++)
        {
            DiagramNode? node = spec.Nodes[index];
            if (node is null || string.IsNullOrWhiteSpace(node.Id))
                throw new ArgumentException($"Node {index + 1} must have a non-empty text id.");
            if (!ids.Add(node.Id))
                throw new ArgumentException($"Duplicate node id: {node.Id}");
            if (string.IsNullOrWhiteSpace(node.Text))
                throw new ArgumentException($"Node {node.Id}: text must be non-empty.");
            if (node.Kind is null || !Shapes.ContainsKey(node.Kind))
                throw new ArgumentException($"Node {node.Id}: kind must be terminal, process, decision, or io.");
            Finite(node.X, $"Node {node.Id}.x");
            Finite(node.Y, $"Node {node.Id}.y");
        }
        if (spec.Edges is null)
            throw new ArgumentException("Diagram edges must be a list.");
        for (int index = 0; index < spec.Edges.Count; index++)
        {
            DiagramEdge? edge = spec.Edges[index];
            string context = $"Edge {index + 1}";
            if (edge is null)
                throw new ArgumentException($"{context} must be an edge object.");
            if (edge.From is null || !ids.Contains(edge.From))
                throw new ArgumentException($"{context}: unknown from node '{edge.From}'.");
            if (edge.To is null || !ids.Contains(edge.To))
                throw new ArgumentException($"{context}: unknown to node '{edge.To}'.");
            if (edge.Via is null)
                throw new ArgumentException($"{context}: via must be a list of [x, y] points.");
            for (int pointIndex = 0; pointIndex < edge.Via.Count; pointIndex++)
                ValidatePoint(edge.Via[pointIndex], $"{context}.via[{pointIndex}]");
            if (edge.LabelAt is not null)
                ValidatePoint(edge.LabelAt, context + ".label_at");
            if (edge.From == edge.To && edge.Via.Count < 2)
                throw new ArgumentException($"{context}: a self-loop needs at least two via points.");
        }
        _ = Canvas(spec);
    }

    public static void Render(DiagramSpec spec, string output)
    {
        Validate(spec);
        ArgumentException.ThrowIfNullOrWhiteSpace(output);
        CanvasInfo layout = Canvas(spec);
        Dictionary<string, DiagramNode> nodes = spec.Nodes.ToDictionary(node => node.Id, StringComparer.Ordinal);
        Dictionary<string, P> centres = spec.Nodes.ToDictionary(node => node.Id,
            node => new P(node.X * GridX + layout.Origin.X, node.Y * GridY + layout.Origin.Y),
            StringComparer.Ordinal);

        using Bitmap canvas = new(layout.Width * Scale, layout.Height * Scale, PixelFormat.Format32bppPArgb);
        canvas.SetResolution(96, 96);
        using Graphics graphics = Graphics.FromImage(canvas);
        graphics.Clear(Color.White);
        graphics.PageUnit = GraphicsUnit.Pixel;
        graphics.ScaleTransform(Scale, Scale);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        using FontSet fonts = new();
        using StringFormat measureFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
        measureFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap;
        using StringFormat centeredFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
        centeredFormat.Alignment = StringAlignment.Center;
        centeredFormat.LineAlignment = StringAlignment.Center;
        centeredFormat.FormatFlags |= StringFormatFlags.NoWrap;
        using Pen edgePen = new(Ink, 3) { LineJoin = LineJoin.Round };
        using Pen outlinePen = new(Ink, 2) { LineJoin = LineJoin.Round };
        using SolidBrush inkBrush = new(Ink);
        List<LabelSpec> labelSpecs = [];

        foreach (DiagramEdge edge in spec.Edges)
        {
            List<P> route = Route(edge, nodes, centres, layout.Origin);
            graphics.DrawLines(edgePen, route.Select(point => point.ToPointF()).ToArray());
            P tip = route[^1];
            P before = route[^2];
            double length = Distance(before, tip);
            P direction = (tip - before) / length;
            double arrowLength = Math.Min(11, length / 2);
            P arrowBase = tip - direction * arrowLength;
            P perpendicular = new(-direction.Y * 5, direction.X * 5);
            graphics.FillPolygon(inkBrush, [tip.ToPointF(), (arrowBase + perpendicular).ToPointF(),
                (arrowBase - perpendicular).ToPointF()]);
            if (!string.IsNullOrWhiteSpace(edge.Label))
                labelSpecs.Add(new(edge.Label.Trim(), route, edge.LabelAt));
        }

        List<Box> occupied = [];
        foreach (DiagramNode node in spec.Nodes)
        {
            Shape shape = Shapes[node.Kind];
            P center = centres[node.Id];
            Box bounds = Box.Centered(center, shape.Width, shape.Height);
            occupied.Add(bounds);
            using SolidBrush fill = new(shape.Fill);
            if (node.Kind == "terminal")
            {
                using GraphicsPath path = RoundedRectangle(bounds, shape.Height / 2);
                graphics.FillPath(fill, path);
                graphics.DrawPath(outlinePen, path);
            }
            else
            {
                PointF[] polygon = Polygon(node.Kind).Select(point => (point + center).ToPointF()).ToArray();
                graphics.FillPolygon(fill, polygon);
                graphics.DrawPolygon(outlinePen, polygon);
            }

            List<string>? lines = null;
            Font? selectedFont = null;
            double lineHeight = 0;
            for (int size = 20; size >= 14; size--)
            {
                Font font = fonts.Get(size);
                List<string> candidate = Wrap(graphics, node.Text.Trim(), font, shape.TextWidth, measureFormat);
                lineHeight = size * 1.2;
                if (candidate.Count * lineHeight <= shape.TextHeight)
                {
                    lines = candidate;
                    selectedFont = font;
                    break;
                }
            }
            if (lines is null || selectedFont is null)
                throw new ArgumentException($"Node {node.Id}: text is too long; shorten the label.");
            DrawCenteredLines(graphics, lines, selectedFont, inkBrush, centeredFormat, center, lineHeight);
        }

        Font labelFont = fonts.Get(18);
        foreach (LabelSpec label in labelSpecs)
        {
            List<string> lines = Wrap(graphics, label.Text, labelFont, 140, measureFormat);
            double width = lines.Max(line => TextWidth(graphics, line, labelFont, measureFormat)) + 14;
            double height = lines.Count * 22 + 8;
            P center;
            if (label.ExplicitPosition is not null)
            {
                center = new(label.ExplicitPosition.X * GridX + layout.Origin.X,
                    label.ExplicitPosition.Y * GridY + layout.Origin.Y);
                occupied.Add(Box.Centered(center, width, height));
            }
            else
                center = LabelPosition(label.Route, width, height, occupied);
            using GraphicsPath patch = RoundedRectangle(Box.Centered(center, width, height), 4);
            graphics.FillPath(Brushes.White, patch);
            DrawCenteredLines(graphics, lines, labelFont, inkBrush, centeredFormat, center, 22);
        }

        using Bitmap result = new(layout.Width, layout.Height, PixelFormat.Format24bppRgb);
        result.SetResolution(96, 96);
        using (Graphics finalGraphics = Graphics.FromImage(result))
        using (ImageAttributes attributes = new())
        {
            finalGraphics.Clear(Color.White);
            finalGraphics.CompositingQuality = CompositingQuality.HighQuality;
            finalGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            finalGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            attributes.SetWrapMode(WrapMode.TileFlipXY);
            finalGraphics.DrawImage(canvas, new Rectangle(0, 0, result.Width, result.Height),
                0, 0, canvas.Width, canvas.Height, GraphicsUnit.Pixel, attributes);
        }
        string pathName = Path.GetFullPath(output);
        Directory.CreateDirectory(Path.GetDirectoryName(pathName)!);
        result.Save(pathName, ImageFormat.Png);
    }

    private static JsonElement RequireObject(JsonElement element, string context)
    {
        if (element.ValueKind != JsonValueKind.Object)
            throw new FormatException($"{context} must be a JSON object.");
        HashSet<string> names = new(StringComparer.Ordinal);
        foreach (JsonProperty property in element.EnumerateObject())
            if (!names.Add(property.Name))
                throw new FormatException($"{context}: duplicate JSON property '{property.Name}'.");
        return element;
    }

    private static JsonElement Required(JsonElement parent, string name, string context) =>
        parent.TryGetProperty(name, out JsonElement value) ? value :
            throw new FormatException($"{context}: required property '{name}' is missing.");

    private static string RequiredText(JsonElement parent, string name, string context) =>
        Text(Required(parent, name, context), context, allowEmpty: false);

    private static string Text(JsonElement element, string context, bool allowEmpty)
    {
        if (element.ValueKind != JsonValueKind.String)
            throw new FormatException($"{context} must be text.");
        string value = element.GetString()!;
        if (!allowEmpty && string.IsNullOrWhiteSpace(value))
            throw new FormatException($"{context} must be non-empty text.");
        return value;
    }

    private static double Coordinate(JsonElement element, string context)
    {
        if (element.ValueKind != JsonValueKind.Number || !element.TryGetDouble(out double value) || !double.IsFinite(value))
            throw new FormatException($"{context}: coordinate must be a finite number.");
        return value;
    }

    private static DiagramPoint ParsePoint(JsonElement element, string context)
    {
        if (element.ValueKind != JsonValueKind.Array || element.GetArrayLength() != 2)
            throw new FormatException($"{context} must be an [x, y] point.");
        return new(Coordinate(element[0], context + ".x"), Coordinate(element[1], context + ".y"));
    }

    private static void Finite(double value, string context)
    {
        if (!double.IsFinite(value))
            throw new ArgumentException($"{context}: coordinate must be a finite number.");
    }

    private static void ValidatePoint(DiagramPoint? point, string context)
    {
        if (point is null)
            throw new ArgumentException($"{context} must be an [x, y] point.");
        Finite(point.X, context + ".x");
        Finite(point.Y, context + ".y");
    }

    private static CanvasInfo Canvas(DiagramSpec spec)
    {
        List<Box> extents = [];
        foreach (DiagramNode node in spec.Nodes)
        {
            Shape shape = Shapes[node.Kind];
            extents.Add(Box.Centered(new(node.X * GridX, node.Y * GridY), shape.Width, shape.Height));
        }
        foreach (DiagramEdge edge in spec.Edges)
        {
            foreach (DiagramPoint point in edge.Via)
                extents.Add(Box.Centered(new(point.X * GridX, point.Y * GridY), 0, 0));
            if (edge.LabelAt is not null)
                extents.Add(Box.Centered(new(edge.LabelAt.X * GridX, edge.LabelAt.Y * GridY), 0, 0));
        }
        double left = extents.Min(box => box.Left), top = extents.Min(box => box.Top);
        double right = extents.Max(box => box.Right), bottom = extents.Max(box => box.Bottom);
        double width = Math.Ceiling(right - left + 2 * Padding);
        double height = Math.Ceiling(bottom - top + 2 * Padding);
        if (!double.IsFinite(left) || !double.IsFinite(top) || !double.IsFinite(right) || !double.IsFinite(bottom) ||
            !double.IsFinite(width) || !double.IsFinite(height) || width <= 0 || height <= 0 ||
            width > 12000 || height > 12000 || width * height > 30_000_000)
            throw new ArgumentException("Diagram canvas is too large; reduce coordinate spacing.");
        return new((int)width, (int)height, new(Padding - left, Padding - top));
    }

    private static double TextWidth(Graphics graphics, string text, Font font, StringFormat format) =>
        text.Length == 0 ? 0 : graphics.MeasureString(text, font, int.MaxValue, format).Width;

    private static List<string> Wrap(Graphics graphics, string text, Font font, double width, StringFormat format)
    {
        List<string> result = [];
        foreach (string paragraph in text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n'))
        {
            string line = "";
            foreach (Match match in Regex.Matches(paragraph, @"\S+"))
            {
                List<string> chunks = [];
                string chunk = "";
                foreach (var rune in match.Value.EnumerateRunes())
                {
                    string character = rune.ToString();
                    if (chunk.Length != 0 && TextWidth(graphics, chunk + character, font, format) > width)
                    {
                        chunks.Add(chunk);
                        chunk = character;
                    }
                    else
                        chunk += character;
                }
                if (chunk.Length != 0)
                    chunks.Add(chunk);
                for (int partIndex = 0; partIndex < chunks.Count; partIndex++)
                {
                    string candidate = (line + " " + chunks[partIndex]).Trim();
                    if (line.Length != 0 && TextWidth(graphics, candidate, font, format) > width)
                    {
                        result.Add(line);
                        line = chunks[partIndex];
                    }
                    else
                        line = candidate;
                    if (partIndex < chunks.Count - 1)
                    {
                        result.Add(line);
                        line = "";
                    }
                }
            }
            result.Add(line);
        }
        return result;
    }

    private static void DrawCenteredLines(Graphics graphics, List<string> lines, Font font, Brush brush,
        StringFormat format, P center, double lineHeight)
    {
        double startY = center.Y - (lines.Count - 1) * lineHeight / 2;
        for (int index = 0; index < lines.Count; index++)
        {
            double y = startY + index * lineHeight;
            graphics.DrawString(lines[index], font, brush,
                new RectangleF((float)(center.X - 1000), (float)(y - 100), 2000, 200), format);
        }
    }

    private static P[] Polygon(string kind)
    {
        Shape shape = Shapes[kind];
        double hx = shape.Width / 2, hy = shape.Height / 2;
        return kind switch
        {
            "decision" => [new(0, -hy), new(hx, 0), new(0, hy), new(-hx, 0)],
            "io" => [new(-hx + 24, -hy), new(hx, -hy), new(hx - 24, hy), new(-hx, hy)],
            _ => [new(-hx, -hy), new(hx, -hy), new(hx, hy), new(-hx, hy)]
        };
    }

    private static bool Inside(string kind, P point)
    {
        if (kind == "terminal")
        {
            Shape shape = Shapes[kind];
            double hx = shape.Width / 2, hy = shape.Height / 2;
            double extra = Math.Max(Math.Abs(point.X) - (hx - hy), 0);
            return extra * extra + point.Y * point.Y <= hy * hy + Epsilon;
        }
        P[] polygon = Polygon(kind);
        bool nonnegative = true, nonpositive = true;
        for (int index = 0; index < polygon.Length; index++)
        {
            P a = polygon[index], b = polygon[(index + 1) % polygon.Length];
            double cross = (b.X - a.X) * (point.Y - a.Y) - (b.Y - a.Y) * (point.X - a.X);
            nonnegative &= cross >= -Epsilon;
            nonpositive &= cross <= Epsilon;
        }
        return nonnegative || nonpositive;
    }

    private static P Boundary(string kind, P direction)
    {
        double length = Length(direction);
        if (length < Epsilon)
            throw new ArgumentException("An edge needs a non-zero route between its nodes.");
        P d = direction / length;
        List<double> candidates = [];
        if (kind == "terminal")
        {
            Shape shape = Shapes[kind];
            double radius = shape.Height / 2, inner = shape.Width / 2 - radius;
            if (Math.Abs(d.Y) > Epsilon)
            {
                double t = radius / Math.Abs(d.Y);
                if (Math.Abs(t * d.X) <= inner + Epsilon)
                    candidates.Add(t);
            }
            foreach (double cx in new[] { -inner, inner })
            {
                double discriminant = Math.Pow(d.X * cx, 2) - cx * cx + radius * radius;
                if (discriminant < 0)
                    continue;
                double root = Math.Sqrt(discriminant);
                foreach (double t in new[] { d.X * cx - root, d.X * cx + root })
                {
                    double x = t * d.X;
                    if (t > 0 && ((cx > 0 && x >= inner - Epsilon) || (cx < 0 && x <= -inner + Epsilon)))
                        candidates.Add(t);
                }
            }
        }
        else
        {
            P[] polygon = Polygon(kind);
            for (int index = 0; index < polygon.Length; index++)
            {
                P a = polygon[index], e = polygon[(index + 1) % polygon.Length] - a;
                double denominator = d.X * e.Y - d.Y * e.X;
                if (Math.Abs(denominator) < Epsilon)
                    continue;
                double t = (a.X * e.Y - a.Y * e.X) / denominator;
                double u = (a.X * d.Y - a.Y * d.X) / denominator;
                if (t >= 0 && u >= -Epsilon && u <= 1 + Epsilon)
                    candidates.Add(t);
            }
        }
        if (candidates.Count == 0)
            throw new ArgumentException($"Could not determine the {kind} node boundary.");
        return d * candidates.Min();
    }

    private static List<P> Route(DiagramEdge edge, Dictionary<string, DiagramNode> nodes,
        Dictionary<string, P> centres, P origin)
    {
        P start = centres[edge.From], end = centres[edge.To];
        List<P> raw = [start];
        raw.AddRange(edge.Via.Select(point => new P(point.X * GridX + origin.X, point.Y * GridY + origin.Y)));
        raw.Add(end);
        List<P> points = [];
        foreach (P point in raw)
            if (points.Count == 0 || points[^1] != point)
                points.Add(point);
        string sourceKind = nodes[edge.From].Kind, targetKind = nodes[edge.To].Kind;
        while (points.Count > 2 && Inside(sourceKind, points[1] - start))
            points.RemoveAt(1);
        while (points.Count > 2 && Inside(targetKind, points[^2] - end))
            points.RemoveAt(points.Count - 2);
        if (points.Count < 2)
            throw new ArgumentException("An edge needs a non-zero route between its nodes.");
        P source = Boundary(sourceKind, points[1] - start);
        P target = Boundary(targetKind, points[^2] - end);
        points[0] = start + source;
        points[^1] = end + target;
        if (Distance(points[^2], points[^1]) < Epsilon)
            throw new ArgumentException("The end of an edge route coincides with the node boundary.");
        return points;
    }

    private static P LabelPosition(List<P> route, double width, double height, List<Box> occupied)
    {
        List<Segment> segments = [];
        for (int index = 0; index < route.Count - 1; index++)
            segments.Add(new(route[index], route[index + 1], Distance(route[index], route[index + 1])));
        double remaining = segments.Sum(segment => segment.Length) / 2;
        List<(P Point, P Direction)> preferred = [];
        foreach (Segment segment in segments)
        {
            if (segment.Length != 0 && remaining <= segment.Length)
            {
                preferred.Add((segment.A + (segment.B - segment.A) * (remaining / segment.Length),
                    (segment.B - segment.A) / segment.Length));
                break;
            }
            remaining -= segment.Length;
        }
        foreach (Segment segment in segments.OrderByDescending(segment => segment.Length))
            if (segment.Length != 0)
                preferred.Add(((segment.A + segment.B) / 2, (segment.B - segment.A) / segment.Length));
        P? firstCandidate = null;
        foreach ((P point, P direction) in preferred)
        {
            double distance = Math.Abs(direction.Y) * width / 2 + Math.Abs(direction.X) * height / 2 + 8;
            double[] offsets = [distance, -distance, 0, distance + 20, -distance - 20,
                distance + 40, -distance - 40, distance + 60, -distance - 60];
            foreach (double offset in offsets)
            {
                P candidate = new(point.X - direction.Y * offset, point.Y + direction.X * offset);
                firstCandidate ??= candidate;
                Box box = Box.Centered(candidate, width, height);
                if (!occupied.Any(existing => Overlaps(box, existing, 8)))
                {
                    occupied.Add(box);
                    return candidate;
                }
            }
        }
        return firstCandidate ?? throw new ArgumentException("An edge label needs a non-zero route.");
    }

    private static bool Overlaps(Box a, Box b, double gap) =>
        !(a.Right + gap <= b.Left || b.Right + gap <= a.Left ||
          a.Bottom + gap <= b.Top || b.Bottom + gap <= a.Top);

    private static GraphicsPath RoundedRectangle(Box box, double radius)
    {
        GraphicsPath path = new();
        float diameter = (float)(radius * 2);
        float left = (float)box.Left, top = (float)box.Top;
        float right = (float)box.Right, bottom = (float)box.Bottom;
        path.AddArc(left, top, diameter, diameter, 180, 90);
        path.AddArc(right - diameter, top, diameter, diameter, 270, 90);
        path.AddArc(right - diameter, bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(left, bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }

    private static double Length(P point) => Math.Sqrt(point.X * point.X + point.Y * point.Y);
    private static double Distance(P a, P b) => Length(b - a);

    private sealed record Shape(double Width, double Height, double TextWidth, double TextHeight, Color Fill);
    private sealed record CanvasInfo(int Width, int Height, P Origin);
    private sealed record LabelSpec(string Text, List<P> Route, DiagramPoint? ExplicitPosition);
    private readonly record struct Segment(P A, P B, double Length);
    private readonly record struct Box(double Left, double Top, double Right, double Bottom)
    {
        public static Box Centered(P center, double width, double height) =>
            new(center.X - width / 2, center.Y - height / 2, center.X + width / 2, center.Y + height / 2);
    }
    private readonly record struct P(double X, double Y)
    {
        public PointF ToPointF() => new((float)X, (float)Y);
        public static P operator +(P a, P b) => new(a.X + b.X, a.Y + b.Y);
        public static P operator -(P a, P b) => new(a.X - b.X, a.Y - b.Y);
        public static P operator *(P a, double factor) => new(a.X * factor, a.Y * factor);
        public static P operator /(P a, double divisor) => new(a.X / divisor, a.Y / divisor);
    }

    private sealed class FontSet : IDisposable
    {
        private readonly PrivateFontCollection? customFonts;
        private readonly FontFamily family;
        private readonly Dictionary<int, Font> fonts = [];

        public FontSet()
        {
            string? explicitFont = Environment.GetEnvironmentVariable("PROCEDURAL_DIAGRAM_FONT");
            if (!string.IsNullOrEmpty(explicitFont))
            {
                customFonts = new();
                try
                {
                    customFonts.AddFontFile(explicitFont);
                    family = customFonts.Families.First();
                }
                catch (Exception error) when (error is ArgumentException or IOException or InvalidOperationException)
                {
                    customFonts.Dispose();
                    throw new ArgumentException($"Cannot load PROCEDURAL_DIAGRAM_FONT: {explicitFont}", error);
                }
            }
            else
                family = new FontFamily("Arial");
        }

        public Font Get(int size)
        {
            if (!fonts.TryGetValue(size, out Font? font))
            {
                font = new(family, size, FontStyle.Regular, GraphicsUnit.Pixel);
                fonts.Add(size, font);
            }
            return font;
        }

        public void Dispose()
        {
            foreach (Font font in fonts.Values)
                font.Dispose();
            family.Dispose();
            customFonts?.Dispose();
        }
    }
}
