---
id: "049"
order: 49
title: "İki dizinin ortak değerlerini bulma"
level: "İleri"
prerequisites: ["031", "042", "048"]
concepts: ["İki dizide arama", "Ortak değerler", "Tekrar eleme", "Geçerli sonuç bölümü", "İlk görülme sırası"]
---

# İki dizinin ortak değerlerini bulma

## Problem tanımı

İki tamsayı dizisinde de bulunan farklı değerleri bulun. Bir değer dizilerden birinde veya ikisinde tekrarlansa da sonuçta yalnızca bir kez yer almalıdır. Çıktı, ilk dizide değerlerin ilk görülme sırasını korumalıdır; sayısal sıralama yapılmaz. Örneğin A dizisi `3, 1, 3, 2`, B dizisi `2, 3, 3, 8, 1` ise sonuç `3 1 2` olur.

A'nın her elemanını önce B içinde arayın. B'de bulunan değeri, sonuç dizisinin doldurulmuş bölümünde de arayın; daha önce eklenmemişse sona ekleyin. Sonuç dizisinin kapasitesi A'nın uzunluğu kadar yeterlidir, ancak geçerli sonuç sayısı ayrıca tutulmalıdır. Yeni bir tamsayı dizisindeki başlangıç sıfırları, doldurulmadıkça sonuç değeri sayılmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, `m` | Sırasıyla A ve B uzunlukları; her biri 1..20 aralığında `int`. |
| Girdi | `a`, `b` | Tam `int` aralığında sırasıyla `n` ve `m` eleman. |
| Ara değer | `ortaklar` | Kapasitesi `n`; yalnızca 0..`adet - 1` bölümü doludur. |
| Çıktı | Ortak liste | `Ortak değerler:` ardından birer boşlukla değerler; boşsa `Ortak değerler: Yok`. |
| Çıktı | `adet` | `Adet: N` biçiminde farklı ortak değer sayısı. |

Önce `A uzunluğu (1..20): `, sonra `B uzunluğu (1..20): ` istemlerini yanıtlayın. Ardından bütün A elemanlarını `A[0]: ` ile, sonra bütün B elemanlarını `B[0]: ` ile başlayan istemlerde ayrı satırlarda girin. Bütün girdiler doğrulanmadan ortak liste yazılmaz.

## Algoritma

1. İki uzunluğu okuyup 1..20 aralığını doğrulayın.
2. İki diziyi oluşturup A ve B elemanlarını ayrı okuma döngülerinde doğrulayarak kaydedin.
3. A uzunluğunda sonuç dizisi oluşturun ve geçerli sonuç adedini 0 yapın.
4. A elemanlarını giriş sırasıyla dolaşın.
5. Mevcut A değerini B içinde arayın; ilk eşleşmede B aramasını bitirin.
6. B'de bulunmuşsa aynı değeri sonuç dizisinin yalnızca geçerli bölümünde arayın.
7. Henüz sonuçta yoksa `ortaklar[adet]` konumuna yazıp adedi artırın.
8. Döngü bitince geçerli sonuç bölümünü ve adedi yazdırın; adet 0 ise listeye ` Yok` ekleyin.

Dış döngü adımı sonunda sonuç bölümünde, işlenmiş A elemanları arasından B'de bulunan bütün farklı değerler vardır. Tekrar denetimi aynı değerin yeniden eklenmesini engeller. A sırayla tarandığı ve her yeni sonuç sona eklendiği için ilk görülme sırası korunur. Her arama sonlu bir dizi bölümünü tarar veya eşleşmede biter. Sonuç adedi A uzunluğunu aşamaz.

Yukarıdaki örneğin A taraması:

| A dizini | Değer | B'de var mı? | Önceden sonuçta mı? | Geçerli sonuç |
| --- | --- | --- | --- | --- |
| 0 | 3 | Evet | Hayır | 3 |
| 1 | 1 | Evet | Hayır | 3, 1 |
| 2 | 3 | Evet | Evet | 3, 1 |
| 3 | 2 | Evet | Hayır | 3, 1, 2 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("A uzunluğu (1..20): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 20)
{
    Console.WriteLine("Hata: A uzunluğu 1 ile 20 arasında bir tamsayı olmalıdır.");
    return;
}
Console.Write("B uzunluğu (1..20): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int m) || m < 1 || m > 20)
{
    Console.WriteLine("Hata: B uzunluğu 1 ile 20 arasında bir tamsayı olmalıdır.");
    return;
}

int[] a = new int[n];
int[] b = new int[m];
for (int i = 0; i < n; i++)
{
    Console.Write("A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: A elemanı int aralığında bir tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

for (int i = 0; i < m; i++)
{
    Console.Write("B[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: B elemanı int aralığında bir tamsayı olmalıdır.");
        return;
    }
    b[i] = deger;
}

int[] ortaklar = new int[n];
int adet = 0;
for (int i = 0; i < n; i++)
{
    bool bdeVar = false;
    for (int j = 0; j < m; j++)
    {
        if (a[i] == b[j])
        {
            bdeVar = true;
            break;
        }
    }
    if (bdeVar)
    {
        bool ekli = false;
        for (int j = 0; j < adet; j++)
        {
            if (ortaklar[j] == a[i])
            {
                ekli = true;
                break;
            }
        }
        if (!ekli)
        {
            ortaklar[adet] = a[i];
            adet++;
        }
    }
}

Console.Write("Ortak değerler:");
for (int i = 0; i < adet; i++)
{
    Console.Write(" " + ortaklar[i].ToString(CultureInfo.InvariantCulture));
}
if (adet == 0)
{
    Console.Write(" Yok");
}
Console.WriteLine();
Console.WriteLine("Adet: " + adet.ToString(CultureInfo.InvariantCulture));
```

İki arama farklı soruları yanıtlar: B taraması değerin ortak olup olmadığını, sonuç taraması daha önce yazılıp yazılmadığını belirler. Her A elemanında iki durum da yeniden başlatılır. Sonuç aramasında `j < ortaklar.Length` kullanmak henüz doldurulmamış sıfırları yanlışlıkla sonuç kabul edebilir; doğru sınır `j < adet` olur.

## Örnek çalıştırmalar

Girdi sırası `n`, `m`, bütün A elemanları, bütün B elemanlarıdır. Metin blokları istemlerden sonraki bütün sonuç satırlarını gösterir.

`n = 4`, `m = 5`, A `3, 1, 3, 2`, B `2, 3, 3, 8, 1` için tam çıktı:

```text
Ortak değerler: 3 1 2
Adet: 3
```

`n = 2`, `m = 2`, A `1, 2`, B `3, 4` için tam çıktı:

```text
Ortak değerler: Yok
Adet: 0
```

`n = 1`, `m = 1`, A `-2147483648`, B `-2147483648` için tam çıktı:

```text
Ortak değerler: -2147483648
Adet: 1
```

`n = 3`, `m = 2`, A `0, 0, 2147483647`, B `2147483647, 0` için tam çıktı:

```text
Ortak değerler: 0 2147483647
Adet: 2
```

Aşağıdaki hatalar tek satır üretir; ortak liste veya adet yazılmaz.

| Girdi | Tam sonuç |
| --- | --- |
| A uzunluğu `0`, `21` veya `iki` | `Hata: A uzunluğu 1 ile 20 arasında bir tamsayı olmalıdır.` |
| A uzunluğu `1`, B uzunluğu `0`, `21` veya `1.5` | `Hata: B uzunluğu 1 ile 20 arasında bir tamsayı olmalıdır.` |
| `n = 1`, `m = 1`, A elemanı `2147483648` veya `sayı` | `Hata: A elemanı int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, `m = 1`, A `0`, B elemanı `-2147483649` veya `1.5` | `Hata: B elemanı int aralığında bir tamsayı olmalıdır.` |
| Geçerli uzunluklardan sonra A için boş satır veya giriş sonu | `Hata: A elemanı int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, `m = 1`, A `0`, sonra B için boş satır veya giriş sonu | `Hata: B elemanı int aralığında bir tamsayı olmalıdır.` |

## Sınır durumları

- Ortak değer bulunmaması geçerli sonuçtur; adet 0 ve `Yok` yazılır.
- Aynı değer A'da, B'de veya ikisinde tekrar etse de sonuçta bir kez bulunur.
- 0 geçerli bir değerdir; sonuç dizisinin boş kapasitesindeki başlangıç sıfırlarından ayrı düşünülmelidir.
- Sonuç sırası A'nın ilk görülme sırasıdır; A ve B yer değiştirirse sıralama değişebilir, ortak değer kümesi değişmez.
- Sonuç adedi hem A'nın hem B'nin farklı değer sayısından büyük olamaz; kapasite `n` yeterlidir.
- Bütün işlemler eşitlik ve dizin denetimidir; elemanların tam `int` uçları üzerinde aritmetik yapılmaz.
- Geçersiz uzunluk, eleman biçimi, boş satır veya EOF durumunda sonraki okumalar durur ve sonuç üretilmez.

## Kazanımlar

- İki farklı dizi bölümünde farklı amaçlarla doğrusal arama uygulama.
- Ortaklık denetimiyle tekrar denetiminin görevlerini ayırma.
- Dizi kapasitesi ile geçerli sonuç sayısını ayrı yönetme.
- 0 gibi başlangıç değeriyle aynı verileri yalnızca dolu bölümde arama.
- İlk dizinin ilk görülme sırasını koruyarak farklı ortak değerler üretme.

## Alıştırmalar

1. A ve B'nin yerini değiştirerek aynı girdiyi deneyin; ortak değerlerin değişmediğini, çıktı sırasının değişebildiğini gösterin.
2. A'da olup B'de olmayan farklı değerleri aynı sonuç dizisi yaklaşımıyla üretin.
3. Her ortak değerin A ve B içindeki ayrı tekrar sayılarını da hesaplayıp değerle beraber yazdırın; sonuçta değer başına tek satır üretin.
