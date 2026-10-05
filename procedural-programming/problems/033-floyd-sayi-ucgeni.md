---
id: "033"
order: 33
title: "Floyd sayı üçgeni oluşturma"
level: "İleri"
prerequisites: ["031", "032"]
concepts: ["İç içe döngü", "Satırlar arası durum", "Artan sayı", "Değişen iç sınır", "Sabit genişlik"]
---

# Floyd sayı üçgeni oluşturma

## Problem tanımı

1 ile 10 arasında verilen N değeri için Floyd sayı üçgeni oluşturun. İlk satırda 1, ikinci satırda 2 ve 3, üçüncü satırda 4, 5 ve 6 yer alır. Her satır, satır numarası kadar sayı içerir. Sayılar 1'den başlayıp her hücrede bir artar; yeni satıra geçerken numaralandırma yeniden başlamaz.

Sayıları sağa yaslanan üç karakterlik alanlarda yazdırın. Çıktı N satırdan oluşur; başlık veya `Sonuç:` öneki içermez. Hücre değerini satır ve sütun çarpımından hesaplamak yerine, satırlar arasında korunan ayrı bir `numara` değişkeni kullanın. Böylece döngü sayaçları yerleşimi, bu değişken ise sıradaki yazılacak değeri yönetir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 10 arasında `int`; satır sayısı. |
| Ara değer | `numara` | 1'den başlayan, her yazdırmadan sonra artan `int`. |
| Çıktı | Sayı üçgeni | Satır R'de R sayı; her sayı üç karakterlik alanda. |

`N (1..10): ` istemine tamsayı girin. Geçersiz girişte `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` yazılır. Alanların solundaki boşluklar korunur; son sayıdan sonra ek bir boşluk yazılmaz. N = 10 için son yazılan değer 55'tir.

## Algoritma

1. N değerini okuyup 1 ile 10 aralığında doğrulayın.
2. Döngülerin dışında `numara` değerini 1 yapın.
3. Dış döngüde `satir` sayacını 1'den N'ye ilerletin.
4. Her satır için sütunu 1'den o satırın numarasına kadar ilerletin.
5. Her iç yinelemede `numara` değerini `PadLeft(3)` ile yazdırın; sonra numarayı bir artırın.
6. İç döngü tamamlandığında satırı bitirin.
7. N. satır tamamlanınca programı bitirin.

Her hücreden önce `numara`, şimdiye kadar yazılan hücrelerin toplam adedinden bir fazladır. Değer yazılıp artırılınca bir sonraki hücre için hazır olur. Sütun sayacı her satırda 1'e döner; numara ise dış döngünün dışında başlatıldığı için satırlar arasında korunur. N = 4 için satırlardaki son değerler 1, 3, 6 ve 10'dur. Genel olarak R. satırın son değeri R × (R + 1) / 2 olur.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("N (1..10): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 10)
{
    Console.WriteLine("Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.");
    return;
}

int numara = 1;
for (int satir = 1; satir <= n; satir++)
{
    for (int sutun = 1; sutun <= satir; sutun++)
    {
        Console.Write(numara.ToString(CultureInfo.InvariantCulture).PadLeft(3));
        numara++;
    }
    Console.WriteLine();
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. Yazdırma artırmadan önce yapılır; aksi sırada ilk değer 2 olurdu. `numara = 1` ifadesini dış döngünün gövdesine taşımak her satırda numaralandırmayı yeniden başlatır ve Floyd üçgeni kuralını bozar. Sayılar bir dizide saklanmaz; çıktı ilerledikçe sıradaki sayı üretilir.

## Örnek çalıştırmalar

Metin blokları istem ve kullanıcının yanıtı dışında tam çıktıyı gösterir. Sabit genişlikli alanların baştaki boşlukları korunmalıdır.

N = 1 için:

```text
  1
```

N = 3 için:

```text
  1
  2  3
  4  5  6
```

N = 4 için tek basamaklı ve iki basamaklı değerler aynı genişlikli alandadır:

```text
  1
  2  3
  4  5  6
  7  8  9 10
```

Üst sınırın tam çıktısını göstermeden aşağıdaki özellikleri denetleyin.

| Girdi | Beklenen özellik |
| --- | --- |
| `10` | 10 satır; toplam 55 sayı; son satır 46'dan 55'e kadar 10 sayı içerir ve 30 karakterdir. |

Hatalı girişlerde aşağıdaki tek satır yazılır ve üçgen üretilmez.

| Girdi | Hata çıktısı |
| --- | --- |
| `0` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `11` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `-1` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `3.2` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `üç`, boş satır veya giriş sonu | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- N = 1 için 1 yazılır; numara daha sonra 2 olur ancak bu ara değer çıktıya eklenmez.
- N = 10 için toplam hücre sayısı 55'tir. Son yazılan sayı 55, döngü sonundaki numara 56 olur; bunlar farklı değerlerdir.
- En büyük sayı iki basamaklı olduğundan üç karakterlik alan yeterlidir; tüm sayaçlar `int` aralığında kalır.
- Sütunun satırlar arasında yeniden başlaması gerekirken numaranın yeniden başlamaması gerekir. İki değişkenin yaşam alanları farklı görevleri destekler.
- Satır R'nin genişliği R × 3 karakterdir; satır sonu karakterleri bu hesaba katılmaz.
- Aralık dışı veya sayı olmayan giriş, ondalıklı yazım, boş satır ve giriş sonu üçgen başlamadan reddedilir.

## Kazanımlar

- Yerleşim sayaçları ile hücreye yazılan değerin görevlerini ayırma.
- Bir değişkeni döngü dışında başlatarak satırlar arasında durumu koruma.
- Yazdırma ve artırma sırasının ilk ve son değerlere etkisini açıklama.
- Son yazılan sayı ile döngü sonundaki sıradaki sayıyı ayırt etme.
- Hücre adedini üçgensel toplamla doğrulama.

## Alıştırmalar

1. Başlangıç numarasını ayrı girdi olarak alın. 5'ten başlayan üç satırlı çıktının son değerini önceden hesaplayın.
2. Sayıları bir yerine iki artırarak yalnızca pozitif tek sayılardan bir üçgen oluşturun. N = 4 için son değeri bulun.
3. Her satır sonunda o satırda yazılmış sayıların toplamını ayrı bir çıktı alanına ekleyin. Satır toplamını sıfırlayın fakat numarayı sıfırlamayın.
