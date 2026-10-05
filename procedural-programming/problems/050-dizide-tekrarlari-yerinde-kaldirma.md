---
id: "050"
order: 50
title: "Dizide tekrarları yerinde kaldırma"
level: "İleri"
prerequisites: ["042", "043", "049"]
concepts: ["Yerinde güncelleme", "Okuma ve yazma konumu", "Geçerli dizi bölümü", "Tekrar eleme", "İlk görülme sırası"]
---

# Dizide tekrarları yerinde kaldırma

## Problem tanımı

Bir tamsayı dizisindeki tekrarları, ek bir sonuç dizisi oluşturmadan kaldırın. Her farklı değeri ilk görülme sırasıyla dizinin başlangıcına yerleştirin ve kaç farklı değer kaldığını bulun. Örneğin `3, 1, 3, 2, 1` için geçerli sonuç bölümü `3, 1, 2` olur.

Buradaki kaldırma, dizinin fiziksel uzunluğunu değiştirmez. Aynı dizinin başlangıcındaki geçerli bölümü sıkıştırır; geride kalan konumlar sonuca dahil edilmez. Giriş tamamlandıktan sonra okuma konumuyla bütün eski elemanları dolaşın. Geçerli bölümde henüz bulunmayan değeri sıradaki yazma konumuna alın. Diziyi sıralamayın; değerlerin ilk görülme sırası korunmalıdır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; fiziksel dizi uzunluğu. |
| Girdi | `a` | Tam `int` aralığında `n` adet eleman. |
| Ara değer | `adet` | Geçerli farklı değer sayısı; aynı zamanda sonraki yazma dizini. |
| Çıktı | Farklı değerler | `Farklı değerler:` ardından başlangıçtaki geçerli bölümün değerleri. |
| Çıktı | Geçerli adet | `Adet: N` biçiminde farklı değer sayısı. |

`Eleman sayısı (1..50): ` isteminden sonra `A[0]: ` ile başlayan istemlerde bütün elemanları ayrı satırlarda girin. Her girdi doğrulanmadan sıkıştırma başlamaz. Hatalı uzunluk veya elemanda tek hata satırı yazılır; kısmi farklı değer listesi üretilmez.

## Algoritma

1. Uzunluğu doğrulayıp diziyi oluşturun ve bütün elemanları okuyarak doğrulayın.
2. Geçerli farklı değer adedini 0 yapın.
3. Okuma dizinini 0'dan `n - 1` dahil olacak şekilde ilerletin.
4. Okunan değeri bir tamsayı değişkeninde saklayın.
5. Bu değeri dizinin 0..`adet - 1` geçerli bölümünde arayın.
6. Zaten bulunuyorsa diziyi ve adedi değiştirmeden sıradaki eski elemana geçin.
7. Bulunmuyorsa `a[adet]` konumuna yazın ve adedi bir artırın.
8. Bütün okuma konumları bitince yalnızca 0..`adet - 1` bölümünü ve adedi yazdırın.

Her adımın başında dizinin geçerli başlangıç bölümü, daha önce okunmuş değerlerin farklı olanlarını ilk görülme sırasıyla içerir. Okuma dizini `okuma` iken en fazla `okuma` farklı değer vardır; dolayısıyla yazma dizini `adet <= okuma` olur. Bir değer yalnızca okunmuş bir konuma veya mevcut okuma konumuna yazılır; ileride okunacak konumlar bozulmaz. Yerinde yazmadan önce mevcut değerin saklanması, hesaplama adımında okunacak değerin korunmasını sağlar. Okuma sayacı sınırlı uzunluk boyunca arttığından işlem sonlanır.

`3, 1, 3, 2, 1` örneğinin geçerli bölümü:

| Okuma dizini | Saklanan değer | Daha önce var mı? | Yazma işlemi | Geçerli bölüm |
| --- | --- | --- | --- | --- |
| 0 | 3 | Hayır | `a[0] = 3` | 3 |
| 1 | 1 | Hayır | `a[1] = 1` | 3, 1 |
| 2 | 3 | Evet | Yok | 3, 1 |
| 3 | 2 | Hayır | `a[2] = 2` | 3, 1, 2 |
| 4 | 1 | Evet | Yok | 3, 1, 2 |

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

int adet = 0;
for (int okuma = 0; okuma < n; okuma++)
{
    int deger = a[okuma];
    bool bulundu = false;
    for (int j = 0; j < adet; j++)
    {
        if (a[j] == deger)
        {
            bulundu = true;
            break;
        }
    }
    if (!bulundu)
    {
        a[adet] = deger;
        adet++;
    }
}

Console.Write("Farklı değerler:");
for (int i = 0; i < adet; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Adet: " + adet.ToString(CultureInfo.InvariantCulture));
```

`a.Length` bütün işlem boyunca `n` olarak kalır. Yalnızca sonuçta kullanılan uzunluk `adet` olur. Örnekte işlem sonunda fiziksel içerik `3, 1, 2, 2, 1` olur; son iki konum geçerli bölüm dışındadır ve yazdırılmaz. Tekrar araması da yalnızca geçerli bölümde yapılır; işlenmemiş eski elemanlar daha önce bulunan değerler gibi değerlendirilmez.

## Örnek çalıştırmalar

Uzunluk ve dizi elemanları ayrı satırlarda girilir. Metin blokları istemler dışındaki bütün sonuç satırlarını gösterir.

`n = 5`, dizi `3, 1, 3, 2, 1` için tam çıktı:

```text
Farklı değerler: 3 1 2
Adet: 3
```

`n = 1`, dizi `-2147483648` için tam çıktı:

```text
Farklı değerler: -2147483648
Adet: 1
```

`n = 4`, dizi `8, 8, 8, 8` için tam çıktı:

```text
Farklı değerler: 8
Adet: 1
```

`n = 4`, dizi `-2147483648, 2147483647, -2147483648, 0` için tam çıktı:

```text
Farklı değerler: -2147483648 2147483647 0
Adet: 3
```

Aşağıdaki hatalarda yalnızca belirtilen tek satır yazılır.

| Girdi | Tam sonuç |
| --- | --- |
| Uzunluk `0`, `51` veya `iki` | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| `n = 2`, ilk eleman `7`, ikinci eleman `sayı` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `2147483648` veya `-2147483649` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `1.5` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, sonra boş satır veya girişin sonu | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |

## Sınır durumları

- Tek eleman için geçerli adet 1 olur ve değer aynı konuma yazılır.
- Bütün elemanlar farklıysa her yazma okuma konumuna denk gelir; geçerli adet `n` olur.
- Bütün elemanlar aynıysa ilk değer tutulur, diğerleri atlanır; geçerli adet 1 olur.
- 0, negatif değerler ve tam `int` uçları normal veridir; elemanlar üzerinde aritmetik yapılmaz.
- Fiziksel uzunluk küçülmez; geçerli bölüm dışındaki eski değerler çıktıya katılmaz.
- `adet <= okuma`, yazma işleminin henüz okunmamış bir elemanı bozmamasını garanti eder. Ek bir sonuç dizisi oluşturulmaz.
- Geçersiz girdi, boş satır veya EOF halinde sıkıştırma başlamaz ve kısmi sonuç yazılmaz.

## Kazanımlar

- Fiziksel dizi uzunluğu ile geçerli veri uzunluğunu ayrı yorumlama.
- Okuma ve yazma konumlarını farklı amaçlarla kullanma.
- Yazma konumunun okuma konumunu aşmadığını adım tablosuyla gösterme.
- Ek dizi oluşturmadan ilk görülme sırasıyla farklı değerleri sıkıştırma.
- Sonuç aramasını ve yazdırmayı yalnızca geçerli başlangıç bölümüyle sınırlama.

## Alıştırmalar

1. Sonuçtan sonra `n - adet` değerini kaldırılan tekrar sayısı olarak yazdırın; örnekte 2 bulunduğunu doğrulayın.
2. Geçerli bölüm dışındaki alanı 0 ile doldurun; bu işlemi bütün okumalar tamamlandıktan sonra yapmanın neden gerekli olduğunu açıklayın.
3. 049 numaralı sonuç dizisi yaklaşımıyla aynı dizinin farklı değerlerini ayrı bir dizide üretin; iki yöntemin çıktısını ve ek kapasite kullanımını karşılaştırın.
