using System.Runtime.InteropServices;

namespace Procedural.WorkbookBuilder;

public static class WordFields
{
    public static void Update(string output, bool exportPdf = false)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Word alan güncellemesi için Windows ve masaüstü Microsoft Word gerekir.");
        var type = Type.GetTypeFromProgID("Word.Application") ??
            throw new InvalidOperationException("Microsoft Word bulunamadı. --skip-word-fields ile belgeyi üretip alanları Word'de güncelleyebilirsiniz.");
        dynamic? application = null, documents = null, document = null;
        try
        {
            application = Activator.CreateInstance(type)!;
            application.Visible = false;
            application.DisplayAlerts = 0;
            documents = application.Documents;
            document = documents.Open(Path.GetFullPath(output), false, false, false);
            dynamic fields = document.Fields;
            try { fields.Update(); }
            finally { Release(fields); }
            document.Repaginate();
            UpdateCollection(document.TablesOfContents, false);
            UpdateCollection(document.TablesOfFigures, false);
            document.Repaginate();
            UpdateCollection(document.TablesOfContents, true);
            UpdateCollection(document.TablesOfFigures, true);
            document.Save();
            if (exportPdf) document.ExportAsFixedFormat(Path.ChangeExtension(Path.GetFullPath(output), ".pdf"), 17);
        }
        catch (Exception error)
        {
            throw new InvalidOperationException("Belge üretildi ancak Word alanları güncellenemedi. Belgenin kapalı olduğunu ve Microsoft Word kurulumunu kontrol edin. " + error.Message, error);
        }
        finally
        {
            if (document is not null)
            {
                try { document.Close(0); } catch (COMException) { }
                Release(document);
            }
            Release(documents);
            if (application is not null)
            {
                try { application.Quit(0); } catch (COMException) { }
                Release(application);
            }
        }
    }

    private static void UpdateCollection(dynamic collection, bool pageNumbersOnly)
    {
        try
        {
            for (int i = 1; i <= collection.Count; i++)
            {
                dynamic item = collection.Item(i);
                try
                {
                    if (pageNumbersOnly) item.UpdatePageNumbers();
                    else item.Update();
                }
                finally { Release(item); }
            }
        }
        finally { Release(collection); }
    }

    private static void Release(object? value)
    {
        if (value is not null && Marshal.IsComObject(value)) Marshal.FinalReleaseComObject(value);
    }
}
