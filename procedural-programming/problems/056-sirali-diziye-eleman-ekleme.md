---
id: "056"
order: 56
title: "Sıralı diziye eleman ekleme"
level: "İleri"
prerequisites: ["044", "046", "055"]
concepts: ["Sıralı ekleme", "Ek kapasite", "Geriye kopyalama", "Eşit değerler", "Geçerli uzunluk"]
---

# Sıralı diziye eleman ekleme

## Problem tanımı

Azalmayan sıradaki bir tamsayı dizisine yeni bir tamsayı ekleyin ve sıralılığı koruyun. Başlangıçta 1 ile 49 arasında eleman bulunur. Yeni değer mevcut değerlerle eşitse bütün eşit değerlerin sonrasına yerleşmelidir. Örneğin `1, 2, 2, 4` dizisine 2 eklendiğinde sonuç `1, 2, 2, 2, 4`, ekleme dizini 3 olur. Dizinler sıfırdan başlar.

Başlangıçta kapasitesi `n + 1` olan tek bir dizi oluşturun; ilk `n` konumu giriş değerleriyle doldurun. Son konum ekleme için ayrılmış boş kapasitedir ve başlangıçta geçerli veri değildir. Dizinin sıralılığını doğrulayın. Sırasız veride hata verip yeni değeri istemeden bitirin. Geçerli veride ilk büyük değerin yerini bulun, sağdaki değerleri sondan başlayarak bir konum kaydırın ve yeni değeri açılan konuma yazın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 49 arasında `int`; başlangıçtaki geçerli eleman sayısı. |
| Girdi | `a` | Kapasitesi `n + 1` olan dizi; ilk `n` eleman tam `int` aralığında ve azalmayan sırada. |
| Girdi | `yeni` | -2147483648..2147483647 aralığında eklenecek `int`. |
| Çıktı | Yeni dizi | `Dizi:` ardından birer boşlukla ayrılmış `n + 1` değer. |
| Çıktı | Ekleme konumu | `Ekleme dizini: I`; yeni değerin sıfır tabanlı dizini. |

Önce `Eleman sayısı (1..49): ` istemini, ardından `A[0]: ` ile başlayan eleman istemlerini ayrı satırlarda yanıtlayın. Bütün elemanlar doğru biçimde okunduktan sonra sıralılık denetlenir. Geçerli dizide son istem `Yeni değer: ` olur. Hatalı uzunlukta, elemanda, sıralılıkta veya yeni değerde yalnızca ilgili hata satırı yazılır; sonuç dizisi üretilmez. Boş satır, girişin sonu (EOF), metin, ondalıklı giriş ve `int` taşması reddedilir.

## Algoritma

1. Başlangıç uzunluğunu okuyup 1..49 aralığında doğrulayın.
2. `n + 1` kapasiteli diziyi oluşturun; ilk `n` elemanı okuyup doğrulayın.
3. Komşuları karşılaştırın; bir azalma varsa sıra hatası yazıp bitirin.
4. Yeni değeri okuyup tam `int` aralığında doğrulayın.
5. Ekleme konumunu 0 yapın; mevcut değer yeni değerden küçük veya eşitken konumu ilerletin.
6. `i = n` konumundan başlayıp `i > konum` iken `a[i] = a[i - 1]` ile geriye doğru kopyalayın.
7. Yeni değeri `a[konum]` konumuna yazın.
8. Bütün `n + 1` değeri ve ekleme dizinini yazdırın.

Konum aramasında geçilen bütün değerler yeni değerden küçük veya eşittir. Durulduğunda ya ilk büyük değere ulaşılmıştır ya da bütün eski değerler geçilmiştir. Bu nedenle eşitlerin sonrasına ekleme yapılır. Kaydırma sağ uçtan başlar; bir konuma yazmadan önce o konumdaki eski değer daha sağa taşınmıştır. İleri yönde kopyalamak henüz taşınmamış değerleri ezebilir. Arama en fazla `n` adım ilerler; kaydırma sayacı her adımda azalır ve ekleme konumunda durur.

`1, 2, 2, 4` ve yeni değer 2 için arama konumu 3'tür. Kullanılmayan son konumun başlangıçtaki 0 değeri aramaya katılmaz.

| Aşama | İşlem | Fiziksel dizi içeriği |
| --- | --- | --- |
| Giriş tamamlandı | Son konum henüz geçerli değildir. | `1, 2, 2, 4, 0` |
| Geriye kopyalama | `a[4] = a[3]` | `1, 2, 2, 4, 4` |
| Ekleme | `a[3] = 2` | `1, 2, 2, 2, 4` |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Eleman sayısı (1..49): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 49)
{
    Console.WriteLine("Hata: Eleman sayısı 1 ile 49 arasında " +
        "bir tamsayı olmalıdır.");
    return;
}

int[] a = new int[n + 1];
for (int i = 0; i < n; i++)
{
    Console.Write("A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: Eleman int aralığında bir tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

for (int i = 1; i < n; i++)
{
    if (a[i - 1] > a[i])
    {
        Console.WriteLine("Hata: Dizi azalmayan sırada olmalıdır.");
        return;
    }
}

Console.Write("Yeni değer: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int yeni))
{
    Console.WriteLine("Hata: Yeni değer int aralığında bir tamsayı olmalıdır.");
    return;
}

int konum = 0;
while (konum < n && a[konum] <= yeni)
{
    konum++;
}

for (int i = n; i > konum; i--)
{
    a[i] = a[i - 1];
}
a[konum] = yeni;

Console.Write("Dizi:");
for (int i = 0; i <= n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Ekleme dizini: " +
    konum.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Fiziksel kapasite baştan ayrılmıştır; ekleme sırasında ikinci bir dizi oluşturulmaz. Sonuçta geçerli uzunluk `n + 1` olur. Değerler yalnızca karşılaştırılır ve kopyalanır; iki `int` ucu arasında fark hesaplanmaz.

## Örnek çalıştırmalar

Girdi sırası uzunluk, eski elemanlar ve yeni değerdir. Her girdi ayrı satırda verilir. Metin blokları istemlerden sonraki bütün sonuç satırlarını gösterir.

`n = 4`, dizi `1, 2, 2, 4`, yeni değer 2 için tam çıktı:

```text
Dizi: 1 2 2 2 4
Ekleme dizini: 3
```

`n = 1`, dizi `7`, yeni değer -3 için tam çıktı:

```text
Dizi: -3 7
Ekleme dizini: 0
```

`n = 2`, dizi `-2147483648, 2147483647`, yeni değer 2147483647 için tam çıktı:

```text
Dizi: -2147483648 2147483647 2147483647
Ekleme dizini: 2
```

`n = 3`, dizi `-5, -1, 0`, yeni değer 6 için tam çıktı:

```text
Dizi: -5 -1 0 6
Ekleme dizini: 3
```

Aşağıdaki tablo uzun çıktının tam eleman listesi değildir; denetlenecek özellikleri özetler.

| Girdi | Beklenen özellik |
| --- | --- |
| `n = 49`, elemanlar sırasıyla 0..48, yeni değer 25 | Sonuçta 50 değer vardır; 25 iki kez bulunur, sıra korunur ve ikinci satır `Ekleme dizini: 26` olur. |

Aşağıdaki örneklerde yalnızca ilgili tek hata satırı üretilir.

| Girdi | Tam sonuç |
| --- | --- |
| Uzunluk `0`, `50`, `iki`, `1.5` veya `2147483648` | `Hata: Eleman sayısı 1 ile 49 arasında bir tamsayı olmalıdır.` |
| Uzunluk için boş satır veya girişin sonu | `Hata: Eleman sayısı 1 ile 49 arasında bir tamsayı olmalıdır.` |
| `n = 2`, ilk eleman 7, ikinci eleman `sayı`, `2.5` veya girişin sonu | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman `2147483648`, `-2147483649` veya boş satır | `Hata: Eleman int aralığında bir tamsayı olmalıdır.` |
| `n = 2`, dizi `3, 2` | `Hata: Dizi azalmayan sırada olmalıdır.`; yeni değer istenmez. |
| `n = 1`, eleman 7, yeni değer `yeni`, `1.5` veya `2147483648` | `Hata: Yeni değer int aralığında bir tamsayı olmalıdır.` |
| `n = 1`, eleman 7, yeni değer için boş satır veya girişin sonu | `Hata: Yeni değer int aralığında bir tamsayı olmalıdır.` |

## Sınır durumları

- Yeni değer bütün eski değerlerden küçükse ekleme dizini 0 olur; bütün eski elemanlar kaydırılır.
- Yeni değer bütün eski değerlerden büyük veya eşitse dizin `n` olur; kaydırma döngüsü çalışmaz.
- Bütün eski değerler yeni değere eşitse yeni değer en sona eklenir.
- Tek eski eleman için en fazla bir kaydırma yapılır; komşu sıralılık döngüsü çalışmaz.
- Başlangıç uzunluğu 49 olduğunda fiziksel kapasite 50 olur. Uzunluk 50 bu problemin giriş sınırının dışındadır.
- 0 ve tam `int` uçları normal değerlerdir; kullanılmayan kapasitenin 0 içeriği eski eleman sayılmaz.
- Kaydırma `i > konum` koşuluyla yapıldığından `i - 1` negatif olmaz. Konum aramasında `konum < n` denetimi dizi erişiminden önce gelir.
- Bütün gerekli girişler doğru olmadan sonuç yazılmaz. Sıra hatası, yeni değer okunmadan belirlenir.

## Kazanımlar

- Fiziksel kapasite ile geçerli eleman sayısını ayırma.
- Eşit değerlerin sonrasındaki ekleme dizinini örnek üzerinde bulma.
- Geriye kopyalamanın henüz taşınmamış değerleri koruduğunu gösterme.
- Başa, ortaya ve sona eklemeyi aynı döngü sınırlarıyla gerçekleştirme.
- Sıralılık önkoşulunu kullanıcıdan yeni değer istemeden doğrulama.

## Alıştırmalar

1. Eşitlerin öncesine ekleyen sürümü yazın; `1, 2, 2, 4` ve 2 için yeni dizinin aynı, ekleme dizininin farklı olduğunu gösterin.
2. Gerçek kaydırma adedini sayın; aynı diziye baştan, ortadan ve sondan eklemede `n - konum` ile eşit olduğunu doğrulayın.
3. Başlangıçta iki boş konum ayırıp iki yeni değeri sırayla ekleyin; ikinci eklemede geçerli uzunluğun neden artırılması gerektiğini açıklayın.
