---
id: "072"
order: 72
title: "Metot zinciriyle kesri sadeleştirme"
level: "İleri"
prerequisites: ["028", "068", "071"]
concepts: ["Metot bileşimi", "Önkoşul", "İşaret normalleştirme", "Tür genişletme", "Kanonik gösterim"]
---

# Metot zinciriyle kesri sadeleştirme

## Problem tanımı

Tam `int` aralığındaki pay ve sıfır olmayan paydadan oluşan kesri sadeleştirin. Son payda pozitif, pay ile payda aralarında asal olmalıdır. Sıfır paylı her geçerli kesir `0/1` biçiminde yazılır. Kesri ondalıklı sayıya çevirmeyin; tamsayı değerini koruyun.

İşaret normalleştirmesinden sonra payın mutlak değerini `Mutlak` metoduyla, ortak böleni `Ebob` metoduyla bulun. İki metot da konsola erişmeden `long` döndürür. `int.MinValue` değerini önce `long` yapın: bu değerin pozitif büyüklüğü `int` aralığında değildir. Sonuç pay veya payda da `int` sınırını aşabilir; örneğin `-2147483648 / -1`, `2147483648/1` olur.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | Pay, payda | Ayrı satırlarda tam `int`; payda sıfır olamaz. |
| Metot | `Mutlak` | Bu problemde -2147483648..2147483648 değerlerinin büyüklüğü. |
| Metot | `Ebob` | `a >= 0`, `b > 0`; pozitif ortak bölen döndürür. |
| Çıktı | Kesir ve bölen | `Sade kesir: P/Q`, ardından `EBOB: G`. |

İstemler `Pay: ` ve `Payda: ` olur. Pay hatalıysa payda istenmez. Geçersiz tamsayı, EOF veya sıfır paydada yalnız `Hata: Pay ve payda int olmalı, payda sıfır olmamalıdır.` yazılır. Sayısal giriş çevre boşlukları ve işaret kabul eder. Çıktıda boşluksuz `P/Q` gösterimi ve kültürden bağımsız sayılar kullanılır.

## Algoritma

1. Pay ve paydayı doğrulayıp ayrı `long` değişkenlere aktarın.
2. Payda negatifse hem payın hem paydanın işaretini değiştirin.
3. `Mutlak(pay)` sonucunu pozitif payda ile `Ebob` metoduna verin.
4. EBOB içinde `(a, b)` çiftini `(b, a % b)` ile küçültün; `b == 0` olunca `a` döndürün.
5. Pay ve paydayı ortak bölenle bölüp kesri ve böleni yazdırın.

İki değeri aynı sıfır olmayan sayıyla bölmek kesrin değerini korur. EBOB bütün ortak çarpanları kaldırır. Sıfır payda dışlandığından `Ebob(0, b)` sonucu `b` olur ve bölme güvenlidir. `Mutlak` genel `long.MinValue` için sözleşme vermez; çağıran yalnız belirtilen dar aralıktaki değerleri gönderir.

`-18/-24` işaret normalleştirmesinden sonra `18/24` olur. EBOB çağrısı:

| Başlangıç `a` | Başlangıç `b` | Kalan |
| --- | --- | --- |
| 18 | 24 | 18 |
| 24 | 18 | 6 |
| 18 | 6 | 0 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Pay: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int girilenPay))
{
    Console.WriteLine("Hata: Pay ve payda int olmalı, " +
        "payda sıfır olmamalıdır.");
    return;
}
Console.Write("Payda: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int girilenPayda) || girilenPayda == 0)
{
    Console.WriteLine("Hata: Pay ve payda int olmalı, " +
        "payda sıfır olmamalıdır.");
    return;
}

long pay = girilenPay, payda = girilenPayda;
if (payda < 0)
{
    pay = -pay;
    payda = -payda;
}
long ortak = Ebob(Mutlak(pay), payda);
pay /= ortak;
payda /= ortak;
Console.WriteLine("Sade kesir: " +
    pay.ToString(CultureInfo.InvariantCulture) + "/" +
    payda.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("EBOB: " + ortak.ToString(CultureInfo.InvariantCulture));

static long Mutlak(long deger)
{
    return deger < 0 ? -deger : deger;
}

static long Ebob(long a, long b)
{
    while (b != 0)
    {
        long kalan = a % b;
        a = b;
        b = kalan;
    }
    return a;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `Mutlak(pay)` önce hesaplanır; dönen değer `Ebob` çağrısının ilk parametresi olur. Her metodun yerel değişkenleri kendi çağrısına aittir. `Ebob` içindeki atamalar ana programın `pay` ve `payda` değişkenlerini değiştirmez. Son kesir, dönen ortak bölen kullanılarak çağıranda oluşturulur.

## Örnek çalıştırmalar

Önce payı, sonra paydayı ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`pay = -18`, `payda = -24`:

```text
Sade kesir: 3/4
EBOB: 6
```

`pay = 0`, `payda = -7`:

```text
Sade kesir: 0/1
EBOB: 7
```

`pay = 2147483647`, `payda = -2147483648`:

```text
Sade kesir: -2147483647/2147483648
EBOB: 1
```

`pay = 5`, `payda = 0`:

```text
Hata: Pay ve payda int olmalı, payda sıfır olmamalıdır.
```

## Sınır durumları

- İki negatif işaret birbirini kaldırır; yalnız payın işareti negatif kalabilir.
- `int.MinValue` önce genişletilir; negatifini `int` içinde hesaplamak taşar.
- Payın büyüklüğü paydadan büyük olabilir; bileşik kesir geçerlidir.
- Aralarında asal değerlerde EBOB 1'dir; kesir zaten sade olabilir.
- `-2147483648/-2147483648` sonucu `1/1`, EBOB 2147483648 olur.
- İki girdinin de sıfır olması payda kuralıyla reddedilir.

## Kazanımlar

- Bir metodun dönüşünü başka metodun parametresi olarak kullanma.
- Metot önkoşullarını çağıran programda sağlama.
- Tür genişletmesini işaret değişiminden önce yapma.
- Sayısal değer korunurken tek bir standart gösterim üretme.
- Değer parametrelerinin çağıranın değişkenlerini değiştirmediğini gösterme.

## Alıştırmalar

1. Sade kesrin tamsayı ve kalan pay bölümünü ayrıca yazdırın; negatif pay kuralını tanımlayın.
2. Aynı kesri tekrar sadeleştirmenin neden sonucu değiştirmediğini açıklayın.
3. İki sade kesri toplamak için ortak payda hesabını metotlara ayırın; tür sınırlarını yeniden belirleyin.
