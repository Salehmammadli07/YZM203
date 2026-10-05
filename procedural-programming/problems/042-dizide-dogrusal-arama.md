---
id: "042"
order: 42
title: "Dizide doğrusal arama yapma"
level: "İleri"
prerequisites: ["041"]
concepts: ["Doğrusal arama", "Sıfır tabanlı dizin", "İlk eşleşme", "Erken döngü çıkışı", "Bulunamadı işareti"]
---

# Dizide doğrusal arama yapma

## Problem tanımı

1 ile 50 tamsayı içeren bir dizide, ayrıca girilen bir tamsayıyı arayın. Elemanlar ve aranan değer `int` türünün bütün aralığında olabilir. Dizinin sıralı olması gerekmez. Elemanları baştan sona birer birer karşılaştırarak aranan değerin ilk bulunduğu dizini yazdırın. Hiç eşleşme yoksa bulunamadı sonucu verin.

Dizinler sıfırdan başlar. Örneğin 4, 9, 4, -1, 9 dizisinde 9'un ilk dizini 1'dir. Sonraki 9 da eşleşse bile sonuç 4 olmamalıdır. İlk eşleşmede döngüden çıkın. Arama sonucunu tutan değişkende -1 kullanın; bu değer geçerli bir dizi dizini olmadığı için bulunamadı durumunu temsil edebilir. Dizideki bir elemanın -1 olması ise normal veridir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `sayilar` | N ayrı `int` eleman. |
| Girdi | `aranan` | Elemanlardan sonra girilen, tam `int` aralığında arama değeri. |
| Çıktı | İlk dizin veya bulunamadı | `İlk dizin: I` ya da `Sonuç: Bulunamadı`. |

Önce `Eleman sayısı (1..50): `, sonra `Eleman[0]: ` ile başlayan N eleman istemi, en son `Aranan: ` yanıtlanır. Her tamsayı ayrı satırda girilir. Eleman sayısı ve bütün elemanlar doğrulandıktan sonra aranan değer istenir; aranan da doğrulanmadan arama sonucu verilmez. Hata satırları alanına göre `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, `Hata: Eleman geçerli bir int tamsayı olmalıdır.` veya `Hata: Aranan geçerli bir int tamsayı olmalıdır.` olur.

## Algoritma

1. N değerini doğrulayın; N uzunluklu diziyi oluşturup bütün elemanları doğrulayarak okuyun.
2. Aranan değeri okuyup geçerli bir `int` olduğunu doğrulayın.
3. Sonuç dizinini -1 yapın.
4. Dizinleri 0'dan N - 1'e kadar sırayla dolaşın.
5. Eleman aranan değere eşitse sonuç dizinini kaydedin ve `break` ile arama döngüsünü bitirin.
6. Sonuç dizini -1 ise bulunamadı mesajını, aksi halde ilk dizini yazdırın.

Her yinelemenin başında daha önce denenmiş dizinlerin hiçbirinde eşleşme bulunmamıştır. Bu nedenle ilk doğru karşılaştırma, en küçük eşleşen dizini verir. Döngü bu noktada biter; eşit sonraki değerler sonucu değiştiremez. Başta sonuç -1'dir; eşleşme bulunmadığında değiştirilmeden kalır. Arama en fazla N karşılaştırma yapar ve dizideki değerleri değiştirmez.

4, 9, 4, -1, 9 dizisinde 9 arandığında:

| Dizin | Eleman | Eşit mi? | İşlem |
| --- | --- | --- | --- |
| 0 | 4 | Hayır | Sonraki dizine geç. |
| 1 | 9 | Evet | Sonucu 1 yap; döngüden çık. |
| 2, 3, 4 | Önceden okunmuş değerler | Karşılaştırılmaz | İlk eşleşme zaten bulunmuştur. |

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

Console.Write("Aranan: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int aranan))
{
    Console.WriteLine("Hata: Aranan geçerli bir int tamsayı olmalıdır.");
    return;
}

int dizin = -1;
for (int i = 0; i < n; i++)
{
    if (sayilar[i] == aranan)
    {
        dizin = i;
        break;
    }
}
if (dizin == -1)
{
    Console.WriteLine("Sonuç: Bulunamadı");
}
else
{
    Console.WriteLine("İlk dizin: " + dizin.ToString(CultureInfo.InvariantCulture));
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `break` yalnızca arama döngüsünden çıkar; program daha sonra sonuç satırını yazdırır. Veri doğrulamasındaki `return` tüm programı bitirir. Dizinin tamamı okunmadan ilk elemanda arama yapıp sonuç yazmak, sonraki geçersiz girdiyi gizleyebilirdi; iki aşama bu yüzden ayrıdır.

## Örnek çalıştırmalar

Girdi sırası N, N eleman ve aranan değerdir; her değer ayrı satıra yazılır. Metin blokları istemler dışındaki tam çıktıları gösterir.

N = 5; elemanlar 4, 9, 4, -1, 9; aranan 9:

```text
İlk dizin: 1
```

N = 3; elemanlar 8, 0, -8; aranan 5:

```text
Sonuç: Bulunamadı
```

N = 1; eleman -2147483648; aranan -2147483648:

```text
İlk dizin: 0
```

N = 4; elemanlar 2, 7, 0, -1; aranan -1:

```text
İlk dizin: 3
```

Uzun diziler için tam çıktı yerine aşağıdaki özellikleri denetleyin.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; elemanlar sırasıyla 0..49; aranan 49 | Son dizin 49 bulunur; 50 karşılaştırma gerekir. |
| N = 50; bütün elemanlar 2147483647; aranan 2147483647 | İlk dizin 0 bulunur; yalnızca bir karşılaştırma yapılır. |

N için 0, 51, -1, `2.5`, metin, boş satır veya giriş sonu verilirse:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

N = 2; ilk eleman 5, ikinci eleman `2147483648` ise aşağıdaki hata yazılır. Herhangi bir elemanda `-2147483649`, `3.5`, metin, boş satır veya giriş sonu aynı hatayı üretir; aranan istenmez.

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

N = 1; eleman 5; aranan `beş` ise aşağıdaki hata yazılır. Aranan değerde `2147483648`, `-2147483649`, `5.0`, boş satır veya giriş sonu da reddedilir.

```text
Hata: Aranan geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- Tek elemanda eşleşme varsa dizin 0, yoksa bulunamadı sonucu verilir.
- İlk ve son elemanlar geçerli arama konumlarıdır; döngü her iki sınırı kapsar.
- Tekrarlanan eşleşmelerde ilk dizin seçilir. Erken çıkış kaldırılıp her eşleşmede kayıt yapılırsa son dizin seçilirdi.
- Sonuç değişkenindeki -1 bir konum işaretidir; eleman değeri -1'in aranmasını engellemez.
- Sıralama şartı yoktur; arama karşılaştırma dışında elemanlara işlem uygulamaz.
- Boş veya eksik dizi, geçersiz aranan ve tür sınırını aşan tamsayılar sonuç verilmeden reddedilir.

## Kazanımlar

- Sırasız dizide doğrusal tarama ile ilk eşleşmeyi bulma.
- Geçerli dizinlerin dışında bir değerle bulunamadı durumunu temsil etme.
- Veri değeri ile o verinin dizinini ayırt etme.
- `break` kullanarak ilk eşleşmeden sonra gereksiz karşılaştırmaları durdurma.
- En iyi ve en kötü karşılaştırma adetlerini ilk ve son konum örnekleriyle açıklama.

## Alıştırmalar

1. İlk eşleşme yerine aranan değerin bütün dizinlerini yazdırın; eşleşme yoksa tek bulunamadı satırı üretin.
2. Karşılaştırma adedini bir sayaçla bulun. İlk konum, son konum ve bulunamadı durumlarının sonuçlarını karşılaştırın.
3. Diziyi son dizinden başlayarak tarayıp son eşleşmenin dizinini bulun. Tekrarlı değerlerde ileri taramadan farkını gösterin.
