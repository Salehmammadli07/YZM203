---
id: "067"
order: 67
title: "Ardışık harfleri adetleriyle kodlama"
level: "İleri"
prerequisites: ["059", "063", "066"]
concepts: ["Ardışık grup", "Kodlama kuralı", "Çok basamaklı adet", "Çıktı uzunluğu", "Sıkıştırma sınırı"]
---

# Ardışık harfleri adetleriyle kodlama

## Problem tanımı

Yalnız A–Z ve a–z harflerinden oluşan, en fazla 200 karakterlik satırı ardışık grup uzunluklarıyla kodlayın. Her grubun harfini hemen ardından pozitif ondalık adediyle yazın; adet 1 olsa da yazılmalıdır. Büyük/küçük harf farklıdır. Örneğin `AAAbbA` sonucu `A3b2A1` olur. Grup sayısını ve kodun karakter uzunluğunu da bildirin.

Boş satır geçerlidir ve boş kod üretir. Rakam, boşluk veya diğer karakterler giriş değildir; böylece harf ile adet sınırı belirsiz olmaz. Grupların sırası korunur; `aba` içindeki iki a birleştirilmez. Bu temsil ardışık tekrarları kısaltabilir, ancak her girdiyi küçültmez: `ab` sonucu `a1b1` dört karakterdir. Hazır gruplama veya sıkıştırma aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 ASCII harfi; harf büyüklüğü korunur. |
| Çıktı | Kod | `Kod:` ardından dikey çubuklar arasında harf-adet çiftleri; çubuklar koda dahil değildir. |
| Çıktı | Sayaçlar | `Gruplar: G`, ardından `Kod uzunluğu: L`; ayraçlar L'ye dahil değildir. |

`Metin: ` istemini tek satırla yanıtlayın. EOF, fazla uzunluk, rakam, boşluk, Türkçe harf veya diğer karakter için yalnız `Hata: Metin 0..200 ASCII harfi içermelidir.` yazılır. Satırın tamamı kod çıktısı başlamadan önce doğrulanır.

## Algoritma

1. Satırı ve bütün karakterlerini doğrulayın.
2. Tarama dizini, grup sayısı ve kod uzunluğunu 0 yapın.
3. Mevcut harfi saklayın; aynı harf sürerken adedi ve dizini artırın.
4. Harfi ve grubun adedini yan yana yazın; grup sayısını artırın.
5. Adedin bir kopyasını 10'a bölerek basamak adedini bulun; kod uzunluğuna 1 harf ve basamak adedini ekleyin.
6. Sonraki farklı harfle yeni gruba başlayın; giriş bitince kodu kapatıp sayaçları yazdırın.

Grup taramasında yalnız mevcut harfe eşit karakterler tüketilir; ilk farklı karakter sonraki grubun başlangıcı olarak bırakılır. Her karakter bir gruba girer, grup adetlerinin toplamı giriş uzunluğudur. Bir grup kodda 1 harf ve adedin ondalık basamak sayısı kadar karakter üretir. Tarama dizini sürekli arttığı için sonlanır.

Adet 9'dan 10'a veya 99'dan 100'e çıktığında koddaki basamak adedi bir artar. Bu nedenle grup sayısı aynı kalsa da kod uzunluğu değişebilir. Grup adedi ile kodun karakter uzunluğunu ayrı sayaçlarda tutmak bu iki bilgiyi karıştırmayı önler.

`AAAbbA` için:

| Grup | Adet | Kod parçası | Birikimli kod uzunluğu |
| --- | --- | --- | --- |
| A | 3 | A3 | 2 |
| b | 2 | b2 | 4 |
| A | 1 | A1 | 6 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Metin: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: Metin 0..200 ASCII harfi içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    char c = metin[i];
    if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
    {
        Console.WriteLine("Hata: Metin 0..200 ASCII harfi içermelidir.");
        return;
    }
}

int dizin = 0, grup = 0, kodUzunlugu = 0;

Console.Write("Kod: |");

while (dizin < metin.Length)
{
    char harf = metin[dizin];
    int adet = 0;
    while (dizin < metin.Length && metin[dizin] == harf)
    {
        adet++;
        dizin++;
    }
    Console.Write(harf);
    Console.Write(adet.ToString(CultureInfo.InvariantCulture));
    grup++;

    int kopya = adet, basamak = 1;
    while (kopya >= 10)
    {
        kopya /= 10;
        basamak++;
    }
    kodUzunlugu += 1 + basamak;
}
Console.WriteLine("|");
Console.WriteLine("Gruplar: " + grup.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Kod uzunluğu: " +
    kodUzunlugu.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Doğrulama ayrı bir ilk geçiş olduğu için sonradan görülen hatalı karakter yarım kod çıktısı üretmez. Sayıların `ToString` kullanımı yalnız çıktı biçimlendirmesidir; grup bulma ve uzunluk hesabı döngülerle yapılır. En uzun kod 200 tekli grup için 400 karakterdir.

## Örnek çalıştırmalar

Bloklar `Metin: ` isteminden sonraki tam sonuçtur.

`AAAbbA`:

```text
Kod: |A3b2A1|
Gruplar: 3
Kod uzunluğu: 6
```

`abab`:

```text
Kod: |a1b1a1b1|
Gruplar: 4
Kod uzunluğu: 8
```

11 adet A:

```text
Kod: |A11|
Gruplar: 1
Kod uzunluğu: 3
```

Boş satır:

```text
Kod: ||
Gruplar: 0
Kod uzunluğu: 0
```

| Girdi | Beklenen özellik |
| --- | --- |
| 200 adet z | Kod z200; 1 grup, kod uzunluğu 4. |
| a ve b dönüşümlü 200 harf | 200 grup, kod uzunluğu 400; temsil daha uzun. |
| Aa | A1a1; büyük/küçük harf iki ayrı grup. |
| A1, boşluk, ç, 201 karakter veya EOF | Yalnız tanımlanan hata satırı; kod başlangıcı bile yazılmaz. |

## Sınır durumları

- Adet 1 için rakam atlanmaz; açma kuralı bunu gerektirir.
- Boş kod ile hatalı giriş farklı durumlardır.
- Aynı harfin ayrı grupları yer değiştirmez veya birleştirilmez.
- 9, 10, 99, 100 ve 200 adetlerinde kod uzunluğu basamak sayısına bağlıdır.
- Kod ayraçları ve başlıklar ölçülen kod uzunluğuna dahil değildir.
- Bu temsil her metinde daha az yer kullanma garantisi vermez.

## Kazanımlar

- Önceki grup sayma yaklaşımını sıralama yapmadan ardışık veriye uygulama.
- Bir kodlama biçiminin açık ve çözülebilir kurallarını tanımlama.
- Grubun bitiş karakterini tüketmeden sonraki gruba bırakma.
- Çıktı uzunluğunu algoritmanın ürettiği bileşenlerden hesaplama.
- Tekrar biçiminin sıkıştırmanın yararını nasıl değiştirdiğini açıklama.

## Alıştırmalar

1. Giriş uzunluğu ile kod uzunluğu arasındaki farkı yazdırın; farkın negatif olabildiğini gösterin.
2. Kod yerine her grubun başlangıç dizini, harfi ve adedini birer satırda yazdırın.
3. A ile a'yı aynı kabul eden sürüm yazın; bunun özgün büyük/küçük harf bilgisini neden kaybettirdiğini açıklayın.
