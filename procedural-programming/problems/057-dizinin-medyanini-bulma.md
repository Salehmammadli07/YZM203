---
id: "057"
order: 57
title: "Dizinin medyanını bulma"
level: "İleri"
prerequisites: ["008", "051", "053"]
concepts: ["Seçmeli sıralama", "Medyan", "Tek ve çift uzunluk", "Tür genişletme", "Sabit çıktı biçimi"]
---

# Dizinin medyanını bulma

## Problem tanımı

1 ile 50 tamsayı içeren bir diziyi küçükten büyüğe sıralayın ve medyanını bulun. Medyan, sıralı verinin orta değeridir: uzunluk tekse ortadaki eleman, çiftse ortadaki iki elemanın aritmetik ortalamasıdır. Örneğin `7, 1, 4, 2` sıralanınca `1, 2, 4, 7` olur; medyan `(2 + 4) / 2 = 3.0` olarak yazılır. Medyan bütün elemanların aritmetik ortalaması değildir.

Sıralama için seçmeli sıralamayı döngülerle gerçekleştirin. Her turda kalan bölümün en küçük değerini bulup başına taşıyın. Elemanlar `int` türünün bütün aralığında olabilir. Çift uzunlukta orta değerleri toplarken toplamadan önce `long` türüne genişletin; sonucu `decimal` olarak hesaplayıp her zaman bir ondalık basamakla yazdırın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `a` | Her biri -2147483648..2147483647 aralığında `n` tamsayı. |
| Ara değer | `toplam` | Çift uzunlukta iki orta değerin `long` toplamı. |
| Çıktı | Sıralı değerler | `Sıralı dizi:` ardından birer boşlukla ayrılmış bütün değerler. |
| Çıktı | Medyan | `Medyan: X`; `decimal` sonuç, nokta ayırıcıyla bir ondalık basamak. |

`Eleman sayısı (1..50): ` istemini, ardından `A[0]: ` ile başlayan istemleri ayrı satırlarda yanıtlayın. Hatalı uzunluk veya elemanda yalnızca ilgili hata yazılır. Boş satır, girişin sonu (EOF), metin, ondalıklı giriş ve `int` sınırını aşan değer reddedilir. Bütün elemanlar doğrulanmadan sıralama veya sonuç yazımı başlamaz.

## Algoritma

1. Uzunluğu okuyup 1..50 aralığında doğrulayın; diziyi oluşturun.
2. Bütün elemanları ayrı satırlarda okuyup tam `int` aralığında doğrulayın.
3. Her `i` için kalan `i..n - 1` bölümünün en küçük değerinin dizinini bulun.
4. En küçük değer başka konumdaysa `a[i]` ile yerini değiştirin; ilk `n - 1` tur tamamlanınca dizi sıralıdır.
5. Uzunluk tekse medyanı `a[n / 2]` olarak alın.
6. Uzunluk çiftse `a[n / 2 - 1]` ile `a[n / 2]` değerlerini, ilk değeri toplamadan önce `long` yaparak toplayın; `2.0m` ile bölün.
7. Sıralı diziyi ve medyanı bir ondalık basamakla yazdırın.

Her sıralama turunun başında `i` konumunun solundaki bölüm doğru son sırasındadır. Kalan bölümün en küçük değeri `i` konumuna taşındığında bu bölüm bir eleman büyür. İç arama sonlu bölümde ilerler; dış döngü en fazla `n - 1` tur sürer. Sıralı dizi hazır olduğunda `n / 2` tamsayı bölümü tek uzunlukta ortayı, çift uzunlukta sağ orta dizini verir.

`7, 1, 4, 2` için sıralama adımları:

| Turun `i` dizini | En küçük değerin dizini | Tur sonrası dizi |
| --- | --- | --- |
| 0 | 1 | `1, 7, 4, 2` |
| 1 | 3 | `1, 2, 4, 7` |
| 2 | 2; takas gerekmez. | `1, 2, 4, 7` |

İki orta dizin 1 ve 2'dir; değerleri 2 ve 4 olduğundan medyan 3.0 olur.

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
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: Eleman int aralığında bir tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

for (int i = 0; i < n - 1; i++)
{
    int enKucuk = i;
    for (int j = i + 1; j < n; j++)
    {
        if (a[j] < a[enKucuk])
        {
            enKucuk = j;
        }
    }
    if (enKucuk != i)
    {
        int gecici = a[i];
        a[i] = a[enKucuk];
        a[enKucuk] = gecici;
    }
}

decimal medyan;
if (n % 2 == 1)
{
    medyan = a[n / 2];
}
else
{
    long toplam = (long)a[n / 2 - 1] + a[n / 2];
    medyan = toplam / 2.0m;
}

Console.Write("Sıralı dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Medyan: " +
    medyan.ToString("0.0", CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `(long)` dönüşümü toplamın ilk terimine uygulanır; böylece toplama `long` ile yapılır. Toplamı önce `int` olarak hesaplayıp daha sonra dönüştürmek taşmayı önlemez. `2.0m` bir `decimal` sabitidir; tamsayı bölmesinin yarım birimi kaybetmesini engeller. `"0.0"` biçimi tamsayı medyanlarda da `.0` yazdırır.

## Örnek çalıştırmalar

Önce uzunluğu, sonra bütün elemanları ayrı satırlarda girin. Metin blokları istemlerden sonraki bütün sonuç satırlarını gösterir.

`n = 4`, dizi `7, 1, 4, 2` için tam çıktı:

```text
Sıralı dizi: 1 2 4 7
Medyan: 3.0
```

`n = 3`, dizi `5, -1, 8` için tam çıktı:

```text
Sıralı dizi: -1 5 8
Medyan: 5.0
```

`n = 1`, dizi `-8` için tam çıktı:

```text
Sıralı dizi: -8
Medyan: -8.0
```

`n = 2`, dizi `-2147483648, 2147483647` için tam çıktı:

```text
Sıralı dizi: -2147483648 2147483647
Medyan: -0.5
```

`n = 2`, dizi `2147483647, 2147483647` için tam çıktı:

```text
Sıralı dizi: 2147483647 2147483647
Medyan: 2147483647.0
```

Aşağıdaki tablo uzun çıktıların tam eleman listesi değildir; denetlenecek özellikleri özetler.

| Girdi | Beklenen özellik |
| --- | --- |
| `n = 50`, bütün değerler 2147483647 | Sıralı satırda 50 kez aynı değer vardır; `Medyan: 2147483647.0` yazılır. Ara toplam 4294967294'tür. |
| `n = 50`, bütün değerler -2147483648 | Medyan satırı `Medyan: -2147483648.0` olur. Ara toplam -4294967296'dır ve `long` içinde kalır. |

Aşağıdaki örneklerde yalnızca ilgili tek hata satırı yazılır.

| Girdi | Tam sonuç |
| --- | --- |
| Uzunluk `0`, `51`, `iki`, `1.5` veya `2147483648` | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| Uzunluk için boş satır veya girişin sonu | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| `n = 2`, ilk eleman 7, ikinci eleman `sayı` veya `2.5` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `2147483648` veya `-2147483649` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman için boş satır veya girişin sonu | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |

## Sınır durumları

- Tek eleman hem sıralı dizinin hem medyanın tek değeridir; sıralama turu gerekmez.
- Tek uzunlukta orta dizin `n / 2` olur. Çift uzunluk en az 2 olduğundan `n / 2 - 1` negatif değildir.
- Tekrarlanan değerler medyan hesabında ayrı elemanlar olarak kalır; farklı değer sırası hesaplanmaz.
- İki orta değerin toplamı -4294967296..4294967294 aralığındadır. `long` bu aralığı kapsar.
- İki `int` ucunun toplamı -1'dir; medyan -0.5 olur. Tamsayı bölmesi bu sonucu yanlışlıkla 0 yapardı.
- Eşit iki en küçük veya en büyük `int` değeri de güvenle işlenir; medyan giriş değerlerinin aralığında kalır.
- En fazla 50 eleman sıralanır; sıralama hazır bir araç çağrısıyla yapılmaz.
- Eksik, boş, biçimi yanlış veya tür sınırını aşan girişte sıralı dizi ve medyan satırları üretilmez.

## Kazanımlar

- Seçmeli sıralamanın her turunda kalan bölümün en küçük değerini bulma.
- Tek ve çift uzunlukta medyan dizinlerini sıfır tabanlı olarak hesaplama.
- Medyan ile bütün değerlerin aritmetik ortalamasını örnek üzerinde ayırma.
- Tür dönüşümünün neden toplamadan önce yapılması gerektiğini uç değerlerle gösterme.
- Negatif yarım sonucu ve tamsayı sonucu aynı bir basamaklı biçimde yazdırma.

## Alıştırmalar

1. Medyanın yanında bütün elemanların aritmetik ortalamasını da hesaplayın; `1, 2, 100` için iki sonucun neden farklı olduğunu açıklayın.
2. Çift uzunlukta iki orta değeri ayrı satırlarda yazdırın; `-2147483648, 2147483647` için ara toplamı ve medyanı doğrulayın.
3. Aynı diziyi kabarcık sıralamayla sıralayıp medyanı hesaplayın; tekrar içeren, tek uzunluklu ve çift uzunluklu üç örnekte sonuçları karşılaştırın.
