---
id: "036"
order: 36
title: "Pascal üçgeni oluşturma"
level: "İleri"
prerequisites: ["025", "031", "033"]
concepts: ["İç içe döngü", "Satır içi yineleme", "Pascal katsayıları", "Tam bölme", "Sabit genişlikli alan"]
---

# Pascal üçgeni oluşturma

## Problem tanımı

İstenen sayıda satır içeren Pascal üçgenini yazdırın. Satırları sıfırdan numaralandırın: 0 numaralı satır yalnızca 1, 1 numaralı satır iki tane 1 içerir. Her satırın ilk ve son değeri 1'dir. İç değerler önceki satırda üstündeki iki değerin toplamına eşittir. Örneğin 4 numaralı satır `1 4 6 4 1` olur.

Önceki satırı bir dizide saklamadan, aynı satırdaki ardışık katsayıları hesaplayın. `r` satırında `k` konumundaki katsayı biliniyorsa sonraki katsayı `katsayi * (r - k) / (k + 1)` ile bulunur. Bu problem iç içe döngülerin yanı sıra, önce çarpıp sonra tam bölmenin önemini gösterir. Çıktıda her katsayı dört karakter genişliğindeki bir alana sağa hizalanır; satırların başlangıcı sola hizalıdır ve bütün üçgen ortalanmaz.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 10 arasında `int`; yazdırılacak satır sayısı. |
| Çıktı | Pascal satırları | `r = 0` ile `r = n - 1` arasında, satır başına `r + 1` katsayı. |
| Biçim | Katsayı alanı | Dört karakter genişliğinde, soldan boşlukla doldurulmuş tamsayı. |

`Satır sayısı (1..10): ` istemine tek tamsayı girin. Sonuçların başında ek bir başlık veya sonuç öneki yazılmaz. İlk satırda 1'den önce üç boşluk bulunur. Geçersiz girişte `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` yazılır.

## Algoritma

1. Satır sayısını okuyup 1 ile 10 arasında bir tamsayı olduğunu doğrulayın.
2. Dış döngüde `r` değerini 0'dan `n - 1` değerine kadar artırın.
3. Her yeni satırda katsayıyı 1 yapın.
4. İç döngüde `k` değerini 0'dan `r` dahil olacak şekilde artırın.
5. Katsayıyı dört karakter genişliğindeki alanda yazdırın.
6. Satırın son konumunda değilseniz sonraki katsayıyı `katsayi * (r - k) / (k + 1)` ile hesaplayın.
7. İç döngü bitince satırı bitirin ve sonraki dış döngü adımına geçin.

İç döngü başında `katsayi`, `r` satırındaki `k` konumunun değeridir. İlk konum için bu değer 1'dir; ardışık katsayı ilişkisi her güncellemede korunur. Bölme çarpımdan sonra yapılır ve sonuç tamdır. Örneğin `4 * 3 / 2 = 6` olur; önce `3 / 2` tamsayı bölmesi yapılırsa 1 elde edilir ve yanlış katsayı bulunur. Her iki sayaç da sabit üst sınıra kadar arttığı için döngüler sonlanır.

4 numaralı satırın katsayıları şöyle hesaplanır. Son konumdan sonra yeni katsayı hesaplanmaz.

| Konum k | Yazılan katsayı | Sonraki katsayının hesabı |
| --- | --- | --- |
| 0 | 1 | `1 * 4 / 1 = 4` |
| 1 | 4 | `4 * 3 / 2 = 6` |
| 2 | 6 | `6 * 2 / 3 = 4` |
| 3 | 4 | `4 * 1 / 4 = 1` |
| 4 | 1 | Son konum; hesaplama yok. |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Satır sayısı (1..10): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 10)
{
    Console.WriteLine("Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.");
    return;
}

for (int r = 0; r < n; r++)
{
    long katsayi = 1;
    for (int k = 0; k <= r; k++)
    {
        Console.Write(katsayi.ToString(CultureInfo.InvariantCulture).PadLeft(4));
        if (k < r)
        {
            katsayi = katsayi * (r - k) / (k + 1);
        }
    }
    Console.WriteLine();
}
```

`PadLeft(4)`, kısa sayının başına boşluk ekler; sayının değerini değiştirmez. `katsayi` her dış döngü adımında yeniden 1 olur. Böylece bir satırın son değeri sonraki satırın iç hesabına taşınmaz. Katsayıların tamamını saklayan bir dizi kullanılmaz.

## Örnek çalıştırmalar

Aşağıdaki metin blokları giriş istemi dışındaki bütün çıktı satırlarını gösterir. Başlangıç boşlukları çıktı biçiminin parçasıdır.

Girdi `1` için tam çıktı:

```text
   1
```

Girdi `2` için tam çıktı:

```text
   1
   1   1
```

Girdi `5` için tam çıktı:

```text
   1
   1   1
   1   2   1
   1   3   3   1
   1   4   6   4   1
```

Üst sınır için aşağıdaki tablo tam çıktıyı göstermez; üretilen çıktıda denetlenecek özellikleri özetler.

| Girdi | Beklenen özellik veya denetim |
| --- | --- |
| `10` | 10 satır; son satırın değerleri `1 9 36 84 126 126 84 36 9 1`; son satır genişliği 40 karakter. |

Aşağıdaki sonuçlar tek hata satırıdır; Pascal satırları üretilmez.

| Girdi | Tam sonuç | Açıklama |
| --- | --- | --- |
| `0` | `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` | Alt sınır dışı. |
| `11` | `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` | Üst sınır dışı. |
| `satır` | `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` | Metin. |
| `2.5` | `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` | Ondalık sayı. |
| Boş satır | `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` | Sayı verilmemiştir. |
| Hiç satır vermeden girişin sonu | `Hata: Satır sayısı 1 ile 10 arasında bir tamsayı olmalıdır.` | `ReadLine()` sonuç vermez. |

## Sınır durumları

- `n = 1` için dış döngü bir kez, iç döngü bir kez çalışır; yalnızca ilk 1 yazılır.
- `r = 0` ve her satırın son konumunda `k < r` yanlış olduğu için sonraki katsayı hesabı yapılmaz.
- En büyük satır numarası 9'dur; en büyük katsayı 126 ve katsayı hesabındaki en büyük ara çarpım 630'dur. Bunlar `long` aralığındadır.
- En uzun satır 10 alandan oluşur: `10 * 4 = 40` karakter. Dört karakterlik alan katsayıları ayırmaya yeterlidir.
- Önce bölme yapmak tamsayı kırpması nedeniyle yanlış sonuç verebilir; çarpma-bölme sırası korunmalıdır.
- Metin, ondalık sayı, boş satır, giriş sonu ve aralık dışı değerler hesaplamadan önce reddedilir.

## Kazanımlar

- Dış döngüyü satır, iç döngüyü satırdaki konum olarak yorumlama.
- Her satır başında yeniden başlatılması gereken değişkeni belirleme.
- 4 numaralı satırın katsayılarını yalnızca önceki katsayıdan hesaplama.
- Tam bölme için işlem sırasını koruyup yanlış sıralamanın sonucunu gösterme.
- Dört karakterlik alanların toplam çıktı genişliğini hesaplama.

## Alıştırmalar

1. Her satırın katsayılarını toplamak için bir biriktirici ekleyin; ilk beş satırın toplamlarının 1, 2, 4, 8 ve 16 olduğunu doğrulayın.
2. Dört karakterlik alanı altı karaktere çıkarın; 10 satırlık çıktının son satır genişliğini hesaplayın.
3. Satır başına uygun sayıda boşluk ekleyerek üçgeni ortalayın; katsayı hesabını koruyup yalnızca çıktı yerleşimini değiştirin.
