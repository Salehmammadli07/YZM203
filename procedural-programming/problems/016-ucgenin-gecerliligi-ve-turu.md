---
id: "016"
order: 16
title: "Üçgenin geçerliliğini ve türünü belirleme"
level: "Orta"
prerequisites: ["007", "011", "013"]
concepts: ["Bileşik koşul", "Mantıksal VE", "Mantıksal VEYA", "Üçgen eşitsizliği", "Önce doğrulama sonra sınıflandırma"]
---

# Üçgenin geçerliliğini ve türünü belirleme

## Problem tanımı

Üç kenar uzunluğunun bir üçgen oluşturup oluşturmadığını belirleyin. Üçgen oluşuyorsa kenarların eşitliğine göre eşkenar, ikizkenar veya çeşitkenar sonucunu yazdırın. Kenarlar aynı birimle ölçülmüş pozitif tamsayılardır; açıları hesaplamayın.

Bir üçgende herhangi iki kenarın toplamı üçüncü kenardan kesinlikle büyük olmalıdır. Örneğin `2`, `3`, `5` uzunlukları üçgen oluşturmaz: iki kenarın toplamı üçüncü kenara eşittir. Üç kenarı eşit üçgen eşkenar, tam iki kenarı eşit üçgen ikizkenar, bütün kenarları farklı üçgen çeşitkenardır. Bu çalışmada eşkenar sonucu ayrıca verildiği için ikizkenar dalı yalnızca tam iki kenarın eşit olduğu durumu kapsar.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `a`, `b`, `c` | Her biri 1 ile 1000000 arasında, aynı birimde üç `int` kenar uzunluğu. |
| Çıktı | Geçerlilik veya tür | `Sonuç: Üçgen oluşmaz.`, `Sonuç: Eşkenar`, `Sonuç: İkizkenar` veya `Sonuç: Çeşitkenar`. |

`a kenarı (1..1000000): `, `b kenarı (1..1000000): ` ve `c kenarı (1..1000000): ` istemlerinde her kenarı ayrı satıra girin. Ondalık veya basamak ayırıcı kullanmayın. Biçimi veya aralığı yanlış olan ilk girdide `Hata: Kenar 1 ile 1000000 arasında bir tamsayı olmalıdır.` yazılır ve program sona erer.

## Algoritma

1. `a`, `b` ve `c` uzunluklarını sırayla okuyun; her birinin kabul edilen aralıkta bir tamsayı olduğunu doğrulayın.
2. `a + b > c`, `a + c > b` ve `b + c > a` koşullarının üçünün de sağlanıp sağlanmadığını hesaplayın.
3. Koşullardan en az biri sağlanmıyorsa üçgen oluşmadığını yazıp bitirin.
4. `a == b` ve `b == c` birlikte doğruysa eşkenar sonucunu yazdırın.
5. Değilse `a == b`, `a == c` veya `b == c` karşılaştırmalarından biri doğruysa ikizkenar sonucunu yazdırın.
6. Hiçbir eşitlik yoksa çeşitkenar sonucunu yazdırın.

`&&` işleci bütün koşulların birlikte doğru olmasını, `||` işleci en az bir koşulun doğru olmasını ister. Üçgen geçerliliği `&&`, iki kenarın eşitliğini bulma `||` gerektirir. Eşkenar kontrolünü önce yapmak, üç kenarı eşit bir girdinin ikizkenar dalına girmesini engeller.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("a kenarı (1..1000000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int a) || a < 1 || a > 1000000)
{
    Console.WriteLine("Hata: Kenar 1 ile 1000000 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("b kenarı (1..1000000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int b) || b < 1 || b > 1000000)
{
    Console.WriteLine("Hata: Kenar 1 ile 1000000 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("c kenarı (1..1000000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int c) || c < 1 || c > 1000000)
{
    Console.WriteLine("Hata: Kenar 1 ile 1000000 arasında bir tamsayı olmalıdır.");
    return;
}

bool ucgenOlur = a + b > c && a + c > b && b + c > a;
if (!ucgenOlur)
{
    Console.WriteLine("Sonuç: Üçgen oluşmaz.");
}
else if (a == b && b == c)
{
    Console.WriteLine("Sonuç: Eşkenar");
}
else if (a == b || a == c || b == c)
{
    Console.WriteLine("Sonuç: İkizkenar");
}
else
{
    Console.WriteLine("Sonuç: Çeşitkenar");
}
```

Girdi doğrulaması, üçgenin geometrik geçerliliğinden ayrıdır. `1`, `2`, `3` geçerli üç tamsayı girdisidir; üçgen oluşturmaması hata mesajı gerektirmez ve normal bir sonuç olarak bildirilir.

## Örnek çalıştırmalar

Sonuç sütunu, giriş istemlerinden sonra yazılan satırı gösterir. Girdiler `a`, `b`, `c` sırasındadır.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `3`, `3`, `3` | `Sonuç: Eşkenar` | Üç kenar eşittir. |
| `5`, `3`, `5` | `Sonuç: İkizkenar` | Birinci ve üçüncü kenar eşittir. |
| `3`, `4`, `5` | `Sonuç: Çeşitkenar` | Eşitsizlikler sağlanır, kenarlar farklıdır. |
| `2`, `3`, `5` | `Sonuç: Üçgen oluşmaz.` | Toplamın eşit olması yeterli değildir. |
| `8`, `2`, `3` | `Sonuç: Üçgen oluşmaz.` | En uzun kenar ilk sırada olsa da kontrol edilir. |
| `1`, `1`, `1` | `Sonuç: Eşkenar` | Alt sınırdaki geçerli üçgen. |
| `1000000`, `1000000`, `1000000` | `Sonuç: Eşkenar` | Üst sınırdaki geçerli üçgen. |
| İlk kenar `0` | `Hata: Kenar 1 ile 1000000 arasında bir tamsayı olmalıdır.` | Diğer kenarlar istenmez. |
| İlk kenar `2.5` | `Hata: Kenar 1 ile 1000000 arasında bir tamsayı olmalıdır.` | Ondalıklı girdi reddedilir. |

## Sınır durumları

- Sıfır veya negatif bir kenar uzunluğu geometrik sınıflandırmadan önce reddedilir.
- Toplam üçüncü kenara eşitse üçgen oluşmaz; `>` yerine `>=` kullanılmamalıdır.
- En uzun kenarın hangi sırada girildiği önemli değildir; üç eşitsizlik de denetlenir.
- İki kenarın toplamı en fazla 2000000 olur ve `int` aralığında kalır.
- Boş satır, metin, ondalıklı sayı veya aralık dışı değer ilk görüldüğü anda programı bitirir.

## Kazanımlar

- Üç ayrı gerekliliği `&&` ile tek bir mantıksal ifadede birleştirme.
- Birden çok eşitlik olasılığını `||` ile sınama.
- Karar dallarının sırasının sınıflandırmayı nasıl etkilediğini örnekle gösterme.
- Geçersiz girdi ile geçerli girdinin olumsuz sonucunu ayırt etme.

## Alıştırmalar

1. Kenarları farklı sıralarda girip sonucun neden değişmediğini üç örnekle gösterin.
2. İlk tür kontrolünü ikizkenar koşulu yaparsanız `3`, `3`, `3` girdisinin neden yanlış sınıflanacağını açıklayın.
3. Geçerli bir üçgen için çevreyi de hesaplayıp sonuç satırının ardından yazdırın.
