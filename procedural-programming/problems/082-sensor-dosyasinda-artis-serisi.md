---
id: "082"
order: 82
title: "Sensör dosyasında artış serisi"
level: "İleri"
prerequisites: ["021","075","081"]
concepts: ["Satır doğrulama","Ardışık ölçümler","out ile okunan adet","Kültürden bağımsız çıktı"]
---

# Sensör dosyasında artış serisi

## Problem tanımı

Her satırında bir tamsayı ölçüm olan sensör dosyasından toplamı, ortalamayı ve ardışık kesin artan en uzun ölçüm serisinin uzunluğunu bulun. Eşit veya azalan ölçüm seriyi yeniden 1 yapar. Sonuç bir zaman serisinin sürekliliğini ölçer; değerleri sıralamayın.

## Girdi ve çıktı

Tek yol: `ornek-082/olcum.txt`. Her satır -1000..1000 tamsayısıdır; boş satır geçersizdir. Boş dosya kabul edilir: toplam 0, ortalama Yok, seri uzunluğu 0. Ortalama iki ondalık basamakla yazılır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Ölçüm isteğe bağlı + veya - işareti ve ASCII rakamlardan oluşur; baştaki sıfırlar kabul edilir. Çevre boşluğu, binlik ayırıcı ve ondalık ölçüm geçersizdir. Taşan sayı ayrıştırma hatasıdır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır.

## Algoritma

1. Dosyayı okuyun; her satırı aralığıyla doğrulayın.
2. Toplama ekleyin. İlk ölçümde veya artış bozulduğunda seriyi 1 yapın.
3. Artışta seriyi uzatıp en uzun adayı güncelleyin.
4. EOF sonrasında boş veri durumunu ayırıp özeti yazın.

| Ölçüm | Seri uzunluğu | En uzun | Toplam |
| --- | --- | --- | --- |
| 2 | 1 | 1 | 2 |
| 5 | 2 | 2 | 7 |
| 5 | 1 | 2 | 12 |
| 8 | 2 | 2 | 20 |
| 9 | 3 | 3 | 29 |

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
    int toplam = 0, onceki = 0, seri = 0, enUzun = 0;
    for (int i = 0; i < n; i++)
    {
        int x = Tam(args[0], i + 1, s[i], -1000, 1000);
        toplam += x;
        seri = i > 0 && x > onceki ? seri + 1 : 1;
        if (seri > enUzun) enUzun = seri;
        onceki = x;
    }
    Console.WriteLine("Toplam: " + toplam.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Ortalama: " + (n == 0 ? "Yok" :
        ((decimal)toplam / n).ToString("0.00", CultureInfo.InvariantCulture)));
    Console.WriteLine("Artan seri: " + enUzun.ToString(CultureInfo.InvariantCulture));
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



static int Tam(string yol, int satir, string metin,
    int alt, int ust)
{
    if (!int.TryParse(metin, NumberStyles.AllowLeadingSign,
        CultureInfo.InvariantCulture, out int deger) ||
        deger < alt || deger > ust)
        Hata(yol, satir, "Geçersiz tamsayı.");
    return deger;
}
```

## Örnek çalıştırmalar

`dotnet run -- ornek-082/olcum.txt`; dosya:

```text
2
5
5
8
9
```

Konsolun tam çıktısı:

```text
Toplam: 29
Ortalama: 5.80
Artan seri: 3
```

Sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Toplam: 0
Ortalama: Yok
Artan seri: 0
```

Tek ölçüm:

```text
-1000
```

Konsolun tam çıktısı:

```text
Toplam: -1000
Ortalama: -1000.00
Artan seri: 1
```

Geçersiz ikinci satır:

```text
1
x
```

Konsolun tam çıktısı:

```text
Hata: olcum.txt: satır 2: Geçersiz tamsayı.
```

## Sınır durumları

- Tekrar eden değerler geçerlidir fakat kesin artışı bozar; anahtar yoktur.
- Boş satır EOF değildir ve tamsayı hatasıdır. 1001 veya int sınırını aşan belirteç de aynı hatayı verir.
- Toplamın mutlak değeri en fazla 100000, seri uzunluğu en fazla 100 olur.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Dosya sırasını anlamlı bir veri ilişkisi olarak koruma.
- Sonuçları bütün satırlar doğrulandıktan sonra yazma.
- Önceki değer ile geçerli seri durumunu birlikte izleme.

## Alıştırmalar

1. En uzun serinin ilk başlangıç satırını da raporlayın.
2. Eşit değerlere izin verilen azalmayan seri için koşulu değiştirin.
