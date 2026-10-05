---
id: "096"
order: 96
title: "Vardiyaları ücret dilimlerine bölerek maliyet hesaplama"
level: "İleri"
prerequisites: ["084", "089", "092"]
concepts: ["Aralık parçalama", "Takvim dakikası", "Gün sınırı", "Süre korunumu", "Çoklu veri kaynağı"]
---

# Vardiyaları ücret dilimlerine bölerek maliyet hesaplama

## Problem tanımı

Çalışan, vardiya ve her gün tekrarlanan dakika ücretlerini birleştirin. Vardiyaları ücret değişimlerinde ve gece yarısında parçalayıp çalışan başına toplam süreyi ve maliyeti bulun. Aynı çalışanın çakışan vardiyaları hatadır. Bu problemde verilen dakika ücretleri kullanılır; mola, fazla mesai veya başka ücret kuralı uygulanmaz.

## Girdi ve çıktı

`dotnet run -- ornek-096/calisanlar.txt ornek-096/vardiyalar.txt ornek-096/tarife.txt ornek-096/rapor.txt`

Çalışan satırı `kod` (1..9999, en fazla 30 çalışan); vardiya satırı `vardiyaKod;çalışanKod;başlangıç;bitiş` (kodlar 1..9999, en fazla 100 vardiya); tarife satırı `başlangıçDakika;bitişDakika;dakikaÜcretiKuruş` (0..1439, 1..1440, 1..100000, en fazla 24 dilim). Çalışan ve vardiya kodları kendi dosyalarında benzersizdir. Tarifeler dosyada sırasız olabilir; sıralandıktan sonra 0..1440 gününü boşluk ve çakışma olmadan kapsamalıdır.

Vardiya zamanları tam `yyyy-MM-dd HH:mm`, 2000..2099 yılları arasındadır. Süre 1..1440 dakikadır. Aralık `[başlangıç,bitiş)` biçimindedir; bitiş ile sonraki başlangıcın eşitliği çakışma değildir. Hesap takvim dakikalarıyla yapılır; saat dilimi veya yaz saati dönüşümü uygulanmaz. `24:00` yerine sonraki günün `00:00` değeri yazılır.

Rapor çalışan kodu artan sırada `kod;toplamDakika;maliyetKuruş` biçimindedir. Vardiyası olmayan çalışan sıfırlarla bulunur. Tarife zorunludur; çalışan ve vardiya dosyaları boş ve tarife geçerliyse rapor boştur. Boşluk/kapsama gibi bütün dosyayı ilgilendiren tarife hatasında satır ve alan 0 olabilir.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.


## Algoritma

1. Üç dosyayı ve benzersiz anahtarları doğrulayın; tarife dizinlerini başlangıca göre sıralayın.
2. İlk tarifenin 0'dan başlayıp her bitişin sonraki başlangıç olduğunu ve son bitişin 1440 olduğunu doğrulayın.
3. Çalışan ilişkisini, vardiya süresini ve aynı çalışanın bütün önceki vardiyalarıyla çakışmayı denetleyin.
4. Her vardiyada bulunduğunuz dakikanın günlük tarife dilimini bulun.
5. Vardiya bitişi ile dilim bitişinin erken olanına kadar süre ve maliyet ekleyin; zamanı bu sınıra taşıyın.
6. Çalışan koduna göre rapor üretin ve yeni dosyaya yazın.

Takvim dakikası, yıl 1'in başından bu yana tam dakika sayısıdır; kabul edilen tarihlerde `int` aralığına sığar. `an % 1440` gün içi konumu, `an - gunIci` gün başlangıcını verir. Günün son diliminden sonra yeni günün ilk dilimi bulunur. Her parça pozitiftir; toplam parçalar vardiya süresini tam korur. Maliyet en fazla 100 × 1440 × 100000 = 14400000000 kuruştur ve `long` gerektirir.

21:00–ertesi gün 01:00 için tarife 00:00–08:00 ve 22:00–24:00 arasında 2, diğer saatlerde 1 kuruş/dakika olsun:

| Parça | Dakika | Birim ücret | Maliyet |
| --- | --- | --- | --- |
| 21:00–22:00 | 60 | 1 | 60 |
| 22:00–24:00 | 120 | 2 | 240 |
| 00:00–01:00 | 60 | 2 | 120 |

`Zaman` kesin biçimdeki metni takvim dakikasına dönüştürür. `Oku` zaman sütunlarını -2 işaretiyle tanır; bu işaret dosya verisi değildir. `Sira` yalnız dizinleri taşır, hata satırları özgün dosyayı gösterir. Doğrulama bittikten sonra her parçanın maliyeti biriktirilir.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 4) throw new FormatException("4 argüman gerekli.");
    checked
    {
        int[][] cal = Oku(args[0], new[] { 1 }, new[] { 9999 }, 30);
        int[][] vard = Oku(args[1], new[] { 1, 1, 0, 0 },
            new[] { 9999, 9999, -2, -2 });
        int[][] tar = Oku(args[2], new[] { 0, 1, 1 },
            new[] { 1439, 1440, 100000 }, 24);
        Tekil(cal, args[0], 0); Tekil(vard, args[1], 0);
        int[] ts = Sira(tar);
        int beklenen = 0;
        foreach (int p in ts)
        {
            if (tar[p][0] != beklenen || tar[p][1] <= tar[p][0])
                Hata(args[2], p + 1, 0, "Tarife boşluklu veya çakışıyor.");
            beklenen = tar[p][1];
        }
        if (beklenen != 1440)
            Hata(args[2], 0, 0, "Tarife günü kapsamıyor.");

        int[] ci = new int[vard.Length];
        for (int i = 0; i < vard.Length; i++)
        {
            ci[i] = Bul(cal, vard[i][1]);
            if (ci[i] < 0)
                Hata(args[1], i + 1, 2, "Çalışan bulunamadı.");
            int sure = vard[i][3] - vard[i][2];
            if (sure < 1 || sure > 1440)
                Hata(args[1], i + 1, 0, "Süre 1..1440 olmalıdır.");
            for (int j = 0; j < i; j++)
                if (ci[j] == ci[i] &&
                    Math.Max(vard[i][2], vard[j][2]) <
                    Math.Min(vard[i][3], vard[j][3]))
                    Hata(args[1], i + 1, 3, "Vardiyalar çakışıyor.");
        }

        long[] dakika = new long[cal.Length], maliyet = new long[cal.Length];
        for (int i = 0; i < vard.Length; i++)
        {
            int an = vard[i][2];
            while (an < vard[i][3])
            {
                int gunIci = an % 1440, p = 0;
                while (gunIci >= tar[ts[p]][1]) p++;
                int t = ts[p];
                int son = Math.Min(vard[i][3], an - gunIci + tar[t][1]);
                int parca = son - an;
                dakika[ci[i]] += parca;
                maliyet[ci[i]] += (long)parca * tar[t][2];
                an = son;
            }
        }
        var rapor = new StringBuilder();
        foreach (int p in Sira(cal))
            Satir(rapor, cal[p][0], dakika[p], maliyet[p]);
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
            if (ust[j] == -2)
                veri[n][j] = Zaman(yol, n + 1, j + 1, a[j]);
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

static int Zaman(string yol, int satir, int alan, string metin)
{
    if (metin.Length != 16) Hata(yol, satir, alan, "Geçersiz zaman.");
    for (int i = 0; i < metin.Length; i++)
    {
        char beklenen = i == 4 || i == 7 ? '-' :
            i == 10 ? ' ' : i == 13 ? ':' : '\0';
        if (beklenen != '\0' ? metin[i] != beklenen :
            metin[i] < '0' || metin[i] > '9')
            Hata(yol, satir, alan, "Geçersiz zaman.");
    }
    if (!DateTime.TryParseExact(metin, "yyyy-MM-dd HH:mm",
        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime an) ||
        an.Year < 2000 || an.Year > 2099)
        Hata(yol, satir, alan, "Geçersiz zaman.");
    return (int)(an.Ticks / TimeSpan.TicksPerMinute);
}
```

## Örnek çalıştırmalar

Yukarıdaki komut için giriş dosyalarının tam içerikleri aşağıdadır; rapor başlangıçta yoktur.

calisanlar.txt:

```text
2
1
```

vardiyalar.txt:

```text
7;1;2026-10-05 21:00;2026-10-06 01:00
```

tarife.txt:

```text
1320;1440;2
0;480;2
480;1320;1
```

Yeni rapor.txt:

```text
1;240;420
2;0;0
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Sınır örneğinde çalışan dosyası `1`, vardiya dosyası `1;1;2099-12-30 23:59;2099-12-31 23:59`, tarife dosyası `0;1440;100000` satırıdır. Yeni rapor `1;1440;144000000` satırı, konsol yine `Rapor yazıldı.` olur. Her satır LF ile biter.

Normal örnekte vardiya dosyasına `8;1;2026-10-05 23:00;2026-10-06 02:00` ikinci satırını ekleyin. Rapor oluşmaz; tam konsol çıktısı:

```text
Hata: vardiyalar.txt: satır 2, alan 3: Vardiyalar çakışıyor.
```

## Sınır durumları

- Bir dakikalık veya tam 1440 dakikalık vardiya kabul edilir; sıfır, negatif ve daha uzun süre reddedilir.
- Farklı çalışanlar aynı anda çalışabilir; aynı çalışanın bitişte başlayan vardiyası kabul edilir.
- Tarifede boşluk, örtüşme ve eksik son bölüm hata olur. Tek `0;1440;ücret` satırı geçerlidir.
- Ay, yıl ve artık gün geçişleri kesin takvim ayrıştırmasıyla doğrulanır; saat veya dakika taşırılmaz.
- Satırların farklı sırada olması sonucu değiştirmez. Çakışma denetimi ilk hatalı fiziksel vardiya satırını bildirir.

## Kazanımlar

- Aralığı doğru sınırda parçalarken süreyi ve maliyeti koruma.
- Günlük konumu genel takvim konumundan ayırma; sayısal türü toplam sınırına göre seçme.
- Çalışan kimliği, vardiya aralığı ve ücret diliminin veri/davranış ilişkisini belirleme; sonraki derste bu doğrulamaların hangi nesnelerde toplanabileceğini tartışma.

## Alıştırmalar

1. Tarife dilimi başına dakika toplamlarını da raporlayın; vardiya süreleriyle toplamlarının eşitliğini denetleyin.
2. Çakışma hatasında önceki vardiyanın kodunu da bildirin.
3. Bir Vardiya nesnesinde süre, çakışma ve parçalama davranışlarının nasıl birlikte korunabileceğini açıklayın.
