---
id: "043"
order: 43
title: "Diziyi yerinde ters çevirme"
level: "İleri"
prerequisites: ["004", "010", "041", "042"]
concepts: ["Dizide yerinde değişiklik", "İki uçtan ilerleme", "Geçici değişken", "Takas", "Tek ve çift uzunluk"]
---

# Diziyi yerinde ters çevirme

## Problem tanımı

1 ile 50 tamsayı içeren dizinin eleman sırasını, aynı dizi üzerinde ters çevirin. İlk eleman sona, son eleman başa geçer. Örneğin 1, 2, 3, 4, 5 dizisi 5, 4, 3, 2, 1 olur. Elemanlar tam `int` aralığındadır; sayının kendi basamakları değil, dizideki elemanların konumları ters çevrilir.

Yeni bir sonuç dizisi oluşturmayın. Başlangıçta ilk ve son dizine yerleştirilen iki sayacı merkeze doğru ilerletin. Her adımda bu iki konumdaki değeri bir geçici değişkenle takas edin. Böylece yalnızca iki dizin ve bir geçici değer kullanılarak dizi yerinde değiştirilir. Tek uzunlukta orta eleman yerinde kalır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 50 arasında `int`; dizi uzunluğu. |
| Girdi | `sayilar` | Her biri tam `int` aralığında N eleman. |
| Çıktı | Ters çevrilmiş dizi | Tek satır; `Dizi:` ardından her eleman için bir boşluk ve değer. |

Önce `Eleman sayısı (1..50): ` istemini, sonra `Eleman[0]: ` ile başlayan N istemi yanıtlayın. Her tamsayı ayrı satırda girilir. Dizinler sıfırdan N - 1'e kadar gider. Sonuç satırında elemanlar arasında tam bir boşluk bulunur; satır sonunda fazladan boşluk yoktur. Bütün girdiler doğrulanmadan takas ve çıktı yapılmaz.

## Algoritma

1. N değerini doğrulayın; diziyi oluşturup bütün elemanları doğrulayarak doldurun.
2. `sol` değerini 0, `sag` değerini N - 1 yapın.
3. `sol < sag` olduğu sürece soldaki değeri geçici değişkene alın.
4. Soldaki elemana sağdaki değeri, sağdaki elemana geçici değeri atayın.
5. Sol dizini bir artırıp sağ dizini bir azaltın; 3. adımdan devam edin.
6. Döngü bitince dizinin bütün elemanlarını sırasıyla yazdırın.

Döngü başında `sol` değerinin solunda ve `sag` değerinin sağında kalan elemanlar doğru son konumlarına yerleşmiştir. Henüz çevrilmemiş bölüm iki dizin arasında kalır. Takas bu bölümün iki ucunu tamamlar; dizinlerin ilerletilmesi kalan bölümü daraltır. Dizinler buluşunca veya birbirini geçince tüm gerekli takaslar bitmiştir. N / 2 tamsayı bölümü kadar takas yapılır.

1, 2, 3, 4, 5 dizisindeki adımlar:

| Sol | Sağ | İşlem sonrası dizi |
| --- | --- | --- |
| 0 | 4 | 5, 2, 3, 4, 1 |
| 1 | 3 | 5, 4, 3, 2, 1 |
| 2 | 2 | Koşul yanlış; orta değer değiştirilmez. |

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

int sol = 0, sag = n - 1;
while (sol < sag)
{
    int gecici = sayilar[sol];
    sayilar[sol] = sayilar[sag];
    sayilar[sag] = gecici;
    sol++;
    sag--;
}
Console.Write("Dizi:");
for (int i = 0; i < n; i++)
{
    Console.Write(" " + sayilar[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Geçici değişken soldaki eski değeri korur; aksi halde ilk atama sonrasında bu değer kaybolurdu. Tüm değerler yalnızca taşınır, toplanmaz; bu nedenle iki uç `int` değerinin takası taşma oluşturmaz. Girdi dizisine ek olarak ikinci bir dizi ayrılmaz.

## Örnek çalıştırmalar

Girdiler N ve ardından N elemandır; her biri ayrı satırda verilir. Bloklar istemler dışındaki tam sonucu gösterir.

N = 5; elemanlar 1, 2, 3, 4, 5:

```text
Dizi: 5 4 3 2 1
```

N = 4; elemanlar -3, 0, 8, 8:

```text
Dizi: 8 8 0 -3
```

N = 1; eleman -2147483648:

```text
Dizi: -2147483648
```

N = 2; elemanlar -2147483648, 2147483647:

```text
Dizi: 2147483647 -2147483648
```

Uzun dizi için aşağıdaki özellikleri denetleyin; tablo tam dizi çıktısı değildir.

| Girdi | Beklenen özellik |
| --- | --- |
| N = 50; elemanlar sırasıyla 0..49 | 25 takas; sonuçta 49..0 sırası; ilk eleman 49, son eleman 0. |

N için 0, 51, -1, `2.5`, metin, boş satır veya giriş sonu verilirse:

```text
Hata: Eleman sayısı 1 ile 50 arasında olmalıdır.
```

N = 2; ilk eleman 7, ikinci eleman `2147483648` ise aşağıdaki hata yazılır. Herhangi bir elemanda `-2147483649`, `1.0`, metin, boş satır veya giriş sonu aynı hatayı üretir. Kısmi dizi ters çevrilmez ve `Dizi:` satırı yazılmaz.

```text
Hata: Eleman geçerli bir int tamsayı olmalıdır.
```

## Sınır durumları

- N = 1 için sol ve sağ aynı dizindir; döngü hiç çalışmaz, tek eleman korunur.
- Çift uzunlukta dizinler birbirini geçer; tek uzunlukta orta dizinde buluşur. İki durumda da `sol < sag` koşulu biter.
- `sol <= sag` kullanılması tek uzunlukta orta elemanı kendisiyle gereksiz yere takas ederdi.
- Aynı değerler takas edildiğinde görünüm değişmeyebilir; işlem yine doğru konum çiftini ele alır.
- Her iki dizin de yalnızca döngü koşulu doğruyken kullanılır; dizinin dışına erişilmez.
- Ters çevirme iki kez uygulanırsa başlangıç sırası geri gelir. Değerlerin tekrarlanması bu özelliği bozmaz.
- Hatalı, aralık dışı veya eksik girdide sonuç verilmeden program biter.

## Kazanımlar

- Diziyi ikinci bir dizi oluşturmadan yerinde dönüştürme.
- İlk ve son dizinden merkeze doğru ilerleyen iki sayaç kullanma.
- Değer kaybını geçici değişkenli takasla önleme.
- Tek ve çift uzunlukta döngünün bitiş konumlarını açıklama.
- Takas adedini dizi uzunluğunun tamsayı yarısıyla ilişkilendirme.

## Alıştırmalar

1. Yalnızca iki kullanıcı dizini arasındaki bölümü ters çevirin; dizinleri doğrulayın ve bölümün dışındaki elemanları koruyun.
2. Takas adedini sayıp N = 1, 4 ve 5 için sırasıyla 0, 2 ve 2 sonucunu doğrulayın.
3. Aynı diziye ters çevirme döngüsünü iki kez uygulayın. Tek, çift ve tekrarlı elemanlı üç örnekte ilk sıranın geri geldiğini gösterin.
