---
id: "034"
order: 34
title: "Ortalanmış yıldız piramidi oluşturma"
level: "İleri"
prerequisites: ["032", "033"]
concepts: ["İç içe döngü", "Ardışık iç döngüler", "Başlangıç boşlukları", "Tek sayılı genişlik", "Merkez hizalama"]
---

# Ortalanmış yıldız piramidi oluşturma

## Problem tanımı

1 ile 15 arasında verilen N değeri için N satırlı ortalanmış bir yıldız piramidi oluşturun. İlk satır bir, ikinci satır üç, üçüncü satır beş yıldız içerir. Her sonraki satır iki yıldız genişler. Satır R için yıldız adedi 2 × R - 1'dir; son satır 2 × N - 1 yıldız içerir.

Üst satırları aynı merkez sütuna hizalamak için satır R'nin başına N - R boşluk yazın. Bu boşluklardan sonra yıldızlar kesintisiz gelir. Satırın yıldızlarından sonra boşluk eklemeyin. Piramit için sol girinti yeterlidir; satır sonuna sağ girintiyi doldurmak gerekmez. Aynı dış yineleme içinde önce boşluklar, sonra yıldızlar için iki ayrı iç döngü çalıştırın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 15 arasında `int`; piramidin yüksekliği. |
| Ara değer | `satir` | 1 ile N arasında satır numarası. |
| Çıktı | Piramit | Satır R'de N - R başlangıç boşluğu ve 2 × R - 1 yıldız. |

`N (1..15): ` istemine tamsayı girin. Geçersiz girişte `Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.` yazılır. Başlık veya `Sonuç:` öneki bulunmaz. Baştaki boşluklar şeklin hizasını belirler; satır sonunda fazladan boşluk yoktur.

## Algoritma

1. N değerini okuyup 1 ile 15 aralığında doğrulayın.
2. Dış döngüde satırı 1'den N'ye kadar ilerletin.
3. İlk iç döngüyle N - satır kadar boşluk yazdırın.
4. İlk iç döngü bittikten sonra ikinci iç döngüyle 2 × satır - 1 yıldız yazdırın.
5. İki iç döngü de tamamlandığında satırı bitirin.
6. Son satır tamamlandığında programı bitirin.

Satır başında önceki satırlar tamamlanmıştır. Boşluk döngüsü bittiğinde yıldızların başlayacağı sütun hazırdır. Yıldız döngüsü başladığında boşluk üretimi bitmiş olur; bu iki döngü birbirinin içinde değil, dış döngünün içinde art arda yer alır. Yıldız sayısı tek olduğundan bir orta yıldız vardır. Bu yıldızdan önce N - R boşluk ve R - 1 yıldız bulunduğu için orta yıldız her satırda N. sütundadır.

N = 4 için boşluk adetleri 3, 2, 1, 0; yıldız adetleri 1, 3, 5, 7 olur. Son satırda boşluk döngüsü hiç çalışmaz. Toplam yıldız sayısı ilk N tek sayının toplamı olan N²'dir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("N (1..15): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 15)
{
    Console.WriteLine("Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.");
    return;
}

for (int satir = 1; satir <= n; satir++)
{
    for (int bosluk = 0; bosluk < n - satir; bosluk++)
    {
        Console.Write(" ");
    }
    for (int yildiz = 0; yildiz < 2 * satir - 1; yildiz++)
    {
        Console.Write("*");
    }
    Console.WriteLine();
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. İki iç sayaç 0'dan başlayıp istenen adet kadar ilerler; `bosluk < adet` ve `yildiz < adet` koşulları tam bu kadar karakter üretir. Yıldız döngüsü boşluk döngüsünün gövdesine konulursa her boşluk için yıldız grubu tekrar edilir ve piramit bozulur. Satır sonu yalnızca iki iç döngü tamamlandıktan sonra yazılır.

## Örnek çalıştırmalar

Metin blokları istem ve kullanıcının yanıtı dışındaki tam desen çıktılarıdır. Baştaki boşluklar şeklin parçasıdır; satır sonunda boşluk bulunmaz.

N = 1 için boşluk döngüsü çalışmaz:

```text
*
```

N = 3 için merkez sütun 3'tür:

```text
  *
 ***
*****
```

N = 4 için:

```text
   *
  ***
 *****
*******
```

Üst sınırı tam desen yerine aşağıdaki çıktı özellikleriyle denetleyin.

| Girdi | Beklenen özellik |
| --- | --- |
| `15` | 15 satır; ilk satırda 14 boşluk ve 1 yıldız; son satırda 29 yıldız ve hiç boşluk yok; toplam 225 yıldız. |

Geçersiz girişlerde desen yerine aşağıdaki hata satırı yazılır.

| Girdi | Hata çıktısı |
| --- | --- |
| `0` | `Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.` |
| `16` | `Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.` |
| `-3` | `Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.` |
| `3.5` | `Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.` |
| `üç`, boş satır veya giriş sonu | `Hata: N 1 ile 15 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- N = 1 için boşluk sayısı 0, yıldız sayısı 1'dir; tek yıldız özel dal olmadan oluşur.
- Son satırda N - satır = 0 olur. Başlangıç koşulu yanlış olduğu için boşluk döngüsü gövdesi hiç çalışmaz.
- Yıldız sayısı her satırda en az 1 ve en fazla 29'dur; sayaç ifadeleri `int` aralığında kalır.
- Yıldızların bittiği sütun N + R - 1'dir. Satırlar sağ tarafta boşlukla doldurulmadığından satır uzunlukları eşit değildir; merkezleri yine aynıdır.
- Üst sınırdaki toplam başlangıç boşluğu 105, yıldız adedi 225'tir. Satır sonları bunların dışındadır.
- Aralık dışı, metin, ondalıklı veya boş giriş ve giriş sonu doğrulamada reddedilir; desen yazılmaya başlanmaz.

## Kazanımlar

- Aynı dış döngü içinde ardışık iki iç döngüyle farklı karakter grupları üretme.
- Satır numarasından başlangıç boşluğu ve yıldız sayısı formüllerini türetme.
- Tek sayıda yıldızın merkezini ortak sütuna hizalama.
- Bir iç döngünün sıfır kez çalışmasının geçerli olabileceğini gösterme.
- Başlangıç boşluğu ile gereksiz satır sonu boşluğunu ayırt etme.

## Alıştırmalar

1. Satırları N'den 1'e doğru dolaşarak ters piramit üretin. N = 4 için boşluk ve yıldız sayılarını tabloya yazın.
2. Piramidin son satırından sonra iki yıldız genişliğinde bir gövde ekleyin. Gövdenin hangi sütundan başlayacağına açık bir hizalama kuralı belirleyin.
3. Her satırın orta yıldızını `|` karakteriyle değiştirin. Ortayı belirlemek için yıldız sayacını kullanın ve N = 1 durumunu ayrıca sınayın.
