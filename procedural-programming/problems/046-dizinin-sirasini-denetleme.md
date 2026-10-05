---
id: "046"
order: 46
title: "Dizinin azalmayan sırada olduğunu denetleme"
level: "İleri"
prerequisites: ["042", "043", "045"]
concepts: ["Komşu karşılaştırma", "Azalmayan sıra", "İlk ihlal", "Erken çıkış", "Dizin sınırı"]
---

# Dizinin azalmayan sırada olduğunu denetleme

## Problem tanımı

Bir tamsayı dizisinin azalmayan sırada olup olmadığını belirleyin. Azalmayan sıra, her elemanın kendinden önceki elemandan küçük olmaması demektir; eşit değerler kabul edilir. Örneğin `-7, 0, 0, 9` sıralıdır. `2, 2, -1, 7` dizisinde ise -1, önceki 2'den küçüktür; ilk ihlal -1'in bulunduğu 2 numaralı dizindedir.

Diziyi sıralamayın veya değiştirmeyin. Komşu çiftleri soldan sağa karşılaştırıp ilk azalmada durun. İhlal dizini, karşılaştırılan çiftin sağındaki elemanın sıfırdan başlayan dizinidir. Bu denetim, sonraki problemde ikili aramanın önkoşulunu doğrulayacaktır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `a` | Her biri -2147483648..2147483647 aralığında `n` adet `int`. |
| Çıktı | Sıra durumu | `Sonuç: Azalmayan sırada` veya `Sonuç: Azalmayan sırada değil`. |
| Çıktı | `ihlal` | Sıra bozuksa ikinci satır `İlk ihlal dizini: I`; çiftin sağ dizini. |

Önce `Eleman sayısı (1..50): ` istemini yanıtlayın, sonra `A[0]: ` ile başlayan istemlerde bütün elemanları ayrı satırlarda girin. Dizinler 0'dan başlar. Geçersiz uzunluk veya eleman girişinde yalnızca ilgili hata yazılır; önceki elemanlar için kısmi sıra sonucu verilmez.

## Algoritma

1. Uzunluğu doğrulayıp bu uzunlukta bir dizi oluşturun.
2. Bütün elemanları sırayla okuyup doğrulayarak diziye kaydedin.
3. İlk ihlal dizinini -1 ile başlatın; bu değer henüz ihlal olmadığını belirtir.
4. Dizini 1'den `n - 1` dahil olacak şekilde ilerletin.
5. `a[i - 1] > a[i]` ise ihlal dizinini `i` yapıp döngüden çıkın.
6. İhlal -1 kaldıysa sıralı mesajı; aksi halde sırasız mesajı ve ihlal dizinini yazdırın.

`2, 2, -1, 7` dizisi için izleme:

| Sağ dizin i | Sol değer | Sağ değer | `Sol > sağ` | İşlem |
| --- | --- | --- | --- | --- |
| 1 | 2 | 2 | Yanlış | Eşitlik geçerli; devam. |
| 2 | 2 | -1 | Doğru | İhlal 2; döngü biter. |

Döngü başında daha önce denetlenen bütün komşu çiftlerin azalmadığı bilinir. Soldan sağa ilerlemek, bulunan ilk ihlalin en küçük sağ dizine sahip olmasını sağlar. Bir ihlal bütün dizinin sıralı olmadığını kesinleştirdiği için sonraki çiftler gereksizdir. Döngü ihlalde veya sınırlı sayıdaki komşu çiftlerin sonunda biter. Karşılaştırma çıkarma kullanmadığından `int` uç değerlerinde fark taşması oluşmaz.

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

int ihlal = -1;
for (int i = 1; i < n; i++)
{
    if (a[i - 1] > a[i])
    {
        ihlal = i;
        break;
    }
}

if (ihlal == -1)
{
    Console.WriteLine("Sonuç: Azalmayan sırada");
}
else
{
    Console.WriteLine("Sonuç: Azalmayan sırada değil");
    Console.WriteLine("İlk ihlal dizini: " +
        ihlal.ToString(CultureInfo.InvariantCulture));
}
```

Sayaç 1'den başladığı için `i - 1` hiçbir zaman negatif olmaz. `i < n`, sağ elemanın da geçerli dizinde kalmasını sağlar. `>=` karşılaştırması kullanmak eşit komşuları yanlışlıkla ihlal sayardı.

## Örnek çalıştırmalar

Uzunluk ve elemanlar belirtilen sırayla ayrı satırlarda girilir. Metin blokları istemlerden sonraki bütün sonuç satırlarını gösterir.

`n = 4`, dizi `-7, 0, 0, 9` için tam çıktı:

```text
Sonuç: Azalmayan sırada
```

`n = 4`, dizi `2, 2, -1, 7` için tam çıktı:

```text
Sonuç: Azalmayan sırada değil
İlk ihlal dizini: 2
```

`n = 1`, dizi `-2147483648` için tam çıktı:

```text
Sonuç: Azalmayan sırada
```

`n = 2`, dizi `2147483647, -2147483648` için tam çıktı:

```text
Sonuç: Azalmayan sırada değil
İlk ihlal dizini: 1
```

Aşağıdaki örneklerde yalnızca ilgili tek hata satırı yazılır.

| Girdi | Tam sonuç |
| --- | --- |
| Uzunluk `0` veya `51` | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| Uzunluk `iki` veya `2.5` | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| `n = 2`, ilk eleman `7`, ikinci eleman `sayı` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `2147483648` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `-2147483649` veya `1.5` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, sonra boş satır veya girişin sonu | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |

## Sınır durumları

- Tek elemanın karşılaştırılacak komşusu yoktur; dizi azalmayan sıradadır.
- Bütün değerlerin eşit olması geçerlidir; azalmayan sıra, kesin artış gerektirmez.
- İlk ihlal en erken 1, en geç `n - 1` dizinindedir; 0 sağ dizin olamaz.
- İhlalden sonra gelen bir artış, daha önceki ihlali düzeltmez.
- `int` alt ve üst uçları doğrudan karşılaştırılır; elemanları birbirinden çıkarma yapılmaz.
- Bütün dizi doğrulanmadan denetim ve sonuç yazdırma başlamaz. Boş satır, EOF, metin, kesirli sayı ve `int` taşması eleman hatasıdır.

## Kazanımlar

- Azalmayan sıra ile kesin artan sıra arasındaki eşitlik farkını açıklama.
- Komşu çiftlerin sağ dizininden sol dizine güvenli erişme.
- Soldan sağa taramayla ilk ihlali bulup erken çıkma.
- Veri doğrulamasıyla sıra denetimini ayrı adımlarda yürütme.
- Karşılaştırmayı çıkarma yerine doğrudan yaparak uç değerleri güvenli işleme.

## Alıştırmalar

1. Dizinleri biriktirmeden bütün ihlal çiftlerini sırayla yazdırın ve sayın; ilk ihlalde çıkışı kaldırın.
2. Kesin artan sırayı denetleyen sürümü yazın; `2, 2, 3` dizisinin iki tanımda farklı sonuç verdiğini gösterin.
3. Azalmayan yerine artmayan sırayı denetleyin; ilk ihlal için hangi karşılaştırmanın değişeceğini açıklayın.
