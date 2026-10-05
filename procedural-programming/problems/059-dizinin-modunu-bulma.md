---
id: "059"
order: 59
title: "Dizinin modunu sıralama ile bulma"
level: "İleri"
prerequisites: ["048", "051", "058"]
concepts: ["Mod", "Sıralama", "Ardışık eşit değer grupları", "Grup sınırı", "Eşit sıklık kuralı"]
---

# Dizinin modunu sıralama ile bulma

## Problem tanımı

1..50 elemanlı bir tamsayı dizisinin en sık görülen değerini, yani modunu bulun. Aynı en yüksek adede sahip birden çok değer varsa en küçük değeri seçin. Örneğin `3, 1, 3, 1, 2` için hem 1 hem 3 iki kez bulunur; sonuç mod 1, frekans 2'dir. Bütün değerler farklıysa bu çalışmanın eşitlik kuralına göre en küçük değer seçilir ve frekansı 1 olur.

Girdi sırasız olabilir; bütün elemanlar tam `int` aralığındadır. 048'deki 21 konumlu frekans dizisi bu alanı kapsamaz. Önce seçmeli sıralamayla aynı diziyi sıralayın; ardından ardışık eşit değer gruplarının uzunluklarını sayın. Hazır sıralama veya gruplama kullanmayın. Sonuç bu problemde tanımlanan eşitlik kuralına göre tek değer olarak verilir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1..50 arasında eleman sayısı. |
| Girdi | `a` | N adet tam `int`; tekrarlar olabilir. |
| Çıktı | Mod | `Mod: X`; en sık değer, eşit sıklıkta en küçük. |
| Çıktı | Frekans | `Frekans: Y`; modun kaç kez geçtiği. |

`Eleman sayısı (1..50): ` ve `A[0]: ` ile başlayan istemler ayrı satırlarda yanıtlanır. Bütün elemanlar doğrulanmadan işlem başlamaz. Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.` yazılır. Metin, ondalık, tür taşması, boş ve EOF ilgili hatayı üretir; mod ve frekans satırları yazılmaz.

## Algoritma

1. Uzunluğu ve bütün elemanları doğrulayarak okuyun.
2. Diziyi seçmeli sıralamayla azalmayan sıraya getirin.
3. Mod adayını ilk değer, en yüksek adedi 0 ve tarama dizinini 0 yapın.
4. Tarama bitmediyse mevcut değeri saklayıp grup adedini 0 yapın.
5. Dizin sınır içinde ve eleman saklanan değere eşitken grup adedini ve dizini artırın.
6. Grup adedi kayıtlı en yüksek adetten büyükse mod ve adedi güncelleyin; eşitse değiştirmeyin.
7. Tarama sürüyorsa 4. adıma dönün; bütün gruplar bittiğinde mod ve frekansı yazdırın.

`3, 1, 3, 1, 2` sıralanınca `1, 1, 2, 3, 3` olur:

| Grup değeri | Grup adedi | Sonraki dizin | Kayıtlı mod / frekans |
| --- | --- | --- | --- |
| 1 | 2 | 2 | 1 / 2 |
| 2 | 1 | 3 | 1 / 2 |
| 3 | 2 | 5 | 1 / 2; eşitlikte değişmez. |

Her dış tur başında önceki gruplar tamamen sayılmıştır ve kayıtlı mod onların kuralı sağlayan değeridir. Sıralılık her farklı değeri tek bir ardışık gruba toplar. İç döngü yalnız o grubun elemanlarını tüketir; eşit olmayan ilk değeri tüketmez. Gruplar küçükten büyüğe geldiğinden yalnız kesin büyük adette güncellemek eşit sıklıkta küçük değeri korur. Tarama dizini hiçbir zaman geri gitmez; toplam N eleman tüketilince son grup da değerlendirilmiş olur.

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

int mod = a[0], enYuksekAdet = 0, dizin = 0;
while (dizin < n)
{
    int deger = a[dizin], adet = 0;
    while (dizin < n && a[dizin] == deger)
    {
        adet++;
        dizin++;
    }
    if (adet > enYuksekAdet)
    {
        mod = deger;
        enYuksekAdet = adet;
    }
}

Console.WriteLine("Mod: " + mod.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Frekans: " +
    enYuksekAdet.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. İç koşul dizin denetimini önce yapar; son grubun ardından `a[n]` okunmaz. Mod gerçek bir elemanla başlatılır; 0 gibi dizide bulunmayabilecek bir aday kullanılmaz. En yüksek adedin 0 olması ilk grubun mutlaka kayda girmesini sağlar.

## Örnek çalıştırmalar

N ve N eleman ayrı satırlarda girilir. Bloklar istemler dışındaki tam çıktılardır.

N = 5; dizi 3, 1, 3, 1, 2:

```text
Mod: 1
Frekans: 2
```

N = 3; dizi 3, 1, 2:

```text
Mod: 1
Frekans: 1
```

N = 4; dizi -2147483648, 2147483647, 2147483647, 0:

```text
Mod: 2147483647
Frekans: 2
```

N = 1; tek eleman -8:

```text
Mod: -8
Frekans: 1
```

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; bütün değerler -2147483648 | Bir grup vardır; mod -2147483648, frekans 50 olur. |
| N = 50; 25 kez 2147483647 ve 25 kez -2147483648 | Eşit sıklıkta mod -2147483648, frekans 25 olur. |

Uzunluk 0, 51, metin, ondalık, boş veya EOF ise:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

Herhangi bir elemanda `2147483648`, `-2147483649`, metin, ondalık, boş veya EOF için:

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- Tek eleman bir gruptur; mod kendisi, frekans 1 olur.
- Bütün elemanlar eşitse iç döngü bütün diziyi tüketir; dış döngü bir tur çalışır.
- Bütün değerler farklıysa bu çalışmanın kuralıyla en küçük değer seçilir. Birden çok mod tanımı yerine tek sonuç kuralı uygulanır.
- Son grubun adedi, iç döngü sınır nedeniyle bittiğinde de değerlendirilir.
- En yüksek frekans en fazla 50'dir; sayaçlar `int` türünde güvenlidir.
- Tam `int` aralığı için geniş bir frekans dizisi ayrılmaz; sıralama ve gruplama kullanılır.
- Eksik veya hatalı girişte sonuç verilmez; bütün veriler önce doğrulanır.

## Kazanımlar

- Sıralılığı aynı değerleri komşu gruplara toplamak için kullanma.
- Bir grubun bittiği ilk farklı elemanı sonraki gruba bırakma.
- Her grupta adedi yeniden başlatırken tarama dizinini sürdürme.
- Eşit sıklık kuralını sıralama yönü ve kesin karşılaştırmayla uygulama.
- Sınırlı alan frekans dizisiyle tam alan sıralama yaklaşımını karşılaştırma.

## Alıştırmalar

1. En yüksek frekansa sahip bütün değerleri yazdırın; ilk taramada frekansı bulup ikinci taramada eşit grupları seçin.
2. Eşit sıklıkta en büyük değeri seçen sürümü yazın; güncelleme koşulunu açıklayın.
3. Her farklı değer için grup adedini yazdırın; toplamlarının N olduğunu ve son grubun da sayıldığını doğrulayın.
