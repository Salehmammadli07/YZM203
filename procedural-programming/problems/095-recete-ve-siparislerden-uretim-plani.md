---
id: "095"
order: 95
title: "Reçete ve siparişlerden üretim planı"
level: "İleri"
prerequisites: ["053","079","083","089"]
concepts: ["Paylaşılan kaynak","Öncelikli tahsis","İhtiyaç matrisi","Tam sipariş kabulü"]
---

# Reçete ve siparişlerden üretim planı

## Problem tanımı

Malzeme stoklarını, ürün reçetelerini ve siparişleri birleştirerek öncelikli bir üretim planı çıkarın. Her sipariş ya bütünüyle karşılanır ya da hiç malzeme tüketmez. Kısmi stok azaltımı yapmayın. Bu politika dosya sırasından bağımsız bir açgözlü tahsistir; toplam üretimi en büyük yapan planı bulduğu iddia edilmez.

## Girdi ve çıktı

`dotnet run -- ornek-095/malzemeler.txt ornek-095/receteler.txt ornek-095/siparisler.txt ornek-095/rapor.txt`

`malzemeler.txt`: `malzemeKod;stok` (kod 1..9999, stok 0..1000000), kod tekildir. `receteler.txt`: `ürünKod;malzemeKod;birimİhtiyaç` (kodlar 1..9999, ihtiyaç 1..1000), ürün/malzeme çifti tekildir. Ürünler reçete dosyasında tanımlanır; her reçete malzemesi katalogda bulunmalıdır. `siparisler.txt`: `siparişKod;ürünKod;adet;öncelik` (kodlar 1..9999, adet 1..1000, öncelik 1..9), sipariş kodu tekildir, ürünün en az bir reçete satırı bulunmalıdır.

İşlem sırası öncelik azalan, eşitlikte sipariş kodu artandır. Her malzemenin ihtiyaç × adet miktarı eldeki stokla karşılaştırılır. Bir malzeme yetmezse bütün sipariş BEKLER, aksi halde tüm gereksinimler düşülür ve URET olur. Rapor işlem sırasıyla `S;siparişKod;durum`, ardından malzeme kodu artan `M;malzemeKod;kalanStok` satırlarıdır. Yinelenen ürün farklı siparişlerde kabul edilir. Sipariş yokken yalnız stok bölümü yazılır.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.

## Algoritma

1. Tabloları, anahtarları ve reçete malzeme ilişkilerini doğrulayın.
2. Siparişlerde ürün için reçete varlığını doğrulayın.
3. Sipariş dizinlerini öncelik azalan ve kod artan eklemeli sıralayın.
4. Her siparişte önce tüm gereksinimleri kontrol edin; ancak yeterliyse ikinci geçişte stok düşürün.
5. Karar satırlarını ve son malzeme stoklarını bellek raporuna ekleyip yeni dosyaya yazın.

| Sipariş | Öncelik | İhtiyaç | Önceki stok | Karar / sonraki stok |
| --- | --- | --- | --- | --- |
| 2 | 9 | 2 × 2 = 4 | 5 | URET / 1 |
| 1 | 1 | 1 × 2 = 2 | 1 | BEKLER / 1 |

`Oku` yalnız geçerli satırları içeren diziyi döndürür. `Tekil` seçilen sütunları birlikte anahtar sayar; `Bul` ilk sütunda doğrusal arar, yoksa -1 verir. `Sira` kaynak kayıtları değiştirmeden kod artan eklemeli sıralamayla dizin dizisi döndürür. `Satir` yalnız bellek raporunu değiştirir; `Yaz` dış dosyayı oluşturur. Hazır LINQ, GroupBy, Sort veya arama algoritması kullanılmaz.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 4) throw new FormatException(
        "4 argüman gerekli.");
    checked
    {
        int[][] mal = Oku(args[0], new[] { 1, 0 },
            new[] { 9999, 1000000 });
        int[][] rec = Oku(args[1], new[] { 1, 1, 1 },
            new[] { 9999, 9999, 1000 });
        int[][] sip = Oku(args[2], new[] { 1, 1, 1, 1 },
            new[] { 9999, 9999, 1000, 9 });
        Tekil(mal, args[0], 0); Tekil(rec, args[1], 0, 1);
        Tekil(sip, args[2], 0);
        int[] mi = new int[rec.Length], kalan = new int[mal.Length];
        for (int i = 0; i < mal.Length; i++) kalan[i] = mal[i][1];
        for (int i = 0; i < rec.Length; i++)
        {
            mi[i] = Bul(mal, rec[i][1]);
            if (mi[i] < 0) Hata(args[1], i + 1, 2, "Malzeme bulunamadı.");
        }
        for (int i = 0; i < sip.Length; i++)
            if (Bul(rec, sip[i][1]) < 0)
                Hata(args[2], i + 1, 2, "Reçete bulunamadı.");

        int[] sira = Sira(sip);
        for (int i = 1; i < sira.Length; i++)
        {
            int p = sira[i], j = i - 1;
            while (j >= 0 && sip[sira[j]][3] < sip[p][3])
            { sira[j + 1] = sira[j]; j--; }
            sira[j + 1] = p;
        }
        var rapor = new StringBuilder();
        foreach (int p in sira)
        {
            bool yeter = true;
            for (int r = 0; r < rec.Length; r++)
                if (rec[r][0] == sip[p][1] &&
                    kalan[mi[r]] < rec[r][2] * sip[p][2]) yeter = false;
            if (yeter)
                for (int r = 0; r < rec.Length; r++)
                    if (rec[r][0] == sip[p][1])
                        kalan[mi[r]] -= rec[r][2] * sip[p][2];
            Satir(rapor, "S", sip[p][0], yeter ? "URET" : "BEKLER");
        }
        foreach (int p in Sira(mal)) Satir(rapor, "M", mal[p][0], kalan[p]);

        Yaz(args[3], rapor.ToString());
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

malzemeler.txt dosyasının tam içeriği:

```text
7;5
```

receteler.txt dosyasının tam içeriği:

```text
9;7;2
```

siparisler.txt dosyasının tam içeriği:

```text
1;9;1;1
2;9;2;9
```

Yeni rapor.txt dosyasının tam içeriği:

```text
S;2;URET
S;1;BEKLER
M;7;1
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Sınır örneği: aynı dosya adlarını ayrı boş bir klasörde hazırlayın; aynı komutu çalıştırın.

malzemeler.txt dosyasının tam içeriği:

```text
9999;1000000
```

receteler.txt dosyasının tam içeriği:

```text
9999;9999;1000
```

siparisler.txt dosyasının tam içeriği:

```text
9999;9999;1000;9
```

Yeni rapor.txt dosyasının tam içeriği:

```text
S;9999;URET
M;9999;0
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Geçersiz örnek: ayrı bir klasörde, aynı komut.

malzemeler.txt dosyasının tam içeriği:

```text
7;5
```

receteler.txt dosyasının tam içeriği:

```text
9;7;2
```

siparisler.txt dosyasının tam içeriği:

```text
1;8;1;1
```

Rapor oluşturulmaz. Konsolun tam çıktısı:

```text
Hata: siparisler.txt: satır 1, alan 2: Reçete bulunamadı.
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

- Her dosya 100 satırdır. Tek malzemenin tek sipariş gereksinimi en fazla 1000000; stok ve çarpım int aralığındadır. Bileşik anahtar tekil olduğu için bir malzemenin aynı ürün gereksinimi gizlice iki satırda toplanmaz. Malzeme yokken reçete, reçete yokken sipariş hata olur. Tümü boşsa rapor boştur. Üretilemeyen sipariş normal sonuçtur.
- Satır kapasitesinin tam dolması kabul edilir; bir sonraki satır saklanmadan hata olur. Tek bir boş satır sıfır kayıt sayılmaz; alan/sayı hatasıdır. Çok büyük veya taşan sayısal alan ayrıştırma hatasıdır.
- Aynı sayıyı farklı baştaki sıfırlarla yazmak anahtar tekrarını gizlemez. Sıralama sırasında dizinler taşındığı için dosya satırı ile hata konumu ve ilişkiler değişmez.
- Hatalı son kayıt dahil bütün giriş doğrulanmadan rapor açılmaz. Ortak I/O ve çıktı güvenliği sözleşmesi yukarıda tanımlıdır.

## Kazanımlar

- Kontrol et ve uygula geçişlerini ayırma; sınırlı kaynakta sıranın etkisini açıklama; açgözlü politikanın en iyi plan garantisi vermediğini gösterme.
- Bir siparişin miktarı, önceliği ve karşılanabilirlik davranışı; reçetenin ürün/malzeme ilişkileri tekrar birlikte kullanılır. Sonraki derste bu birliktelikleri nesnelerde toplamak, karar ve stok güncellemesinin farklı yerlerde ayrışmasını önleyebilir.

## Alıştırmalar

1. BEKLER siparişinde ilk yetersiz malzeme kodunu, malzeme kodu artan sırada seçerek ekleyin.
2. Daha fazla toplam ürün veren başka bir sıra bulun; Sipariş nesnesinin hangi doğrulama ve hesap davranışını bir arada tutacağını tartışın.

