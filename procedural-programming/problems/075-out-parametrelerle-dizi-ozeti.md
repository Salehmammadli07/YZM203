---
id: "075"
order: 75
title: "out parametrelerle tek geçişte dizi özeti"
level: "İleri"
prerequisites: ["041", "057", "073", "074"]
concepts: ["Birden çok sonuç", "out sözleşmesi", "Boş veri", "Okuma amacıyla dizi parametresi", "Güvenli türetilmiş ölçüler"]
---

# out parametrelerle tek geçişte dizi özeti

## Problem tanımı

0..30 elemanlı bir tamsayı dizisini değiştirmeden en küçük değeri, en büyük değeri ve toplamı tek geçişte hesaplayın. Bu üç değeri `Ozetle` metodunun `out` parametreleriyle döndürün. Metot, dizi boşsa `false`, doluysa `true` döndürür. Dolu dizide ana program ayrıca ortalamayı ve değer aralığının genişliğini hesaplar.

Boş dizi için minimum veya maksimum 0 kabul edilmez: bu ölçüler tanımsızdır. Başarısız dönüşte `out` değerler 0'a atanır ancak sonuç olarak kullanılmaz. Sıralama, hazır minimum/maksimum/toplam metotları veya yeni veri sınıfı kullanmayın. Amaç, başarının ayrı bildirildiği çok sonuçlu bir prosedürel sözleşme kurmaktır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, elemanlar | 0..30 uzunluk; tam `int` aralığında ayrı satırlar. |
| Metot çıktısı | `min`, `max`, `toplam` | Dolu dizide geçerli iki `int` ve bir `long`. |
| Sonuç | Beş ölçü | Minimum, maksimum, toplam, iki ondalıklı ortalama ve genişlik. |
| Boş sonuç | Durum | Tek satır `Sonuç: Dizi boş`. |

İstemler `Eleman sayısı: `, sonra `A[0]: ` ile başlayan istemlerdir. İlk hatalı alanda yalnız `Hata: Geçersiz giriş.` yazılır. EOF, boş satır, ondalıklı değer veya aralık ihlali geçersizdir. Sayısal giriş çevre boşluklarını ve işareti kabul eder; bütün değerler alınmadan özet üretilmez.

## Algoritma

1. Uzunluğu ve bütün elemanları doğrulayın.
2. `Ozetle` girişinde bütün `out` parametrelerine 0 atayın; boş dizide `false` dönün.
3. Minimum ve maksimumu ilk elemanla başlatın.
4. Her elemanda iki sınırı güncelleyip `long` toplamı artırın.
5. `true` dönüşte ortalamayı `decimal` bölmeyle, genişliği `long` farkla hesaplayın.
6. Beş ölçüyü yazdırın; `false` dönüşte yalnız boş durumunu yazdırın.

İlk elemanla başlatmak bütün değerlerin negatif veya pozitif olduğu dizilerde doğrudur. Toplamın `long` olması en fazla 30 tam `int` değerinin toplamını taşmadan tutar. Genişlik hesaplanırken çıkarmadan önce tür genişletilir. Metot sadece okur; dizi parametresinin referans olması elemanları değiştirme zorunluluğu doğurmaz.

Minimum veya maksimumun 0 olması başarı hakkında bilgi vermez; yalnız metodun `bool` dönüşü kontrol edilir. Dolu dizide bütün çıktı parametreleri aynı taramanın sonuçlarıdır. Ortalama ve genişlik, bu sonuçlardan çağıran tarafından türetilir; hesaplama metodu yazdırma biçimini bilmez.

`4, -2, 4` için kısmi sonuçlar:

| İşlenen değer | Minimum | Maksimum | Toplam |
| --- | --- | --- | --- |
| 4 | 4 | 4 | 4 |
| -2 | -2 | 4 | 2 |
| 4 | -2 | 4 | 6 |

## C# çözümü

```csharp
using System;
using System.Globalization;

if (!Oku("Eleman sayısı: ", out int n) || n < 0 || n > 30)
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}
int[] a = new int[n];
for (int i = 0; i < n; i++)
{
    string istem = "A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ";
    if (!Oku(istem, out a[i]))
    {
        Console.WriteLine("Hata: Geçersiz giriş.");
        return;
    }
}

if (!Ozetle(a, out int min, out int max, out long toplam))
{
    Console.WriteLine("Sonuç: Dizi boş");
    return;
}
decimal ortalama = (decimal)toplam / n;
long genislik = (long)max - min;
Console.WriteLine("Minimum: " + min.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Maksimum: " + max.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Toplam: " + toplam.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Ortalama: " +
    ortalama.ToString("0.00", CultureInfo.InvariantCulture));
Console.WriteLine("Genişlik: " + genislik.ToString(CultureInfo.InvariantCulture));

static bool Oku(string istem, out int deger)
{
    Console.Write(istem);
    return int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out deger);
}

static bool Ozetle(int[] a, out int min, out int max, out long toplam)
{
    min = 0;
    max = 0;
    toplam = 0;
    if (a.Length == 0)
    {
        return false;
    }

    min = a[0];
    max = a[0];
    for (int i = 0; i < a.Length; i++)
    {
        if (a[i] < min)
        {
            min = a[i];
        }
        if (a[i] > max)
        {
            max = a[i];
        }
        toplam += a[i];
    }
    return true;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Diziye yeni değer atanmaz; bütün sonuçlar çıktı parametreleri üzerinden ana programa aktarılır.

## Örnek çalıştırmalar

Önce `n`, sonra elemanları ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`n = 3`, dizi `4, -2, 4`:

```text
Minimum: -2
Maksimum: 4
Toplam: 6
Ortalama: 2.00
Genişlik: 6
```

`n = 2`, dizi `-2147483648, 2147483647`:

```text
Minimum: -2147483648
Maksimum: 2147483647
Toplam: -1
Ortalama: -0.50
Genişlik: 4294967295
```

`n = 1`, dizi `0`:

```text
Minimum: 0
Maksimum: 0
Toplam: 0
Ortalama: 0.00
Genişlik: 0
```

`n = 0`:

```text
Sonuç: Dizi boş
```

## Sınır durumları

- Boş dizide ilk elemana erişilmez; ortalama hesabı da yapılmaz.
- Tek elemanda minimum ve maksimum eşittir; genişlik 0'dır.
- Bütün değerler negatifken minimum/maksimum ilk elemandan belirlenir.
- `int` uçları arasındaki fark 4294967295'tir ve `long` gerektirir.
- Otuz `int.MaxValue` toplamı 64424509410'dur; `int` yeterli değildir.
- Başarısız giriş ile geçerli boş dizi farklı sonuçlar üretir.

## Kazanımlar

- Birden fazla hesaplama sonucunu `out` parametreleriyle aktarma.
- Boş veri durumunu sahte sayısal sonuçlarla temsil etmemeyi öğrenme.
- Dizi parametresini değiştirmeden ortak tarama yapma.
- Genişletilmiş toplamdan güvenli ortalama ve fark türetme.
- Sonuç geçerliliği ile çıktı biçimini farklı sorumluluklarda tutma.

## Alıştırmalar

1. En küçük değerin kaç kez geçtiğini yeni `out` parametresiyle döndürün.
2. İlk minimum ve maksimum dizinlerini de aynı geçişte bulun.
3. Girdi dizisinin bütün elemanlarının metot çağrısından sonra korunduğunu denetleyin.
