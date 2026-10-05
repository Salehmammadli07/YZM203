---
id: "013"
order: 13
title: "Atölyeye katılım uygunluğunu belirleme"
level: "Temel"
prerequisites: ["011", "012"]
concepts: ["Mantıksal VE", "Mantıksal VEYA", "Parantez", "Karar tablosu", "Birleşik koşul"]
---

# Atölyeye katılım uygunluğunu belirleme

## Problem tanımı

Örnek bir programlama atölyesi için bir adayın katılım koşulunu sağlayıp sağlamadığını belirleyin. Bu alıştırmada adayın yaşı en az 18 olmalıdır. Ayrıca aday ya ön hazırlık çalışmasını tamamlamış olmalı ya da hazırlık değerlendirmesinden en az 70 puan almış olmalıdır.

Ön hazırlığın tamamlanması ve değerlendirme puanı iki alternatif koşuldur; ikisinin birden sağlanması gerekmez. Yaş koşulu ise her durumda zorunludur. Kurallar yalnızca bu alıştırmadaki kurgusal atölyeye aittir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `yas` | 0 ile 120 arasında tamsayı. |
| Girdi | `onHazirlik` | Tamamlandıysa `1`, tamamlanmadıysa `0`. |
| Girdi | `puan` | 0 ile 100 arasında tamsayı değerlendirme puanı. |
| Çıktı | Uygunluk | `Sonuç: Katılabilir` veya `Sonuç: Katılamaz`. |

`Yaş (0..120): `, `Ön hazırlık tamamlandı mı (0/1): ` ve `Değerlendirme puanı (0..100): ` istemlerine sırayla ayrı satırlarda yanıt verin. Bütün girdiler doğrulanır; ön hazırlık tamamlanmış olsa da değerlendirme puanı istenir. Geçersiz girişte hata yazılır ve sonraki girdiler istenmeden program sona erer.

## Algoritma

1. Yaşı okuyun ve 0 ile 120 arasında tamsayı olduğunu doğrulayın.
2. Ön hazırlık durumunu okuyun ve yalnızca 0 veya 1 olduğunu doğrulayın.
3. Değerlendirme puanını okuyun ve 0 ile 100 arasında tamsayı olduğunu doğrulayın.
4. `yas >= 18` ile yaş koşulunu kurun.
5. `onHazirlik == 1 || puan >= 70` ile iki alternatif hazırlık koşulunu kurun.
6. Yaş koşulunu hazırlık koşuluyla `&&` kullanarak birleştirin.
7. Birleşik koşul doğruysa katılabilir, yanlışsa katılamaz sonucunu yazdırın.

`&&` iki koşulun da doğru olmasını ister. `||` en az bir koşul doğru olduğunda doğrudur. Parantez, iki hazırlık seçeneğinin birlikte değerlendirilmesini açıkça gösterir.

| Yaş en az 18 mi? | Ön hazırlık tamam mı? | Puan en az 70 mi? | Karar |
| --- | --- | --- | --- |
| Hayır | Hayır | Hayır | Katılamaz |
| Hayır | Hayır | Evet | Katılamaz |
| Hayır | Evet | Hayır | Katılamaz |
| Hayır | Evet | Evet | Katılamaz |
| Evet | Hayır | Hayır | Katılamaz |
| Evet | Hayır | Evet | Katılabilir |
| Evet | Evet | Hayır | Katılabilir |
| Evet | Evet | Evet | Katılabilir |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Yaş (0..120): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int yas) || yas < 0 || yas > 120)
{
    Console.WriteLine("Hata: Yaş 0 ile 120 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Ön hazırlık tamamlandı mı (0/1): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int onHazirlik) ||
    (onHazirlik != 0 && onHazirlik != 1))
{
    Console.WriteLine("Hata: Ön hazırlık için 0 veya 1 girin.");
    return;
}

Console.Write("Değerlendirme puanı (0..100): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int puan) || puan < 0 || puan > 100)
{
    Console.WriteLine("Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.");
    return;
}

bool uygun = yas >= 18 && (onHazirlik == 1 || puan >= 70);
if (uygun)
{
    Console.WriteLine("Sonuç: Katılabilir");
}
else
{
    Console.WriteLine("Sonuç: Katılamaz");
}
```

Kod, bir konsol projesinin `Program.cs` dosyasına tek başına konabilir. `==` eşitliği, `!=` eşit olmamayı denetler. C# dilinde `&&`, `||` işlecinden önce değerlendirilir. Parantez kaldırılıp `yas >= 18 && onHazirlik == 1 || puan >= 70` yazılsaydı yüksek puanlı 17 yaşındaki bir aday da kabul edilirdi. Parantez bu hatayı engeller.

Mantıksal işleçler kısa devre yapar: `&&` sol tarafı yanlışsa sağ tarafı değerlendirmez; `||` sol tarafı doğruysa sağ tarafı değerlendirmez. Burada bütün girdilerin önce alınması ve doğrulanması, kısa devre nedeniyle bir girdinin kontrol dışı kalmasını önler.

## Örnek çalıştırmalar

Tablo, istemlerden sonra yazılan tam satırı gösterir. `—`, hata nedeniyle henüz istenmeyen girdidir.

| Yaş | Ön hazırlık | Puan | Sonuç satırı |
| --- | --- | --- | --- |
| `18` | `0` | `70` | `Sonuç: Katılabilir` |
| `18` | `0` | `69` | `Sonuç: Katılamaz` |
| `18` | `1` | `0` | `Sonuç: Katılabilir` |
| `17` | `1` | `100` | `Sonuç: Katılamaz` |
| `17` | `0` | `70` | `Sonuç: Katılamaz` |
| `25` | `1` | `85` | `Sonuç: Katılabilir` |
| `0` | `1` | `100` | `Sonuç: Katılamaz` |
| `120` | `0` | `100` | `Sonuç: Katılabilir` |
| `121` | `—` | `—` | `Hata: Yaş 0 ile 120 arasında bir tamsayı olmalıdır.` |
| `20` | `2` | `—` | `Hata: Ön hazırlık için 0 veya 1 girin.` |
| `20` | `1` | `101` | `Hata: Puan 0 ile 100 arasında bir tamsayı olmalıdır.` |
| `yirmi` | `—` | `—` | `Hata: Yaş 0 ile 120 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- 18 yaş dahil edilir, 17 yaş dahil edilmez; yüksek puan yaş şartını kaldırmaz.
- 70 puan yeterlidir; 69 puan ancak ön hazırlık tamamlanmışsa yeterlidir.
- Ön hazırlık tamamlandıysa sıfır puanla katılım mümkündür, ancak puanın geçerli aralıkta olması yine zorunludur.
- Ön hazırlık için 0 ve 1 dışında değerler, `evet` gibi metinler ve boş giriş reddedilir.
- Yaş ile puan için sınırlar dahil edilir; ondalıklı girdiler tamsayıya dönüştürülmez.
- Adayın koşulu sağlamaması normal bir karar sonucudur; biçimi geçersiz girdiden doğan hata ile aynı şey değildir.

## Kazanımlar

- Zorunlu koşulu alternatif koşullarla `&&` ve `||` kullanarak birleştirme.
- Parantezlerin bir uygunluk kuralının anlamını nasıl değiştirdiğini örnekle gösterme.
- Üç mantıksal değişken için sekiz satırlık karar tablosunu yorumlama.
- Geçerli fakat uygun olmayan bir girdiyi geçersiz girdiden ayırt etme.

## Alıştırmalar

1. Atölye için iki hazırlık koşulunun da sağlanmasını isteyin; değişen karar tablosu satırlarını bulun.
2. Yaş üst sınırını katılım kuralında 65 yapın; 65 ve 66 yaşları için test ekleyin. Girdi doğrulama aralığının neden aynı kalabileceğini açıklayın.
3. Parantezsiz hatalı koşulu deneyin ve 17 yaş, ön hazırlık 0, puan 100 girdisinin iki sürümdeki sonuçlarını karşılaştırın.
