---
id: "058"
order: 58
title: "K numaralı farklı küçük değeri bulma"
level: "İleri"
prerequisites: ["050", "051", "057"]
concepts: ["Sıralama", "Farklı değer sırası", "Eşit komşular", "Bir tabanlı sıra", "Yetersiz farklı değer"]
---

# K numaralı farklı küçük değeri bulma

## Problem tanımı

Tam `int` aralığında 1..50 eleman içeren dizinin K numaralı farklı küçük değerini bulun. K sıra numarası 1'den başlar; tekrarlar yeni sıra oluşturmaz. Örneğin `7, 2, 7, -1, 2` dizisinin farklı değerleri küçükten büyüğe `-1, 2, 7` olur. K = 2 için sonuç 2, K = 4 için yeterli farklı değer olmadığı sonucu verilir.

Girdi sırasız olabilir; seçmeli sıralamayı döngülerle uygulayın. Sıralı dizide eşit değerler komşu olacağı için yeni bir farklı değere yalnız ilk konumda veya önceki değerden farklı bir değerde rastlanır. K, 1..N aralığında doğrulanır. Geçerli K farklı değer sayısından büyükse bu bir giriş hatası değildir; normal bir bulunamadı sonucu üretin. Hazır sıralama veya farklı değer çıkarma aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1..50 arasında eleman sayısı. |
| Girdi | `a` | N adet tam `int`; tekrarlar olabilir. |
| Girdi | `k` | 1..N arasında sıra numarası; sıfır tabanlı dizin değildir. |
| Çıktı | Sonuç | `Değer: X` ya da `Sonuç: Yeterli farklı değer yok`. |

Önce `Eleman sayısı (1..50): `, sonra `A[0]: ` ile başlayan N eleman istemi, en son `Sıra (1..n): ` yanıtlanır. Her giriş ayrı satırdadır; son istemdeki N daha önce girilen uzunluktur. Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.`, sırada `Hata: K 1 ile n arasında bir tamsayı olmalıdır.` yazılır. Hata halinde sonuç verilmez.

## Algoritma

1. Uzunluğu, bütün elemanları ve K değerini okuyup doğrulayın.
2. Seçmeli sıralamayla diziyi azalmayan sıraya getirin.
3. Farklı değer sırasını 0, bulunma durumunu yanlış yapın.
4. Sıralı diziyi baştan sona ilerletin.
5. İlk konumda veya değer önceki elemandan farklıysa sıra sayacını artırın.
6. Sayaç K'ye ulaştıysa değeri saklayın, bulunma durumunu doğru yapıp taramayı bitirin.
7. Bulunmuşsa değeri, aksi halde yetersiz farklı değer sonucunu yazdırın.

`7, 2, 7, -1, 2` sıralanınca `-1, 2, 2, 7, 7` olur. K = 2 için tarama:

| Dizin | Değer | Yeni farklı değer mi? | Sıra | İşlem |
| --- | --- | --- | --- | --- |
| 0 | -1 | Evet; ilk konum | 1 | Devam |
| 1 | 2 | Evet | 2 | Değeri kaydet; bitir |

Tarama başında sıra sayacı, incelenmiş bölümdeki farklı değer sayısıdır. Sıralılık, aynı değerin bütün kopyalarını yan yana getirir; komşu eşitleri atlamak her farklı değeri bir kez sayar. Sıra K'ye ulaştığında bu değer istenen küçük değerdir. Ulaşılmazsa dizide K kadar farklı değer yoktur. Sıralama ve tarama sonlu dizinlerle sınırlıdır.

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
        CultureInfo.InvariantCulture, out int deger))
    {
        Console.WriteLine("Hata: Eleman geçerli bir int tamsayı olmalıdır.");
        return;
    }
    a[i] = deger;
}

Console.Write("Sıra (1..n): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int k) || k < 1 || k > n)
{
    Console.WriteLine("Hata: K 1 ile n arasında bir tamsayı olmalıdır.");
    return;
}

for (int i = 0; i < n - 1; i++)
{
    int enKucuk = i;
    for (int j = i + 1; j < n; j++)
    {
        if (a[j] < a[enKucuk])
        {
            enKucuk = j;
        }
    }
    if (enKucuk != i)
    {
        int gecici = a[i];
        a[i] = a[enKucuk];
        a[enKucuk] = gecici;
    }
}

int sira = 0, sonuc = 0;
bool bulundu = false;
for (int i = 0; i < n; i++)
{
    if (i == 0 || a[i] != a[i - 1])
    {
        sira++;
        if (sira == k)
        {
            sonuc = a[i];
            bulundu = true;
            break;
        }
    }
}

if (bulundu)
{
    Console.WriteLine("Değer: " + sonuc.ToString(CultureInfo.InvariantCulture));
}
else
{
    Console.WriteLine("Sonuç: Yeterli farklı değer yok");
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `i == 0 || ...` kısa devre değerlendirmesi ilk konumda `a[-1]` erişimini önler. Sonuç değişkeninin başlangıçtaki 0 değeri bulunamadı işareti değildir; ayrı `bulundu` durumu kullanılır. Böylece 0 ve iki `int` ucu da normal sonuç olabilir.

## Örnek çalıştırmalar

N, N eleman ve K ayrı satırlarda girilir. Bloklar istemler dışındaki bütün çıktı satırlarıdır.

N = 5; dizi 7, 2, 7, -1, 2; K = 2:

```text
Değer: 2
```

Aynı dizi için K = 4:

```text
Sonuç: Yeterli farklı değer yok
```

N = 3; dizi 0, -2147483648, 2147483647; K = 2:

```text
Değer: 0
```

N = 1; dizi -2147483648; K = 1:

```text
Değer: -2147483648
```

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; değerler 49..0; K = 50 | 50 farklı değer vardır; `Değer: 49` yazılır. |
| N = 50; bütün değerler 7; K = 2 | K geçerlidir; normal yetersiz farklı değer sonucu verilir. |

Uzunluk 0, 51, biçimi yanlış, boş veya EOF ise:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

Herhangi bir elemanda metin, ondalık, `2147483648`, `-2147483649`, boş veya EOF verilirse K istenmeden:

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

N = 2 ve geçerli iki elemandan sonra K için 0, 3, `1.5`, metin, boş veya EOF:

```text
Hata: K 1 ile n arasında bir tamsayı olmalıdır.
```

## Sınır durumları

- K = 1 her zaman dizinin en küçük değerini verir; N en az 1'dir.
- Tekrarlanan değerler korunarak sıralanır, ancak sıra sayacını yalnız bir kez artırır.
- K = N yalnız bütün elemanlar farklıysa bulunabilir; aksi halde normal yetersizlik sonucu verilir.
- Tek elemanda K = 1'dir ve sıralama turu gerekmez.
- 0, negatif değerler ve iki `int` ucu bulunamadı işareti olarak kullanılmaz.
- K için hatalı biçim ve alan sınırı hatası, geçerli K için yetersiz farklı değer durumundan ayrıdır.
- Bütün girdiler doğrulanmadan sıralama ve sonuç üretimi başlamaz.

## Kazanımlar

- Bir tabanlı sıra numarası ile sıfır tabanlı dizini ayırt etme.
- Sıralı dizide eşit komşuları atlayarak farklı değerleri sayma.
- Değer alanından bir işaret seçmek yerine ayrı bulunma durumu kullanma.
- Geçersiz giriş ile geçerli fakat sonucu olmayan sorguyu ayırma.
- Önceki sıralama ve tekrar eleme becerilerini bir sıra sorgusunda birleştirme.

## Alıştırmalar

1. Tarama sonunda toplam farklı değer sayısını da yazdırın; erken çıkışın bu sayımı neden eksik bırakacağını açıklayın.
2. K numaralı farklı büyük değeri bulun; tekrarları sıra hesabına katmayın.
3. Tekrarları atlamadan K numaralı elemanı bulun; `7, 2, 7, -1, 2` için K = 3 sonucunun neden değiştiğini açıklayın.
