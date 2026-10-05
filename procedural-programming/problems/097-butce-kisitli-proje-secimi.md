---
id: "097"
order: 97
title: "Bütçe kısıtlı proje seçimi"
level: "İleri"
prerequisites: ["053", "075", "079", "095"]
concepts: ["0/1 seçim", "Dinamik programlama", "Tam maliyet durumu", "Çözümü geri kurma", "Sözlük sırası"]
---

# Bütçe kısıtlı proje seçimi

## Problem tanımı

Her projeyi en fazla bir kez seçerek verilen bütçeyle toplam kazanımı en büyük yapan kümeyi bulun. Eşit kazanımda en düşük harcamayı; yine eşitlikte artan proje kodları listesinin sözlük sırasına göre ilkini seçin. Kazanım/maliyet oranına göre seçim her zaman doğru değildir; tüm geçerli seçimler için en iyi sonucu garanti eden dinamik programlama kurun.

## Girdi ve çıktı

`dotnet run -- ornek-097/projeler.txt 8 ornek-097/rapor.txt`

Proje satırı `kod;maliyet;kazanım` biçimindedir: kod 1..9999 benzersiz; maliyet 1..500, kazanım 1..10000, en fazla 30 proje. İkinci argüman bütçedir, 0..500 tamsayıdır. `maliyet` ve `kazanım` bu problemin soyut birimleridir.

Rapor önce `T;harcama;kazanım`, ardından seçilen projeler için kod artan `P;kod` satırlarıdır. Boş küme geçerlidir; hiçbir proje seçilmediğinde yalnız `T;0;0` yazılır. Sözlük sırası kodları sayısal olarak karşılaştırır: ilk farklı kodu küçük olan liste önce gelir; bir liste diğerinin tam başlangıcıysa kısa liste önce gelir. Pozitif kazanım nedeniyle eşit optimumda bir listenin diğerinin gerçek başlangıcı olması mümkün değildir.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.


## Algoritma

1. Projeleri ve bütçeyi doğrulayın; proje dizinlerini kod artan sıralayın.
2. `dp[i,b]` durumunu i'den sonraki projelerle tam b harcayarak erişilen en yüksek kazanım olarak tanımlayın; erişilemez durum -1'dir.
3. Taban olarak projeler bitmişken yalnız sıfır harcama/sıfır kazanımı kabul edin.
4. Son projeden başa gidin. i projesini atlama ile bir kez seçme adaylarının kazanımlarını karşılaştırın.
5. Bütçeyi aşmayan harcamaları küçükten büyüğe tarayın; yalnız daha yüksek kazanımda güncelleyerek en düşük harcama eşitliğini koruyun.
6. Kod artan sırada, mevcut proje kalan tam maliyet ve kazanımı sağlayabiliyorsa onu seçin; kalan değerleri azaltın.
7. Toplamı ve seçimi raporlayın.

Dinamik programlama, aynı alt problemi tekrar tekrar çözmek yerine sonucunu bir durum tablosunda saklar. Seçme ve atlama dalları bütün olasılıkları kapsar; her iki dal i+1 satırına baktığından bir proje tekrar kullanılamaz. Taban `dp[n,0]=0`, diğer durumlar -1'dir. `0` ile `-1` birbirinden ayrılmalıdır.

Geri kurmada küçük kodu seçmek ancak geriye kalan tablonun tamamlayabildiği bir optimum olduğunda yapılır. Böylece sayısal sözlük sırası korunur. Pozitif maliyet kalan bütçeyi azaltır; en fazla n seçim denetlenir. Durum sayısı en fazla 31 × 501, kazanım en fazla 300000'dir. Süre O(n × bütçe), bellek O(n × bütçe) olup bütçenin sayısal büyüklüğüne bağlıdır.

Kodlar 10:(5,10), 20:(4,7), 30:(4,7), bütçe 8 için:

| Aday küme | Harcama | Kazanım | Karar |
| --- | --- | --- | --- |
| 10 | 5 | 10 | Geçerli, en iyi değil |
| 20 | 4 | 7 | Geçerli |
| 20, 30 | 8 | 14 | En iyi |
| 10, 20 | 9 | 17 | Bütçeyi aşıyor |

10 en yüksek kazanım/maliyet oranına sahipken tek başına seçilmesi 14 kazanımı kaçırır. Tablo, 079'daki seç/dışla yaklaşımının optimum kazanım hedefi için yeniden kullanılmasını sağlar; bu kez hedef toplamlı kümelerin adedi sayılmaz.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 3) throw new FormatException("3 argüman gerekli.");
    checked
    {
        int[][] pro = Oku(args[0], new[] { 1, 1, 1 },
            new[] { 9999, 500, 10000 }, 30);
        int butce = Tam("argüman", 0, 2, args[1], 0, 500);
        Tekil(pro, args[0], 0);
        int[] sira = Sira(pro);
        int n = pro.Length;
        int[,] dp = new int[n + 1, butce + 1];
        for (int i = 0; i <= n; i++)
            for (int b = 0; b <= butce; b++) dp[i, b] = -1;
        dp[n, 0] = 0;

        for (int i = n - 1; i >= 0; i--)
        {
            int p = sira[i], tutar = pro[p][1], kazanc = pro[p][2];
            for (int b = 0; b <= butce; b++)
            {
                dp[i, b] = dp[i + 1, b];
                if (b >= tutar && dp[i + 1, b - tutar] >= 0)
                {
                    int aday = kazanc + dp[i + 1, b - tutar];
                    if (aday > dp[i, b]) dp[i, b] = aday;
                }
            }
        }
        int harcama = 0, enIyi = 0;
        for (int b = 0; b <= butce; b++)
            if (dp[0, b] > enIyi)
            { harcama = b; enIyi = dp[0, b]; }

        var rapor = new StringBuilder();
        Satir(rapor, "T", harcama, enIyi);
        int kalan = harcama, hedef = enIyi;
        for (int i = 0; i < n; i++)
        {
            int p = sira[i], tutar = pro[p][1], kazanc = pro[p][2];
            if (kalan >= tutar && dp[i + 1, kalan - tutar] >= 0 &&
                kazanc + dp[i + 1, kalan - tutar] == hedef)
            {
                Satir(rapor, "P", pro[p][0]);
                kalan -= tutar; hedef -= kazanc;
            }
        }
        Yaz(args[2], rapor.ToString());
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

Yukarıdaki komutta projeler.txt dosyasının tam içeriği:

```text
30;4;7
10;5;10
20;4;7
```

Yeni rapor.txt:

```text
T;8;14
P;20
P;30
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Aynı girdi ve bütçe 4 için tam rapor:

```text
T;4;7
P;20
```

20 ve 30 eşit maliyet/kazanımdadır; küçük kod seçilir. Aynı dosya ile bütçe 0 veya sıfır baytlık dosya ile bütçe 8 için rapor yalnız `T;0;0` satırıdır; konsol yine `Rapor yazıldı.` olur.

Geçersiz örnekte proje dosyası tek `1;0;9` satırıdır; bütçe 8. Rapor açılmaz; konsolun tam çıktısı:

```text
Hata: projeler.txt: satır 1, alan 2: Geçersiz tamsayı.
```

## Sınır durumları

- Bir projeyi bütçe yetse bile iki kez seçemezsiniz; aynı kodlu kayıt hata olur.
- Aynı kazanım daha düşük harcamayla elde edilebiliyorsa düşük harcama önce gelir; sözlük sırası yalnız bundan sonra uygulanır.
- Erişilemez tam harcama -1'dir; boş kümenin kazanımı 0'dır. Erişilemez duruma kazanım eklenmez.
- Başlangıç kodu sırası veya eşit maliyetli projelerin dosya sırası çıktıyı değiştirmez.
- 30 proje ve bütçe 500 tablo sınırıdır. Daha büyük bütçe veya proje sayısına genel performans sözü verilmez.

## Kazanımlar

- Durum, taban ve geçişi tanımlayarak optimumu bütün seçimler üzerinden kurma.
- Optimum sayısal değeri bulmak ile somut çözümü geri kurmayı ayırma.
- Proje maliyeti, kazanımı ve bütçe uygunluğunun neden birlikte doğrulanabileceğini açıklama; sonraki nesneye yönelik derste bu sözleşmeleri modelleme.

## Alıştırmalar

1. Bütçesi çok büyük, proje sayısı küçükken geri izlemeyle çözümü karşılaştırın; durum tablosunun hangi sınırda pahalılaştığını açıklayın.
2. Seçilen proje sayısına da bir üst sınır koymak için durumun hangi boyutunu eklemeniz gerektiğini belirleyin.
3. Eşit kazanımda en düşük harcama kuralını kaldırınca geri kurmanın nasıl değişeceğini örnekleyin.
