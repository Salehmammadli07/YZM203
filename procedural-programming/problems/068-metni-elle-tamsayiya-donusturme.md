---
id: "068"
order: 68
title: "Metni taşma denetimiyle elle tamsayıya dönüştürme"
level: "İleri"
prerequisites: ["022", "057", "061", "067"]
concepts: ["Giriş dilbilgisi", "İsteğe bağlı işaret", "Basamak biriktirme", "Çarpmadan önce sınır", "Asimetrik int aralığı"]
---

# Metni taşma denetimiyle elle tamsayıya dönüştürme

## Problem tanımı

Bir satırı hazır sayı dönüştürme aracı kullanmadan C# `int` değerine dönüştürün. Kabul edilen biçim, isteğe bağlı tek `+` veya `-` işaretinin ardından en az bir ASCII rakamıdır. Satır 1..11 karakter olmalıdır; boşluk, ondalık ayraç ve başka karakter kabul edilmez. Baştaki sıfırlar geçerlidir. Örneğin `+0042` sonucu 42, `-0` sonucu 0 olur.

Sayısal değer -2147483648..2147483647 aralığında olmalıdır. Negatif sınırın büyüklüğü pozitif sınırdan 1 fazladır. Basamakları negatif olmayan `long` büyüklükte biriktirin; işarete göre sınırı belirleyin ve `buyukluk * 10 + basamak` işleminden önce yeni değerin sınırı aşmayacağını doğrulayın. `Parse`, `TryParse`, `Convert`, hazır sayısal ayrıştırma veya taşmayı yakalamak için istisna kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 1..11 karakter; belirtilen işaret ve rakam biçimi. |
| Ara değer | `buyukluk` | İşaretsiz sayısal büyüklük; `long`, en fazla 2147483648. |
| Çıktı | Tamsayı | Tek satır `Değer: X`; işaret ve baştaki sıfırlar normalleşir. |

`Tamsayı metni: ` istemini bir satırla yanıtlayın. EOF, boş satır, yalnız işaret, uzunluk ihlali, biçim ihlali veya `int` aralığı dışındaki değer için yalnız `Hata: Metin geçerli bir int tamsayı belirtmelidir.` yazılır. Girdinin tamamı doğru olmadan değer yazılmaz.

## Algoritma

1. Satırı okuyup 1..11 uzunluğunu doğrulayın.
2. İlk karakter işaretse negatiflik durumunu belirleyip rakam başlangıcını 1 yapın.
3. İşaretten sonra hiç karakter kalmadıysa hata verin.
4. Sınırı negatif girişte 2147483648, diğer girişte 2147483647; büyüklüğü 0 yapın.
5. Her karakter için 0..9 rakamını doğrulayın ve kod farkından basamağı hesaplayın.
6. Büyüklük `(sinir - basamak) / 10` değerinden büyükse çarpmadan önce hata verin.
7. Aksi halde büyüklüğü 10 ile çarpıp basamağı ekleyin; bütün basamaklar bitince işareti uygulayıp `int` sonucu yazın.

Kullanılan eşitsizlik, negatif olmayan sayılarda `10 * buyukluk + basamak <= sinir` koşuluyla eşdeğerdir. Tamsayı bölümünün aşağı yuvarlaması sınırı doğru daraltır. Her adımda büyüklük kabul edilen sınırda kalır; son işaret uygulaması `long` üzerinde yapıldığı için en küçük `int` güvenle oluşur.

`-2147483648` için son iki basamak:

| Yeni basamak | Önceki büyüklük | İzin verilen en büyük önceki değer | Yeni büyüklük |
| --- | --- | --- | --- |
| 4 | 21474836 | 214748364 | 214748364 |
| 8 | 214748364 | 214748364 | 2147483648 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Tamsayı metni: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length < 1 || metin.Length > 11)
{
    Console.WriteLine("Hata: Metin geçerli bir int tamsayı belirtmelidir.");
    return;
}

int baslangic = 0;
bool negatif = false;
if (metin[0] == '+' || metin[0] == '-')
{
    negatif = metin[0] == '-';
    baslangic = 1;
}
if (baslangic == metin.Length)
{
    Console.WriteLine("Hata: Metin geçerli bir int tamsayı belirtmelidir.");
    return;
}

long sinir = negatif ? 2147483648L : 2147483647L;
long buyukluk = 0;
for (int i = baslangic; i < metin.Length; i++)
{
    char c = metin[i];
    if (c < '0' || c > '9')
    {
        Console.WriteLine("Hata: Metin geçerli bir int tamsayı belirtmelidir.");
        return;
    }
    int basamak = c - '0';
    if (buyukluk > (sinir - basamak) / 10)
    {
        Console.WriteLine("Hata: Metin geçerli bir int tamsayı belirtmelidir.");
        return;
    }
    buyukluk = buyukluk * 10 + basamak;
}

long isaretli = negatif ? -buyukluk : buyukluk;
int sonuc = (int)isaretli;
Console.WriteLine("Değer: " + sonuc.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Son `int` dönüşümü ancak bütün rakamlar ve aralık doğrulandıktan sonra yapılır. `ToString` yalnız sonucu yazdırır, giriş ayrıştırmaz. Önceki derslerde kullanılan `TryParse` içindeki temel basamak ve aralık mantığı burada açık döngüyle görünür hale gelir; bu örnek o aracın bütün kabul kurallarını taklit etmez.

## Örnek çalıştırmalar

Bloklar istemden sonraki tek ve tam sonuç satırıdır.

`+0042`:

```text
Değer: 42
```

`-2147483648`:

```text
Değer: -2147483648
```

`2147483647`:

```text
Değer: 2147483647
```

`-0`:

```text
Değer: 0
```

`2147483648`, `-2147483649`, `12a`, `1.5`, `+`, boş satır veya EOF:

```text
Hata: Metin geçerli bir int tamsayı belirtmelidir.
```

| Girdi | Beklenen özellik |
| --- | --- |
| +2147483647 | İşaret dahil 11 karakter; pozitif üst sınır geçerli. |
| 00000000000 | 11 karakter, değeri 0; geçerli. |
| 000000000000 | Değeri küçük olsa da 12 karakter uzunluk kuralını bozar. |
| Başta/sonda boşluk, çift işaret, Arapça rakam veya sekme | Biçim hatası; yalnız hata satırı. |

## Sınır durumları

- İşaret sadece ilk konumda ve en fazla bir kez bulunabilir.
- İşaretin ardından en az bir rakam gerekir.
- Negatif sınırın büyüklüğü `int` içinde pozitif olarak temsil edilemez; `long` kullanılır.
- -0, +0 ve başında sıfırlar bulunan geçerli biçimler aynı değeri verir.
- Baştaki sıfırlar uzunluk sınırını kaldırmaz.
- Taşma denetimi çarpma ve toplama işleminden önce yapılır.
- Hatalı son karakter önceki basamakların kısmi değerini çıktı yapmaz.

## Kazanımlar

- Metin biçimini işaret ve rakam bölümlerinden oluşan bir giriş dili olarak tanımlama.
- Karakter kod farkını sayısal basamağa dönüştürme.
- Aritmetik sınır koşulunu işlem öncesine taşıma.
- Pozitif ve negatif tamsayı sınırlarının asimetrisini yönetme.
- Biçim doğrulaması, aralık doğrulaması ve çıktı biçimini ayırma.

## Alıştırmalar

1. Hata nedenini boş giriş, biçim ve aralık olarak ayrı mesajlarla bildirin.
2. İşaret dışındaki en fazla rakam adedini ayrıca sınırlayın; baştaki sıfırların davranışını açıklayın.
3. -9999..9999 aralığını kullanan sürümü yazın; sınır eşitsizliğini yeni aralığa uyarlayın.
