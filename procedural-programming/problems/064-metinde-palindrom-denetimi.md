---
id: "064"
order: 64
title: "Metinde iki uçtan palindrom denetimi"
level: "İleri"
prerequisites: ["024", "043", "061", "063"]
concepts: ["İki uçtan tarama", "Karakter atlama", "Normalleştirme", "Erken sonlandırma", "Karşılaştırma sayımı"]
---

# Metinde iki uçtan palindrom denetimi

## Problem tanımı

En fazla 200 yazdırılabilir ASCII karakterinden oluşan satırın, yalnız harf ve rakamları dikkate alınca palindrom olup olmadığını bulun. A–Z ile a–z aynı harf kabul edilir. Boşluk ve noktalama karşılaştırmaya katılmaz. Örneğin `A!b!a` anlamlı karakterlerle `aba` olur ve palindromdur. `a1b` palindrom değildir.

Normalleştirilmiş metin veya ters kopya oluşturmadan iki uçtan ilerleyin. Önce her uçtaki harf/rakam olmayan karakterleri atlayın, ardından iki anlamlı karakteri karşılaştırın. Gerçek karakter karşılaştırması adedini sayın; atlama ve dizin sınırı denetimleri bu sayaca dahil değildir. Anlamlı karakter sayısı 0 veya 1 ise palindrom kabul edilir ve karşılaştırma sayısı 0'dır. `Reverse`, `Replace`, hazır harf/rakam denetimi, harf dönüştürme veya palindrom aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 karakter; ASCII kodları 32..126. |
| Ara değer | `sol`, `sag` | Özgün metinde içeri doğru ilerleyen dizinler. |
| Çıktı | Karar | `Palindrom: Evet` veya `Palindrom: Hayır`. |
| Çıktı | Sayaç | `Karşılaştırma: C`; ilk farklı çift de sayılır. |

`Metin: ` istemini tek satırla yanıtlayın. EOF, 201 veya daha fazla karakter, Türkçe harf, emoji veya sekme için yalnız `Hata: Metin 0..200 yazdırılabilir ASCII karakteri içermelidir.` yazılır. Boş satır geçerlidir; bütün veri karar vermeden önce doğrulanır.

## Algoritma

1. Satırı ve bütün karakterlerini doğrulayın.
2. Sol dizini 0, sağ dizini son konum, kararı doğru ve sayacı 0 yapın.
3. Sol uçta harf/rakam olmayanları içeri doğru atlayın; sağ uçta da aynı işlemi yapın.
4. Sol dizin sağa ulaşmış veya geçmişse karşılaştırmadan bitirin.
5. İki uçtaki büyük harfleri yerel küçük harf değerlerine dönüştürün.
6. Sayacı artırıp iki karakteri karşılaştırın; farklıysa kararı yanlış yapıp bitirin.
7. Eşitse iki dizini içeri ilerletin; taramayı sürdürün ve sonucu yazdırın.

İki uç dışında kalmış anlamlı karakter çiftleri eşittir. Anlamsız karakteri atlamak bu özelliği değiştirmez. Yeni eşit çift, doğrulanmış bölümü büyütür; farklı çift normal palindrom sonucunu yanlış yapar. Her adımda sol artar veya sağ azalır; dizinler sonlu aralıkta buluşur.

İki uç içeri ilerler; atlama döngüleri bütün karakter çiftlerini denemez. Her karakter sabit sayıda incelenir, bu yüzden tarama metin uzunluğuyla orantılıdır. Yeni bir anlamlı karakter dizisi veya ters metin için tampon ayrılmaz.

`A!b!a` için:

| Aşama | Sol | Sağ | Sonuç |
| --- | --- | --- | --- |
| İlk anlamlı çift | 0; A | 4; a | Eşit; sayaç 1 |
| Noktalama atlama | 2; b | 2; b | Dizinler buluştu |
| Bitiş | 2 | 2 | Ortadaki karakter karşılaştırılmaz |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Metin: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: Metin 0..200 yazdırılabilir ASCII " +
        "karakteri içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    if (metin[i] < ' ' || metin[i] > '~')
    {
        Console.WriteLine("Hata: Metin 0..200 yazdırılabilir ASCII " +
            "karakteri içermelidir.");
        return;
    }
}

int sol = 0, sag = metin.Length - 1, karsilastirma = 0;
bool palindrom = true;
while (sol < sag)
{
    while (sol < sag)
    {
        char c = metin[sol];
        if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') ||
            (c >= '0' && c <= '9'))
        {
            break;
        }
        sol++;
    }

    while (sol < sag)
    {
        char c = metin[sag];
        if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') ||
            (c >= '0' && c <= '9'))
        {
            break;
        }
        sag--;
    }
    if (sol >= sag)
    {
        break;
    }

    char a = metin[sol], b = metin[sag];
    if (a >= 'A' && a <= 'Z')
    {
        a = (char)(a + ('a' - 'A'));
    }
    if (b >= 'A' && b <= 'Z')
    {
        b = (char)(b + ('a' - 'A'));
    }
    karsilastirma++;
    if (a != b)
    {
        palindrom = false;
        break;
    }
    sol++;
    sag--;
}

Console.WriteLine(palindrom ? "Palindrom: Evet" : "Palindrom: Hayır");
Console.WriteLine("Karşılaştırma: " +
    karsilastirma.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Büyük harf dönüşümü metni değiştirmez. Boş girişte sağ dizin -1 olur, dış döngü çalışmaz ve karakter erişimi yapılmaz. Çıktıdaki koşullu ifade yalnız iki sabit sonuçtan birini seçer.

## Örnek çalıştırmalar

Bloklar istem dışındaki bütün sonuç satırlarını gösterir.

`A!b!a`:

```text
Palindrom: Evet
Karşılaştırma: 1
```

`a1b`:

```text
Palindrom: Hayır
Karşılaştırma: 1
```

`A man, a plan, a canal: Panama!`:

```text
Palindrom: Evet
Karşılaştırma: 10
```

Boş satır, `...` veya `Z`:

```text
Palindrom: Evet
Karşılaştırma: 0
```

| Girdi | Beklenen özellik |
| --- | --- |
| 200 adet A | Palindrom; 100 karşılaştırma. |
| a ardından 198 boşluk ardından b | İlk anlamlı çift farklı; 1 karşılaştırma. |
| Türkçe harf, sekme, 201 karakter veya EOF | Yalnız tanımlanan hata satırı. |

## Sınır durumları

- Anlamlı karakteri olmayan metin bu problem kuralıyla palindromdur.
- Rakamlar atlanmaz; `1a2` palindrom değildir.
- İlk farklı çiftte durulur; kalan çiftlerin sayımı yapılmaz.
- Büyük/küçük harf eşitliği yalnız ASCII harfleri için tanımlanmıştır.
- Atlama sonrasında dizinlerin buluşması yeniden denetlenir.
- Gerçek karakter karşılaştırması en fazla 100'dür.

## Kazanımlar

- Birden fazla iç taramayı ortak iki dizinle yönetme.
- Atlanacak veri ile karşılaştırılacak veriyi ayırma.
- Dönüştürülmüş kopya oluşturmadan normalleştirilmiş karşılaştırma yapma.
- Sonlanma ve sınır güvenliğini dizinlerin hareketiyle açıklama.
- Erken sonlandırmanın işlem sayısına etkisini ölçme.

## Alıştırmalar

1. Karşılaştırılan özgün sol/sağ dizin çiftlerini yazdıran izleme sürümü hazırlayın.
2. Noktalama atlanmasını kaldırıp yalnız boşluk atlayan sürümü yazın; `A!b!a` ve `a!a?` sonuçlarını karşılaştırın.
3. Anlamlı karakter sayısı 0 olan girdiyi ayrı sonuç yapın; tek karakterli metinlerin davranışını koruyun.
