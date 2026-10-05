---
id: "092"
order: 92
title: "Salon rezervasyonlarında çakışma taraması"
level: "İleri"
prerequisites: ["053","084","088","089"]
concepts: ["Yarı açık zaman aralığı","İkili çakışma","İlişki doğrulama","Çok ölçütlü sıralama"]
---

# Salon rezervasyonlarında çakışma taraması

## Problem tanımı

Salon kapasitesi ile rezervasyonları birleştirin. Geçerli rezervasyonların aynı salonda ve günde çakışan bütün çiftlerini bulun. Çakışma, raporlanacak normal bir sonuçtur; bozuk girdi değildir. Her çifti yalnız bir kez yazın. Aralıkların bitişi hariç olduğu için art arda toplantılar çakışmaz.

## Girdi ve çıktı

`dotnet run -- ornek-092/salonlar.txt ornek-092/rezervasyonlar.txt ornek-092/rapor.txt`

`salonlar.txt`: `salonKod;kapasite` (kod 1..9999, kapasite 1..1000). `rezervasyonlar.txt`: `rezKod;salonKod;tarih;başlangıçDakika;bitişDakika;katılımcı` (kodlar 1..9999, başlangıç 0..1439, bitiş 1..1440, katılımcı 1..1000). Tarih ortak ISO sözleşmesindedir. Salon ve rezervasyon kodları benzersizdir. Salon bulunmalı, başlangıç bitişten küçük ve katılımcı salon kapasitesini aşmamalıdır.

Rapor `küçükRezKod;büyükRezKod;ortakDakika` biçimindedir. Önce küçük, sonra büyük rezervasyon koduna göre artar. `max(başlangıçlar) < min(bitişler)` ise çakışma vardır. Farklı salon veya tarihte aralıklar karşılaştırılmaz. Çakışma yoksa tam içerik `CAKISMA_YOK` ve LF'dir; iki dosya boşken de aynıdır. Sıralama eşitliğinde ikinci kod kullanılır; aynı çift bir kez vardır.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.

## Algoritma

1. Tabloları ve tekil kodları doğrulayın.
2. Her rezervasyonun salonunu, zaman aralığını ve kapasitesini doğrulayın.
3. Rezervasyon dizinlerini kodlarına göre sıralayın; asıl kayıtları taşımayın.
4. Sıralı dizinlerde i < j bütün çiftlerini tarayın; salon ve tarih aynıysa yarı açık kesişimi hesaplayın.
5. Tüm çakışmaları bellek raporunda biriktirip yeni dosyaya yazın.

| Çift | Aralıklar | Kesişim | Sonuç |
| --- | --- | --- | --- |
| 2 ve 3 | `[60,120)`, `[90,150)` | `[90,120)` | 30 dakika |
| 2 ve 4 | `[60,120)`, `[120,180)` | boş | çakışmaz |
| 3 ve 4 | `[90,150)`, `[120,180)` | `[120,150)` | 30 dakika |

`Oku` yalnız geçerli satırları içeren diziyi döndürür. `Tekil` seçilen sütunları birlikte anahtar sayar; `Bul` ilk sütunda doğrusal arar, yoksa -1 verir. `Sira` kaynak kayıtları değiştirmeden kod artan eklemeli sıralamayla dizin dizisi döndürür. `Satir` yalnız bellek raporunu değiştirir; `Yaz` dış dosyayı oluşturur. Hazır LINQ, GroupBy, Sort veya arama algoritması kullanılmaz.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 3) throw new FormatException(
        "3 argüman gerekli.");
    checked
    {
        int[][] salon = Oku(args[0], new[] { 1, 1 },
            new[] { 9999, 1000 });
        int[][] rez = Oku(args[1], new[] { 1, 1, 0, 0, 1, 1 },
            new[] { 9999, 9999, -1, 1439, 1440, 1000 });
        Tekil(salon, args[0], 0); Tekil(rez, args[1], 0);
        for (int i = 0; i < rez.Length; i++)
        {
            int p = Bul(salon, rez[i][1]);
            if (p < 0) Hata(args[1], i + 1, 2, "Salon bulunamadı.");
            if (rez[i][3] >= rez[i][4])
                Hata(args[1], i + 1, 5, "Bitiş başlangıçtan büyük olmalı.");
            if (rez[i][5] > salon[p][1])
                Hata(args[1], i + 1, 6, "Salon kapasitesi yetersiz.");
        }

        int[] sira = Sira(rez);
        var rapor = new StringBuilder();
        for (int i = 0; i < sira.Length; i++)
            for (int j = i + 1; j < sira.Length; j++)
            {
                int[] a = rez[sira[i]], b = rez[sira[j]];
                if (a[1] != b[1] || a[2] != b[2]) continue;
                int ortak = Kesisim(a[3], a[4], b[3], b[4]);
                if (ortak > 0) Satir(rapor, a[0], b[0], ortak);
            }
        if (rapor.Length == 0) rapor.Append("CAKISMA_YOK\n");

        Yaz(args[2], rapor.ToString());
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

static int Kesisim(int a, int b, int c, int d)
{
    int bas = a > c ? a : c, son = b < d ? b : d;
    return son > bas ? son - bas : 0;
}

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

salonlar.txt dosyasının tam içeriği:

```text
7;10
```

rezervasyonlar.txt dosyasının tam içeriği:

```text
4;7;2026-10-05;120;180;8
2;7;2026-10-05;60;120;10
3;7;2026-10-05;90;150;5
```

Yeni rapor.txt dosyasının tam içeriği:

```text
2;3;30
3;4;30
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Sınır örneği: aynı dosya adlarını ayrı boş bir klasörde hazırlayın; aynı komutu çalıştırın.

salonlar.txt dosyasının tam içeriği:

```text
9999;1000
```

rezervasyonlar.txt dosyasının tam içeriği:

```text
9999;9999;2099-12-31;0;1440;1000
```

Yeni rapor.txt dosyasının tam içeriği:

```text
CAKISMA_YOK
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçersiz örnek: ayrı bir klasörde, aynı komut.

salonlar.txt dosyasının tam içeriği:

```text
7;10
```

rezervasyonlar.txt dosyasının tam içeriği:

```text
1;7;2026-10-05;60;120;11
```

Rapor oluşturulmaz. Konsolun tam çıktısı:

```text
Hata: rezervasyonlar.txt: satır 1, alan 6: Salon kapasitesi yetersiz.
```

Bütün giriş dosyaları sıfır bayt ve rapor.txt yokken yeni raporun tam içeriği:

```text
CAKISMA_YOK
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçerli normal girdilerle rapor.txt önceden varsa mevcut baytlar korunur. Konsolun tam çıktısı:

```text
Hata: Dosya işlemi başarısız.
```

## Sınır durumları

- 100 rezervasyonda en fazla 4950 çift üretilir; kayıt kapasitesi 100 olsa da sonuç satırı sayısı 4950 olabilir. Dakika farkı en fazla 1440; int taşmaz. Çapraz gece aralığı kabul edilmez, iki güne bölünmelidir. Salon boş ve rezervasyon doluysa eksik ilişki hatasıdır. Boş rezervasyon dosyası CAKISMA_YOK üretir.
- Satır kapasitesinin tam dolması kabul edilir; bir sonraki satır saklanmadan hata olur. Tek bir boş satır sıfır kayıt sayılmaz; alan/sayı hatasıdır. Çok büyük veya taşan sayısal alan ayrıştırma hatasıdır.
- Aynı sayıyı farklı baştaki sıfırlarla yazmak anahtar tekrarını gizlemez. Sıralama sırasında dizinler taşındığı için dosya satırı ile hata konumu ve ilişkiler değişmez.
- Hatalı son kayıt dahil bütün giriş doğrulanmadan rapor açılmaz. Ortak I/O ve çıktı güvenliği sözleşmesi yukarıda tanımlıdır.

## Kazanımlar

- Yarı açık aralıkların uç eşitliğini açıklama; her çifti bir kez tarama; fiziksel dosya sırasından bağımsız rapor düzeni kurma.
- Rezervasyonun salon, tarih ve iki zaman alanı ile çakışma davranışı birlikte kullanılır. Sonraki derste bunları aynı nesnede tutmak, bitişi başlangıçtan önce olan bir rezervasyonun uygulamada dolaşmasını engellemeyi kolaylaştırabilir.

## Alıştırmalar

1. Çakışan çift başına toplam katılımcı sayısını ekleyin; bunun salon kapasitesi aşımı kararından nasıl ayrıldığını açıklayın.
2. Çakışma metodunun konsoldan bağımsız sözleşmesini yazın ve nesneye taşındığında hangi verileri kullanacağını belirtin.

