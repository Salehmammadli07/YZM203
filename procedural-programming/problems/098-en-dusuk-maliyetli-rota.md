---
id: "098"
order: 98
title: "Yönlü ulaşım ağında en düşük maliyetli rota"
level: "İleri"
prerequisites: ["041", "047", "078", "089", "090"]
concepts: ["Yönlü çizge", "Komşuluk matrisi", "Dijkstra", "İki ölçütlü uzaklık", "Rota geri kurma"]
---

# Yönlü ulaşım ağında en düşük maliyetli rota

## Problem tanımı

Bir yönlü ağda başlangıçtan hedefe toplam maliyeti en küçük rotayı bulun. Aynı maliyette daha az bağlantıyı; yine eşitlikte düğüm kodları listesinin sayısal sözlük sırasına göre ilkini seçin. Ulaşılamayan hedef normal sonuçtur. Ağı hazır çizge kitaplığı veya öncelik kuyruğuyla çözmeyin; dizilerle Dijkstra algoritmasını kurun.

## Girdi ve çıktı

`dotnet run -- ornek-098/dugumler.txt ornek-098/baglantilar.txt 1 4 ornek-098/rapor.txt`

Düğüm satırı `kod` (1..9999, benzersiz, en fazla 20 düğüm); bağlantı satırı `kaynakKod;hedefKod;maliyet` (kodlar 1..9999, maliyet 1..10000, en fazla 100 bağlantı). İki uç mevcut olmalıdır. Aynı yönlü çift tekrar edemez; düğümün kendisine bağlantısı yasaktır. Ters yöndeki bağlantı ayrı kayıttır. Başlangıç ve hedef üçüncü/dördüncü argümandaki mevcut düğüm kodlarıdır.

Rapor `M;toplamMaliyet`, `K;bağlantıSayısı`, `Y;kod1;kod2;...` olmak üzere üç satırdır. Yol başlangıç ve hedefi içerir. Yol yoksa tek `ROTA_YOK` satırıdır. Başlangıç hedefse maliyet ve bağlantı sayısı 0, yol tek düğümdür. Sözlük sırası ilk farklı düğüm kodunu sayısal karşılaştırır.

Her çözüm bağımsız .NET 10 konsol projesinin Program.cs dosyasında çalışır. Komut satırı argümanları aşağıdaki sıradadır; konsol girdi istemi yoktur. Göreli yollar çalışma klasörüne göredir. Örnek dosyalarını kendinize ait boş bir klasörde hazırlayın. Son argüman yeni rapor yoludur; üst klasörü önceden oluşturun.

Girişler başlıksız UTF-8 metindir; isteğe bağlı ilk BOM, LF ve CRLF kabul edilir. Ayırıcı noktalı virgüldür; tırnak, çevre boşluğu, yorum veya boş satır kabul edilmez. Dosya başına 32768 bayt, varsayılan 100 fiziksel satır, satır başına 120 UTF-16 kod birimi sınırı vardır; daha küçük problem sınırları ayrıca belirtilir. `ReadLine` sonucunda null EOF, boş metin gerçek boş satırdır. Son tek satır sonu ek kayıt oluşturmaz. Sıfır baytlık dosya sıfır kayıttır.

Tamsayılar yalnız ASCII rakamlarla yazılır; baştaki sıfırlar kabul edilir ve aynı sayısal anahtarı belirtir. İşaret, binlik ayırıcı, üs ve ondalık kabul edilmez. Ayrıştırma ve çıktı CultureInfo.InvariantCulture kullanır. Tarih alanı varsa tam `yyyy-MM-dd`, 2000–2099 aralığında gerçek takvim tarihidir; kod DateOnly ile doğrulayıp gün numarasına dönüştürür. Ondalıklı sonuçlarda nokta ve problemde belirtilen basamak sayısı kullanılır.

İlk hata işlemi durdurur. Alan hatası `Hata: dosya: satır S, alan A: açıklama` biçimindedir; satır ve alan 1 tabanlıdır, kayıt/genel satır hatasında alan 0 kullanılır. Bütün dosyalar argüman sırasıyla okunur ve ayrıştırılır; ardından anahtar ve ilişki denetimleri çözümde gösterilen sırayla yapılır. Daha sonraki dosyanın biçim hatası önceki dosyanın ilişki hatasından önce bildirilebilir. Eksik dosya veya I/O sorunu `Hata: Dosya işlemi başarısız.`; erişim engeli `Hata: Dosyaya erişim izni yok.`; geçersiz yol veya UTF-8 `Hata: Yol veya UTF-8 kodlaması geçersiz.`; desteklenmeyen yol `Hata: Yol biçimi desteklenmiyor.` üretir. `using` hata sırasında da kaynakları kapatır.

Rapor yalnız bütün doğrulama ve hesaplamalar başarılıysa açılır. `FileMode.CreateNew` mevcut dosyayı, girişle aynı yol olsa bile, ezmez; I/O hatası bildirir. Giriş dosyaları yalnız okunur. Çıktı UTF-8 BOM olmadan LF ile yazılır; her kayıt son LF ile biter. Boş rapor sıfır bayttır. Başarıda konsolun tam çıktısı `Rapor yazıldı.` satırıdır; bu mesaj akış kapandıktan sonra yazılır. Yazım sırasında I/O hatası yeni dosyada kısmi içerik bırakabilir; başarı bildirilmez, yeni dosyayı denetleyip farklı adla tekrar deneyin. Atomik yayın garantisi yoktur. Giriş hatasında rapor hiç oluşturulmaz. Hesaplar `checked` bloğundadır; beklenmeyen taşmada `Hata: Hesaplama taştı.` yazılır ve rapor açılmaz. Aşağıdaki sınırlar geçerli girdilerde bu taşmayı önler.


## Algoritma

1. Katalog, bağlantılar, benzersiz çiftler ve bütün ilişkileri doğrulayın; yönlü maliyet matrisini kurun.
2. Hedefin uzaklığını ve bağlantı sayısını 0, diğerlerini sonsuz yapın.
3. Kesinleşmemiş düğümlerden uzaklık/bağlantı sayısı en küçük olanı seçin.
4. Seçilen v düğümüne gelen u→v bağlantılarıyla u'nun hedefe gidiş çiftini iyileştirin.
5. Ulaşılabilir düğümler bitene kadar devam edin.
6. Başlangıçtan itibaren en iyi maliyet ve bağlantı çiftini koruyan komşuların en küçük kodlusunu seçerek yolu geri kurun.
7. Yol veya ulaşılamama sonucunu raporlayın.

Çizge, düğümler ve onları birleştiren bağlantılardan oluşur. `maliyet[u,v]`, u→v bağlantısının değeridir; 0 bağlantı yok demektir ve girdi maliyetleri pozitiftir. Dijkstra, geçici uzaklıkların en küçüğünü kesinleştirir. Pozitif bağlantılarla daha sonra daha pahalı bir düğüm üzerinden bu kesin değeri düşürmek mümkün değildir.

Bu çözüm hedefe doğru uzaklıkları bulur: hedefte başlayıp gelen bağlantıları inceler. Bu, yönleri ters çevrilmiş ağda Dijkstra uygulamaktır; asıl yolun yönü değiştirilmez. Eşit maliyette en az bağlantı da saklanır. Sonsuz değere toplama yapılmaz. En iyi yol pozitiftir ve döngü içermez; en fazla 19 bağlantı, 190000 maliyet vardır.

Rota kurulurken yalnız optimum tamamlaması olan komşular kabul edilir. En küçük sonraki kod, sözlük sırasının ilk farkını belirler. Her adımda kalan bağlantı sayısı 1 azalır; yol sonlanır. Uzaklık hesabı ve geri kurma, doğrulama sonrasında O(n²) süre, matris O(n²) bellek kullanır.

1→2:2, 1→3:2, 2→4:3, 3→4:3 ve 1→4:9 için:

| Hedefe doğru adım | Kesin düğüm | Yeni uzaklık / bağlantı |
| --- | --- | --- |
| 1 | 4 | 2:(3,1), 3:(3,1), 1:(9,1) |
| 2 | 2 | 1:(5,2) |
| 3 | 3 | 1 aynı kalır |
| 4 | 1 | Hesap biter |

1'den geri kurmada 2 ve 3 optimumu korur; küçük kod 2 seçilir. Matris dizinleri kod değildir; `Bul` bu dönüşümü yapar, `EnKisa` yalnız bellek sonuçlarını döndürür.

## C# çözümü

```csharp
using System;
using System.IO;
using System.Text;
using System.Globalization;

try
{
    if (args.Length != 5) throw new FormatException("5 argüman gerekli.");
    checked
    {
        int[][] dug = Oku(args[0], new[] { 1 }, new[] { 9999 }, 20);
        int[][] bag = Oku(args[1], new[] { 1, 1, 1 },
            new[] { 9999, 9999, 10000 });
        int basKod = Tam("argüman", 0, 3, args[2], 1, 9999);
        int sonKod = Tam("argüman", 0, 4, args[3], 1, 9999);
        Tekil(dug, args[0], 0); Tekil(bag, args[1], 0, 1);
        int n = dug.Length;
        int[,] maliyet = new int[n, n];
        for (int i = 0; i < bag.Length; i++)
        {
            int u = Bul(dug, bag[i][0]), v = Bul(dug, bag[i][1]);
            if (u < 0 || v < 0)
                Hata(args[1], i + 1, 0, "Düğüm bulunamadı.");
            if (u == v) Hata(args[1], i + 1, 0, "Öz bağlantı yasak.");
            maliyet[u, v] = bag[i][2];
        }
        int bas = Bul(dug, basKod), son = Bul(dug, sonKod);
        if (bas < 0 || son < 0)
            throw new FormatException("Başlangıç veya hedef bulunamadı.");

        EnKisa(maliyet, dug, son, out long[] uzak, out int[] adim);
        var rapor = new StringBuilder();
        if (uzak[bas] == long.MaxValue) rapor.Append("ROTA_YOK\n");
        else
        {
            Satir(rapor, "M", uzak[bas]);
            Satir(rapor, "K", adim[bas]);
            int an = bas;
            rapor.Append("Y;").Append(dug[an][0].ToString(
                CultureInfo.InvariantCulture));
            while (an != son)
            {
                int sec = -1;
                for (int v = 0; v < n; v++)
                    if (maliyet[an, v] > 0 && uzak[v] != long.MaxValue &&
                        maliyet[an, v] + uzak[v] == uzak[an] &&
                        1 + adim[v] == adim[an] &&
                        (sec < 0 || dug[v][0] < dug[sec][0])) sec = v;
                if (sec < 0) throw new FormatException("Rota kurulamadı.");
                an = sec;
                rapor.Append(';').Append(dug[an][0].ToString(
                    CultureInfo.InvariantCulture));
            }
            rapor.Append('\n');
        }
        Yaz(args[4], rapor.ToString());
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

static void EnKisa(int[,] maliyet, int[][] dug, int hedef,
    out long[] uzak, out int[] adim)
{
    int n = dug.Length;
    uzak = new long[n]; adim = new int[n];
    bool[] kesin = new bool[n];
    for (int i = 0; i < n; i++)
    { uzak[i] = long.MaxValue; adim[i] = int.MaxValue; }
    uzak[hedef] = 0; adim[hedef] = 0;
    int[] sira = Sira(dug);

    for (int tur = 0; tur < n; tur++)
    {
        int v = -1;
        foreach (int i in sira)
            if (!kesin[i] && uzak[i] != long.MaxValue &&
                (v < 0 || uzak[i] < uzak[v] ||
                (uzak[i] == uzak[v] && adim[i] < adim[v]))) v = i;
        if (v < 0) break;
        kesin[v] = true;
        for (int u = 0; u < n; u++)
        {
            if (kesin[u] || maliyet[u, v] == 0) continue;
            long yeni = uzak[v] + maliyet[u, v];
            int uzunluk = adim[v] + 1;
            if (yeni < uzak[u] ||
                (yeni == uzak[u] && uzunluk < adim[u]))
            { uzak[u] = yeni; adim[u] = uzunluk; }
        }
    }
}
```

## Örnek çalıştırmalar

Yukarıdaki komutta dugumler.txt tam içeriği:

```text
4
1
3
2
```

baglantilar.txt tam içeriği:

```text
1;2;2
1;3;2
2;4;3
3;4;3
1;4;9
```

Yeni rapor.txt:

```text
M;5
K;2
Y;1;2;4
```

Konsolun tam çıktısı:

```text
Rapor yazıldı.
```

Son bağlantıyı `1;4;5` yapınca rapor `M;5`, `K;1`, `Y;1;4` satırlarıdır. Aynı katalogda başlangıç 4, hedef 1 ise tek `ROTA_YOK`; başlangıç/hedef 4 ise `M;0`, `K;0`, `Y;4` satırlarıdır. Bu başarılı durumlarda konsol yine `Rapor yazıldı.` olur.

Bağlantı dosyası tek `1;4;0` satırına değiştirilirse rapor açılmaz; konsolun tam çıktısı:

```text
Hata: baglantilar.txt: satır 1, alan 3: Geçersiz tamsayı.
```

## Sınır durumları

- Dosyada bulunmayan başlangıç/hedef hatadır; mevcut ama bağlantısız hedef `ROTA_YOK` sonucudur.
- Döngülü ağ geçerlidir; pozitif maliyet nedeniyle döngü eklemek en iyi yolu iyileştiremez.
- Bir yöndeki yol ters yönde var sayılmaz. Aynı çift tekrar ederse daha ucuzunu sessizce seçmek yerine hata verilir.
- Bağlantı sayısı eşitliği maliyet eşitliğinden sonra, sözlük sırası ikisinden sonra uygulanır.
- Negatif veya sıfır maliyet kabul edilmez; bu çözümün önkoşuludur.

## Kazanımlar

- Ağ ilişkilerini matrisle temsil etme; geçici ve kesin uzaklıkları ayırma.
- Optimumu koruyan seçimlerle deterministik yolu geri kurma.
- Düğüm kimliği, yönlü bağlantı ve maliyet doğrulamasının hangi veri/davranış birlikteliklerine karşılık geldiğini açıklama.

## Alıştırmalar

1. İki yönde aynı maliyetli bağlantıyı dosyada iki kayıtla gösterin; yönlülük kuralını açıklayın.
2. Her düğümün hedefe uzaklığını ayrıca raporlayın; ulaşılamayan düğümleri sayısal sonsuz yazmadan gösterin.
3. Sıfır maliyet kabul edilseydi sonlanma ve rota geri kurma gerekçelerinde hangi değişikliklerin gerekeceğini tartışın.
