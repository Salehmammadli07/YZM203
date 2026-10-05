---
id: "053"
order: 53
title: "Eklemeli sıralama ile diziyi sıralama"
level: "İleri"
prerequisites: ["044", "051", "052"]
concepts: ["Eklemeli sıralama", "Sıralı başlangıç bölümü", "Anahtarı saklama", "Geriye kaydırma", "Değer karşılaştırması"]
---

# Eklemeli sıralama ile diziyi sıralama

## Problem tanımı

Tam `int` aralığında 1 ile 50 eleman içeren diziyi eklemeli sıralama ile azalmayan sıraya getirin. İlk elemanı sıralı bir bölüm kabul edin. Her sonraki elemanı anahtar olarak saklayıp soldaki sıralı bölümde doğru yerine ekleyin. Anahtardan büyük değerleri bir konum sağa kaydırın; eşit değerleri kaydırmayın. Örneğin `3, 1, 2, 1` dizisi `1, 1, 2, 3` olur; tekrarlar korunur.

İki sayaç tutun. Karşılaştırma, mevcut sol eleman ile anahtar arasındaki `a[j] <= anahtar` değerlendirmesidir; aramayı bitiren doğru değerlendirme de sayılır. `j >= 0` dizin denetimi sayılmaz. Kaydırma, eski bir elemanın `a[j + 1] = a[j]` ile sağa taşınmasıdır. Anahtarın son konumuna yazılması kaydırma değildir. Hazır sıralama veya ekleme aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1..50 aralığında `int`; eleman sayısı. |
| Girdi | `a` | Tam `int` aralığında N eleman; sırasız olabilir. |
| Çıktı | Sıralı dizi | `Dizi:` ardından her değer için bir boşluk ve değer. |
| Çıktı | Sayaçlar | `Karşılaştırma: X`, ardından `Kaydırma: Y`. |

Önce `Eleman sayısı (1..50): `, sonra `A[0]: ` ile başlayan istemlerde her değeri ayrı satıra girin. Bütün elemanlar doğrulanmadan işlem başlamaz. Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, geçersiz elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.` yazılır. Boş satır ve giriş sonu (EOF) ilgili alanın hatasını üretir; kısmi dizi veya sayaç yazılmaz.

## Algoritma

1. Uzunluğu ve bütün elemanları okuyup doğrulayın; iki sayacı 0 yapın.
2. `i` dizinini 1'den N - 1'e ilerletin; `a[i]` değerini anahtarda saklayın.
3. `j = i - 1` ile soldaki sıralı bölümün sonundan başlayın.
4. `j >= 0` ise değer karşılaştırması sayacını artırın.
5. Sol eleman anahtardan küçük veya eşitse aramayı bitirin.
6. Aksi halde sol elemanı sağa kaydırın, kaydırma sayacını artırın ve J'yi azaltın; 4. adıma dönün.
7. Anahtarı `a[j + 1]` konumuna yazın; sonraki anahtara geçin.
8. Sonunda diziyi ve sayaçları yazdırın.

Dış tur başında ilk I eleman sıralıdır. Büyük değerleri sağa kaydırmak bu değerlerin sırasını korur ve anahtar için bir yer açar. Anahtar önceden saklandığı için eski konumunun üzerine yazılması onu kaybettirmez. Eşit değerde durmak yeni anahtarı eski eşitlerin sonrasına yerleştirir. J her kaydırmada azalır; dış dizin sonlu uzunluk boyunca artar. Döngüler sonlanır ve sıralı bölüm her turda bir eleman büyür.

`3, 1, 2, 1` için sayaçlar birikimlidir:

| Anahtarın eski dizini | Anahtar | Tur sonundaki dizi | Karşılaştırma | Kaydırma |
| --- | --- | --- | --- | --- |
| 1 | 1 | 1, 3, 2, 1 | 1 | 1 |
| 2 | 2 | 1, 2, 3, 1 | 3 | 2 |
| 3 | 1 | 1, 1, 2, 3 | 6 | 4 |

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

int karsilastirma = 0, kaydirma = 0;
for (int i = 1; i < n; i++)
{
    int anahtar = a[i];
    int j = i - 1;
    while (j >= 0)
    {
        karsilastirma++;
        if (a[j] <= anahtar)
        {
            break;
        }
        a[j + 1] = a[j];
        kaydirma++;
        j--;
    }
    a[j + 1] = anahtar;
}

Console.Write("Dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Karşılaştırma: " +
    karsilastirma.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Kaydırma: " + kaydirma.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. J -1 olursa artık eleman erişimi yapılmaz; anahtar 0. konuma yazılır. Değer karşılaştırması çıkarma kullanmaz. Sıralama ikinci bir dizi gerektirmez; anahtar ayrı bir tamsayı değişkenidir.

## Örnek çalıştırmalar

Girdi sırası N, ardından N elemandır; her değer ayrı satırda girilir. Bloklar istemler dışındaki tam çıktılardır.

N = 4; dizi 3, 1, 2, 1:

```text
Dizi: 1 1 2 3
Karşılaştırma: 6
Kaydırma: 4
```

N = 4; dizi 1, 2, 3, 4:

```text
Dizi: 1 2 3 4
Karşılaştırma: 3
Kaydırma: 0
```

N = 4; dizi 4, 3, 2, 1:

```text
Dizi: 1 2 3 4
Karşılaştırma: 6
Kaydırma: 6
```

N = 1; tek değer -2147483648:

```text
Dizi: -2147483648
Karşılaştırma: 0
Kaydırma: 0
```

Aşağıdaki tablo büyük girdilerin tam çıktısı değildir.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; farklı değerler 50..1 sırasında | 1225 karşılaştırma ve 1225 kaydırma; sonuç 1..50. |
| N = 50; bütün değerler 2147483647 | 49 karşılaştırma, 0 kaydırma; 50 eşit değer korunur. |

N için 0, 51, metin, ondalık, boş satır veya EOF:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

N = 2; ilk eleman 7, ikinci eleman `iki` için aşağıdaki tek satır yazılır. Herhangi bir elemanda `2147483648`, `-2147483649`, `2.5`, boş satır veya EOF aynı hatayı verir.

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- Tek elemanda sıralama turu ve iki sayaç da 0'dır.
- Sıralı veya eşit elemanlı N > 1 dizisinde anahtar başına bir değer karşılaştırması, toplam N - 1 karşılaştırma yapılır; kaydırma olmaz.
- Anahtar önceki bütün değerlerden küçükse J -1'e iner. Geçersiz dizine erişmeden anahtar başa yazılır.
- Eşit değerler kaydırılmaz; eşitlerin özgün göreli sırası korunur.
- En fazla 1225 kaydırma ve 1225 karşılaştırma vardır; `int` sayaçlar yeterlidir.
- Negatif değerler ve iki `int` ucu karşılaştırma ve atamayla güvenle işlenir.
- Hatalı son eleman bile sıralama başlamadan reddedilir; kısmi sonuç yoktur.

## Kazanımlar

- Sıralı başlangıç bölümünü her turda bir eleman büyütme.
- Üzerine yazılacak değeri önceden saklama.
- Geriye ilerlerken dizi erişiminden önce dizin sınırını denetleme.
- Aramayı bitiren değer karşılaştırmasını da sayma.
- Kaydırma ile anahtarın son yerleştirme atamasını ayırma.

## Alıştırmalar

1. Her anahtarın ardından diziyi yazdırın; izleme tablosundaki üç durumu doğrulayın.
2. Karşılaştırmayı değiştirip azalan sıralama yapın; eşit anahtarları yine kaydırmadan yerleştirin.
3. 051 ve 052 ile aynı üç girdiyi deneyin. Değer karşılaştırması sayılarını karşılaştırın; takas ve kaydırmanın farklı hareketler olduğunu açıklayın.
