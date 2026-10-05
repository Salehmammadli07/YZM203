---
id: "037"
order: 37
title: "Bir aralıktaki asal sayıları bulma"
level: "İleri"
prerequisites: ["027", "031", "036"]
concepts: ["İç içe döngü", "Aralık tarama", "Her adayda yeniden başlatma", "Erken çıkış", "Sonuç sayacı"]
---

# Bir aralıktaki asal sayıları bulma

## Problem tanımı

Kullanıcının verdiği iki sınır arasındaki bütün asal sayıları küçükten büyüğe yazdırın ve kaç tane olduklarını bulun. İki uç da aralığa dahildir. Asal sayı, 1'den büyük olup yalnızca 1 ile kendisine bölünen tamsayıdır; 0 ve 1 aralıkta bulunabilir, ancak sonuç listesine girmez.

027 numaralı problemde tek sayıya uygulanan bölen aramasını her aralık adayı için yeniden çalıştırın. Dış döngü hangi sayının incelendiğini, iç döngü o sayının olası bölenlerini belirler. Her yeni sayıda asallık durumu yeniden başlatılmalıdır; önceki sayının sonucu sonraki sayıya taşınmaz. Aralık en fazla 0..10000 olabilir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `alt` | 0 ile 10000 arasında `int`; dahil edilen alt sınır. |
| Girdi | `ust` | `alt` ile 10000 arasında `int`; dahil edilen üst sınır. |
| Çıktı | Asal listesi | `Asal sayılar:` öneki ve birer boşlukla ayrılmış asal sayılar; bulunamazsa `Asal sayılar: Yok`. |
| Çıktı | `adet` | `Adet: N` biçiminde bulunan asal sayısı. |

Önce `Alt sınır (0..10000): `, sonra `Üst sınır (alt..10000): ` istemine tamsayı girin. İkinci istemdeki `alt`, daha önce girilmiş değeri temsil eder. Üst sınır alt sınırdan küçükse hata verilir; sınırlar kendiliğinden yer değiştirmez. İlk alan hatalıysa ikinci alan istenmez.

## Algoritma

1. Alt sınırı okuyup 0..10000 aralığını doğrulayın.
2. Üst sınırı okuyup alt sınırdan küçük olmadığını ve 10000'i aşmadığını doğrulayın.
3. Adet sayacını 0 yapın; liste önekini satırı bitirmeden yazın.
4. Dış döngüde sayıyı alt sınırdan üst sınır dahil olacak şekilde artırın.
5. Her sayı için asallık durumunu `sayi >= 2` ile yeniden başlatın.
6. Asal olabilecek sayı için 2'den başlayıp `bolen <= sayi / bolen` koşuluyla bölen arayın.
7. Tam bölen bulunursa durumu yanlış yapıp yalnızca iç döngüden çıkın.
8. Durum hâlâ doğruysa sayıyı listeye ekleyip adedi bir artırın.
9. Dış döngü bittiğinde adet 0 ise listeye ` Yok` ekleyin; ardından satırı bitirip adedi yazdırın.

İç döngünün her adımında önceki bölen adaylarının sayıyı bölmediği bilinir. Bir tam bölen sonucu kesinleştirdiği için `break` kalan bölen adaylarını atlar; dış döngü sıradaki sayıyla sürer. Dış döngü adımının sonunda liste ve adet, o ana kadar işlenmiş aralık bölümünün asal sayılarını temsil eder. Sayı ve bölen sayaçları sınırlı aralıklarda arttığından bütün işlem sonlanır.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Alt sınır (0..10000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int alt) || alt < 0 || alt > 10000)
{
    Console.WriteLine("Hata: Alt sınır 0 ile 10000 arasında bir tamsayı olmalıdır.");
    return;
}
Console.Write("Üst sınır (alt..10000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ust) || ust < alt || ust > 10000)
{
    Console.WriteLine("Hata: Üst sınır alt sınır ile 10000 arasında " +
        "bir tamsayı olmalıdır.");
    return;
}
int adet = 0;
Console.Write("Asal sayılar:");
for (int sayi = alt; sayi <= ust; sayi++)
{
    bool asal = sayi >= 2;
    if (asal)
    {
        for (int bolen = 2; bolen <= sayi / bolen; bolen++)
        {
            if (sayi % bolen == 0)
            {
                asal = false;
                break;
            }
        }
    }
    if (asal)
    {
        Console.Write(" " + sayi.ToString(CultureInfo.InvariantCulture));
        adet++;
    }
}
if (adet == 0)
{
    Console.Write(" Yok");
}
Console.WriteLine();
Console.WriteLine("Adet: " + adet.ToString(CultureInfo.InvariantCulture));
```

`asal` değişkeni dış döngünün gövdesinde oluşturulur. Örneğin 4 için yanlış olan durum, 5 için yeniden doğru başlayabilir. İçteki `break`, yalnızca en yakın bölen döngüsünü bitirir; aralık taramasını sona erdirmez. Çıktı listesi bir dizide biriktirilmeden doğrudan yazdırılır.

## Örnek çalıştırmalar

Girdiler alt, üst sırasındadır. Metin blokları istemlerden sonraki bütün çıktı satırlarını gösterir.

`0`, `5` için tam çıktı:

```text
Asal sayılar: 2 3 5
Adet: 3
```

`2`, `2` için tam çıktı:

```text
Asal sayılar: 2
Adet: 1
```

`4`, `4` için tam çıktı:

```text
Asal sayılar: Yok
Adet: 0
```

`10`, `20` için tam çıktı:

```text
Asal sayılar: 11 13 17 19
Adet: 4
```

Uzun aralığın aşağıdaki denetimi tam liste değildir; sonucun özetidir.

| Girdi | Beklenen özellik veya denetim |
| --- | --- |
| `0`, `10000` | 1229 asal; ilk sayı 2, son sayı 9973; adet satırı `Adet: 1229`. |

Aşağıdaki örneklerde tek hata satırı yazılır; liste ve adet satırları üretilmez.

| Girdi | Tam sonuç |
| --- | --- |
| İlk girdi `-1` | `Hata: Alt sınır 0 ile 10000 arasında bir tamsayı olmalıdır.` |
| `5`, `4` | `Hata: Üst sınır alt sınır ile 10000 arasında bir tamsayı olmalıdır.` |
| `0`, `10001` | `Hata: Üst sınır alt sınır ile 10000 arasında bir tamsayı olmalıdır.` |
| İlk girdi `alt` | `Hata: Alt sınır 0 ile 10000 arasında bir tamsayı olmalıdır.` |
| `0`, `2.5` | `Hata: Üst sınır alt sınır ile 10000 arasında bir tamsayı olmalıdır.` |
| `0`, sonra boş satır | `Hata: Üst sınır alt sınır ile 10000 arasında bir tamsayı olmalıdır.` |
| `0`, sonra girişin sonu | `Hata: Üst sınır alt sınır ile 10000 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- Eşit sınırlar tek aday oluşturur; o aday asal değilse liste boştur.
- 0 ve 1 için bölen döngüsü çalışmaz; ikisi de listeden dışlanır.
- Bir asalın karesinde eşitlik sınırı denenmelidir; 49 için bölen 7 kontrol edilir.
- Aralıkta asal bulunmaması geçerli bir sonuçtur; hata değildir ve adet 0 yazılır.
- Her adayda asallık yeniden başlatılır; bir adayın yanlış durumu sonraki adayı etkilemez.
- En fazla 10001 sayı incelenir; bölen 2'den başladığı için sıfıra bölme yapılmaz.
- Metin, ondalık sayı, boş satır, giriş sonu ve alan aralığının dışındaki girdiler sonuç başlıkları yazılmadan reddedilir.

## Kazanımlar

- Tek sayı algoritmasını dış döngüyle bir aralıktaki bütün adaylara uygulama.
- İç ve dış döngülerin farklı sayaç görevlerini açıklama.
- Asallık durumunu her aday için yeniden başlatma.
- İç döngüden erken çıkışın dış döngüyü neden durdurmadığını örnekle gösterme.
- Boş sonuç listesini adet 0 ile tutarlı biçimde yazdırma.

## Alıştırmalar

1. Her aday için kaç bölen denendiğini ayrıca sayın; 2, 4, 5 ve 49 için deneme sayısını karşılaştırın.
2. Bulunan asal sayıların toplamını bir `long` biriktiricide hesaplayıp adet satırının ardından yazdırın.
3. Aralığı büyükten küçüğe tarayan bir sürüm yazın; listedeki sayı sırasının değiştiğini, adedin aynı kaldığını doğrulayın.
