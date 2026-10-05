---
id: "100"
order: 100
title: "Uygunluk ve maliyetle en iyi görev ataması"
level: "İleri"
prerequisites: ["074", "079", "095", "097", "098"]
concepts: ["Bire bir atama", "Uygunluk matrisi", "Geri izleme", "İyimser alt sınır", "Küresel optimum"]
---

# Uygunluk ve maliyetle en iyi görev ataması

## Problem tanımı

Eşit sayıda çalışanı ve görevi, her çalışan tam bir görev ve her görev tam bir çalışan alacak şekilde eşleştirin. Yalnız izin verilen çiftleri kullanarak toplam maliyeti en küçük yapan tam atamayı bulun. Eşit maliyette, çalışan kodu sırasındaki görev kodları listesinin sayısal sözlük sırasına göre ilkini seçin. Her çalışan için yerel olarak en ucuz görevi seçmek küresel en iyi atamayı garanti etmez.

## Girdi ve çıktı

`dotnet run -- ornek-100/calisanlar.txt ornek-100/gorevler.txt ornek-100/uygunluk.txt ornek-100/rapor.txt`

Çalışan ve görev dosyaları ayrı `kod` kataloglarıdır: 1..9999 benzersiz kodlar, eşit ve 1..8 kayıt. Uygunluk satırı `çalışanKod;görevKod;maliyet` biçimindedir: mevcut kodlar, maliyet 0..1000000, çift benzersiz, en fazla 64 satır. Listelenmeyen çift yasaktır; bu bozuk veri değildir. Sıfır maliyet geçerlidir.

Rapor önce `T;toplamMaliyet`, ardından çalışan kodu artan `A;çalışanKod;görevKod;çiftMaliyeti` satırlarıdır. Geçerli veriden tam eşleştirme kurulamıyorsa tek `ATAMA_YOK` satırı yazılır. Katalog boyutu hatası veya eksik kod, bu normal sonuçtan ayrıdır. Sözlük sırası ilk farklı görev kodunu sayısal olarak karşılaştırır; bütün aday listeleri aynı boydadır.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.


## Algoritma

1. Üç dosyayı, katalog boyutlarını, benzersiz anahtarları ve ilişkileri doğrulayın. Yasak çiftleri -1 maliyetle saklayın.
2. Çalışan ve görev dizinlerini kod artan sıralayın.
3. Sıradaki çalışan için kullanılmamış izinli görevleri kod sırasıyla deneyin.
4. Her çağrıda kalan her çalışanın kullanılmamış izinli görevlerdeki en düşük maliyetini mevcut toplama ekleyerek alt sınır kurun.
5. Kalan bir çalışana aday yoksa veya alt sınır mevcut en iyiye eşit/büyükse dalı bırakın.
6. Tam atamada daha düşük toplamı ve seçimi saklayın; geri dönerken görevi serbest bırakın.
7. En iyi tam atamayı veya yok sonucunu raporlayın.

Geri izleme, bir seçimi denedikten sonra ortak durumu eski haline getirerek diğer seçeneğe geçer. `kullanildi[g]`, aynı görevin iki çalışana verilmesini engeller; dönüşte `false` yapmak sonraki dallar için gereklidir. Çağrı i+1'e ilerler ve derinlik en fazla n+1'dir.

Alt sınır gerçek bir atama olmayabilir: iki çalışan için aynı ucuz görevi ayrı ayrı sayabilir. Bu iyimserlik sınırı gerçek tamamlamadan büyük yapmaz; dolayısıyla daha iyi olamayacak dallar güvenle budanır. Kod artan derinlik öncelikli tarama tam atamaları sözlük sırasında ziyaret eder. Eşit en iyi toplam güncellenmez; eşit alt sınırdaki sonraki dallar da ilk bulunan eşit optimumu iyileştiremez.

Budamasız en fazla n! tam atama vardır; 8! = 40320. Bir alt sınır hesabı O(n²)'dir. Boyut üst sınırı bu öğretim örneğini yönetilebilir tutar; büyük atama problemlerine genel verimlilik iddiası kurulmaz. Toplam en fazla 8000000'dur; hesap ve en iyi işareti `long` tutulur.

İki çalışan ve iki görev için:

| Çalışan | Görev 10 maliyeti | Görev 20 maliyeti |
| --- | --- | --- |
| 1 | 1 | 2 |
| 2 | 1 | 100 |

1→10 ardından 2→20 toplam 101 verir. 1→20, 2→10 toplam 3'tür. İlk çağrının alt sınırı 1+1=2 aynı görevi iki kez düşünür; gerçek çözüm diye raporlanmaz. `Ata` sadece bellek durumlarını değiştirir; dış dosya en iyi sonuç bulununca oluşturulur.

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
        int[][] cal = Oku(args[0], new[] { 1 }, new[] { 9999 }, 8);
        int[][] gor = Oku(args[1], new[] { 1 }, new[] { 9999 }, 8);
        int[][] uy = Oku(args[2], new[] { 1, 1, 0 },
            new[] { 9999, 9999, 1000000 }, 64);
        Tekil(cal, args[0], 0); Tekil(gor, args[1], 0);
        Tekil(uy, args[2], 0, 1);
        int n = cal.Length;
        if (n == 0 || gor.Length != n)
            throw new FormatException("Eşit ve 1..8 katalog boyutu gerekli.");
        int[,] maliyet = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++) maliyet[i, j] = -1;
        for (int i = 0; i < uy.Length; i++)
        {
            int c = Bul(cal, uy[i][0]), g = Bul(gor, uy[i][1]);
            if (c < 0 || g < 0)
                Hata(args[2], i + 1, 0, "Çalışan veya görev bulunamadı.");
            maliyet[c, g] = uy[i][2];
        }

        int[] cs = Sira(cal), gs = Sira(gor);
        int[] an = new int[n], en = new int[n];
        long enIyi = long.MaxValue;
        Ata(maliyet, cs, gs, new bool[n], an, en, 0, 0, ref enIyi);
        var rapor = new StringBuilder();
        if (enIyi == long.MaxValue) rapor.Append("ATAMA_YOK\n");
        else
        {
            Satir(rapor, "T", enIyi);
            for (int i = 0; i < n; i++)
                Satir(rapor, "A", cal[cs[i]][0], gor[en[i]][0],
                    maliyet[cs[i], en[i]]);
        }
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

static void Ata(int[,] maliyet, int[] cs, int[] gs, bool[] kullanildi,
    int[] an, int[] en, int i, long toplam, ref long enIyi)
{
    int n = cs.Length;
    if (i == n)
    {
        if (toplam < enIyi)
        {
            enIyi = toplam;
            for (int k = 0; k < n; k++) en[k] = an[k];
        }
        return;
    }

    long altSinir = toplam;
    for (int k = i; k < n; k++)
    {
        int ucuz = int.MaxValue;
        foreach (int g in gs)
            if (!kullanildi[g] && maliyet[cs[k], g] >= 0 &&
                maliyet[cs[k], g] < ucuz) ucuz = maliyet[cs[k], g];
        if (ucuz == int.MaxValue) return;
        altSinir += ucuz;
    }
    if (altSinir >= enIyi) return;

    foreach (int g in gs)
    {
        if (kullanildi[g] || maliyet[cs[i], g] < 0) continue;
        kullanildi[g] = true; an[i] = g;
        Ata(maliyet, cs, gs, kullanildi, an, en, i + 1,
            toplam + maliyet[cs[i], g], ref enIyi);
        kullanildi[g] = false;
    }
}
```

## Örnek çalıştırmalar

Yukarıdaki komutta calisanlar.txt tam içeriği:

```text
2
1
```

gorevler.txt tam içeriği:

```text
20
10
```

uygunluk.txt tam içeriği:

```text
1;10;1
1;20;2
2;10;1
2;20;100
```

Yeni rapor.txt:

```text
T;3
A;1;20;2
A;2;10;1
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Aynı kataloglarla dört maliyeti de 0 yapınca rapor `T;0`, `A;1;10;0`, `A;2;20;0` satırlarıdır. Yalnız `1;10;1` ve `2;10;1` uygunluk satırları varsa rapor tek `ATAMA_YOK` satırıdır. Her iki durumda konsol yine `Rapor yazıldı.` olur.

Normal örneğe `3;10;1` uygunluk satırı eklenirse rapor açılmaz; tam konsol çıktısı:

```text
Hata: uygunluk.txt: satır 5, alan 0: Çalışan veya görev bulunamadı.
```

## Sınır durumları

- Her çalışana en az bir aday bulunması tam eşleştirme varlığını garanti etmez; adaylar aynı görev olabilir.
- Sıfır maliyetli çift izinlidir; -1 yalnız bellek içindeki yasak işaretidir ve dosyada kabul edilmez.
- Uygunluk dosyası boşken geçerli kataloglarla `ATAMA_YOK` yazılır; boş veya farklı boylu kataloglar hata olur.
- Eşit optimum, dosya sırasından bağımsız çalışan ve görev kodu sırasıyla seçilir.
- Bir görev dönüşte serbest bırakılmazsa bazı geçerli atamalar kaybolur; bu durum özel bir testle yakalanmalıdır.

## Kazanımlar

- Yerel seçim ile küresel optimumu ayırma; kısıtı her dalda koruma.
- İyimser alt sınırı gerçek sonuçtan ayırıp budamanın güvenliğini açıklama.
- Çalışan/görev kimlikleri, uygunluk, maliyet ve atama davranışlarının nesneye yönelik modelde nasıl ilişkilenebileceğini belirleme.

## Alıştırmalar

1. Budamayı kaldırıp aynı en iyi sonucu doğrulayın; çağrı sayısı farkını ölçün.
2. Eşit optimumların tamamını saymak istendiğinde `>=` budaması ve eşitlik güncellemesinin nasıl değişeceğini açıklayın.
3. Bu problemdeki Çalışan, Görev ve Atama adaylarının verilerini ve doğrulama davranışlarını ayrı ayrı listeleyin; prosedürel çözümde tekrar eden ilişkileri gösterin.
