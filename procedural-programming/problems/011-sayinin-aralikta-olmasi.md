---
id: "011"
order: 11
title: "Sayının kapalı bir aralıkta olup olmadığını belirleme"
level: "Temel"
prerequisites: ["005", "006"]
concepts: ["Kapalı aralık", "Mantıksal VE", "Birleşik koşul", "Girdiler arası doğrulama"]
---

# Sayının kapalı bir aralıkta olup olmadığını belirleme

## Problem tanımı

Kullanıcıdan bir tamsayı, bir alt sınır ve bir üst sınır alın. Sayının bu iki sınırla tanımlanan kapalı aralıkta olup olmadığını belirleyin. Kapalı aralık, sınırların kendisini de içerir: alt sınır 3 ve üst sınır 8 ise 3 ve 8 de aralıktadır.

Her girdi -1000000 ile 1000000 arasında olmalıdır. Alt sınır üst sınırdan büyükse aralık geçersizdir; program sınırları kendiliğinden değiştirmek yerine hata verir. Alt ve üst sınırın eşit olması geçerlidir ve yalnızca tek sayıyı içeren bir aralık oluşturur.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `sayi` | Aralıkta olup olmadığı araştırılan tamsayı. |
| Girdi | `altSinir` | Aralığın dahil edilen alt sınırı. |
| Girdi | `ustSinir` | Aralığın dahil edilen üst sınırı. |
| Çıktı | Karar | `Sonuç: Aralıkta` veya `Sonuç: Aralık dışında`. |

`Sayı: `, `Alt sınır: ` ve `Üst sınır: ` istemlerine sırasıyla birer tamsayıyı ayrı satırda yazın. Basamak ayırıcı kullanmayın. Her tamsayı -1000000 ile 1000000 arasında olmalıdır. Bir girdinin biçimi veya aralığı geçersizse sonraki girdiler istenmeden program sona erer.

## Algoritma

1. Sayıyı okuyun; belirtilen aralıkta bir tamsayı değilse hata yazıp bitirin.
2. Alt sınırı okuyun ve aynı biçimde doğrulayın.
3. Üst sınırı okuyun ve aynı biçimde doğrulayın.
4. Alt sınır üst sınırdan büyükse hata yazıp bitirin.
5. `sayi >= altSinir` ve `sayi <= ustSinir` koşullarını `&&` ile birleştirin.
6. İki koşul da doğruysa `Sonuç: Aralıkta`, aksi durumda `Sonuç: Aralık dışında` yazdırın.

`&&` mantıksal VE işlecidir; birleşik koşulun doğru olması için iki karşılaştırmanın da doğru olması gerekir. Örneğin 2, üst sınır 8'den küçük olsa bile alt sınır 3'ten küçük olduğu için aralıkta değildir. `>=` ve `<=` kullanılması sınırların dahil edilmesini sağlar.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Sayı: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int sayi) ||
    sayi < -1000000 || sayi > 1000000)
{
    Console.WriteLine("Hata: Sayı -1000000 ile 1000000 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Alt sınır: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int altSinir) ||
    altSinir < -1000000 || altSinir > 1000000)
{
    Console.WriteLine("Hata: Alt sınır -1000000 ile 1000000 arasında " +
        "bir tamsayı olmalıdır.");
    return;
}

Console.Write("Üst sınır: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ustSinir) ||
    ustSinir < -1000000 || ustSinir > 1000000)
{
    Console.WriteLine("Hata: Üst sınır -1000000 ile 1000000 arasında " +
        "bir tamsayı olmalıdır.");
    return;
}

if (altSinir > ustSinir)
{
    Console.WriteLine("Hata: Alt sınır üst sınırdan büyük olamaz.");
    return;
}

bool aralikta = sayi >= altSinir && sayi <= ustSinir;
if (aralikta)
{
    Console.WriteLine("Sonuç: Aralıkta");
}
else
{
    Console.WriteLine("Sonuç: Aralık dışında");
}
```

Kod, bir konsol projesinin `Program.cs` dosyasına tek başına konabilir. `bool` değişkeni doğru veya yanlış değerini saklar. Girdi doğrulaması önce tek tek değerleri, sonra sınırların birbiriyle ilişkisini denetler. `return`, hata durumunda sonraki adımların çalışmasını engeller.

## Örnek çalıştırmalar

Tablo, istemleri hariç tutarak yazdırılan tam sonuç veya hata satırını gösterir. `—`, hata nedeniyle o girdinin henüz istenmediğini belirtir.

| Sayı | Alt sınır | Üst sınır | Sonuç satırı |
| --- | --- | --- | --- |
| `5` | `3` | `8` | `Sonuç: Aralıkta` |
| `3` | `3` | `8` | `Sonuç: Aralıkta` |
| `8` | `3` | `8` | `Sonuç: Aralıkta` |
| `2` | `3` | `8` | `Sonuç: Aralık dışında` |
| `9` | `3` | `8` | `Sonuç: Aralık dışında` |
| `-4` | `-4` | `-4` | `Sonuç: Aralıkta` |
| `0` | `-1000000` | `1000000` | `Sonuç: Aralıkta` |
| `5` | `8` | `3` | `Hata: Alt sınır üst sınırdan büyük olamaz.` |
| `2.5` | `—` | `—` | `Hata: Sayı -1000000 ile 1000000 arasında bir tamsayı olmalıdır.` |
| `0` | `-1000001` | `—` | `Hata: Alt sınır -1000000 ile 1000000 arasında bir tamsayı olmalıdır.` |
| `0` | `0` | `1000001` | `Hata: Üst sınır -1000000 ile 1000000 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- Alt veya üst sınıra eşit sayı aralıktadır; `>` ve `<` kullanılsaydı bu değerler dışarıda kalırdı.
- Eşit sınırlar geçerlidir. Sayı bu ortak değerden farklıysa aralık dışındadır.
- Negatif sınırlarda da aynı karşılaştırmalar kullanılır; ek bir işaret kontrolü gerekmez.
- Sınırların her biri geçerli bir sayı olsa bile alt sınırın üst sınırdan büyük olması ayrıca reddedilir.
- Boş satır, metin, ondalıklı sayı veya izin verilen aralık dışında bir değer için karar yazılmaz.
- Yalnızca karşılaştırma yapıldığı için toplama veya çarpma taşması oluşmaz.

## Kazanımlar

- Kapalı bir aralığı `>=`, `<=` ve `&&` ile doğru biçimde ifade etme.
- İki ayrı karşılaştırmanın sonucunu bir `bool` değişkeninde saklama.
- Tek bir girdinin doğrulanması ile iki girdi arasındaki ilişkinin doğrulanmasını ayırt etme.
- Alt sınır, üst sınır ve sınırların hemen dışındaki değerler için test sonuçlarını açıklama.

## Alıştırmalar

1. Sınırları içermeyen açık aralık için koşulu değiştirin; 3 ile 8 aralığında 3, 4 ve 8'in sonuçlarını karşılaştırın.
2. Aralık dışındaki sayılar için `Alt sınırın altında` ve `Üst sınırın üstünde` biçiminde iki farklı sonuç yazdırın.
3. Koşulda `&&` yerine `||` kullanılırsa alt sınır 3 ve üst sınır 8 iken 2 için neden yanlış sonuç çıkacağını açıklayın.
