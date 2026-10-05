---
id: "087"
order: 87
title: "Hesap hareketlerini sırayla denetleme"
level: "İleri"
prerequisites: ["080","083"]
concepts: ["Sıralı durum geçişi","İşlem kimliği","İş kuralı doğrulama","Kısmi sonuçtan kaçınma"]
---

# Hesap hareketlerini sırayla denetleme

## Problem tanımı

Başlangıç bakiyesi sıfır olan bir deneme hesabının hareket dosyasını denetleyin. Para yatırma ve çekme işlemlerini dosya sırasıyla uygulayın; hiçbir ara bakiye negatif veya bir milyonun üstünde olmasın. İşlem kodu tekrarlanırsa aynı hareket iki kez uygulanmasın.

## Girdi ve çıktı

Tek yol: `ornek-087/hareket.txt`. Satır `işlemKodu;tür;tutar`: kod 1..9999 benzersiz, tür tam D (yatırma) veya W (çekme), tutar 0.01..10000 ondalıktır. Başlık yoktur. Bakiye her adımda 0..1000000 aralığındadır. Gerçek banka hesabına erişim yoktur; yalnız verilen deneme dosyası hesaplanır. Boş dosyada hareket 0, bakiye 0.00 yazılır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Sayısal alanlarda çevre boşluğu, binlik ayırıcı, üs gösterimi ve virgüllü ondalık kabul edilmez. Tamsayıda isteğe bağlı + veya - işareti ve ASCII rakamlar kullanılır; baştaki sıfırlar kabul edilir. Ondalıkta isteğe bağlı işaret, en az bir tam kısım rakamı ve varsa noktadan sonra 1..2 rakam vardır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır. Taşan sayı ayrıştırma hatasıdır.

## Algoritma

1. Alanları ve işlem kimliğini doğrulayın; kodun önceki hareketlerde bulunmadığını denetleyin.
2. Türü ve tutarı okuyun; D için ekleyip W için çıkararak aday bakiye oluşturun.
3. Aday sınırı ihlal ediyorsa o satırda durun; bakiye değişmesin.
4. Adayı kabul edin; bütün dosya geçince hareket adedi ve son bakiyeyi yazın.

| Satır | Hareket | Önceki bakiye | Aday | Karar |
| --- | --- | --- | --- | --- |
| 1 | D 20.00 | 0.00 | 20.00 | kabul |
| 2 | W 7.50 | 20.00 | 12.50 | kabul |
| 3 | W 12.50 | 12.50 | 0.00 | kabul |

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
    decimal bakiye = 0;
    for (int i = 0; i < n; i++)
    {
        string[] a = Alanlar(args[0], i + 1, s[i], 3);
        int kod = Tam(args[0], i + 1, a[0], 1, 9999);
        if (Bul(kodlar, i, kod) >= 0)
            Hata(args[0], i + 1, "Yinelenen kod.");
        if (a[1] != "D" && a[1] != "W")
            Hata(args[0], i + 1, "Tür D veya W olmalı.");
        decimal tutar = Ondalik(args[0], i + 1, a[2], 0.01m, 10000);
        decimal aday = a[1] == "D" ? bakiye + tutar : bakiye - tutar;
        if (aday < 0 || aday > 1000000)
            Hata(args[0], i + 1, "Bakiye sınırı aşıldı.");
        bakiye = aday;
        kodlar[i] = kod;
    }
    Console.WriteLine("Hareket: " + n.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Bakiye: " + bakiye.ToString("0.00", CultureInfo.InvariantCulture));
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

`dotnet run -- ornek-087/hareket.txt`; dosya:

```text
10;D;20
11;W;7.50
12;W;12.50
```

Konsolun tam çıktısı:

```text
Hareket: 3
Bakiye: 0.00
```

Sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Hareket: 0
Bakiye: 0.00
```

Yetersiz bakiye:

```text
1;D;5
2;W;5.01
```

Konsolun tam çıktısı:

```text
Hata: hareket.txt: satır 2: Bakiye sınırı aşıldı.
```

Yinelenen hareket:

```text
1;D;5
1;D;3
```

Konsolun tam çıktısı:

```text
Hata: hareket.txt: satır 2: Yinelenen kod.
```

## Sınır durumları

- W ilk satırda geçerli pozitif tutarla daima bakiye hatasıdır. Tam bakiyeyi çekmek geçerlidir; sıfır tutar geçersizdir.
- Boş satır alan sayısı hatasıdır. d veya w küçük harfleri kabul edilmez.
- En fazla 100 × 10000 = 1000000 yatırılabilir; decimal ara hesapları taşmaz. Üst iş kuralı sınırı da açıkça denetlenir.
- Önceki geçerli hareketlerin özeti hata halinde yazılmaz; giriş dosyası hiçbir durumda güncellenmez.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Kayıt doğruluğu ile iş kuralı doğruluğunu ayrı ele alma.
- Önce aday durumu hesaplayıp kabul edildiğinde atama yapma.
- İşlem sırasının sonuç üzerindeki etkisini açıklama.

## Alıştırmalar

1. Kabul edilen ara bakiyeleri yalnız tam başarıda yazılan bir izleme raporuna ekleyin.
2. Başlangıç bakiyesini ikinci argümandan almak için sözleşme oluşturun.
