---
id: "078"
order: 78
title: "Özyinelemeli birleştirmeli sıralama"
level: "İleri"
prerequisites: ["055", "060", "074", "077"]
concepts: ["Böl ve yönet", "İki alt çağrı", "Paylaşılan çalışma tamponu", "Kararlı birleştirme", "Karşılaştırma sayacı"]
---

# Özyinelemeli birleştirmeli sıralama

## Problem tanımı

0..30 tamsayıyı birleştirmeli sıralamayla küçükten büyüğe sıralayın. `Sirala` metodu aralığı ikiye bölsün, iki alt aralığı özyinelemeli sıralasın ve `Birlestir` metodu bunları birleştirsin. Çalışma tamponu ana programda bir kez oluşturulsun; her çağrıda yeni dizi oluşturulmasın.

Sıralama boyunca iki eleman değeri arasında yapılan `<=` karşılaştırmalarını sayın. Döngü/dizin koşulları, veri kopyalama ve giriş doğrulama bu sayaca dahil değildir. Eşit değerlerde soldaki elemanı önce alarak kararlı birleştirme kurun. Yalnız tamsayı çıktısı eşit değerlerin kimliğini göstermez; kararlılık, eşitlikteki seçim kuralıyla açıklanmalıdır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, elemanlar | 0..30 uzunluk; tam `int` değerleri ayrı satırlarda. |
| Ara değer | `tampon` | Diziyle aynı kapasite; alt çağrılar kendi aralıklarını kullanır. |
| Çıktı | Sıralı dizi | `Dizi:` ardından değerler; boşsa `Dizi: Boş`. |
| Çıktı | Sayaç | `Karşılaştırma: C`; yalnız iki değer arasındaki karşılaştırmalar. |

İstemler `Eleman sayısı: `, sonra `A[0]: ` ile başlayan istemlerdir. EOF, geçersiz tamsayı veya uzunluk ihlalinde yalnız `Hata: Geçersiz giriş.` yazılır. İlk hata sonraki okumaları durdurur. Bütün elemanlar doğrulanmadan sıralama başlamaz. Sayısal giriş çevre boşluklarını ve işareti kabul eder.

## Algoritma

1. Diziyi okuyun; bir çalışma tamponu ve 0 karşılaştırma sayacı oluşturun.
2. `Sirala` içinde sıfır veya tek elemanlı aralıkta dönün.
3. Ortayı hesaplayıp sol ve sağ aralıkları sıralayın.
4. `Birlestir` içinde iki sıralı bölümün başlarını karşılaştırın; küçük olanı tampona alın.
5. Eşitlikte soldan alın; iki taraftan biri bitince ötekinin kalanını karşılaştırmasız kopyalayın.
6. Tamponun yalnız bu aralığını asıl diziye aktarın; sonuç ve sayacı yazdırın.

Alt çağrılar döndüğünde iki bölüm sıralıdır. Birleştirme her adımda kalanların en küçüğünü seçer; çıktı sıralı olur ve elemanlar kaybolmaz. Her çağrı daha kısa aralıklarla ilerler. Genel iş miktarı `n log n`, çalışma tamponu `n`, çağrı yığını derinliği `log n` ile orantılıdır. Bu bellek kullanımı, önceki yerinde sıralamaların farklı bir tercihidir.

`Sirala` sözleşmesi geçerli dahil uçları, aynı uzunluktaki tamponu ve başlangıç sayacını ister; boş aralık da kabul edilir. `Birlestir` iki bölümün zaten sıralı olduğu önkoşuluyla çağrılır. Tampondaki diğer bölümlere ait eski değerler okunmaz. Soldan ve sağdan alınan elemanların toplamı aralık uzunluğudur.

`4, 1, 3, 2` için birleştirmeler:

| Birleştirilen bölümler | Sonuç | Yeni karşılaştırma |
| --- | --- | --- |
| `4` ve `1` | `1, 4` | 1 |
| `3` ve `2` | `2, 3` | 1 |
| `1, 4` ve `2, 3` | `1, 2, 3, 4` | 3 |

## C# çözümü

```csharp
using System;
using System.Globalization;

if (!Oku("Eleman sayısı: ", out int n) || n < 0 || n > 30)
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}
int[] a = new int[n];
for (int i = 0; i < n; i++)
{
    string istem = "A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ";
    if (!Oku(istem, out a[i]))
    {
        Console.WriteLine("Hata: Geçersiz giriş.");
        return;
    }
}

int[] tampon = new int[n];
int karsilastirma = 0;
Sirala(a, tampon, 0, n - 1, ref karsilastirma);
Console.Write("Dizi:");
if (n == 0)
{
    Console.Write(" Boş");
}
for (int i = 0; i < n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Karşılaştırma: " +
    karsilastirma.ToString(CultureInfo.InvariantCulture));

static bool Oku(string istem, out int deger)
{
    Console.Write(istem);
    return int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out deger);
}

static void Sirala(int[] a, int[] tampon, int sol, int sag, ref int sayac)
{
    if (sol >= sag)
    {
        return;
    }

    int orta = sol + (sag - sol) / 2;
    Sirala(a, tampon, sol, orta, ref sayac);
    Sirala(a, tampon, orta + 1, sag, ref sayac);
    Birlestir(a, tampon, sol, orta, sag, ref sayac);
}

static void Birlestir(int[] a, int[] tampon,
    int sol, int orta, int sag, ref int sayac)
{
    int i = sol, j = orta + 1, yazma = sol;
    while (i <= orta && j <= sag)
    {
        sayac++;
        if (a[i] <= a[j])
        {
            tampon[yazma++] = a[i++];
        }
        else
        {
            tampon[yazma++] = a[j++];
        }
    }

    while (i <= orta)
    {
        tampon[yazma++] = a[i++];
    }
    while (j <= sag)
    {
        tampon[yazma++] = a[j++];
    }
    for (int k = sol; k <= sag; k++)
    {
        a[k] = tampon[k];
    }
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Tampon parametresi her çağrıda aynı diziyi gösterir; aralık parametreleri hangi hücrelerin işleneceğini sınırlar.

## Örnek çalıştırmalar

Önce `n`, sonra elemanları ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`n = 4`, dizi `4, 1, 3, 2`:

```text
Dizi: 1 2 3 4
Karşılaştırma: 5
```

`n = 3`, dizi `1, 1, 1`:

```text
Dizi: 1 1 1
Karşılaştırma: 3
```

`n = 0`:

```text
Dizi: Boş
Karşılaştırma: 0
```

`n = 1`, dizi `-2147483648`:

```text
Dizi: -2147483648
Karşılaştırma: 0
```

## Sınır durumları

- Sıfır ve tek elemanda birleştirme çağrılmaz.
- Tek uzunlukta iki alt bölüm eşit boyda olmayabilir; dahil uçlar kapsama boşluğu bırakmaz.
- Eşit değerler atılmaz; soldan önce alınır.
- Negatif değerler ve `int` uçları karşılaştırılır; eleman değerleriyle aritmetik yapılmaz.
- Kalan bölüm kopyalanırken karşılaştırma sayacı artırılmaz.
- Tampon her aralık için yeniden yazılır; önceki çağrıdan kalan hücreler sonucu etkilemez.

## Kazanımlar

- İki alt problemin dönüşünden sonra birleştirme yapma.
- Paylaşılan tamponu aralık sınırlarıyla güvenli kullanma.
- Birleştirme metodunun önkoşulunu özyinelemeyle sağlama.
- Veri değerleri karşılaştırmasını kopyalama ve dizin koşullarından ayırma.
- Kararlılık, ek bellek ve iş miktarı arasındaki tercihleri açıklama.

## Alıştırmalar

1. Azalan sıralama ekleyin; eşitlikte soldan alma kuralını koruyun.
2. Veri kopyalamalarını ayrı sayaçla ölçün; karşılaştırmayla karıştırmayın.
3. Elemanların özgün dizinlerini paralel dizide taşıyarak eşit değerlerin kararlılığını görünür kılın.
