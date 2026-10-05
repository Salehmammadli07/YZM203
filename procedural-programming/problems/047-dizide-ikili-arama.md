---
id: "047"
order: 47
title: "Dizide ikili arama ile ilk eşleşmeyi bulma"
level: "İleri"
prerequisites: ["042", "046"]
concepts: ["İkili arama", "Sıralılık önkoşulu", "Arama aralığı", "İlk eşleşme", "Tekrarlanan değerler"]
---

# Dizide ikili arama ile ilk eşleşmeyi bulma

## Problem tanımı

Azalmayan sırada verilmiş bir tamsayı dizisinde aranan değerin ilk bulunduğu dizini ikili aramayla belirleyin. Dizinler sıfırdan başlar. Aynı değer birden fazla kez bulunabilir; bu durumda herhangi bir eşleşme yerine en küçük dizin istenir. Örneğin `-5, 0, 0, 0, 8` içinde 0'ın ilk dizini 1'dir.

Önce bütün diziyi okuyun ve 046 numaralı problemdeki komşu karşılaştırmasıyla sıralılığını doğrulayın. Sırasız dizi için hata verin; diziyi kendiliğinden sıralamayın. Geçerli sıralı dizi için aranan değeri okuyun. İkili aramada ortadaki elemanı karşılaştırıp aday aralığının bir yarısını eleyin. Eşleşme bulunduğunda daha küçük bir dizin olabileceği için soldaki bölümde aramaya devam edin.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `a` | Tam `int` aralığında `n` eleman; azalmayan sırada olmalıdır. |
| Girdi | `aranan` | -2147483648..2147483647 aralığında `int`. |
| Çıktı | İlk dizin | Bulunursa `İlk dizin: I`; bulunamazsa `Sonuç: Bulunamadı`. |

Sırasıyla `Eleman sayısı (1..50): `, `A[0]: ` ile başlayan eleman istemleri ve `Aranan değer: ` yanıtlanır. Dizi sırasızsa `Hata: Dizi azalmayan sırada olmalıdır.` yazılır ve aranan değer istenmez. Girdi biçimi hatalarında ilgili hata satırı yazılır; arama sonucu verilmez.

## Algoritma

1. Uzunluğu ve bütün elemanları doğrulayarak diziyi oluşturun.
2. Dizinin komşularını karşılaştırın; bir azalma varsa sıra hatası yazıp bitirin.
3. Aranan tamsayıyı okuyup doğrulayın.
4. `sol = 0`, `sag = n - 1`, `bulunan = -1` ile başlayın.
5. `sol <= sag` olduğu sürece `orta = sol + (sag - sol) / 2` hesaplayın.
6. Orta değer aranan değere eşitse dizini kaydedip `sag = orta - 1` yapın.
7. Orta değer küçükse `sol = orta + 1`; büyükse `sag = orta - 1` yapın.
8. Aralık boşaldığında kayıtlı dizini veya bulunamadı mesajını yazdırın.

Sıralılık, orta değer küçükken sol bölümün de küçük olduğunu; büyükken sağ bölümün de büyük olduğunu garanti eder. Eşitlikte kaydedilen dizin bir çözümdür, ancak daha küçük eşleşmeler yalnızca solda bulunabilir. Her adımda orta konum aralıktan çıkarılır; kalan aralık küçülür ve sonunda boşalır. Orta hesabındaki fark önce alındığı için sol ve sağ sınırın doğrudan toplamına gerek yoktur.

`-5, 0, 0, 0, 8` dizisinde 0 araması:

| Sol | Sağ | Orta | Orta değer | İşlem |
| --- | --- | --- | --- | --- |
| 0 | 4 | 2 | 0 | Bulunan 2; sağ 1 olur. |
| 0 | 1 | 0 | -5 | Sol 1 olur. |
| 1 | 1 | 1 | 0 | Bulunan 1; sağ 0 olur. |

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

for (int i = 1; i < n; i++)
{
    if (a[i - 1] > a[i])
    {
        Console.WriteLine("Hata: Dizi azalmayan sırada olmalıdır.");
        return;
    }
}
Console.Write("Aranan değer: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int aranan))
{
    Console.WriteLine("Hata: Aranan değer int aralığında bir tamsayı olmalıdır.");
    return;
}

int sol = 0;
int sag = n - 1;
int bulunan = -1;
while (sol <= sag)
{
    int orta = sol + (sag - sol) / 2;
    if (a[orta] == aranan)
    {
        bulunan = orta;
        sag = orta - 1;
    }
    else if (a[orta] < aranan)
    {
        sol = orta + 1;
    }
    else
    {
        sag = orta - 1;
    }
}

if (bulunan == -1)
{
    Console.WriteLine("Sonuç: Bulunamadı");
}
else
{
    Console.WriteLine("İlk dizin: " +
        bulunan.ToString(CultureInfo.InvariantCulture));
}
```

Eşitlik dalında `break` kullanılmaz; soldaki eşleşmeler denenir. `bulunan` değişkeni bu arada önceki çözümü korur. Sol tarafta başka eşleşme çıkmazsa kayıtlı dizin kaybolmaz. Sıralılık denetimi bütün diziyi tarar; dolayısıyla bu tam program yalnızca ikili aramanın işlem sayısıyla değerlendirilmez.

## Örnek çalıştırmalar

Girdi sırası uzunluk, bütün dizi elemanları, ardından aranan değerdir. Metin blokları istemler dışındaki bütün sonuç satırlarını gösterir.

`n = 5`, dizi `-5, 0, 0, 0, 8`, aranan `0` için tam çıktı:

```text
İlk dizin: 1
```

`n = 4`, dizi `-4, 0, 3, 8`, aranan `4` için tam çıktı:

```text
Sonuç: Bulunamadı
```

`n = 1`, dizi `7`, aranan `7` için tam çıktı:

```text
İlk dizin: 0
```

`n = 2`, dizi `-2147483648, 2147483647`, aranan `2147483647` için tam çıktı:

```text
İlk dizin: 1
```

Hatalı örnekler yalnızca aşağıdaki tek satırı üretir. Sırasız örnekte aranan değer okunmaz.

| Girdi | Tam sonuç |
| --- | --- |
| Uzunluk `0` veya `51` | `Hata: Eleman sayısı 1 ile 50 arasında bir tamsayı olmalıdır.` |
| `n = 2`, dizi `3, 2` | `Hata: Dizi azalmayan sırada olmalıdır.` |
| `n = 1`, eleman `sayı`, `1.5` veya `2147483648` | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman için boş satır veya giriş sonu | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, dizi `7`, aranan `yedi` veya `7.5` | `Hata: Aranan değer int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, dizi `7`, aranan `-2147483649` | `Hata: Aranan değer int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, dizi `7`, sonra boş satır veya giriş sonu | `Hata: Aranan değer int aralığında bir tamsayı olmalıdır.` |

## Sınır durumları

- Tek elemanlı dizide ilk orta 0'dır; karşılaştırmadan sonra aralık boşalır.
- Bütün elemanlar aranan değere eşitse arama sola ilerleyerek 0 dizinini bulur.
- Aranan değer ilk veya son elemanda olabilir; sınır konumları dahildir.
- Aranan değer dizinin en küçük değerinden küçük veya en büyük değerinden büyükse aralık boşalır ve bulunamadı yazılır.
- `sag = -1` veya `sol = n` oluşması aramayı bitiren geçerli durumdur; koşul yanlış olduğundan bu dizinlerde elemana erişilmez.
- Tam `int` uçları doğrudan karşılaştırılır; eleman farkı veya hazır arama işlevi kullanılmaz.
- Girdi veya sıra hatasında sonuç verilmez. Aranan değer, sıralılık doğrulamasından sonra istenir.

## Kazanımlar

- İkili aramanın sıralılık önkoşulunu denetleme.
- Orta karşılaştırmasıyla aday aralığının hangi yarısının elenebileceğini açıklama.
- Tekrarlı değerlerde eşleşmeyi kaydedip daha küçük dizinleri arama.
- Aralık boşaldığında geçersiz dizin erişimi yapılmadığını gösterme.
- Sıralılık denetiminin işlem sayısıyla ikili arama adımlarının işlem sayısını ayırma.

## Alıştırmalar

1. Aynı koşullarda son eşleşmeyi bulmak için eşitlik dalındaki sınır güncellemesini değiştirin.
2. Arama sırasında denenen orta dizinleri ve karşılaştırma sayısını yazdırın; beş aynı elemanda ilk dizinin nasıl bulunduğunu izleyin.
3. 042 numaralı doğrusal aramayla aynı sıralı girdileri deneyin; iki aramanın bulduğu ilk dizinin aynı olduğunu karşılaştırın.
