---
id: "091"
order: 91
title: "Ders denemelerinden ağırlıklı başarı"
level: "İleri"
prerequisites: ["053","073","083","089"]
concepts: ["Bileşik anahtar","En iyi deneme","Ağırlıklı ortalama","Dizinle raporlama"]
---

# Ders denemelerinden ağırlıklı başarı

## Problem tanımı

Öğrencilerin bir dersi birden fazla denemede alabileceği bir programda, her öğrenci için ders başına en yüksek puanı seçin ve kredi ağırlıklı ortalamayı hesaplayın. Üç dosya öğrenci, ders ve deneme verilerini ayrı tutar. Satış toplamından farklı olarak burada aynı ilişkiyi tek bir en iyi sonuç temsil eder; bir denemeyi iki kez saymayın.

## Girdi ve çıktı

`dotnet run -- ornek-091/ogrenciler.txt ornek-091/dersler.txt ornek-091/denemeler.txt ornek-091/rapor.txt`

`ogrenciler.txt`: `öğrenciKod` (1..9999). `dersler.txt`: `dersKod;kredi` (kod 1..9999, kredi 1..10). `denemeler.txt`: `öğrenciKod;dersKod;deneme;puan` (deneme 1..3, puan 0..100). Öğrenci ve ders kodları benzersizdir; deneme anahtarı öğrenci/ders/deneme üçlüsüdür. Farklı denemeler normaldir. Her denemede iki ana kayıt da bulunmalıdır.

Rapor öğrenci kodu artan sırada `kod;farklıDers;krediToplamı;ortalama` biçimindedir. Ortalamada en iyi puan × kredi toplamını kredi toplamına bölün; iki basamakla, midpoint to even yuvarlama ile yazın. Eşit en iyi puanlarda ilk deneme korunur, sonuç değişmez. Denemesi olmayan öğrencide `kod;0;0;VERI_YOK` yazın; hiç öğrenci yoksa rapor boştur. Bir öğrencinin almadığı katalog dersi eksik kayıt değildir.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.

## Algoritma

1. Üç tabloyu ayrıştırın ve benzersizlikleri doğrulayın.
2. Denemelerin öğrenci ve ders kodlarını doğrusal aramayla dizi dizinlerine dönüştürün.
3. Her öğrenci/ders hücresini -1 ile başlatın; puan daha yüksekse değiştirin. Sıfır puanı veri yok durumundan ayırın.
4. Her öğrenci için dolu hücrelerde kredi ve ağırlıklı puan biriktirin; önce bütün hesapları tamamlayın.
5. Öğrenci dizinlerini eklemeli sıralayıp raporu oluşturun; yeni dosyaya yazın.

| Öğrenci | Ders | Denemeler | Seçilen puan | Ağırlıklı katkı |
| --- | --- | --- | --- | --- |
| 2 | 7 | 40, 80 | 80 | 80 × 3 = 240 |
| 2 | 9 | 60 | 60 | 60 × 1 = 60 |
| 2 | toplam | 2 ders | 4 kredi | 300 / 4 = 75.00 |

`Oku` yalnız geçerli satırları içeren diziyi döndürür. `Tekil` seçilen sütunları birlikte anahtar sayar; `Bul` ilk sütunda doğrusal arar, yoksa -1 verir. `Sira` kaynak kayıtları değiştirmeden kod artan eklemeli sıralamayla dizin dizisi döndürür. `Satir` yalnız bellek raporunu değiştirir; `Yaz` dış dosyayı oluşturur. Hazır LINQ, GroupBy, Sort veya arama algoritması kullanılmaz.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 4) throw new FormatException(
        "4 argüman gerekli.");
    checked
    {
        int[][] ogr = Oku(args[0], new[] { 1 }, new[] { 9999 });
        int[][] ders = Oku(args[1], new[] { 1, 1 }, new[] { 9999, 10 });
        int[][] den = Oku(args[2], new[] { 1, 1, 1, 0 },
            new[] { 9999, 9999, 3, 100 });
        Tekil(ogr, args[0], 0); Tekil(ders, args[1], 0);
        Tekil(den, args[2], 0, 1, 2);
        int[,] enIyi = new int[ogr.Length, ders.Length];
        for (int i = 0; i < ogr.Length; i++)
            for (int j = 0; j < ders.Length; j++) enIyi[i, j] = -1;

        for (int i = 0; i < den.Length; i++)
        {
            int o = Bul(ogr, den[i][0]), d = Bul(ders, den[i][1]);
            if (o < 0) Hata(args[2], i + 1, 1, "Öğrenci bulunamadı.");
            if (d < 0) Hata(args[2], i + 1, 2, "Ders bulunamadı.");
            if (den[i][3] > enIyi[o, d]) enIyi[o, d] = den[i][3];
        }

        var rapor = new StringBuilder();
        foreach (int o in Sira(ogr))
        {
            int kredi = 0, agirlik = 0, adet = 0;
            for (int d = 0; d < ders.Length; d++)
                if (enIyi[o, d] >= 0)
                {
                    kredi += ders[d][1];
                    agirlik += enIyi[o, d] * ders[d][1]; adet++;
                }
            string ort = kredi == 0 ? "VERI_YOK" :
                Math.Round((decimal)agirlik / kredi, 2,
                    MidpointRounding.ToEven).ToString("F2",
                    CultureInfo.InvariantCulture);
            Satir(rapor, ogr[o][0], adet, kredi, ort);
        }

        Yaz(args[3], rapor.ToString());
    }
    Console.WriteLine("Rapor yazıldı.");
}
catch (FormatException e) { Console.WriteLine("Hata: " + e.Message); }
catch (IOException) { Console.WriteLine("Hata: Dosya işlemi başarısız."); }
catch (UnauthorizedAccessException)
{ Console.WriteLine("Hata: Dosyaya erişim izni yok."); }
catch (ArgumentException)
{ Console.WriteLine("Hata: Yol veya UTF-8 kodlaması geçersiz."); }
catch (NotSupportedException)
{ Console.WriteLine("Hata: Yol biçimi desteklenmiyor."); }
catch (OverflowException) { Console.WriteLine("Hata: Hesaplama taştı."); }

static int[][] Oku(string yol, int[] alt, int[] ust,
    int kapasite = 100)
{
    int[][] veri = new int[kapasite][];
    int n = 0;
    using var akis = new FileStream(yol, FileMode.Open,
        FileAccess.Read, FileShare.Read);
    if (akis.Length > 32768)
        throw new FormatException(Path.GetFileName(yol) +
            ": Dosya 32768 baytı aşıyor.");
    using var okuyucu = new StreamReader(akis,
        new UTF8Encoding(false, true), false);
    string? metin;
    while ((metin = okuyucu.ReadLine()) is not null)
    {
        if (n == 0 && metin.StartsWith('\uFEFF'))
            metin = metin.Substring(1);
        if (n == kapasite) Hata(yol, n + 1, 0, "Kapasite aşıldı.");
        if (metin.Length > 120)
            Hata(yol, n + 1, 0, "Satır 120 karakteri aşıyor.");
        string[] a = metin.Split(';');
        if (a.Length != alt.Length)
            Hata(yol, n + 1, 0, "Alan sayısı yanlış.");
        veri[n] = new int[a.Length];
        for (int j = 0; j < a.Length; j++)
        {
            if (ust[j] == -1)
                veri[n][j] = Tarih(yol, n + 1, j + 1, a[j]);
            else
                veri[n][j] = Tam(yol, n + 1, j + 1,
                    a[j], alt[j], ust[j]);
        }
        n++;
    }
    Array.Resize(ref veri, n);
    return veri;
}

static void Hata(string yol, int satir, int alan, string mesaj)
{
    throw new FormatException(Path.GetFileName(yol) +
        ": satır " + satir.ToString(CultureInfo.InvariantCulture) +
        ", alan " + alan.ToString(CultureInfo.InvariantCulture) +
        ": " + mesaj);
}

static int Tam(string yol, int satir, int alan, string metin,
    int alt, int ust)
{
    if (metin.Length == 0) Hata(yol, satir, alan, "Geçersiz tamsayı.");
    for (int i = 0; i < metin.Length; i++)
        if (metin[i] < '0' || metin[i] > '9')
            Hata(yol, satir, alan, "Geçersiz tamsayı.");
    if (!int.TryParse(metin, NumberStyles.None,
        CultureInfo.InvariantCulture, out int deger) ||
        deger < alt || deger > ust)
        Hata(yol, satir, alan, "Geçersiz tamsayı.");
    return deger;
}

static int Tarih(string yol, int satir, int alan, string metin)
{
    if (!DateOnly.TryParseExact(metin, "yyyy-MM-dd",
        CultureInfo.InvariantCulture, DateTimeStyles.None,
        out DateOnly tarih) || tarih.Year < 2000 || tarih.Year > 2099)
        Hata(yol, satir, alan, "Geçersiz tarih.");
    return tarih.DayNumber;
}

static void Tekil(int[][] veri, string yol, params int[] alanlar)
{
    for (int i = 0; i < veri.Length; i++)
        for (int j = 0; j < i; j++)
        {
            bool ayni = true;
            for (int k = 0; k < alanlar.Length; k++)
                if (veri[i][alanlar[k]] != veri[j][alanlar[k]])
                    ayni = false;
            if (ayni) Hata(yol, i + 1, alanlar[0] + 1,
                "Yinelenen anahtar.");
        }
}

static int Bul(int[][] veri, int kod)
{
    for (int i = 0; i < veri.Length; i++)
        if (veri[i][0] == kod) return i;
    return -1;
}

static int[] Sira(int[][] veri)
{
    int[] sira = new int[veri.Length];
    for (int i = 0; i < sira.Length; i++) sira[i] = i;
    for (int i = 1; i < sira.Length; i++)
    {
        int p = sira[i], j = i - 1;
        while (j >= 0 && veri[sira[j]][0] > veri[p][0])
        { sira[j + 1] = sira[j]; j--; }
        sira[j + 1] = p;
    }
    return sira;
}

static void Satir(StringBuilder rapor, params object[] alanlar)
{
    for (int i = 0; i < alanlar.Length; i++)
    {
        if (i != 0) rapor.Append(';');
        rapor.Append(Convert.ToString(alanlar[i],
            CultureInfo.InvariantCulture));
    }
    rapor.Append('\n');
}

static void Yaz(string yol, string metin)
{
    using var akis = new FileStream(yol, FileMode.CreateNew,
        FileAccess.Write, FileShare.None);
    using var yazici = new StreamWriter(akis, new UTF8Encoding(false));
    yazici.Write(metin);
}
```

## Örnek çalıştırmalar

Normal örnek için yukarıdaki komutu kullanın; rapor.txt başlangıçta yoktur.

ogrenciler.txt dosyasının tam içeriği:

```text
3
2
```

dersler.txt dosyasının tam içeriği:

```text
9;1
7;3
```

denemeler.txt dosyasının tam içeriği:

```text
2;7;1;40
2;7;2;80
2;9;1;60
```

Yeni rapor.txt dosyasının tam içeriği:

```text
2;2;4;75.00
3;0;0;VERI_YOK
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Sınır örneği: aynı dosya adlarını ayrı boş bir klasörde hazırlayın; aynı komutu çalıştırın.

ogrenciler.txt dosyasının tam içeriği:

```text
9999
```

dersler.txt dosyasının tam içeriği:

```text
9999;10
```

denemeler.txt dosyasının tam içeriği:

```text
9999;9999;3;0
```

Yeni rapor.txt dosyasının tam içeriği:

```text
9999;1;10;0.00
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçersiz örnek: ayrı bir klasörde, aynı komut.

ogrenciler.txt dosyasının tam içeriği:

```text
2
```

dersler.txt dosyasının tam içeriği:

```text
7;3
```

denemeler.txt dosyasının tam içeriği:

```text
2;8;1;60
```

Rapor oluşturulmaz. Konsolun tam çıktısı:

```text
Hata: denemeler.txt: satır 1, alan 2: Ders bulunamadı.
```

Bütün giriş dosyaları sıfır bayt ve rapor.txt yokken yeni rapor sıfır bayttır.

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçerli normal girdilerle rapor.txt önceden varsa mevcut baytlar korunur. Konsolun tam çıktısı:

```text
Hata: Dosya işlemi başarısız.
```

## Sınır durumları

- En fazla 100 öğrenci, 100 ders ve 100 deneme satırı vardır. Öğrenci başına en fazla 1000 kredi ve 100000 ağırlıklı puan; int güvenlidir, bölme decimal ile yapılır. Ders dosyası boşken deneme varsa eksik ders hatasıdır. Boş deneme dosyası tüm öğrencileri VERI_YOK yapar. Aynı üçlü ikinci kez görünürse, puan farklı olsa da hata olur.
- Satır kapasitesinin tam dolması kabul edilir; bir sonraki satır saklanmadan hata olur. Tek bir boş satır sıfır kayıt sayılmaz; alan/sayı hatasıdır. Çok büyük veya taşan sayısal alan ayrıştırma hatasıdır.
- Aynı sayıyı farklı baştaki sıfırlarla yazmak anahtar tekrarını gizlemez. Sıralama sırasında dizinler taşındığı için dosya satırı ile hata konumu ve ilişkiler değişmez.
- Hatalı son kayıt dahil bütün giriş doğrulanmadan rapor açılmaz. Ortak I/O ve çıktı güvenliği sözleşmesi yukarıda tanımlıdır.

## Kazanımlar

- Veri yok durumunu gerçek sıfırdan ayırma; bileşik anahtar ile tekrar denemeyi ayırt etme; seçim ve ağırlıklı özetlemeyi farklı metot sorumlulukları olarak açıklama.
- Öğrenciye ait ders sonuçlarını paralel hücrelerde tutarken aynı doğrulama ve ortalama davranışları tekrar eder. Sonraki derste bu verileri bir öğrenci nesnesinde toplamak, tutarsız puan/kredi eşleşmelerini önleyebilir.

## Alıştırmalar

1. En iyi deneme numarasını rapora ekleyin; eşit puanda küçük deneme numarasını seçin.
2. Sıfır puan ile VERI_YOK değerinin neden farklı olduğunu ve Öğrenci/Ders verisini bir arada korumanın yararını açıklayın.

