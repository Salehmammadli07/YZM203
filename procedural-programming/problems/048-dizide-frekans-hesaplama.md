---
id: "048"
order: 48
title: "Dizide değerlerin frekanslarını hesaplama"
level: "İleri"
prerequisites: ["010", "041", "046"]
concepts: ["Frekans dizisi", "Değerden dizine dönüşüm", "Sıfır başlangıç", "Biriktirme", "Artan değer sırası"]
---

# Dizide değerlerin frekanslarını hesaplama

## Problem tanımı

-10 ile 10 arasındaki tamsayılardan oluşan bir dizide her farklı değerin kaç kez bulunduğunu hesaplayın. Yalnızca dizide bulunan değerleri küçükten büyüğe yazdırın. Örneğin `-10, 0, 10, -10, 0` için -10 iki kez, 0 iki kez, 10 bir kez bulunur.

Değer aralığı dar ve bilindiği için her olası değere ayrı bir sayaç ayırın. -10 için 0, 0 için 10, 10 için 20 dizinini kullanın; dönüşüm `deger + 10` olur. Dizi elemanlarını önce okuyup saklayın, ardından 21 elemanlı frekans dizisini doldurun. Böylece bir eleman hatalı olduğunda kısmi frekans listesi yazılmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; veri dizisinin uzunluğu. |
| Girdi | `a` | Her biri -10..10 aralığında `n` adet `int`. |
| Ara değer | `frekans` | 21 tamsayı sayacı; `i` dizini `i - 10` değerini sayar. |
| Çıktı | Frekans listesi | `Frekanslar:` başlığından sonra `Değer: X, Adet: Y`; sıfır olmayan sayaçlar. |

`Eleman sayısı (1..50): ` isteminden sonra `A[0]: ` ile başlayan istemlerde her elemanı ayrı satıra girin. Geçersiz uzunlukta uzunluk hatası, geçersiz elemanda aralık hatası yazılır; liste başlığı üretilmez. Çıktı değer sırasındadır; girdide ilk görülme sırası kullanılmaz.

## Algoritma

1. Uzunluğu doğrulayıp veri dizisini oluşturun.
2. Bütün elemanları okuyup -10..10 aralığını doğrulayarak diziye kaydedin.
3. 21 elemanlı frekans dizisi oluşturun; başlangıçtaki bütün sayaçlar 0'dır.
4. Veri dizisinin her elemanı için `frekans[a[i] + 10]` sayacını bir artırın.
5. Liste başlığını yazdırın.
6. Frekans dizinlerini 0'dan 20'ye tarayın.
7. Sayaç sıfırdan büyükse değeri `i - 10` ile geri dönüştürüp değer ve adedi yazdırın.

| Veri değeri | Frekans dizini | Ters dönüşüm |
| --- | --- | --- |
| -10 | 0 | `0 - 10 = -10` |
| 0 | 10 | `10 - 10 = 0` |
| 10 | 20 | `20 - 10 = 10` |

Sayma döngüsünün her adımı sonunda frekanslar, işlenmiş veri bölümündeki adetleri temsil eder. Frekansların toplamı işlenmiş eleman sayısına eşittir; döngü sonunda bu toplam `n` olur. Değerden dizine dönüşüm, kabul edilen bütün değerleri 0..20 aralığına taşır. Son tarama dizin artışını değer artışına dönüştürür. Bütün döngüler sonlu dizi uzunluklarıyla sınırlıdır.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Eleman sayısı (1..50): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 50)
{
    Console.WriteLine("Hata: Eleman sayısı 1 ile 50 arasında " +
        "bir tamsayı olmalıdır.");
    return;
}

int[] a = new int[n];
for (int i = 0; i < n; i++)
{
    Console.Write("A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger) ||
        deger < -10 || deger > 10)
    {
        Console.WriteLine("Hata: Eleman -10 ile 10 arasında " +
            "bir tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

int[] frekans = new int[21];
for (int i = 0; i < n; i++)
{
    frekans[a[i] + 10]++;
}

Console.WriteLine("Frekanslar:");
for (int i = 0; i < frekans.Length; i++)
{
    if (frekans[i] > 0)
    {
        Console.WriteLine("Değer: " +
            (i - 10).ToString(CultureInfo.InvariantCulture) +
            ", Adet: " + frekans[i].ToString(CultureInfo.InvariantCulture));
    }
}
```

`new int[21]`, bütün elemanları otomatik olarak 0 ile başlatır; ayrıca sıfırlama döngüsü gerekmez. `frekans[a[i] + 10]++` hem hangi sayacın kullanılacağını seçer hem o sayacı artırır. Veri dizisini sıralamak gerekmez; çıktı frekans dizisi taranarak sıralı olur.

## Örnek çalıştırmalar

Uzunluk ve elemanlar ayrı satırlarda girilir. Metin blokları istemler dışındaki bütün sonuç satırlarını gösterir.

`n = 5`, dizi `-10, 0, 10, -10, 0` için tam çıktı:

```text
Frekanslar:
Değer: -10, Adet: 2
Değer: 0, Adet: 2
Değer: 10, Adet: 1
```

`n = 1`, dizi `-10` için tam çıktı:

```text
Frekanslar:
Değer: -10, Adet: 1
```

`n = 4`, dizi `5, 5, 5, 5` için tam çıktı:

```text
Frekanslar:
Değer: 5, Adet: 4
```

Aşağıdaki hata örneklerinde yalnızca belirtilen tek satır yazılır.

| Girdi | Tam sonuç |
| --- | --- |
| Uzunluk `0`, `51` veya `iki` | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `11` veya `-11` | `Hata: Eleman -10 ile 10 arasında bir tamsayı olmalıdır.` |
| `n = 2`, ilk eleman `0`, ikinci eleman `sayı` | `Hata: Eleman -10 ile 10 arasında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `1.5` veya `2147483648` | `Hata: Eleman -10 ile 10 arasında bir tamsayı olmalıdır.` |
| `n = 1`, sonra boş satır veya girişin sonu | `Hata: Eleman -10 ile 10 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- -10 ve 10, sırasıyla ilk ve son frekans konumuna denk gelir; ikisi de geçerlidir.
- 0 özel bir bitiş işareti değildir; normal bir veri değeridir ve 10 numaralı sayaçta sayılır.
- Bütün elemanlar eşitse yalnızca bir değer satırı yazılır; sayacı `n` olur.
- Bütün sayaçlar en fazla 50'dir ve toplamları dizi uzunluğuna eşittir; `int` taşması oluşmaz.
- Dizide bulunmayan değerlerin sayaçları 0 kalır ve çıktı satırı üretilmez.
- Girdi aralığı denetlenmeden `deger + 10` dizini kullanılmaz. Boş, EOF, metin, ondalık ve `int` dışı girişler tek hata satırıyla reddedilir.

## Kazanımlar

- Dar bir değer aralığının tamamı için sayaç dizisi oluşturma.
- Negatif değerleri kaydırmayla geçerli sıfır tabanlı dizine dönüştürme.
- Aynı dönüşümün tersini kullanarak dizinden veri değerini elde etme.
- Frekans toplamının veri sayısına eşit olduğunu kontrol etme.
- Veri sırası değişse de frekansların aynı kaldığını gösterme.

## Alıştırmalar

1. En yüksek frekansı ve bu frekansa sahip bütün değerleri yazdırın; eşit adet durumunda değerleri artan sırada tutun.
2. Değer aralığını -20..20 yapın; frekans uzunluğunu, kaydırmayı ve ters dönüşümü birlikte güncelleyin.
3. Her değer için adet kadar `*` yazdırarak yatay bir frekans görünümü oluşturun; boş sayaçları yine atlayın.
