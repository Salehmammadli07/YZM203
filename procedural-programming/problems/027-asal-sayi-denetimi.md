---
id: "027"
order: 27
title: "Bir sayının asal olup olmadığını belirleme"
level: "İleri"
prerequisites: ["006", "021", "026"]
concepts: ["Bölen arama", "Erken döngü çıkışı", "Boolean durum", "Taşmasız döngü sınırı"]
---

# Bir sayının asal olup olmadığını belirleme

## Problem tanımı

Sıfır veya pozitif bir tamsayının asal olup olmadığını belirleyin. Asal sayı, 1'den büyük ve yalnızca 1 ile kendisine tam bölünen tamsayıdır. 0 ve 1 asal değildir; bunlar geçerli girdilerdir. Örneğin 13 asal, 15 ise 3'e bölünebildiği için asal değildir.

Sayıyı 2'den başlayarak olası bölenlere bölün. Bir bölen bulunca aramayı sonlandırın. Her bileşik sayı, bir çarpan çiftine sahiptir; çiftin en az bir üyesi sayının karekökünü aşmaz. Bu nedenle sayıya kadar bütün adayları denemek gerekmez. Karekökü hazır bir işlevle hesaplamak yerine tamsayı bölmesiyle güvenli bir döngü sınırı kurun.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 0 ile 2147483647 arasında `int`. |
| Çıktı | Asallık durumu | `Sonuç: Asal` veya `Sonuç: Asal değil`. |

`Sayı (0..2147483647): ` istemine tek tamsayı girin. Geçersiz girişte `Hata: Sayı 0 ile 2147483647 arasında bir tamsayı olmalıdır.` yazılır; asallık sonucu verilmez.

## Algoritma

1. Sayıyı okuyup 0 ile 2147483647 arasında bir tamsayı olduğunu doğrulayın.
2. `asal` değişkenini `n >= 2` ifadesinin sonucuyla başlatın.
3. Asal olabilecek sayılarda bölen adayını 2'den başlatın.
4. `bolen <= n / bolen` olduğu sürece adayları deneyin.
5. `n % bolen == 0` ise sayının asal olmadığını kaydedip döngüden `break` ile çıkın.
6. Bölen bulunmadıysa adayı bir artırıp koşulu yeniden denetleyin.
7. `asal` durumuna göre sonuç satırını yazdırın.

Her adımın başında 2 ile `bolen - 1` arasındaki adayların hiçbirinin sayıyı tam bölmediği bilinir. `bolen <= n / bolen` koşulu, pozitif adaylar için `bolen * bolen <= n` sınırını tamsayı çarpımı yapmadan ifade eder. Örneğin 49 için 7 denenir; eşitlik korunmalıdır. Her başarısız denemede aday artar, üst sınır sınırlı kalır; bir bölen bulunması da aramayı hemen bitirir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Sayı (0..2147483647): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 0)
{
    Console.WriteLine("Hata: Sayı 0 ile 2147483647 arasında bir tamsayı olmalıdır.");
    return;
}

bool asal = n >= 2;
if (asal)
{
    for (int bolen = 2; bolen <= n / bolen; bolen++)
    {
        if (n % bolen == 0)
        {
            asal = false;
            break;
        }
    }
}

if (asal)
{
    Console.WriteLine("Sonuç: Asal");
}
else
{
    Console.WriteLine("Sonuç: Asal değil");
}
```

`int.TryParse`, `int` üst sınırını aşan girdileri reddeder; bu nedenle ayrıca 2147483647 ile karşılaştırmak gerekmez. `bolen` 2'den başladığı için bölmede sıfır kullanılmaz. `break`, döngüden çıkar; programın sonuç yazdıran bölümü yine çalışır.

## Örnek çalıştırmalar

İstem metni sonuç sütununa dahil değildir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `0` | `Sonuç: Asal değil` | Geçerli girdi; asal tanımının dışında. |
| `1` | `Sonuç: Asal değil` | 1 asal kabul edilmez. |
| `2` | `Sonuç: Asal` | En küçük asal, gövde çalışmaz. |
| `13` | `Sonuç: Asal` | 2 ve 3 bölen değildir. |
| `15` | `Sonuç: Asal değil` | 3 bulunduğunda arama biter. |
| `49` | `Sonuç: Asal değil` | Karekök sınırındaki 7 mutlaka denenir. |
| `2147483647` | `Sonuç: Asal` | En büyük `int` değeri asal sayıdır. |
| `2147483646` | `Sonuç: Asal değil` | İlk aday olan 2'ye bölünür. |
| `-1` | `Hata: Sayı 0 ile 2147483647 arasında bir tamsayı olmalıdır.` | Negatif sayı reddedilir. |
| `2147483648` | `Hata: Sayı 0 ile 2147483647 arasında bir tamsayı olmalıdır.` | `int` sınırı aşılır. |
| `asal` | `Hata: Sayı 0 ile 2147483647 arasında bir tamsayı olmalıdır.` | Metin girişi reddedilir. |

## Sınır durumları

- 0 ve 1 için döngüye girilmez; sonuç doğrudan asal değildir.
- 2 ve 3 için ilk sınır koşulu yanlıştır; bir bölen aranmadan doğru asal sonucu elde edilir.
- Bir asalın karesinde karekök adayı denenmelidir; `<` kullanılması 49'u yanlış sınıflandırabilir.
- Büyük `int` girdisinde `bolen * bolen` ifadesi taşabilir. Bölme kullanan koşul bu çarpımı yapmaz.
- Bir bölen bulunması yeterlidir; bütün bölenlerin çıkarılması bu problemde gerekli değildir.
- Boş giriş, kesirli sayı, negatif sayı ve `int` aralığını aşan metin tek hata satırıyla reddedilir.

## Kazanımlar

- Asal tanımını 0, 1 ve 2 örnekleri üzerinden doğru uygulama.
- Kalan işleciyle bir bölenin sayıyı tam bölüp bölmediğini belirleme.
- Bir çarpan çiftinden en az birinin karekök sınırında veya altında olduğunu açıklama.
- `break` kullanarak sonucu değiştirmeyecek sonraki denemeleri durdurma.
- Çarpma kullanan döngü sınırını taşmasız bölme koşuluna dönüştürme.

## Alıştırmalar

1. Asal olmayan, 1'den büyük sayılarda ilk bulunan böleni de yazdırın; 0 ve 1 için bölen mesajı üretmeyin.
2. Yapılan bölünebilirlik denemelerini sayın; 2, 49 ve 97 için sayaç değerlerini adım adım çıkarın.
3. 2'yi ayrı denedikten sonra yalnızca tek adayları deneyecek biçimde çözümü değiştirin; 2 ve çift sayılardaki davranışı koruyun.
