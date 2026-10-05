---
id: "001"
order: 1
title: "İki sayının toplamı"
level: "Başlangıç"
prerequisites: []
concepts: ["Girdi ve çıktı", "Değişken", "Tamsayı", "Aritmetik işlem", "Tür dönüşümü"]
---

# İki sayının toplamı

## Problem tanımı

Kullanıcının girdiği iki tamsayının toplamını hesaplayıp ekrana yazdırın. Örneğin 12 ve 8 için sonuç 20 olmalıdır. Amaç, bir programın **girdi → işlem → çıktı** akışını görmektir. Kullanıcıdan gelen metin önce sayıya dönüştürülür; ardından toplama işlemi yapılır.

Her girdi, `int` türünün -2147483648 ile 2147483647 arasındaki değerlerinden biri olmalıdır. İki geçerli `int` değerinin toplamı bu aralığı aşabileceği için sonuç `long` değişkende tutulur.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `birinci` | Birinci tamsayı |
| Girdi | `ikinci` | İkinci tamsayı |
| Çıktı | `toplam` | İki sayının toplamı |

Girdiler ayrı satırlarda verilir. Sayılar kültürden bağımsız tamsayı biçimiyle okunur ve yazılır; basamak ayırıcı kullanmayın. Geçersiz bir girdi için program `Hata: Tamsayı giriniz.` yazar ve sona erer.

## Algoritma

1. Birinci sayıyı isteyin ve okuyun.
2. Girdi geçerli bir `int` değilse hata yazıp bitirin.
3. İkinci sayıyı isteyin ve okuyun.
4. Girdi geçerli bir `int` değilse hata yazıp bitirin.
5. Birinci sayıyı `long` türüne dönüştürüp ikinci sayıyla toplayın.
6. Toplamı ekrana yazdırın.

Tür dönüşümü işlemden **önce** yapılır. Yalnızca sonucu `long` değişkene atamak, toplamanın `int` olarak yapılmasını engellemez.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Birinci tamsayı: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int birinci))
{
    Console.WriteLine("Hata: Tamsayı giriniz.");
    return;
}

Console.Write("İkinci tamsayı: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ikinci))
{
    Console.WriteLine("Hata: Tamsayı giriniz.");
    return;
}

long toplam = (long)birinci + ikinci;
Console.WriteLine("Toplam: " + toplam.ToString(CultureInfo.InvariantCulture));
```

Kod, bir C# konsol projesinin `Program.cs` dosyasına tek başına yerleştirilebilir. `TryParse`, dönüşümün başarılı olup olmadığını bildirir; `out` ile dönüştürülen değeri verir. `return`, bu örnekte programı sonlandırır. Kendi sınıfınızı veya nesnenizi tanımlamanız gerekmez.

## Örnek çalıştırmalar

Tablo, giriş istemlerinden sonra yazılan sonuç satırını gösterir. Girdi sütunundaki virgül, iki ayrı satırı ayırır.

| Girdi (sırasıyla) | Sonuç satırı |
| --- | --- |
| `12`, `8` | `Toplam: 20` |
| `-5`, `3` | `Toplam: -2` |
| `0`, `0` | `Toplam: 0` |
| `2147483647`, `2147483647` | `Toplam: 4294967294` |
| `abc` | `Hata: Tamsayı giriniz.` |

## Sınır durumları

- Negatif değerler ve sıfır geçerli girdilerdir.
- `2147483648`, `int` aralığının dışında olduğu için reddedilir.
- `2.5` bir tamsayı olmadığı için reddedilir.
- Boş satır veya giriş akışının bitmesi hata mesajıyla sonlanır.
- İki `int` değerinin olası bütün toplamları `long` aralığında kalır.

## Kazanımlar

- Metin girdisiyle sayısal değer arasındaki farkı açıklamak.
- Değişken tanımlamak ve bir aritmetik ifadeyi değerlendirmek.
- Girdiyi doğrulamak ve hata durumunda işlemi durdurmak.
- İşlem türünün taşma riskiyle ilişkisini görmek.

## Alıştırmalar

1. Aynı iki sayının farkını da yazdırın; çıkarma işleminden önce tür dönüşümünü uygulayın.
2. Üç tamsayının toplamını hesaplayacak biçimde programı genişletin.
3. `long toplam = birinci + ikinci;` yazıldığında büyük girdiler için oluşabilecek sorunu açıklayın.
