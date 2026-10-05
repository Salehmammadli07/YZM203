---
id: "052"
order: 52
title: "Erken bitişli kabarcık sıralama"
level: "İleri"
prerequisites: ["051"]
concepts: ["Kabarcık sıralama", "Komşu takası", "Daralan döngü sınırı", "Erken sonlandırma", "Tur ve işlem sayıları"]
---

# Erken bitişli kabarcık sıralama

## Problem tanımı

Tam `int` aralığındaki 1 ile 50 elemanı kabarcık sıralama ile azalmayan sıraya getirin. Soldan sağa komşu elemanları karşılaştırın; soldaki büyükse yerlerini değiştirin. Bir geçişin sonunda henüz sıralanmamış bölümün en büyük değeri bu bölümün sonuna taşınır. Sonraki turda bu konumu yeniden incelemeyin.

Bir turda hiç takas yapılmadıysa dizi zaten azalmayan sıradadır; kalan turları çalıştırmadan bitirin. Örneğin `1, 2, 3, 4` dizisi için tek tur yeterlidir. Takas koşulu kesinlikle `>` olmalıdır; eşit komşuların yerleri değiştirilmez ve tekrarlar korunur.

Üç sayaç tutun. Karşılaştırma, iç döngünün bir gövdesinde `a[j] > a[j + 1]` değerlendirmesidir. Takas, komşu çiftin bir kez yer değiştirmesidir. Tur, en az bir komşu karşılaştırması içeren bir dış döngü geçişidir; hiç takas yapılmayan son denetim turu da sayılır. Girdi doğrulaması ve döngü sınırı denetimleri karşılaştırma sayısına dahil değildir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`. |
| Girdi | `a` | Her eleman -2147483648..2147483647 aralığında. |
| Çıktı | Sıralı dizi | `Dizi:` ardından boşlukla ayrılan N değer. |
| Çıktı | Sayaçlar | Ayrı satırlarda `Karşılaştırma: X`, `Takas: Y`, `Tur: Z`. |

`Eleman sayısı (1..50): ` istemini ve ardından `A[0]: `, `A[1]: ` gibi istemleri yanıtlayın. N ve her eleman ayrı satırda okunur; ondalık bölüm veya basamak ayırıcı kullanılmaz. Bütün elemanlar doğrulanmadan sıralama başlamaz.

Geçersiz uzunlukta `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, geçersiz elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.` yazılır. Boş satır veya EOF ilgili alanın hatasıdır. Hata halinde dizi ve sayaç sonuçları üretilmez.

## Algoritma

1. N değerini ve bütün dizi elemanlarını doğrulayarak okuyun.
2. Karşılaştırma, takas ve tur sayaçlarını 0 yapın.
3. Dış döngüyü `gecis = 0` ile başlatıp `n - 2` dahil ilerletin.
4. Her geçişte `degisti` durumunu yanlış yapın.
5. İç döngüde `j < n - 1 - gecis` olduğu sürece komşu çifti karşılaştırıp sayacı artırın.
6. Soldaki büyükse elemanları takas edin, takas sayacını artırın ve `degisti` durumunu doğru yapın.
7. İç döngü tamamlanınca tur sayacını bir artırın.
8. Hiç takas yapılmadıysa dış döngüden çıkın; sonunda diziyi ve sayaçları yazdırın.

Bir iç döngü adımından sonra o ana kadar gezilen bölümün en büyük değeri sağdaki konuma ilerlemiştir. Geçiş sonunda kalan bölümün en büyüğü son konumuna ulaşır. Dış döngü başında en sağdaki `gecis` eleman artık sıralıdır; iç sınır bu bölümü dışarıda tutar. Bir geçişte takas yoksa gezilen bütün komşular doğru sıradadır; önceden tamamlanan sağ bölümle birlikte bütün dizi sıralıdır. İç ve dış sınırlar sonlu olduğu için erken çıkış olmasa da algoritma biter.

`3, 1, 2, 1` dizisi için turlar; karşılaştırma ve takas sütunları birikimlidir:

| Tur | İç döngüde karşılaştırılan j dizinleri | Tur sonundaki dizi | Karşılaştırma | Takas |
| --- | --- | --- | --- | --- |
| 1 | 0, 1, 2 | 1, 2, 1, 3 | 3 | 3 |
| 2 | 0, 1 | 1, 1, 2, 3 | 5 | 4 |
| 3 | 0 | 1, 1, 2, 3 | 6 | 4 |

Üçüncü turda takas olmaz ve çıkış yapılır. Bu denetim gerçek bir karşılaştırma içerdiği için tur sayısı 3'tür. N = 1 durumunda iç döngülü bir geçiş oluşmadığından tur sayısı 0'dır.

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

int karsilastirma = 0, takas = 0, tur = 0;
for (int gecis = 0; gecis < n - 1; gecis++)
{
    bool degisti = false;
    for (int j = 0; j < n - 1 - gecis; j++)
    {
        karsilastirma++;
        if (a[j] > a[j + 1])
        {
            int gecici = a[j];
            a[j] = a[j + 1];
            a[j + 1] = gecici;
            takas++;
            degisti = true;
        }
    }
    tur++;
    if (!degisti)
    {
        break;
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
Console.WriteLine("Tur: " + tur.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `j < n - 1 - gecis` koşulu hem sağdaki tamamlanmış bölümü atlar hem de `j + 1` erişimini dizi sınırında tutar. Tur sayacının iç döngüden sonra artırılması, takassız son turun da kayda girmesini sağlar. Elemanlar üzerinde aritmetik yapılmadığı için tam `int` aralığı güvenlidir.

## Örnek çalıştırmalar

Girdi sırası N ve N elemandır; her değer ayrı satırdadır. Metin blokları istemler dışındaki bütün sonuç satırlarıdır.

N = 4; elemanlar 3, 1, 2, 1:

```text
Dizi: 1 1 2 3
Karşılaştırma: 6
Takas: 4
Tur: 3
```

N = 4; elemanlar 1, 2, 3, 4:

```text
Dizi: 1 2 3 4
Karşılaştırma: 3
Takas: 0
Tur: 1
```

N = 1; tek eleman 2147483647:

```text
Dizi: 2147483647
Karşılaştırma: 0
Takas: 0
Tur: 0
```

N = 2; elemanlar -2147483648, -2147483648:

```text
Dizi: -2147483648 -2147483648
Karşılaştırma: 1
Takas: 0
Tur: 1
```

N = 4; elemanlar 4, 3, 2, 1:

```text
Dizi: 1 2 3 4
Karşılaştırma: 6
Takas: 6
Tur: 3
```

Bu tablo büyük çıktının tamamı değildir; beklenen özellikleri tanımlar.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; elemanlar 50'den 1'e doğru | 1'den 50'ye sıralı dizi; 1225 karşılaştırma, 1225 takas, 49 tur. |
| N = 50; bütün elemanlar 2147483647 | 50 eşit değer korunur; 49 karşılaştırma, 0 takas, 1 tur. |

N için 0, 51, `1.5`, metin, boş satır veya EOF verilirse tam hata çıktısı:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

N = 2 ve ilk eleman 7 sonrasında `iki` girilirse tek hata satırı aşağıdadır. Herhangi bir elemanda `2147483648`, `-2147483649`, `2.5`, boş satır veya EOF da aynı hatayı verir; sayaçlar yazılmaz.

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- Tek elemanda karşılaştırma, takas ve tur sayıları 0'dır.
- Sıralı veya eşit elemanlı N > 1 dizisinde ilk tur N - 1 karşılaştırmayla biter; takas 0, tur 1 olur.
- `>` yerine `>=` kullanılması eşit komşularda gereksiz takas yapar ve erken çıkışı bozabilir.
- Ters sıralı farklı elemanlarda en fazla `n × (n - 1) / 2` karşılaştırma ve takas yapılır; N = 50 için üst sınır 1225'tir.
- Tur sayısı en fazla N - 1, yani 49'dur. `int` sayaçlar bu sınırların tamamını karşılar.
- Hatalı veya eksik eleman halinde ilk sıralama turu başlamaz; kısmi dizi sonucu yoktur.

## Kazanımlar

- Komşu takaslarının en büyük değeri sağa nasıl taşıdığını adım adım açıklama.
- Tamamlanmış sağ bölüme göre iç döngü sınırını daraltma.
- Bir tur başında değişim durumunu yeniden başlatma.
- Takassız bir turdan bütün dizinin sıralı olduğunu çıkarma.
- Değer karşılaştırması, takas ve tur sayılarını ayrı anlamlarla ölçme.

## Alıştırmalar

1. Erken çıkış koşulunu kaldırıp 1, 2, 3, 4 dizisi için sayaçları karşılaştırın. Sonuç aynı kalırken karşılaştırma ve tur sayılarının neden arttığını açıklayın.
2. Her turun sonunda diziyi ve o turdaki takas sayısını yazdırın; 3, 1, 2, 1 girdisini tabloyla karşılaştırın.
3. 051 ve bu çözümü aynı sıralı ve ters sıralı dizilerde çalıştırın. İki algoritmanın farklı işlemler saydığı takas sonuçlarını ve aynı anlamdaki değer karşılaştırmalarını karşılaştırın.
