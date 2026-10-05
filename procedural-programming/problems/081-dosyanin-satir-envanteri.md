---
id: "081"
order: 81
title: "Dosyanın satır envanteri"
level: "İleri"
prerequisites: ["062","073"]
concepts: ["ReadLine ve EOF","Fiziksel satır","UTF-8","using ile kaynak kapatma"]
---

# Dosyanın satır envanteri

## Problem tanımı

Bir metin dosyasını inceleyen araç yazın. Fiziksel satır sayısını, uzunluğu sıfır olan boş satırları ve en uzun satırın uzunluğu ile ilk satır numarasını bulun. Boşluk içeren satır boş sayılmaz. Metni değiştirmeyin; amaç dosyanın yapısını ölçmektir.

## Girdi ve çıktı

Tek yol: `ornek-081/metin.txt`. Dosyada serbest UTF-8 metin bulunabilir. Çıktı dört satırdır: `Satır`, `Boş`, `En uzun`, `İlk konum`. Boş dosyada ilk konum 0 olur; bu gerçek bir satır numarası değildir.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

## Algoritma

1. Tek yol argümanını denetleyip dosyayı `Oku` ile okuyun.
2. Her fiziksel satır için boşluk ve uzunluk ölçülerini güncelleyin.
3. Yalnız daha uzun satır geldiğinde adayın konumunu değiştirin.
4. Dosyanın tamamı doğrulanınca dört ölçüyü yazın.

| Satır | Metin | Boş sayısı | En uzun | İlk konum |
| --- | --- | --- | --- | --- |
| 1 | `Ada` | 0 | 3 | 1 |
| 2 | boş | 1 | 3 | 1 |
| 3 | `Deniz` | 1 | 5 | 3 |
| 4 | `Ekin` | 1 | 5 | 3 |

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
    string[] satirlar = Oku(args[0], out int n);
    int bos = 0, uzun = 0, ilk = 0;
    for (int i = 0; i < n; i++)
    {
        if (satirlar[i].Length == 0) bos++;
        if (ilk == 0 || satirlar[i].Length > uzun)
        {
            uzun = satirlar[i].Length;
            ilk = i + 1;
        }
    }
    Console.WriteLine("Satır: " + n.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Boş: " + bos.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("En uzun: " + uzun.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("İlk konum: " + ilk.ToString(CultureInfo.InvariantCulture));
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
```

## Örnek çalıştırmalar

`dotnet run -- ornek-081/metin.txt`; dosya (2. satır boştur):

```text
Ada

Deniz
Ekin
```

Konsolun tam çıktısı:

```text
Satır: 4
Boş: 1
En uzun: 5
İlk konum: 3
```

Aynı yol, sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Satır: 0
Boş: 0
En uzun: 0
İlk konum: 0
```

Dosyada yalnız bir boş satır (tek LF):

Konsolun tam çıktısı:

```text
Satır: 1
Boş: 1
En uzun: 0
İlk konum: 1
```

Dosyanın ilk satırı 121 adet A:

Konsolun tam çıktısı:

```text
Hata: metin.txt: satır 1: Satır en fazla 120 karakter.
```

## Sınır durumları

- Yinelenen satırlar geçerlidir; bu problemde anahtar yoktur. Eşit uzunlukta ilk satır korunur.
- Uzunluk görsel harf sayısı değildir: emoji gibi bir simge iki UTF-16 kod birimi kullanabilir.
- Sayaçlar en fazla 100, uzunluk en fazla 120 olduğundan int taşması olmaz.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- EOF ile boş fiziksel satırı ayrı ele alma.
- Dosya okuma ve ölçüm işlemlerini static yerel fonksiyonlarla ayırma.
- Hata halinde using ile kaynağın kapandığını açıklama.

## Alıştırmalar

1. Boşluklardan oluşan satırları ayrı bir sayaçla raporlayın.
2. Tamamen boş satırlardan oluşan dosyada ilk konumun neden 1 olduğunu açıklayın.
