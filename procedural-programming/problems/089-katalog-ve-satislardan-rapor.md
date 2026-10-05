---
id: "089"
order: 89
title: "Katalog ve satışlardan rapor"
level: "İleri"
prerequisites: ["042","053","074","083","088"]
concepts: ["İlişkili dosyalar","Referans doğrulama","Paralel dizilerle sıralama","CreateNew ile rapor"]
---

# Katalog ve satışlardan rapor

## Problem tanımı

Ürün kataloğundaki birim bedelleri satış dosyasındaki adetlerle birleştirip ürün başına toplam satış adedi ve gelir raporu üretin. Katalogda satılmayan ürün de sıfır gelirle yer alsın. Bilinmeyen ürün kodunu sessizce atmayın; satış dosyasındaki konumuyla bildirin.

## Girdi ve çıktı

Üç yol: `ornek-089/katalog.txt ornek-089/satis.txt ornek-089/rapor.txt`. Katalog satırı `kod;bedel`: benzersiz kod 1..9999, bedel 0..10000. Satış satırı `kod;adet`: katalogda mevcut kod, adet 1..1000. Satış kodları tekrarlanabilir ve birleştirilir. Her iki dosya sırasız olabilir. Rapor kod sırasıyla `kod;toplamAdet;gelir` biçiminde, başlıksızdır. Konsola yalnız `Rapor yazıldı.` yazılır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Sayısal alanlarda çevre boşluğu, binlik ayırıcı, üs gösterimi ve virgüllü ondalık kabul edilmez. Tamsayıda isteğe bağlı + veya - işareti ve ASCII rakamlar kullanılır; baştaki sıfırlar kabul edilir. Ondalıkta isteğe bağlı işaret, en az bir tam kısım rakamı ve varsa noktadan sonra 1..2 rakam vardır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır. Taşan sayı ayrıştırma hatasıdır.

Çıktı yolu üçüncü argümandır; dosya henüz var olmamalıdır ve üst klasörünü siz önceden oluşturmalısınız. FileMode.CreateNew mevcut bir dosyayı girişle aynı yol olsa bile ezmez; bu durumda I/O hatası oluşur. Başarı mesajı yalnız yazıcı ve akış kapandıktan sonra verilir. Giriş doğrulama hatasında çıktı dosyası hiç açılmaz. Yazım sırasında disk dolması gibi I/O hatasında yeni dosya kısmi kalabilir; program başarı bildirmez ve bu yeni dosyayı siz denetleyip başka adla tekrar deneyebilirsiniz. Bu küçük öğretim örneği atomik yayın sözü vermez. Çıktı UTF-8 BOM olmadan, LF satır sonlarıyla yazılır; her kayıt son LF ile biter. Bu örnekte kayıtsız rapor sıfır bayttır.

## Algoritma

1. Kataloğu kod ve bedel dizilerine okuyun; kod tekrarını reddedin.
2. Satışları doğrulayın; her kodu katalogda doğrusal arayın, bulunamazsa durun.
3. Ürün başına adet dizisini güncelleyin.
4. Eklemeli sıralamada kod, bedel ve adet dizilerini birlikte kaydırın.
5. Rapor metnini oluşturup yeni dosyaya yazın; kaynaklar kapandıktan sonra başarıyı bildirin.

| Satış | Katalog dizini | Birikmiş adet | Gelir |
| --- | --- | --- | --- |
| 2;3 | 1 | 3 | 12.00 |
| 7;2 | 0 | 2 | 5.00 |
| 2;1 | 1 | 4 | 16.00 |

`Oku` yalnız dosya sınırlarını ve kodlamayı denetler, `out n` ile geçerli satır adedini döndürür. 100 hücrelik dizinin yalnız ilk n hücresi okunabilir; kalan hücreler veri değildir. Hesaplama ana akışta ve göreve özgü static yerel fonksiyonlarda yapılır.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 3) throw new FormatException("İki giriş ve bir çıktı yolu gerekli.");
    string[] k = Oku(args[0], out int kn);
    int[] kod = new int[100], adet = new int[100];
    decimal[] bedel = new decimal[100];

    for (int i = 0; i < kn; i++)
    {
        string[] a = Alanlar(args[0], i + 1, k[i], 2);
        kod[i] = Tam(args[0], i + 1, a[0], 1, 9999);
        bedel[i] = Ondalik(args[0], i + 1, a[1], 0, 10000);
        if (Bul(kod, i, kod[i]) >= 0) Hata(args[0], i + 1, "Yinelenen kod.");
    }

    string[] s = Oku(args[1], out int sn);
    for (int i = 0; i < sn; i++)
    {
        string[] a = Alanlar(args[1], i + 1, s[i], 2);
        int kimlik = Tam(args[1], i + 1, a[0], 1, 9999);
        int miktar = Tam(args[1], i + 1, a[1], 1, 1000);
        int p = Bul(kod, kn, kimlik);
        if (p < 0) Hata(args[1], i + 1, "Katalogda olmayan kod.");
        adet[p] += miktar;
    }

    Sirala(kod, bedel, adet, kn);
    var rapor = new StringBuilder();
    for (int i = 0; i < kn; i++)
        rapor.Append(kod[i].ToString(CultureInfo.InvariantCulture) + ";" +
            adet[i].ToString(CultureInfo.InvariantCulture) + ";" +
            (adet[i] * bedel[i]).ToString("0.00", CultureInfo.InvariantCulture) + "\n");
    Yaz(args[2], rapor.ToString());
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


static void Yaz(string yol, string metin)
{
    using var akis = new FileStream(yol, FileMode.CreateNew,
        FileAccess.Write, FileShare.None);
    using var yazici = new StreamWriter(akis, new UTF8Encoding(false));
    yazici.NewLine = "\n";
    yazici.Write(metin);
}


static void Sirala(int[] kod, decimal[] bedel, int[] adet, int n)
{
    for (int i = 1; i < n; i++)
    {
        int k = kod[i], a = adet[i], j = i - 1;
        decimal b = bedel[i];
        while (j >= 0 && kod[j] > k)
        {
            kod[j + 1] = kod[j]; bedel[j + 1] = bedel[j];
            adet[j + 1] = adet[j]; j--;
        }
        kod[j + 1] = k; bedel[j + 1] = b; adet[j + 1] = a;
    }
}
```

## Örnek çalıştırmalar

`dotnet run -- ornek-089/katalog.txt ornek-089/satis.txt ornek-089/rapor.txt`; katalog.txt:

```text
7;2.50
2;4
9;10
```

satis.txt:
```text
2;3
7;2
2;1
```

Yeni rapor.txt dosyasının tam içeriği:
```text
2;4;16.00
7;2;5.00
9;0;0.00
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

İki giriş dosyası boş, rapor.txt yok:

Yeni rapor.txt sıfır bayttır.

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

katalog.txt yalnız `7;2.50`; satis.txt:

```text
2;1
```

rapor.txt oluşturulmaz.

Konsolun tam çıktısı:

```text
Hata: satis.txt: satır 1: Katalogda olmayan kod.
```

Geçerli girişler, rapor.txt önceden var:

Mevcut raporun baytları korunur.

Konsolun tam çıktısı:

```text
Hata: Dosya işlemi başarısız.
```

## Sınır durumları

- Katalog boşken satış kaydı varsa bilinmeyen kod hatasıdır; satış boşken tüm katalog ürünleri sıfır gelirle yazılır. Boş satır alan sayısı hatasıdır.
- Katalog kodu tekrarı hata; satış kodu tekrarı normal birikimdir.
- Ürün adedi en fazla 100000, ürün geliri ve bütün raporun gelir toplamı en fazla 1000000000; int adet ve decimal gelir taşmaz.
- Kaydırma sırasında üç paralel dizi aynı kaydın alanları olarak birlikte taşınır. Hazır LINQ, GroupBy veya Sort kullanılmaz.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Bir dosyadaki anahtarı diğer dosyadaki referansla ilişkilendirme.
- Paralel dizilerde kayıt bütünlüğünü sıralama boyunca koruma.
- Doğrulama ve yeni rapor oluşturmayı ayrı aşamalarda yürütme.

## Alıştırmalar

1. Ürün başına satış satırı sayısını dördüncü paralel diziye ekleyin.
2. Toplam gelire göre azalan, eşitlikte koda göre artan sıralama yazın.
