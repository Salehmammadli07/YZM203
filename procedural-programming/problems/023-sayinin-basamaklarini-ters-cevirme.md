---
id: "023"
order: 23
title: "Sayının basamaklarını ters çevirme"
level: "Orta"
prerequisites: ["022"]
concepts: ["Basamak ayırma", "Sayısal birleştirme", "Çalışma kopyası", "long türü", "Döngü değişmezi"]
---

# Sayının basamaklarını ters çevirme

## Problem tanımı

0 ile 2147483647 arasında girilen bir tamsayının basamaklarını ters sırayla birleştirerek yeni bir sayı oluşturun. Hazır ters çevirme işlemleri veya metin karakterlerini kullanmayın. Son basamağı `% 10` ile çıkarın; yeni sayıyı 10 ile çarpıp çıkarılan basamağı ekleyin.

Örneğin 407 değeri 704'e dönüşür. 1200 için ters basamak sırası 0021 olsa da çıktı bir sayıdır ve baştaki sıfırlar gösterilmez; sonuç 21 olur. Sıfırın tersi 0'dır. Girdi `int` aralığındadır, ancak ters çevrilen sayı aynı aralığa sığmayabilir. Bu nedenle sonuç `long` türünde tutulur; 2147483647'nin tersi 7463847412'dir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `sayi` | 0 ile 2147483647 arasında `int`; değiştirilmeden korunur. |
| Ara değer | `kalanSayi` | İşlenmemiş basamakları taşıyan kopya. |
| Çıktı | `tersSayi` | `long` türünde sayısal ters; `Ters sayı: N`. |

`Sayı (0..2147483647): ` istemine tamsayı girin. Geçersiz biçim veya aralık için `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` yazılır. Çıktı metin basamak dizisi değil, sayısal değerdir; başta sıfır doldurulmaz.

## Algoritma

1. Sayıyı okuyup biçimini ve aralığını doğrulayın.
2. Başlangıç değerini koruyup `kalanSayi` içine kopyalayın.
3. `long` türündeki `tersSayi` değerini 0 yapın.
4. `do` gövdesinde son basamağı `kalanSayi % 10` ile alın.
5. `tersSayi = tersSayi * 10 + basamak` işlemini yapın.
6. `kalanSayi /= 10` ile işlenen basamağı çıkarın.
7. Kalan sayı pozitif olduğu sürece 4. adımdan devam edin.
8. Ters sayıyı yazdırın.

Her yineleme sonunda `tersSayi`, o ana kadar sağdan çıkarılan basamakların çıkarılma sırasıyla oluşturduğu sayıdır. `kalanSayi` ise işlenmemiş sol kısmı taşır. 407 için ters sayı sırasıyla 7, 70 ve 704 olur. 10 ile çarpmak mevcut basamakları bir basamak sola kaydırır; yeni basamağı eklemek sağdaki yeri doldurur. Sıfır basamakları da aynı güncelleme kuralından geçer.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Sayı (0..2147483647): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int sayi) || sayi < 0)
{
    Console.WriteLine("Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.");
    return;
}

int kalanSayi = sayi;
long tersSayi = 0;

do
{
    int basamak = kalanSayi % 10;
    tersSayi = tersSayi * 10 + basamak;
    kalanSayi /= 10;
}
while (kalanSayi > 0);

Console.WriteLine("Ters sayı: " + tersSayi.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `tersSayi` daha ilk işlemden itibaren `long` olduğu için çarpma ve ekleme de bu türde yapılır. Sonucu yalnızca çıktıdan hemen önce `long` türüne çevirmek, daha önce gerçekleşmiş bir `int` taşmasını engellemez. Başlangıç sayısı bu çözümde yazdırılmasa da değişmeden tutulur; sonraki problemde karşılaştırma için kullanılabilir.

## Örnek çalıştırmalar

Tablo giriş isteminden sonra yazılan sonuç veya hata satırını gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `407` | `Ters sayı: 704` | Aradaki sıfır korunur. |
| `0` | `Ters sayı: 0` | Bir yineleme sonucu sıfır. |
| `7` | `Ters sayı: 7` | Tek basamak değişmez. |
| `10` | `Ters sayı: 1` | Başta oluşan sıfır gösterilmez. |
| `1200` | `Ters sayı: 21` | İki son sıfır sayısal tersin başında kaybolur. |
| `1001` | `Ters sayı: 1001` | Simetrik basamaklar. |
| `00120` | `Ters sayı: 21` | Girdi sayısal olarak 120'dir. |
| `2147483647` | `Ters sayı: 7463847412` | Sonuç `int` sınırını aşar, `long` içine sığar. |
| `-12` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Negatif sayı reddedilir. |
| `2147483648` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Girdi tür sınırını aşar. |
| `sayı` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Metin reddedilir. |

## Sınır durumları

- Sıfır için döngü bir kez çalışır ve ters sayı 0 olur.
- Son sıfırlar sayısal tersin başına geldiğinde görünmez. Bu yüzden ters çevirme işlemini iki kez uygulamak her sayıyı geri vermez: 1200 → 21 → 12.
- Başta yazılmış sıfırlar `int.TryParse` sonrasında sayısal değerin parçası değildir.
- Girdi en fazla 10 basamaklıdır; ters sayı 9999999999 değerinden büyük olamaz. Bu üst sınır `long` aralığına rahatça sığar.
- `kalanSayi` her pozitif yinelemede küçülür; son basamak alındığında 0 olur ve döngü biter.
- Negatif, aralık dışı, boş veya metin girişinde ters sayı yazdırılmaz.

## Kazanımlar

- Çıkarılan basamaklarla yeni bir sayı kurma.
- Çarpma ve toplamanın basamak konumunu nasıl değiştirdiğini açıklama.
- Girdi türü ile sonuç türünün farklı aralıklar gerektirebileceğini görme.
- Çalışma kopyası kullanarak ilk değeri koruma.
- Sayısal ters çevirme ile metin ters çevirmenin sıfırlardaki farkını açıklama.

## Alıştırmalar

1. 5080 için her yinelemenin basamak, kalan sayı ve ters sayı değerlerini tabloya yazın.
2. Başlangıç sayısını da `Başlangıç sayısı: N` satırıyla yazdırın. Sonuca ulaştıktan sonra `sayi` ve `kalanSayi` değişkenlerinin farkını gösterin.
3. Ters çevirme sırasında kaç sıfır basamağı işlendiğini ayrı sayaçta tutun. 1000 ve 1001 için sonuçları karşılaştırın.
