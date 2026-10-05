---
id: "012"
order: 12
title: "Puanı harf notuna dönüştürme"
level: "Temel"
prerequisites: ["005", "011"]
concepts: ["Aralık sınıflandırması", "Sıralı koşul zinciri", "Eşik değer", "İlk doğru dal"]
---

# Puanı harf notuna dönüştürme

## Problem tanımı

Kullanıcının girdiği 0 ile 100 arasındaki tamsayı puanı bir harf notuna dönüştürün. Bu problem için kullanılan ölçek yalnızca programlama alıştırmasına ait örnek bir sınıflandırmadır; herhangi bir kurumun not yönetmeliği değildir.

90–100 aralığı AA, 80–89 BA, 70–79 BB, 60–69 CB, 50–59 CC ve 0–49 FF olarak sınıflandırılır. Aralıklar bütün geçerli puanları kapsar ve bir puan yalnızca tek harf notuna karşılık gelir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `puan` | 0 ile 100 arasında tamsayı. |
| Çıktı | `harfNotu` | `AA`, `BA`, `BB`, `CB`, `CC` veya `FF`. |

`Puan (0..100): ` istemine bir tamsayı girin. Basamak ayırıcı veya ondalık bölüm kullanmayın. Geçerli girişte `Harf notu: ` ile başlayan tek sonuç satırı yazılır. Geçersiz girişte `Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.` yazılır ve program sona erer.

## Algoritma

1. Puanı okuyun.
2. Girdi tamsayı değilse veya 0 ile 100 arasında değilse hata yazıp bitirin.
3. Puan en az 90 ise harf notunu AA belirleyin.
4. Aksi durumda puan en az 80 ise BA belirleyin.
5. Aksi durumda puan en az 70 ise BB belirleyin.
6. Aksi durumda puan en az 60 ise CB belirleyin.
7. Aksi durumda puan en az 50 ise CC belirleyin.
8. Hiçbir eşik sağlanmadıysa FF belirleyin.
9. Harf notunu yazdırın.

Koşullar yüksek eşikten düşük eşiğe doğru denenir. Örneğin 85 puanı 90 kontrolünü geçemez, 80 kontrolünü geçer ve BA olur. Bu dalda ayrıca `puan <= 89` yazılması gerekmez; önceki koşulun yanlış olması zaten puanın 90'dan küçük olduğunu gösterir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Puan (0..100): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int puan) || puan < 0 || puan > 100)
{
    Console.WriteLine("Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.");
    return;
}

string harfNotu;
if (puan >= 90)
{
    harfNotu = "AA";
}
else if (puan >= 80)
{
    harfNotu = "BA";
}
else if (puan >= 70)
{
    harfNotu = "BB";
}
else if (puan >= 60)
{
    harfNotu = "CB";
}
else if (puan >= 50)
{
    harfNotu = "CC";
}
else
{
    harfNotu = "FF";
}

Console.WriteLine("Harf notu: " + harfNotu);
```

Kod, bir konsol projesinin `Program.cs` dosyasına tek başına konabilir. `if / else if / else` zinciri ilk doğru dalı seçer. Her dal `harfNotu` değişkenine bir metin atar; tek ortak çıktı satırı sınıflandırma tamamlandıktan sonra çalışır. Ayrı `if` ifadeleri kullanılması aynı puan için birden fazla koşulun doğru olmasına ve önceki atamanın değiştirilmesine yol açabilir.

## Örnek çalıştırmalar

Tablo, giriş isteminden sonra yazılan tam satırı gösterir.

| Puan | Sonuç satırı | Denenen durum |
| --- | --- | --- |
| `100` | `Harf notu: AA` | Geçerli en büyük puan. |
| `90` | `Harf notu: AA` | AA alt sınırı. |
| `89` | `Harf notu: BA` | AA sınırının hemen altı. |
| `80` | `Harf notu: BA` | BA alt sınırı. |
| `79` | `Harf notu: BB` | BA sınırının hemen altı. |
| `70` | `Harf notu: BB` | BB alt sınırı. |
| `60` | `Harf notu: CB` | CB alt sınırı. |
| `50` | `Harf notu: CC` | CC alt sınırı. |
| `49` | `Harf notu: FF` | CC sınırının hemen altı. |
| `0` | `Harf notu: FF` | Geçerli en küçük puan. |
| `101` | `Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.` | Aralık dışı. |
| `-1` | `Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.` | Aralık dışı. |
| `89.5` | `Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.` | Tamsayı değil. |

## Sınır durumları

- 0 ve 100 kabul edilir; bu değerlerin dışındaki puanlar sınıflandırılmaz.
- Eşiklerde eşitlik dahildir. 90, BA yerine AA olur.
- `puan >= 50` koşulu zincirin başına taşınırsa 90 puanı yanlışlıkla CC olarak sınıflandırılır.
- FF dalına yalnızca doğrulanmış 0–49 puanları ulaşır; negatif puanlar bu dala bırakılmaz.
- Boş giriş ve sayı olmayan metin hata üretir. Ondalıklı puanlar yuvarlanmaz; reddedilir.

## Kazanımlar

- Birbirini dışlayan sayı aralıklarını yüksekten düşüğe bir koşul zincirinde sınıflandırma.
- Önceki dalın yanlış olmasının sonraki dalın aralığını nasıl daralttığını açıklama.
- Bir metin sonucunu dallarda belirleyip ortak bir satırda yazdırma.
- Her eşik için eşik değerini ve hemen altındaki değeri içeren testler tasarlama.

## Alıştırmalar

1. Aynı sınıflandırmayı `puan < 50` koşulundan başlayarak düşükten yükseğe kurun ve sonuçları karşılaştırın.
2. 59, 60, 69 ve 70 için hangi koşulların hangi sırayla denendiğini yazın.
3. Örnek ölçeğe 95–100 aralığında `AA+` ekleyin; yeni koşulun zincirde nereye gelmesi gerektiğini açıklayın.
