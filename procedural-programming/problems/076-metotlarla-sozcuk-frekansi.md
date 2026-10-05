---
id: "076"
order: 76
title: "Metotlarla sözcük frekansı raporu oluşturma"
level: "İleri"
prerequisites: ["053", "061", "062", "065", "075"]
concepts: ["İşlem hattı", "Geçerli eleman adedi", "Paralel diziler", "İki ölçütlü sıralama", "Metot sorumlulukları"]
---

# Metotlarla sözcük frekansı raporu oluşturma

## Problem tanımı

0..200 karakterlik, yalnız ASCII harfleri ve normal boşluk içeren bir satırın sözcük frekanslarını bulun. Büyük/küçük harf aynı kabul edilir. Sözcük, boşluklar arasında kalan kesintisiz harf bölümüdür. Farklı sözcükleri önce azalan frekansa, eşitlikte alfabetik sıraya göre yazdırın. Örneğin `Ali veli ALI` için `ali: 2`, ardından `veli: 1` gelir.

İşi üç hesaplama metoduna ayırın: sözcüklere ayırma, farklı sözcükleri sayma ve sözcük/adet çiftlerini birlikte sıralama. Her metot yalnız parametreleriyle çalışır; konsola erişmez. `Split`, düzenli ifade, sözlük, LINQ veya hazır sıralama kullanmayın. Metin için ASCII sınırı burada da geçerlidir; Türkçe alfabe sırası desteklendiği varsayılmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 ASCII harfi veya U+0020 boşluk; tek satır. |
| Ara değer | Sözcük ve adet dizileri | Yalnız döndürülen geçerli eleman adedi işlenir. |
| Çıktı | Özet | `Toplam sözcük: N`, `Farklı sözcük: K`. |
| Çıktı | Frekanslar | `Frekanslar:` ve her farklı sözcüğün satırı; boşsa `Frekanslar: Yok`. |

`Metin: ` istemini yanıtlayın. EOF, 201 veya daha fazla karakter, rakam, noktalama, Türkçe harf veya sekme için yalnız `Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.` yazılır. Boş satır ve yalnız boşluklar geçerlidir. Bütün karakterler, herhangi bir rapor oluşturulmadan doğrulanır.

## Algoritma

1. Satırı ve karakter kümesini doğrulayın; çalışma dizilerini giriş uzunluğunda oluşturun.
2. `Ayir` içinde boşlukları atlayın; her sözcüğü küçük harfe dönüştürüp sözcük dizisine yazın, adedi döndürün.
3. `Say` içinde her sözcüğü farklı sözcük dizisinde arayın; varsa sayacı artırın, yoksa yeni çift ekleyin.
4. `Sirala` içinde eklemeli sıralama yapın; frekansı büyük olan, eşit frekansta sözcüğü alfabetik küçük olan önce gelsin.
5. Her kaydırmada sözcük ile adet hücresini birlikte taşıyın.
6. Yalnız geçerli çiftleri ana programda yazdırın.

Her sözcük en az bir karakter içerdiği için giriş uzunluğundaki diziler yeterlidir. `Ayir` ve `Say` dönüşleri fiziksel kapasiteyi değil geçerli veri adedini bildirir. `string.CompareOrdinal`, küçük ASCII harfleri için kültürden bağımsız alfabetik karşılaştırma sağlar. Sayma ve sıralama en kötü durumda sözcük adedinin karesiyle orantılıdır; 200 karakter sınırı bu eğitim örneğinin iş yükünü sınırlar.

`B a b C a b` için metotlar arasındaki veri:

| Aşama | Geçerli veri |
| --- | --- |
| `Ayir` | `b, a, b, c, a, b`; adet 6 |
| `Say` | `b: 3`, `a: 2`, `c: 1`; farklı adet 3 |
| `Sirala` | `b: 3`, `a: 2`, `c: 1` |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Metin: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    char c = metin[i];
    if (!((c >= 'A' && c <= 'Z') ||
        (c >= 'a' && c <= 'z') || c == ' '))
    {
        Console.WriteLine("Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.");
        return;
    }
}

string[] sozcukler = new string[metin.Length];
string[] farkli = new string[metin.Length];
int[] adetler = new int[metin.Length];
int n = Ayir(metin, sozcukler);
int k = Say(sozcukler, n, farkli, adetler);
Sirala(farkli, adetler, k);
Console.WriteLine("Toplam sözcük: " + n.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Farklı sözcük: " + k.ToString(CultureInfo.InvariantCulture));
Console.WriteLine(k == 0 ? "Frekanslar: Yok" : "Frekanslar:");
for (int i = 0; i < k; i++)
{
    Console.WriteLine(farkli[i] + ": " +
        adetler[i].ToString(CultureInfo.InvariantCulture));
}

static int Ayir(string metin, string[] sozcukler)
{
    char[] tampon = new char[metin.Length];
    int i = 0, adet = 0;
    while (i < metin.Length)
    {
        while (i < metin.Length && metin[i] == ' ')
        {
            i++;
        }
        int uzunluk = 0;
        while (i < metin.Length && metin[i] != ' ')
        {
            char c = metin[i++];
            tampon[uzunluk++] = c >= 'A' && c <= 'Z'
                ? (char)(c + ('a' - 'A')) : c;
        }
        if (uzunluk > 0)
        {
            sozcukler[adet++] = new string(tampon, 0, uzunluk);
        }
    }
    return adet;
}

static int Say(string[] sozcukler, int n, string[] farkli, int[] adetler)
{
    int k = 0;
    for (int i = 0; i < n; i++)
    {
        int j = 0;
        while (j < k && farkli[j] != sozcukler[i])
        {
            j++;
        }
        if (j == k)
        {
            farkli[k] = sozcukler[i];
            adetler[k] = 0;
            k++;
        }
        adetler[j]++;
    }
    return k;
}

static void Sirala(string[] sozcukler, int[] adetler, int n)
{
    for (int i = 1; i < n; i++)
    {
        string sozcuk = sozcukler[i];
        int adet = adetler[i], j = i - 1;
        while (j >= 0 && (adetler[j] < adet ||
            (adetler[j] == adet &&
            string.CompareOrdinal(sozcukler[j], sozcuk) > 0)))
        {
            sozcukler[j + 1] = sozcukler[j];
            adetler[j + 1] = adetler[j];
            j--;
        }
        sozcukler[j + 1] = sozcuk;
        adetler[j + 1] = adet;
    }
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Hesaplama metotlarının önkoşulları, doğrulanmış ASCII metni ve yeterli kapasiteli dizilerdir. `Say` tarafından oluşturulan çiftler aynı dizinle ilişkilidir; yalnız adetleri sıralamak yanlış sözcüğe yanlış frekans bağlar. Kullanılmayan `string` hücrelerine erişilmez. Özgün giriş metni korunur; küçük harf dönüşümü yalnız sözcük kopyalarında yapılır.

## Örnek çalıştırmalar

Bloklar `Metin: ` isteminden sonraki tam sonuçtur.

`Ali veli ALI`:

```text
Toplam sözcük: 3
Farklı sözcük: 2
Frekanslar:
ali: 2
veli: 1
```

`z A z a b`:

```text
Toplam sözcük: 5
Farklı sözcük: 3
Frekanslar:
a: 2
z: 2
b: 1
```

Boş satır veya yalnız boşluk:

```text
Toplam sözcük: 0
Farklı sözcük: 0
Frekanslar: Yok
```

## Sınır durumları

- Başta, sonda veya arada birden fazla boşluk boş sözcük üretmez.
- Aynı harfin farklı büyüklükleri aynı sözcükte birleştirilir.
- Frekans eşitliğinde `a`, `z` sözcüğünden önce gelir.
- `a` ve `aa` farklıdır; önek olan kısa sözcük alfabetik olarak önce gelir.
- 200 harflik tek sözcük geçerlidir; tamponun tamamı kullanılabilir.
- 200 karakterde en fazla 100 sözcük bulunur; bu üst sınıra tek harf/boşluk düzeniyle ulaşılır.

## Kazanımlar

- Bir metin raporunu birbirini tamamlayan üç metotla kurma.
- Dizi kapasitesi ile metotların döndürdüğü geçerli adetleri ayırma.
- Paralel dizilerdeki ilişkileri kaydırma boyunca koruma.
- İki sıralama ölçütünü açık öncelikle birleştirme.
- Hesaplama metotlarını konsol bağımlılığı olmadan yeniden kullanma.

## Alıştırmalar

1. Yalnız ilk üç farklı sözcüğü raporlayın; üçten az sözcük durumunu koruyun.
2. En yüksek frekansa sahip bütün sözcükleri ayrı bir metotla bulun.
3. Sözcüklerin ilk görüldükleri dizinlerini üçüncü paralel diziyle izleyin ve sıralarken birlikte taşıyın.
