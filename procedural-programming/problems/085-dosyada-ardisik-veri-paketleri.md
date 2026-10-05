---
id: "085"
order: 85
title: "Dosyada ardışık veri paketleri"
level: "İleri"
prerequisites: ["073","075","083"]
concepts: ["Çoklu veri kümesi","Başlık ve gövde","Beklenen kayıt sayısı","Eksik EOF"]
---

# Dosyada ardışık veri paketleri

## Problem tanımı

Tek dosyada art arda yer alan ölçüm paketlerini çözümleyin. Her paket kendi kimliği, ölçüm adedi ve ölçümleriyle bağımsızdır. Paket toplamlarını dosyadaki sırayla üretin; önceki paketin toplamı yeni pakete taşınmasın.

## Girdi ve çıktı

Tek yol: `ornek-085/paketler.txt`. Paket başlığı `kimlik;n`, ardından tam n satır ölçüm gelir. Kimlik 1..9999 benzersiz, n 0..20, ölçüm -1000..1000 tamsayıdır. En fazla 20 paket ve ortak 100 fiziksel satır sınırı vardır. Sıfır ölçümlü paket yalnız başlıktan oluşur. Başlık veya ölçümde boş satır hatadır. Boş dosyanın çıktısı `Paket: Yok` olur.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Tamsayılar isteğe bağlı + veya - işareti ve ASCII rakamlardan oluşur; baştaki sıfırlar kabul edilir. Çevre boşluğu, binlik ayırıcı ve ondalık sayı geçersizdir. Taşan sayı ayrıştırma hatasıdır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır.

## Algoritma

1. Okuma konumu p sıfırdan başlasın; EOF sonrasında yeni başlık beklenmez.
2. Başlığı doğrulayın; kodun benzersizliğini ve n ölçüm için yeterli satırı denetleyin.
3. Yeni toplamı sıfırlayıp sonraki n satırı okuyun; ölçüm hatasında gerçek fiziksel konumu bildirin.
4. Paket sonucunu bir çıktı tamponuna ekleyin; p sonraki başlığa ilerlesin.
5. Bütün paketler geçerliyse tamponu yazın.

| Başlık satırı | Başlık | Ölçüm satırları | Sonraki başlık konumu |
| --- | --- | --- | --- |
| 1 | 7;2 | 2, 3 | 4 |
| 4 | 9;0 | yok | 5 |
| 5 | 4;1 | 6 | EOF |

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
    int[] kodlar = new int[20];
    int p = 0, paket = 0;
    var sonuc = new StringBuilder();
    while (p < n)
    {
        int baslik = p + 1;
        if (paket == 20) Hata(args[0], baslik, "En fazla 20 paket.");
        string[] a = Alanlar(args[0], baslik, s[p++], 2);
        int kod = Tam(args[0], baslik, a[0], 1, 9999);
        int adet = Tam(args[0], baslik, a[1], 0, 20);
        if (Bul(kodlar, paket, kod) >= 0)
            Hata(args[0], baslik, "Yinelenen kod.");
        if (n - p < adet)
            Hata(args[0], n + 1, "Paket tamamlanmadan EOF.");
        int toplam = 0;
        for (int j = 0; j < adet; j++)
        {
            toplam += Tam(args[0], p + 1, s[p], -1000, 1000);
            p++;
        }
        kodlar[paket++] = kod;
        sonuc.AppendLine(kod.ToString(CultureInfo.InvariantCulture) +
            ";" + adet.ToString(CultureInfo.InvariantCulture) +
            ";" + toplam.ToString(CultureInfo.InvariantCulture));
    }
    if (paket == 0) Console.WriteLine("Paket: Yok");
    else Console.Write(sonuc.ToString());
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


static int Bul(int[] kodlar, int n, int kod)
{
    for (int i = 0; i < n; i++)
        if (kodlar[i] == kod) return i;
    return -1;
}
```

## Örnek çalıştırmalar

`dotnet run -- ornek-085/paketler.txt`; dosya:

```text
7;2
4
-1
9;0
4;1
10
```

Konsolun tam çıktısı:

```text
7;2;3
9;0;0
4;1;10
```

Sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Paket: Yok
```

Eksik paket:

```text
7;2
4
```

Konsolun tam çıktısı:

```text
Hata: paketler.txt: satır 3: Paket tamamlanmadan EOF.
```

Boş ölçüm satırı (dosya tam iki LF ile biter):

```text
7;1

```

Konsolun tam çıktısı:

```text
Hata: paketler.txt: satır 2: Geçersiz tamsayı.
```

## Sınır durumları

- Başlıkta n=0 EOF anlamına gelmez; sonraki satır yeni bir başlıktır. n=20 geçerlidir.
- Eksik ölçümde hata konumu EOF sonrası beklenen ilk satırdır. Eksik adet önce denetlendiği için yetersiz dosyada içteki olası sayı hatalarından önce EOF hatası gelir.
- Başlık fazla alanlıysa başlık hatasıdır; ölçüm satırında noktalı virgül tamsayı hatasıdır. Paket kodları sayısal olarak benzersiz olmalıdır.
- Paket toplamı mutlak en fazla 20000; toplamlar paketler arasında birleştirilmez.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Başlıkla bildirilen veri uzunluğunu fiziksel satırlarla eşleştirme.
- Paket başında durum değişkenlerini sıfırlama.
- Son pakette eksik EOF ile normal EOF farkını gösterme.

## Alıştırmalar

1. Her paket için boş değilse minimum ve maksimumu da üretin.
2. Paket adedini ilk satırda bildiren alternatif bir biçim tasarlayın.
