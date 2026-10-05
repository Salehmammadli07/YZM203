---
id: "084"
order: 84
title: "Tarihli ölçümlerde eksik günler"
level: "İleri"
prerequisites: ["019","020","083"]
concepts: ["TryParseExact","Kesin artan tarih","Gün farkı","Eksik gözlem"]
---

# Tarihli ölçümlerde eksik günler

## Problem tanımı

Günde bir kez alınan sıcaklık kayıtlarında ilk ve son tarihi, kayıt sayısını ve arada ölçüm yapılmayan gün sayısını bulun. Tarihler kesin artan sırada olmalıdır. Aynı güne iki kayıt kabul etmeyin; dosyayı sıralamak veri üretim hatasını gizlemesin.

## Girdi ve çıktı

Tek yol: `ornek-084/gunler.txt`. Her satır `yyyy-MM-dd;sıcaklık` biçimindedir; tarih 0001-01-01..9999-12-31 arasında geçerli Gregoryen tarih, sıcaklık -100..100 ondalıktır. Başlık ve boş satır yoktur. Boş dosyada `Kayıt: 0` ve `Aralık: Yok`, `Eksik gün: 0` yazılır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Sayısal alanlarda çevre boşluğu, binlik ayırıcı, üs gösterimi ve virgüllü ondalık kabul edilmez. Tamsayıda isteğe bağlı + veya - işareti ve ASCII rakamlar kullanılır; baştaki sıfırlar kabul edilir. Ondalıkta isteğe bağlı işaret, en az bir tam kısım rakamı ve varsa noktadan sonra 1..2 rakam vardır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır. Taşan sayı ayrıştırma hatasıdır.

## Algoritma

1. Satırı iki alana ayırıp tarihi tam yyyy-MM-dd biçimiyle ve sıcaklığı doğrulayın.
2. İlk kaydın tarihini saklayın. Sonrakilerde tarih önceki tarihten büyük olmalıdır.
3. İki tarih arasındaki gün farkından 1 çıkarıp eksik güne ekleyin.
4. Son tarihi güncelleyin ve tüm dosya kabul edildiğinde aralığı yazın.

| Tarih | Öncekiyle fark | Yeni eksik | Birikmiş eksik |
| --- | --- | --- | --- |
| 2024-02-28 | ilk kayıt | 0 | 0 |
| 2024-03-01 | 2 | 1 | 1 |
| 2024-03-04 | 3 | 2 | 3 |

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
    DateTime ilk = default, onceki = default;
    int eksik = 0;
    for (int i = 0; i < n; i++)
    {
        string[] a = Alanlar(args[0], i + 1, s[i], 2);
        if (!DateTime.TryParseExact(a[0], "yyyy-MM-dd",
            CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTime tarih)) Hata(args[0], i + 1, "Geçersiz tarih.");
        Ondalik(args[0], i + 1, a[1], -100, 100);
        if (i == 0) ilk = tarih;
        else
        {
            if (tarih <= onceki)
                Hata(args[0], i + 1, "Tarihler kesin artmalı.");
            eksik += (tarih - onceki).Days - 1;
        }
        onceki = tarih;
    }
    Console.WriteLine("Kayıt: " + n.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Aralık: " + (n == 0 ? "Yok" :
        ilk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " / " +
        onceki.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
    Console.WriteLine("Eksik gün: " + eksik.ToString(CultureInfo.InvariantCulture));
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
```

## Örnek çalıştırmalar

`dotnet run -- ornek-084/gunler.txt`; dosya:

```text
2024-02-28;1.5
2024-03-01;-2
2024-03-04;0
```

Konsolun tam çıktısı:

```text
Kayıt: 3
Aralık: 2024-02-28 / 2024-03-04
Eksik gün: 3
```

Sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Kayıt: 0
Aralık: Yok
Eksik gün: 0
```

Tek tarih:

```text
0001-01-01;100
```

Konsolun tam çıktısı:

```text
Kayıt: 1
Aralık: 0001-01-01 / 0001-01-01
Eksik gün: 0
```

Geçersiz artık gün:

```text
2023-02-29;4
```

Konsolun tam çıktısı:

```text
Hata: gunler.txt: satır 1: Geçersiz tarih.
```

## Sınır durumları

- Aynı tarih ve geriye giden tarih aynı sıralılık hatasıdır. 2024-2-01, çevre boşluğu ve saat alanı kabul edilmez.
- Eksik gün yalnız ilk ve son kayıt arasında sayılır; dosya öncesi ve sonrası için gözlem dönemi varsayılmaz.
- DateTime farkı tüm kabul edilen aralıkta en fazla 3652058 gündür; toplam eksik de bu aralığı aşmaz. Son tarihe gün eklenmediği için üst tarih sınırında taşma yoktur.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Takvim doğrulamasını sabit bir metin biçimine bağlama.
- Sıralılık önkoşulunu dosya kaynağında denetleme.
- Eksik gözlem ile sıfır sıcaklık ölçümünü ayırt etme.

## Alıştırmalar

1. En uzun eksik gün aralığını başlangıç ve bitiş tarihleriyle raporlayın.
2. Aynı gün tekrarı ile geriye gidiş için farklı hata mesajları üretin.
