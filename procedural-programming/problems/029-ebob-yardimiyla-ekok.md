---
id: "029"
order: 29
title: "EBOB yardımıyla EKOK hesaplama"
level: "İleri"
prerequisites: ["025", "028"]
concepts: ["Algoritmaları birleştirme", "EKOK ve EBOB ilişkisi", "Girdi kopyası", "İşlem sırası", "Tür dönüşümü"]
---

# EBOB yardımıyla EKOK hesaplama

## Problem tanımı

İki pozitif tamsayının en küçük ortak katını (EKOK) bulun. EKOK, iki sayıya da kalansız bölünen pozitif tamsayıların en küçüğüdür. Örneğin 12 ve 18 için 36, her iki sayının da katıdır ve en küçük ortak kattır.

Ortak katları tek tek aramak yerine `EKOK(a, b) = (a / EBOB(a, b)) * b` ilişkisini kullanın. EBOB'u önceki problemdeki Öklid döngüsüyle hesaplayın. Döngü çalışma değerlerini değiştirdiği için asıl girdileri ayrıca koruyun. Önce bölme yapıp çarpmayı `long` türünde gerçekleştirmek, büyük girdilerde yanlış taşmış sonuç üretmeyi önler. Bu çalışmada sıfır ve negatif sayılar kabul edilmez.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `ilk` | 1 ile 2147483647 arasında ilk `int`. |
| Girdi | `ikinci` | 1 ile 2147483647 arasında ikinci `int`. |
| Çıktı | `ekok` | `long` türünde sonuç; `EKOK: değer` biçiminde. |

`İlk sayı (1..2147483647): ` ve `İkinci sayı (1..2147483647): ` istemlerine ayrı satırlarda tamsayı girin. Geçersiz bir alan için o alanın hata satırı yazılır; EKOK hesaplanmaz.

## Algoritma

1. İki girdiyi okuyup pozitif `int` olduklarını doğrulayın.
2. Girdileri değiştirmeden `a = ilk`, `b = ikinci` çalışma kopyalarını oluşturun.
3. `b != 0` olduğu sürece `kalan = a % b`, `a = b`, `b = kalan` güncellemelerini sırayla uygulayın.
4. Döngü bitince EBOB'u `a` olarak alın.
5. İlk girdiyi EBOB'a bölün; bu bölüm tam sayıdır.
6. Bölümü çarpmadan önce `long` türüne dönüştürüp ikinci girdiyi bununla çarpın.
7. EKOK'u yazdırın.

EBOB ortak çarpanların bir kopyasını temsil eder; iki sayıyı doğrudan çarpmak bu ortak kısmı iki kez içerir. EBOB'a bölmek fazladan kopyayı çıkarır. `ilk / ebob` bölümünde kalan yoktur. Öklid döngüsü her adımda ikinci çalışma değerini küçülttüğünden sonlanır; sonrasında yalnızca bir bölme ve bir çarpma yapılır.

Örneğin 12 ve 18 için EBOB 6'dır. Önce `12 / 6 = 2`, sonra `2 * 18 = 36` bulunur. İlk sayının döngü sonunda 6'ya dönüşmüş çalışma kopyası, 12 değerinin yerine çarpıma konulmamalıdır.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("İlk sayı (1..2147483647): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ilk) || ilk < 1)
{
    Console.WriteLine("Hata: İlk sayı 1 ile 2147483647 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("İkinci sayı (1..2147483647): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ikinci) || ikinci < 1)
{
    Console.WriteLine("Hata: İkinci sayı 1 ile 2147483647 arasında bir tamsayı olmalıdır.");
    return;
}

int a = ilk;
int b = ikinci;
while (b != 0)
{
    int kalan = a % b;
    a = b;
    b = kalan;
}

int ebob = a;
long ekok = (long)(ilk / ebob) * ikinci;
Console.WriteLine("EKOK: " + ekok.ToString(CultureInfo.InvariantCulture));
```

`(long)(ilk / ebob)` dönüşümü çarpımdan önce yapılır; diğer çarpan da çarpma sırasında `long` olarak değerlendirilir. Yalnızca sonucun saklandığı değişkeni `long` yapmak yeterli değildir: `long ekok = ilk * ikinci;` ifadesinin sağındaki iki `int` önce `int` olarak çarpılır ve sonuç atanmadan taşabilir.

## Örnek çalıştırmalar

Girdiler ilk ve ikinci sayı sırasındadır. İstem metinleri sonuç sütununa dahil değildir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `12`, `18` | `EKOK: 36` | EBOB 6 kullanılır. |
| `18`, `12` | `EKOK: 36` | Girdi sırası sonucu değiştirmez. |
| `7`, `9` | `EKOK: 63` | Aralarında asal girdiler doğrudan çarpılır. |
| `24`, `24` | `EKOK: 24` | Eşit sayılarda sonuç sayının kendisidir. |
| `1`, `1` | `EKOK: 1` | Her iki alt sınır. |
| `1`, `2147483647` | `EKOK: 2147483647` | Bir girdinin 1 olması. |
| `2147483647`, `2147483647` | `EKOK: 2147483647` | Büyük ortak bölen çarpımdan önce çıkarılır. |
| `2147483647`, `2147483646` | `EKOK: 4611686011984936962` | EBOB 1; sonuç `int` sınırını aşar. |
| İlk girdi `0` | `Hata: İlk sayı 1 ile 2147483647 arasında bir tamsayı olmalıdır.` | Pozitif girdi şartı. |
| `12`, `-3` | `Hata: İkinci sayı 1 ile 2147483647 arasında bir tamsayı olmalıdır.` | Negatif ikinci sayı reddedilir. |
| İlk girdi `2147483648` | `Hata: İlk sayı 1 ile 2147483647 arasında bir tamsayı olmalıdır.` | `int` sınırı aşılır. |
| `12`, `kat` | `Hata: İkinci sayı 1 ile 2147483647 arasında bir tamsayı olmalıdır.` | Metin girişi reddedilir. |

## Sınır durumları

- Sıfır veya negatif girdi reddedildiğinden bulunan EBOB en az 1'dir; EBOB'a bölmede sıfır kullanılmaz.
- Girdiler eşitse EKOK aynı değerdir; 1 ile başka sayının EKOK'u diğer sayıdır.
- `ilk / ebob` tamsayı bölmesi tamdır; EBOB, ilk sayının bölenidir.
- Öklid için kullanılan `a` ve `b` kopyaları değiştirilir; `ilk` ve `ikinci` korunur.
- En büyük olası çarpım `(2147483647)^2 = 4611686014132420609` olur. EKOK bu çarpımdan büyük olamaz; `long` üst sınırı `9223372036854775807` olduğundan kabul edilen girdilerde sonucun taşması mümkün değildir.
- Boş giriş, ondalık sayı, metin ve `int` dışındaki değerler alan doğrulamasında reddedilir.

## Kazanımlar

- Öklid döngüsünün sonucunu ikinci bir hesaplama adımında kullanma.
- Çalışma kopyalarıyla başlangıç girdilerini hesaplama boyunca koruma.
- EBOB ve EKOK ilişkisini 12 ile 18 üzerinden ara değerlerle açıklama.
- Çarpma öncesi tür dönüşümünün neden gerekli olduğunu büyük girdilerle gösterme.
- Girdi üst sınırından hareketle `long` sonuç aralığının yeterliliğini hesaplama.

## Alıştırmalar

1. EBOB'u ve EKOK'u iki ayrı sonuç satırında gösterin; 12 ve 18 için aradaki ilişkiyi doğrulayın.
2. 6, 8 ve 12 sayılarına ait üçlü EKOK'u iki aşamada hesaplayın. Ara EKOK'un `int` sınırını aşabileceği durumda çalışma türlerinin nasıl değiştirilmesi gerektiğini açıklayın.
3. Döngü kopyaları yerine doğrudan girdileri değiştiren hatalı bir sürümü 12 ve 18 ile izleyin; EKOK hesabında hangi bilginin kaybolduğunu gösterin.
