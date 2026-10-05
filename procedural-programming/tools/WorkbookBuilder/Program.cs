using System.Text;
using Procedural.WorkbookBuilder;

Console.OutputEncoding = Encoding.UTF8;
try
{
    var options = BuildOptions.Parse(args);
    if (options.Help)
    {
        Console.WriteLine("Markdown problemlerinden C# ile Word çalışma kitabı üretir.");
        Console.WriteLine("Seçenekler: --root KLASÖR --output DOSYA --check --skip-word-fields --pdf --help");
        return 0;
    }

    var lessons = LessonLoader.Load(options.Root);
    Console.WriteLine($"{lessons.Count} problem doğrulandı.");
    var chapters = ChapterLoader.Load(options.Root, lessons);
    if (chapters.Count != 0) Console.WriteLine($"{chapters.Count} bölüm doğrulandı; bölüm başına 10 problem.");
    if (FrontMatterLoader.LoadPreface(options.Root) is not null)
        Console.WriteLine("Önsöz kaynağı doğrulandı.");
    if (FrontMatterLoader.LoadIntroduction(options.Root) is not null)
        Console.WriteLine("Giriş kaynağı doğrulandı; ön bölümler problem sayımının dışında.");
    if (options.Check) return 0;
    WordBookWriter.Write(lessons, options.Root, options.Output);
    Console.WriteLine($"Word belgesi: {options.Output}");
    if (options.SkipWordFields)
        Console.WriteLine("Tıklanabilir dizinler hazır. Sayfa numaraları için Word'de Ctrl+A ve F9 ile alanları güncelleyin.");
    else
    {
        WordFields.Update(options.Output, options.Pdf);
        Console.WriteLine("İçindekiler, şekil dizini ve sayfa numaraları güncellendi.");
    }
    return 0;
}
catch (Exception error)
{
    Console.Error.WriteLine($"Derleme hatası: {error.Message}");
    return 1;
}

internal sealed record BuildOptions(string Root, string Output, bool Check, bool SkipWordFields, bool Pdf, bool Help)
{
    public static BuildOptions Parse(string[] arguments)
    {
        string? root = null, output = null;
        bool check = false, skip = false, pdf = false, help = false;
        for (var i = 0; i < arguments.Length; i++)
        {
            switch (arguments[i])
            {
                case "--root": root = Value(arguments, ref i); break;
                case "--output": output = Value(arguments, ref i); break;
                case "--check": check = true; break;
                case "--skip-word-fields": skip = true; break;
                case "--pdf": pdf = true; break;
                case "--help" or "-h": help = true; break;
                default: throw new ArgumentException($"Bilinmeyen seçenek: {arguments[i]}");
            }
        }
        if (pdf && skip) throw new ArgumentException("--pdf için Word alan güncellemesi gerekir.");
        root = Path.GetFullPath(root ?? FindRoot());
        output = Path.GetFullPath(output ?? Path.Combine(root, "build", "prosedurel-programlama.docx"));
        if (!output.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Çıktı dosyasının uzantısı .docx olmalı.");
        return new(root, output, check, skip, pdf, help);
    }

    private static string Value(string[] arguments, ref int index)
    {
        if (++index >= arguments.Length || arguments[index].StartsWith("--"))
            throw new ArgumentException($"{arguments[index - 1]} için bir değer gerekli.");
        return arguments[index];
    }

    private static string FindRoot()
    {
        foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            for (var folder = new DirectoryInfo(start); folder is not null; folder = folder.Parent)
            {
                if (Directory.Exists(Path.Combine(folder.FullName, "problems")) &&
                    Directory.Exists(Path.Combine(folder.FullName, "templates"))) return folder.FullName;
                var child = Path.Combine(folder.FullName, "procedural-programming");
                if (Directory.Exists(Path.Combine(child, "problems"))) return child;
            }
        }
        throw new DirectoryNotFoundException("procedural-programming bulunamadı. --root ile klasörü belirtin.");
    }
}
