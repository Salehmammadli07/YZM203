---
id: "086"
order: 86
title: "Tırnaklı CSV kayıtlarını çözümleme"
level: "İleri"
prerequisites: ["066","068","080","083"]
concepts: ["Durumla ayrıştırma","Kaçırılmış tırnak","Sütun konumu","CSV alt kümesi"]
---

# Tırnaklı CSV kayıtlarını çözümleme

## Problem tanımı

Bir ürün açıklama dosyasında virgül içeren ve çift tırnakla kaçırılmış açıklamaları çözümleyin. Her kaydın kodunu ve açılmış açıklamasını yazın. Basit Split virgülü alan ayırıcıyla içerikteki virgülden ayıramaz; bu ayrımı karakter karakter kurun.

## Girdi ve çıktı

Tek yol: `ornek-086/aciklamalar.csv`. Desteklenen CSV alt kümesi tam `kod,"açıklama"` biçimindedir. Kod 1..9999 benzersiz tamsayı; açıklama 1..60 yazdırılabilir ASCII karakteridir (32..126). Açıklama içindeki çift tırnak dosyada iki çift tırnakla yazılır. Çok satırlı alan, başlık, üçüncü alan, alan çevresinde fazladan boşluk ve boş satır desteklenmez. Çıktı `kod: açılmış açıklama`; boş dosyada `Kayıt: Yok` olur. UTF-8 dosya kullanılmasına rağmen açıklama dilbilgisi bu problem için ASCII ile sınırlıdır.

Yollar komut satırı argümanlarıdır; program istem yazmaz. Kod bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Örnekleri yalnız kendinizin oluşturduğu ayrı bir örnek klasöründe hazırlayın; program giriş dosyalarını değiştirmez. Proje klasöründen `dotnet run --` sonrasında belirtilen yolları verin. Göreli yollar programın çalışma klasörüne göredir.

Her giriş dosyası isteğe bağlı UTF-8 BOM ile UTF-8 metindir. LF ve CRLF kabul edilir. Dosya başına en fazla 32768 bayt, 100 fiziksel satır ve satır başına 120 UTF-16 kod birimi kabul edilir. Sınırlar dosya açıldıktan sonra ve her satır saklanmadan önce denetlenir. Bu küçük sınırlar öğretim amaçlıdır; büyük dosya akışı ayrı bir tasarım gerektirir. ReadLine satır sonunu çıkarır: null EOF, boş metin ise gerçek boş satırdır. Sondaki tek satır sonu ek bir boş kayıt oluşturmaz; art arda iki satır sonu aradaki boş satırı oluşturur.

İlk hata işlemi durdurur; doğrulama bitmeden sonuç yazılmaz. Biçim hatasında dosyanın adı ve 1 tabanlı fiziksel satır numarası bildirilir. Eksik dosya, klasörün bulunamaması veya diğer I/O sorunlarında `Hata: Dosya işlemi başarısız.`; yetki sorununun ayrı yakalanması halinde `Hata: Dosyaya erişim izni yok.` yazılır. Geçersiz yol veya bozuk UTF-8 için `Hata: Yol veya UTF-8 kodlaması geçersiz.` yazılır. `using` blokları hata halinde de akışı ve okuyucuyu kapatır.

Sayısal alanlarda çevre boşluğu, binlik ayırıcı, üs gösterimi ve virgüllü ondalık kabul edilmez. Tamsayıda isteğe bağlı + veya - işareti ve ASCII rakamlar kullanılır; baştaki sıfırlar kabul edilir. Ondalıkta isteğe bağlı işaret, en az bir tam kısım rakamı ve varsa noktadan sonra 1..2 rakam vardır. Girdi ve çıktı CultureInfo.InvariantCulture kullanır. Taşan sayı ayrıştırma hatasıdır.

## Algoritma

1. İlk virgülü karakter döngüsüyle bulun; ondan önceki kodu doğrulayın.
2. Virgülden hemen sonra açılış tırnağı bekleyin.
3. Normal karakteri tampona ekleyin; çift tırnak çifti tek tırnak üretir.
4. Eşlenmemiş tırnak kapanıştır; ardından satır sonu gelmelidir.
5. Kapanış, açıklama uzunluğu ve kod benzersizliği doğrulanınca kayıt sonucunu tampona alın.
6. Bütün dosya kabul edildiğinde sonuçları yazın.

| Okunan bölüm | Eylem | Açılmış tampon |
| --- | --- | --- |
| `"` | açılış | boş |
| `A,` | içerik | A, |
| `""` | kaçırılmış tırnak | A," |
| `B` | içerik | A,"B |
| `""` | kaçırılmış tırnak | A,"B" |
| `"` ve satır sonu | kapanış | A,"B" |

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

    var sonuc = new StringBuilder();
    for (int i = 0; i < n; i++)
    {
        Coz(args[0], i + 1, s[i], out int kod, out string aciklama);
        if (Bul(kodlar, i, kod) >= 0)
            Hata(args[0], i + 1, "Yinelenen kod.");
        kodlar[i] = kod;
        sonuc.AppendLine(kod.ToString(CultureInfo.InvariantCulture) +
            ": " + aciklama);
    }

    if (n == 0) Console.WriteLine("Kayıt: Yok");
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


static void Coz(string yol, int satir, string s,
    out int kod, out string aciklama)
{
    int p = 0;
    while (p < s.Length && s[p] != ',') p++;
    if (p == s.Length) CsvHata(yol, satir, p + 1);
    kod = Tam(yol, satir, s.Substring(0, p), 1, 9999);
    p++;
    if (p == s.Length || s[p] != '"') CsvHata(yol, satir, p + 1);
    p++;
    var metin = new StringBuilder();
    bool kapandi = false;

    while (p < s.Length)
    {
        char c = s[p++];
        if (c == '"')
        {
            if (p < s.Length && s[p] == '"')
            {
                metin.Append('"');
                p++;
            }
            else { kapandi = true; break; }
        }
        else
        {
            if (c < 32 || c > 126) CsvHata(yol, satir, p);
            metin.Append(c);
        }
        if (metin.Length > 60) CsvHata(yol, satir, p);
    }

    if (!kapandi || p != s.Length || metin.Length == 0)
        CsvHata(yol, satir, p + 1);
    aciklama = metin.ToString();
}

static void CsvHata(string yol, int satir, int sutun)
{
    Hata(yol, satir, "CSV sütun " +
        sutun.ToString(CultureInfo.InvariantCulture) + ": Geçersiz kayıt.");
}
```

## Örnek çalıştırmalar

`dotnet run -- ornek-086/aciklamalar.csv`; dosya:

```text
7,"Vida, buyuk"
2,"A,""B"""
```

Konsolun tam çıktısı:

```text
7: Vida, buyuk
2: A,"B"
```

Sıfır baytlık dosya:

Konsolun tam çıktısı:

```text
Kayıt: Yok
```

Açılmayan tırnak:

```text
1,abc
```

Konsolun tam çıktısı:

```text
Hata: aciklamalar.csv: satır 1: CSV sütun 3: Geçersiz kayıt.
```

Kapanış sonrası üçüncü alan:

```text
1,"a",b
```

Konsolun tam çıktısı:

```text
Hata: aciklamalar.csv: satır 1: CSV sütun 6: Geçersiz kayıt.
```

## Sınır durumları

- Hata sütunu 1 tabanlı UTF-16 konumudur. Beklenen karakter EOF ise uzunluk + 1 verilir; kod sayısı ve tekrar hataları yalnız satır bildirir.
- Boş açıklama geçersizdir; açıklama içindeki boşluk veya virgül geçerlidir. Açılmış uzunluk 60, fiziksel satır 120 sınırlarını ayrı düşünün: tümü tırnak olan 60 karakterin kaçırılmış biçimi fiziksel sınırı aşabilir.
- İki art arda tırnak içerikte tek tırnaktır; kapanış için ek bir tırnak gerekir.
- Anahtarlar sayısal olarak benzersizdir. Sayaçlar ve tampon bu sınırlarla taşmaz.
- Ortak dosya boyutu, fiziksel satır, kodlama, eksik dosya ve erişim davranışları Girdi ve çıktı bölümündeki sözleşmeye tabidir.

## Kazanımlar

- Alan ayırıcı ile içerikteki ayırıcıyı durum üzerinden ayırt etme.
- İlk sözdizimi hatasını sütun konumuyla raporlama.
- CSV standardının açıkça tanımlanmış bir alt kümesini uygulama.

## Alıştırmalar

1. Açıklama dışında isteğe bağlı boşluk için ayrı kurallar ekleyin.
2. Bir satır sonunu tırnaklı alan içinde desteklemek için fiziksel ve mantıksal kayıt ayrımını açıklayın.
