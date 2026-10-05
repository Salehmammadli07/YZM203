---
id: "017"
order: 17
title: "Kademeli tüketim bedelini hesaplama"
level: "Orta"
prerequisites: ["012", "015", "016"]
concepts: ["Aralık sınırları", "Kademeli hesaplama", "Decimal", "Koşul zinciri", "Karar tablosu"]
---

# Kademeli tüketim bedelini hesaplama

## Problem tanımı

Kurgusal bir hizmetin tüketim bedelini üç kademeli bir tarifeye göre hesaplayın. İlk 100 birimin her biri 2.00 TL, sonraki 100 birimin her biri 3.00 TL, 200 birimin üzerindeki her birim 5.00 TL olarak ücretlendirilir. Bu öğretim örneği herhangi bir kurumun gerçek tarifesini temsil etmez; vergi, sabit ücret ve indirim içermez.

Kademeli tarifede her aralıktaki miktar kendi birim fiyatıyla çarpılır. Örneğin 201 birim tüketimde ilk 100 birim için 200.00 TL, ikinci 100 birim için 300.00 TL ve kalan bir birim için 5.00 TL ödenir. Toplam 505.00 TL olur; 201 birimin tamamına 5.00 TL uygulanmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `tuketim` | 0 ile 10000 arasında tüketim miktarı; `int` ve birimi bu örneğe özgü soyut tüketim birimidir. |
| Çıktı | `bedel` | `decimal` olarak hesaplanan toplam; `Bedel: 505.00 TL` biçiminde, iki ondalık basamakla. |

`Tüketim (0..10000 birim): ` isteminde tamsayıyı tek satıra girin. Çıktının ondalık ayırıcısı noktadır. Geçersiz biçim veya aralık için `Hata: Tüketim 0 ile 10000 arasında bir tamsayı olmalıdır.` yazılır ve bedel hesaplanmaz.

## Algoritma

1. Tüketimi okuyun; 0 ile 10000 arasında bir tamsayı olduğunu doğrulayın.
2. Tüketim en fazla 100 ise bedeli `tuketim * 2.00` olarak hesaplayın.
3. Tüketim 100'den büyük ve en fazla 200 ise ilk kademenin bedeline, `tuketim - 100` birimin 3.00 TL üzerinden bedelini ekleyin.
4. Tüketim 200'den büyükse ilk iki kademenin toplam bedeline, `tuketim - 200` birimin 5.00 TL üzerinden bedelini ekleyin.
5. Bedeli iki ondalık basamakla ve TL birimiyle yazdırın.

Karar tablosu sınırların hangi dala ait olduğunu açıkça gösterir:

| Tüketim aralığı | İlk kademe | İkinci kademe | Üçüncü kademe |
| --- | --- | --- | --- |
| `0 <= tuketim <= 100` | `tuketim * 2.00` | Yok | Yok |
| `100 < tuketim <= 200` | `100 * 2.00` | `(tuketim - 100) * 3.00` | Yok |
| `200 < tuketim <= 10000` | `100 * 2.00` | `100 * 3.00` | `(tuketim - 200) * 5.00` |

Her satırdaki bedeller toplanır. İlk dal başarısız olduğunda tüketimin 100'den büyük olduğu zaten bilinir; ikinci dalda yalnızca `tuketim <= 200` kontrolü yeterlidir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Tüketim (0..10000 birim): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int tuketim) || tuketim < 0 || tuketim > 10000)
{
    Console.WriteLine("Hata: Tüketim 0 ile 10000 arasında bir tamsayı olmalıdır.");
    return;
}

decimal bedel;
if (tuketim <= 100)
{
    bedel = tuketim * 2.00m;
}
else if (tuketim <= 200)
{
    bedel = 100 * 2.00m + (tuketim - 100) * 3.00m;
}
else
{
    bedel = 100 * 2.00m + 100 * 3.00m + (tuketim - 200) * 5.00m;
}

Console.WriteLine("Bedel: " + bedel.ToString("F2", CultureInfo.InvariantCulture) + " TL");
```

Ondalık sabitlerin sonundaki `m`, değerin `decimal` türünde olduğunu belirtir. Bu tür ondalık parasal hesaplamalar için uygundur. `F2`, sonuçtaki iki ondalık basamağın her çalıştırmada görünmesini sağlar.

## Örnek çalıştırmalar

Tablo giriş isteminden sonra yazılan sonuç satırını gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `0` | `Bedel: 0.00 TL` | Hiç tüketim yoktur. |
| `100` | `Bedel: 200.00 TL` | İlk kademenin son birimi. |
| `101` | `Bedel: 203.00 TL` | Yalnızca bir birim 3.00 TL ile ücretlenir. |
| `150` | `Bedel: 350.00 TL` | İlk kademe 200.00 TL, kalan 50 birim 150.00 TL. |
| `200` | `Bedel: 500.00 TL` | İkinci kademenin son birimi. |
| `201` | `Bedel: 505.00 TL` | Yalnızca bir birim 5.00 TL ile ücretlenir. |
| `10000` | `Bedel: 49500.00 TL` | İlk iki kademe 500.00 TL, kalan 9800 birim 49000.00 TL. |
| `-1` | `Hata: Tüketim 0 ile 10000 arasında bir tamsayı olmalıdır.` | Negatif tüketim reddedilir. |
| `10001` | `Hata: Tüketim 0 ile 10000 arasında bir tamsayı olmalıdır.` | Üst sınır aşılır. |
| `1.5` | `Hata: Tüketim 0 ile 10000 arasında bir tamsayı olmalıdır.` | Bu model yalnızca tamsayı tüketim kabul eder. |

## Sınır durumları

- 100 ve 200 değerleri önceki kademelerin son birimleridir; sonraki fiyatlar 101 ve 201 ile başlar.
- Sıfır tüketimde sabit ücret bulunmadığından bedel sıfırdır.
- Tüketim arttığında önceki kademelerde hesaplanmış ücretler korunur.
- En yüksek bedel 49500.00 TL'dir ve `decimal` türüne rahatça sığar.
- Boş satır, metin ve tamsayı olmayan girdiler için hesaplama yapılmaz.

## Kazanımlar

- Kademeli bir kuralı birbirini dışlayan aralıklara ayırma.
- Eşik değeri ve hemen sonraki değeri ayrı örneklerle sınama.
- Her kademede yalnızca o aralığa düşen miktarı ücretlendirme.
- `decimal` sabitleri ve `F2` biçimini kullanarak parasal sonuç yazdırma.

## Alıştırmalar

1. 99, 100 ve 101 birim için bedelleri hesaplayıp ardışık artışların neden değiştiğini açıklayın.
2. Üç kademenin bedelini ayrı değişkenlerde tutup toplamın yanında ayrı satırlarda gösterin.
3. Üst sınırı koruyarak 500 birimden sonraki miktara 7.00 TL uygulayan dördüncü kademe ekleyin.
