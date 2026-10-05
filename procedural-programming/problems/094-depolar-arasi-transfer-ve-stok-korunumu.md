---
id: "094"
order: 94
title: "Depolar arası transfer ve stok korunumu"
level: "İleri"
prerequisites: ["053","083","087","089"]
concepts: ["İki boyutlu stok","Transfer güncellemesi","Korunum denetimi","Bileşik anahtar"]
---

# Depolar arası transfer ve stok korunumu

## Problem tanımı

Depo ve ürün tanımlarından bir stok matrisi kurun. Açılış sayımlarını yükledikten sonra transferleri dosya sırasında uygulayın. Her transfer bir depodan eksiltirken diğerine aynı miktarı eklemelidir. Sadece son bakiyeleri denetlemek yeterli değildir; ara adımda kaynak stok yetmezse işlem hattını durdurun.

## Girdi ve çıktı

`dotnet run -- ornek-094/depolar.txt ornek-094/urunler.txt ornek-094/acilis.txt ornek-094/transferler.txt ornek-094/rapor.txt`

`depolar.txt`: `depoKod`; `urunler.txt`: `ürünKod`; kodlar 1..9999 ve tekildir. `acilis.txt`: `depoKod;ürünKod;adet` (adet 0..1000000); depo/ürün çifti tekildir, verilmemiş çiftin açılışı sıfırdır. `transferler.txt`: `transferKod;kaynakDepo;hedefDepo;ürünKod;adet` (kodlar 1..9999, adet 1..1000000). Transfer kodu tekil; depo ve ürünler bulunmalı, kaynak ve hedef farklı olmalıdır.

Transfer sırası dosya sırasıdır; transfer koduna göre işlem sıralaması yapılmaz. Kaynak stok yetmiyorsa hata; hedef stokun en fazla 1000000 sınırını aşması da hatadır. İki koşul denetlenmeden hücrelerin hiçbiri değiştirilmez. Başarılı rapor tüm depo/ürün çiftlerini, stok sıfır olsa da `depoKod;ürünKod;sonAdet` biçiminde yazar; depo, ardından ürün kodu artar. Ürün bazında bütün depoların toplamı açılış toplamına eşit kalmalıdır.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.

## Algoritma

1. Dört tabloyu ve tekil/bileşik anahtarları doğrulayın.
2. Açılış ilişkilerini eşleyip stok ve ürün başına long açılış toplamlarını kurun.
3. Transferi iki depo ve bir ürüne eşleyin; kaynak yeterliliği ve hedef üst sınırını birlikte kontrol edin.
4. Eksiltme ve eklemeyi uygulayın; her hücre için 0..1000000 değişmezini koruyun.
5. Ürün toplamını yeniden hesaplayıp korunum denetleyin; depo ve ürün dizinlerini ayrı sıralayıp matrisi raporlayın.

| İşlem | Depo 1 / ürün 7 | Depo 2 / ürün 7 | Toplam |
| --- | --- | --- | --- |
| Açılış | 10 | 0 | 10 |
| 1 → 2, adet 4 | 6 | 4 | 10 |
| 2 → 1, adet 1 | 7 | 3 | 10 |

`Oku` yalnız geçerli satırları içeren diziyi döndürür. `Tekil` seçilen sütunları birlikte anahtar sayar; `Bul` ilk sütunda doğrusal arar, yoksa -1 verir. `Sira` kaynak kayıtları değiştirmeden kod artan eklemeli sıralamayla dizin dizisi döndürür. `Satir` yalnız bellek raporunu değiştirir; `Yaz` dış dosyayı oluşturur. Hazır LINQ, GroupBy, Sort veya arama algoritması kullanılmaz.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 5) throw new FormatException(
        "5 argüman gerekli.");
    checked
    {
        int[][] depo = Oku(args[0], new[] { 1 }, new[] { 9999 });
        int[][] urun = Oku(args[1], new[] { 1 }, new[] { 9999 });
        int[][] acilis = Oku(args[2], new[] { 1, 1, 0 },
            new[] { 9999, 9999, 1000000 });
        int[][] aktar = Oku(args[3], new[] { 1, 1, 1, 1, 1 },
            new[] { 9999, 9999, 9999, 9999, 1000000 });
        Tekil(depo, args[0], 0); Tekil(urun, args[1], 0);
        Tekil(acilis, args[2], 0, 1); Tekil(aktar, args[3], 0);
        int[,] stok = new int[depo.Length, urun.Length];
        long[] ilk = new long[urun.Length];
        for (int i = 0; i < acilis.Length; i++)
        {
            int d = Bul(depo, acilis[i][0]), u = Bul(urun, acilis[i][1]);
            if (d < 0) Hata(args[2], i + 1, 1, "Depo bulunamadı.");
            if (u < 0) Hata(args[2], i + 1, 2, "Ürün bulunamadı.");
            stok[d, u] = acilis[i][2]; ilk[u] += acilis[i][2];
        }

        for (int i = 0; i < aktar.Length; i++)
        {
            int a = Bul(depo, aktar[i][1]), b = Bul(depo, aktar[i][2]);
            int u = Bul(urun, aktar[i][3]), miktar = aktar[i][4];
            if (a < 0) Hata(args[3], i + 1, 2, "Depo bulunamadı.");
            if (b < 0) Hata(args[3], i + 1, 3, "Depo bulunamadı.");
            if (u < 0) Hata(args[3], i + 1, 4, "Ürün bulunamadı.");
            if (a == b) Hata(args[3], i + 1, 3, "Depolar farklı olmalı.");
            if (stok[a, u] < miktar)
                Hata(args[3], i + 1, 5, "Kaynak stok yetersiz.");
            if (stok[b, u] > 1000000 - miktar)
                Hata(args[3], i + 1, 5, "Hedef stok sınırı aşıldı.");
            stok[a, u] -= miktar; stok[b, u] += miktar;
        }

        for (int u = 0; u < urun.Length; u++)
        {
            long son = 0;
            for (int d = 0; d < depo.Length; d++) son += stok[d, u];
            if (son != ilk[u]) throw new FormatException("Korunum bozuldu.");
        }
        var rapor = new StringBuilder();
        int[] ds = Sira(depo), us = Sira(urun);
        foreach (int d in ds)
            foreach (int u in us) Satir(rapor, depo[d][0], urun[u][0], stok[d, u]);

        Yaz(args[4], rapor.ToString());
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

depolar.txt dosyasının tam içeriği:

```text
2
1
```

urunler.txt dosyasının tam içeriği:

```text
7
```

acilis.txt dosyasının tam içeriği:

```text
1;7;10
```

transferler.txt dosyasının tam içeriği:

```text
9;1;2;7;4
3;2;1;7;1
```

Yeni rapor.txt dosyasının tam içeriği:

```text
1;7;7
2;7;3
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Sınır örneği: aynı dosya adlarını ayrı boş bir klasörde hazırlayın; aynı komutu çalıştırın.

depolar.txt dosyasının tam içeriği:

```text
9999
1
```

urunler.txt dosyasının tam içeriği:

```text
9999
```

acilis.txt dosyasının tam içeriği:

```text
1;9999;1000000
```

transferler.txt dosyasının tam içeriği:

```text
9999;1;9999;9999;1000000
```

Yeni rapor.txt dosyasının tam içeriği:

```text
1;9999;0
9999;9999;1000000
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçersiz örnek: ayrı bir klasörde, aynı komut.

depolar.txt dosyasının tam içeriği:

```text
1
2
```

urunler.txt dosyasının tam içeriği:

```text
7
```

acilis.txt dosyasının tam içeriği:

```text
1;7;10
```

transferler.txt dosyasının tam içeriği:

```text
1;2;1;7;1
```

Rapor oluşturulmaz. Konsolun tam çıktısı:

```text
Hata: transferler.txt: satır 1, alan 5: Kaynak stok yetersiz.
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

- 100 depo × 100 ürün en fazla 10000 rapor satırıdır. Açılış ve transfer dosyaları ayrı ayrı 100 satırdır. Her hücre en fazla 1000000; ürün toplamı en fazla 100000000, long ile tutulur. Depo veya ürün boşsa matris raporu boştur; dolu açılış/transferde eksik ilişki hatasıdır. Açılış dosyası boşken bütün stoklar sıfırdır. Sıfır transfer miktarı hata olur.
- Satır kapasitesinin tam dolması kabul edilir; bir sonraki satır saklanmadan hata olur. Tek bir boş satır sıfır kayıt sayılmaz; alan/sayı hatasıdır. Çok büyük veya taşan sayısal alan ayrıştırma hatasıdır.
- Aynı sayıyı farklı baştaki sıfırlarla yazmak anahtar tekrarını gizlemez. Sıralama sırasında dizinler taşındığı için dosya satırı ile hata konumu ve ilişkiler değişmez.
- Hatalı son kayıt dahil bütün giriş doğrulanmadan rapor açılmaz. Ortak I/O ve çıktı güvenliği sözleşmesi yukarıda tanımlıdır.

## Kazanımlar

- Tek hareketi iki bağlantılı güncellemeye ayrıştırma; ara durum ile son durumu ayırma; toplamın korunmasını ek güvence olarak kullanma.
- Stok hücresinin depo, ürün ve adet ilişkisi ile transferin iki taraflı davranışı birlikte değişir. Sonraki derste bu veriyi koruyan nesneler, bir tarafı güncelleyip diğerini unutma riskini azaltabilir; bu çözümde matrisi ve metot sözleşmelerini izleyin.

## Alıştırmalar

1. Her depo için kaç farklı üründe pozitif stok kaldığını rapora ekleyin.
2. Yalnız son stokların kontrol edilmesinin hangi hatalı transfer dizisini kaçıracağını örnekleyin; davranışı bir nesnede korumanın gerekçesini yazın.

