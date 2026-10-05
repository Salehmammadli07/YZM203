---
id: "088"
order: 88
title: "İki sayım dosyasını uzlaştırma"
level: "İleri"
prerequisites: ["046","055","075","083"]
concepts: ["İki dosyada anahtar eşleme","Sıralılık sözleşmesi","Dış birleştirme","Dosya kaynağına göre hata"]
---

# İki sayım dosyasını uzlaştırma

## Problem tanımı

İki bağımsız depo sayımını ürün koduna göre uzlaştırın. Her iki dosyada bulunan üründe sağ adet eksi sol adet farkını, yalnız bir dosyada bulunan üründe SOL veya SAG durumunu üretin. Aynı adede sahip ürünleri de ORTAK 0 olarak gösterin; tek dosyada görünmeyen ürünün adedini sıfır varsaymayın.

## Girdi ve çıktı

İki yol: `ornek-088/sol.txt ornek-088/sag.txt`. Her satır `kod;adet`: kod 1..9999, adet 0..1000. Her dosyada kodlar kesin artmalı ve benzersiz olmalıdır. İki dosyada aynı kod eşleştirilir. Çıktı artan kod sırasıyla `kod;ORTAK;fark`, `kod;SOL;adet` veya `kod;SAG;adet` olur. İki dosya boşsa `Kayıt: Yok` yazılır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

İsteğe bağlı işaret ve ASCII rakamlar kabul edilir. Boşluk, binlik ayırıcı ve ondalık sayı geçersizdir. Biçim kültürden bağımsızdır.

## Algoritma

1. Önce sol sonra sağ dosyayı tamamen doğrulayıp paralel dizilere okuyun.
2. İki okuma konumu sıfırdan başlasın.
3. Kodlar eşitse farkı üretip iki konumu ilerletin.
4. Küçük kodun bulunduğu tarafta tek taraflı satır üretip yalnız o konumu ilerletin.
5. Biten tarafın ardından diğer tarafın kalan kayıtlarını tamamlayın.

| Sol kod | Sağ kod | Üretilen kayıt | İlerleyen konum |
| --- | --- | --- | --- |
| 1 | 2 | 1;SOL;3 | sol |
| 3 | 2 | 2;SAG;4 | sağ |
| 3 | 3 | 3;ORTAK;-1 | ikisi |

`Oku` yalnız dosya sınırlarını ve kodlamayı denetler, `out n` ile geçerli satır adedini döndürür. 100 hücrelik dizinin yalnız ilk n hücresi okunabilir; kalan hücreler veri değildir. Hesaplama ana akışta ve göreve özgü static yerel fonksiyonlarda yapılır.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 2) throw new FormatException("İki giriş yolu gerekli.");
    Ciftler(args[0], out int[] sol, out int[] sa, out int sn);
    Ciftler(args[1], out int[] sag, out int[] ga, out int gn);
    var sonuc = new StringBuilder();
    int i = 0, j = 0;
    while (i < sn || j < gn)
    {
        if (i < sn && j < gn && sol[i] == sag[j])
        {
            Satir(sonuc, sol[i], "ORTAK", ga[j] - sa[i]);
            i++; j++;
        }
        else if (j == gn || (i < sn && sol[i] < sag[j]))
        {
            Satir(sonuc, sol[i], "SOL", sa[i]); i++;
        }
        else { Satir(sonuc, sag[j], "SAG", ga[j]); j++; }
    }
    if (sn + gn == 0) Console.WriteLine("Kayıt: Yok");
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


static void Ciftler(string yol, out int[] kod, out int[] adet,
    out int n)
{
    string[] s = Oku(yol, out n);
    kod = new int[100]; adet = new int[100];
    for (int i = 0; i < n; i++)
    {
        string[] a = Alanlar(yol, i + 1, s[i], 2);
        kod[i] = Tam(yol, i + 1, a[0], 1, 9999);
        adet[i] = Tam(yol, i + 1, a[1], 0, 1000);
        if (i > 0 && kod[i] <= kod[i - 1])
            Hata(yol, i + 1, "Kodlar kesin artmalı.");
    }
}


static void Satir(StringBuilder s, int kod, string durum, int deger)
{
    s.AppendLine(kod.ToString(CultureInfo.InvariantCulture) +
        ";" + durum + ";" + deger.ToString(CultureInfo.InvariantCulture));
}
```

## Örnek çalıştırmalar

`dotnet run -- ornek-088/sol.txt ornek-088/sag.txt`; sol.txt:

```text
1;3
3;5
```

sag.txt:
```text
2;4
3;4
```

Konsolun tam çıktısı:

```text
1;SOL;3
2;SAG;4
3;ORTAK;-1
```

Her iki dosya sıfır bayt:

Konsolun tam çıktısı:

```text
Kayıt: Yok
```

sol.txt boş; sag.txt:

```text
9;0
```

Konsolun tam çıktısı:

```text
9;SAG;0
```

sol.txt boş; sag.txt tekrar içeriyor:

```text
3;1
3;2
```

Konsolun tam çıktısı:

```text
Hata: sag.txt: satır 2: Kodlar kesin artmalı.
```

## Sınır durumları

- Dosya içindeki tekrar hata, dosyalar arasındaki tekrar eşleşmedir. Sayısal eşitlik 01 ve 1 biçimlerini de kapsar.
- Sıralılık doğrulanmadan birleştirme başlamaz; sağ dosyanın hatası sol dosyanın geçerli sonuçlarının yazılmasını önler.
- Tek boş dosya diğerinin tüm kayıtlarını tek taraflı üretir; boş satır geçersizdir.
- Çıktı en fazla 200 kayıt; fark -1000..1000 aralığındadır. Okuma ve birleştirme doğrusal, saklama kapasitesi dosya başına 100 kayıttır.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Tam dış eşleştirme ile yalnız kesişim arasındaki farkı açıklama.
- Kısa devre koşullarıyla bitmiş diziyi okumadan yönetme.
- Hatanın hangi dosyadan geldiğini koruma.

## Alıştırmalar

1. Yalnız farkı sıfır olmayan ortak ürünleri yazan bir seçenek ekleyin.
2. Aynı kaynağın iki argümanda verilmesi halinde beklenen çıktıyı açıklayın.
