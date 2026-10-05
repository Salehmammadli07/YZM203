---
id: "022"
order: 22
title: "Basamak sayısını ve basamak toplamını bulma"
level: "Orta"
prerequisites: ["006", "018", "021"]
concepts: ["do / while döngüsü", "Tamsayı bölmesi", "Kalan işleci", "Basamak ayırma", "Döngü değişmezi"]
---

# Basamak sayısını ve basamak toplamını bulma

## Problem tanımı

0 ile 2147483647 arasında girilen bir tamsayının onluk yazımındaki basamak sayısını ve basamaklarının toplamını bulun. Sayıyı metne çevirerek karakterlerini dolaşmayın; her adımda `% 10` ile son basamağı alın ve `/ 10` ile bu basamağı sayıdan çıkarın.

Örneğin 407 üç basamaklıdır ve basamak toplamı 4 + 0 + 7 = 11'dir. Sıfırın onluk yazımı tek basamaklıdır; basamak toplamı 0'dır. Giriş sayısal olarak yorumlanır: `00407` yazılırsa 407 değeri işlenir ve sonuç üç basamaktır. Negatif sayılar kabul edilmez; böylece eksi işareti ve negatif kalanların yorumu bu problemde yer almaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `sayi` | 0 ile 2147483647 arasında `int`. |
| Ara değer | `kalanSayi` | Basamakları çıkarıldıkça küçülen kopya. |
| Çıktı | `basamakSayisi` | Onluk yazımın basamak adedi; `Basamak sayısı: N`. |
| Çıktı | `basamakToplami` | Basamak değerlerinin toplamı; `Basamak toplamı: N`. |

`Sayı (0..2147483647): ` istemine bir tamsayı girin. Biçimi veya aralığı yanlışsa `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` yazılır ve sonuç verilmez. Sıfır için basamak sayısı 1 olarak kabul edilir.

## Algoritma

1. Sayıyı okuyup biçimini ve aralığını doğrulayın.
2. Sayıyı `kalanSayi` değişkenine kopyalayın; sayaç ve toplamı 0 yapın.
3. `do` gövdesinde `kalanSayi % 10` ile son basamağı bulun.
4. Bu basamağı toplama ekleyin ve basamak sayısını bir artırın.
5. `kalanSayi /= 10` ile işlenen son basamağı çıkarın.
6. `kalanSayi > 0` olduğu sürece 3. adımdan devam edin.
7. Basamak sayısını ve toplamını yazdırın.

`do / while`, koşulu gövdeden sonra denetler; gövde en az bir kez çalışır. Bu nedenle 0 için de bir basamak işlenir. Her yineleme sonunda sayaç, çıkarılmış basamakların adedidir; toplam ise aynı basamakların toplamıdır. Kalan sayı, henüz işlenmemiş soldaki basamakları temsil eder. Pozitif bir kalan sayı 10'a tamsayı bölününce küçülür; en fazla 10 yinelemede sıfıra ulaşılır.

407 için ara değerler aşağıdaki gibidir. Tablo her yinelemenin sonundaki sayaç ve toplamı gösterir.

| Yineleme | İşlem öncesi kalan sayı | Alınan basamak | Yeni kalan sayı | Basamak sayısı | Basamak toplamı |
| --- | --- | --- | --- | --- | --- |
| 1 | 407 | 7 | 40 | 1 | 7 |
| 2 | 40 | 0 | 4 | 2 | 7 |
| 3 | 4 | 4 | 0 | 3 | 11 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Sayı (0..2147483647): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int sayi) || sayi < 0)
{
    Console.WriteLine("Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.");
    return;
}

int kalanSayi = sayi;
int basamakSayisi = 0;
int basamakToplami = 0;

do
{
    int basamak = kalanSayi % 10;
    basamakToplami += basamak;
    basamakSayisi++;
    kalanSayi /= 10;
}
while (kalanSayi > 0);

Console.WriteLine("Basamak sayısı: " +
    basamakSayisi.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Basamak toplamı: " +
    basamakToplami.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `int.TryParse` üst tür sınırını aşan girdiyi reddeder. `/=` değişkeni bölümün tamsayı sonucuyla günceller. Örneğin 407 / 10 işlemi 40 olur; `% 10` ise 7 verir. Asıl `sayi` değiştirilmediği için daha sonra başlangıç değeri gerektiğinde kullanılabilir.

## Örnek çalıştırmalar

Sonuç sütunu giriş isteminden sonraki iki satırı veya hata satırını tam olarak gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `407` | `Basamak sayısı: 3`<br>`Basamak toplamı: 11` | Aradaki sıfır da bir basamaktır. |
| `0` | `Basamak sayısı: 1`<br>`Basamak toplamı: 0` | Gövde en az bir kez çalışır. |
| `7` | `Basamak sayısı: 1`<br>`Basamak toplamı: 7` | Tek basamak. |
| `10` | `Basamak sayısı: 2`<br>`Basamak toplamı: 1` | Sondaki sıfır sayaca katılır. |
| `1000` | `Basamak sayısı: 4`<br>`Basamak toplamı: 1` | Birden çok sıfır basamak. |
| `999` | `Basamak sayısı: 3`<br>`Basamak toplamı: 27` | Tekrarlanan basamaklar. |
| `00407` | `Basamak sayısı: 3`<br>`Basamak toplamı: 11` | Başta yazılan sıfırlar sayısal değeri değiştirmez. |
| `2147483647` | `Basamak sayısı: 10`<br>`Basamak toplamı: 46` | En büyük geçerli girdi. |
| `-1` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Negatif sayı reddedilir. |
| `2147483648` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | `int` aralığı aşılır. |
| `4.5` | `Hata: 0 ile 2147483647 arasında bir tamsayı giriniz.` | Ondalıklı yazım kabul edilmez. |

## Sınır durumları

- Başlangıç değeri 0 olsa da `do / while` bir yineleme yapar. Yalnızca başta koşul denetleyen bir `while`, ek bir sıfır kuralı olmadan yanlışlıkla sıfır basamak bulurdu.
- Sayının içindeki veya sonundaki sıfırlar toplamı değiştirmez; basamak sayısını artırır.
- Girdi en fazla 10 basamaklıdır. Her basamak en fazla 9 olduğu için basamak toplamı en fazla 90 olabilecek bir üst sınırla güvence altındadır; her iki sonuç `int` içinde kalır.
- `kalanSayi` sıfıra ulaştığında işlem biter; başlangıç sayısı korunur.
- Negatif, aralık dışı, metin, boş satır ve giriş sonu için hata yazılır; basamak sonuçları verilmez.

## Kazanımlar

- `% 10` ve `/ 10` ile onluk basamakları aritmetik yoldan ayırma.
- En az bir işlem gerektiren bir durumda `do / while` seçme.
- Bir yineleme tablosuyla sayaç, biriktirici ve kalan sayıyı birlikte izleme.
- Asıl girdiyi korumak için çalışma kopyası kullanma.
- Döngünün sonlanmasını küçülen bir ara değerle açıklama.

## Alıştırmalar

1. Tek ve çift basamak adetlerini ayrı sayaçlarda bulun. Sıfır basamağının çift sayılması gerektiğini örneklerle kontrol edin.
2. Basamaklar arasındaki en büyük değeri de hesaplayın. 0, 407 ve 999 girdileri için başlangıç değerinizin neden doğru olduğunu açıklayın.
3. Aynı çözümü `while` döngüsüyle yazın; 0 için tek basamak kuralını ayrıca uygulayın ve iki sürümün sonuçlarını karşılaştırın.
