---
id: "019"
order: 19
title: "Bir ayın gün sayısını bulma"
level: "Orta"
prerequisites: ["014", "015"]
concepts: ["Switch deyimi", "Gruplanmış case dalları", "Artık yıl", "Mantıksal ifade", "Takvim kuralları"]
---

# Bir ayın gün sayısını bulma

## Problem tanımı

Yıl ve ay numarasını kullanarak o ayın kaç gün sürdüğünü bulun. Ocak, mart, mayıs, temmuz, ağustos, ekim ve aralık 31 gün; nisan, haziran, eylül ve kasım 30 gündür. Şubat normal yılda 28, artık yılda 29 gün sürer.

Bu çalışmada Gregoryen takvimin hesap kuralları 1 ile 9999 arasındaki bütün yıllara aynı biçimde uygulanır; 1582 öncesindeki yıllar da aynı hesap modeline dahildir. Tarihsel takvim geçişleri ve bölgesel uygulamalar dikkate alınmaz. Bir yıl 400'e tam bölünüyorsa artık yıldır. 400'e bölünmüyorsa, 4'e bölünüp 100'e bölünmeyen yıllar artık yıldır. Bu nedenle 2000 artık yıl, 1900 normal yıldır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `yil` | 1 ile 9999 arasında `int`. |
| Girdi | `ay` | 1 ile 12 arasında `int`; 1 ocak, 2 şubat, 12 aralık. |
| Çıktı | `gunSayisi` | `Gün sayısı: N` biçiminde 28, 29, 30 veya 31. |

Önce `Yıl (1..9999): `, sonra `Ay (1..12): ` istemine birer tamsayı girin. Geçersiz biçimde veya aralıkta bir değer görülürse ilgili alanın hata mesajı yazılır ve program sona erer. Ondalık veya basamak ayırıcı kullanmayın.

## Algoritma

1. Yılı okuyun ve 1 ile 9999 arasında bir tamsayı olduğunu doğrulayın.
2. Ayı okuyun ve 1 ile 12 arasında bir tamsayı olduğunu doğrulayın.
3. `yil % 400 == 0 || (yil % 4 == 0 && yil % 100 != 0)` ifadesiyle artık yıl durumunu hesaplayın.
4. `switch` deyiminde ayı seçin; 31 gün süren ayları aynı dalda, 30 gün süren ayları başka bir dalda gruplayın.
5. Şubat dalında artık yıl doğruysa 29, değilse 28 gün atayın.
6. Gün sayısını yazdırın.

`switch`, bir değerin farklı sabitlerle eşleşmesini düzenler. Arka arkaya yazılan `case` etiketleri aynı işlem grubunu kullanabilir. Böylece yedi ayrı 31 gün ataması yerine tek atama yeterli olur. Artık yıl kuralı yalnızca şubatın gün sayısını değiştirir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Yıl (1..9999): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int yil) || yil < 1 || yil > 9999)
{
    Console.WriteLine("Hata: Yıl 1 ile 9999 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Ay (1..12): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ay) || ay < 1 || ay > 12)
{
    Console.WriteLine("Hata: Ay 1 ile 12 arasında bir tamsayı olmalıdır.");
    return;
}

bool artikYil = yil % 400 == 0 || (yil % 4 == 0 && yil % 100 != 0);
int gunSayisi;
switch (ay)
{
    case 1:
    case 3:
    case 5:
    case 7:
    case 8:
    case 10:
    case 12:
        gunSayisi = 31;
        break;
    case 4:
    case 6:
    case 9:
    case 11:
        gunSayisi = 30;
        break;
    case 2:
        if (artikYil)
        {
            gunSayisi = 29;
        }
        else
        {
            gunSayisi = 28;
        }
        break;
    default:
        Console.WriteLine("Hata: Ay 1 ile 12 arasında bir tamsayı olmalıdır.");
        return;
}

Console.WriteLine("Gün sayısı: " + gunSayisi.ToString(CultureInfo.InvariantCulture));
```

Her dalın sonundaki `break`, `switch` deyiminden çıkışı sağlar. `default` geçerli aralıktaki girdiler için çalışmaz; ancak hiçbir ay etiketiyle eşleşmeyen bir durumda programın gün sayısı yazdırmasını engeller. Şubat dalı, seçilmiş bir durumun içinde ek koşul kurulabileceğini gösterir.

## Örnek çalıştırmalar

Girdiler yıl ve ay sırasındadır; sonuç sütunu giriş istemlerinden sonraki satırı gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `2025`, `1` | `Gün sayısı: 31` | Ocak yılın artık olmasından etkilenmez. |
| `2025`, `4` | `Gün sayısı: 30` | Nisan 30 gün sürer. |
| `2024`, `2` | `Gün sayısı: 29` | Dörde bölünen, yüze bölünmeyen yıl. |
| `2025`, `2` | `Gün sayısı: 28` | Normal yılın şubatı. |
| `1900`, `2` | `Gün sayısı: 28` | Yüze bölünür, dört yüze bölünmez. |
| `2000`, `2` | `Gün sayısı: 29` | Dört yüze bölünür. |
| `1`, `2` | `Gün sayısı: 28` | Modelin alt yıl sınırı. |
| `9999`, `12` | `Gün sayısı: 31` | Üst yıl ve ay sınırı. |
| İlk girdi `0` | `Hata: Yıl 1 ile 9999 arasında bir tamsayı olmalıdır.` | Ay istenmez. |
| `2024`, `13` | `Hata: Ay 1 ile 12 arasında bir tamsayı olmalıdır.` | Geçersiz ay. |
| `2024`, `şubat` | `Hata: Ay 1 ile 12 arasında bir tamsayı olmalıdır.` | Ayın adı yerine numarası beklenir. |

## Sınır durumları

- Yıl sıfır kabul edilmez; kabul edilen yıl aralığı 1 ile başlar.
- Ay numarası 0 veya 13 olamaz.
- Yalnızca 4'e bölünebilirlik kontrolü, 1900 gibi yüzyıl yıllarında yanlış sonuç üretir.
- Yalnızca 100'e bölünebilirlik kontrolü de 2000 gibi 400'e bölünen artık yılları kaçırır.
- Her geçerli ay için gün sayısı mutlaka atanır; şubat dışındaki aylarda yıl değeri sonucu değiştirmez.
- Boş satır, metin veya ondalıklı girdi için gün sayısı hesaplanmaz.

## Kazanımlar

- Bir tamsayı değerini `switch` ile sabit durumlara ayırma.
- Aynı sonucu üreten ayları tek işlem dalında toplama.
- `&&`, `||` ve parantezlerle artık yıl kuralını tam olarak ifade etme.
- 1900 ve 2000 örnekleriyle genel kuralın istisnalarını sınama.

## Alıştırmalar

1. Aynı işlemi yalnızca `if / else if / else` kullanarak yazıp iki çözümün okunabilirliğini karşılaştırın.
2. 2100 ve 2400 yıllarının şubat gün sayılarını bulun ve artık yıl ifadesindeki her karşılaştırmayı açıklayın.
3. İkinci bir `switch` ekleyip gün sayısının yanında ayın Türkçe adını da yazdırın.
