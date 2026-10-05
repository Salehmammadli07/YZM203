---
id: "018"
order: 18
title: "Saate saniye ekleme"
level: "Orta"
prerequisites: ["006", "008"]
concepts: ["Tamsayı bölmesi", "Kalan işleci", "Birim dönüşümü", "Gün taşması", "Sabit genişlikte biçimleme"]
---

# Saate saniye ekleme

## Problem tanımı

Saat, dakika ve saniye olarak verilen bir saate belirli sayıda saniye ekleyin. Yeni saati ve başlangıç gününden kaç gün sonrasına geçildiğini bulun. Bir gün 24 saat, bir saat 60 dakika, bir dakika 60 saniye kabul edilir. Tarih, saat dilimi, yaz saati ve artık saniye kuralları bu modelin dışında tutulur.

Örneğin `23:59:50` saatine 20 saniye eklenirse bir sonraki günün `00:00:10` saatine ulaşılır. Gündeki saat bir noktayı, eklenen saniye ise bir süreyi belirtir. Bütün değerleri önce saniyeye çevirmek, dakika ve saat taşmalarını tek hesapta yönetmeyi sağlar.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `saat` | 0 ile 23 arasında `int`. |
| Girdi | `dakika` | 0 ile 59 arasında `int`. |
| Girdi | `saniye` | 0 ile 59 arasında `int`. |
| Girdi | `eklenen` | 0 ile 172800 arasında eklenecek saniye sayısı; `int`. |
| Çıktı | `gun` | Başlangıç gününün sonrasına geçilen gün sayısı; `Geçen gün: N`. |
| Çıktı | Yeni saat | `Yeni saat: HH:mm:ss` biçiminde iki basamaklı saat, dakika ve saniye. |

Sırasıyla `Saat (0..23): `, `Dakika (0..59): `, `Saniye (0..59): ` ve `Eklenecek saniye (0..172800): ` istemlerine her tamsayıyı ayrı satırda girin. Başlangıç günü 0 kabul edilir. Örneğin 1 gün geçmesi, başlangıç gününün ertesi gününe ulaşılmasıdır; mutlaka 24 saatlik bir sürenin eklenmiş olması değildir.

## Algoritma

1. Saat, dakika, saniye ve eklenecek süreyi sırayla okuyup aralıklarını doğrulayın.
2. Başlangıç saatini `saat * 3600 + dakika * 60 + saniye` ile saniyeye çevirin.
3. Eklenecek saniyeyi bu değere ekleyin.
4. Toplamı 86400'e tamsayı bölerek geçilen gün sayısını bulun.
5. Toplamın 86400'e bölümünden kalanı alarak yeni günün içindeki saniye sayısını bulun.
6. Gün içindeki saniyeyi 3600'e bölerek yeni saati bulun.
7. Gün içindeki saniyenin 3600'e bölümünden kalanını 60'a bölerek yeni dakikayı bulun.
8. Gün içindeki saniyenin 60'a bölümünden kalanını alarak yeni saniyeyi bulun.
9. Gün sayısını ve her alanı iki basamaklı yeni saati yazdırın.

Pozitif tamsayılar için `/` bölümün tamsayı kısmını, `%` kalanını verir. `toplam = gun * 86400 + gunIci` eşitliği, tam günlerle yeni gün içindeki saati birbirinden ayırır. `gunIci` her zaman 0 ile 86399 arasındadır.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Saat (0..23): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int saat) || saat < 0 || saat > 23)
{
    Console.WriteLine("Hata: Saat 0 ile 23 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Dakika (0..59): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int dakika) || dakika < 0 || dakika > 59)
{
    Console.WriteLine("Hata: Dakika 0 ile 59 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Saniye (0..59): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int saniye) || saniye < 0 || saniye > 59)
{
    Console.WriteLine("Hata: Saniye 0 ile 59 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Eklenecek saniye (0..172800): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int eklenen) || eklenen < 0 || eklenen > 172800)
{
    Console.WriteLine("Hata: Eklenecek saniye 0 ile 172800 arasında " +
        "bir tamsayı olmalıdır.");
    return;
}

int toplam = saat * 3600 + dakika * 60 + saniye + eklenen;
int gun = toplam / 86400;
int gunIci = toplam % 86400;
int yeniSaat = gunIci / 3600;
int yeniDakika = (gunIci % 3600) / 60;
int yeniSaniye = gunIci % 60;

Console.WriteLine("Geçen gün: " + gun.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Yeni saat: " +
    yeniSaat.ToString("D2", CultureInfo.InvariantCulture) + ":" +
    yeniDakika.ToString("D2", CultureInfo.InvariantCulture) + ":" +
    yeniSaniye.ToString("D2", CultureInfo.InvariantCulture));
```

`D2`, tamsayının en az iki basamakla yazılmasını sağlar; örneğin 7 değeri `07` olur. Hesaplama tamsayı aritmetiğiyle yapılır; saat parçalarını bulmak için yuvarlama veya ondalık tür gerekmez.

## Örnek çalıştırmalar

Girdi sütununda saat, dakika, saniye ve eklenecek saniye sırasıyla verilmiştir. Sonuç sütunu iki çıktı satırını gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `12`, `30`, `0`, `90` | `Geçen gün: 0`<br>`Yeni saat: 12:31:30` | Aynı gün içinde dakika taşması. |
| `23`, `59`, `50`, `20` | `Geçen gün: 1`<br>`Yeni saat: 00:00:10` | Gece yarısı aşılır. |
| `0`, `0`, `0`, `0` | `Geçen gün: 0`<br>`Yeni saat: 00:00:00` | Alt sınırlar ve sıfır süre. |
| `23`, `59`, `59`, `1` | `Geçen gün: 1`<br>`Yeni saat: 00:00:00` | Tam gece yarısına ulaşılır. |
| `7`, `8`, `9`, `86400` | `Geçen gün: 1`<br>`Yeni saat: 07:08:09` | Tam bir gün eklenir, saat aynı kalır. |
| `23`, `59`, `59`, `172800` | `Geçen gün: 2`<br>`Yeni saat: 23:59:59` | En büyük geçerli toplam. |
| İlk girdi `24` | `Hata: Saat 0 ile 23 arasında bir tamsayı olmalıdır.` | Sonraki girdiler istenmez. |
| `0`, `60` | `Hata: Dakika 0 ile 59 arasında bir tamsayı olmalıdır.` | Doğrulama dakika adımında durur. |
| `0`, `0`, `0`, `-1` | `Hata: Eklenecek saniye 0 ile 172800 arasında bir tamsayı olmalıdır.` | Negatif süre reddedilir. |
| İlk girdi `sabah` | `Hata: Saat 0 ile 23 arasında bir tamsayı olmalıdır.` | Metin girişi reddedilir. |

## Sınır durumları

- Saat 24 olamaz; tam gece yarısı `00:00:00` ve bir sonraki gün olarak gösterilir.
- Sıfır saniye eklenmesi başlangıç saatini ve günü değiştirmez.
- En büyük toplam `86399 + 172800 = 259199` saniyedir; `int` aralığında kalır.
- Gün sayısı bu aralıkta 0, 1 veya 2 olur; örneğin gece yarısına yakın başlangıç, kısa ek sürede bile günü değiştirebilir.
- Negatif süre kabul edilmediği için negatif kalanların yorumlanması gerekmez.
- Geçersiz alan görüldüğünde sonraki alanlar okunmaz ve sonuç hesaplanmaz.

## Kazanımlar

- Farklı zaman birimlerini ortak bir tamsayı birimine dönüştürme.
- Tamsayı bölmesi ve kalan işleciyle gün, saat, dakika ve saniyeyi ayırma.
- Bir değerin tam katlarını ve kalan kısmını birlikte yorumlama.
- `D2` biçimini kullanarak sabit genişlikli saat yazdırma.

## Alıştırmalar

1. `01:59:59` saatine bir saniye eklenirken yeni saatin `02:00:00` oluşmasını ara değerlerle gösterin.
2. Başlangıç saatini değiştirmeden 86399, 86400 ve 86401 saniye için üç sonucu karşılaştırın.
3. Yeni gün içindeki saniyeyi çıktı olarak ekleyip `toplam = gun * 86400 + gunIci` eşitliğini doğrulayın.
