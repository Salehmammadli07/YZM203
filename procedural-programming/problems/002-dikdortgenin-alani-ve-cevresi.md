---
id: "002"
order: 2
title: "Dikdörtgenin alanı ve çevresi"
level: "Başlangıç"
prerequisites: ["001"]
concepts: ["Ondalıklı sayı", "Çarpma", "İşlem önceliği", "Girdi doğrulama", "Çıktı biçimlendirme"]
---

# Dikdörtgenin alanı ve çevresi

## Problem tanımı

Kısa ve uzun kenarı verilen bir dikdörtgenin alanını ve çevresini hesaplayın. İki kenar aynı ölçü biriminde girilmelidir. Kenarlar metre ise alan metrekare, çevre metre olarak yorumlanır.

Alan iki kenarın çarpımıdır. Çevre ise kenarların toplamının iki katıdır. Bu alıştırmada kenarların her biri sıfırdan büyük ve en fazla 1000000000 olabilir. Ondalıklı kenarlar `decimal` türünde saklanır. Sonuçlar iki ondalık basamakla gösterilir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `birinciKenar` | `0 < kenar <= 1000000000` |
| Girdi | `ikinciKenar` | `0 < kenar <= 1000000000` |
| Çıktı | `alan` | `birinciKenar * ikinciKenar` |
| Çıktı | `cevre` | `2 * (birinciKenar + ikinciKenar)` |

Ondalık ayırıcı olarak nokta kullanın: `3.5`. Program virgüllü girdiyi kabul etmez. Geçersiz biçim veya aralık dışındaki kenar için hata yazılır ve program biter.

## Algoritma

1. Birinci kenarı okuyun ve ondalıklı sayıya dönüştürün.
2. Dönüşüm başarısızsa veya kenar izin verilen aralıkta değilse hata yazıp bitirin.
3. İkinci kenarı okuyun ve aynı denetimi uygulayın.
4. İki kenarı çarparak alanı hesaplayın.
5. Kenarları toplayıp sonucu ikiyle çarparak çevreyi hesaplayın.
6. Alanı ve çevreyi iki ondalık basamakla yazdırın.

Çevre formülündeki parantez, önce kenarların toplanacağını belirtir. `2 * birinciKenar + ikinciKenar` farklı bir sonuç verir.

## C# çözümü

```csharp
using System;
using System.Globalization;

NumberStyles bicim = NumberStyles.AllowLeadingSign
                   | NumberStyles.AllowDecimalPoint;
CultureInfo kultur = CultureInfo.InvariantCulture;

Console.Write("Birinci kenar (ondalık için nokta): ");
if (!decimal.TryParse(Console.ReadLine(), bicim, kultur,
                      out decimal birinciKenar)
    || birinciKenar <= 0 || birinciKenar > 1000000000m)
{
    Console.WriteLine("Hata: Kenar 0'dan büyük ve en fazla 1000000000 olmalıdır.");
    return;
}

Console.Write("İkinci kenar (ondalık için nokta): ");
if (!decimal.TryParse(Console.ReadLine(), bicim, kultur,
                      out decimal ikinciKenar)
    || ikinciKenar <= 0 || ikinciKenar > 1000000000m)
{
    Console.WriteLine("Hata: Kenar 0'dan büyük ve en fazla 1000000000 olmalıdır.");
    return;
}

decimal alan = birinciKenar * ikinciKenar;
decimal cevre = 2m * (birinciKenar + ikinciKenar);

Console.WriteLine($"Alan: {alan.ToString("F2", kultur)}");
Console.WriteLine($"Çevre: {cevre.ToString("F2", kultur)}");
```

Kodu bir konsol projesinin `Program.cs` dosyasında çalıştırın. `m` eki, sabitin `decimal` olduğunu belirtir. `InvariantCulture`, bilgisayarın bölge ayarından bağımsız olarak noktayı kullanır. `F2`, sonucu iki ondalık basamakla gösterir; hesaplanan değişkenleri değiştirmez.

## Örnek çalıştırmalar

Tablo giriş istemlerini içermez; sonuç satırları sırayla gösterilir.

| Girdi (ayrı satırlarda) | Sonuç satırları |
| --- | --- |
| `5`, `3` | `Alan: 15.00`<br>`Çevre: 16.00` |
| `2.5`, `4` | `Alan: 10.00`<br>`Çevre: 13.00` |
| `6`, `6` | `Alan: 36.00`<br>`Çevre: 24.00` |
| `0` | `Hata: Kenar 0'dan büyük ve en fazla 1000000000 olmalıdır.` |

## Sınır durumları

- Eşit kenarlar geçerlidir; kare de dikdörtgenin özel bir durumudur.
- Negatif kenar, sıfır, boş girdi ve `3,5` reddedilir.
- İzin verilen en büyük kenarlarda alan 1000000000000000000 olur; `decimal` aralığı aşılmaz.
- Çok küçük alanlar iki basamaklı gösterimde `0.00` görünebilir. Bu, kenarın sıfır olduğu anlamına gelmez.

## Kazanımlar

- Bir problemden matematiksel formül çıkarmak.
- Parantez ve işlem önceliğini doğru kullanmak.
- Ondalıklı girdi, ölçü birimi ve çıktı biçimini ilişkilendirmek.
- Bir girdinin hem biçimini hem değer aralığını doğrulamak.

## Alıştırmalar

1. Sonuçları üç ondalık basamakla gösterin.
2. Kenarlar eşitse ekrana `Bu dikdörtgen aynı zamanda karedir.` yazdırın.
3. Çevre formülündeki parantezi kaldırınca hangi test örneğinin hatayı ortaya çıkaracağını açıklayın.
