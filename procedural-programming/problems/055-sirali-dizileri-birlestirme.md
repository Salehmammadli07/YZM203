---
id: "055"
order: 55
title: "İki sıralı diziyi birleştirme"
level: "İleri"
prerequisites: ["046", "049", "054"]
concepts: ["Sıralı birleştirme", "İki okuma konumu", "Sonuç konumu", "Kalan bölüm", "Sıralılık önkoşulu"]
---

# İki sıralı diziyi birleştirme

## Problem tanımı

Her biri 1..25 eleman içeren iki azalmayan tamsayı dizisini tek bir azalmayan dizide birleştirin. Elemanlar tam `int` aralığındadır. Bütün tekrarlar korunur; bu işlem farklı ortak değerleri bulma değildir. Örneğin A `1, 3, 3`, B `-2, 3, 8` için sonuç `-2, 1, 3, 3, 3, 8` olur.

Her dizinin sıradaki kullanılmamış elemanını bir okuma diziniyle izleyin. Küçük olanı sonuç dizisine yazıp yalnız o dizinin konumunu ilerletin; eşitlikte A'dan alın. Bir dizi bitince diğerinin kalan bölümünü tamamlayın. Girişleri yeniden sıralamayın; sıralılık önkoşulunu doğrulayın. Hazır birleştirme veya kopyalama aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, `m` | A ve B uzunlukları; her biri 1..25. |
| Girdi | `a`, `b` | Tam `int` değerleri; her dizi kendi içinde azalmayan sırada. |
| Ara değer | `sonuc` | N + M uzunluğunda sonuç dizisi; en fazla 50 değer. |
| Çıktı | Birleşik dizi | `Birleşik dizi:` ardından birer boşlukla N + M değer. |

Girdi sırası `A uzunluğu (1..25): `, `B uzunluğu (1..25): `, bütün `A[0]: ` ile başlayan istemler, ardından bütün `B[0]: ` ile başlayan istemlerdir. Her değer ayrı satırda girilir. Bütün elemanlar okunduktan sonra önce A'nın, sonra B'nin sıralılığı doğrulanır. Geçersiz uzunluk, eleman veya sıralılık için yalnız ilgili `Hata:` satırı yazılır; birleşik dizi üretilmez. Boş satır, EOF, metin, ondalık veya `int` dışı giriş reddedilir.

## Algoritma

1. İki uzunluğu doğrulayın; dizileri oluşturup bütün elemanları doğrulayarak okuyun.
2. A ve B içindeki komşu çiftleri ayrı ayrı denetleyin; azalma varsa ilgili sıra hatasıyla bitirin.
3. N + M uzunluğunda sonuç dizisi oluşturun; iki okuma dizinini ve yazma konumunu 0 yapın.
4. İki dizide de eleman varken sıradaki elemanları karşılaştırın.
5. A'nınki küçük veya eşitse A'dan, aksi halde B'den alın; seçilen dizini ve yazma konumunu artırın.
6. A'da kalan bütün elemanları sonuç dizisine yazın.
7. B'de kalan bütün elemanları sonuç dizisine yazın.
8. Sonuç dizisini yazdırın.

Her adımda yazılmış bölüm sıralıdır ve iki diziden tüketilmiş bütün elemanları içerir. Her kaynak kendi içinde sıralı olduğundan kalanların en küçüğü iki sıradaki değerden biridir. Küçüğünü almak bu özelliği korur. Yazma konumu iki okuma konumunun toplamıdır; her yazım bir kaynağı tüketir. Bir kaynak bittiğinde ötekinin sıralı kalanı sonuçtan küçük değildir. Toplam N + M yazım yapılır; hiçbir kaynak elemanı atlanmaz veya iki kez tüketilmez.

A `1, 3, 3`, B `-2, 3, 8` için:

| Yazılan değer | Kaynak | Sonraki A dizini | Sonraki B dizini |
| --- | --- | --- | --- |
| -2 | B | 0 | 1 |
| 1 | A | 1 | 1 |
| 3 | A; eşitlik | 2 | 1 |
| 3 | A; eşitlik | 3 | 1 |
| 3, 8 | B'nin kalanı | 3 | 3 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("A uzunluğu (1..25): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 25)
{
    Console.WriteLine("Hata: A uzunluğu 1 ile 25 arasında bir tamsayı olmalıdır.");
    return;
}
Console.Write("B uzunluğu (1..25): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int m) || m < 1 || m > 25)
{
    Console.WriteLine("Hata: B uzunluğu 1 ile 25 arasında bir tamsayı olmalıdır.");
    return;
}

int[] a = new int[n];
int[] b = new int[m];
for (int i = 0; i < n; i++)
{
    Console.Write("A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: A elemanı int aralığında bir tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

for (int i = 0; i < m; i++)
{
    Console.Write("B[" + i.ToString(CultureInfo.InvariantCulture) + "]: ");
    if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: B elemanı int aralığında bir tamsayı olmalıdır.");
        return;
    }
    b[i] = deger;
}

for (int i = 1; i < n; i++)
{
    if (a[i - 1] > a[i])
    {
        Console.WriteLine("Hata: A dizisi azalmayan sırada olmalıdır.");
        return;
    }
}
for (int i = 1; i < m; i++)
{
    if (b[i - 1] > b[i])
    {
        Console.WriteLine("Hata: B dizisi azalmayan sırada olmalıdır.");
        return;
    }
}

int[] sonuc = new int[n + m];
int sol = 0, sag = 0, konum = 0;
while (sol < n && sag < m)
{
    if (a[sol] <= b[sag])
    {
        sonuc[konum++] = a[sol++];
    }
    else
    {
        sonuc[konum++] = b[sag++];
    }
}

while (sol < n)
{
    sonuc[konum++] = a[sol++];
}
while (sag < m)
{
    sonuc[konum++] = b[sag++];
}

Console.Write("Birleşik dizi:");
for (int i = 0; i < sonuc.Length; i++)
{
    Console.Write(" " + sonuc[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `konum++`, mevcut konumu kullanıp sonra artırır; kaynak dizinlerinde de aynı sıra geçerlidir. İki kaynak dizi değiştirilmez. Döngü koşulları eleman erişiminden önce sınırları doğrular; sonucun kapasitesi tam N + M'dir. Elemanlar üzerinde aritmetik yapılmadığı için `int` uçları güvenlidir.

## Örnek çalıştırmalar

N, M, bütün A elemanları ve bütün B elemanları ayrı satırlarda girilir. Bloklar istemler dışındaki tam çıktılardır.

N = 3, M = 3; A 1, 3, 3; B -2, 3, 8:

```text
Birleşik dizi: -2 1 3 3 3 8
```

N = 1, M = 1; A -2147483648; B 2147483647:

```text
Birleşik dizi: -2147483648 2147483647
```

N = 2, M = 3; A 7, 7; B 7, 7, 7:

```text
Birleşik dizi: 7 7 7 7 7
```

N = 2, M = 1; A -5, -1; B 4:

```text
Birleşik dizi: -5 -1 4
```

| Girdi | Beklenen özellik |
| --- | --- |
| N = M = 25; A 0..24, B 25..49 | Sonuç 50 değerle 0..49'dur; A bitince B'nin tamamı kopyalanır. |

Aşağıdaki durumlar tek hata satırı üretir; sonuç yazılmaz.

| Girdi | Tam sonuç |
| --- | --- |
| A uzunluğu 0, 26, metin, ondalık, boş veya EOF | `Hata: A uzunluğu 1 ile 25 arasında bir tamsayı olmalıdır.` |
| Geçerli A uzunluğundan sonra B uzunluğu 0, 26, metin, boş veya EOF | `Hata: B uzunluğu 1 ile 25 arasında bir tamsayı olmalıdır.` |
| N = M = 1; A elemanı `2147483648`, metin, ondalık, boş veya EOF | `Hata: A elemanı int aralığında bir tamsayı olmalıdır.` |
| N = M = 1; A 0; B elemanı `-2147483649`, metin, ondalık, boş veya EOF | `Hata: B elemanı int aralığında bir tamsayı olmalıdır.` |
| N = 2, M = 1; A 3, 2; B 0 | `Hata: A dizisi azalmayan sırada olmalıdır.` |
| N = 1, M = 2; A 0; B 3, 2 | `Hata: B dizisi azalmayan sırada olmalıdır.` |

## Sınır durumları

- İki uzunluk da en az 1'dir; boş kaynaklar bu çalışmanın giriş sınırı dışındadır.
- Eşit değerlerde A önce tüketilir; B'deki aynı değerler sonradan ayrıca yazılır.
- Kaynaklardan biri erken biterse diğerinin kalanı atlanmaz. İki tamamlama döngüsünden biri hiç çalışmayabilir.
- 0, negatif ve tam `int` uçları normal veridir; sıralılığı çıkarma yerine doğrudan karşılaştırma denetler.
- Sonuçta N + M eleman vardır; farklı değer sayısı hesaplanmaz.
- Başlangıçtan sonra gelen herhangi bir azalma önkoşulu bozar; sıralama yaparak hata gizlenmez.
- Geçersiz veya eksik girişte hiçbir birleşik sonuç üretilmez.

## Kazanımlar

- Sıralı veri önkoşulunu birleşim işleminden önce doğrulama.
- İki okuma dizinini ve bir yazma dizinini ayrı amaçlarla yönetme.
- Her adımda yalnız tüketilen kaynak dizinini artırma.
- Kaynaklar bittiğinde kalan bölümün tamamını yazma.
- Birleştirmede tekrarları koruma ve çıktı uzunluğunu kaynak uzunluklarından hesaplama.

## Alıştırmalar

1. Birleştirme aşamasındaki değer karşılaştırmalarını sayın; sıra doğrulama denetimlerini bu sayaca katmayın.
2. Eşitlikte B'yi seçin; tamsayı çıktısının aynı kaldığını ve kaynak seçim sırasının değiştiğini açıklayın.
3. Birleşik dizinin elemanlarını `long` ile toplayıp iki kaynak toplamının toplamıyla karşılaştırın.
