---
id: "066"
order: 66
title: "Örtüşen alt metin eşleşmelerini bulma"
level: "İleri"
prerequisites: ["042", "064", "065"]
concepts: ["Aday başlangıç", "İç karşılaştırma döngüsü", "Örtüşen eşleşme", "Başarısız aday", "Karşılaştırma sayımı"]
---

# Örtüşen alt metin eşleşmelerini bulma

## Problem tanımı

Bir metin içinde verilen desenin bütün başlangıç dizinlerini bulun. Karakterleri birebir karşılaştırın; büyük/küçük harf ve boşluk anlamlıdır. Örtüşen eşleşmeleri de sayın. Örneğin `aaaa` içinde `aa` deseni 0, 1 ve 2 dizinlerinde bulunur. Bir eşleşmeden sonra desen uzunluğu kadar atlamak bu sonucu eksik bırakır.

Metin 0..200, desen 1..50 karakter uzunluğundadır; iki girdi de yalnız yazdırılabilir ASCII 32..126 karakterlerini içerir. Boş metin geçerlidir, boş desen geçersizdir. Her uygun başlangıçta deseni soldan sağa karşılaştırın ve ilk farkta o adayı bırakın. Gerçek karakter eşitliği değerlendirmelerini sayın; ilk farklı karakter de sayılır. `IndexOf`, `Contains`, `Substring`, düzenli ifade veya hazır arama aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 yazdırılabilir ASCII karakteri. |
| Girdi | `desen` | 1..50 yazdırılabilir ASCII karakteri; ayrı satır. |
| Çıktı | Dizinler | `Dizinler:` ardından artan dizinler; bulunamazsa `Dizinler: Yok`. |
| Çıktı | Sayaçlar | `Eşleşme: N`, ardından `Karşılaştırma: C`. |

İstemler `Metin: ` ve `Desen: ` olur. İlk giriş hatalıysa desen istenmez. Metin hatasında `Hata: Metin 0..200 yazdırılabilir ASCII karakteri içermelidir.`, desen hatasında `Hata: Desen 1..50 yazdırılabilir ASCII karakteri içermelidir.` yazılır. EOF ilgili alanın hatasıdır. İki giriş doğru olmadan arama sonucu yazılmaz.

## Algoritma

1. Metni ve deseni kendi uzunluk/karakter kurallarıyla doğrulayın.
2. Eşleşme ve karşılaştırma sayaçlarını 0 yapın; dizin sonuçları için tampon ayırın.
3. Aday başlangıcını 0'dan `metin.Length - desen.Length` dahil son başlangıca ilerletin.
4. Desen dizinini 0 yapın; her karakter eşitliği değerlendirmesinden önce sayacı artırın.
5. İlk farklı karakterde iç aramayı bitirin; eşitlikte desen dizinini artırın.
6. Desenin tamamı eşitse aday başlangıcını sonuç tamponuna ekleyin.
7. Adayı her durumda yalnız 1 artırarak sürdürün; sonunda bütün dizinleri ve sayaçları yazdırın.

Her adayın yalnız karşılaştırılmış desen başlangıcı eşittir. İlk fark o adayı eler; tamamının eşitliği bir eşleşmedir. Son uygun başlangıç sınırı `i + j` erişiminin metin içinde kalmasını sağlar. Her başlangıç bir kez denenir; bu yüzden örtüşmeler korunur ve dizinler artan sıradadır. En fazla 200 eşleşme vardır; gevşek karşılaştırma üst sınırı 200 × 50 = 10000'dir.

`ababa` ve `aba` için:

| Başlangıç | Bu adayda karşılaştırma | Eşleşme |
| --- | --- | --- |
| 0 | 3 | Evet |
| 1 | 1; b ile a farklı | Hayır |
| 2 | 3 | Evet |

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

Console.Write("Desen: ");
string? desen = Console.ReadLine();
if (desen is null || desen.Length < 1 || desen.Length > 50)
{
    Console.WriteLine("Hata: Desen 1..50 yazdırılabilir ASCII " +
        "karakteri içermelidir.");
    return;
}
for (int i = 0; i < desen.Length; i++)
{
    if (desen[i] < ' ' || desen[i] > '~')
    {
        Console.WriteLine("Hata: Desen 1..50 yazdırılabilir ASCII " +
            "karakteri içermelidir.");
        return;
    }
}

int[] dizinler = new int[metin.Length];
int eslesme = 0, karsilastirma = 0;
for (int i = 0; i <= metin.Length - desen.Length; i++)
{
    int j = 0;
    while (j < desen.Length)
    {
        karsilastirma++;
        if (metin[i + j] != desen[j])
        {
            break;
        }
        j++;
    }
    if (j == desen.Length)
    {
        dizinler[eslesme++] = i;
    }
}

Console.Write("Dizinler:");
if (eslesme == 0)
{
    Console.Write(" Yok");
}
else
{
    for (int i = 0; i < eslesme; i++)
    {
        Console.Write(" " + dizinler[i].ToString(CultureInfo.InvariantCulture));
    }
}
Console.WriteLine();
Console.WriteLine("Eşleşme: " + eslesme.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Karşılaştırma: " +
    karsilastirma.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Desen metinden uzunsa son başlangıç negatiftir ve aday döngüsü çalışmaz. Sonuç tamponunun kapasitesi metin uzunluğudur, ancak yalnız `eslesme` kadar hücre yazdırılır. Sayaç desen/metin uzunluğu karşılaştırmalarını içermez.

## Örnek çalıştırmalar

Metin ve desen ayrı satırlarda girilir; bloklar istemler dışındaki tam çıktıdır.

`aaaa` ve `aa`:

```text
Dizinler: 0 1 2
Eşleşme: 3
Karşılaştırma: 6
```

`ababa` ve `aba`:

```text
Dizinler: 0 2
Eşleşme: 2
Karşılaştırma: 7
```

`abc` ve `d`:

```text
Dizinler: Yok
Eşleşme: 0
Karşılaştırma: 3
```

`abc` ve `abcd`; boş metin ve `a` için de aynı tam çıktı:

```text
Dizinler: Yok
Eşleşme: 0
Karşılaştırma: 0
```

| Girdi | Beklenen özellik |
| --- | --- |
| 200 adet a ve 50 adet a | Dizinler 0..150; eşleşme 151, karşılaştırma 7550. |
| 200 adet a ve a | 200 eşleşme ve 200 karşılaştırma. |
| Tek boşluk metni ve tek boşluk deseni | Dizin 0; eşleşme 1 ve karşılaştırma 1. |
| Hatalı metin veya desen; desen için boş satır | Yalnız ilgili hata satırı. |

## Sınır durumları

- Boş metin geçerli arama alanıdır; hiç aday yoktur.
- Boş desenin eşleşme tanımı bu problemin dışında bırakılmıştır.
- Büyük/küçük harf ayrımı vardır; A ile a eşit değildir.
- Son uygun başlangıç döngüye dahildir.
- Eşleşmeden sonra 1 ilerlemek örtüşen sonuçları korur.
- İlk farkta durmak yalnız mevcut adayı eler; dış arama sürer.
- Hatalı ikinci girişten önce hiçbir eşleşme veya sayaç yazılmaz.

## Kazanımlar

- Başlangıç konumu ve desen içi konumu birlikte yönetme.
- Pencere sınırından güvenli karakter erişimi türetme.
- Örtüşen eşleşme ile ayrık eşleşmenin farkını örnekleme.
- İç döngüde erken çıkışın dış döngüyü sonlandırmadığını gösterme.
- Aynı karar için yapılan gerçek karşılaştırmaları sayma.

## Alıştırmalar

1. Örtüşmeyen eşleşmeleri bulan sürümü yazın; `aaaa` ve `aa` için 0, 2 sonucunu doğrulayın.
2. Yalnız ilk eşleşmeyi bulan sürümde erken çıkışı iki döngü arasında yönetin.
3. ASCII büyük/küçük harf farkını yok sayın; noktalama ve boşluk eşitliğini koruyun.
