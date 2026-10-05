---
id: "025"
order: 25
title: "Bir tamsayının kuvvetini hesaplama"
level: "Orta"
prerequisites: ["009", "011", "021"]
concepts: ["Tekrarlı çarpma", "for döngüsü", "Çarpma biriktiricisi", "Birim eleman", "Taşma sınırı", "İşaret"]
---

# Bir tamsayının kuvvetini hesaplama

## Problem tanımı

-10 ile 10 arasında bir tamsayı tabanın, 0 ile 18 arasında bir tamsayı üs ile belirtilen kuvvetini hesaplayın. Hazır kuvvet fonksiyonu kullanmadan tabanı üs kadar kez çarpın. Örneğin 3 tabanı ve 4 üssü için 3 × 3 × 3 × 3 = 81 bulunur.

Üs 0 olduğunda hiç çarpma yapılmaz ve çarpma biriktiricisinin başlangıç değeri olan 1 elde edilir. Bu algoritmada boş çarpım kuralı kullanılarak 0 üzeri 0 için de 1 çıktısı verilir. Bu seçim programın açık bir kabulüdür; 0 üzeri 0 için bütün matematik bağlamlarında geçerli tek bir tanım olduğu ileri sürülmez. Negatif üs kabul edilmediği için kesirli sonuç hesaplanmaz.

Negatif tabanda her çarpma işareti değiştirir: -2'nin üçüncü kuvveti -8, dördüncü kuvveti 16'dır. En büyük sonuç büyüklüğü 10 üzeri 18, yani 1000000000000000000 olur. Sonuç `long` türünde tutularak bu sınır güvenle karşılanır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `taban` | -10 ile 10 arasında `int`. |
| Girdi | `us` | 0 ile 18 arasında `int`. |
| Çıktı | `sonuc` | `long` türünde tamsayı kuvveti; `Sonuç: N`. |

`Taban (-10..10): ` ve `Üs (0..18): ` istemlerine sırasıyla iki ayrı satırda tamsayı girin. Taban geçersizse üs istenmez ve `Hata: Taban -10 ile 10 arasında bir tamsayı olmalıdır.` yazılır. Üs geçersizse `Hata: Üs 0 ile 18 arasında bir tamsayı olmalıdır.` yazılır. Her iki hata durumunda kuvvet sonucu verilmez.

## Algoritma

1. Tabanı okuyup -10 ile 10 aralığında doğrulayın.
2. Üssü okuyup 0 ile 18 aralığında doğrulayın.
3. `long` türündeki `sonuc` değerini 1 yapın.
4. Sayacı 0'dan başlatıp `i < us` olduğu sürece `sonuc *= taban` işlemini uygulayın.
5. Her çarpmadan sonra sayacı bir artırın.
6. Döngü bittiğinde sonucu yazdırın.

Döngü başında `i`, tamamlanmış çarpma adedidir; `sonuc` tabanın bu kadar kez çarpılmasıyla elde edilen değerdir. Başta sıfır çarpma vardır ve sonuç 1'dir. Gövde bir çarpma yaparak sonraki kuvveti üretir. Sayaç her seferinde arttığı için döngü tam `us` kez çalışır. Üs 0 olduğunda koşul ilk denetimde yanlış olur; başlangıç değeri korunur.

Örneğin taban -2 ve üs 3 için biriktirici 1 → -2 → 4 → -8 yolunu izler. Başlangıç değeri 0 seçilseydi her çarpım 0 kalırdı; bu nedenle çarpma biriktiricisi toplam biriktiricisi gibi başlatılmaz.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Taban (-10..10): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int taban) || taban < -10 || taban > 10)
{
    Console.WriteLine("Hata: Taban -10 ile 10 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Üs (0..18): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int us) || us < 0 || us > 18)
{
    Console.WriteLine("Hata: Üs 0 ile 18 arasında bir tamsayı olmalıdır.");
    return;
}

long sonuc = 1;
for (int i = 0; i < us; i++)
{
    sonuc *= taban;
}

Console.WriteLine("Sonuç: " + sonuc.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `sonuc` değişkeni `long` olduğu için her çarpma geniş tamsayı türünde yapılır. `i < us` yerine `i <= us` yazılsaydı bir fazla çarpma yapılırdı. Tam tamsayı çarpımları kullanıldığı için ondalıklı yuvarlama yapılmaz.

## Örnek çalıştırmalar

Girdiler taban ve üs sırasındadır. Tablo giriş istemlerinden sonraki tam sonuç veya hata satırını gösterir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `3`, `4` | `Sonuç: 81` | Dört çarpma. |
| `5`, `0` | `Sonuç: 1` | Hiç çarpma yapılmaz. |
| `0`, `0` | `Sonuç: 1` | Bu programın boş çarpım kabulü. |
| `0`, `7` | `Sonuç: 0` | İlk çarpmadan itibaren sonuç sıfır. |
| `-2`, `3` | `Sonuç: -8` | Negatif taban, tek üs. |
| `-2`, `4` | `Sonuç: 16` | Negatif taban, çift üs. |
| `1`, `18` | `Sonuç: 1` | Birin tüm kabul edilen kuvvetleri 1'dir. |
| `-10`, `17` | `Sonuç: -100000000000000000` | Büyük negatif sonuç. |
| `10`, `18` | `Sonuç: 1000000000000000000` | En büyük sonuç büyüklüğü. |
| `-10`, `18` | `Sonuç: 1000000000000000000` | Çift üs işareti pozitif yapar. |
| İlk girdi `11` | `Hata: Taban -10 ile 10 arasında bir tamsayı olmalıdır.` | Üs istenmeden durur. |
| `2`, `-1` | `Hata: Üs 0 ile 18 arasında bir tamsayı olmalıdır.` | Negatif üs reddedilir. |
| `2`, `19` | `Hata: Üs 0 ile 18 arasında bir tamsayı olmalıdır.` | Üst üs sınırı aşılır. |
| İlk girdi `iki` | `Hata: Taban -10 ile 10 arasında bir tamsayı olmalıdır.` | Metin reddedilir. |

## Sınır durumları

- Üs 0 ise taban ne olursa olsun bu algoritma 1 üretir; 0 üzeri 0 kuralı da açıkça bu kabulden gelir.
- Taban 0 ve üs pozitifse sonuç 0 olur; bölme yapılmadığı için sıfıra bölme sorunu yoktur.
- Negatif tabanın tek üssü negatif, çift üssü pozitiftir; bu davranış çarpmanın işaret kurallarıyla kendiliğinden oluşur.
- Sonuç büyüklüğü en fazla 1000000000000000000'dır. `long` üst sınırı 9223372036854775807 olduğundan tüm ara çarpımlar da tür aralığında kalır.
- Girdi aralıkları büyütülürse taşma sınırı yeniden incelenmelidir; mevcut `long` seçimi sınırsız kuvvet hesabı sağlamaz.
- Her hatada sonuç yazılmadan program biter. Boş satır veya giriş sonu da ilgili alandaki geçersiz tamsayı olarak ele alınır.

## Kazanımlar

- Kuvveti hazır fonksiyon yerine tekrarlı tamsayı çarpmasıyla hesaplama.
- Çarpma biriktiricisinde başlangıç değeri 1'in nedenini açıklama.
- Sayaç koşulundan gerçek yineleme adedini çıkarma.
- Girdi sınırlarını kullanarak sonuç ve ara işlem taşmasını değerlendirme.
- Sıfır ve negatif tabanların davranışını algoritmanın güncellemeleriyle izleme.

## Alıştırmalar

1. Her yinelemeden sonra çarpma adedini ve ara sonucu yazdırın. -3 tabanı, 4 üssü için işaret değişimini izleyin.
2. Taban 0 ve üs 0 olduğunda sonuç yerine özel bir hata mesajı veren sürüm hazırlayın. Bu kontrolün döngüden önce yapılmasını açıklayın.
3. Aynı tabanın 0'dan girilen üssüne kadar tüm kuvvetlerini tek döngüyle yazdırın. Biriktiriciyi her kuvvet için baştan hesaplamadan güncelleyin.
