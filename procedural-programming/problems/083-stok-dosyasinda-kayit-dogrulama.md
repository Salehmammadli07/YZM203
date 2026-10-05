---
id: "083"
order: 83
title: "Stok dosyasında kayıt doğrulama"
level: "İleri"
prerequisites: ["068","075","082"]
concepts: ["Noktalı virgülle alan ayırma","Kayıt sözleşmesi","Yinelenen anahtar","decimal tutar"]
---

# Stok dosyasında kayıt doğrulama

## Problem tanımı

Küçük bir stok dosyasında ürünlerin adet ve birim bedellerini doğrulayın; ürün sayısını ve toplam stok değerini hesaplayın. Bir kayıt yalnız bütün alanları ve benzersiz ürün kodu kabul edildiğinde hesaplamaya girsin.

## Girdi ve çıktı

Tek yol: `ornek-083/stok.txt`. Başlık yoktur; her satır `kod;adet;bedel` biçimindedir. Kod 1..9999 benzersiz tamsayı, adet 0..1000 tamsayı, bedel 0..10000 ondalıktır. Tırnak ve kaçış yoktur. Kodun metinsel biçimi değil sayısal değeri anahtardır: 01 ile 1 aynı koddur. Boş dosyada ürün 0 ve değer 0.00 yazılır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Sayısal alanlarda çevre boşluğu, binlik ayırıcı, üs gösterimi ve virgüllü ondalık kabul edilmez. Tamsayıda isteğe bağlı + veya - işareti ve ASCII rakamlar kullanılır; baştaki sıfırlar kabul edilir. Ondalıkta isteğe bağlı işaret, en az bir tam kısım rakamı ve varsa noktadan sonra 1..2 rakam vardır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır. Taşan sayı ayrıştırma hatasıdır.

## Algoritma

1. Her satırı tam üç alana ayırın; eksik veya fazla ayırıcıyı reddedin.
2. Kod, adet ve bedeli sınırlarıyla doğrulayın.
3. Önceki kodlar dizisinde doğrusal arama yapın; tekrar varsa durun.
4. Kodu saklayıp adet × bedeli toplama ekleyin; sonunda iki sonucu yazın.

| Satır | Kod | Adet × bedel | Birikmiş değer |
| --- | --- | --- | --- |
| 1 | 12 | 3 × 2.50 | 7.50 |
| 2 | 7 | 2 × 4.00 | 15.50 |

`Oku` yalnız dosya sınırlarını ve kodlamayı denetler, `out n` ile geçerli satır adedini döndürür. 100 hücrelik dizinin yalnız ilk n hücresi okunabilir; kalan hücreler veri değildir. Hesaplama ana akışta ve göreve özgü static yerel fonksiyonlarda yapılır.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 1) throw new FormatException("Bir giriş yolu gerekli.");
    string[] s = Oku(args[0], out int n);
    int[] kodlar = new int[100];
    decimal toplam = 0;
    for (int i = 0; i < n; i++)
    {
        string[] a = Alanlar(args[0], i + 1, s[i], 3);
        int kod = Tam(args[0], i + 1, a[0], 1, 9999);
        int adet = Tam(args[0], i + 1, a[1], 0, 1000);
        decimal bedel = Ondalik(args[0], i + 1, a[2], 0, 10000);
        if (Bul(kodlar, i, kod) >= 0)
            Hata(args[0], i + 1, "Yinelenen kod.");
        kodlar[i] = kod;
        toplam += adet * bedel;
    }
    Console.WriteLine("Ürün: " + n.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Değer: " + toplam.ToString("0.00", CultureInfo.InvariantCulture));
}
catch (FormatException e) { Console.WriteLine("Hata: " + e.Message); }
catch (IOException) { Console.WriteLine("Hata: Dosya işlemi başarısız."); }
catch (UnauthorizedAccessException)
{ Console.WriteLine("Hata: Dosyaya erişim izni yok."); }
catch (ArgumentException)
{ Console.WriteLine("Hata: Yol veya UTF-8 kodlaması geçersiz."); }
catch (NotSupportedException)
{ Console.WriteLine("Hata: Yol biçimi desteklenmiyor."); }

static string[] Oku(string yol, out int n)
{
    string[] satirlar = new string[100];
    n = 0;
    using var akis = new FileStream(yol, FileMode.Open,
        FileAccess.Read, FileShare.Read);
    if (akis.Length > 32768) throw new FormatException(
        Path.GetFileName(yol) + ": Dosya 32768 baytı aşıyor.");
    using var okuyucu = new StreamReader(akis,
        new UTF8Encoding(false, true), false);
    string? satir;
    while ((satir = okuyucu.ReadLine()) is not null)
    {
        if (n == 0 && satir.StartsWith('\uFEFF'))
            satir = satir.Substring(1);
        if (n == 100) Hata(yol, n + 1, "En fazla 100 satır.");
        if (satir.Length > 120) Hata(yol, n + 1,
            "Satır en fazla 120 karakter.");
        satirlar[n++] = satir;
    }
    return satirlar;
}

static void Hata(string yol, int satir, string mesaj)
{
    throw new FormatException(Path.GetFileName(yol) +
        ": satır " + satir.ToString(CultureInfo.InvariantCulture) +
        ": " + mesaj);
}


static string[] Alanlar(string yol, int satir, string metin,
    int adet)
{
    string[] alan = metin.Split(';');
    if (alan.Length != adet)
        Hata(yol, satir, "Alan sayısı yanlış.");
    return alan;
}

static int Tam(string yol, int satir, string metin,
    int alt, int ust)
{
    if (!int.TryParse(metin, NumberStyles.AllowLeadingSign,
        CultureInfo.InvariantCulture, out int deger) ||
        deger < alt || deger > ust)
        Hata(yol, satir, "Geçersiz tamsayı.");
    return deger;
}


static decimal Ondalik(string yol, int satir, string metin,
    decimal alt, decimal ust)
{
    int bas = metin.StartsWith('-') || metin.StartsWith('+') ? 1 : 0;
    int nokta = -1, rakam = 0;
    for (int i = bas; i < metin.Length; i++)
    {
        if (metin[i] >= '0' && metin[i] <= '9') rakam++;
        else if (metin[i] == '.' && nokta == -1) nokta = i;
        else Hata(yol, satir, "Geçersiz ondalık.");
    }
    if (rakam == 0 || nokta == bas ||
        (nokta >= 0 && (metin.Length - nokta - 1 < 1 ||
        metin.Length - nokta - 1 > 2)))
        Hata(yol, satir, "Geçersiz ondalık.");
    if (!decimal.TryParse(metin,
        NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture, out decimal deger) ||
        deger < alt || deger > ust)
        Hata(yol, satir, "Geçersiz ondalık.");
    return deger;
}


static int Bul(int[] kodlar, int n, int kod)
{
    for (int i = 0; i < n; i++)
        if (kodlar[i] == kod) return i;
    return -1;
}
```

## Örnek çalıştırmalar

`dotnet run -- ornek-083/stok.txt`; dosya:

```text
12;3;2.50
7;2;4
```

Konsolun tam çıktısı:

```text
Ürün: 2
Değer: 15.50
```

Sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Ürün: 0
Değer: 0.00
```

Kod tekrarı:

```text
1;0;0
01;1;1
```

Konsolun tam çıktısı:

```text
Hata: stok.txt: satır 2: Yinelenen kod.
```

Virgüllü bedel:

```text
1;1;2,50
```

Konsolun tam çıktısı:

```text
Hata: stok.txt: satır 1: Geçersiz ondalık.
```

## Sınır durumları

- Boş satır alan sayısı hatasıdır. Boş alan, çevre boşluğu, üçten fazla alan veya geçersiz sayı kabul edilmez.
- Sıfır adet ve sıfır bedel geçerlidir; boş dosyayla tek sıfır kayıt aynı ürün sayısını vermez.
- En büyük toplam 100 × 1000 × 10000 = 1000000000 olur; decimal güvenle taşır.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Alan sayısını, değer aralığını ve anahtar benzersizliğini ayrı doğrulama.
- Paralel veri yerine yalnız gereken kod dizisini saklama.
- Ondalık dilbilgisini kültürden bağımsız tutma.

## Alıştırmalar

1. Hata mesajlarına 1 tabanlı alan numarası ekleyin.
2. En değerli ürünün ilk kodunu da raporlayın.
