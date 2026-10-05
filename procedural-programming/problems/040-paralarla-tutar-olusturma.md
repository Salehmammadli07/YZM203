---
id: "040"
order: 40
title: "Bir tutarı 1, 2 ve 5 birimlik paralarla oluşturma"
level: "İleri"
prerequisites: ["018", "031", "039"]
concepts: ["İç içe döngü", "Kısıtlı kombinasyon üretimi", "Kalan tutar", "Eşitlikten değişken bulma", "Sıfır çözümü"]
---

# Bir tutarı 1, 2 ve 5 birimlik paralarla oluşturma

## Problem tanımı

Bir tamsayı tutarı, 1, 2 ve 5 birimlik paralardan kaç adet kullanarak oluşturabileceğinizi bulun. Her para türünden sınırsız sayıda bulunduğunu kabul edin. Bütün farklı adet kombinasyonlarını üretin ve kaç tane olduklarını bulun.

Paraların diziliş sırası önemli değildir. Örneğin iki tane 1 ile bir tane 2 kullanılması, bu paraların hangi sırayla dizildiğinden bağımsız olarak tek kombinasyondur. Her çözüm `birler + 2 * ikiler + 5 * besler = tutar` eşitliğini sağlamalıdır. Beşlik ve ikilik adetlerini iki döngüyle seçin; birlik adedini kalan tutardan bulun. Üçüncü bir arama döngüsüne gerek yoktur. Tutar 0 ise hiçbir para kullanılmayan tek çözüm vardır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `tutar` | 0 ile 100 arasında `int`; oluşturulacak soyut birim sayısı. |
| Çıktı | Kombinasyonlar | `Çözümler:` başlığından sonra `1: X, 2: Y, 5: Z` biçiminde her satırda para adetleri. |
| Çıktı | `adet` | Son satırda `Adet: N`; farklı kombinasyon sayısı. |

`Tutar (0..100): ` istemine tek tamsayı girin. Çözümler önce beşlik adedi, aynı beşlik adedinde ikilik adedi artacak şekilde yazılır. Geçersiz girişte `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` yazılır; çözüm başlığı üretilmez.

## Algoritma

1. Tutarı okuyup 0..100 aralığını doğrulayın.
2. Adedi 0 yapıp çözüm başlığını yazdırın.
3. Dış döngüde beşlik adedini 0'dan `tutar / 5` dahil olacak şekilde artırın.
4. Seçilen beşliklerden sonra kalan tutarı `tutar - 5 * besler` ile hesaplayın.
5. İç döngüde ikilik adedini 0'dan `kalan / 2` dahil olacak şekilde artırın.
6. Birlik adedini `kalan - 2 * ikiler` ile hesaplayın.
7. Üç adedi belirtilen biçimde yazdırıp çözüm sayacını bir artırın.
8. Bütün döngüler bitince adedi yazdırın.

İç döngünün üst sınırı ikiliklerin kalan tutarı aşmasını önler; birlik adedi bu nedenle hiçbir zaman negatif olmaz. Her beşlik-ikilik çifti için birlik adedi eşitlikle tek bir değere belirlenir. Bütün geçerli çiftler birer kez denenir; farklı para dizilişleri ayrıca üretilmez. İki sayaç da sınırlı aralıkta arttığından işlem sonlanır. Birimlik para bulunduğu için her geçerli tutar en az bir çözüme sahiptir.

Tutar 5 için aday üretimi şöyledir.

| Beşlik adedi | Beşliklerden kalan | İkilik adedi | Birlik adedi | Eşitlik |
| --- | --- | --- | --- | --- |
| 0 | 5 | 0 | 5 | `5 + 2 * 0 + 5 * 0 = 5` |
| 0 | 5 | 1 | 3 | `3 + 2 * 1 + 5 * 0 = 5` |
| 0 | 5 | 2 | 1 | `1 + 2 * 2 + 5 * 0 = 5` |
| 1 | 0 | 0 | 0 | `0 + 2 * 0 + 5 * 1 = 5` |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Tutar (0..100): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int tutar) || tutar < 0 || tutar > 100)
{
    Console.WriteLine("Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.");
    return;
}
int adet = 0;
Console.WriteLine("Çözümler:");
for (int besler = 0; besler <= tutar / 5; besler++)
{
    int kalan = tutar - 5 * besler;
    for (int ikiler = 0; ikiler <= kalan / 2; ikiler++)
    {
        int birler = kalan - 2 * ikiler;
        Console.WriteLine("1: " + birler.ToString(CultureInfo.InvariantCulture) +
            ", 2: " + ikiler.ToString(CultureInfo.InvariantCulture) +
            ", 5: " + besler.ToString(CultureInfo.InvariantCulture));
        adet++;
    }
}
Console.WriteLine("Adet: " + adet.ToString(CultureInfo.InvariantCulture));
```

`tutar / 5` ve `kalan / 2` tamsayı bölmeleridir; sığabilecek en büyük para adedini verir. Örneğin kalan 5 iken en fazla iki tane ikilik kullanılabilir. Birlik adedi aranmadan doğrudan hesaplandığı için her yazdırılan aday baştan geçerli bir çözümdür.

## Örnek çalıştırmalar

Metin blokları giriş isteminden sonraki bütün çıktı satırlarını gösterir.

Girdi `0` için tam çıktı:

```text
Çözümler:
1: 0, 2: 0, 5: 0
Adet: 1
```

Girdi `2` için tam çıktı:

```text
Çözümler:
1: 2, 2: 0, 5: 0
1: 0, 2: 1, 5: 0
Adet: 2
```

Girdi `5` için tam çıktı:

```text
Çözümler:
1: 5, 2: 0, 5: 0
1: 3, 2: 1, 5: 0
1: 1, 2: 2, 5: 0
1: 0, 2: 0, 5: 1
Adet: 4
```

Hacimli çıktılar için aşağıdaki tablo tam çözüm listesi değildir; denetlenecek özellikleri özetler.

| Girdi | Beklenen özellik veya denetim |
| --- | --- |
| `10` | 10 farklı kombinasyon; son satır `Adet: 10`. |
| `100` | 541 farklı kombinasyon; ilk çözüm `1: 100, 2: 0, 5: 0`, son çözüm `1: 0, 2: 0, 5: 20`; son satır `Adet: 541`. |

Aşağıdaki örneklerde tek hata satırı yazılır.

| Girdi | Tam sonuç |
| --- | --- |
| `-1` | `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` |
| `101` | `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` |
| `para` | `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` |
| `2.5` | `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` |
| Boş satır | `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` |
| Hiç satır vermeden girişin sonu | `Hata: Tutar 0 ile 100 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- Tutar 0 olduğunda dış ve iç döngüler 0 adedi için birer kez çalışır; hiçbir para kullanılmayan tek çözüm sayılır.
- Tutar 1 olduğunda yalnızca bir birlik para içeren çözüm vardır.
- Tutarın 5'e veya kalan tutarın 2'ye tam bölünmesi gerekmez; tamsayı bölümünün kalan kısmı birliklerle tamamlanır.
- Bütün adetler sıfır veya pozitiftir. Birlikler en fazla 100, ikilikler en fazla 50, beşlikler en fazla 20 olabilir; hesaplamalar `int` içine sığar.
- Her sabit beşlik adedi için iç döngü `kalan / 2 + 1` çözüm üretir. Tutar 100 için bu sayıların toplamı 541'dir.
- Aynı adetler farklı para sıralamasıyla yeniden sayılmaz. Çıktı sırası önce beşlik, sonra ikilik sayacına bağlıdır.
- Metin, ondalık sayı, boş satır, giriş sonu ve aralık dışı değerler sonuç başlığı yazılmadan reddedilir.

## Kazanımlar

- Üç değişkenli bir eşitlikte iki değişkeni seçip üçüncüyü hesaplama.
- İç döngü sınırını dış döngüde kalan tutara bağlı kurma.
- Para adetlerinin negatif olmamasını aday sınırlarıyla sağlama.
- Para sıralamasıyla adet kombinasyonunu birbirinden ayırma.
- Sıfır tutarın boş seçim içeren bir geçerli çözüm olduğunu açıklama.
- 5 birimlik tutar için dört çözümü döngü sırasıyla üretme.

## Alıştırmalar

1. Her çözümde kullanılan toplam para adedini de yazdırın; tutar 5 için dört satırdaki adetleri karşılaştırın.
2. Yalnızca toplam para sayısı çift olan kombinasyonları yazdırın ve sayın; sıfır tutarın bu koşuldaki durumunu açıklayın.
3. Para değerlerini 1, 3 ve 7 olarak değiştirin; döngü sınırlarını ve kalan tutar eşitliğini güncelleyip tutar 7'nin bütün kombinasyonlarını çıkarın.
