---
id: "051"
order: 51
title: "Seçmeli sıralama ile diziyi sıralama"
level: "İleri"
prerequisites: ["041", "043", "046"]
concepts: ["Seçmeli sıralama", "Sıralı başlangıç bölümü", "En küçük adayı", "Değer karşılaştırması", "Takas sayısı"]
---

# Seçmeli sıralama ile diziyi sıralama

## Problem tanımı

1 ile 50 arasında tamsayı içeren bir diziyi seçmeli sıralama ile azalmayan sıraya getirin. Azalmayan sırada her değer kendisinden sonraki değerden küçük veya ona eşittir; tekrarlar korunur. Her eleman -2147483648 ile 2147483647 arasında, yani tam `int` aralığında olabilir.

Her turda henüz sıralanmamış bölümün en küçük elemanını bulun ve bu bölümün ilk konumuna taşıyın. Örneğin `3, 1, 2, 1` dizisi `1, 1, 2, 3` olur. Hazır sıralama kullanmayın; aday seçimini ve takası görünür işlemlerle gerçekleştirin.

Algoritmanın yaptığı değer karşılaştırmalarını ve takasları ayrıca sayın. Bir karşılaştırma, `a[j] < a[enKucuk]` ifadesinin bir kez değerlendirilmesidir. Döngü koşulları, girdi doğrulaması ve `enKucuk != i` dizin denetimi bu sayaca dahil değildir. En küçük zaten tur başındaysa takas yapmayın; üç atamadan oluşan bir yer değiştirme tek takastır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; eleman sayısı. |
| Girdi | `a` | Tam `int` aralığında N eleman. |
| Çıktı | Sıralı dizi | `Dizi:` ardından her eleman için bir boşluk ve değer. |
| Çıktı | Sayaçlar | Ayrı satırlarda `Karşılaştırma: X` ve `Takas: Y`. |

Önce `Eleman sayısı (1..50): ` istemini yanıtlayın. Sonra `A[0]: `, `A[1]: ` gibi sıfır tabanlı istemlerde her elemanı ayrı satırda girin. Girdiler tamsayı olmalıdır; basamak ayırıcı veya ondalık bölüm kullanılmaz. Bütün elemanlar doğrulanmadan sıralama başlamaz.

Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, geçersiz elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.` yazılır. Boş satır veya giriş sonu (EOF) da ilgili alanın hata satırını üretir. Hata halinde dizi ve sayaç sonuçları yazılmaz.

## Algoritma

1. Uzunluğu doğrulayın ve bütün elemanları okuyup doğrulayın.
2. Karşılaştırma ve takas sayaçlarını 0 yapın.
3. Tur başı dizinini `i = 0` ile başlatıp `n - 2` dahil ilerletin.
4. En küçük adayının dizinini `i` yapın.
5. `j = i + 1` ile başlayan taramada her eleman için karşılaştırma sayacını artırın.
6. `a[j] < a[enKucuk]` ise aday dizinini `j` yapın; eşitlikte değiştirmeyin.
7. Aday dizini `i` değilse geçici değişkenle iki elemanı takas edip takas sayacını artırın.
8. Bütün turlar bitince diziyi ve iki sayacı yazdırın.

Tur başında 0..`i - 1` bölümü, bütün dizinin en küçük `i` değerini sıralı biçimde içerir. İç döngüde aday, `i` konumundan o ana kadar incelenen konuma kadarki en küçük değerdir. Tarama tamamlanınca kalan bölümün en küçüğü `i` konumuna yerleşir; sıralı bölüm bir eleman büyür. Takas elemanları kaybetmez; aynı değerlerin adetleri korunur. Sınırlı dizinler arttığından döngüler sonlanır.

`3, 1, 2, 1` dizisinin turları; sayaçlar birikimli verilmiştir:

| Tur başı i | Seçilen dizin / değer | Tur sonundaki dizi | Karşılaştırma | Takas |
| --- | --- | --- | --- | --- |
| 0 | 1 / 1 | 1, 3, 2, 1 | 3 | 1 |
| 1 | 3 / 1 | 1, 1, 2, 3 | 5 | 2 |
| 2 | 2 / 2 | 1, 1, 2, 3 | 6 | 2 |

Her turda kalan bölüm tamamen taranır. Karşılaştırma sayısı girdinin sırasından bağımsız olarak `(n - 1) + (n - 2) + ... + 1 = n × (n - 1) / 2` olur. Bu formül sonuç sayacını önceden atamak için kullanılmaz; kod gerçekten yapılan karşılaştırmaları sayar.

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

int karsilastirma = 0, takas = 0;
for (int i = 0; i < n - 1; i++)
{
    int enKucuk = i;
    for (int j = i + 1; j < n; j++)
    {
        karsilastirma++;
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
        takas++;
    }
}

Console.Write("Dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
Console.WriteLine("Karşılaştırma: " +
    karsilastirma.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Takas: " + takas.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Elemanlar çıkarılarak karşılaştırılmadığı için iki `int` uç değeri arasında taşma oluşmaz. Takas için yalnız bir geçici tamsayı gerekir; ek sonuç dizisi oluşturulmaz. Eşit adayda kayıtlı dizin korunur; ancak uzaktaki elemanların takası, eşit değerlerin özgün konumlarını koruyan bir yöntem olarak yorumlanmamalıdır.

## Örnek çalıştırmalar

Girdi sırası N ve ardından N elemandır; her girdi ayrı satırdadır. Metin blokları istemler dışındaki bütün sonuç satırlarını gösterir.

N = 4; elemanlar 3, 1, 2, 1:

```text
Dizi: 1 1 2 3
Karşılaştırma: 6
Takas: 2
```

N = 1; tek eleman -2147483648:

```text
Dizi: -2147483648
Karşılaştırma: 0
Takas: 0
```

N = 4; elemanlar 1, 2, 3, 4:

```text
Dizi: 1 2 3 4
Karşılaştırma: 6
Takas: 0
```

N = 3; elemanlar 2147483647, 0, -2147483648:

```text
Dizi: -2147483648 0 2147483647
Karşılaştırma: 3
Takas: 1
```

Aşağıdaki tablo büyük girdilerin tam çıktısı değildir; denetlenecek özellikleri belirtir.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; elemanlar 50'den 1'e doğru | Dizi 1'den 50'ye sıralanır; 1225 karşılaştırma, 25 takas. |
| N = 50; bütün elemanlar 2147483647 | 50 değer korunur; 1225 karşılaştırma, 0 takas. |

N için 0, 51, -1, `1.5`, metin, boş satır veya EOF verilirse tam hata çıktısı:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

N = 3 sonrasında 7 ve `iki` girilirse tek hata satırı aşağıdadır. Herhangi bir elemanda `2147483648`, `-2147483649`, `2.5`, boş satır veya EOF da aynı hatayı üretir; sıralama sonucu ve sayaçlar verilmez.

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- N = 1 için hiçbir sıralama turu yoktur; iki sayaç da 0 kalır.
- Sıralı veya tamamen eşit bir dizide takas yapılmaz; bütün değer karşılaştırmaları yine yapılır.
- Son konum için ayrı bir tur gerekmez; önceki konumlar doğruysa kalan tek eleman zaten yerindedir.
- N en fazla 50 olduğundan karşılaştırma sayısı en fazla 1225, takas sayısı en fazla 49'dur; sayaçlar `int` türüne sığar.
- Negatif değerler, 0 ve tam `int` uçları normal veridir; değerler üzerinde toplama veya çıkarma yapılmaz.
- Hatalı son eleman bile işlemi başlatmaz; eksik dizinin kısmi sıralaması yazılmaz.

## Kazanımlar

- Sıralı başlangıç bölümü ile henüz sıralanmamış bölümü ayırma.
- Her turda en küçük adayını yeniden başlatıp kalan bölümün tamamını tarama.
- Gereksiz kendi kendine takası dizin karşılaştırmasıyla önleme.
- Değer karşılaştırması ile döngü denetiminin sayımını ayırt etme.
- Aynı N için farklı başlangıç sıralarının karşılaştırma sayısını değiştirmediğini gösterme.

## Alıştırmalar

1. Her turdan sonra diziyi ve o turdaki karşılaştırma sayısını yazdırın; 3, 1, 2, 1 girdisini izleme tablosuyla karşılaştırın.
2. Kalan bölümün en büyüğünü seçerek diziyi azalan sıraya getirin; sayaçların neyi saydığı sözleşmesini koruyun.
3. N = 5 için sıralı, ters sıralı ve eşit elemanlı üç dizi deneyin. Her birinde 10 karşılaştırma olduğunu doğrulayın ve takas farkını açıklayın.
