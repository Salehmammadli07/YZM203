---
id: "039"
order: 39
title: "Pisagor üçlülerini üretme"
level: "İleri"
prerequisites: ["031", "037", "038"]
concepts: ["Üç iç içe döngü", "Aday uzayı", "Sıralama kısıtı", "Tekrarsız üretim", "Pisagor eşitliği"]
---

# Pisagor üçlülerini üretme

## Problem tanımı

Hipotenüsü verilen üst sınırı aşmayan bütün pozitif tamsayı Pisagor üçlülerini üretin. Bir üçlü `a * a + b * b = c * c` koşulunu sağlamalıdır; `a` ve `b` dik kenarlar, `c` hipotenüstür. Örneğin `3 4 5`, `9 + 16 = 25` olduğu için bir sonuçtur.

Her üçlüyü yalnızca bir kez yazmak için `a < b < c` düzenini kullanın. `3 4 5` yazılırken `4 3 5` ayrıca yazılmaz. Ortak çarpanı olan üçlüler de kabul edilir; `6 8 10`, `3 4 5` üçlüsünün iki katı olsa da ayrı bir geçerli sonuçtur. Dış döngü önce `a`, orta döngü `b`, iç döngü `c` değerini ilerletir. Çıktı bu döngü sırasındadır; hipotenüse göre sıralanmış olması gerekmez.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 100 arasında `int`; hipotenüs için dahil edilen üst sınır. |
| Çıktı | Üçlüler | `Pisagor üçlüleri:` başlığının ardından her satırda `a b c`; sonuç yoksa bir satır `Yok`. |
| Çıktı | `adet` | Son satırda `Adet: N`; bulunan farklı sıralı üçlü sayısı. |

`Hipotenüs üst sınırı (1..100): ` istemine tek tamsayı girin. Üçlülerde sayılar birer boşlukla ayrılır. Sınır 5'ten küçük olduğunda geçerli girişe rağmen sonuç bulunmaz. Hatalı girişte `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` yazılır; liste başlığı üretilmez.

## Algoritma

1. Üst sınırı okuyup 1..100 aralığını doğrulayın.
2. Adedi sıfır yapıp liste başlığını yazdırın.
3. `a` değerini 1'den `n - 2` dahil olacak şekilde artırın.
4. Her `a` için `b` değerini `a + 1` ile `n - 1` arasında artırın.
5. Her `a`, `b` çifti için `c` değerini `b + 1` ile `n` arasında artırın.
6. Pisagor eşitliği sağlanıyorsa üçlüyü yazıp adedi artırın.
7. Bütün döngüler bitince adet sıfırsa `Yok` yazın.
8. Adedi sonuç satırında yazdırın.

Başlangıç sınırları her adayda `a < b < c <= n` ilişkisini kurar. Uygun aralıktaki her artan üçlü bu döngülerde tam bir kez denenir. Bu nedenle kenarları yer değiştirerek aynı çözüm tekrar üretilmez. Bir `a` değerine ait bütün `b`, `c` adayları tamamlandıktan sonra `a` artar. Sayaçların her biri sonlu aralıkta arttığı için üç döngü de sonlanır.

Üst sınır 13 olduğunda `3 4 5`, ardından `5 12 13`, ardından `6 8 10` yazılır. Sonucun hipotenüsleri 5, 13, 10 sırasındadır; sıralamayı hipotenüs değil dıştaki `a` belirler.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Hipotenüs üst sınırı (1..100): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 100)
{
    Console.WriteLine("Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.");
    return;
}
int adet = 0;
Console.WriteLine("Pisagor üçlüleri:");
for (int a = 1; a <= n - 2; a++)
{
    for (int b = a + 1; b <= n - 1; b++)
    {
        for (int c = b + 1; c <= n; c++)
        {
            if (a * a + b * b == c * c)
            {
                Console.WriteLine(a.ToString(CultureInfo.InvariantCulture) + " " +
                    b.ToString(CultureInfo.InvariantCulture) + " " +
                    c.ToString(CultureInfo.InvariantCulture));
                adet++;
            }
        }
    }
}
if (adet == 0)
{
    Console.WriteLine("Yok");
}
Console.WriteLine("Adet: " + adet.ToString(CultureInfo.InvariantCulture));
```

Karekök hesaplamak yerine her hipotenüs adayının karesi karşılaştırılır. `b = a + 1` ve `c = b + 1` başlangıçları, bir son koşulla elenecek tekrarları en baştan aday uzayının dışına çıkarır. Doğrulama sonucunda sınır en fazla 100 olduğu için bütün kareler ve toplamları `int` içinde kalır.

## Örnek çalıştırmalar

Metin blokları istemden sonraki bütün çıktı satırlarını gösterir.

Girdi `1` için tam çıktı:

```text
Pisagor üçlüleri:
Yok
Adet: 0
```

Girdi `5` için tam çıktı:

```text
Pisagor üçlüleri:
3 4 5
Adet: 1
```

Girdi `13` için tam çıktı:

```text
Pisagor üçlüleri:
3 4 5
5 12 13
6 8 10
Adet: 3
```

Hacimli çıktılar için aşağıdaki tablo tam liste değildir; doğrulanacak özellikleri özetler.

| Girdi | Beklenen özellik veya denetim |
| --- | --- |
| `4` | Hiç üçlü bulunmaz; adet 0'dır. |
| `20` | Altı üçlü bulunur; katlar dahildir; son satır `Adet: 6`. |
| `100` | 52 üçlü bulunur; bütün adaylarda `a < b < c <= 100`; son satır `Adet: 52`. |

Aşağıdaki örneklerin sonucu tek hata satırıdır.

| Girdi | Tam sonuç |
| --- | --- |
| `0` | `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` |
| `101` | `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` |
| `üçlü` | `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` |
| `5.5` | `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` |
| Boş satır | `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` |
| Hiç satır vermeden girişin sonu | `Hata: Üst sınır 1 ile 100 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- `n = 1` veya `n = 2` olduğunda dış döngü bile çalışmaz; başlık, `Yok` ve adet 0 yazılır.
- 3 ve 4 sınırlarında aday denenebilir, ancak en küçük Pisagor üçlüsünün hipotenüsü 5'tir; sonuç bulunmaz.
- Üst sınır dahil olduğundan `n = 5` için `3 4 5` yazılır.
- Kat üçlüler dışlanmaz; yalnızca aynı kenarların farklı sıralamaları dışlanır.
- `a`, `b`, `c` en fazla 100 olabilir. Bir kare en fazla 10000, iki kare toplamı en fazla 20000 olur; `int` taşmaz.
- Üst sınırda en fazla `100 * 99 * 98 / 6 = 161700` artan üçlü adayı vardır; adet `int` aralığındadır.
- Boş satır, giriş sonu, metin, ondalık sayı ve aralık dışı girişlerde tek hata yazılır.

## Kazanımlar

- Üç bağımsız aday değişkenini üç iç içe döngüyle üretme.
- Başlangıç ve bitiş sınırlarıyla `a < b < c` ilişkisini kurma.
- Tekrarlı çözümleri sonradan silmek yerine aday üretiminden dışlama.
- Çıktı sırasını dış, orta ve iç döngülerin sırasından çıkarma.
- Girdi üst sınırından karelerin ve toplamın güvenli sayı aralığını hesaplama.

## Alıştırmalar

1. Çözüm sayacına ek olarak denenen aday üçlü sayısını yazdırın; 5 için 10 adayın denendiğini doğrulayın.
2. Döngü sırasını önce `c`, sonra `a`, sonra `b` olacak biçimde değiştirin; aynı üçlülerin hipotenüse göre sıralandığını kontrol edin.
3. 028 numaralı problemdeki Öklid algoritmasını her bulunan üçlüye uygulayın; üç kenarı aralarında asal olan üçlüleri filtreleyerek 6, 8, 10 gibi katları dışlayın.
