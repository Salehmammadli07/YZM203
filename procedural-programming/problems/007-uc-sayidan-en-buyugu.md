---
id: "007"
order: 7
title: "Üç sayıdan en büyüğünü bulma"
level: "Başlangıç"
prerequisites: ["005"]
concepts: ["Karşılaştırma", "Koşul", "Değişken güncelleme", "Girdi doğrulama"]
---

# Üç sayıdan en büyüğünü bulma

## Problem tanımı

Kullanıcıdan üç tamsayı alın ve bunların en büyüğünü ekrana yazdırın. Sayılar pozitif, negatif veya sıfır olabilir. İki ya da üç sayının eşit olduğu durumlarda da tek bir en büyük değer üretilmelidir.

Çözümde önce ilk sayı geçici en büyük değer kabul edilir. İkinci sayı bu değerden büyükse geçici değer güncellenir. Aynı işlem üçüncü sayı için tekrar edilir. Böylece her karşılaştırmadan sonra değişken, o ana kadar incelenmiş sayıların en büyüğünü tutar. Başlangıç değerini ilk sayıdan almak, tüm girdilerin negatif olduğu durumlarda da doğru sonuç verir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `birinci`, `ikinci`, `ucuncu` | Her biri `int` aralığında üç tamsayı. |
| Çıktı | `enBuyuk` | Girilen üç değerden en büyüğü. |

Program sırasıyla `Birinci sayı: `, `İkinci sayı: ` ve `Üçüncü sayı: ` istemlerini gösterir. Her değeri ayrı satırda girin. Sayıların izin verilen aralığı -2147483648 ile 2147483647'dir. Basamak ayırıcı kullanmayın; ilk geçersiz girişte hata mesajı gösterilir ve program biter.

## Algoritma

1. Birinci sayıyı okuyun ve doğrulayın.
2. İkinci sayıyı okuyun ve doğrulayın.
3. Üçüncü sayıyı okuyun ve doğrulayın.
4. `enBuyuk` değişkenine birinci sayıyı atayın.
5. İkinci sayı `enBuyuk` değerinden büyükse değişkeni ikinci sayıyla güncelleyin.
6. Üçüncü sayı `enBuyuk` değerinden büyükse değişkeni üçüncü sayıyla güncelleyin.
7. `enBuyuk` değerini yazdırın.

İkinci karşılaştırma, ilk karşılaştırmada güncellenmiş olabilen değerle yapılır. Burada iki bağımsız `if` kullanılır; üçüncü sayının değerlendirilmesi ikinci sayının sonucuna bağlı olarak atlanmaz.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Birinci sayı: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int birinci))
{
    Console.WriteLine("Hata: int aralığında bir tamsayı girin.");
    return;
}

Console.Write("İkinci sayı: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ikinci))
{
    Console.WriteLine("Hata: int aralığında bir tamsayı girin.");
    return;
}

Console.Write("Üçüncü sayı: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ucuncu))
{
    Console.WriteLine("Hata: int aralığında bir tamsayı girin.");
    return;
}

int enBuyuk = birinci;
if (ikinci > enBuyuk)
{
    enBuyuk = ikinci;
}
if (ucuncu > enBuyuk)
{
    enBuyuk = ucuncu;
}

Console.WriteLine("En büyük sayı: " +
    enBuyuk.ToString(CultureInfo.InvariantCulture));
```

## Örnek çalıştırmalar

Girdi sütunu, üç ayrı satırda girilen sayıları sırasıyla gösterir. Sonuç sütununda istemler yer almaz.

| Girdiler | Sonuç |
| --- | --- |
| `4`, `9`, `2` | `En büyük sayı: 9` |
| `12`, `3`, `8` | `En büyük sayı: 12` |
| `-8`, `-2`, `-5` | `En büyük sayı: -2` |
| `7`, `7`, `1` | `En büyük sayı: 7` |
| `0`, `0`, `0` | `En büyük sayı: 0` |

## Sınır durumları

- Eşit değerler için güncelleme gerekmese de sonuç doğrudur.
- Tüm sayılar negatifse en büyük sayı sıfır kabul edilmez.
- Toplama veya çıkarma yapılmadığından karşılaştırmalar `int` sınırlarında taşma üretmez.
- Geçersiz girişte eksik verilerle karşılaştırma yapılmaz.

## Kazanımlar

- Bir sonucun önceki adımlarda nasıl korunduğunu izleme.
- Karşılaştırma operatörleriyle değişken güncelleme.
- Başlangıç değerinin algoritmanın doğruluğuna etkisini açıklama.
- Eşitlik ve negatif değerler için örnekler hazırlama.

## Alıştırmalar

1. Üç sayıdan en küçüğünü bulan çözümü yazın.
2. Dördüncü bir sayı ekleyerek aynı yaklaşımı genişletin.
3. `enBuyuk` başlangıçta sıfır olsaydı hangi girdilerde hata oluşurdu?
