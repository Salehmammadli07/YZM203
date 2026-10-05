---
id: "038"
order: 38
title: "Bir üst sınıra kadar mükemmel sayıları bulma"
level: "İleri"
prerequisites: ["027", "031", "037"]
concepts: ["İç içe döngü", "Bölen çiftleri", "Çift saymayı önleme", "Biriktirici", "Mükemmel sayı"]
---

# Bir üst sınıra kadar mükemmel sayıları bulma

## Problem tanımı

2'den başlayıp kullanıcı tarafından verilen üst sınıra kadar bütün mükemmel sayıları bulun. İki uç da aramaya dahildir. Bir pozitif tamsayı, kendisi dışındaki pozitif bölenlerinin toplamına eşitse mükemmeldir. Örneğin 6'nın kendisi dışındaki bölenleri 1, 2 ve 3'tür; `1 + 2 + 3 = 6` olduğu için 6 mükemmel sayıdır. 8 için toplam `1 + 2 + 4 = 7` olur; 8 mükemmel değildir.

Bölenleri sayının sonuna kadar tek tek aramak yerine bölen çiftlerini kullanın. 28'in 2 böleni, aynı zamanda 14 bölenini de verir. Bir tam karede eşleşen iki bölen aynı olabilir; örneğin 36 için 6 ile 6 bulunur ve yalnızca bir 6 toplama eklenmelidir. Dış döngü sayıları tarar, iç döngü bir sayının bölen çiftlerini bulur. Her yeni sayı için toplam yeniden başlatılır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `ust` | 2 ile 10000 arasında `int`; dahil edilen üst sınır. |
| Çıktı | Mükemmel sayı listesi | `Mükemmel sayılar:` öneki ve birer boşlukla ayrılmış sonuçlar; boşsa `Mükemmel sayılar: Yok`. |
| Çıktı | `adet` | `Adet: N` biçiminde bulunan mükemmel sayı sayısı. |

`Üst sınır (2..10000): ` istemine tek tamsayı girin. Geçersiz girişte `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` yazılır; sonuç listesi ve adet üretilmez. 1 bu aramada aday değildir.

## Algoritma

1. Üst sınırı okuyup 2..10000 aralığını doğrulayın.
2. Adedi 0 yapıp liste önekini satırı bitirmeden yazın.
3. Dış döngüde sayıyı 2'den üst sınır dahil olacak şekilde artırın.
4. Her sayı için bölen toplamını 1 ile başlatın; 1 bütün adayların kendileri dışındaki bir bölenidir.
5. İç döngüde böleni 2'den başlatıp `bolen <= sayi / bolen` koşuluyla artırın.
6. Tam bölünme varsa böleni toplama ekleyin; eşini `sayi / bolen` ile hesaplayın.
7. Eş bölen mevcut bölenle aynı değilse onu da ekleyin.
8. İç döngü bittiğinde toplam sayıya eşitse sayıyı yazıp adedi artırın.
9. Dış döngü bitince hiç sonuç yoksa ` Yok` ekleyin; satırı bitirip adedi yazdırın.

Kareköke kadar bulunan her bölenin diğer eşi karekök sınırında veya üstündedir. Bu sayede uygun bölenlerin tamamı bulunur. Adayın kendisi toplama girmez: 1 başta eklenmiştir; 2 ve daha büyük bölenlerin eşi en fazla sayının yarısıdır. Tam kare sınırında iki eş aynı olduğundan ikinci ekleme atlanır. İki döngünün sayaçları sınırlı üst değerlere kadar arttığı için arama sonlanır.

28 için toplam 1 ile başlar. Bölen 2 bulunduğunda toplam `1 + 2 + 14 = 17`, bölen 4 bulunduğunda `17 + 4 + 7 = 28` olur. Diğer denemeler toplamı değiştirmez; sayı sonuç listesine girer.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Üst sınır (2..10000): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ust) || ust < 2 || ust > 10000)
{
    Console.WriteLine("Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.");
    return;
}
int adet = 0;
Console.Write("Mükemmel sayılar:");
for (int sayi = 2; sayi <= ust; sayi++)
{
    int toplam = 1;
    for (int bolen = 2; bolen <= sayi / bolen; bolen++)
    {
        if (sayi % bolen == 0)
        {
            toplam += bolen;
            int es = sayi / bolen;
            if (es != bolen)
            {
                toplam += es;
            }
        }
    }
    if (toplam == sayi)
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

`toplam` dış döngü gövdesinde başlatılır. Eş bölen denetimi bir tam karede aynı değerin iki kez eklenmesini önler. Asal sayıların kendileri dışındaki tek pozitif böleni 1'dir; toplamları 1 kaldığı için hiçbir asal sayı bu aramada mükemmel sayılmaz.

## Örnek çalıştırmalar

Metin blokları giriş isteminden sonraki bütün çıktı satırlarını gösterir.

Girdi `2` için tam çıktı:

```text
Mükemmel sayılar: Yok
Adet: 0
```

Girdi `6` için tam çıktı:

```text
Mükemmel sayılar: 6
Adet: 1
```

Girdi `28` için tam çıktı:

```text
Mükemmel sayılar: 6 28
Adet: 2
```

Girdi `10000` için tam çıktı:

```text
Mükemmel sayılar: 6 28 496 8128
Adet: 4
```

Aşağıdaki örneklerde tek hata satırı yazılır.

| Girdi | Tam sonuç | Açıklama |
| --- | --- | --- |
| `1` | `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` | Alt sınır dışı. |
| `10001` | `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` | Üst sınır dışı. |
| `mükemmel` | `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` | Metin girişi. |
| `6.5` | `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` | Ondalık sayı. |
| Boş satır | `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` | Değer verilmemiştir. |
| Hiç satır vermeden girişin sonu | `Hata: Üst sınır 2 ile 10000 arasında bir tamsayı olmalıdır.` | Okunacak satır yoktur. |

## Sınır durumları

- Üst sınır 2 ile 5 arasında olduğunda sonuç listesi boştur; bu geçerli bir sonuçtur.
- Üst sınır 6 olduğunda 6 listeye katılır; üst sınırın dahil edilmesi için `<=` kullanılır.
- Asal adaylarda toplam 1 kalır; bölen yokluğu için özel bir hata verilmez.
- Tam karede eş bölen aynıysa yalnızca bir kez eklenir. 36 için kendisi dışındaki bölenlerin toplamı 55'tir; 6'nın ikinci kez eklenmesi bu hesabı bozar.
- Her aday için toplamın yeniden 1 yapılması, önceki sayının bölenlerinin sonraki sayıya taşınmasını önler.
- 10000 sınırında bütün olası pozitif uygun bölenler 1..9999 aralığındadır. Bu aralıktaki bütün tamsayıların toplamı 49995000 olduğundan bölen toplamı `int` içine sığar.
- Hatalı biçim, boş satır, giriş sonu veya aralık dışı değer tek hata satırıyla reddedilir.

## Kazanımlar

- Bir sayının kendisi dışındaki pozitif bölenlerini doğru ayırma.
- Bölen çiftleriyle kareköke kadar aramadan bütün uygun bölenleri toplama.
- Tam karede aynı bölenin iki kez sayılmasını önleme.
- Her aralık adayı için toplamı yeniden başlatma.
- Bulunan sayıların listesiyle adet sayacının aynı sonuç kümesini temsil etmesini sağlama.

## Alıştırmalar

1. 28 ve 36 için bulunan bölen çiftlerini ve her eklemeden sonraki toplamı yazdırın; karedeki eşit çiftin davranışını gösterin.
2. Uygun bölen toplamı sayısından büyük olan sayıları aynı yöntemle listeleyin; 12 ve 18 için beklenen durumu hesaplayın.
3. Önce bölenleri 1'den `sayi - 1` değerine kadar deneyen bir sürüm yazın, sonra iki sürümdeki bölünebilirlik denemelerini sayarak karşılaştırın.
