---
id: "024"
order: 24
title: "Palindrom sayı olup olmadığını belirleme"
level: "Orta"
prerequisites: ["005", "023"]
concepts: ["Palindrom", "Aritmetik ters çevirme", "Asıl değeri koruma", "Karşılaştırma", "Döngü ve karar"]
---

# Palindrom sayı olup olmadığını belirleme

## Problem tanımı

0 ile 2147483647 arasında girilen bir tamsayının onluk basamaklarının soldan sağa ve sağdan sola aynı olup olmadığını belirleyin. Böyle sayılara palindrom sayı denir. Örneğin 1221 ve 707 palindromdur; 123 ve 10 değildir. Tek basamaklı sayılar, 0 dahil, palindrom kabul edilir.

Sayıyı metne dönüştürmeden aritmetik işlemlerle tersini oluşturun ve başlangıç sayısıyla karşılaştırın. Karşılaştırmada ilk sayı gerektiği için basamakları çıkarırken bir kopyayı değiştirin. Girdi sayısal olarak yorumlanır: `00100` girişi 100 değeridir, baştaki sıfırlar karşılaştırmaya katılmaz. Negatif sayılar çalışma kapsamına alınmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `sayi` | 0 ile 2147483647 arasında `int`; karşılaştırma için korunur. |
| Ara değer | `kalanSayi` | Basamakları çıkarılan kopya. |
| Ara değer | `tersSayi` | Aritmetik ters, `long` türünde. |
| Çıktı | Sınıflandırma | `Sonuç: Palindrom` veya `Sonuç: Palindrom değil`. |

`Sayı (0..2147483647): ` istemine tamsayı girin. Hatalı biçim veya aralıkta `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` yazılır. Başta yazılan sıfırlar değil, dönüştürülen tamsayının basamakları incelenir.

## Algoritma

1. Sayıyı okuyup biçimini ve aralığını doğrulayın.
2. Sayıyı `kalanSayi` içine kopyalayın; `tersSayi` değerini 0 yapın.
3. Son basamağı `kalanSayi % 10` ile alın.
4. `tersSayi = tersSayi * 10 + basamak` ile ters sayıya ekleyin.
5. `kalanSayi /= 10` ile işlenen basamağı çıkarın.
6. `do / while` ile kalan sayı pozitif olduğu sürece 3. adımdan devam edin.
7. Başlangıç sayısı ters sayıya eşitse palindrom, değilse palindrom değil sonucunu yazdırın.

Döngü boyunca `sayi` sabit kalır. Her yinelemede `tersSayi`, sağdan çıkarılan basamakları çıkarılma sırasıyla taşır; `kalanSayi` işlenmemiş sol kısmı tutar. Döngü sonunda bütün basamaklar çıkarılmıştır. 1221 için ters sayı sırasıyla 1, 12, 122 ve 1221 olur; son karşılaştırma doğrudur. Başlangıç değerini de bölerek değiştirmek, doğru karşılaştırma için gerekli bilgiyi kaybettirirdi.

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

if (tersSayi == sayi)
{
    Console.WriteLine("Sonuç: Palindrom");
}
else
{
    Console.WriteLine("Sonuç: Palindrom değil");
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `long` ters sayı ile `int` girdi karşılaştırılırken `int` değeri sayısal olarak `long` türüne genişletilir; bilgi kaybı olmaz. Girdi `int` içine sığsa da tersi sığmayabileceği için ters sayı yine `long` türündedir. Koşul zinciri yalnızca bir sınıflandırma satırı üretir.

## Örnek çalıştırmalar

Tablo giriş isteminden sonraki tam sonuç veya hata satırını gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `1221` | `Sonuç: Palindrom` | Çift sayıda simetrik basamak. |
| `707` | `Sonuç: Palindrom` | Ortada sıfır bulunabilir. |
| `123` | `Sonuç: Palindrom değil` | Ters sayı 321'dir. |
| `0` | `Sonuç: Palindrom` | Sıfırın tersi sıfırdır. |
| `9` | `Sonuç: Palindrom` | Tek basamaklı sayı. |
| `10` | `Sonuç: Palindrom değil` | Sayısal ters 1'dir. |
| `1001` | `Sonuç: Palindrom` | İki iç sıfır simetriyi bozmaz. |
| `00100` | `Sonuç: Palindrom değil` | İncelenen değer 100, tersi 1'dir. |
| `2147447412` | `Sonuç: Palindrom` | `int` üst sınırına yakın palindrom. |
| `2147483647` | `Sonuç: Palindrom değil` | Tersi 7463847412 olur. |
| `-121` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Negatif sayılar kapsam dışıdır. |
| `2147483648` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Girdi aralık dışındadır. |
| `palindrom` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Metin reddedilir. |

## Sınır durumları

- 0 ile 9 arasındaki bütün sayılar palindromdur; tek basamak sağdan ve soldan aynıdır.
- Sıfırla biten pozitif sayının sayısal tersi daha az basamaklı olur; bu sayı palindrom olamaz.
- Sayının içindeki sıfırlar korunur. 1001 palindrom olduğu halde 1000 değildir.
- Başta yazılan sıfırlar girdinin metinsel görünümünü değiştirir; tamsayı değerini değiştirmez.
- Ters sayı en fazla 10 basamaklı olduğundan `long` aralığında kalır. Sonucun `int` türüne zorla daraltılması gerekmez.
- Geçersiz sayı, boş satır veya giriş sonu halinde karşılaştırma yapılmaz ve sınıflandırma yazılmaz.

## Kazanımlar

- Bir önceki basamak algoritmasını karar problemi içinde yeniden uygulama.
- Döngüyle dönüştürülen kopya ile korunan asıl değerin görevlerini ayırma.
- Sayısal eşitlik üzerinden palindrom tanımını gerçekleştirme.
- Daha geniş türde üretilen ara sonucu güvenle karşılaştırma.
- Metinsel görünüm ile sayısal değer arasındaki farkı örneklerle açıklama.

## Alıştırmalar

1. Palindrom kararından önce `Ters sayı: N` satırını ekleyin. 120, 1001 ve 0 için iki çıktı arasındaki ilişkiyi açıklayın.
2. Palindrom olmayan sayılarda başlangıç ile ters sayı arasındaki farkı `long` türünde hesaplayın. 2147483647 için neden `long` gerektiğini gösterin.
3. 100 ile 150 arasındaki sayıları dolaşarak palindrom olanları yazdırın. Her yeni adayda kalan sayı ve ters sayı değişkenlerini yeniden başlatın.
