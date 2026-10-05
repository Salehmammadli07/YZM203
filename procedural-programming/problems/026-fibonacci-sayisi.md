---
id: "026"
order: 26
title: "Fibonacci dizisinin N numaralı terimini hesaplama"
level: "Orta"
prerequisites: ["004", "008", "025"]
concepts: ["Ardışık durum saklama", "For döngüsü", "Güncelleme sırası", "Sayısal taşma sınırı"]
---

# Fibonacci dizisinin N numaralı terimini hesaplama

## Problem tanımı

Fibonacci dizisinin kullanıcı tarafından belirtilen numaradaki terimini hesaplayın. Bu çalışmada numaralandırma sıfırdan başlar: `F(0) = 0`, `F(1) = 1`. Sonraki her terim önceki iki terimin toplamıdır: `F(n) = F(n - 1) + F(n - 2)`. İlk terimler `0, 1, 1, 2, 3, 5, 8` olur; örneğin 6 numaralı terim 8'dir.

Bütün diziyi bir dizide saklamak yerine yalnızca önceki ve güncel terimi tutun. Yeni terim hesaplandıktan sonra bu iki değişkenin görevini ilerletin. Amaç, birbirine bağlı iki durumun döngü içinde hangi sırada güncellenmesi gerektiğini öğrenmektir. Sonuç `long` aralığında kalmalıdır; kabul edilen en büyük terim numarası 92'dir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 0 ile 92 arasında `int`; istenen terimin sıfırdan başlayan numarası. |
| Çıktı | `sonuc` | `long` türünde Fibonacci terimi; `F(n): değer` biçiminde. |

`Terim numarası (0..92): ` istemine tek tamsayı girin. Sayının içinde basamak ayırıcı kullanmayın. Örneğin 10 girildiğinde sonuç satırı `F(10): 55` olur. Geçersiz girişte `Hata: Terim numarası 0 ile 92 arasında bir tamsayı olmalıdır.` yazılır.

## Algoritma

1. Terim numarasını okuyup 0 ile 92 arasında bir tamsayı olduğunu doğrulayın.
2. Önceki terimi 0, güncel terimi 1 yapın.
3. `n = 0` ise sonucu 0 kabul edin; döngü çalıştırmayın.
4. Diğer durumlarda sayacı 2'den başlatın ve `n` dahil olacak şekilde artırın.
5. Her adımda yeni terimi önceki terim ile güncel terimin toplamı olarak hesaplayın.
6. Önceki terime güncel terimi, sonra güncel terime yeni terimi atayın.
7. Döngü bitince güncel terimi sonuç olarak seçip terim numarasıyla birlikte yazdırın.

`i` numaralı terimi hesaplamadan hemen önce `onceki = F(i - 2)` ve `guncel = F(i - 1)` olur. Toplam ayrı bir değişkende tutulduğu için eski değerler kaybolmaz. Gövde sonunda bu iki değişken sırasıyla `F(i - 1)` ve `F(i)` olur. Sayaç her adımda artar; `n` sabit ve en fazla 92 olduğu için döngü sonlanır. `n = 1` durumunda döngü çalışmaz ve başlangıç güncel terimi olan 1 korunur.

| İşlenen terim | Adım başında önceki | Adım başında güncel | Yeni terim | Adım sonunda önceki / güncel |
| --- | --- | --- | --- | --- |
| 2 | 0 | 1 | 1 | 1 / 1 |
| 3 | 1 | 1 | 2 | 1 / 2 |
| 4 | 1 | 2 | 3 | 2 / 3 |
| 5 | 2 | 3 | 5 | 3 / 5 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Terim numarası (0..92): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 0 || n > 92)
{
    Console.WriteLine("Hata: Terim numarası 0 ile 92 arasında bir tamsayı olmalıdır.");
    return;
}

long onceki = 0;
long guncel = 1;
long sonuc;
if (n == 0)
{
    sonuc = 0;
}
else
{
    for (int i = 2; i <= n; i++)
    {
        long yeni = onceki + guncel;
        onceki = guncel;
        guncel = yeni;
    }
    sonuc = guncel;
}

Console.WriteLine("F(" + n.ToString(CultureInfo.InvariantCulture) + "): " +
    sonuc.ToString(CultureInfo.InvariantCulture));
```

`yeni` hesaplanmadan `onceki = guncel` yapılırsa toplama katılacak eski terim kaybolur. Döngü yalnızca istenen terime kadar ilerler. 92 numaralı terim hesaplandıktan sonra kullanılmayacak bir 93 numaralı terim hesaplanmaz; bu ek toplama `long` sınırını aşardı.

## Örnek çalıştırmalar

İstem metni gösterilmez; sonuç sütunu programın ürettiği tam sonuç veya hata satırıdır.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `0` | `F(0): 0` | Ayrı ele alınan ilk terim. |
| `1` | `F(1): 1` | Döngü çalışmadan başlangıç terimi alınır. |
| `2` | `F(2): 1` | Tek toplama yapılır. |
| `6` | `F(6): 8` | Birkaç ardışık durum güncellemesi. |
| `10` | `F(10): 55` | Normal örnek. |
| `92` | `F(92): 7540113804746346429` | En büyük kabul edilen terim. |
| `93` | `Hata: Terim numarası 0 ile 92 arasında bir tamsayı olmalıdır.` | Taşabilecek terim kabul edilmez. |
| `-1` | `Hata: Terim numarası 0 ile 92 arasında bir tamsayı olmalıdır.` | Negatif numara reddedilir. |
| `iki` | `Hata: Terim numarası 0 ile 92 arasında bir tamsayı olmalıdır.` | Tamsayı olmayan giriş. |
| `2.5` | `Hata: Terim numarası 0 ile 92 arasında bir tamsayı olmalıdır.` | Kesirli numara reddedilir. |

## Sınır durumları

- Sıfır numaralı terim, güncel değişkeninin başlangıç değeri olan 1'den ayrı ele alınır.
- 1 numaralı terimde sayaç 2'den başladığı için gövde hiç çalışmaz.
- `F(92) = 7540113804746346429`, `long` üst sınırı olan `9223372036854775807` değerinden küçüktür.
- `F(93) = 12200160415121876738` aynı aralığa sığmaz. Girdi sınırı hesaplama başlamadan denetlenir.
- Boş giriş, ondalık sayı, metin ve 0..92 dışındaki tamsayılar tek hata satırıyla reddedilir.
- Dizi oluşturulmaz; saklanan ara değerlerin sayısı terim numarasıyla artmaz.

## Kazanımlar

- `F(0)` ve `F(1)` taban durumlarını döngü başlangıcıyla birlikte açıklama.
- Ardışık iki terimi saklayarak bir sonraki terimi hesaplama.
- `n = 5` için her adımdaki önceki, güncel ve yeni değerleri tabloya yazma.
- Yanlış atama sırasının hangi eski değeri kaybettirdiğini gösterme.
- Kullanılmayacak bir sonraki terimin neden hesaplanmaması gerektiğini sayısal sınırla açıklama.

## Alıştırmalar

1. 0 ile `n` arasındaki bütün Fibonacci terimlerini sırayla yazdırın; `n = 0` için yalnızca 0 yazıldığını kontrol edin.
2. Başlangıcı `F(0) = 2`, `F(1) = 1` olan benzer bir dizi için çözümü uyarlayın; girdi üst sınırını yeniden hesaplayın.
3. Döngüyü `while` ile yazın ve 1, 2, 6 girdilerinde iki sürümün aynı sonuç verdiğini ara değerlerle doğrulayın.
