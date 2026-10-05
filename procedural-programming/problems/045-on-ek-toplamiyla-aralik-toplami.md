---
id: "045"
order: 45
title: "Ön ek toplamıyla aralık toplamı hesaplama"
level: "İleri"
prerequisites: ["008", "010", "041", "044"]
concepts: ["Ön ek toplamı", "Ek dizi", "Dahil aralık", "Dizin dönüşümü", "long biriktirme", "Aralık sorgusu"]
---

# Ön ek toplamıyla aralık toplamı hesaplama

## Problem tanımı

1 ile 50 tamsayı içeren dizide, kullanıcının belirttiği başlangıç ve son dizin arasındaki elemanların toplamını bulun. İki sınır da toplama dahildir. Dizinler sıfırdan başlar; 0 ilk elemanı, N - 1 son elemanı gösterir. Başlangıç son dizinden büyük olamaz. Elemanlar tam `int` aralığındadır; toplam daha geniş `long` türünde tutulmalıdır.

Aralık toplamı için önce bir ön ek toplamı dizisi oluşturun. `onEk[j]`, asıl dizinin ilk J elemanının toplamıdır; bu bir adet bilgisidir. Bu nedenle `onEk[0] = 0`, `onEk[1] = sayilar[0]` ve `onEk[n]` bütün elemanların toplamıdır. Başlangıç B, son S olduğunda dahil aralığın toplamı `onEk[S + 1] - onEk[B]` olur. Böylece istenen bölgeden önceki elemanlar çıkarılır; son eleman ise dahil kalır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `sayilar` | N adet tam `int` eleman. |
| Girdi | `baslangic` | 0 ile N - 1 arasında dahil başlangıç dizini. |
| Girdi | `son` | Başlangıç ile N - 1 arasında dahil son dizin. |
| Ara değer | `onEk` | N + 1 uzunluklu `long` dizisi; ilk J elemanın toplamları. |
| Çıktı | `toplam` | `Toplam: X` biçiminde dahil aralık toplamı. |

Sırayla `Eleman sayısı (1..50): `, `Eleman[0]: ` ile başlayan N eleman istemi, `Başlangıç dizini: ` ve `Son dizin: ` yanıtlanır. Her tamsayı ayrı satırda girilir. Bütün elemanlar ve iki dizin doğrulanmadan sonuç yazılmaz. Örneğin beş elemanda başlangıç 1 ve son 3 ise 1, 2 ve 3. dizinler toplama katılır.

## Algoritma

1. N değerini doğrulayın; asıl diziyi oluşturup bütün elemanları doğrulayarak okuyun.
2. Başlangıç dizinini 0..N - 1 aralığında doğrulayın.
3. Son dizini başlangıç..N - 1 aralığında doğrulayın.
4. N + 1 uzunluğunda bir `long` ön ek dizisi oluşturun; başlangıç öğesi 0'dır.
5. I dizinini 0'dan N - 1'e kadar dolaşın; `onEk[I + 1] = onEk[I] + sayilar[I]` hesaplayın.
6. `onEk[son + 1] - onEk[baslangic]` ile aralık toplamını bulun.
7. Toplamı yazdırın.

Ön ek kurma döngüsü başında `onEk[i]`, işlenmiş ilk I elemanın toplamıdır. Sıradaki eleman eklendiğinde bir sonraki öğe ilk I + 1 elemanın toplamı olur. Ön ek dizisi bu nedenle asıl diziden bir öğe uzundur. `son + 1` konumu en fazla N olabilir ve ayrılan diziye sığar. Ön ek dizisi hazırlandıktan sonra aralık için yeniden eleman taraması gerekmez; iki ön ek değeri okunup çıkarılır. Bu örnek bir sorgu yapar; aynı ön ek dizisi daha sonra birden çok sorguda da kullanılabilir.

3, -2, 7, 4, -1 dizisinin ön ekleri:

| Ön ek dizini | Eklenen eleman | Ön ek toplamı |
| --- | --- | --- |
| 0 | Henüz eleman yok | 0 |
| 1 | `sayilar[0] = 3` | 3 |
| 2 | `sayilar[1] = -2` | 1 |
| 3 | `sayilar[2] = 7` | 8 |
| 4 | `sayilar[3] = 4` | 12 |
| 5 | `sayilar[4] = -1` | 11 |

Başlangıç 1, son 3 için `onEk[4] - onEk[1] = 12 - 3 = 9` olur. Doğrudan bölüm toplamı da -2 + 7 + 4 = 9'dur.

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

int[] sayilar = new int[n];
for (int i = 0; i < n; i++)
{
    Console.Write("Eleman[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: Eleman geçerli bir int tamsayı olmalıdır.");
        return;
    }
    sayilar[i] = deger;
}

Console.Write("Başlangıç dizini: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int baslangic) ||
    baslangic < 0 || baslangic >= n)
{
    Console.WriteLine("Hata: Başlangıç dizini dizi sınırları içinde olmalıdır.");
    return;
}

Console.Write("Son dizin: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int son) || son < baslangic || son >= n)
{
    Console.WriteLine("Hata: Son dizin başlangıç ile n-1 arasında olmalıdır.");
    return;
}

long[] onEk = new long[n + 1];
for (int i = 0; i < n; i++)
{
    onEk[i + 1] = onEk[i] + sayilar[i];
}

long toplam = onEk[son + 1] - onEk[baslangic];
Console.WriteLine("Toplam: " + toplam.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Yeni sayısal dizinin öğeleri başlangıçta 0 olduğundan `onEk[0]` ayrıca atanmasa da 0'dır. Toplama işleminde `onEk[i]` zaten `long` olduğu için `int` eleman bu türe genişletilerek eklenir; toplamı önce `int` içinde hesaplayıp sonradan dönüştürmek gerekmez. Asıl dizi değiştirilmez; ek dizi hesaplanmış ara bilgiyi taşır.

## Örnek çalıştırmalar

Girdi sırası N, N eleman, başlangıç ve son dizindir. Hepsi ayrı satırda verilir. Bloklar istemler dışındaki tam sonucu gösterir.

N = 5; elemanlar 3, -2, 7, 4, -1; başlangıç 1, son 3:

```text
Toplam: 9
```

N = 3; elemanlar -7, 0, 8; başlangıç 2, son 2:

```text
Toplam: 8
```

N = 2; elemanlar 2147483647, 2147483647; başlangıç 0, son 1:

```text
Toplam: 4294967294
```

N = 2; elemanlar -2147483648, -2147483648; başlangıç 0, son 1:

```text
Toplam: -4294967296
```

N = 1; eleman -2147483648; başlangıç 0, son 0:

```text
Toplam: -2147483648
```

Uzun dizilerde aşağıdaki özellikleri denetleyin; tablo tam eleman listesi değildir.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; bütün elemanlar 2147483647; başlangıç 0, son 49 | Toplam 107374182350 olur; `long` içinde kalır. |
| N = 50; bütün elemanlar -2147483648; başlangıç 0, son 49 | Toplam -107374182400 olur; `long` içinde kalır. |

N için 0, 51, -1, `2.5`, metin, boş satır veya giriş sonu verilirse:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

Herhangi bir elemanda `2147483648`, `-2147483649`, `3.5`, metin, boş satır veya giriş sonu verilirse dizinler istenmeden:

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

N = 3 ve elemanlar 1, 2, 3 sonrasında başlangıç için -1, 3, `1.5`, metin, boş satır veya giriş sonu verilirse son dizin istenmez:

```text
Hata: Başlangıç dizini dizi sınırları içinde olmalıdır.
```

Aynı dizide başlangıç 1 iken son için 0, 3, `2.5`, metin, boş satır veya giriş sonu verilirse:

```text
Hata: Son dizin başlangıç ile n-1 arasında olmalıdır.
```

## Sınır durumları

- Başlangıç ve son eşitse tek eleman toplanır; `onEk[B + 1] - onEk[B]` o elemanı verir.
- Başlangıç 0 ise çıkarılan ön ek 0'dır; son N - 1 ise okunan son ön ek `onEk[n]` olur.
- Sonun başlangıçtan küçük olması boş aralık gibi yorumlanmaz; bu çalışma hatalı aralığı reddeder.
- Negatif elemanlar ön ekleri küçültebilir; ön ek dizisinin artan olması gerekmez. Fark formülü yine geçerlidir.
- En fazla 50 tam `int` elemanın toplamı -107374182400 ile 107374182350 arasındadır. Ön ekler ve geçerli aralık toplamları `long` türüne sığar; `int` yeterli değildir.
- Dizi doldurulduktan sonra değişirse mevcut ön ek bilgisi güncelliğini kaybeder; yeni değerler için ön ekler yeniden hesaplanmalıdır.
- Herhangi bir yanlış biçim, tür taşması, boş veya eksik giriş ve geçersiz dizin sonuç yazılmadan reddedilir.

## Kazanımlar

- İlk J elemanın toplamını N + 1 uzunluklu ek dizide saklama.
- Asıl eleman dizini ile ön ekteki eleman adedinin farkını açıklama.
- İki ucu dahil aralık toplamını iki ön ek farkıyla hesaplama.
- İlk elemandan başlayan ve tek elemanlı aralıkları aynı formülle işleme.
- Tam `int` veride biriktiriciyi baştan `long` seçerek taşmayı önleme.
- Ön hesaplamanın birden çok aralık sorgusuna nasıl hizmet ettiğini açıklama.

## Alıştırmalar

1. Ön ek dizisini `Ön ek:` satırında 0. öğe dahil yazdırın; 3, -2, 7, 4, -1 için altı değeri izleme tablosuyla karşılaştırın.
2. Aynı dizi ve aynı ön ekler için üç farklı geçerli aralık okuyup toplamlarını yazdırın. Her sorgunun dizinlerini ayrıca doğrulayın.
3. Aynı aralığı bir döngüyle doğrudan toplayın; bulunan `long` sonuçla ön ek farkını karşılaştırın. Tek eleman ve bütün dizi durumlarını sınayın.
