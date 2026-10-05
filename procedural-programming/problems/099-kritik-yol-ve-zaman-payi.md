---
id: "099"
order: 99
title: "Bağımlı işlerde kritik yol ve zaman payı"
level: "İleri"
prerequisites: ["078", "090", "098"]
concepts: ["Bağımlılık çizgesi", "Topolojik sıra", "Kritik yol", "İleri ve geri geçiş", "Zaman payı"]
---

# Bağımlı işlerde kritik yol ve zaman payı

## Problem tanımı

İş süreleri ve öncelik ilişkilerinden projenin en erken bitişini bulun. Her işin en erken/en geç başlama ve bitişini, zaman payını ve kritik olup olmadığını raporlayın. Bir iş bütün öncülleri bitince başlayabilir. Hazır işler için yeterli kaynak vardır; aynı anda çalıştırılabilecek iş sayısı sınırlandırılmaz. Döngü varsa plan üretmeyin.

## Girdi ve çıktı

`dotnet run -- ornek-099/isler.txt ornek-099/bagimliliklar.txt ornek-099/rapor.txt`

İş satırı `kod;süreDakika`: kod 1..9999 benzersiz, süre 1..1000, en fazla 30 iş. Bağımlılık satırı `öncekiKod;sonrakiKod`: mevcut kodlar, en fazla 100 benzersiz yönlü çift. Öz bağımlılık yasaktır. Bir iş birden çok öncüle sahip olabilir; bu bir ağaç olmak zorunda değildir.

Proje 0. dakikada başlar. Rapor önce `T;projeBitişi`, ardından kod artan `kod;erkenBaşla;erkenBitir;geçBaşla;geçBitir;pay;durum` satırlarıdır. Durum sıfır payda `KRITIK`, diğerinde `PAYLI` olur. Boş proje için yalnız `T;0` yazılır. Birden fazla son iş ve bağımsız bileşen olabilir; hepsi aynı proje bitişine göre değerlendirilir.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.


## Algoritma

1. İşleri ve bağımlılıkları doğrulayın; komşuluk matrisi ile her işin giriş derecesini kurun.
2. Henüz alınmamış giriş derecesi 0 olan en küçük kodlu işi topolojik sıraya ekleyin; ardıllarının derecesini azaltın.
3. İşler kalmışken böyle bir aday yoksa döngü hatası verin.
4. Topolojik sırada erken bitişi erken başlangıç + süre yapın; ardılların erken başlangıcını bu bitişle yükseltin.
5. Proje bitişini erken bitişlerin en büyüğü yapın.
6. Ters topolojik sırada geç bitişi ardılların geç başlangıçlarının en küçüğü; ardıl yoksa proje bitişi yapın. Süreyi çıkarıp geç başlangıcı bulun.
7. Geç başlangıç - erken başlangıç farkını zaman payı olarak raporlayın.

Topolojik sıra her öncülü ardılından önce yerleştirir; döngüsüz yönlü çizgede mümkündür. Giriş derecesi, henüz alınmamış öncül sayısıdır. Bir döngüde hiçbir döngü işi derece 0'a inemez. Tüm bileşenler tarandığından bağımsız bir bileşendeki döngü de yakalanır.

Erken başlangıç öncül bitişlerinin en büyüğünü bekler; geri geçişte ardılları geciktirmemek için en küçük geç başlangıç sınırı kullanılır. Pay kadar gecikme, diğer işler uygun planlanırsa proje bitişini uzatmaz. Birden çok işin paylarını aynı anda bağımsız kullanabilecekleri varsayılmaz. Kritik işler sıfır paylıdır; birden çok kritik yol olabilir. Bu, 098'deki en kısa yol hedefinden farklı olarak süreyi belirleyen en uzun bağımlılık zincirini inceler.

Süre toplamı en fazla 30000 dakikadır. Doğrulama sonrası matris taramaları O(n²) süre ve O(n²) bellek kullanır. `topo` yürütme sırasıdır, kod sıralaması yalnız deterministik aday seçimi ve çıktıda kullanılır.

1:3, 2:2, 3:4, 4:1 süreleri ve 1→3, 2→3, 3→4 için:

| İş | Erken başla/bitir | Geç başla/bitir | Pay |
| --- | --- | --- | --- |
| 1 | 0 / 3 | 0 / 3 | 0 |
| 2 | 0 / 2 | 1 / 3 | 1 |
| 3 | 3 / 7 | 3 / 7 | 0 |
| 4 | 7 / 8 | 7 / 8 | 0 |

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
        int[][] isler = Oku(args[0], new[] { 1, 1 },
            new[] { 9999, 1000 }, 30);
        int[][] bag = Oku(args[1], new[] { 1, 1 },
            new[] { 9999, 9999 });
        Tekil(isler, args[0], 0); Tekil(bag, args[1], 0, 1);
        int n = isler.Length;
        bool[,] once = new bool[n, n];
        int[] derece = new int[n];
        for (int i = 0; i < bag.Length; i++)
        {
            int u = Bul(isler, bag[i][0]), v = Bul(isler, bag[i][1]);
            if (u < 0 || v < 0)
                Hata(args[1], i + 1, 0, "İş bulunamadı.");
            if (u == v) Hata(args[1], i + 1, 0, "Öz bağımlılık yasak.");
            once[u, v] = true; derece[v]++;
        }

        int[] kodSirasi = Sira(isler), topo = new int[n];
        bool[] alindi = new bool[n];
        for (int i = 0; i < n; i++)
        {
            int sec = -1;
            foreach (int p in kodSirasi)
                if (!alindi[p] && derece[p] == 0) { sec = p; break; }
            if (sec < 0)
                throw new FormatException("İş bağımlılıkları döngü içeriyor.");
            topo[i] = sec; alindi[sec] = true;
            for (int v = 0; v < n; v++) if (once[sec, v]) derece[v]--;
        }

        int[] eb = new int[n], es = new int[n];
        int[] gb = new int[n], gs = new int[n];
        int bitis = 0;
        foreach (int u in topo)
        {
            es[u] = eb[u] + isler[u][1];
            if (es[u] > bitis) bitis = es[u];
            for (int v = 0; v < n; v++)
                if (once[u, v] && eb[v] < es[u]) eb[v] = es[u];
        }
        for (int i = n - 1; i >= 0; i--)
        {
            int u = topo[i];
            gs[u] = bitis;
            for (int v = 0; v < n; v++)
                if (once[u, v] && gb[v] < gs[u]) gs[u] = gb[v];
            gb[u] = gs[u] - isler[u][1];
        }

        var rapor = new StringBuilder();
        Satir(rapor, "T", bitis);
        foreach (int u in kodSirasi)
        {
            int pay = gb[u] - eb[u];
            Satir(rapor, isler[u][0], eb[u], es[u], gb[u], gs[u], pay,
                pay == 0 ? "KRITIK" : "PAYLI");
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

Yukarıdaki komutta isler.txt tam içeriği:

```text
4;1
2;2
3;4
1;3
```

bagimliliklar.txt tam içeriği:

```text
1;3
2;3
3;4
```

Yeni rapor.txt:

```text
T;8
1;0;3;0;3;0;KRITIK
2;0;2;1;3;1;PAYLI
3;3;7;3;7;0;KRITIK
4;7;8;7;8;0;KRITIK
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

İki giriş dosyası sıfır baytken yeni rapor yalnız `T;0` satırıdır; konsol yine `Rapor yazıldı.` olur.

Normal örneğin bağımlılıklarına `4;1` satırı eklenirse rapor açılmaz; konsolun tam çıktısı:

```text
Hata: İş bağımlılıkları döngü içeriyor.
```

## Sınır durumları

- Öncülsüz işler 0'da başlayabilir; ardılsız işler ortak proje bitişine kadar pay taşıyabilir.
- Birleşen bağımlılıklarda bütün öncüller beklenir; bir önceki işin bitişini kullanmak yeterli değildir.
- Aynı çift iki kez yazılırsa giriş derecesi iki kat sayılmadan hata olur.
- Kısa bağımsız bileşenin işi otomatik kritik sayılmaz; proje bitişine göre hesaplanır.
- Negatif/sıfır süre kabul edilmez. Kaynak veya çalışan sayısı kısıtı eklenirse bu çözüm yeni koşul için yeterli değildir.

## Kazanımlar

- Topolojik sıranın hesaplama bağımlılıklarını nasıl sağladığını açıklama.
- İleri en büyük, geri en küçük sınırlarla zaman aralıklarını kurma.
- İş kimliği, süre ve bağımlılık davranışlarının sonraki derste hangi nesneler ve ilişkilerle modellenebileceğini belirleme.

## Alıştırmalar

1. Birden fazla kritik yol üreten küçük bir ağ kurun; sıfır paylı bütün işleri gösterin.
2. İş 2'nin payını kullanıp 1'de başlatın; proje bitişinin değişmediğini doğrulayın.
3. Tek çalışan şartı eklendiğinde bu planın neden uygulanamayabileceğini örnekleyin.
