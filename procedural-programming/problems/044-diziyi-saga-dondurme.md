---
id: "044"
order: 44
title: "Diziyi sağa döndürme"
level: "İleri"
prerequisites: ["018", "031", "043"]
concepts: ["Dizi döndürme", "Kalanla adım azaltma", "Geriye kopyalama", "Son elemanı koruma", "İç içe döngü"]
---

# Diziyi sağa döndürme

## Problem tanımı

1 ile 50 tamsayı içeren diziyi, kullanıcının belirttiği K adım sağa döndürün. K, 0 ile 1000 arasında bir tamsayıdır. Bir sağa döndürme adımında son eleman başa gelir; diğer elemanlar bir konum sağa kayar. Örneğin 10, 20, 30, 40, 50 dizisi iki adım sonra 40, 50, 10, 20, 30 olur.

Elemanları ikinci bir diziye taşımadan aynı dizide işlem yapın. Dizi uzunluğu kadar döndürme başlangıç sırasını geri verdiğinden gerekli adım sayısı K % N'dir. Her adımda önce son elemanı saklayın; sonra değerleri son dizinden ilk dizine doğru kopyalayın. İleri yönde kopyalamak, henüz kullanılmamış eski değerleri ezebilir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `sayilar` | Her biri tam `int` aralığında N eleman. |
| Girdi | `k` | Elemanlardan sonra girilen 0..1000 döndürme sayısı. |
| Çıktı | Döndürülmüş dizi | `Dizi:` ardından her eleman için bir boşluk ve değer; tek satır. |

Sırayla `Eleman sayısı (1..50): `, `Eleman[0]: ` ile başlayan N istem ve `Adım sayısı (0..1000): ` yanıtlanır. Her tamsayı ayrı satırda yazılır. Dizinler sıfır tabanlıdır. Bütün elemanlar ve K doğrulanmadan dönüşüm veya sonuç yazımı yapılmaz. Sonuçta satır sonuna fazladan boşluk eklenmez.

## Algoritma

1. N değerini doğrulayın; diziyi oluşturup N elemanı doğrulayarak doldurun.
2. K değerini okuyup 0..1000 aralığını doğrulayın.
3. Gerçek adım sayısını `k % n` ile bulun.
4. Her adımda son elemanı geçici değişkene kaydedin.
5. Dizini N - 1'den 1'e kadar geriye ilerletin; her konuma sol komşusunun değerini kopyalayın.
6. Sıfırıncı konuma saklanan eski son elemanı yazın.
7. Gerekli adımlar tamamlanınca diziyi yazdırın.

Her adımın başında dizi o ana kadar tamamlanmış döndürmelerin sonucudur. Geriye kopyalama sırasında kaynak olan sol komşu henüz değiştirilmemiştir; eski değer güvenle alınır. Son eleman önceden saklandığı için son dizine yapılan ilk kopyada kaybolmaz. Bir tam tur bütün elemanları başlangıç konumuna döndürür; bu yüzden K'nin N'ye bölümünden kalan kadar adım yeterlidir. N pozitif olduğundan `% n` işlemi sıfıra bölme oluşturmaz.

10, 20, 30, 40, 50 dizisinde K = 2 için:

| Tamamlanan adım | Dizi |
| --- | --- |
| 0 | 10, 20, 30, 40, 50 |
| 1 | 50, 10, 20, 30, 40 |
| 2 | 40, 50, 10, 20, 30 |

İlk adımda 50 saklanır; 4. konuma 40, 3. konuma 30, 2. konuma 20 ve 1. konuma 10 taşınır. En son 0. konuma 50 yazılır.

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
Console.Write("Adım sayısı (0..1000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int k) || k < 0 || k > 1000)
{
    Console.WriteLine("Hata: Adım sayısı 0 ile 1000 arasında olmalıdır.");
    return;
}

int adim = k % n;
for (int tur = 0; tur < adim; tur++)
{
    int son = sayilar[n - 1];
    for (int i = n - 1; i >= 1; i--)
    {
        sayilar[i] = sayilar[i - 1];
    }
    sayilar[0] = son;
}
Console.Write("Dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + sayilar[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Geriye giden iç döngü 1'de sonlandığı için `i - 1` hiçbir zaman negatif olmaz. `adim` 0 ise döndürme gövdesi hiç çalışmaz; dizi aynı biçimde yazdırılır. Elemanlara aritmetik uygulanmaz, yalnızca değerleri taşınır. Girdi dizisinin dışında ikinci bir dizi yoktur.

## Örnek çalıştırmalar

Girdi sırası N, N eleman ve K'dir; her değer ayrı satırdadır. Bloklar istemler dışındaki tüm çıktıları gösterir.

N = 5; elemanlar 10, 20, 30, 40, 50; K = 2:

```text
Dizi: 40 50 10 20 30
```

N = 3; elemanlar 1, 2, 3; K = 0:

```text
Dizi: 1 2 3
```

N = 3; elemanlar 7, -2, 7; K = 6:

```text
Dizi: 7 -2 7
```

N = 1; eleman 2147483647; K = 1000:

```text
Dizi: 2147483647
```

N = 3; elemanlar -1, 0, 1; K = 1000, gerçek adım 1:

```text
Dizi: 1 -1 0
```

N = 2; elemanlar -2147483648, 2147483647; K = 1:

```text
Dizi: 2147483647 -2147483648
```

Uzun dizi için tam çıktı yerine aşağıdaki özellikleri denetleyin.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; elemanlar 0..49; K = 1000 | Gerçek adım 0; başlangıç sırası korunur. |
| N = 50; elemanlar 0..49; K = 999 | Gerçek adım 49; ilk değer 1, son değer 0; her başlangıç elemanı bir kez bulunur. |

N için 0, 51, -1, `2.5`, metin, boş satır veya giriş sonu verilirse:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

Herhangi bir elemanda `2147483648`, `-2147483649`, `1.0`, metin, boş satır veya giriş sonu verilirse K istenmeden:

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

N = 1 ve eleman 7 sonrasında K için -1, 1001, `2.5`, metin, boş satır veya giriş sonu verilirse:

```text
Hata: Adım sayısı 0 ile 1000 arasında olmalıdır.
```

## Sınır durumları

- K = 0 veya K'nin N'ye tam bölünmesi durumunda gerçek adım 0'dır; dizi değişmez.
- N = 1 için bütün geçerli K değerlerinin kalanı 0'dır; tek eleman korunur.
- Bir adım son elemanı başa getirir; ters çevirme gibi bütün sırayı tersine döndürmez.
- İleri kopyalama, örneğin 1, 2, 3 dizisinde önce 1'i ikinci konuma yazıp sonra bu değiştirilmiş değeri üçüncü konuma taşıyarak veri kaybettirir.
- Gerçek adım N'den küçüktür. N = 50 için en fazla 49 tur ve tur başına 49 komşu kopyası yapılır; bu yöntem öğrenim için küçük girişlerde uygundur.
- Değerlerin tekrarlanması ve `int` uç değerleri güvenle işlenir; değerler yalnızca saklanıp atanır.
- Eksik veya geçersiz girişte `Dizi:` satırı yazılmaz ve kısmi sonuç verilmez.

## Kazanımlar

- Döndürmeyi sıralamadan ve ters çevirmeden ayırt etme.
- Tam tur tekrarlarını kalan işleciyle azaltma.
- Yerinde kaydırmada kopyalama yönünü eski veriyi koruyacak şekilde seçme.
- Eski son elemanı geçici değişkenle saklayıp başa yerleştirme.
- Sıfır gerçek adım ve tek eleman durumlarını döngü koşuluyla ele alma.

## Alıştırmalar

1. Bir sola döndürme sürümü yazın; ilk elemanı saklayıp kopyalama yönünü değiştirin. Aynı üç elemanı sağa ve sola bir adım döndürün.
2. Gerçek tur ve komşu kopyası adetlerini iki sayaçla bulun. N = 5 ve K = 12 için beklenen 2 tur ve 8 kopyayı doğrulayın.
3. Diziyi K adım sağa döndürdükten sonra K adım sola döndürün; tekrarlı ve uç değerli örneklerde başlangıç sırasının geri geldiğini gösterin.
