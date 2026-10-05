---
id: "054"
order: 54
title: "Sayarak sıralama ile diziyi sıralama"
level: "İleri"
prerequisites: ["048", "053"]
concepts: ["Sayarak sıralama", "Sınırlı değer aralığı", "Frekans dizisi", "Değer ve dizin dönüşümü", "Sonuç konumu"]
---

# Sayarak sıralama ile diziyi sıralama

## Problem tanımı

-10 ile 10 arasında değerler içeren, uzunluğu 1..50 olan diziyi azalmayan sıraya getirin. Komşuları veya minimum adaylarını karşılaştırarak sıralamak yerine her değerin adedini bulun; değerleri küçükten büyüğe kendi adetleri kadar yeniden yazın. Örneğin `3, -10, 3, 0, -10` sonucu `-10, -10, 0, 3, 3` olur.

Değer aralığı bu problemin temel kısıtıdır: 21 olası değer için 21 sayaç yeterlidir. Veri değeri `x` için frekans dizini `x + 10`, frekans dizini J için değer `j - 10` olur. Sonuçta tekrarlar korunmalıdır. Önce verinin tamamını doğrulayın; sonra frekansları üretip giriş dizisinin hücrelerini sıralı değerlerle yeniden doldurun. Hazır sıralama kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1..50 arasında `int`. |
| Girdi | `a` | Her elemanı -10..10 arasında olan N değer. |
| Ara değer | `frekans` | 21 sayaç; J konumu J - 10 değerinin adedi. |
| Çıktı | Sıralı dizi | `Dizi:` ardından birer boşlukla bütün değerler. |

`Eleman sayısı (1..50): ` ve `A[0]: ` ile başlayan istemlerde her tamsayı ayrı satırda girilir. Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`; geçersiz elemanda `Hata: Eleman -10 ile 10 arasında bir tamsayı olmalıdır.` yazılır. Hata halinde `Dizi:` satırı üretilmez. Metin, ondalık, boş satır, EOF ve tür taşması reddedilir.

## Algoritma

1. Uzunluğu ve bütün elemanları doğrulayarak okuyun.
2. Sıfırla başlatılmış 21 elemanlı frekans dizisi oluşturun.
3. Her eleman için `frekans[a[i] + 10]` sayacını artırın.
4. Sonuç yazma konumunu 0 yapın.
5. Frekans dizinini 0'dan 20'ye ilerletin.
6. İlgili sayaç pozitifken `a[konum] = j - 10` yazın, konumu artırın ve sayacı azaltın.
7. Bütün sayaçlar tüketilince giriş dizisini yazdırın.

Frekansların başlangıç toplamı N'dir. Her yazım bu toplamı bir azaltıp sonuç konumunu bir artırır; yazılan ve kalan adetlerin toplamı N kalır. Bu yüzden tam N değer yazılır ve hiçbir yazım N. dizine ulaşmaz. Küçük frekans dizinleri küçük değerlere karşılık geldiği için sıralı başlangıç bölümü sürekli büyür. Her iç döngü sayacını azaltır; dış döngü 21 konumla sınırlıdır.

`3, -10, 3, 0, -10` için yalnız sıfır olmayan sayaçlar:

| Değer | Frekans dizini | Başlangıç adedi | Yeniden yazılan değerler |
| --- | --- | --- | --- |
| -10 | 0 | 2 | -10, -10 |
| 0 | 10 | 1 | 0 |
| 3 | 13 | 2 | 3, 3 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Eleman sayısı (1..50): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 50)
{
    Console.WriteLine("Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.");
    return;
}

int[] a = new int[n];
for (int i = 0; i < n; i++)
{
    Console.Write("A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger) ||
        deger < -10 || deger > 10)
    {
        Console.WriteLine("Hata: Eleman -10 ile 10 arasında " +
            "bir tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

int[] frekans = new int[21];
for (int i = 0; i < n; i++)
{
    frekans[a[i] + 10]++;
}

int konum = 0;
for (int j = 0; j < frekans.Length; j++)
{
    while (frekans[j] > 0)
    {
        a[konum] = j - 10;
        konum++;
        frekans[j]--;
    }
}

Console.Write("Dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Sayısal dizinin ilk sayaçları otomatik 0'dır. Sayma tamamlandıktan sonra A'nın eski elemanlarına ihtiyaç kalmaz; yeniden yazım onları güvenle değiştirebilir. Bu sürüm frekans sayaçlarını tüketir; işlem sonunda bütün sayaçlar 0 olur. Ek bellek olarak 21 elemanlı frekans dizisi kullanılır.

## Örnek çalıştırmalar

Önce N, ardından her eleman ayrı satırda girilir. Bloklar istemler dışındaki tam sonuçtur.

N = 5; değerler 3, -10, 3, 0, -10:

```text
Dizi: -10 -10 0 3 3
```

N = 1; değer 10:

```text
Dizi: 10
```

N = 4; değerler 0, 0, 0, 0:

```text
Dizi: 0 0 0 0
```

N = 3; değerler 10, 0, -10:

```text
Dizi: -10 0 10
```

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; 25 kez 10, ardından 25 kez -10 | Sonuç önce 25 kez -10, sonra 25 kez 10 içerir; toplam 50 değer vardır. |
| N = 21; değerler 10'dan -10'a doğru | Sonuç -10'dan 10'a kadar her değeri bir kez içerir. |

Uzunluk 0, 51, metin, ondalık, boş satır veya EOF ise tek çıktı:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

Herhangi bir elemanda -11, 11, `2147483647`, `2147483648`, metin, `1.5`, boş satır veya EOF için:

```text
Hata: Eleman -10 ile 10 arasında bir tamsayı olmalıdır.
```

## Sınır durumları

- -10 ve 10 frekans dizisinin sırasıyla 0 ve 20. dizinlerini kullanır.
- 0 normal bir veri değeridir; bitiş işareti değildir.
- Bütün değerler eşitse yalnızca bir frekans konumu kullanılır; N kopya yeniden yazılır.
- Her sayaç en fazla 50'dir; `int` türü yeterlidir.
- Frekansların toplamı N olduğu için sonuç dizisine tam N yazım yapılır; son yazım N - 1 konumundadır.
- Bu 21 konumlu yöntem tam `int` aralığına doğrudan uygulanamaz. -10..10 sınırı kaldırılırsa dönüşüm ve bellek planı da değiştirilmelidir.
- Hatalı veri frekans dizini olarak kullanılmaz; bütün girişler önce doğrulanır.

## Kazanımlar

- Bilinen dar değer aralığını bir sayaç dizisiyle temsil etme.
- Değerden dizine ve dizinden değere iki yönlü dönüşüm yapma.
- Frekans bilgisinden tekrarları koruyan sıralı dizi üretme.
- Sayaç azalmasıyla sonuç konumunun artışı arasındaki ilişkiyi kurma.
- Bir sıralama yönteminin hangi girdi kısıtına bağlı olduğunu açıklama.

## Alıştırmalar

1. Frekansları sondan başa tarayarak azalan sıralama üretin.
2. Frekansları tüketmeden bir tekrar sayacıyla yazın; sıralama sonrasında özgün adetleri de gösterin.
3. Değer aralığını -20..20 yapın; 41 konum, kaydırma ve ters dönüşümü birlikte güncelleyin.
