---
id: "093"
order: 93
title: "Ödünç ve iadelerden gecikme raporu"
level: "İleri"
prerequisites: ["042","053","084","089"]
concepts: ["Birden çok ilişki","İade eşleştirme","Kesim tarihi","Açık kayıt"]
---

# Ödünç ve iadelerden gecikme raporu

## Problem tanımı

Üye ve kitap kataloglarını, ödünç kayıtlarını ve ayrı iade kayıtlarını birleştirin. Her ödünç için verilen kesim tarihinde gecikme gününü ve durumunu üretin. Kesim tarihinden sonra gerçekleşen bir iade veride geçerli olsa bile o tarihte kitap hâlâ açıktır. Böylece bugünkü veriyle geçmiş bir rapor hazırlanabilir.

## Girdi ve çıktı

`dotnet run -- ornek-093/uyeler.txt ornek-093/kitaplar.txt ornek-093/oduncler.txt ornek-093/iadeler.txt 2026-10-10 ornek-093/rapor.txt`

`uyeler.txt`: `üyeKod`; `kitaplar.txt`: `kitapKod`; kodlar 1..9999. `oduncler.txt`: `ödünçKod;üyeKod;kitapKod;alışTarihi;sonTarih`. `iadeler.txt`: `ödünçKod;iadeTarihi`. Tüm tarihler ISO sözleşmesindedir. Her katalog kodu, ödünç kodu ve iadedeki ödünç kodu kendi dosyasında benzersizdir. Ödünçte üye ve kitap, iadede ödünç bulunmalıdır. Son tarih alıştan, iade tarihi alıştan önce olamaz. Rapor kesim tarihi beşinci argümandır, çıktı altıncıdır.

Rapor kesim tarihine kadar başlayan ödünçler için artan ödünç koduyla `ödünçKod;üyeKod;kitapKod;durum;gecikmeGün` yazar. Kesime kadar iade varsa durum IADE, gecikme `max(0, iade-son)`; yoksa ACIK, gecikme `max(0, kesim-son)` olur. Kesimden sonraki alışlar raporda yer almaz. Aynı kitap için birden fazla ödünç kabul edilir; bu çalışma fiziksel kitap nüshası veya çakışma denetimi yapmaz. İade eksikliği hata değil ACIK durumudur.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.

## Algoritma

1. Dört dosyayı okuyun, ardından kesim tarihini doğrulayın.
2. Tekil anahtarları ve ödünçlerin katalog ilişkilerini denetleyin.
3. İadeleri ödünç dizinleriyle eşleyin; alıştan önce iadeyi reddedin.
4. Her ödünç için kesimde görünür olup olmadığını ve etkin bitiş gününü belirleyin.
5. Gecikmeyi negatif olmayacak biçimde hesaplayıp kod sırasıyla yazın.

| Ödünç | Son tarih | İade | Kesim | Durum ve gecikme |
| --- | --- | --- | --- | --- |
| 1 | 2026-10-07 | 2026-10-09 | 2026-10-10 | IADE, 2 |
| 2 | 2026-10-08 | 2026-10-12 | 2026-10-10 | ACIK, 2 |

`Oku` yalnız geçerli satırları içeren diziyi döndürür. `Tekil` seçilen sütunları birlikte anahtar sayar; `Bul` ilk sütunda doğrusal arar, yoksa -1 verir. `Sira` kaynak kayıtları değiştirmeden kod artan eklemeli sıralamayla dizin dizisi döndürür. `Satir` yalnız bellek raporunu değiştirir; `Yaz` dış dosyayı oluşturur. Hazır LINQ, GroupBy, Sort veya arama algoritması kullanılmaz.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 6) throw new FormatException(
        "6 argüman gerekli.");
    checked
    {
        int[][] uye = Oku(args[0], new[] { 1 }, new[] { 9999 });
        int[][] kitap = Oku(args[1], new[] { 1 }, new[] { 9999 });
        int[][] odunc = Oku(args[2], new[] { 1, 1, 1, 0, 0 },
            new[] { 9999, 9999, 9999, -1, -1 });
        int[][] iade = Oku(args[3], new[] { 1, 0 }, new[] { 9999, -1 });
        int kesim = Tarih("kesim", 1, 1, args[4]);
        Tekil(uye, args[0], 0); Tekil(kitap, args[1], 0);
        Tekil(odunc, args[2], 0); Tekil(iade, args[3], 0);
        for (int i = 0; i < odunc.Length; i++)
        {
            if (Bul(uye, odunc[i][1]) < 0)
                Hata(args[2], i + 1, 2, "Üye bulunamadı.");
            if (Bul(kitap, odunc[i][2]) < 0)
                Hata(args[2], i + 1, 3, "Kitap bulunamadı.");
            if (odunc[i][4] < odunc[i][3])
                Hata(args[2], i + 1, 5, "Son tarih alıştan önce.");
        }

        int[] iadeGun = new int[odunc.Length];
        for (int i = 0; i < iade.Length; i++)
        {
            int p = Bul(odunc, iade[i][0]);
            if (p < 0) Hata(args[3], i + 1, 1, "Ödünç bulunamadı.");
            if (iade[i][1] < odunc[p][3])
                Hata(args[3], i + 1, 2, "İade alıştan önce.");
            iadeGun[p] = iade[i][1];
        }

        var rapor = new StringBuilder();
        foreach (int p in Sira(odunc))
        {
            if (odunc[p][3] > kesim) continue;
            bool dondu = iadeGun[p] != 0 && iadeGun[p] <= kesim;
            int son = dondu ? iadeGun[p] : kesim;
            int gecikme = son > odunc[p][4] ? son - odunc[p][4] : 0;
            Satir(rapor, odunc[p][0], odunc[p][1], odunc[p][2],
                dondu ? "IADE" : "ACIK", gecikme);
        }

        Yaz(args[5], rapor.ToString());
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

uyeler.txt dosyasının tam içeriği:

```text
2
```

kitaplar.txt dosyasının tam içeriği:

```text
7
```

oduncler.txt dosyasının tam içeriği:

```text
2;2;7;2026-10-02;2026-10-08
1;2;7;2026-10-01;2026-10-07
```

iadeler.txt dosyasının tam içeriği:

```text
1;2026-10-09
2;2026-10-12
```

Yeni rapor.txt dosyasının tam içeriği:

```text
1;2;7;IADE;2
2;2;7;ACIK;2
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Sınır örneği: aynı dosya adlarını ayrı boş bir klasörde hazırlayın; aynı komutu, kesim tarihini `2000-02-29` yaparak çalıştırın.

uyeler.txt dosyasının tam içeriği:

```text
9999
```

kitaplar.txt dosyasının tam içeriği:

```text
9999
```

oduncler.txt dosyasının tam içeriği:

```text
9999;9999;9999;2000-02-29;2000-02-29
```

iadeler.txt sıfır bayttır.

Yeni rapor.txt dosyasının tam içeriği:

```text
9999;9999;9999;ACIK;0
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçersiz örnek: ayrı bir klasörde, aynı komut, normal örneğin kesim tarihiyle.

uyeler.txt dosyasının tam içeriği:

```text
2
```

kitaplar.txt dosyasının tam içeriği:

```text
7
```

oduncler.txt dosyasının tam içeriği:

```text
1;2;7;2026-10-01;2026-10-07
```

iadeler.txt dosyasının tam içeriği:

```text
1;2026-09-30
```

Rapor oluşturulmaz. Konsolun tam çıktısı:

```text
Hata: iadeler.txt: satır 1, alan 2: İade alıştan önce.
```

Bütün giriş dosyaları sıfır bayt ve rapor.txt yokken, kesim tarihi normal örnekteki gibi geçerliyken yeni rapor sıfır bayttır.

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçerli normal girdilerle rapor.txt önceden varsa mevcut baytlar korunur. Konsolun tam çıktısı:

```text
Hata: Dosya işlemi başarısız.
```

## Sınır durumları

- Dört dosyanın her biri en fazla 100 satırdır. Gün numarası farkı 2000–2099 içinde en fazla 36524; int yeterlidir. Her dosya boşken rapor boştur. Üye veya kitap boşken ödünç kaydı hata olur. İade dosyası boş olabilir; olmayan ödünç için iade hata olur. Kesim tarihinden sonraki kayıtlar da rapor dışında kalsa bile doğrulanır.
- Satır kapasitesinin tam dolması kabul edilir; bir sonraki satır saklanmadan hata olur. Tek bir boş satır sıfır kayıt sayılmaz; alan/sayı hatasıdır. Çok büyük veya taşan sayısal alan ayrıştırma hatasıdır.
- Aynı sayıyı farklı baştaki sıfırlarla yazmak anahtar tekrarını gizlemez. Sıralama sırasında dizinler taşındığı için dosya satırı ile hata konumu ve ilişkiler değişmez.
- Hatalı son kayıt dahil bütün giriş doğrulanmadan rapor açılmaz. Ortak I/O ve çıktı güvenliği sözleşmesi yukarıda tanımlıdır.

## Kazanımlar

- Eksik ilişki ile isteğe bağlı ilişkiyi ayırma; kesim tarihiyle geçmiş görünüm kurma; dosyadaki sıra ile kimlik sırasını ayırt etme.
- Ödünçün alış, son tarih ve iade durumu aynı gecikme davranışında birleşir. Sonraki derste bu alanları bir nesnede korumak, tarih kurallarının farklı raporlarda tekrar edilmesini azaltabilir.

## Alıştırmalar

1. Üye başına açık ve gecikmiş ödünç sayısını ikinci bir bellek raporunda üretin.
2. Kesim tarihinden sonraki iadenin neden ACIK sayıldığını açıklayın; bir Ödünç nesnesi hangi tarih tutarlılığını koruyabilir?

