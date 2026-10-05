---
id: "041"
order: 41
title: "Dizinin en küçük ve en büyük değerlerini bulma"
level: "İleri"
prerequisites: ["007", "010", "031"]
concepts: ["Dizi tarama", "Sıfır tabanlı dizin", "İlk elemanla başlatma", "En küçük ve en büyük", "İlk geçişi koruma"]
---

# Dizinin en küçük ve en büyük değerlerini bulma

## Problem tanımı

Kullanıcının girdiği 1 ile 50 arasında tamsayıyı bir dizide saklayın. Dizinin en küçük ve en büyük değerlerini, bu değerlerin ilk görüldüğü dizinlerle birlikte bulun. Elemanlar `int` türünün bütün aralığında, yani -2147483648 ile 2147483647 arasında olabilir.

Dizi dizinleri sıfırdan başlar: beş elemanın dizinleri 0, 1, 2, 3 ve 4'tür. Aynı en küçük veya en büyük değer birden çok kez bulunabilir. Örneğin 7, -2, 7, 4, -2 dizisinde en küçük değer -2'nin ilk dizini 1; en büyük değer 7'nin ilk dizini 0'dır. Eşit değerde kayıtlı dizini değiştirmeyerek ilk geçişi koruyun. Hazır en küçük/en büyük araçları yerine diziyi kendiniz tarayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; eleman sayısı. |
| Girdi | `sayilar` | `int[n]`; her eleman tam `int` aralığında. |
| Çıktı | En küçük ve dizini | `En küçük: X`, ardından `En küçük dizini: I`. |
| Çıktı | En büyük ve dizini | `En büyük: Y`, ardından `En büyük dizini: J`. |

Önce `Eleman sayısı (1..50): ` istemini yanıtlayın; sonra `Eleman[0]: ` ile başlayan istemlere her elemanı ayrı satırda girin. İstemdeki sayı elemanın sıfır tabanlı dizinidir. Basamak ayırıcı veya ondalık bölüm kullanmayın. Bütün elemanlar doğrulanmadan sonuç yazılmaz. Geçersiz eleman sayısında `Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.`, geçersiz elemanda `Hata: Eleman geçerli bir int tamsayı olmalıdır.` yazılır.

## Algoritma

1. Eleman sayısını okuyup 1..50 aralığını doğrulayın.
2. N uzunluklu diziyi oluşturun; bütün elemanları doğrulayarak doldurun.
3. En küçük ve en büyük değerleri dizinin ilk elemanı ile başlatın; iki dizini de 0 yapın.
4. Diziyi 1. dizinden son dizine kadar dolaşın.
5. Eleman kayıtlı en küçükten kesinlikle küçükse en küçük değeri ve dizinini güncelleyin.
6. Eleman kayıtlı en büyükten kesinlikle büyükse en büyük değeri ve dizinini güncelleyin.
7. Değerleri ve dizinleri belirtilen dört satırla yazdırın.

Tarama başında kayıtlı değerler, daha önce incelenmiş elemanların en küçüğü ve en büyüğüdür; kayıtlı dizinler bunların ilk görüldüğü yerlerdir. Yeni değer daha küçük veya daha büyükse ilgili kayıt değişir. Eşitlikte güncelleme yapılmadığı için ilk dizin korunur. Başlangıçta gerçek bir dizi elemanını kullanmak, bütün değerler negatif veya pozitif olduğunda da geçerli bir aday sağlar. En küçüğü 0 yapmak, yalnızca pozitif elemanlarda dizide bulunmayan yanlış bir sonuç oluşturabilirdi.

7, -2, 7, 4, -2 dizisi için tarama:

| İncelenen dizin | Değer | En küçük / ilk dizin | En büyük / ilk dizin |
| --- | --- | --- | --- |
| 0, başlangıç | 7 | 7 / 0 | 7 / 0 |
| 1 | -2 | -2 / 1 | 7 / 0 |
| 2 | 7 | -2 / 1 | 7 / 0 |
| 3 | 4 | -2 / 1 | 7 / 0 |
| 4 | -2 | -2 / 1 | 7 / 0 |

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

int enKucuk = sayilar[0], enBuyuk = sayilar[0];
int enKucukDizini = 0, enBuyukDizini = 0;
for (int i = 1; i < n; i++)
{
    if (sayilar[i] < enKucuk)
    {
        enKucuk = sayilar[i];
        enKucukDizini = i;
    }
    if (sayilar[i] > enBuyuk)
    {
        enBuyuk = sayilar[i];
        enBuyukDizini = i;
    }
}
Console.WriteLine("En küçük: " + enKucuk.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("En küçük dizini: " +
    enKucukDizini.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("En büyük: " + enBuyuk.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("En büyük dizini: " +
    enBuyukDizini.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `sayilar[0]` güvenlidir çünkü N en az 1'dir. Veri okuma ve tarama ayrı döngülerdir; geçersiz bir son elemanda bile sonuç üretilmez. `int.TryParse` tür sınırının dışını reddeder; eleman değerlerine ayrıca dar bir aralık konulmaz.

## Örnek çalıştırmalar

Her girdiyi ayrı satırda verin: önce N, sonra listelenen elemanlar. Metin blokları istemler dışındaki bütün çıktı satırlarını gösterir.

N = 5; elemanlar 7, -2, 7, 4, -2:

```text
En küçük: -2
En küçük dizini: 1
En büyük: 7
En büyük dizini: 0
```

N = 1; tek eleman -8:

```text
En küçük: -8
En küçük dizini: 0
En büyük: -8
En büyük dizini: 0
```

N = 4; elemanlar -5, -2, -9, -2:

```text
En küçük: -9
En küçük dizini: 2
En büyük: -2
En büyük dizini: 1
```

N = 3; elemanlar -2147483648, 0, 2147483647:

```text
En küçük: -2147483648
En küçük dizini: 0
En büyük: 2147483647
En büyük dizini: 2
```

Uzun dizi için tam eleman listesi yerine aşağıdaki özelliği denetleyin.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; bütün elemanlar 2147483647 | En küçük ve en büyük aynı değerdir; ikisinin ilk dizini 0'dır. |

N için 0, 51, -1, `1.5`, metin, boş satır veya giriş sonu verilirse tek çıktı:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

N = 3 sonrasında ilk eleman 7 ve ikinci eleman `iki` ise aşağıdaki hata yazılır. Aynı hata herhangi bir elemanda `2147483648`, `-2147483649`, `2.5`, boş satır veya giriş sonu için de geçerlidir; sonuç satırları yazılmaz.

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- Tek eleman hem en küçük hem en büyüktür; tarama döngüsü çalışmadan iki dizin de 0 kalır.
- Bütün değerler eşitse ilk elemanın dizini korunur. `<` yerine `<=` veya `>` yerine `>=` kullanılması son eşit elemanın dizinini seçerdi.
- Bütün değerler negatif olduğunda en büyük değer yine dizideki bir elemandır; başlangıçta 0 kullanılmaz.
- Son geçerli dizin N - 1'dir; `i < n` koşulu dizinin dışına erişimi engeller.
- Değerlere aritmetik işlem yapılmadığı için iki `int` uç değeri arasında karşılaştırma taşma oluşturmaz.
- Bütün girişler doğru olmadan taramaya başlanmaz; eksik veya hatalı dizinin kısmi sonucu yoktur.

## Kazanımlar

- Diziyi sıfır tabanlı dizinlerle doldurup ikinci döngüde tarama.
- Aday değerleri gerçek bir dizi elemanıyla başlatma.
- Bir değerle birlikte onun bulunduğu dizini tutarlı güncelleme.
- Kesin karşılaştırma kullanarak tekrarlanan değerlerin ilk geçişini koruma.
- Tek eleman, eşit elemanlar ve bütün değerlerin negatif olduğu durumları açıklama.

## Alıştırmalar

1. İlk yerine son geçişin dizinini seçen sürümü yazın; 7, -2, 7, 4, -2 dizisindeki iki dizin sonucunu karşılaştırın.
2. En küçük ve en büyük değerlerin kaç kez geçtiğini ikinci bir taramayla bulun. Bütün değerler eşitken iki adedin de N olmasını sağlayın.
3. En büyük ile en küçük arasındaki farkı hesaplayın. İki uç `int` değeri için sonucu `long` türünde güvenle üretmenin neden gerekli olduğunu açıklayın.
