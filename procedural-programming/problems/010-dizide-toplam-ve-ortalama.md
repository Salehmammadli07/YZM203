---
id: "010"
order: 10
title: "Bir dizinin toplamını ve ortalamasını bulma"
level: "Temel"
prerequisites: ["008", "009"]
concepts: ["Dizi", "Dizin", "For döngüsü", "Toplam biriktirme", "Tür dönüşümü"]
---

# Bir dizinin toplamını ve ortalamasını bulma

## Problem tanımı

Kullanıcının belirlediği sayıda tamsayıyı bir dizide saklayın. Ardından dizi elemanlarını dolaşarak toplamlarını ve aritmetik ortalamalarını hesaplayın. Aritmetik ortalama, bütün elemanların toplamının eleman sayısına bölünmesiyle bulunur.

Örneğin `2`, `5`, `8` değerlerinin toplamı 15, ortalaması 5'tir. Dizi, aynı türde birden fazla değeri tek bir değişken adı altında saklar. Her elemana sıfırdan başlayan bir dizinle erişilir. Üç elemanlı bir dizinin geçerli dizinleri `0`, `1` ve `2` olur.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `adet` | 1 ile 100 arasında eleman sayısı. |
| Girdi | `sayilar` | Her biri -1000000 ile 1000000 arasında `adet` tamsayı. |
| Çıktı | `toplam` | Elemanların toplamı; `long` türünde. |
| Çıktı | `ortalama` | `double` türündeki ortalama; iki ondalık basamakla gösterilir. |

Önce `Eleman sayısı (1..100): ` istemine cevap verin. Sonra `1. eleman: ` ile başlayan istemlerde her tamsayıyı ayrı satıra yazın. Basamak ayırıcı kullanmayın. Ortalama çıktısında ondalık ayırıcı noktadır; örneğin `1.50`.

## Algoritma

1. Eleman sayısını okuyun ve 1 ile 100 aralığında olduğunu doğrulayın.
2. Bu uzunlukta bir tamsayı dizisi oluşturun.
3. Dizinleri sıfırdan başlayarak dolaşın; her girdiyi doğrulayıp ilgili elemana kaydedin.
4. Geçersiz girişte hata mesajı gösterip programı bitirin.
5. `toplam` değişkenini sıfırla başlatın.
6. Diziyi ikinci bir döngüyle dolaşarak her elemanı toplama ekleyin.
7. Toplamı `double` türüne dönüştürüp eleman sayısına bölün.
8. Toplamı ve iki ondalık basamakla ortalamayı yazdırın.

İlk döngü verileri saklar, ikinci döngü saklanmış veriler üzerinde hesaplama yapar. Bu ayrım, aynı diziyi daha sonra farklı işlemler için kullanmayı kolaylaştırır.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Eleman sayısı (1..100): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int adet) || adet < 1 || adet > 100)
{
    Console.WriteLine("Hata: 1 ile 100 arasında bir tamsayı girin.");
    return;
}

int[] sayilar = new int[adet];
for (int i = 0; i < sayilar.Length; i++)
{
    Console.Write((i + 1).ToString(CultureInfo.InvariantCulture) + ". eleman: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int sayi) ||
        sayi < -1000000 || sayi > 1000000)
    {
        Console.WriteLine("Hata: -1000000 ile 1000000 arasında bir tamsayı girin.");
        return;
    }
    sayilar[i] = sayi;
}

long toplam = 0;
for (int i = 0; i < sayilar.Length; i++)
{
    toplam += sayilar[i];
}

double ortalama = (double)toplam / sayilar.Length;
Console.WriteLine("Toplam: " + toplam.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Ortalama: " + ortalama.ToString("F2", CultureInfo.InvariantCulture));
```

Tür dönüşümü bölme işleminden önce yapılır. Böylece `3 / 2` tamsayı bölmesi yerine `3.0 / 2` işlemi gerçekleşir ve kesirli sonuç korunur. `F2` yalnızca gösterimi iki ondalık basamağa yuvarlar.

## Örnek çalıştırmalar

Elemanlar giriş sırasındadır; sonuç sütunu iki çıktı satırını gösterir.

| Eleman sayısı | Elemanlar | Sonuç |
| --- | --- | --- |
| `3` | `2`, `5`, `8` | `Toplam: 15`<br>`Ortalama: 5.00` |
| `2` | `1`, `2` | `Toplam: 3`<br>`Ortalama: 1.50` |
| `3` | `-4`, `0`, `4` | `Toplam: 0`<br>`Ortalama: 0.00` |
| `1` | `-7` | `Toplam: -7`<br>`Ortalama: -7.00` |
| `100` | Her eleman `1000000` | `Toplam: 100000000`<br>`Ortalama: 1000000.00` |

## Sınır durumları

- Eleman sayısı sıfır olamaz; sıfıra bölme engellenir.
- `i < sayilar.Length`, son geçerli dizinin aşılmasını önler.
- Negatif değerler ve sıfır normal biçimde toplama katılır.
- Toplam -100000000 ile 100000000 arasındadır ve `long` türüne sığar.
- Geçersiz eleman girişinde kısmi dizi için sonuç yazılmaz.

## Kazanımlar

- Diziyi oluşturma, doldurma ve dizinle okuma.
- Dizi uzunluğuna bağlı bir döngü kurma.
- Bölme öncesi tür dönüşümünün etkisini açıklama.
- Hesaplanan değerle ekranda gösterilen biçimi ayırma.

## Alıştırmalar

1. Pozitif ve negatif elemanların sayılarını bulun.
2. Dizideki en büyük değeri 007 numaralı problemdeki yaklaşımla belirleyin.
3. Ortalamanın üzerinde olan elemanları yazdırın.
