---
id: "032"
order: 32
title: "Yıldızlarla dik üçgen oluşturma"
level: "İleri"
prerequisites: ["031"]
concepts: ["İç içe döngü", "Değişen iç sınır", "Satır numarası", "Karakter çıktısı", "Üçgensel toplam"]
---

# Yıldızlarla dik üçgen oluşturma

## Problem tanımı

1 ile 20 arasında verilen N değeri için yıldız karakterlerinden N satırlı bir dik üçgen oluşturun. İlk satırda bir, ikinci satırda iki yıldız bulunur; her sonraki satır bir yıldız daha içerir. Son satırda N yıldız vardır. Bütün satırlar aynı sol kenardan başlar.

Bir satırın uzunluğu, satır numarasına eşittir. Önceki çarpım tablosunda iç döngünün sınırı her satırda sabitti. Bu problemde iç sınır dış döngünün `satir` sayacına bağlıdır. Yıldızları bir dizide veya uzun bir metinde biriktirmeden, iç döngüde birer birer yazdırın. Yıldızların arasına veya satır sonuna boşluk koymayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 20 arasında `int`; üçgenin satır sayısı. |
| Ara değer | `satir` | 1'den N'ye kadar satır numarası. |
| Çıktı | Dik üçgen | Satır R'de R adet `*`; toplam N satır. |

`N (1..20): ` istemine tamsayı girin. Geçersiz girişte `Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.` yazılır. Geçerli çıktıda başlık, sayı, ayraç veya `Sonuç:` öneki bulunmaz; yalnızca yıldız satırları yazılır.

## Algoritma

1. N değerini okuyup 1 ile 20 aralığını doğrulayın.
2. Dış döngüde `satir` değerini 1'den N'ye kadar ilerletin.
3. Her satırın başında iç döngünün `sutun` sayacını 1 yapın.
4. `sutun <= satir` olduğu sürece bir yıldız yazdırıp sütunu artırın.
5. İç döngü tamamlandığında satırı bitirin.
6. Son satır tamamlanınca programı bitirin.

Dış döngü başında önceki satırlar tam olarak yazılmıştır. İç döngü başında mevcut satırda `sutun - 1` yıldız vardır. Bir yıldız yazıp sütunu artırınca bu ilişki korunur. Koşul yanlış olduğunda sütun `satir + 1` değerindedir; tam `satir` yıldız yazılmıştır. N = 4 için iç gövde sırasıyla 1, 2, 3 ve 4 kez çalışır; toplam 10 yıldız üretir. Genel toplam N × (N + 1) / 2'dir.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("N (1..20): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 20)
{
    Console.WriteLine("Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.");
    return;
}

for (int satir = 1; satir <= n; satir++)
{
    for (int sutun = 1; sutun <= satir; sutun++)
    {
        Console.Write("*");
    }
    Console.WriteLine();
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. İç döngünün üst sınırı `n` olsaydı her satır N yıldız içeren bir kare oluşurdu. Üçgen için bu sınırın satır numarası olması gerekir. Her satırın başında `sutun = 1` yeniden çalışır; satır içindeki tekrarlar önceki satırdan devam etmez.

## Örnek çalıştırmalar

Metin blokları, istem ve kullanıcının yanıtı dışındaki tam desen çıktılarıdır. Satırların başında veya sonunda boşluk bulunmaz.

N = 1 için:

```text
*
```

N = 4 için:

```text
*
**
***
****
```

N = 5 için:

```text
*
**
***
****
*****
```

Aşağıdaki üst sınır satırı tam çıktı yerine denetlenecek özellikleri belirtir.

| Girdi | Beklenen özellik |
| --- | --- |
| `20` | 20 satır; ilk satır 1, son satır 20 yıldız; toplam 210 yıldız; en geniş satır 20 karakter. |

Geçersiz girişlerde yalnızca ilgili hata satırı yazılır.

| Girdi | Hata çıktısı |
| --- | --- |
| `0` | `Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.` |
| `21` | `Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.` |
| `-4` | `Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.` |
| `4.5` | `Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.` |
| `dört`, boş satır veya giriş sonu | `Hata: N 1 ile 20 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- N = 1 için tek yıldız ve bir satır sonu yazılır; özel bir dal gerekmez.
- İç koşuldaki eşitlik korunmalıdır. `sutun < satir` kullanılması her satırdan bir yıldız eksiltir ve ilk satırı boş bırakır.
- İç sayaç her satır için 1'den başlar; önceki satırın son sütun değeri yeni satırda kullanılmaz.
- En büyük çıktıda yalnızca 210 yıldız yazılır. Üst sınır hem sayacı hem ekran çıktısının uzunluğunu küçük tutar.
- Yıldızların arasına boşluk eklemek geometrik genişliği değiştirir; bu sürümde her yıldız tam bir sütun kaplar.
- Boş, metin, ondalıklı, aralık dışı giriş ve giriş sonu reddedilir; kısmi desen oluşmaz.

## Kazanımlar

- İç döngü sınırını dış sayaca bağlayarak satır uzunluğunu değiştirme.
- Bir satırın yıldız adedini döngü koşulundan çıkarma.
- Üçgen çıktıda toplam işlem adedini üçgensel toplamla hesaplama.
- Sabit ve değişken iç döngü sınırlarının ürettiği şekilleri karşılaştırma.
- Satır sonu işlemini doğru döngü düzeyinde tutma.

## Alıştırmalar

1. İlk satır N, son satır 1 yıldız içeren ters dik üçgeni oluşturun. İç sınırı dış sayaca bağlı bir ifadeyle kurun.
2. Yıldız yerine satır numarasını yazdırın; satırdaki değerleri birer boşlukla ayırın. N = 4 için beklenen dört satırı önce elle yazın.
3. Satır R'de R yerine 2 × R yıldız yazdırın. N = 5 için toplam karakter adedini hesaplayıp çıktıyla doğrulayın.
