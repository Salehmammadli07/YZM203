---
id: "060"
order: 60
title: "İki sıralama algoritmasını karşılaştırma"
level: "İleri"
prerequisites: ["051", "052", "053", "055"]
concepts: ["Ortak girdi", "Bağımsız kopyalar", "İşlem sayımı", "Karşılaştırma ölçütü", "Girdiye bağlı davranış"]
---

# İki sıralama algoritmasını karşılaştırma

## Problem tanımı

Tam `int` aralığında 1..50 eleman içeren aynı veriyi seçmeli sıralama ve erken bitişli kabarcık sıralamayla ayrı ayrı sıralayın. İki sonuç dizisini, değer karşılaştırması ve gerçek takas sayılarını, ayrıca kabarcığın gerçek tur sayısını yazdırın. Son satırda hangi yöntemin daha az değer karşılaştırması yaptığını veya sayıların eşit olduğunu belirtin.

Özgün veriyi okuyup döngüyle iki ayrı diziye kopyalayın. İkinci yöntemi birincinin sıraladığı dizi üzerinde çalıştırmayın; iki yöntem aynı başlangıç sırasını görmelidir. Hazır sıralama, kopyalama veya süre ölçümü kullanmayın. Bu çalışma belirli bir girdide belirli işlemleri karşılaştırır; genel bir hız veya bütün girdiler için üstünlük iddiası üretmez.

Değer karşılaştırması seçmelide `secmeli[j] < secmeli[enKucuk]`, kabarcıkta `kabarcik[j] > kabarcik[j + 1]` ifadesinin bir değerlendirmesidir. Dizin koşulları, doğrulama, kopyalama ve sayaçlar arasındaki son karar bu ölçüye dahil değildir. Takas iki değerin gerçekten yer değiştirmesidir; seçmelide aynı dizine takas yapılmaz. Kabarcıkta takassız son denetim turu sayılır; tek elemanda tur yoktur.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, `a` | 1..50 uzunluk ve N ayrı tam `int` değeri. |
| Ara değer | İki kopya | `secmeli`, `kabarcik`; başlangıçta özgün veriyle aynı. |
| Çıktı | Seçmeli sonuç | `Seçmeli dizi:`, `Seçmeli karşılaştırma:`, `Seçmeli takas:` satırları. |
| Çıktı | Kabarcık sonuç | `Kabarcık dizi:`, `Kabarcık karşılaştırma:`, `Kabarcık takas:`, `Kabarcık tur:` satırları. |
| Çıktı | Ortak ölçü kararı | `Daha az değer karşılaştırması: Seçmeli`, `Kabarcık` veya `Eşit`. |

`Eleman sayısı (1..50): ` ve `A[0]: ` ile başlayan istemleri ayrı satırlarda yanıtlayın. Çıktı tam sekiz satırdır. İki dizi satırında başlıktan sonra her değer için bir boşluk vardır; sonunda fazladan boşluk yoktur. Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.` yazılır. Bütün girişler doğru olmadan kopyalama ve sıralama başlamaz.

## Algoritma

1. Uzunluğu ve bütün özgün elemanları doğrulayarak okuyun.
2. İki N uzunluklu dizi oluşturup özgün veriyi bir döngüyle ikisine de kopyalayın.
3. Seçmeli sayaçlarını 0 yapın; her turda kalan bölümün minimumunu bulun, gerekirse takas edin.
4. Kabarcık sayaçlarını 0 yapın; daralan bölümde komşu büyükleri takas edin.
5. Kabarcıkta her gerçek turu sayın; takassız tur tamamlanınca durun.
6. İki diziyi ve ayrı sayaçlarını yazdırın.
7. Değer karşılaştırması sayılarını karşılaştırıp son karar satırını yazın.

Dört elemanlı üç girdi için beklenen sayaçlar:

| Girdi | Seçmeli karşılaştırma / takas | Kabarcık karşılaştırma / takas / tur |
| --- | --- | --- |
| 1, 2, 3, 4 | 6 / 0 | 3 / 0 / 1 |
| 4, 3, 2, 1 | 6 / 2 | 6 / 6 / 3 |
| 2, 1, 3, 4 | 6 / 1 | 5 / 1 / 2 |

Her kopyalama adımı sonunda iki kopyanın doldurulmuş bölümü özgün veriyle aynıdır. Sıralamalar yalnız kendi kopyalarını değiştirir; biri ötekinin girdisini etkileyemez. Seçmelide sıralı başlangıç, kabarcıkta kesinleşmiş son bölüm büyür. Her değer karşılaştırması tam bir kez ilgili sayaçta kayda girer. Sonlu uzunluklar iki sıralamayı da bitirir. Son karar yalnız ortak ölçüyü karşılaştırır; daha az karşılaştırmanın daha az takas anlamına gelmesi gerekmez.

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

int[] secmeli = new int[n];
int[] kabarcik = new int[n];
for (int i = 0; i < n; i++)
{
    secmeli[i] = a[i];
    kabarcik[i] = a[i];
}

int secimKarsilastirma = 0, secimTakas = 0;
for (int i = 0; i < n - 1; i++)
{
    int enKucuk = i;
    for (int j = i + 1; j < n; j++)
    {
        secimKarsilastirma++;
        if (secmeli[j] < secmeli[enKucuk])
        {
            enKucuk = j;
        }
    }
    if (enKucuk != i)
    {
        int gecici = secmeli[i];
        secmeli[i] = secmeli[enKucuk];
        secmeli[enKucuk] = gecici;
        secimTakas++;
    }
}

int kabarcikKarsilastirma = 0, kabarcikTakas = 0, kabarcikTur = 0;
for (int gecis = 0; gecis < n - 1; gecis++)
{
    bool degisti = false;
    for (int j = 0; j < n - 1 - gecis; j++)
    {
        kabarcikKarsilastirma++;
        if (kabarcik[j] > kabarcik[j + 1])
        {
            int gecici = kabarcik[j];
            kabarcik[j] = kabarcik[j + 1];
            kabarcik[j + 1] = gecici;
            kabarcikTakas++;
            degisti = true;
        }
    }
    kabarcikTur++;
    if (!degisti)
    {
        break;
    }
}

Console.Write("Seçmeli dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + secmeli[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Seçmeli karşılaştırma: " +
    secimKarsilastirma.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Seçmeli takas: " +
    secimTakas.ToString(CultureInfo.InvariantCulture));

Console.Write("Kabarcık dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + kabarcik[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Kabarcık karşılaştırma: " +
    kabarcikKarsilastirma.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Kabarcık takas: " +
    kabarcikTakas.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Kabarcık tur: " +
    kabarcikTur.ToString(CultureInfo.InvariantCulture));

if (secimKarsilastirma < kabarcikKarsilastirma)
{
    Console.WriteLine("Daha az değer karşılaştırması: Seçmeli");
}
else if (kabarcikKarsilastirma < secimKarsilastirma)
{
    Console.WriteLine("Daha az değer karşılaştırması: Kabarcık");
}
else
{
    Console.WriteLine("Daha az değer karşılaştırması: Eşit");
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Özgün A dizisi korunur; iki sıralama için toplam iki ek dizi ayrılır. 051 ve 052'nin sayım kuralları aynen uygulanır. Değerleri çıkararak karşılaştırmak yerine doğrudan `<` ve `>` kullanılır; `int` uç değerleri taşma oluşturmaz.

## Örnek çalıştırmalar

N ve ardından N eleman ayrı satırlarda girilir. Bloklar istemler dışındaki tam sekiz sonuç satırıdır.

N = 4; dizi 1, 2, 3, 4:

```text
Seçmeli dizi: 1 2 3 4
Seçmeli karşılaştırma: 6
Seçmeli takas: 0
Kabarcık dizi: 1 2 3 4
Kabarcık karşılaştırma: 3
Kabarcık takas: 0
Kabarcık tur: 1
Daha az değer karşılaştırması: Kabarcık
```

N = 4; dizi 4, 3, 2, 1:

```text
Seçmeli dizi: 1 2 3 4
Seçmeli karşılaştırma: 6
Seçmeli takas: 2
Kabarcık dizi: 1 2 3 4
Kabarcık karşılaştırma: 6
Kabarcık takas: 6
Kabarcık tur: 3
Daha az değer karşılaştırması: Eşit
```

N = 1; tek eleman -2147483648:

```text
Seçmeli dizi: -2147483648
Seçmeli karşılaştırma: 0
Seçmeli takas: 0
Kabarcık dizi: -2147483648
Kabarcık karşılaştırma: 0
Kabarcık takas: 0
Kabarcık tur: 0
Daha az değer karşılaştırması: Eşit
```

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; farklı değerler 50..1 sırasında | İki dizi 1..50 olur; karşılaştırmalar 1225/1225, takaslar 25/1225, kabarcık turu 49. |
| N = 50; bütün değerler 2147483647 | Karşılaştırmalar 1225/49; iki takas sayısı 0, kabarcık turu 1; son karar Kabarcık. |

Uzunluk 0, 51, metin, ondalık, boş veya EOF için tek çıktı:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

Herhangi bir elemanda metin, ondalık, `2147483648`, `-2147483649`, boş veya EOF için:

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- N = 1 için bütün sayaçlar 0'dır; sonuçlar eşittir.
- Sıralı N > 1 veride seçmeli bütün kalan bölümleri tarar; kabarcık N - 1 karşılaştırmadan sonra durur.
- Kabarcık en fazla seçmeli kadar değer karşılaştırması yapar; bu sürümlerde son karar Seçmeli olamaz. Üçlü karar, ölçü sayılarının genel karşılaştırmasını gösterir.
- Ters sıralı farklı değerlerde karşılaştırmalar aynı olmasına rağmen takas sayıları farklı olabilir.
- Tekrarlar iki sonuç dizisinde de aynı adetlerle korunur; eşit komşular kabarcıkta takas edilmez.
- Karşılaştırma ve kabarcık takası en fazla 1225; seçmeli takas ve kabarcık turu en fazla 49'dur.
- Sonuçlar seçilen veri, sayaç tanımı ve sürümlere aittir. Kopyalama, bellek, doğrulama ve diğer işlemler bu ölçünün dışındadır.
- Geçersiz veya eksik girişte sekiz satırlık sonuç üretilmez.

## Kazanımlar

- İki yönteme aynı özgün verinin ayrı kopyalarını sağlama.
- Ortak ölçünün hangi işlemleri içerdiğini kesin tanımlama.
- Farklı anlamdaki sayaçları birbirinin yerine kullanmama.
- Sıralı, ters ve tekrarlı verinin sayaçları nasıl etkilediğini açıklama.
- Gözlenen işlem sayısından genel çalışma süresi üstünlüğü çıkarmanın sınırlarını açıklama.

## Alıştırmalar

1. Aynı veri için üçüncü kopyada eklemeli sıralama çalıştırın; değer karşılaştırmasını ortak ölçü olarak tutup kaydırmaları ayrı verin.
2. Kopyalama atamalarını ayrı sayaçta sayın; iki kopya için 2N olduğunu doğrulayın, sıralama karşılaştırmalarına eklemeyin.
3. Kabarcığın erken çıkışını kaldırın; sıralı veride sayaçların ve son kararın nasıl değiştiğini açıklayın.
