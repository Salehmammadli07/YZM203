---
id: "006"
order: 6
title: "Bir sayının çift veya tek olduğunu belirleme"
level: "Başlangıç"
prerequisites: ["005"]
concepts: ["Tamsayı", "Kalan operatörü", "Koşul", "Girdi doğrulama"]
---

# Bir sayının çift veya tek olduğunu belirleme

## Problem tanımı

Klavyeden girilen bir tamsayının çift mi, tek mi olduğunu belirleyin. İkiye tam bölünen tamsayılar çifttir; diğer tamsayılar tektir. Bu tanım sıfır ve negatif sayılar için de geçerlidir. Örneğin `-3` tektir; `0` ve `-8` çifttir.

Bu problemde sonuç, sayının büyüklüğüne değil, ikiye bölümünden kalan değere bağlıdır. C# dilindeki `%` operatörü bölümün kalanını verir. Sayı için `sayi % 2 == 0` ifadesi doğruysa çift, yanlışsa tek sonucu üretilir. Negatif tek sayılarda kalan negatif olabilir; bu nedenle tekliği yalnızca `kalan == 1` koşuluyla sınamayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `sayi` | `int` aralığında bir tamsayı: -2147483648 ile 2147483647. |
| Çıktı | Sonuç | `Sayı çifttir.` veya `Sayı tektir.` |

Program `Bir tamsayı girin: ` istemini gösterir. Bir sayı yazıp Enter tuşuna basın. Ondalık sayı, boş giriş veya aralık dışındaki değer geçersizdir. Sayı yazımında kültürden bağımsız tamsayı biçimi kullanılır; basamak ayırıcı eklemeyin.

## Algoritma

1. Kullanıcıdan bir tamsayı isteyin.
2. Girdiyi tamsayıya dönüştürmeyi deneyin.
3. Dönüştürme başarısızsa hata mesajını gösterip programı bitirin.
4. Sayının ikiye bölümünden kalanı hesaplayın.
5. Kalan sıfırsa çift, değilse tek olduğunu yazdırın.

Karar yapısının yalnızca iki sonucu vardır. Koşulun doğru olduğu durumda `if` gövdesi, yanlış olduğu durumda `else` gövdesi çalışır. Her geçerli giriş için tam bir sonuç üretilir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Bir tamsayı girin: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int sayi))
{
    Console.WriteLine("Hata: int aralığında bir tamsayı girin.");
    return;
}

if (sayi % 2 == 0)
{
    Console.WriteLine("Sayı çifttir.");
}
else
{
    Console.WriteLine("Sayı tektir.");
}
```

`TryParse`, geçersiz bir girişte programın dönüşüm hatasıyla kesilmesini önler. Başarısız dönüşümden sonra kullanılan `return`, karar adımına geçilmeden programı sonlandırır.

## Örnek çalıştırmalar

Aşağıdaki tabloda istem metni yerine yalnızca giriş ve sonuç satırı gösterilir.

| Girdi | Sonuç |
| --- | --- |
| `12` | `Sayı çifttir.` |
| `7` | `Sayı tektir.` |
| `0` | `Sayı çifttir.` |
| `-3` | `Sayı tektir.` |
| `-2147483648` | `Sayı çifttir.` |

## Sınır durumları

- Sıfırın ikiye bölümünden kalan sıfırdır; sıfır çifttir.
- Negatif sayılar da aynı koşulla değerlendirilir.
- `2147483648` ve `abc` için hata mesajı yazılır; çiftlik testi yapılmaz.
- Kalan işlemi geçerli `int` girdileri için ek bir aralık kısıtı gerektirmez.

## Kazanımlar

- Bölme işlemi ile kalan işleminin farklı amaçlarını açıklama.
- Bir matematiksel tanımı mantıksal koşula dönüştürme.
- `if` ve `else` ile birbirini dışlayan sonuçları üretme.
- Negatif sayılar ve sıfırla sınır durumlarını denetleme.

## Alıştırmalar

1. Aynı yaklaşımı kullanarak sayının üçe tam bölünüp bölünmediğini gösterin.
2. Sayının hem ikiye hem üçe tam bölünüp bölünmediğini belirleyin.
3. `kalan == 1` koşulunun neden `-3` için yanlış sonuç verdiğini açıklayın.
