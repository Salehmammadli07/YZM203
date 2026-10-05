---
id: "003"
order: 3
title: "Celsius ve Fahrenheit dönüşümü"
level: "Başlangıç"
prerequisites: ["001", "002"]
concepts: ["Formül uygulama", "Ondalıklı bölme", "Sabit", "Negatif değer", "Çıktı biçimlendirme"]
---

# Celsius ve Fahrenheit dönüşümü

## Problem tanımı

Celsius ölçeğinde girilen sıcaklığı Fahrenheit ölçeğine dönüştürün. Dönüşüm formülü `F = C * 9 / 5 + 32` şeklindedir. Örneğin 0 Celsius, 32 Fahrenheit; 100 Celsius, 212 Fahrenheit olur.

Bu alıştırma tek yönlü dönüşüm yapar: kullanıcı Celsius değerini girer. Hesaplama kapsamı -1000000 ile 1000000 arasındaki değerlerdir. Değer `decimal` türünde saklanır. Fahrenheit sonucu iki ondalık basamakla gösterilir. Buradaki aralık, uygulamanın kabul ettiği sayısal aralıktır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `celsius` | -1000000 ile 1000000 arasında ondalıklı sayı |
| Çıktı | `fahrenheit` | `celsius * 9 / 5 + 32` |

Ondalık ayırıcı nokta olmalıdır. Örneğin `36.5` geçerlidir. Hatalı biçimde veya aralık dışında girdi verilirse program hata mesajıyla sona erer.

## Algoritma

1. Celsius sıcaklığını isteyin ve okuyun.
2. Girdiyi ondalıklı sayıya dönüştürmeyi deneyin.
3. Dönüşüm başarısızsa veya değer kabul edilen aralıkta değilse hata yazıp bitirin.
4. Celsius değerini 9 ile çarpın ve 5'e bölün.
5. Elde edilen değere 32 ekleyin.
6. Fahrenheit sonucunu iki ondalık basamakla yazdırın.

Çarpma ve bölme, toplamadan önce yapılır. C# içinde `9 / 5` ifadesi iki tamsayının bölünmesidir ve sonuç 1 olur. Formülde `9m / 5m` kullanmak, ondalıklı hesaplama niyetini açıkça gösterir.

## C# çözümü

```csharp
using System;
using System.Globalization;

NumberStyles bicim = NumberStyles.AllowLeadingSign
                   | NumberStyles.AllowDecimalPoint;
CultureInfo kultur = CultureInfo.InvariantCulture;

Console.Write("Celsius sıcaklığı (ondalık için nokta): ");
if (!decimal.TryParse(Console.ReadLine(), bicim, kultur,
                      out decimal celsius)
    || celsius < -1000000m || celsius > 1000000m)
{
    Console.WriteLine("Hata: -1000000 ile 1000000 arasında sayı giriniz.");
    return;
}

decimal fahrenheit = celsius * 9m / 5m + 32m;
Console.WriteLine($"Fahrenheit: {fahrenheit.ToString("F2", kultur)}");
```

Bu kod tek başına bir konsol projesinin `Program.cs` dosyasında çalışır. `TryParse`, sayı biçimini denetler; karşılaştırmalar değer aralığını denetler. `||`, bu hata koşullarından herhangi birinin gerçekleşmesini ifade eder. `InvariantCulture`, girdi ve çıktıdaki ondalık ayracını bilgisayarın bölge ayarlarından bağımsız kılar.

## Örnek çalıştırmalar

Tablo, giriş isteminden sonra yazılan sonuç satırını gösterir.

| Celsius girdisi | Sonuç satırı |
| --- | --- |
| `0` | `Fahrenheit: 32.00` |
| `100` | `Fahrenheit: 212.00` |
| `-40` | `Fahrenheit: -40.00` |
| `36.5` | `Fahrenheit: 97.70` |
| `1000001` | `Hata: -1000000 ile 1000000 arasında sayı giriniz.` |
| `36,5` | `Hata: -1000000 ile 1000000 arasında sayı giriniz.` |

## Sınır durumları

- Sıfır ve negatif değerler geçerli girdilerdir.
- Alt ve üst sınırlar aralığa dahildir.
- Boş girdi ve sayı olmayan metinler reddedilir.
- Kabul edilen aralıktaki hesaplamalar `decimal` kapasitesini aşmaz.
- Çok fazla ondalık basamak içeren girdiler `decimal` hassasiyetine göre yuvarlanabilir; `F2` ayrıca yalnızca iki basamak gösterir.

## Kazanımlar

- Sözel bir dönüşüm kuralını aritmetik ifadeye çevirmek.
- Tamsayı bölmesiyle ondalıklı bölme arasındaki farkı görmek.
- Negatif girdileri ve bilinen referans değerlerini test etmek.
- Hesaplama hassasiyetiyle ekrandaki basamak sayısını ayırt etmek.

## Alıştırmalar

1. Ters dönüşümü uygulayın: `C = (F - 32) * 5 / 9`.
2. `-40` değerinin iki ölçekte de aynı olmasını formülden gösterin.
3. `decimal oran = 9 / 5;` ile `decimal oran = 9m / 5m;` ifadelerinin sonuçlarını karşılaştırın.
